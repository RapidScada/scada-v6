// Depends on jquery, scada-common.js, view-hub.js, main-api.js

// Represent metadata about a data cell.
class CellMeta {
    constructor(cnlNum, showVal, joinLen, cellElem) {
        this.cnlNum = cnlNum;
        this.showVal = showVal;
        this.joinLen = joinLen;
        this.cellElem = cellElem;
    }
}

// Represent metadata about a historical data column.
class HistColMeta {
    constructor(time, colElem, hdrElem) {
        this.isVisible = true;
        this.time = time;
        this.colElem = colElem;
        this.hdrElem = hdrElem;
        this.cells = []; // array of CellMeta
    }
}

// The variables below are set in TableView.cshtml
var viewID = 0;

const START_TIME_KEY = "TableView.StartTime";
const END_TIME_KEY = "TableView.EndTime";
const DEFAULT_CELL_COLOR = "Black";
const NEXT_TIME_SYMBOL = "*";
const UPDATE_HIST_DATA_OFFSET = 500; //ms

var localDate = "";
var timeRange = null;
var serverTime = null;
var cnlListID = 0;
var arcWriteTime = 0;
var curCells = []; // array of CellMeta
var histCols = []; // array of HistColMeta

function prepare() {
    mainApi.rootPath = viewHub.appEnv.rootPath;
    localDate = $("#localDate").val();

    restoreTimeRange();
    initTimeRange(false);
    initCurCells();
    initHistCols();
    setColVisibe();
    initTooltips();
    bindEvents();
    updateLayout();
    startUpdatingCurData();
    setTimeout(startUpdatingHistData, UPDATE_HIST_DATA_OFFSET);
}

function restoreTimeRange() {
    let startTimeVal = ScadaUtils.getStorageItem(localStorage, START_TIME_KEY);
    let endTimeVal = ScadaUtils.getStorageItem(localStorage, END_TIME_KEY);

    if (startTimeVal && endTimeVal) {
        ScadaUtils.selectOptionIfExists($("#selStartTime"), startTimeVal);
        ScadaUtils.selectOptionIfExists($("#selEndTime"), endTimeVal);
    }
}

function initTimeRange(saveState) {
    let startTime = $("#selStartTime option:selected").attr("data-time");
    let endTime = $("#selEndTime option:selected").attr("data-time");
    timeRange = startTime && endTime ? new TimeRange(startTime, endTime, true) : null;
    arcWriteTime = 0;

    // save selected values
    let startTimeVal = $("#selStartTime").val();
    let endTimeVal = $("#selEndTime").val();

    if (saveState) {
        ScadaUtils.setStorageItem(localStorage, START_TIME_KEY, startTimeVal);
        ScadaUtils.setStorageItem(localStorage, END_TIME_KEY, endTimeVal);
    }

    // show or hide dates in table header
    if (!startTimeVal || startTimeVal.endsWith("-1d")) {
        $("table.table-main:first").removeClass("hide-date");
    } else {
        $("table.table-main:first").addClass("hide-date");
    }
}

function initCurCells() {
    $("table.table-main:first tr.row-item").each(function () {
        let cnlNum = parseInt($(this).attr("data-cnlnum"));
        let showVal = $(this).attr("data-showval") === "true";
        let joinLen = parseInt($(this).attr("data-joinlen"));
        let cell = $(this).children("td.cell-cur:first");
        curCells.push(new CellMeta(cnlNum, showVal, joinLen, cell));
    });
}

function initHistCols() {
    let tableElem = $("table.table-main:first");
    let colElems = tableElem.find("colgroup col.col-hist");
    let hdrElems = tableElem.find("thead tr.row-hdr:first th.hdr-hist");

    tableElem.find("tr.row-item").each(function () {
        let rowElem = $(this);
        let cnlNum = parseInt(rowElem.attr("data-cnlnum"));
        let showVal = rowElem.attr("data-showval") === "true";
        let joinLen = parseInt(rowElem.attr("data-joinlen"));

        rowElem.children("td.cell-hist").each(function (index) {
            let cellElem = $(this);
            let colMeta = histCols[index];

            if (!colMeta) {
                let colElem = colElems.eq(index);
                let hdrElem = hdrElems.eq(index);
                colMeta = new HistColMeta(colElem.attr("data-time"), colElem, hdrElem);
                histCols[index] = colMeta;
            }

            colMeta.cells.push(new CellMeta(cnlNum, showVal, joinLen, cellElem));
        });
    });
}

function setColVisibe() {
    const HiddenClass = "hid";

    if (timeRange) {
        for (let colMeta of histCols) {
            if (timeRange.startTime <= colMeta.time && colMeta.time <= timeRange.endTime) {
                // show cells
                colMeta.isVisible = true;
                colMeta.colElem.removeClass(HiddenClass);
                colMeta.hdrElem.removeClass(HiddenClass);

                for (let cellMeta of colMeta.cells) {
                    cellMeta.cellElem.removeClass(HiddenClass)
                }
            } else {
                // hide cells
                colMeta.isVisible = true;
                colMeta.colElem.addClass(HiddenClass);
                colMeta.hdrElem.addClass(HiddenClass);

                for (let cellMeta of colMeta.cells) {
                    cellMeta.cellElem
                        .addClass(HiddenClass)
                        .text("")
                        .css("color", "");
                }
            }
        }
    } else {
        // show and clear cells
        $("col.col-hist, th.hdr-hist, td.cell-hist").removeClass(HiddenClass);
        $("td.cell-hist").text("").css("color", "");
    }
}

function initTooltips() {
    $("[data-bs-toggle='tooltip']").each(function () {
        return new bootstrap.Tooltip($(this)[0]);
    });
};

function bindEvents() {
    $(window).resize(function () {
        updateLayout();
    });

    $("#localDate").change(function () {
        $("form:first").submit();
    });

    $("#selStartTime, #selEndTime").change(function () {
        // update time range
        initTimeRange(true);
        setColVisibe();
    });

    $("#spanPrintBtn").click(function () {
        developmentAlert();
    });

    $(".item-link").click(function () {
        // show chart
        let cnlNum = $(this).closest(".row-item").attr("data-cnlnum");
        let startDate = $("#localDate").val();
        viewHub.features.chart.show(cnlNum, startDate);
    });

    $(".item-cmd").click(function () {
        // show command dialog
        let cnlNum = $(this).closest(".row-item").attr("data-cnlnum");
        viewHub.features.command.show(cnlNum);
    });
}

function updateLayout() {
    let toolbarHeight = $("#divToolbar").outerHeight() || 0;
    let h = $(window).height() - toolbarHeight;
    $("#divError").outerHeight(h);
    $("#divTableWrapper").outerHeight(h);
};

function startUpdatingCurData() {
    updateCurData(function () {
        setTimeout(startUpdatingCurData, pluginOptions.refreshRate);
    });
}

function startUpdatingHistData() {
    undateHistData(function () {
        setTimeout(startUpdatingHistData, pluginOptions.refreshRate);
    });
}

function updateCurData(callback) {
    mainApi.getCurDataByView(viewID, cnlListID, function (dto) {
        if (dto.ok) {
            checkNewDate(dto.data.serverTime);
            cnlListID = dto.data.cnlListID;
        } else {
            showErrorBadge();
        }

        showCurData(dto.data);
        callback();
    });
}

function undateHistData(callback) {
    if (timeRange && archiveBit >= 0) {
        mainApi.getArcWriteTime(archiveBit, function (dto) {
            if (dto.ok) {
                let newArcWriteTime = dto.data;

                if (arcWriteTime !== newArcWriteTime) {
                    // request historical data
                    mainApi.getHistDataByView(archiveBit, timeRange, viewID, function (dto) {
                        if (dto.ok) {
                            arcWriteTime = newArcWriteTime;
                            showHistData(dto.data);
                        } else {
                            showErrorBadge();
                        }

                        callback();
                    });
                } else {
                    // data has not changed
                    callback();
                }
            } else {
                showErrorBadge();
                callback();
            }
        });
    } else {
        console.warn("Unable to request historical data");
        showErrorBadge();
        callback();
    }
}

function checkNewDate(newServerTime) {
    let localTime = serverTime ? serverTime.lt : null;
    let newLocalTime = newServerTime.lt;

    if (localTime && localDate && localTime.startsWith(localDate) &&
        newLocalTime && !newLocalTime.startsWith(localDate)) {
        // switch table view to new date
        let newLocalDate = newLocalTime.substr(0, 10); // yyyy-MM-dd
        $("#localDate").val(newLocalDate).change();    // submit form
    }

    serverTime = newServerTime;
}

function showCurData(data) {
    let map = mainApi.mapCurData(data);

    for (let cellMeta of curCells) {
        let record = MainApi.getCurData(map, cellMeta.cnlNum, cellMeta.joinLen);
        displayCell(cellMeta, record);
    }
}

function showHistData(data) {
    let map = mainApi.mapCnlNums(data.cnlNums);
    let startIdx = 0;
    let prevColMeta = null;

    for (let colMeta of histCols) {
        if (colMeta.isVisible) {
            let recordIdx = findRecordIndex(data.timestamps, colMeta.time, startIdx);
            startIdx = recordIdx >= 0 ? recordIdx + 1 : ~recordIdx;

            let isNext = prevColMeta && serverTime &&
                prevColMeta.time < serverTime.ut && serverTime.ut <= colMeta.time;

            for (let cellMeta of colMeta.cells) {
                if (recordIdx >= 0) {
                    let cnlNumIdx = map.get(cellMeta.cnlNum);
                    let record = cnlNumIdx >= 0 ? data.trends[cnlNumIdx][recordIdx] : null;
                    let subrecords = [];

                    // append string parts
                    for (let i = 1; i < cellMeta.joinLen; i++) {
                        let cnlNumIdx = map.get(cellMeta.cnlNum + i);
                        let subrecord = cnlNumIdx >= 0 ? data.trends[cnlNumIdx][recordIdx] : null;
                        subrecords.push(subrecord);
                    }

                    displayCell(cellMeta, record, subrecords);
                } else if (isNext && cellMeta.cnlNum > 0) {
                    cellMeta.cellElem.text(NEXT_TIME_SYMBOL).css(DEFAULT_CELL_COLOR);
                } else {
                    displayCell(cellMeta, null, null);
                }
            }
        }

        prevColMeta = colMeta;
    }
}

function findRecordIndex(timestamps, time, startIdx) {
    let timestampCnt = timestamps.length;

    for (let recordIdx = startIdx; recordIdx < timestampCnt; recordIdx++) {
        let ut = timestamps[recordIdx].ut;
        if (ut === time) {
            return recordIdx;
        } else if (ut > time) {
            return ~recordIdx;
        }
    }

    return ~timestampCnt;
}

function displayCell(cellMeta, record, opt_subrecords) {
    let cellElem = cellMeta.cellElem;

    if (cellMeta.showVal && record) {
        let cellText = record.df.dispVal;

        if (Array.isArray(opt_subrecords) && record.d.stat > 0) {
            for (let subrecord of opt_subrecords) {
                if (subrecord && subrecord.d.stat > 0) {
                    cellText += subrecord.df.dispVal;
                }
            }
        }

        cellElem.text(cellText);
        cellElem.css("color", getCellColor(record));
    } else {
        cellElem.text("");
        cellElem.css("color", "");
    }
}

function getCellColor(record) {
    let colors = record.df.colors;
    return Array.isArray(colors) && colors.length > 0 && colors[0]
        ? colors[0]
        : DEFAULT_CELL_COLOR;
}

$(document).ready(function () {
    if ($("#frmTableView").length > 0) {
        prepare();
    } else {
        bindEvents();
        updateLayout();
    }
});
