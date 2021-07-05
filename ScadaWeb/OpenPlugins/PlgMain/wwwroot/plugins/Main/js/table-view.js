// Represent metadata about a current data cell.
class CurCellMeta {
    constructor(cnlNum, jqCell) {
        this.cnlNum = cnlNum;
        this.jqCell = jqCell;
    }
}

// The variables below are set from TableView.cshtml
var viewID = 0;
var pluginOptions = {
    refreshRate: 1000
};

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

        if (record) {
            cellMeta.jqCell.text(record.df.dispVal);
        } else {
            cellMeta.jqCell.text("");
        }
    }
}

$(document).ready(function () {
    mainApi.rootPath = viewHub.appEnv.rootPath;
    initCurCells();
    initTooltips();
    updateLayout();
    startUpdatingCurData();
});
