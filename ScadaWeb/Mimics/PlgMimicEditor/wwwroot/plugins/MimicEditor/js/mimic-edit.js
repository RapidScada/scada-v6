// Depends on jquery, mimic-common.js, mimic-model.js, mimic-render.js

const mimic = new rs.mimic.Mimic();
const rendererSet = new rs.mimic.RendererSet();
const renderContext = new rs.mimic.RenderContext();

var rootPath = "/";
var mimicKey = "0";
var splitter = null;
var mimicWrapperElem = $();

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
        renderContext.imageMap = mimic.imageMap;
        createDom();
    } else {
        // show error
    }
}

function createDom() {
    let startTime = Date.now();
    rendererSet.mimicRenderer.createDom(mimic, renderContext);

    for (let component of mimic.components) {
        let componentRenderer = rendererSet.componentRenderers.get(component.typeName);

        if (componentRenderer) {
            component.renderer = componentRenderer;
            componentRenderer.createDom(component, renderContext);

            if (component.dom && component.parent?.dom) {
                component.parent.dom.append(component.dom);
            }
        } else {
            console.warn("Renderer not found for component with ID=" + component.id +
                " of type '" + component.typeName + "'");
        }
    }

    if (mimic.dom) {
        mimicWrapperElem.append(mimic.dom);
        console.info(ScadaUtils.getCurrentTime() + " Mimic DOM created in " + (Date.now() - startTime) + " ms");
    }
}

function getLoaderUrl() {
    return rootPath + "Api/MimicEditor/Loader/";
}

$(async function () {
    renderContext.editMode = true;
    splitter = new Splitter("divSplitter");
    mimicWrapperElem = $("#divMimicWrapper");

    bindEvents();
    updateLayout();
    loadMimic();
});
