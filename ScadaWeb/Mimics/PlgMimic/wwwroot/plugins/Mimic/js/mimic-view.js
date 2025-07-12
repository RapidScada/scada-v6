// Handles the mimic view page.
// Depends on jquery, scada-common.js, view-hub.js, main-api.js, mimic-common.js, mimic-model.js, mimic-render.js

class MimicDataProvider extends rs.mimic.DataProvider {
    getCurData = (cnlNum, opt_joinLen) =>
        MainApi.getCurDataFromMap(this.curCnlDataMap, cnlNum, opt_joinLen);
    getPrevData = (cnlNum, opt_joinLen) =>
        MainApi.getCurDataFromMap(this.prevCnlDataMap, cnlNum, opt_joinLen);
}

const viewHub = ViewHub.getInstance();
const mainApi = new MainApi({ rootPath: viewHub.appEnv.rootPath });
const mimic = new rs.mimic.Mimic();
const unitedRenderer = new rs.mimic.UnitedRenderer(mimic, false);
const dataProvider = new MimicDataProvider();

// Set in MimicView.cshtml
var viewID = 0;
var refreshRate = 1000;
var runtimeOptions = {};
var phrases = {};

let cnlListID = 0;

function bindEvents() {
    $(window).on("resize", function () {
        updateLayout();
    });
}

function updateLayout() {
    let h = $(window).height();
    $("#divMimicWrapper").outerHeight(h);
}

async function loadMimic() {
    let result = await mimic.load(getLoaderUrl(), viewID);

    if (result.ok) {
        $("#divMimicWrapper").append(unitedRenderer.createMimicDom());
        startUpdatingData();
    } else {
        // show error
    }
}

function getLoaderUrl() {
    return viewHub.appEnv.rootPath + "Api/Mimic/";
}

function startUpdatingData() {
    updateData(function () {
        setTimeout(startUpdatingData, refreshRate);
    });
}

function updateData(callback) {
    mainApi.getCurDataByView(viewID, cnlListID, function (dto) {
        if (dto.ok) {
            cnlListID = dto.data.cnlListID;
        } else {
            // show error
        }

        dataProvider.prevCnlDataMap = dataProvider.curCnlDataMap;
        dataProvider.curCnlDataMap = MainApi.mapCurData(dto.data);
        unitedRenderer.updateData(dataProvider);
        callback();
    });
}

$(async function () {
    bindEvents();
    updateLayout();
    await loadMimic();
});
