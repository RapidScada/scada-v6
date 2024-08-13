// Depends on jquery, scada-common.js, view-hub.js, main-api.js, mimic-common.js, mimic-model.js, mimic-render.js

const viewHub = ViewHub.getInstance();
const mainApi = new MainApi({ rootPath: viewHub.appEnv.rootPath });
const mimic = new rs.mimic.Mimic();
const unitedRenderer = new rs.mimic.UnitedRenderer(mimic, false);

var viewID = 0;
var mimicWrapperElem = $();

function bindEvents() {
    $(window).on("resize", function () {
        updateLayout();
        alignHorizontally();
    });
}

function updateLayout() {
    let h = $(window).height();
    $("#divMimicWrapper").outerHeight(h);
}

function alignHorizontally() {
    let wrapperPadding = 0;

    if (mimic.dom && mimic.dom.length > 0) {
        let actualMimicWidth = mimic.dom[0].getBoundingClientRect().width;
        let wrapperWidth = mimicWrapperElem.innerWidth();

        if (wrapperWidth > actualMimicWidth) {
            wrapperPadding = parseInt((wrapperWidth - actualMimicWidth) / 2);
        }
    }

    mimicWrapperElem.css("padding-left", wrapperPadding);
};

async function loadMimic() {
    let result = await mimic.load(getLoaderUrl(), viewID);

    if (result.ok) {
        mimicWrapperElem.append(unitedRenderer.createMimicDom());
        alignHorizontally();
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
