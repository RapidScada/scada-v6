// Depends on jquery, scada-common.js, view-hub.js, main-api.js

// Represent metadata about a current data cell.
class CurCellMeta {
    constructor(cnlNum, jqCell) {
        this.cnlNum = cnlNum;
        this.jqCell = jqCell;
    }
}

// The variables below are set from TableView.cshtml
var viewID = 0;
var archiveBit = -1;
var pluginOptions = {
    refreshRate: 1000
};

const DEFAULT_CELL_COLOR = "Black";
var viewHub = ViewHub.getInstance();
var mainApi = new MainApi();
var curCells = []; // array of CurCellMeta

function initCurCells() {
    $("table.table-main:first tr.row-item").each(function () {
        let cnlNum = parseInt($(this).attr("data-cnlNum"));
        let cell = $(this).children("td.cell-cur:first");
        curCells.push(new CurCellMeta(cnlNum, cell));
    });
}

function initTooltips() {
    $("[data-bs-toggle='tooltip']").each(function () {
        return new bootstrap.Tooltip($(this)[0]);
    });
};

function bindEvents() {
    let thisObj = this;

    $(window).resize(function () {
        thisObj.updateLayout();
    });

    $("#localDate").change(function () {
        $("form:first").submit();
    });
}

function updateLayout() {
    let h = $(window).height() - $("#divToolbar").outerHeight();
    $("#divTableWrapper").outerHeight(h);
    $("#divError").outerHeight(h);
};

function startUpdatingCurData() {
    updateCurData(function () {
        setTimeout(startUpdatingCurData, pluginOptions.refreshRate);
    });
}

function updateCurData(callback) {
    mainApi.getCurDataByView(viewID, 0, function (dto) {
        showCurrentData(dto.data);
        callback();
    });
}

function showCurrentData(data) {
    let map = mainApi.mapCurData(data);

    for (let cellMeta of curCells) {
        let record = map.get(cellMeta.cnlNum);
        let cell = cellMeta.jqCell;

        if (record) {
            cell.text(record.df.dispVal);
            cell.css("color", getCellColor(record));
        } else {
            cell.text("");
            cell.css("color", "");
        }
    }
}

function getCellColor(record) {
    let colors = record.df.colors;
    return Array.isArray(colors) && colors.length > 0 && colors[0]
        ? colors[0]
        : DEFAULT_CELL_COLOR;
}

$(document).ready(function () {
    mainApi.rootPath = viewHub.appEnv.rootPath;
    initCurCells();
    initTooltips();
    bindEvents();
    updateLayout();
    startUpdatingCurData();
});
