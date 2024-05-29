// Depends on jquery, mimic-common.js, mimic-model.js, mimic-render.js

const mimic = new rs.mimic.Mimic();
const rendererSet = new rs.mimic.RendererSet();
const renderContext = new rs.mimic.RenderContext();

var editorKey = "0";

function updateLayout() {
    let toolbarHeight = $("#divToolbar").height();
    let windowHeight = $(window).height();
    let mainHeight = windowHeight - toolbarHeight;
    $("#divMain").outerHeight(mainHeight);
    $("#divLeftPanel").outerHeight(mainHeight);
    $("#divSplitter").outerHeight(mainHeight);
    $("#divMimicWrapper").outerHeight(mainHeight);
}

$(function () {
    renderContext.editMode = true;
    updateLayout();

    $(window).on("resize", function () {
        updateLayout();
    });
});
