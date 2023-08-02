// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

// Contains a chart control class.
// Depends on jquery, chart-prereq.js

scada.chart.Chart = class Chart {
    // The date format options.
    static _DATE_OPTIONS = { month: "short", day: "2-digit", timeZone: "UTC" };
    // The time format options.
    static _TIME_OPTIONS = { hour: "2-digit", minute: "2-digit", timeZone: "UTC" };
    // The time format options that include seconds.
    static _TIME_OPTIONS_SEC = { hour: "2-digit", minute: "2-digit", second: "2-digit", timeZone: "UTC" };
    // The date and time format options.
    static _DATE_TIME_OPTIONS = $.extend({}, Chart._DATE_OPTIONS, Chart._TIME_OPTIONS);
    // The date and time format options that include seconds.
    static _DATE_TIME_OPTIONS_SEC = $.extend({}, Chart._DATE_OPTIONS, Chart._TIME_OPTIONS_SEC);
    // The threshold that determines whether to show seconds on X-axis. Equals 1 hour.
    static SHOW_SEC_THRESHOLD = scada.chart.ChartConst.MS_PER_HOUR;
    // The default fore color.
    static DEFAULT_FORE_COLOR = "#000000";

    // The chart jQuery element having the div tag.
    _chartJqElem;
    // The title jQuery element.
    _titleJqElem = null;
    // The canvas jQuery element.
    _canvasJqElem = null;
    // The canvas DOM element where the chart is drawn.
    _canvas = null;
    // The drawing context of the canvas.
    _context = null;
    // The time mark jQuery element.
    _timeMarkJqElem = null;
    // The trend hint jQuery element.
    _trendHintJqElem = null;
    // The information related to the X-axis.
    _xAxisTag = new scada.chart.AxisTag();
    // The information related to the Y-axes.
    _yAxisTags = [];
    // Describes the chart layout.
    _chartLayout = new scada.chart.ChartLayout();
    // The zoom mode flag that affects calculation of the vertical range.
    _zoomMode = false;
    // Indicates whether to enable or disable a trend hint.
    _hintEnabled = true;

    // The control options.
    controlOptions = new scada.chart.ControlOptions();
    // The display options.
    displayOptions = new scada.chart.DisplayOptions();
    // The time range.
    timeRange = new scada.chart.TimeRange();
    // The chart data.
    chartData = null;

    constructor(chartElemID) {
        this._chartJqElem = $("#" + chartElemID);
    }

    // Initializes the axis tags.
    _initAxisTags(opt_reinit) {
        if (this._xAxisTag.isNotInitialized() || opt_reinit) {
            // reset zoom
            this._zoomMode = false;

            // define X-axis range
            this._xAxisTag.min = this.timeRange.startTime;
            this._xAxisTag.max = this.timeRange.endTime;
            this._xAxisTag.alignToGrid = false;

            // create Y-axis tags and assign Y-axis for each trend
            this._yAxisTags = [];
            let axisTagMap = new Map();

            for (let yAxis of this.displayOptions.yAxes) {
                let yAxisTag = new scada.chart.AxisTag();
                yAxisTag.axisConfig = yAxis;
                this._yAxisTags.push(yAxisTag);

                for (let id of yAxis.quantityIDs) {
                    axisTagMap.set(id, yAxisTag);
                }
            }

            if (this._yAxisTags.length > 0) {
                let firstTag = this._yAxisTags[0];

                for (let trend of this.chartData.trends) {
                    let tag = axisTagMap.has(trend.quantityID) ? axisTagMap.get(trend.quantityID) : firstTag;
                    trend.color = tag.axisConfig.trendColor;
                    tag.trends.push(trend);
                }
            }

            // remove unused Y-axis tags
            let idx = this._yAxisTags.length;
            while (--idx >= 0) {
                if (this._yAxisTags[idx].trends.length === 0) {
                    this._yAxisTags.splice(idx, 1);
                }
            }

            // define Y-axis ranges
            this._calcAllYRanges();
        }
    }

    // Calculates ranges for all Y-axes.
    _calcAllYRanges() {
        for (let yAxisTag of this._yAxisTags) {
            if (yAxisTag.axisConfig.autoScale) {
                // auto scale
                this._calcYRange(yAxisTag);
            } else if (yAxisTag.axisConfig.min < yAxisTag.axisConfig.max) {
                // fixed scale
                yAxisTag.min = yAxisTag.axisConfig.min;
                yAxisTag.max = yAxisTag.axisConfig.max;
            } else {
                // default scale
                yAxisTag.min = -1;
                yAxisTag.max = 1;
            }

            this._defineAxisTitle(yAxisTag);
        }
    }

    // Calculates top and bottom edges of the displayed range.
    _calcYRange(yAxisTag, opt_startPtInd) {
        // find min and max trend value
        let minY = NaN;
        let maxY = NaN;
        let minX = this._xAxisTag.min - this.controlOptions.gapBetweenPoints;
        let maxX = this._xAxisTag.max + this.controlOptions.gapBetweenPoints;

        let timePoints = this.chartData.timePoints;
        let startPtInd = opt_startPtInd ? opt_startPtInd : 0;
        let ptCnt = timePoints.length;

        for (let trend of yAxisTag.trends) {
            for (let ptInd = startPtInd; ptInd < ptCnt; ptInd++) {
                let x = scada.chart.TimePoint.getUtc(timePoints[ptInd]);

                if (minX <= x && x <= maxX) {
                    let y = scada.chart.TrendPoint.getValue(trend.points[ptInd]);
                    if (isNaN(minY) || minY > y) {
                        minY = y;
                    }
                    if (isNaN(maxY) || maxY < y) {
                        maxY = y;
                    }
                }
            }
        }

        if (isNaN(minY)) {
            minY = -1;
            maxY = 1;
        } else {
            // calculate extra space
            const EXTRA_SPACE_COEF = 0.05;
            let extraSpace = minY === maxY && typeof opt_startPtInd === "undefined" ?
                1 : (maxY - minY) * EXTRA_SPACE_COEF;

            if (yAxisTag.axisConfig.includeZero && !this._zoomMode) {
                // include zero in the Y-range
                let origMinY = minY;
                let origMaxY = maxY;

                if (minY > 0 && maxY > 0) {
                    minY = 0;
                }
                else if (minY < 0 && maxY < 0) {
                    maxY = 0;
                }

                extraSpace = Math.max(extraSpace, (maxY - minY) * EXTRA_SPACE_COEF);

                // add extra space
                if (origMinY - minY < extraSpace) {
                    minY -= extraSpace;
                }
                if (maxY - origMaxY < extraSpace) {
                    maxY += extraSpace;
                }
            } else {
                // add extra space
                minY -= extraSpace;
                maxY += extraSpace;
            }
        }

        yAxisTag.min = minY;
        yAxisTag.max = maxY;
    }

    // Defines the axis title.
    _defineAxisTitle(yAxisTag) {
        // title should be the same for all trends of the axis
        let axisTitle = "";
        let axisTitleSet = false;

        for (let trend of yAxisTag.trends) {
            let curTitle = trend.getAxisTitle();

            if (axisTitleSet) {
                if (axisTitle !== curTitle) {
                    axisTitle = "";
                    break;
                }
            } else {
                axisTitle = curTitle;
                axisTitleSet = true;
            }
        }

        yAxisTag.title = axisTitle;
    }

    // Initializes the trend colors and captions.
    _initTrendFields() {
        let trendInd = 0;
        for (let trend of this.chartData.trends) {
            trend.color = trend.color || this._getColorByTrend(trendInd);
            trend.caption = "[" + trend.cnlNum + "] " + trend.cnlName;
            trendInd++;
        }
    }

    // Checks if top and bottom edges are outdated because of new data.
    _yRangeIsOutdated(startPtInd) {
        for (let yAxisTag of this._yAxisTags) {
            if (yAxisTag.axisConfig.autoScale) {
                let oldMinY = yAxisTag.min;
                let oldMaxY = yAxisTag.max;
                this._calcYRange(yAxisTag, startPtInd);
                let outdated = yAxisTag.min < oldMinY || yAxisTag.max > oldMaxY;

                // restore the range
                yAxisTag.min = oldMinY;
                yAxisTag.max = oldMaxY;

                if (outdated) {
                    return true;
                }
            }
        }

        return false;
    }

    // Converts the trend X-coordinate to the canvas X-coordinate.
    _trendXToCanvasX(x) {
        return Math.round((x - this._xAxisTag.min) * this._xAxisTag.coef +
            this._chartLayout.plotAreaRect.left + this._chartLayout.canvasXOffset);
    }

    // Converts the trend Y-coordinate to the canvas Y-coordinate.
    _trendYToCanvasY(y, yAxisTag) {
        return Math.round((yAxisTag.max - y) * yAxisTag.coef +
            this._chartLayout.plotAreaRect.top + this._chartLayout.canvasYOffset + 1);
    }

    // Converts the trend X-coordinate to the page X-coordinate.
    _trendXToPageX(x) {
        return Math.round((x - this._xAxisTag.min) * this._xAxisTag.coef + this._chartLayout.absPlotAreaRect.left);
    }

    // Converts the page X-coordinate to the trend X-coordinate.
    _pageXToTrendX(pageX) {
        return (pageX - this._chartLayout.absPlotAreaRect.left) / this._xAxisTag.coef + this._xAxisTag.min;
    }

    // Gets the index of the point nearest to the specified page X-coordinate.
    _getPointIndex(pageX) {
        const timePoints = this.chartData.timePoints;
        const ptCnt = timePoints.length;

        if (ptCnt < 1) {
            return -1;
        } else {
            let x = this._pageXToTrendX(pageX);
            let ptInd = 0;
            let xL, xR;

            if (ptCnt === 1) {
                ptInd = 0;
            } else {
                // binary search
                let iL = 0;
                let iR = ptCnt - 1;
                xL = scada.chart.TimePoint.getUtc(timePoints[iL]);
                xR = scada.chart.TimePoint.getUtc(timePoints[iR]);

                if (x < xL || x > xR)
                    return -1;

                while (iR - iL > 1) {
                    let iM = Math.floor((iR + iL) / 2);
                    let xM = scada.chart.TimePoint.getUtc(timePoints[iM]);

                    if (xM === x)
                        return iM;
                    else if (xM < x)
                        iL = iM;
                    else
                        iR = iM;
                }

                xL = scada.chart.TimePoint.getUtc(timePoints[iL]);
                xR = scada.chart.TimePoint.getUtc(timePoints[iR]);
                ptInd = x - xL < xR - x ? iL : iR;
            }

            let xResult = scada.chart.TimePoint.getUtc(timePoints[ptInd]);
            return Math.abs(x - xResult) <= this.controlOptions.gapBetweenPoints ? ptInd : -1;
        }
    };

    // Converts the time point into a date and time string.
    _pointToDateTimeString(timePoint, format) {
        let localTime = scada.chart.TimePoint.getLocal(timePoint);
        let dateTime = new Date(Date.parse(localTime + "Z"));
        return dateTime.toLocaleString(this.controlOptions.locale, format);
    }

    // Converts the trend X-coordinate into a date and time string.
    _trendXToDateTimeString(x, formatOptions) {
        const MS_PER_HOUR = scada.chart.ChartConst.MS_PER_HOUR;
        let hourMs = Math.trunc(x / MS_PER_HOUR) * MS_PER_HOUR;
        let localHour = this.timeRange.hourMap.get(hourMs);

        if (localHour) {
            let dateTime = new Date(Date.parse(localHour + "Z"));
            dateTime.setMilliseconds(x % MS_PER_HOUR);
            return dateTime.toLocaleString(this.controlOptions.locale, formatOptions);
        } else {
            return "";
        }
    }

    // Converts the trend X-coordinate into a date string.
    _trendXToDateString(x) {
        return this._trendXToDateTimeString(x, Chart._DATE_OPTIONS);
    }

    // Converts the trend X-coordinate that means a time into a time string.
    _trendXToTimeString(x, showSeconds) {
        return this._trendXToDateTimeString(x,
            showSeconds ? Chart._TIME_OPTIONS_SEC : Chart._TIME_OPTIONS);
    }

    // Draws the pixel on the chart.
    _drawPixel(x, y, opt_boundRect, opt_size) {
        let size = 1;

        if (opt_size && opt_size > 1) {
            size = opt_size;
            let offset = size / 2;
            x -= offset;
            y -= offset;
        }

        if (opt_boundRect) {
            // check if the given coordinates are located within the drawing area
            if (opt_boundRect.left <= x && x + size <= opt_boundRect.right &&
                opt_boundRect.top <= y && y + size <= opt_boundRect.bottom) {
                this._context.fillRect(x, y, size, size);
            }
        } else {
            // just draw a pixel
            this._context.fillRect(x, y, size, size);
        }
    }

    // Draws the line on the chart.
    _drawLine(x1, y1, x2, y2, opt_boundRect) {
        if (opt_boundRect) {
            let minX = Math.min(x1, x2);
            let maxX = Math.max(x1, x2);
            let minY = Math.min(y1, y2);
            let maxY = Math.max(y1, y2);

            if (opt_boundRect.left <= minX && maxX < opt_boundRect.right &&
                opt_boundRect.top <= minY && maxY < opt_boundRect.bottom) {
                opt_boundRect = null; // the line is fully inside the drawing area
            } else if (opt_boundRect.left > maxX || minX >= opt_boundRect.right ||
                opt_boundRect.top > maxY || minY > opt_boundRect.bottom) {
                return; // the line is outside the drawing area
            }
        }

        let dx = x2 - x1;
        let dy = y2 - y1;

        if (dx !== 0 || dy !== 0) {
            if (Math.abs(dx) > Math.abs(dy)) {
                let a = dy / dx;
                let b = -a * x1 + y1;

                if (dx < 0) {
                    let x0 = x1;
                    x1 = x2;
                    x2 = x0;
                }

                for (let x = x1; x <= x2; x++) {
                    let y = Math.round(a * x + b);
                    this._drawPixel(x, y, opt_boundRect);
                }
            } else {
                let a = dx / dy;
                let b = -a * y1 + x1;

                if (dy < 0) {
                    let y0 = y1;
                    y1 = y2;
                    y2 = y0;
                }

                for (let y = y1; y <= y2; y++) {
                    let x = Math.round(a * y + b);
                    this._drawPixel(x, y, opt_boundRect);
                }
            }
        }
    }

    // Draws the trend line on the chart.
    _drawTrendLine(x1, y1, x2, y2, boundRect, lineWidth) {
        if (lineWidth > 1) {
            // draw the line if at least a part is inside the drawing area
            let minX = Math.min(x1, x2);
            let maxX = Math.max(x1, x2);
            let minY = Math.min(y1, y2);
            let maxY = Math.max(y1, y2);

            if (!(boundRect.left > maxX || minX >= boundRect.right ||
                boundRect.top > maxY && minY >= boundRect.bottom)) {
                // line width already set
                let ctx = this._context;
                ctx.beginPath();
                ctx.moveTo(x1, y1);
                ctx.lineTo(x2, y2);
                ctx.stroke();
            }
        } else {
            this._drawLine(x1, y1, x2, y2, boundRect);
        }
    }

    // Clears the specified rectangle by filling it with the background color.
    _clearRect(x, y, width, height) {
        this._setColor(this.displayOptions.chartArea.backColor);
        this._context.fillRect(x, y, width, height);
    }

    // Sets the current drawing color.
    _setColor(color) {
        this._context.fillStyle = this._context.strokeStyle =
            color ? color : Chart.DEFAULT_FORE_COLOR;
    }

    // Gets the color of the trend with the specified index.
    _getColorByTrend(trendInd) {
        let colors = this.displayOptions.plotArea.trendColors;
        return colors[trendInd % colors.length];
    }

    // Determines whether to set trend color according to point status.
    _getUseStatusColor() {
        return this.displayOptions.plotArea.trendColorAsStatus && this.chartData.trends.length === 1;
    }

    // Draws the chart frame.
    _drawFrame() {
        let rect = this._chartLayout.canvasPlotAreaRect;
        let frameL = rect.left - 1;
        let frameR = rect.right;
        let frameT = rect.top;
        let frameB = rect.bottom - 1;

        this._setColor(this.displayOptions.plotArea.backColor);
        this._context.fillRect(rect.left, rect.top, rect.width, rect.height);

        this._setColor(this.displayOptions.plotArea.frameColor);
        this._drawLine(frameL, frameT, frameL, frameB);
        this._drawLine(frameR, frameT, frameR, frameB);
        this._drawLine(frameL, frameT, frameR, frameT);
        this._drawLine(frameL, frameB, frameR, frameB);
    }

    // Draws chart grid of the X-axis.
    _drawXGrid() {
        const layout = this._chartLayout;
        const xAxisConfig = this.displayOptions.xAxis;

        this._context.textAlign = "center";
        this._context.textBaseline = "top";
        this._context.font = xAxisConfig.fontSize + "px " + this.displayOptions.chartArea.fontName;

        let prevLblX = NaN;
        let prevLblHalfW = NaN;
        let tickT = layout.plotAreaRect.bottom + layout.canvasYOffset;
        let tickB = tickT + xAxisConfig.majorTickSize - 1;
        let minorTickB = tickT + xAxisConfig.minorTickSize - 1;
        let gridT = layout.plotAreaRect.top + layout.canvasYOffset + 1;
        let gridB = gridT + layout.plotAreaRect.height - 3;
        let labelMarginT = scada.chart.DisplayOptions.getMargin(xAxisConfig.labelMargin, 0);
        let labelMarginR = scada.chart.DisplayOptions.getMargin(xAxisConfig.labelMargin, 1);
        let labelMarginB = scada.chart.DisplayOptions.getMargin(xAxisConfig.labelMargin, 2);
        let labelMarginL = scada.chart.DisplayOptions.getMargin(xAxisConfig.labelMargin, 3);
        let lblY = tickB + labelMarginT + 1;
        let lblDateY = lblY + xAxisConfig.fontSize + labelMarginB + labelMarginT;
        let gridStep = layout.xAxisLayout.gridStep;
        let minorTickStep = this.displayOptions.xAxis.showMinorTicks ? layout.xAxisLayout.minorTickStep : 0;
        let showSeconds = this._xAxisTag.max - this._xAxisTag.min <= Chart.SHOW_SEC_THRESHOLD;
        let dayBegTimeText = new Date(0).toLocaleString(this.controlOptions.locale,
            showSeconds ? Chart._TIME_OPTIONS_SEC : Chart._TIME_OPTIONS);

        for (let x = this._xAxisTag.min; x <= this._xAxisTag.max; x += gridStep) {
            let ptX = this._trendXToCanvasX(x);

            // vertical grid line
            if (xAxisConfig.showGridLines) {
                this._setColor(this.displayOptions.plotArea.gridColor);
                this._drawLine(ptX, gridT, ptX, gridB);
            }

            // major tick
            this._setColor(xAxisConfig.lineColor);
            this._drawLine(ptX, tickT, ptX, tickB);

            // minor ticks
            if (minorTickStep > 0) {
                for (let minorTickX = x + minorTickStep,
                    maxMinorTickX = Math.min(x + gridStep, this._xAxisTag.max);
                    minorTickX < maxMinorTickX; minorTickX += minorTickStep) {

                    let minorTickCnvX = this._trendXToCanvasX(minorTickX);
                    this._drawLine(minorTickCnvX, tickT, minorTickCnvX, minorTickB);
                }
            }

            // label
            this._setColor(xAxisConfig.textColor);
            let lblX = ptX;
            let timeText = this._trendXToTimeString(x, showSeconds);
            let lblHalfW = this._context.measureText(timeText).width / 2;

            if (isNaN(prevLblX) || lblX - lblHalfW - labelMarginL > prevLblX + prevLblHalfW + labelMarginR) {
                this._context.fillText(timeText, lblX, lblY);

                if (xAxisConfig.showDates && timeText === dayBegTimeText) {
                    this._context.fillText(this._trendXToDateString(x), lblX, lblDateY);
                }

                prevLblX = lblX;
                prevLblHalfW = lblHalfW;
            }
        }
    }

    // Draws chart grid of the Y-axis.
    _drawYGrid(yAxisTag) {
        let layout = this._chartLayout;
        let yAxisConfig = yAxisTag.axisConfig;
        let yAxisLayout = yAxisTag.axisLayout;
        let yAxisRect = yAxisLayout.areaRect;

        this._context.textBaseline = "middle";
        this._context.font = yAxisConfig.fontSize + "px " + this.displayOptions.chartArea.fontName;

        let labelMarginT = scada.chart.DisplayOptions.getMargin(yAxisConfig.labelMargin, 0);
        let labelMarginR = scada.chart.DisplayOptions.getMargin(yAxisConfig.labelMargin, 1);
        let labelMarginB = scada.chart.DisplayOptions.getMargin(yAxisConfig.labelMargin, 2);
        let labelMarginL = scada.chart.DisplayOptions.getMargin(yAxisConfig.labelMargin, 3);
        let prevLblY = NaN;
        let tickL, tickR, minorTickL, minorTickR, axisX, lblX;

        if (yAxisConfig.position === scada.chart.AreaPosition.LEFT) {
            this._context.textAlign = "right";
            tickR = yAxisRect.right - 1;
            tickL = tickR - yAxisConfig.majorTickSize;
            minorTickR = tickR;
            minorTickL = minorTickR - yAxisConfig.minorTickSize;
            axisX = yAxisRect.right - 1;
            lblX = tickL - labelMarginR;
        } else {
            this._context.textAlign = "left";
            tickL = yAxisRect.left;
            tickR = tickL + yAxisConfig.majorTickSize;
            minorTickL = tickL;
            minorTickR = minorTickL + yAxisConfig.minorTickSize;
            axisX = yAxisRect.left;
            lblX = tickR + labelMarginL;
        }

        let gridL = layout.plotAreaRect.left + layout.canvasXOffset;
        let gridR = gridL + layout.plotAreaRect.width - 1;
        let axisT = yAxisRect.top + layout.canvasYOffset;
        let axisB = axisT + yAxisRect.height - 1;

        for (let y = yAxisLayout.gridStart; y < yAxisTag.max; y += yAxisLayout.gridStep) {
            let ptY = this._trendYToCanvasY(y, yAxisTag);

            // horizontal grid line
            if (yAxisConfig.showGridLines) {
                this._setColor(this.displayOptions.plotArea.gridColor);
                this._drawLine(gridL, ptY, gridR, ptY);
            }

            // major tick and axis line
            this._setColor(yAxisConfig.lineColor);
            this._drawLine(tickL, ptY, tickR, ptY);
            this._drawLine(axisX, axisT, axisX, axisB);

            // minor ticks
            if (yAxisConfig.minorTickCount > 0) {
                for (let minorTickY = y + yAxisLayout.minorTickStep, maxMinorTickY = y + yAxisLayout.gridStep;
                    minorTickY < maxMinorTickY && maxMinorTickY < yAxisTag.max; minorTickY += yAxisLayout.minorTickStep) {

                    let minorTickCnvY = this._trendYToCanvasY(minorTickY, yAxisTag);
                    this._drawLine(minorTickL, minorTickCnvY, minorTickR, minorTickCnvY);
                }
            }

            // label
            this._setColor(yAxisConfig.textColor);
            let lblY = ptY;

            if (isNaN(prevLblY) || prevLblY - lblY > yAxisConfig.fontSize) {
                this._context.fillText(y.toFixed(yAxisLayout.gridDigits), lblX, lblY);
                prevLblY = lblY;
            }
        }

        // axis title
        if (yAxisConfig.showTitle && yAxisTag.title) {
            this._context.textAlign = "center";
            this._context.save();
            this._context.translate(yAxisRect.left + layout.canvasXOffset, yAxisRect.bottom + layout.canvasYOffset);
            this._context.rotate(-Math.PI / 2);
            let titleY;

            if (yAxisConfig.position === scada.chart.AreaPosition.LEFT) {
                this._context.textBaseline = "top";
                titleY = labelMarginT;
            } else {
                this._context.textBaseline = "bottom";
                titleY = yAxisRect.width - labelMarginB;
            }

            this._context.fillText(yAxisTag.title, yAxisRect.height / 2, titleY, yAxisRect.height);
            this._context.restore();
        }
    }

    // Draws the chart legend.
    _drawLegend() {
        if (this.displayOptions.legend.position !== scada.chart.AreaPosition.NONE) {
            const layout = this._chartLayout;
            const legend = this.displayOptions.legend;

            let trendCnt = this.chartData.trends.length;
            let lineCnt = Math.ceil(trendCnt / legend.columnCount);
            let useStatusColor = this._getUseStatusColor();

            let columnMarginTop = scada.chart.DisplayOptions.getMargin(legend.columnMargin, 0);
            let columnMarginRight = scada.chart.DisplayOptions.getMargin(legend.columnMargin, 1);
            let columnMarginLeft = scada.chart.DisplayOptions.getMargin(legend.columnMargin, 3);

            let fullColumnWidth = legend.columnWidth + columnMarginLeft + columnMarginRight;
            let lineXOffset = layout.legendAreaRect.left + layout.canvasXOffset + columnMarginLeft;
            let lineYOffset = layout.legendAreaRect.top + layout.canvasYOffset + columnMarginTop;

            this._context.textAlign = "left";
            this._context.textBaseline = "middle";
            this._context.font = legend.fontSize + "px " + this.displayOptions.chartArea.fontName;

            for (let trendInd = 0; trendInd < trendCnt; trendInd++) {
                let trend = this.chartData.trends[trendInd];
                let columnIndex = Math.trunc(trendInd / lineCnt);
                let lineIndex = trendInd % lineCnt;
                let lineX = fullColumnWidth * columnIndex + lineXOffset;
                let lineY = legend.lineHeight * lineIndex + lineYOffset;
                let iconX = lineX + 0.5;
                let iconY = lineY + (legend.lineHeight - legend.iconHeight) / 2 - 0.5;
                let lblX = lineX + legend.iconWidth + legend.fontSize / 2;
                let lblY = lineY + legend.lineHeight / 2;

                if (!useStatusColor) {
                    this._setColor(trend.color);
                    this._context.fillRect(iconX, iconY, legend.iconWidth, legend.iconHeight);
                    this._setColor(legend.foreColor);
                    this._context.strokeRect(iconX, iconY, legend.iconWidth, legend.iconHeight);
                }

                this._context.fillText(trend.caption, lblX, lblY);
            }
        }
    }

    // Draws the trends assigned to the specified Y-axis.
    _drawTrends(yAxisTag, opt_startPtInd) {
        let useStatusColor = this._getUseStatusColor();

        for (let trend of yAxisTag.trends) {
            this._drawTrend(this.chartData.timePoints, trend, yAxisTag, useStatusColor, opt_startPtInd);
        }
    }

    // Draws the specified trend.
    _drawTrend(timePoints, trend, yAxisTag, useStatusColor, opt_startPtInd) {
        const boundRect = this._chartLayout.canvasPlotAreaRect;
        const lineWidth = this.displayOptions.plotArea.lineWidth;

        // set clipping region and line width
        if (lineWidth > 1) {
            this._context.save();
            this._context.rect(boundRect.left, boundRect.top, boundRect.width, boundRect.height);
            this._context.clip();
            this._context.lineWidth = lineWidth;
        }

        // set color
        this._setColor(useStatusColor ? "" : trend.color);

        // draw lines
        let prevX = NaN;
        let prevPtX = NaN;
        let prevPtY = NaN;
        let startPtInd = opt_startPtInd ? opt_startPtInd : 0;
        let ptCnt = timePoints.length;

        for (let ptInd = startPtInd; ptInd < ptCnt; ptInd++) {
            let trendPoint = trend.points[ptInd];
            let y = scada.chart.TrendPoint.getValue(trendPoint);

            if (!isNaN(y)) {
                let x = scada.chart.TimePoint.getUtc(timePoints[ptInd]);
                let ptX = this._trendXToCanvasX(x);
                let ptY = this._trendYToCanvasY(y, yAxisTag);

                if (isNaN(prevX)) {
                    // do nothing
                }
                else if (x - prevX > this.controlOptions.gapBetweenPoints) {
                    // draw start and end points
                    this._drawPixel(prevPtX, prevPtY, boundRect, lineWidth);

                    if (useStatusColor) {
                        this._setColor(this.chartData.getTrendPointColor(trendPoint));
                    }

                    this._drawPixel(ptX, ptY, boundRect, lineWidth);
                } else if (prevPtX !== ptX || prevPtY !== ptY) {
                    // draw line
                    if (useStatusColor) {
                        this._setColor(this.chartData.getTrendPointColor(trendPoint));
                    }

                    this._drawTrendLine(prevPtX, prevPtY, ptX, ptY, boundRect, lineWidth);
                }

                prevX = x;
                prevPtX = ptX;
                prevPtY = ptY;
            }
        }

        if (!isNaN(prevPtX))
            this._drawPixel(prevPtX, prevPtY, boundRect, lineWidth);

        // restore clipping region and line width
        if (lineWidth > 1) {
            this._context.restore();
        }
    }

    // Creates a time mark jQuery element if it doesn't exist.
    _initTimeMark() {
        if (this._timeMarkJqElem) {
            this._timeMarkJqElem.addClass("hidden");
        } else {
            this._timeMarkJqElem = $("<div class='chart-time-marker hidden'></div>")
                .css("background-color", this.displayOptions.plotArea.markerColor);
            this._chartJqElem.append(this._timeMarkJqElem);
        }
    }

    // Creates a trend hint jQuery element if it doesn't exist.
    _initTrendHint() {
        if (this._trendHintJqElem) {
            this._trendHintJqElem.addClass("hidden");
        } else if (this.chartData.trends.length > 0) {
            this._trendHintJqElem = $("<div class='chart-trend-hint hidden'><div class='time'></div><table></table></div>");
            let table = this._trendHintJqElem.children("table");
            let useStatusColor = this._getUseStatusColor();

            for (let trend of this.chartData.trends) {
                let row = $("<tr><td class='color'><span></span></td><td class='text'></td>" +
                    "<td class='colon'>:</td><td class='val'></td></tr>");

                if (useStatusColor) {
                    row.find("td.color").addClass("hidden");
                } else {
                    row.find("td.color span").css("background-color", trend.color);
                }

                row.children("td.text").text(trend.caption);
                table.append(row);
            }

            this._chartJqElem.append(this._trendHintJqElem);
        } else {
            this._trendHintJqElem = $();
        }
    }

    // Shows a hint with the values nearest to the pointer.
    _showHint(pageX, pageY, opt_touch) {
        let layout = this._chartLayout;
        let hideHint = true;
        layout.updateAbsCoordinates(this._canvasJqElem);

        if (this._hintEnabled && layout.pointInPlotArea(pageX, pageY)) {
            let ptInd = this._getPointIndex(pageX);

            if (ptInd >= 0) {
                let areaRect = layout.absPlotAreaRect;
                let chartOffset = this._chartJqElem.offset();

                let timePoint = this.chartData.timePoints[ptInd];
                let x = scada.chart.TimePoint.getUtc(timePoint);
                let ptPageX = this._trendXToPageX(x);

                if (areaRect.left <= ptPageX && ptPageX < areaRect.right) {
                    hideHint = false;

                    // set position and show the time mark
                    this._timeMarkJqElem
                        .removeClass("hidden")
                        .css({
                            "left": ptPageX - chartOffset.left,
                            "top": areaRect.top - chartOffset.top,
                            "height": areaRect.height
                        });

                    // set text, position and show the trend hint
                    this._trendHintJqElem.find("div.time")
                        .text(this._pointToDateTimeString(timePoint, Chart._DATE_TIME_OPTIONS_SEC));
                    let trendCnt = this.chartData.trends.length;
                    let hintValCells = this._trendHintJqElem.find("td.val");

                    for (let trendInd = 0; trendInd < trendCnt; trendInd++) {
                        let trend = this.chartData.trends[trendInd];
                        let trendPoint = trend.points[ptInd];
                        hintValCells.eq(trendInd)
                            .text(this.chartData.getTrendPointHintText(trendPoint, trend.unitName))
                            .css("color", this.chartData.getTrendPointColor(trendPoint));
                    }

                    // allow measuring the hint size
                    this._trendHintJqElem
                        .css({
                            "left": 0,
                            "top": 0,
                            "visibility": "hidden"
                        })
                        .removeClass("hidden");

                    let hintWidth = this._trendHintJqElem.outerWidth();
                    let hintHeight = this._trendHintJqElem.outerHeight();
                    let winScrollLeft = $(window).scrollLeft();
                    let winRight = winScrollLeft + $(window).width();
                    let chartRight = winScrollLeft + chartOffset.left + layout.width;
                    let maxRight = Math.min(winRight, chartRight);
                    let absHintLeft = pageX + hintWidth < maxRight ? pageX : Math.max(pageX - hintWidth, 0);

                    this._trendHintJqElem.css({
                        "left": absHintLeft,
                        "top": pageY - chartOffset.top - hintHeight -
                            (opt_touch ? scada.chart.ChartLayout.HINT_OFFSET /*above a finger*/ : 0),
                        "visibility": ""
                    });
                }
            }
        }

        if (hideHint) {
            this._hideHint();
        }
    }

    // Hides the hint.
    _hideHint() {
        this._timeMarkJqElem.addClass("hidden");
        this._trendHintJqElem.addClass("hidden");
    }

    // Draws chart areas as colored rectangles for testing.
    _drawAreas() {
        let layout = this._chartLayout;

        // plot area
        this._setColor("#ffa500");
        this._context.fillRect(
            layout.plotAreaRect.left + layout.canvasXOffset, layout.plotAreaRect.top + layout.canvasYOffset,
            layout.plotAreaRect.width, layout.plotAreaRect.height);

        // X-axis area
        this._setColor("#008000");
        this._context.fillRect(
            layout.xAxisLayout.areaRect.left + layout.canvasXOffset, layout.xAxisLayout.areaRect.top + layout.canvasYOffset,
            layout.xAxisLayout.areaRect.width, layout.xAxisLayout.areaRect.height);

        // legend area
        this._setColor("#ff00ff");
        this._context.fillRect(
            layout.legendAreaRect.left + layout.canvasXOffset, layout.legendAreaRect.top + layout.canvasYOffset,
            layout.legendAreaRect.width, layout.legendAreaRect.height);

        // Y-axis areas
        let n = 0;
        for (let yAxisRect of layout.yAxisAreaRects) {
            this._setColor(++n % 2 ? "#0000ff" : "#00ffff");
            this._context.fillRect(
                yAxisRect.left + layout.canvasXOffset, yAxisRect.top + layout.canvasYOffset,
                yAxisRect.width, yAxisRect.height);
        }
    }

    // Shows the chart title.
    _showTitle() {
        if (this._titleJqElem) {
            this._titleJqElem.find(".chart-title-text:first").text(this.controlOptions.chartTitle);
        }
    }

    // Shows the chart status.
    _showStatus() {
        if (this._titleJqElem) {
            let statusElem = this._titleJqElem.find(".chart-status:first");
            statusElem.children(".chart-status-text:first").text(this.controlOptions.chartStatus);

            if (this.controlOptions.hasError) {
                statusElem.addClass("error");
            } else {
                statusElem.removeClass("error");
            }
        }
    }

    // Builds the DOM of the chart.
    buildDom() {
        if (this._chartJqElem && this.displayOptions) {
            // get chart padding
            let chartPadding = this.displayOptions.chartArea.chartPadding;
            let paddingTop = scada.chart.DisplayOptions.getMargin(chartPadding, 0) + "px";
            let paddingRight = scada.chart.DisplayOptions.getMargin(chartPadding, 1) + "px";
            let paddingBottom = scada.chart.DisplayOptions.getMargin(chartPadding, 2) + "px";
            let paddingLeft = scada.chart.DisplayOptions.getMargin(chartPadding, 3) + "px";

            // find menu
            let menuJqElem = this._chartJqElem.siblings(".chart-menu");
            let menuExists = menuJqElem.length > 0;

            // create title
            if (this.displayOptions.chartTitle.showTitle) {
                this._titleJqElem = $("<div class='chart-title'></div>");
                let chartTitle = this.displayOptions.chartTitle;

                this._titleJqElem.css({
                    "height": chartTitle.height + "px",
                    "padding-left": paddingLeft,
                    "padding-right": paddingRight,
                    "font-size": chartTitle.fontSize + "px",
                    "line-height": chartTitle.fontSize + "px",
                    "color": chartTitle.foreColor
                });

                let menuHolderHtml = chartTitle.showMenu && menuExists ? "<div class='chart-menu-holder'></div>" : "";
                let titleTextHtml = "<div class='chart-title-text'></div>";
                let statusHtml = chartTitle.showStatus ?
                    "<div class='chart-status'><span class='chart-status-text'></span></div>" : "";

                this._titleJqElem.append(menuHolderHtml + titleTextHtml + statusHtml);
                this._chartJqElem.append(this._titleJqElem);
                this._showTitle();
                this._showStatus();

                if (menuHolderHtml) {
                    // insert menu
                    let menuBtnWidth = menuJqElem.css("width");
                    menuJqElem.detach();
                    this._titleJqElem.find(".chart-menu-holder:first").append(menuJqElem);
                    this._titleJqElem.find(".chart-title-text:first").css("padding-left", menuBtnWidth);
                } else {
                    menuJqElem.remove();
                }
            } else {
                // remove menu
                menuJqElem.remove();
            }

            // create canvas
            this._canvasJqElem = $("<canvas class='chart-canvas'>Upgrade the browser to display a chart.</canvas>");
            this._chartJqElem.append(this._canvasJqElem);

            this._canvas = this._canvasJqElem[0];
            this._context = this._canvas.getContext("2d");

            // set chart style
            this._chartJqElem.css({
                "border" : 0,
                "padding-top": paddingTop,
                "padding-right": 0,
                "padding-bottom": paddingBottom,
                "padding-left": 0,
                "font-family": this.displayOptions.chartArea.fontName,
                "background-color": this.displayOptions.chartArea.backColor
            });
        }
    }

    // Shows the chart status.
    setStatus(statusText, hasError) {
        this.controlOptions.chartStatus = statusText;
        this.controlOptions.hasError = hasError;
        this._showStatus();
    }

    // Draws the chart.
    draw() {
        if (this._canvas && this.displayOptions && this.timeRange && this.chartData) {
            // initialize necessary objects
            this._initAxisTags();
            this._initTrendFields();
            this._initTimeMark();
            this._initTrendHint();

            // calculate layout
            let layout = this._chartLayout;
            layout.calculate(this.displayOptions, this._chartJqElem, this._context,
                this._xAxisTag, this._yAxisTags, this.chartData.trends.length);

            // update canvas size
            this._chartJqElem.css("overflow", "hidden"); // prevent redundant scrollbars
            this._canvasJqElem
                .outerWidth(layout.canvasAreaRect.width)
                .outerHeight(layout.canvasAreaRect.height);
            this._chartJqElem.css("overflow", "");

            this._canvas.width = layout.canvasAreaRect.width;
            this._canvas.height = layout.canvasAreaRect.height;

            // draw chart
            this._clearRect(0, 0, layout.canvasAreaRect.width, layout.canvasAreaRect.height);
            //this._drawAreas(); // draw colored rectangles for testing
            this._drawFrame();
            this._drawXGrid();
            this._drawLegend();

            for (let yAxisTag of this._yAxisTags) {
                this._drawYGrid(yAxisTag);
            }

            for (let yAxisTag of this._yAxisTags) {
                this._drawTrends(yAxisTag);
            }
        }
    }

    // Resumes drawing of the chart.
    resume(pointIndex) {
        if (pointIndex < this.chartData.timePoints.length) {
            if (this._yRangeIsOutdated(pointIndex)) {
                this._calcAllYRanges();
                this.draw();
            } else {
                for (let yAxisTag of this._yAxisTags) {
                    this._drawTrends(yAxisTag, pointIndex ? pointIndex - 1 : 0);
                }
            }
        }
    }

    // Sets the displayed time range.
    setRange(startX, endX) {
        // swap the range if needed
        if (startX > endX) {
            let xbuf = startX;
            startX = endX;
            endX = xbuf;
        }

        // correct the range
        startX = Math.max(startX, this.timeRange.startTime);
        endX = Math.min(endX, this.timeRange.endTime);

        // apply the new range
        if (startX !== endX) {
            let isZoomed = startX > this.timeRange.startTime || endX < this.timeRange.endTime;
            this._xAxisTag.min = startX;
            this._xAxisTag.max = endX;
            this._xAxisTag.alignToGrid = isZoomed;
            this._zoomMode = isZoomed;
            this._calcAllYRanges();
            this.draw();
        }
    }

    // Resets the displayed time range according to the chart time range.
    resetRange() {
        this._initAxisTags(true);
        this.draw();
    }

    // Binds the events to allow hints.
    bindHintEvents() {
        if (this._canvasJqElem) {
            const thisObj = this;

            $(this._canvasJqElem.parent())
                .off(".scada.chart.hint")
                .on("mousemove.scada.chart.hint touchstart.scada.chart.hint touchmove.scada.chart.hint", function (event) {
                    let touch = false;
                    let stopEvent = false;

                    if (event.type === "touchstart") {
                        event = event.originalEvent.touches[0];
                        touch = true;
                    }
                    else if (event.type === "touchmove") {
                        $(this).off("mousemove");
                        event = event.originalEvent.touches[0];
                        touch = true;
                        stopEvent = true;
                    }

                    thisObj._showHint(event.pageX, event.pageY, touch);
                    return !stopEvent;
                })
                .on("mouseleave.scada.chart.hint", function () {
                    thisObj._hideHint();
                });
        }
    }
};
