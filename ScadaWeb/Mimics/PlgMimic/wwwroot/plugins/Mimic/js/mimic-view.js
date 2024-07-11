// Depends on jquery, scada-common.js, view-hub.js, main-api.js, mimic-common.js, mimic-model.js, mimic-render.js

const viewHub = ViewHub.getInstance();
const mainApi = new MainApi({ rootPath: viewHub.appEnv.rootPath });
const mimic = new rs.mimic.Mimic();
const rendererSet = new rs.mimic.RendererSet();

var viewID = 0;
var mimicWrapperElem = $();

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
        //createMimicDom();
    } else {
        // show error
    }
}

function getLoaderUrl() {
    return viewHub.appEnv.rootPath + "Api/Mimic/";
}

$(async function () {
    mimicWrapperElem = $("#divMimicWrapper");
    bindEvents();
    updateLayout();
    await loadMimic();
});
