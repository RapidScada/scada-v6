// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

// Contains classes required for a chart control.
// Classes:
//     ControlOptions, DisplayOptions, TimeRange, TimePoint, Trend, TrendPoint, CnlStatus, ChartData,
//     AxisTag, AxisLayout, ChartLayout
// Constants and enumerations:
//     ChartConst, KnownStatus, AreaPosition
// No dependencies.

// Namespaces
var scada = scada || {};
scada.chart = scada.chart || {};

// Specifies the chart constants.
scada.chart.ChartConst = class ChartConst {
    static MS_PER_MIN = 60 * 1000;
    static MS_PER_HOUR = 60 * ChartConst.MS_PER_MIN;
    static MS_PER_DAY = 24 * ChartConst.MS_PER_HOUR;
};

// Specifies the channel statuses used by a chart.
scada.chart.KnownCnlStatus = class KnownCnlStatus {
    static UNDEFINED = 0;
    static DEFINED = 1;
    static FORMULA_ERROR = 3;
    static UNRELIABLE = 4;
    static NORMAL = 13;

    static isVisibleOnTrend(status) {
        return status > KnownCnlStatus.UNDEFINED &&
            status !== KnownCnlStatus.FORMULA_ERROR &&
            status !== KnownCnlStatus.UNRELIABLE;
    }

    static isVisibleOnHint(status) {
        return status !== KnownCnlStatus.UNDEFINED &&
            status !== KnownCnlStatus.DEFINED &&
            status !== KnownCnlStatus.NORMAL;
    }
}

// Specifies the chart area positions.
scada.chart.AreaPosition = class {
    static NONE = 0;
    static TOP = 1;
    static RIGHT = 2;
    static BOTTOM = 3;
    static LEFT = 4;
};

// Represents options for a chart control.
scada.chart.ControlOptions = class {
    chartTitle = "";
    chartStatus = "";
    hasError = false;
    locale = "en-GB";
    gapBetweenPoints = 90000; // ms
};

// Represents chart display options.
scada.chart.DisplayOptions = class {
    chartArea = {
        chartPadding: [10, 20, 10, 10],
        fontName: "Arial",
        backColor: "#ffffff"
    };

    chartTitle = {
        showTitle: true,
        showMenu: true,
        showStatus: true,
        height: 30,
        fontSize: 17,
        foreColor: "#333333"
    };

    plotArea = {
        frameColor: "#808080",
        gridColor: "#e0e0e0",
        backColor: "#ffffff",
        markerColor: "#000000",
        selectionColor: "#6aaaea",
        lineWidth: 1,
        trendColorAsStatus: false,
        trendColors: ["#ff0000", "#0000ff", "#008000", "#ff00ff", "#ffa500",
            "#00ffff", "#00ff00", "#4b0082", "#ff1493", "#8b4513"]
    };

    xAxis = {
        height: 30,
        showGridLines: true,
        showDates: true,
        majorTickSize: 4,
        minorTickSize: 2,
        showMinorTicks: true,
        labelMargin: [2, 3],
        fontSize: 12,
        lineColor: "#808080",
        textColor: "#000000"
    };

    yAxes = [{
        position: scada.chart.AreaPosition.LEFT,
        autoWidth: true,
        width: 0,
        showTitle: true,
        showGridLines: true,
        majorTickSize: 4,
        minorTickSize: 2,
        minorTickCount: 4,
        labelMargin: [2, 3],
        fontSize: 12,
        lineColor: "#808080",
        textColor: "#000000",
        trendColor: "",
        autoScale: true,
        includeZero: true,
        min: 0.0,
        max: 0.0,
        quantityIDs: []
    }];

    legend = {
        position: scada.chart.AreaPosition.BOTTOM,
        columnWidth: 300,
        columnMargin: [10, 10, 0],
        columnCount: 1,
        lineHeight: 18,
        iconWidth: 12,
        iconHeight: 12,
        fontSize: 12,
        foreColor: "#000000"
    };

    // Gets the specified maring using CSS rules. Index: 0 - top, 1 - right, 2 - bottom, 3 - left.
    static getMargin(margin, index) {
        if (Array.isArray(margin) && margin.length > 0 && 0 <= index && index <= 3) {
            if (index < margin.length) {
                return margin[index];
            } else if (index >= 2) {
                return this.getMargin(margin, index - 2);
            } else if (index >= 1) {
                return this.getMargin(margin, index - 1);
            } else {
                return 0;
            }
        } else {
            return 0;
        }
    }
};

// Represents a time range.
scada.chart.TimeRange = class {
    // The left edge of the range. Unix time in milliseconds.
    startTime = 0;
    // The right edge of the range.
    endTime = 1;
    // The dictionary of local hours within the time range. Strings, like "2022-12-31T00:00:00", accessed by Unix time.
    hourMap = new Map();
};

// Contains methods for accessing a time point.
scada.chart.TimePoint = class TimePoint {
    static _UTC_IDX = 0;
    static _LOCAL_IDX = 1;

    // Gets the UTC time in milliseconds.
    static getUtc(timePoint) {
        return timePoint[TimePoint._UTC_IDX];
    }

    // Gets the local date and time as a string, for example, "2022-12-31T00:00:00".
    static getLocal(timePoint) {
        return timePoint[TimePoint._LOCAL_IDX];
    }

    // Gets the local date as a string.
    static getLocalDate(timePoint) {
        return timePoint[TimePoint._LOCAL_IDX].substring(0, 10);
    }

    // Gets the local time as a string.
    static getLocalTime(timePoint) {
        return timePoint[TimePoint._LOCAL_IDX].substring(11);
    }
};

// Represents a trend.
scada.chart.Trend = class {
    // The channel number.
    cnlNum = 0;
    // The channel name.
    cnlName = "";
    // The quantity ID.
    quantityID = 0;
    // The quantity name.
    quantityName = "";
    // The unit name.
    unitName = "";
    // The trend points. Each point is an array of [value, status, "text"].
    points = [];
    // The trend color.
    color = "";
    // The trend caption.
    caption = "";

    // Gets an Y-axis title based on the trend quantity and unit.
    getAxisTitle() {
        return this.quantityName && this.unitName
            ? this.quantityName + ", " + this.unitName
            : this.quantityName + this.unitName;
    }
};

// Contains methods for accessing a trend point.
scada.chart.TrendPoint = class TrendPoint {
    static _VAL_IDX = 0;
    static _STAT_IDX = 1;
    static _TEXT_IDX = 2;

    // Gets the trend point value, or NaN if the point should be hidden according to its status.
    static getValue(trendPoint) {
        return scada.chart.KnownCnlStatus.isVisibleOnTrend(trendPoint[TrendPoint._STAT_IDX])
            ? trendPoint[TrendPoint._VAL_IDX]
            : NaN;
    }

    // Gets the trend point status.
    static getStatus(trendPoint) {
        return trendPoint[TrendPoint._STAT_IDX];
    }

    // Gets the trend point text.
    static getText(trendPoint) {
        return trendPoint[TrendPoint._TEXT_IDX];
    }
};

// Represents a channel status.
scada.chart.CnlStatus = class {
    cnlStatusID;
    name;
    mainColor;

    constructor(cnlStatusID, name, mainColor) {
        this.cnlStatusID = cnlStatusID;
        this.name = name;
        this.mainColor = mainColor;
    }
}

// Represents chart data.
scada.chart.ChartData = class ChartData {
    static _DEFAULT_POINT_COLOR = "#000000";

    // The time points. Each point is an array of [UTC, "local"].
    // The number of time points corresponds to the number of trend points.
    timePoints = [];
    // The trends to display. Each array element is of the Trend type.
    trends = [];
    // The dictionary of channel statuses accessed by status ID.
    cnlStatusMap = new Map();

    // Gets the hint text of the trend point.
    getTrendPointHintText(trendPoint, unitName) {
        let status = scada.chart.TrendPoint.getStatus(trendPoint);
        let text = scada.chart.TrendPoint.getText(trendPoint);

        // append unit name
        if (unitName && status > 0) {
            text += " " + unitName;
        }

        // append status name
        if (scada.chart.KnownCnlStatus.isVisibleOnHint(status)) {
            let cnlStatus = this.cnlStatusMap.get(status);

            if (cnlStatus && cnlStatus.name) {
                text += " (" + cnlStatus.name + ")";
            }
        }

        return text;
    }

    // Gets the color of the trend point.
    getTrendPointColor(trendPoint) {
        let cnlStatus = this.cnlStatusMap.get(scada.chart.TrendPoint.getStatus(trendPoint));
        return cnlStatus && cnlStatus.mainColor ? cnlStatus.mainColor : ChartData._DEFAULT_POINT_COLOR;
    }
};

// Represents information related to an axis.
scada.chart.AxisTag = class {
    // The minimum value.
    min = 0;
    // The maximum value.
    max = 0;
    // The transformation coefficient.
    coef = 1;
    // Indicates that min and max should be updated to match grid lines.
    alignToGrid = false;
    // The axis title.
    title = "";
    // The trends that use this axis.
    trends = [];
    // The axis configuration.
    axisConfig = null;
    // The axis layout.
    axisLayout = null;

    // Checks if the axis tag has not been initialized yet.
    isNotInitialized() {
        return this.min === this.max;
    }
};

// Represents an axis layout.
scada.chart.AxisLayout = class {
    // The start grid value.
    gridStart = 0;
    // The grid step.
    gridStep = 0;
    // The step of the minor ticks.
    minorTickStep = 0;
    // The number of decimal digits to use in labels.
    gridDigits = 0;
    // The axis area width.
    areaWidth = 0;
    // The axis area rectangle.
    areaRect = new DOMRect(0, 0, 0, 0);
};

// Represents a chart layout.
scada.chart.ChartLayout = class ChartLayout {
    // The desirable number of horizontal grid lines.
    static _GRID_HOR_LINE_CNT = 10;
    // The vertical hint offset relative to the cursor.
    static HINT_OFFSET = 20;

    // The chart area width.
    width = 0;
    // The chart area height.
    height = 0;
    // The title area rectangle.
    titleAreaRect = new DOMRect(0, 0, 0, 0);
    // The canvas area rectangle.
    canvasAreaRect = new DOMRect(0, 0, 0, 0);
    // The plot area rectangle.
    plotAreaRect = new DOMRect(0, 0, 0, 0);
    // The plot area rectangle relative to the canvas.
    canvasPlotAreaRect = new DOMRect(0, 0, 0, 0);
    // The plot area rectangle relative to the document.
    absPlotAreaRect = new DOMRect(0, 0, 0, 0);
    // The legend area rectangle.
    legendAreaRect = new DOMRect(0, 0, 0, 0);
    // The X-axis layout.
    xAxisLayout = new scada.chart.AxisLayout();
    // The X-axis layouts.
    yAxisLayouts = [];

    // The canvas X offset. Add this value to the X coordinate of an area to draw on the canvas.
    canvasXOffset = 0;
    // The canvas Y offset. Add this value to the Y coordinate of an area to draw on the canvas.
    canvasYOffset = 0;

    // Calculates the X-axis layout.
    _calcXLayout(xAxisTag) {
        const MS_PER_MIN = scada.chart.ChartConst.MS_PER_MIN;
        const MS_PER_HOUR = scada.chart.ChartConst.MS_PER_HOUR;
        const MS_PER_DAY = scada.chart.ChartConst.MS_PER_DAY;
        const rangeCnt = 8;

        // displayed ranges, ms
        let ranges = [16 * MS_PER_DAY, 8 * MS_PER_DAY, 4 * MS_PER_DAY, 2 * MS_PER_DAY, MS_PER_DAY,
            12 * MS_PER_HOUR, 6 * MS_PER_HOUR, 2 * MS_PER_HOUR];

        // grid steps according to the ranges, ms
        let gridSteps = [24 * MS_PER_HOUR, 12 * MS_PER_HOUR, 6 * MS_PER_HOUR, 3 * MS_PER_HOUR, 2 * MS_PER_HOUR,
            MS_PER_HOUR, 30 * MS_PER_MIN, 15 * MS_PER_MIN];

        let minorSteps = [12 * MS_PER_HOUR, 6 * MS_PER_HOUR, 3 * MS_PER_HOUR, MS_PER_HOUR, MS_PER_HOUR,
            30 * MS_PER_MIN, 15 * MS_PER_MIN, 5 * MS_PER_MIN];

        let gridStep = 5 * MS_PER_MIN; // 5 minutes by default
        let minorStep = MS_PER_MIN;    // 1 minute by default
        let range = xAxisTag.max - xAxisTag.min;

        for (let i = 0; i < rangeCnt; i++) {
            if (range > ranges[i]) {
                gridStep = gridSteps[i];
                minorStep = minorSteps[i];
                break;
            }
        }

        this.xAxisLayout.gridStart = xAxisTag.min;
        this.xAxisLayout.gridStep = gridStep;
        this.xAxisLayout.minorTickStep = minorStep;
        this.xAxisLayout.gridDigits = 0;
        this.xAxisLayout.areaWidth = 0;
    }

    // Calculates the Y-axis layout.
    _calcYLayout(yAxisTag, fontName, canvasContext) {
        let gridYStep = (yAxisTag.max - yAxisTag.min) / ChartLayout._GRID_HOR_LINE_CNT;
        let gridYDecDig = 0;
        let n = 1;

        if (gridYStep >= 1) {
            while (gridYStep > 10) {
                gridYStep /= 10;
                n *= 10;
            }
        } else {
            while (gridYStep < 1) {
                gridYStep *= 10;
                n /= 10;
                gridYDecDig++;
            }
        }

        gridYStep = Math.floor(gridYStep);

        // the first significant digit of the grid step is 1, 2 or 5
        if (3 <= gridYStep && gridYStep <= 4) {
            gridYStep = 2;
        }
        else if (6 <= gridYStep && gridYStep <= 9) {
            gridYStep = 5;
        }

        gridYStep *= n;
        let gridYStart = Math.floor(yAxisTag.min / gridYStep) * gridYStep + gridYStep;

        // measure max data label width
        let yAxisConfig = yAxisTag.axisConfig;
        let maxLabelWidth = 0;
        canvasContext.font = yAxisConfig.fontSize + "px " + fontName;

        for (let y = gridYStart; y < yAxisTag.max; y += gridYStep) {
            let w = canvasContext.measureText(y.toFixed(gridYDecDig)).width;
            if (maxLabelWidth < w)
                maxLabelWidth = w;
        }

        let yAxisLayout = yAxisTag.axisLayout;
        yAxisLayout.gridStart = gridYStart;
        yAxisLayout.gridStep = gridYStep;
        yAxisLayout.minorTickStep = yAxisConfig.minorTickCount > 0 ? gridYStep / (yAxisConfig.minorTickCount + 1) : 0;
        yAxisLayout.gridDigits = gridYDecDig;
        yAxisLayout.areaWidth = yAxisConfig.majorTickSize + Math.ceil(maxLabelWidth) +
            scada.chart.DisplayOptions.getMargin(yAxisConfig.labelMargin, 1) +
            scada.chart.DisplayOptions.getMargin(yAxisConfig.labelMargin, 3);

        if (yAxisConfig.showTitle) {
            yAxisLayout.areaWidth += yAxisConfig.fontSize +
                scada.chart.DisplayOptions.getMargin(yAxisConfig.labelMargin, 0) +
                scada.chart.DisplayOptions.getMargin(yAxisConfig.labelMargin, 2);
        }
    }

    // Corrects edges of the X-axis to align to grid.
    _alignXToGrid(xAxisTag) {
        let gridXStep = this.xAxisLayout.gridStep;
        xAxisTag.min = Math.floor(xAxisTag.min / gridXStep) * gridXStep;
        xAxisTag.max = Math.ceil(xAxisTag.max / gridXStep) * gridXStep;
    }

    // Offsets the specified rectangle.
    _offsetRect(rect, xOffset, yOffset) {
        return new DOMRect(rect.x + xOffset, rect.y + yOffset, rect.width, rect.height);
    }

    // Calculates the chart layout.
    calculate(displayOptions, chartJqElem, canvasContext, xAxisTag, yAxisTags, trendCnt) {
        // chart area
        this.width = chartJqElem.outerWidth();
        this.height = chartJqElem.outerHeight();

        let chartPadding = displayOptions.chartArea.chartPadding;
        let mainLeft = scada.chart.DisplayOptions.getMargin(chartPadding, 3);
        let mainRight = this.width - scada.chart.DisplayOptions.getMargin(chartPadding, 1);
        let mainTop = scada.chart.DisplayOptions.getMargin(chartPadding, 0);
        let mainBottom = this.height - scada.chart.DisplayOptions.getMargin(chartPadding, 2);
        let mainWidth = mainRight - mainLeft;
        let mainHeight = mainBottom - mainTop;

        // title
        let titleHeight = displayOptions.chartTitle.showTitle ? displayOptions.chartTitle.height : 0;
        let plotAreaTop = mainTop + titleHeight;
        this.titleAreaRect = new DOMRect(mainLeft, mainTop, mainWidth, titleHeight);

        // canvas
        this.canvasAreaRect = new DOMRect(0, mainTop + titleHeight, this.width, mainHeight - titleHeight);
        this.canvasXOffset = -this.canvasAreaRect.left;
        this.canvasYOffset = -this.canvasAreaRect.top;

        // legend
        let legendWidth = displayOptions.legend.columnCount * (displayOptions.legend.columnWidth +
            scada.chart.DisplayOptions.getMargin(displayOptions.legend.columnMargin, 1) +
            scada.chart.DisplayOptions.getMargin(displayOptions.legend.columnMargin, 3));
        let legendLineCnt = Math.ceil(trendCnt / displayOptions.legend.columnCount);
        let legendHeight = legendLineCnt * displayOptions.legend.lineHeight +
            scada.chart.DisplayOptions.getMargin(displayOptions.legend.columnMargin, 0) +
            scada.chart.DisplayOptions.getMargin(displayOptions.legend.columnMargin, 2);
        let legendAtBottom = displayOptions.legend.position === scada.chart.AreaPosition.BOTTOM;
        let legendAtRight = displayOptions.legend.position === scada.chart.AreaPosition.RIGHT;

        let xAxisBottom = legendAtBottom ? mainBottom - legendHeight : mainBottom;
        let xAxisTop = xAxisBottom - displayOptions.xAxis.height;
        let plotAreaHeight = xAxisTop - plotAreaTop;

        if (legendAtBottom) {
            this.legendAreaRect = new DOMRect(mainLeft, xAxisBottom, mainWidth, legendHeight);
        } else if (legendAtRight) {
            this.legendAreaRect = new DOMRect(mainRight - legendWidth, plotAreaTop, legendWidth, plotAreaHeight);
        } else {
            this.legendAreaRect = new DOMRect(0, 0, 0, 0);
        }

        // X-axis
        this._calcXLayout(xAxisTag);
        this.xAxisLayout.areaRect = new DOMRect(0, xAxisTop, this.width, displayOptions.xAxis.height);

        if (xAxisTag.alignToGrid) {
            this._alignXToGrid(xAxisTag);
        }

        // Y-axes and plot area
        this.yAxisAreaRects = [];
        this.yAxisLayouts = [];
        let xLeft = mainLeft;
        let xRight = legendAtRight ? this.legendAreaRect.left : mainRight;

        // Y-axes on the left
        for (let i = 0, len = yAxisTags.length; i < len; i++) {
            let yAxisTag = yAxisTags[i];

            if (yAxisTag.axisConfig.position === scada.chart.AreaPosition.LEFT) {
                yAxisTag.coef = (plotAreaHeight - 3) / (yAxisTag.max - yAxisTag.min);
                yAxisTag.axisLayout = new scada.chart.AxisLayout();
                this._calcYLayout(yAxisTag, displayOptions.chartArea.fontName, canvasContext);

                let yAxisConfig = yAxisTag.axisConfig;
                let yAxisLayout = yAxisTag.axisLayout;
                let yAxisWidth = yAxisConfig.autoWidth ? yAxisLayout.areaWidth : yAxisConfig.width;

                let yAxisLeft = xLeft;
                xLeft += yAxisWidth;

                yAxisLayout.areaRect = new DOMRect(yAxisLeft, plotAreaTop, yAxisWidth, plotAreaHeight);
                this.yAxisLayouts.push(yAxisLayout);
            }
        }

        // Y-axes on the right
        for (let i = yAxisTags.length - 1; i >= 0; i--) {
            let yAxisTag = yAxisTags[i];

            if (yAxisTag.axisConfig.position === scada.chart.AreaPosition.RIGHT) {
                yAxisTag.coef = (plotAreaHeight - 3) / (yAxisTag.max - yAxisTag.min);
                yAxisTag.axisLayout = new scada.chart.AxisLayout();
                this._calcYLayout(yAxisTag, displayOptions.chartArea.fontName, canvasContext);

                let yAxisConfig = yAxisTag.axisConfig;
                let yAxisLayout = yAxisTag.axisLayout;
                let yAxisWidth = yAxisConfig.autoWidth ? yAxisLayout.areaWidth : yAxisConfig.width;

                xRight -= yAxisWidth;
                let yAxisLeft = xRight;

                yAxisLayout.areaRect = new DOMRect(yAxisLeft, plotAreaTop, yAxisWidth, plotAreaHeight);
                this.yAxisLayouts.push(yAxisLayout);
            }
        }

        // plot area
        this.plotAreaRect = new DOMRect(xLeft, plotAreaTop, xRight - xLeft, plotAreaHeight);
        this.canvasPlotAreaRect = this._offsetRect(this.plotAreaRect, this.canvasXOffset, this.canvasYOffset);
        xAxisTag.coef = (this.plotAreaRect.width - 1) / (xAxisTag.max - xAxisTag.min);
    }

    // Updates the absolute coordinates of the plot area.
    updateAbsCoordinates(canvasJqObj) {
        let offset = canvasJqObj.offset();
        this.absPlotAreaRect = this._offsetRect(this.canvasPlotAreaRect, offset.left, offset.top);
    }

    // Checks if the specified point is located within the plot area.
    pointInPlotArea(pageX, pageY) {
        return this.absPlotAreaRect.left <= pageX && pageX < this.absPlotAreaRect.right &&
            this.absPlotAreaRect.top <= pageY && pageY < this.absPlotAreaRect.bottom;
    }
};
