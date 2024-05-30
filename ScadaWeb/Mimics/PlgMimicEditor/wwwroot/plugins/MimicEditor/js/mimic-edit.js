// Depends on jquery, mimic-common.js, mimic-model.js, mimic-render.js

const mimic = new rs.mimic.Mimic();
const rendererSet = new rs.mimic.RendererSet();
const renderContext = new rs.mimic.RenderContext();

var editorKey = "0";
var splitter = null;

function bindEvents() {
    $(window).on("resize", function () {
        updateLayout();
    });
}

function updateLayout() {
    let windowHeight = $(window).height();
    let toolbarHeight = $("#divToolbar").outerHeight();
    let mainHeight = windowHeight - toolbarHeight;

    let windowWidth = $(window).width();
    let leftPanelWidth = $("#divLeftPanel").width();
    let splitterWidth = $("#divSplitter").width();

    $("#divMain").outerHeight(mainHeight);
    //$("#divLeftPanel").outerHeight(mainHeight);
    //$("#divSplitter").outerHeight(mainHeight);
    $("#divMimicWrapper")
        //.outerHeight(mainHeight)
        .outerWidth(windowWidth - leftPanelWidth - splitterWidth);
}

$(function () {
    renderContext.editMode = true;
    splitter = new Splitter("divSplitter");

    bindEvents();
    updateLayout();
});
