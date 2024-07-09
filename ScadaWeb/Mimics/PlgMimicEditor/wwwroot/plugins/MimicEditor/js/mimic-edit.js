// Depends on jquery, scada-common.js, mimic-common.js, mimic-model.js, mimic-render.js

const UPDATE_RATE = 1000;
const mimic = new rs.mimic.Mimic();
const rendererSet = new rs.mimic.RendererSet();
const updateQueue = [];

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

async function startUpdatingBackend() {
    await postUpdates();
    setTimeout(startUpdatingBackend, UPDATE_RATE);
}

function getLoaderUrl() {
    return rootPath + "Api/MimicEditor/Loader/";
}

function getUpdaterUrl() {
    return rootPath + "Api/MimicEditor/Updater/";
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

async function postUpdates() {
    while (updateQueue.length > 0) {
        let updateDTO = updateQueue.shift();
        let result = await postUpdate(updateDTO);

        if (!result) {
            updateQueue.unshift(updateDTO);
            break;
        }
    }
}

async function postUpdate(updateDTO) {
    try {
        let response = await fetch(getUpdaterUrl() + "UpdateMimic", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(updateDTO)
        });

        if (response.ok) {
            let dto = await response.json();

            if (!dto.ok) {
                console.error("Error processing update: " + dto.msg);
            }
        }

        return true;
    } catch {
        return false;
    }
}

function testEdit() {
    let component = mimic.componentMap.get(1);

    if (component) {
        // update client side
        const textValue = "Hello";
        component.properties.text = textValue;
        updateComponentDom(component);

        // update server side
        let change = Change.updateComponent(component.id, { text: textValue });
        let updateDTO = new UpdateDTO(mimicKey, change);
        updateQueue.push(updateDTO);
    }
}

$(async function () {
    splitter = new Splitter("divSplitter");
    mimicWrapperElem = $("#divMimicWrapper");

    bindEvents();
    updateLayout();
    await loadMimic();
    await startUpdatingBackend();
});
