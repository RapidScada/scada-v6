// The variables below must be defined in ChartPro.cshtml
// displayOptions
// dataOptions
// controlOptions
// timeRange
// chartData
// formArgs
// chartPhrases

// Specifies the chart modes.
ChartMode = class {
    static FIXED = 0;
    static ROLLING = 1;
};

// Specifies the loading states.
LoadState = class {
    static NOT_STARTED = 0;
    static FAST_LOADING = 1;
    static AUTO_REFRESH = 2;
    static COMPLETE = 3;

    static getName(value) {
        switch (value) {
            case LoadState.NOT_STARTED:
                return "NOT_STARTED";

            case LoadState.FAST_LOADING:
                return "FAST_LOADING";

            case LoadState.AUTO_REFRESH:
                return "AUTO_REFRESH";

            case LoadState.COMPLETE:
                return "COMPLETE";

            default:
                return value;
        }
    }
};

// The data refresh rate in FAST_LOADING state.
var FAST_REFRESH_RATE = 100;
// The data refresh rate if an error occurs.
var ERROR_REFRESH_RATE = 10000;
// The chart loading delay.
var LOAD_DELAY = 1000;
// The chart API root path relative to ChartPro.cshtml.
var API_ROOT_PATH = "../";

// Manages modal dialogs.
var modalManager = new ModalManager();
// Displays standard modal dialogs
var dialogs = new Dialogs("../", modalManager);
// The chart.
var chart = null;
// The number of displayed points in the data panel.
var dataPtCnt = 0;
// The time offset to request new data.
var timeOffset = 0;
// The current loading state.
var loadState = LoadState.NOT_STARTED;

// Generates and appends rows for new data.
function appendDataRows() {
    const timePoints = chart.chartData.timePoints;
    const trends = chart.chartData.trends;
    let timePtCnt = timePoints.length;
    let dataTable = $("#tblData");

    // remove outdated rows
    if (formArgs.chartMode === ChartMode.ROLLING) {
        dataPtCnt = 0;

        if (timePtCnt > 0) {
            let firstUtc = scada.chart.TimePoint.getUtc(timePoints[0]);

            dataTable.find("tr").each(function () {
                let rowElem = $(this);
                let utc = rowElem.data("utc");

                if (utc) {
                    if (utc < firstUtc) {
                        rowElem.remove();
                    } else if (rowElem.hasClass("point")) {
                        dataPtCnt++;
                    }
                }
            });
        } else {
            dataTable.find("tr.date, tr.point").remove();
        }
    }

    if (timePtCnt > 0 && trends.length > 0) {
        // create data table if needed
        if (dataTable.length === 0) {
            let dataTableHtml = "<table id='tblData'></table>";
            dataTable = $(dataTableHtml);

            // add table header
            let hdrRowHtml = "<thead><tr class='hdr'><th><span>" + chartPhrases.timeCol + "</span></th>";
            for (let trend of trends) {
                hdrRowHtml += "<th><span>[" + trend.cnlNum + "]</span></th>";
            }
            hdrRowHtml += "</thead></tr>";
            dataTable.append(hdrRowHtml);

            $("#divData").append(dataTable);
            $("#spanNoData").addClass("hidden");
        }

        // add trend data
        if (dataPtCnt < timePtCnt) {
            let newRows = new Array(timePtCnt - dataPtCnt);
            let rowInd = 0;
            let prevDate = dataPtCnt > 0
                ? scada.chart.TimePoint.getLocal(timePoints[dataPtCnt - 1]).substring(0, 10)
                : null;
            let emptyCells = ""; // empty cells for date row

            for (let trend of trends) {
                emptyCells += "<td></td>";
            }

            for (let timePtInd = dataPtCnt; timePtInd < timePtCnt; timePtInd++) {
                let timePoint = timePoints[timePtInd];
                let utc = scada.chart.TimePoint.getUtc(timePoint);

                // add date row if date is changed
                let localDate = scada.chart.TimePoint.getLocalDate(timePoint);

                if (prevDate !== localDate) {
                    prevDate = localDate;
                    newRows.length++;
                    newRows[rowInd] = $("<tr class='date' data-utc='" + utc + "'><td>" +
                        chart.pointToDateString(timePoint) + "</td>" + emptyCells + "</tr>");
                    rowInd++;
                }

                // add data row
                let dataRowElem = $("<tr class='point' data-utc='" + utc + "'><td>" +
                    scada.chart.TimePoint.getLocalTime(timePoint) + "</td></tr>");

                for (let trend of trends) {
                    let trendPoint = trend.points[timePtInd];
                    let cellElem = $("<td></td>")
                        .css("color", chartData.getTrendPointColor(trendPoint))
                        .text(scada.chart.TrendPoint.getText(trendPoint));
                    dataRowElem.append(cellElem);
                }

                newRows[rowInd] = dataRowElem;
                rowInd++;
            }

            dataTable.append(newRows);
            dataPtCnt = timePtCnt;
        }
    }
}

// Constructs chart data markup and shows it.
function showData() {
    const MAX_DATA_WIDTH = 300;

    appendDataRows();

    let divChartSplitter = $("#divChartSplitter");
    let divData = $("#divData");
    divChartSplitter.removeClass("hidden");
    divData.removeClass("hidden");

    // limit data width
    if (!divData.data("widthChecked")) {
        divData.data("widthChecked", true);

        if (divData.outerWidth() > MAX_DATA_WIDTH) {
            divData.outerWidth(MAX_DATA_WIDTH);
        }
    }

    updateLayout();
    chart.draw();
}

// Hides chart data.
function hideData() {
    $("#divChartSplitter").addClass("hidden");
    $("#divData").addClass("hidden");
    updateLayout();
    chart.draw();
}

// Updates the form layout.
function updateLayout() {
    let divChartContent = $("#divChartContent");
    let divChart = $("#divChart");
    let divChartSplitter = $("#divChartSplitter");
    let divData = $("#divData");
    let winH = $(window).height();

    divChartContent.outerHeight(winH);
    divChart.outerHeight(winH);
    divChartSplitter.outerHeight(winH);
    divData.outerHeight(winH, true);

    if (divData.hasClass("hidden")) {
        divChart.outerWidth($(window).width());
    } else {
        divChart.outerWidth($(window).width() - divData.outerWidth() - divChartSplitter.outerWidth());
    }
}

// Setups the Close button.
function setupCloseButton() {
    if (ScadaUtils.isActualFullscreen) {
        $("#divClose")
            .detach()
            .appendTo(".chart-status")
            .removeClass("hidden");

        $("#btnClose").on("click", function () {
            window.close();
            return false;
        });
    }
}

// Binds events to the menu items.
function bindMenuEvents() {
    // disable menu transparency for clicks
    $("#divMenu .dropdown-item").on("mousedown", function () {
        return false;
    });

    //添加3天的周期
    var threeDayPeriod = $(`<li><a id="aPastThreeToday" class="dropdown-item" href="#">Past 3 Days</a></li>`);
    $("#divMenu .dropdown-menu li").eq(2).before(threeDayPeriod);
    if (formArgs.chartMode === ChartMode.FIXED) {
        // fixed mode
        $("#aToday").on("click", function (event) {
            event.preventDefault();
            reloadWithPeriod(0, 1);
            ScadaUtils.setCookie("clickedHistChart", "Today");
        });

        $("#aYesterday").on("click", function (event) {
            event.preventDefault();
            reloadWithPeriod(-1, 1);
            ScadaUtils.setCookie("clickedHistChart", "Yesterday");
        });

        $("#aPastThreeToday").on("click", function (event) {
            event.preventDefault();
            reloadWithPeriod(-3, 4);
            ScadaUtils.setCookie("clickedHistChart", "PastWeek");
        });

        $("#aPastWeek").on("click", function (event) {
            event.preventDefault();
            reloadWithPeriod(-6, 7);
            ScadaUtils.setCookie("clickedHistChart", "PastWeek");
        });

        $("#aPastMonth").on("click", function (event) {
            event.preventDefault();
            reloadForPastMonth();
            ScadaUtils.setCookie("clickedHistChart", "PastMonth");
        });

        $("#aCustomPeriod").on("click", function (event) {
            event.preventDefault();
            showFixedPeriodModal();
            ScadaUtils.setCookie("clickedHistChart", "Custom");
        });
    } else {
        // rolling mode
        $("#aToday, #aYesterday,#aPastThreeToday ,#aPastWeek, #aPastMonth").addClass("hidden");

        $("#aCustomPeriod").on("click", function (event) {
            event.preventDefault();
            showRollingPeriodModal();
        });
    }

    //添加收藏功能
    var saveCurConfig = $(`<li><a id="aSaveCurConfig" class="dropdown-item" href="#">Add to Favorites</a></li>`);
    $("#divMenu .dropdown-menu").append(saveCurConfig);
    $("#aSelectChannels").on("click", function (event) {
        event.preventDefault();
        showSelectChannelsModal();
    });

    $("#aSelectProfile").on("click", function (event) {
        event.preventDefault();
        showProfileModal();
    });

    $("#aShowData").on("click", function (event) {
        event.preventDefault();
        $(this).addClass("hidden");
        $("#aHideData").removeClass("hidden");
        setTimeout(showData, 0); // close dropdown before show data
    });

    $("#aHideData").on("click", function (event) {
        event.preventDefault();
        $(this).addClass("hidden");
        $("#aShowData").removeClass("hidden");
        hideData();
    });

    $("#aExport").on("click", function (event) {
        event.preventDefault();
        showExportModal();
    });

    $("#aSaveCurConfig").on("click", function (event) {
        event.preventDefault();
        showSaveCurConfigModal();
    });
}
var histChartModalAlternative;
var histChartObj = {};
function setupHistChart() {
    var clickedJsonObj = $.cookie("histChartObject");
    if (clickedJsonObj) {
        histChartObj = JSON.parse(decodeURIComponent(clickedJsonObj));
        var content = JSON.parse(histChartObj.content);
        histChartObj.name = content.name;
        histChartObj.btnName = content.btnName;
        histChartObj.offsetFromToday = content.offsetFromToday;
        histChartObj.period = content.period;
        histChartObj.startDate = content.startDate;
        $.removeCookie('histChartObject', { path: '/' });
    }
    else {
        var clickedHistChart = ScadaUtils.getCookie("clickedHistChart");
        if (clickedHistChart) {
            histChartObj.offsetFromToday = ScadaUtils.getCookie("histChartOffsetFromToday");
            histChartObj.period = ScadaUtils.getCookie("histChartPeriod");
            histChartObj.btnName = clickedHistChart
        }
    }
    console.log(histChartObj);
}
function showSaveCurConfigModal() {
    if ($('#saveHistChartModal').length == 0) {
        var newModal = $(`<div class="modal fade" id="saveHistChartModal" tabindex="-1" aria-labelledby="saveHistChartModalLabel" aria-hidden="true">
                        <div class="modal-dialog">
                          <div class="modal-content">
                            <div class="modal-header">
                              <h1 class="modal-title fs-5" id="saveHistChartModalLabel">Save Chart pro config</h1>
                              <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                            </div>
                            <div class="modal-body">
                              <form>
                                <div class="mb-3">
                                  <label for="hist-name" class="col-form-label">Name:</label>
                                  <input type="text" class="form-control" id="hist-name">
                                </div>
                                <div class="mb-3">
                                  <label for="hist-search" class="col-form-label">SearchParams:</label>
                                  <input type="text" readonly class="form-control-plaintext" id="hist-search" value="">
                                </div>
                                <div class="mb-3">
                                  <label for="hist-btnname" class="col-form-label">Period:</label>
                                  <input type="text" readonly class="form-control-plaintext" id="hist-btnname" value="">
                                </div>
                              </form>
                            </div>
                            <div class="modal-footer">
                              <button type="button" class="btn btn-primary" id="btnSaveConfig">Save</button>
                              <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                            </div>
                          </div>
                        </div>
                      </div>`);
        $('body').append(newModal);
        histChartModalAlternative = new bootstrap.Modal('#saveHistChartModal', {});
        $('#btnSaveConfig').on("click", function (event) {
            event.preventDefault();
            var name = $('#hist-name').val();
            if (!!!name.trim()) {
                alert('Please input name!');
            } else {
                var postData = {
                    id: histChartObj.id || 0,
                    content: JSON.stringify({
                        name: $('#hist-name').val(),
                        search: $('#hist-search').val(),
                        btnName: $('#hist-btnname').val(),
                        offsetFromToday: histChartObj.offsetFromToday,
                        period: histChartObj.period,
                        startDate: histChartObj.startDate,
                        origin: location.origin,
                        path: location.pathname,
                    })
                };

                $.ajax({
                    url: '/chartfavor/edit',
                    type: 'POST',
                    contentType: "application/json",
                    dataType: 'json',
                    data: JSON.stringify(postData),
                    success: function (data) {
                        if (data.ok) layer.msg('Save success!');
                        histChartModalAlternative.hide();
                    },
                    error: function (xhr, status, error) {
                        console.error(xhr, status, error);
                        layer.msg('Save with unkonw error');
                    }
                });
            }
        });
    }
    if (histChartObj.btnName) {
        $('#hist-name').val(histChartObj.name || '');
        console.log('11', histChartObj);
    } else {
        histChartObj.btnName = "Today";
        console.log('22', histChartObj);
    }
    let searchParams = new URLSearchParams(location.search);
    histChartObj.search = searchParams.toString();
    console.log(histChartObj);
    $('#hist-search').val(histChartObj.search);
    $('#hist-btnname').val(histChartObj.btnName);
    histChartModalAlternative.show();
}

// Reloads the chart page with the specified period.
function reloadWithPeriod(offsetFromToday, periodInDays, opt_startDate) {
    let startDate = opt_startDate || new Date(formArgs.todayLocal);

    if (offsetFromToday !== 0) {
        startDate.setUTCDate(startDate.getUTCDate() + offsetFromToday);
    }

    let searchParams = new URLSearchParams(location.search);
    searchParams.set("startDate", startDate.toISOString().substring(0, 10));
    searchParams.set("period", periodInDays);
    ScadaUtils.setCookie("histChartOffsetFromToday", offsetFromToday);
    ScadaUtils.setCookie("histChartPeriod", periodInDays);
    location.search = searchParams.toString();
}

// Reloads the chart page with the period for the past month.
function reloadForPastMonth() {
    let startDate = new Date(formArgs.todayLocal);
    let daysInCurMonth = ScadaUtils.daysInMonth(startDate.getUTCFullYear(), startDate.getUTCMonth());
    let daysInMonth = startDate.getUTCDate() === daysInCurMonth
        ? daysInCurMonth
        : ScadaUtils.daysInMonth(startDate.getUTCFullYear(), startDate.getUTCMonth() - 1); // previous month
    reloadWithPeriod(1 - daysInMonth, daysInMonth, startDate);
}

// Shows a dialog for selecting a period in fixed mode.
function showFixedPeriodModal() {
    modalManager.showModal(
        `Period?startDate=${formArgs.startDateLocal}&endDate=${formArgs.endDateLocal}&` +
        `maxPeriod=${dataOptions.maxFixedPeriod}`,
        new ModalOptions({ buttons: ModalButton.OK_CANCEL }),
        function (result) {
            if (result) {
                let searchParams = new URLSearchParams(location.search);
                searchParams.set("startDate", result.startDate);
                searchParams.set("period", result.period);
                location.search = searchParams.toString();
            }
        });
}

// Shows a dialog for selecting a period in rolling mode.
function showRollingPeriodModal() {
    modalManager.showModal(
        `PeriodRolling?period=${parseInt(formArgs.rollingPeriod / scada.chart.ChartConst.MS_PER_MIN)}&` +
        `maxPeriod=${dataOptions.maxRollingPeriod}`,
        new ModalOptions({ buttons: ModalButton.OK_CANCEL }),
        function (result) {
            if (result) {
                let searchParams = new URLSearchParams(location.search);
                searchParams.set("periodMin", result.period);
                location.search = searchParams.toString();
            }
        });
}

// Shows a dialog for selecting channels.
function showSelectChannelsModal() {
    dialogs.selectChannels(formArgs.cnlNumsStr, function (result) {
        if (result) {
            let searchParams = new URLSearchParams(location.search);
            searchParams.set("cnlNums", result.cnlNums);
            location.search = searchParams.toString();
        }
    });
}

// Shows a dialog for selecting a chart profile.
function showProfileModal() {
    modalManager.showModal(
        `Profile?profile=${formArgs.chartProfile}`,
        new ModalOptions({ buttons: ModalButton.OK_CANCEL }),
        function (result) {
            if (result) {
                let searchParams = new URLSearchParams(location.search);
                searchParams.set("profile", result.profile);
                location.search = searchParams.toString();
            }
        });
}

// Shows an export dialog.
function showExportModal() {
    modalManager.showModal("Export", null, function (result) {
        if (result) {
            switch (result.format) {
                case "pdf":
                    let pdfExporter = new ChartPdfExporter();
                    pdfExporter.productName = formArgs.productName;
                    pdfExporter.export("divChart", getExportFileName("pdf"));
                    break;

                case "png":
                    let canvasElem = $("#divChart canvas:first");
                    if (canvasElem.length > 0) {
                        // convert to blob and save by FileSaver.js
                        canvasElem[0].toBlob(function (blob) {
                            saveAs(blob, getExportFileName("png"));
                        });
                    } else {
                        console.error("Canvas not found");
                    }
                    break;

                case "excel":
                    let reportUrl = "../Main/Print/PrintHistDataReport" +
                        "?startTime=" + new Date(timeRange.startTime).toISOString() +
                        "&endTime=" + new Date(timeRange.endTime).toISOString() +
                        "&archive=" + dataOptions.archiveCode +
                        "&cnlNums=" + formArgs.cnlNumsStr;
                    location = reportUrl;
                    break;

                default:
                    console.error("Export format not supported");
                    break;
            }
        }
    });
}

// Gets a file name for export.
function getExportFileName(extenstion) {
    // ChartPro_YYYY-MM-DD_HH:mm:ss.extenstion
    let now = new Date();
    return "ChartPro_" +
        now.getFullYear() + "-" +
        (now.getMonth() + 1).toString().padStart(2, "0") + "-" +
        now.getDate().toString().padStart(2, "0") + "_" +
        now.getHours().toString().padStart(2, "0") + "-" +
        now.getMinutes().toString().padStart(2, "0") + "-" +
        now.getSeconds().toString().padStart(2, "0") + "." +
        extenstion;
}

// Converts the data URL to a BLOB for saving to a file.
function dataUrlToBlob(dataUrl, callback) {
    fetch(dataUrl)
        .then(response => response.blob())
        .then(blob => callback(blob))
        .catch(error => console.error(error))
}

// Starts the chart loading process.
function startLoadingChart() {
    // update time offset and loading state
    if (formArgs.chartMode === ChartMode.FIXED) {
        // fixed mode
        if (timeRange.startTime + dataOptions.requestPeriodMs < timeRange.endTime) {
            setTimeOffset(timeRange.startTime + dataOptions.requestPeriodMs);
            setLoadState(LoadState.FAST_LOADING);
        } else if (timeRange.endTime <= formArgs.now) {
            setLoadState(LoadState.COMPLETE);
        } else {
            let lastUtc = getLastPointUtc();
            setTimeOffset(Math.max(timeRange.startTime, lastUtc));

            if (lastUtc < timeRange.endTime) {
                setLoadState(LoadState.AUTO_REFRESH);
            } else {
                setLoadState(LoadState.COMPLETE);
            }
        }
    } else {
        // rolling mode
        setTimeOffset(0);
        setLoadState(LoadState.FAST_LOADING);
    }

    scheduleRequest();
}

// Schedules the next request for chart data.
function scheduleRequest(opt_hasError) {
    if (loadState === LoadState.FAST_LOADING || loadState === LoadState.AUTO_REFRESH) {
        let refreshRate = dataOptions.refreshRate;

        if (opt_hasError) {
            refreshRate = ERROR_REFRESH_RATE;
        } else if (loadState === LoadState.FAST_LOADING) {
            refreshRate = FAST_REFRESH_RATE;
        }

        setTimeout(continueLoadingChart, refreshRate);
    }
}

// Continues the chart loading process.
function continueLoadingChart() {
    // postpone loading if the chart is zoomed in the Rolling mode
    if (formArgs.chartMode === ChartMode.ROLLING && chart.isZoomed) {
        setTimeout(continueLoadingChart, LOAD_DELAY);
        return;
    }

    // request chart data
    let requestFunction = formArgs.chartMode === ChartMode.FIXED
        ? getFixedChartData
        : getRollingChartData;

    requestFunction(function (dto) {
        let hasError;

        if (dto.ok) {
            hasError = !mergeChartData(dto.data);
        } else {
            hasError = true;
            console.error("Error loading chart data: " + dto.msg);
        }

        // show error
        if (hasError) {
            chart.setStatus(chartPhrases.loadError, true);
        }

        // schedule next request
        scheduleRequest(hasError);
    });
}

// Merges the existing and received chart data.
function mergeChartData(receivedData) {
    // validate received data
    if (receivedData.timePoints && receivedData.trendData &&
        receivedData.trendData.length !== chartData.trends.length) {

        console.error("Invalid chart data received");
        return false;
    }

    console.log("Chart data received from " + new Date(receivedData.startTime).toISOString() +
        " to " + new Date(receivedData.endTime).toISOString());

    if (formArgs.chartMode === ChartMode.ROLLING) {
        // update time range and remove outdated data
        timeRange.startTime = receivedData.endTime - formArgs.rollingPeriod;
        timeRange.endTime = receivedData.endTime;
        removeOutdatedData();
    }

    // merge data if new points are received
    let receivedPointCnt = receivedData.timePoints.length;
    let existingPointCnt = chartData.timePoints.length;
    let lastUtc;

    if (receivedPointCnt > 0) {
        // find indexes of added points
        let startIdx = 0;
        let endIdx = 0; // not included
        lastUtc = getLastPointUtc();

        for (let i = 0; i < receivedPointCnt; i++) {
            let utc = scada.chart.TimePoint.getUtc(receivedData.timePoints[i]);

            if (timeRange.startTime <= utc && lastUtc < utc) {
                startIdx = i;
                break;
            }
        }

        for (let i = receivedPointCnt - 1; i >= 0; i--) {
            let utc = scada.chart.TimePoint.getUtc(receivedData.timePoints[i]);

            if (utc <= timeRange.endTime) {
                if (lastUtc < utc) {
                    endIdx = i + 1;
                }
                break;
            }
        }

        // add points
        if (startIdx < endIdx) {
            for (let i = startIdx; i < endIdx; i++) {
                chartData.timePoints.push(receivedData.timePoints[i]);
            }

            for (let trendIdx = 0, trendCnt = chartData.trends.length; trendIdx < trendCnt; trendIdx++) {
                let srcPoints = receivedData.trendData[trendIdx];
                let destPoints = chartData.trends[trendIdx].points;

                for (let i = startIdx; i < endIdx; i++) {
                    destPoints.push(srcPoints[i]);
                }
            }
        }

        // add to hour map
        for (let hour of receivedData.hours) {
            timeRange.hourMap.set(
                scada.chart.TimePoint.getUtc(hour),
                scada.chart.TimePoint.getLocal(hour)
            );
        }
    }

    lastUtc = getLastPointUtc();

    if (formArgs.chartMode === ChartMode.FIXED) {
        // update time offset and loading state
        if (receivedData.endTime < timeRange.endTime) {
            setTimeOffset(receivedData.endTime);
            setLoadState(LoadState.FAST_LOADING);
        } else {
            if (timeOffset < lastUtc) {
                setTimeOffset(lastUtc);
            }

            if (lastUtc < timeRange.endTime && formArgs.now < timeRange.endTime) {
                setLoadState(LoadState.AUTO_REFRESH);
            } else {
                setLoadState(LoadState.COMPLETE);
            }
        }

        // draw chart
        chart.resume(existingPointCnt);
    } else {
        // set end time to last point
        if (timeRange.endTime - lastUtc <= dataOptions.gapBetweenPointsMs) {
            timeRange.endTime = lastUtc;
        }

        // update time offset and loading state
        setTimeOffset(lastUtc);
        setLoadState(LoadState.AUTO_REFRESH);

        // draw chart
        chart.resetRange();
    }

    // append rows to data table
    if (!$("#divData").hasClass("hidden")) {
        appendDataRows();
    }

    chart.setStatus(chartPhrases.generated + receivedData.genDT, false);
    return true;
}

// Removes chart points that are outside the chart time range.
function removeOutdatedData() {
    // find index of removed points
    let sliceIdx = 0;

    for (let i = 0, cnt = chartData.timePoints.length; i < cnt; i++) {
        let utc = scada.chart.TimePoint.getUtc(chartData.timePoints[i]);

        if (utc < timeRange.startTime) {
            sliceIdx = i + 1;
        } else {
            break;
        }
    }

    // remove outdated points
    if (sliceIdx > 0) {
        chartData.timePoints = chartData.timePoints.slice(sliceIdx);

        for (let trend of chartData.trends) {
            trend.points = trend.points.slice(sliceIdx);
        }
    }

    // remove from hour map
    let hoursToRemove = [];
    let minHour = timeRange.startTime - scada.chart.ChartConst.MS_PER_HOUR;

    for (let hourUtc of timeRange.hourMap.keys()) {
        if (hourUtc < minHour) {
            hoursToRemove.push(hourUtc);
        } else {
            break;
        }
    }

    for (let hourUtc of hoursToRemove) {
        timeRange.hourMap.delete(hourUtc);
    }
}

// Gets the last time point in UTC.
function getLastPointUtc() {
    return chartData.timePoints.length > 0
        ? scada.chart.TimePoint.getUtc(chartData.timePoints[chartData.timePoints.length - 1])
        : 0;
}

// Sets the time offset and logs to the console.
function setTimeOffset(value) {
    if (timeOffset !== value) {
        timeOffset = value;
        console.log("Time offset is " + new Date(value).toISOString());
    }
}

// Sets the loading state and logs to the console.
function setLoadState(value) {
    if (loadState !== value) {
        loadState = value;
        console.log("Chart loading state is " + LoadState.getName(value));
    }
}

// Gets the chart data for the period starting at the specified time offset, inclusive.
// Callback is function (dto)
// URL example: http://localhost/Api/ChartPro/GetFixedChartData?cnlNums=101-105,110&timeOffset=0&periodMs=60000&archiveBit=1
function getFixedChartData(callback) {
    let requestPeriod = timeOffset + dataOptions.requestPeriodMs < timeRange.endTime
        ? dataOptions.requestPeriodMs
        : timeRange.endTime - timeOffset;

    fetch(API_ROOT_PATH + "Api/ChartPro/GetFixedChartData?cnlNums=" + formArgs.cnlNumsStr +
        "&timeOffset=" + timeOffset + "&periodMs=" + requestPeriod + "&archiveBit=" + formArgs.archiveBit)
        .then(response => response.ok ? response.json() : Dto.fail(response.statusText))
        .then(data => callback(data))
        .catch(error => callback(Dto.fail(error.message)));
}

// Gets the tail of the chart data from the specified time offset to the current time within the period.
// Callback is function (dto)
// URL example: http://localhost/Api/ChartPro/GetRollingChartData?cnlNums=101-105,110&timeOffset=0&periodMs=60000&archiveBit=1
function getRollingChartData(callback) {
    fetch(API_ROOT_PATH + "Api/ChartPro/GetRollingChartData?cnlNums=" + formArgs.cnlNumsStr +
        "&timeOffset=" + timeOffset + "&periodMs=" + formArgs.rollingPeriod + "&archiveBit=" + formArgs.archiveBit)
        .then(response => response.ok ? response.json() : Dto.fail(response.statusText))
        .then(data => callback(data))
        .catch(error => callback(Dto.fail(error.message)));
}

$(document).ready(function () {
    // chart parameters must be defined in ChartPro.cshtml
    chart = new scada.chart.ChartPro("divChart");
    chart.controlOptions = controlOptions;
    chart.displayOptions = displayOptions;
    chart.timeRange = timeRange;
    chart.chartData = chartData;
    chart.mergePhrases(chartPhrases);
    chart.buildDom();

    updateLayout();
    setupCloseButton();
    setupHistChart();
    bindMenuEvents();

    chart.draw();
    chart.bindHintEvents();
    chart.bindScalingEvents();

    new Splitter("divChartSplitter").exitResizeModeCallbacks.add(function () {
        chart.draw();
    });

    $(window).on("resize", function () {
        updateLayout();
        chart.draw();
    });

    startLoadingChart();
});
