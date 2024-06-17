// Depends on jquery, mimic-common.js, mimic-model.js, mimic-render.js

const mimic = new rs.mimic.Mimic();
const rendererSet = new rs.mimic.RendererSet();
const renderContext = new rs.mimic.RenderContext();

var rootPath = "/";
var mimicKey = "0";
var splitter = null;

function bindEvents() {
    $(window).on("resize", function () {
        updateLayout();
    });
}

function updateLayout() {
    let windowHeight = $(window).height();
    let toolbarHeight = $("#divToolbar").outerHeight();

    let windowWidth = $(window).width();
    let leftPanelWidth = $("#divLeftPanel").width();
    let splitterWidth = $("#divSplitter").width();

    $("#divMain").outerHeight(windowHeight - toolbarHeight);
    $("#divMimicWrapper").outerWidth(windowWidth - leftPanelWidth - splitterWidth);
}

async function loadMimic() {
    let result = await mimic.load(getLoaderUrl(), mimicKey);

    if (result.ok) {

    } else {
        // show error
    }
}

function getLoaderUrl() {
    return rootPath + "Api/MimicEditor/Loader/";
}

$(async function () {
    renderContext.editMode = true;
    splitter = new Splitter("divSplitter");

    bindEvents();
    updateLayout();
    loadMimic();
});
