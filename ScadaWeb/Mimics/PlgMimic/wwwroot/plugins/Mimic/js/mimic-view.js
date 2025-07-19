// Handles the mimic view page.
// Depends on jquery, scada-common.js, view-hub.js, main-api.js, mimic-common.js, mimic-model.js, mimic-render.js

class MimicDataProvider extends rs.mimic.DataProvider {
    getCurData = (cnlNum, opt_joinLen) =>
        MainApi.getCurDataFromMap(this.curDataMap, cnlNum, opt_joinLen);
    getPrevData = (cnlNum, opt_joinLen) =>
        MainApi.getCurDataFromMap(this.prevDataMap, cnlNum, opt_joinLen);
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
let scale = new rs.mimic.Scale();

function bindEvents() {
    $(window).on("resize", function () {
        updateLayout();

        if (scale.type !== rs.mimic.ScaleType.NUMERIC) {
            updateScale();
        }
    });

    $(document).on("keydown", function (event) {
        let targetElem = $(event.target);

        if ((targetElem.is("body") || targetElem.closest("#divToolbar").length > 0) &&
            handleKeyDown(event.code, event.ctrlKey)) {
            event.preventDefault();
        }
    });

    $("#btnFitScreen").on("click", function () {
        scale = new rs.mimic.Scale(rs.mimic.ScaleType.FIT_SCREEN, 0);
        updateScale();
    });

    $("#btnFitWidthBtn").on("click", function () {
        scale = new rs.mimic.Scale(rs.mimic.ScaleType.FIT_WIDTH, 0);
        updateScale();
    });

    $("#btnZoomOutBtn").on("click", function () {
        scale = scale.getPrev();
        updateScale();
    });

    $("#btnZoomInBtn").on("click", function () {
        scale = scale.getNext();
        updateScale();
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
        initScale();
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

        dataProvider.prevDataMap = dataProvider.curDataMap;
        dataProvider.curDataMap = MainApi.mapCurData(dto.data);
        unitedRenderer.updateData(dataProvider);
        callback();
    });
}

function initScale() {
    if (runtimeOptions.rememberScale) {
        scale.load(localStorage);
    } else {
        scale.type = runtimeOptions.scaleType;
        scale.value = runtimeOptions.scaleValue;
    }

    updateScale(false);
}

function updateScale(saveScale = true) {
    mimic.renderer?.setScale(mimic, scale);
    $("#spanScale").text(Math.round(scale.value * 100) + "%");

    if (saveScale) {
        scale.save(localStorage);
    }
}

function handleKeyDown(code, ctrlKey) {
    if (ctrlKey) {
        switch (code) {
            case "Minus":
                scale = scale.getPrev();
                updateScale();
                return true;

            case "Equal":
                scale = scale.getNext();
                updateScale();
                return true;

            case "Digit0":
                scale = new rs.mimic.Scale();
                updateScale();
                return true;
        }
    }

    return false;
}

$(async function () {
    bindEvents();
    updateLayout();
    await loadMimic();
});
