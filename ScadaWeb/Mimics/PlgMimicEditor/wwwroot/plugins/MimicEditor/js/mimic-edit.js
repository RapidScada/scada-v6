// Depends on jquery, mimic-common.js, mimic-model.js, mimic-render.js

const mimic = new rs.mimic.Mimic();
const rendererSet = new rs.mimic.RendererSet();

var rootPath = "/";
var mimicKey = "0";
var splitter = null;
var mimicWrapperElem = $();

function bindEvents() {
    $(window).on("resize", function () {
        updateLayout();
    });

    $("#btnCut").on("click", function () {
        testEdit();
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
        createMimicDom();
    } else {
        // show error
    }
}

function getLoaderUrl() {
    return rootPath + "Api/MimicEditor/Loader/";
}

function createRenderContext() {
    let renderContext = new rs.mimic.RenderContext();
    renderContext.editMode = true;
    return renderContext;
}

function createMimicDom() {
    let startTime = Date.now();
    let unknownTypes = new Set();

    let renderContext = createRenderContext();
    renderContext.imageMap = mimic.imageMap;
    rendererSet.mimicRenderer.createDom(mimic, renderContext);

    for (let component of mimic.components) {
        if (component.isFaceplate) {
            createFaceplateDom(component, unknownTypes);
        } else {
            let renderer = rendererSet.componentRenderers.get(component.typeName);

            if (renderer) {
                component.renderer = renderer;
                renderer.createDom(component, renderContext);
            } else {
                unknownTypes.add(component.typeName);
            }
        }

        if (component.dom && component.parent?.dom) {
            component.parent.dom.append(component.dom);
        }
    }

    if (unknownTypes.size > 0) {
        console.warn("Unknown component types: " + Array.from(unknownTypes).sort().join(", "));
    }

    if (mimic.dom) {
        mimicWrapperElem.append(mimic.dom);
        console.info(ScadaUtils.getCurrentTime() + " Mimic DOM created in " + (Date.now() - startTime) + " ms");
    }
}

function createFaceplateDom(faceplateInstance, unknownTypes) {
    if (!faceplateInstance.model) {
        unknownTypes.add(faceplateInstance.typeName);
        return;
    }

    let renderContext = createRenderContext();
    renderContext.imageMap = faceplateInstance.model.imageMap;

    faceplateInstance.renderer = rendererSet.faceplateRenderer;
    rendererSet.faceplateRenderer.createDom(faceplateInstance, renderContext);
    renderContext.idPrefix = faceplateInstance.id + "-";

    for (let component of faceplateInstance.components) {
        let renderer = rendererSet.componentRenderers.get(component.typeName);

        if (renderer) {
            component.renderer = renderer;
            renderer.createDom(component, renderContext);

            if (component.dom && component.parent?.dom) {
                component.parent.dom.append(component.dom);
            }
        } else {
            unknownTypes.add(component.typeName);
        }
    }
}

function updateComponentDom(component) {
    if (component.dom && component.renderer) {
        let renderContext = createRenderContext();

        if (component.isFaceplate) {
            renderContext.imageMap = component.model.imageMap;
            component.renderer.updateDom(component, renderContext);
        } else {
            renderContext.imageMap = mimic.imageMap;

            if (component.renderer.canUpdateDom) {
                component.renderer.updateDom(component, renderContext);
            } else {
                let oldDom = component.dom;
                component.renderer.createDom(component, renderContext);
                oldDom.replaceWith(component.dom);
            }
        }
    }
}

function testEdit() {
    let component = mimic.componentMap.get(1);

    if (component) {
        component.properties.text = "Hello";
        updateComponentDom(component);
    }
}

$(async function () {
    splitter = new Splitter("divSplitter");
    mimicWrapperElem = $("#divMimicWrapper");

    bindEvents();
    updateLayout();
    loadMimic();
});
