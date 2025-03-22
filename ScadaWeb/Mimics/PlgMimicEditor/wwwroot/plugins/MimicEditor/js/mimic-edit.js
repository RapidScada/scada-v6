// Depends on jquery, tweakpane, scada-common.js, 
//     mimic-common.js, mimic-model.js, mimic-render.js,
//     editor.js, prop-grid.js

const UPDATE_RATE = 1000;
const mimic = new rs.mimic.Mimic();
const unitedRenderer = new rs.mimic.UnitedRenderer(mimic, true);
const updateQueue = [];

var rootPath = "/";
var mimicKey = "0";
var phrases = {};
var translation = {};

let splitter = null;
let propGrid = null;
let mimicWrapperElem = $();
let selectedElem = $();

function bindEvents() {
    $(window).on("resize", function () {
        updateLayout();
    });

    $("#btnSave").on("click", async function () {
        await save();
    });

    $("#btnUndo").on("click", async function () {
        showToast("Test toast");
    });

    mimicWrapperElem.on("click", function () {
        selectMimic();
        return false;
    });

    mimicWrapperElem.on("click", ".comp", function () {
        selectComponent($(this));
        return false; // prevent parent selection
    });
}

function updateLayout() {
    let windowHeight = $(window).height();
    let toolbarHeight = $("#divToolbar").outerHeight();
    let tabHeight = $("#divLeftPanel .nav-tabs").outerHeight();
    let mainHeight = windowHeight - toolbarHeight;

    let windowWidth = $(window).width();
    let leftPanelWidth = $("#divLeftPanel").width();
    let splitterWidth = $("#divSplitter").width();

    $("#divMain").outerHeight(mainHeight);
    $("#divLeftPanel .tab-content").outerHeight(mainHeight - tabHeight);
    $("#divMimicWrapper").outerWidth(windowWidth - leftPanelWidth - splitterWidth);
}

function initTweakpane() {
    let containerElem = $("<div id='tweakpane'></div>").appendTo("#divProperties");
    let pane = new Pane({
        container: containerElem[0]
    });
    propGrid = new PropGrid(pane);
    propGrid.addEventListener("propertyChanged", function (event) {
        handlePropertyChanged(event.detail);
    });
}

function translateProperties() {
    const DescriptorSet = rs.mimic.DescriptorSet;

    // translate mimic
    translateObject(DescriptorSet.mimicDescriptor, translation.mimic);

    // translate components
    for (let [typeName, componentDescriptor] of DescriptorSet.componentDescriptors) {
        translateObject(componentDescriptor, translation.components.get(typeName));
    }

    // translate object
    function translateObject(objectDescriptor, dictionary) {
        if (!dictionary) {
            return;
        }

        for (let propertyDescriptor of objectDescriptor.propertyDescriptors.values()) {
            let displayName = dictionary[propertyDescriptor.name] ?? translation.component[propertyDescriptor.name];
            let category = translation.category[propertyDescriptor.category];

            if (displayName) {
                propertyDescriptor.displayName = displayName;
            }

            if (category) {
                propertyDescriptor.category = category;
            }
        }
    }
}

async function loadMimic() {
    let result = await mimic.load(getLoaderUrl(), mimicKey);

    if (result.ok) {
        mimicWrapperElem.append(unitedRenderer.createMimicDom());
        showMimicStructure();
        selectMimic();
    } else {
        selectNone();
        console.error(dto.msg);
        showToast(phrases.mimicLoadError, MessageType.ERROR);
    }
}

async function startUpdatingBackend() {
    await postUpdates();
    setTimeout(startUpdatingBackend, UPDATE_RATE);
}

async function save() {
    let response = await fetch(getUpdaterUrl() + "SaveMimic?key=" + mimicKey, { method: "POST" });

    if (response.ok) {
        let dto = await response.json();

        if (dto.ok) {
            console.log(phrases.mimicSaved);
            showToast(phrases.mimicSaved, MessageType.SUCCESS);
        } else {
            console.error(dto.msg);
            showToast(phrases.mimicSaveError, MessageType.ERROR);
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

function enablePointer() {

}

function selectMimic() {
    selectedElem.removeClass("selected");
    selectedElem = $();
    propGrid.selectedObject = mimic;
    console.log("Mimic selected");
}

function selectComponent(compElem) {
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
}

function selectNone() {
    propGrid.selectedObject = null;
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

function showMimicStructure() {
    let listElem = $("<ul class='top-level-list'></ul>");

    // dependencies
    let dependenciesItem = $("<li></li>").text(phrases.dependenciesNode).appendTo(listElem);
    let dependenciesList = $("<ul></ul>").appendTo(dependenciesItem);

    for (let dependency of mimic.dependencies) {
        let dependencyNode = $("<span></span>").text(dependency.typeName);
        $("<li></li>").append(dependencyNode).appendTo(dependenciesList);
    }

    // components
    let mimicNode = $("<span></span>").text(phrases.mimicNode);
    let mimicItem = $("<li></li>").append(mimicNode).appendTo(listElem);
    let componentList = $("<ul></ul>").appendTo(mimicItem);

    function appendComponent(list, component) {
        let componentNode = $("<span></span>").text(component.displayName);
        let componentItem = $("<li></li>").append(componentNode).appendTo(list);

        if (component.isContainer && component.children.length > 0) {
            let childList = $("<ul></ul>").appendTo(componentItem);

            for (let childComponent of component.children) {
                appendComponent(childList, childComponent);
            }
        }
    }

    for (let component of mimic.children) {
        appendComponent(componentList, component);
    }

    // images
    let imagesItem = $("<li></li>").text(phrases.imagesNode).appendTo(listElem);
    let imagesList = $("<ul></ul>").appendTo(imagesItem);

    for (let image of mimic.images) {
        let imageNode = $("<span></span>").text(image.name);
        $("<li></li>").append(imageNode).appendTo(imagesList);
    }

    $("#divStructure").append(listElem);
}

function showToast(message, opt_messageType) {
    // construct toast
    let toastElem = $("<div class='toast align-items-center'></div>");

    if (opt_messageType) {
        switch (opt_messageType) {
            case MessageType.INFO:
                toastElem.addClass("text-bg-info");
                break;
            case MessageType.SUCCESS:
                toastElem.addClass("text-bg-success");
                break;
            case MessageType.WARNING:
                toastElem.addClass("text-bg-warning");
                break;
            case MessageType.ERROR:
                toastElem.addClass("text-bg-danger");
                break;
        }
    }

    let wrapperElem = $("<div class='d-flex'></div>").appendTo(toastElem);
    $("<div class='toast-body'></div>").text(message).appendTo(wrapperElem);
    $("<button type='button' class='btn-close me-2 m-auto' data-bs-dismiss='toast'></button>").appendTo(wrapperElem);
    $("#divToastContainer").prepend(toastElem);

    // show toast
    let toast = bootstrap.Toast.getOrCreateInstance(toastElem[0]);
    toast.show();

    // delete hidden toast
    toastElem.on("hidden.bs.toast", function () {
        toastElem.remove();
    });
}

$(async function () {
    splitter = new Splitter("divSplitter");
    mimicWrapperElem = $("#divMimicWrapper");

    bindEvents();
    updateLayout();
    initTweakpane();
    translateProperties();
    await loadMimic();
    await startUpdatingBackend();
});
