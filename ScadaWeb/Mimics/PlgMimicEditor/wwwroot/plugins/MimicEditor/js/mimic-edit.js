// Depends on jquery, tweakpane, scada-common.js, 
//     mimic-common.js, mimic-model.js, mimic-render.js,
//     editor.js, prop-grid.js

const UPDATE_RATE = 1000;
const mimic = new rs.mimic.Mimic();
const unitedRenderer = new rs.mimic.UnitedRenderer(mimic, true);
const updateQueue = [];

var rootPath = "/";
var mimicKey = "0";
var splitter = null;
var propGrid = null;
var mimicWrapperElem = $();
var selectedElem = $();

function bindEvents() {
    $(window).on("resize", function () {
        updateLayout();
    });

    $("#btnSave").on("click", async function () {
        await save();
    });

    // select mimic
    mimicWrapperElem.on("click", function () {
        selectedElem.removeClass("selected");
        selectedElem = $();
        propGrid.selectedObject = mimic;
        console.log("Mimic selected");
        return false;
    });

    // select component
    mimicWrapperElem.on("click", ".comp", function () {
        let compElem = $(this);
        let faceplateElem = compElem.closest(".comp.faceplate");

        // select faceplate
        if (faceplateElem.length > 0) {
            compElem = faceplateElem;
        }

        // find component by ID
        let id = compElem.data("id");
        let component = mimic.componentMap.get(id);

        // select component
        selectedElem.removeClass("selected");

        if (component) {
            selectedElem = compElem;
            selectedElem.addClass("selected");
        } else {
            selectedElem = $();
        }

        propGrid.selectedObject = component;
        console.log(`Component with ID ${id} selected`);
        return false; // prevent parent selection
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

function initTweakpane() {
    let containerElem = $("<div id='tweakpane'></div>").appendTo("#divLeftPanel");
    let pane = new Pane({
        title: "Properties",
        container: containerElem[0]
    });
    propGrid = new PropGrid(pane);
    propGrid.addEventListener("propertyChanged", function (event) {
        handlePropertyChanged(event.detail);
    });
}

async function loadMimic() {
    let result = await mimic.load(getLoaderUrl(), mimicKey);

    if (result.ok) {
        mimicWrapperElem.append(unitedRenderer.createMimicDom());
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

function handlePropertyChanged(eventData) {
    let selectedObject = eventData.selectedObject;
    let propertyName = eventData.propertyName;
    let value = eventData.value;
    console.log(propertyName + " = " + JSON.stringify(value));

    if (selectedObject instanceof rs.mimic.Component) {
        // update client side
        let component = eventData.selectedObject;
        unitedRenderer.updateComponentDom(component);

        // update server side
        let change = Change.updateComponent(component.id).setProperty(propertyName, value);
        let updateDTO = new UpdateDTO(mimicKey, change);
        updateQueue.push(updateDTO);
    }
}

async function save() {
    let response = await fetch(getUpdaterUrl() + "SaveMimic?key=" + mimicKey, { method: "POST" });

    if (response.ok) {
        let dto = await response.json();

        if (dto.ok) {
            console.log("Mimic saved successfully");
        } else {
            console.error("Error saving mimic: " + dto.msg);
        }
    }
}

function undo() {

}

function redo() {

}

function cut() {

}

function copy() {

}

function paste() {

}

function deleteComponent() {

}

$(async function () {
    splitter = new Splitter("divSplitter");
    mimicWrapperElem = $("#divMimicWrapper");

    bindEvents();
    updateLayout();
    initTweakpane();
    await loadMimic();
    await startUpdatingBackend();
});
