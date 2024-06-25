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
        createMimicDom();
    } else {
        // show error
    }
}

function getLoaderUrl() {
    return rootPath + "Api/MimicEditor/Loader/";
}

function createMimicDom() {
    let startTime = Date.now();
    rendererSet.mimicRenderer.createDom(mimic, renderContext);

    for (let component of mimic.components) {
        let renderer = rendererSet.componentRenderers.get(component.typeName);

        if (renderer) {
            component.renderer = renderer;
            renderer.createDom(component, renderContext);
        } else if (component.isFaceplate) {
            let faceplate = mimic.faceplateMap.get(component.typeName);

            if (faceplate) {
                component.renderer = rendererSet.faceplateRenderer;
                component.model = faceplate;
                createFaceplateDom(component);
            } else {
                console.warn("Faceplate not found for component with ID=" + component.id +
                    " of type '" + component.typeName + "'");
            }
        } else {
            console.warn("Renderer not found for component with ID=" + component.id +
                " of type '" + component.typeName + "'");
        }

        if (component.dom && component.parent?.dom) {
            component.parent.dom.append(component.dom);
        }
    }

    if (mimic.dom) {
        mimicWrapperElem.append(mimic.dom);
        console.info(ScadaUtils.getCurrentTime() + " Mimic DOM created in " + (Date.now() - startTime) + " ms");
    }
}

function createFaceplateDom(faceplateInstance) {
    rendererSet.faceplateRenderer.createDom(faceplateInstance, renderContext);

}

$(async function () {
    renderContext.editMode = true;
    splitter = new Splitter("divSplitter");
    mimicWrapperElem = $("#divMimicWrapper");

    bindEvents();
    updateLayout();
    loadMimic();
});
