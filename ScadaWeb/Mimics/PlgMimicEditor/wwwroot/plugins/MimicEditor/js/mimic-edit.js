// Depends on jquery, tweakpane, scada-common.js,
//     mimic-common.js, mimic-model.js, mimic-render.js,
//     editor.js, mimic-descr.js, mimic-factory.js, prop-grid.js, struct-tree.js

const UPDATE_RATE = 1000; // ms
const KEEP_ALIVE_INTERVAL = 10000; // ms
const TOAST_MESSAGE_LENGTH = 100;
const IMAGES_PATH = "../../plugins/MimicEditor/images/";
const mimic = new rs.mimic.Mimic();
const unitedRenderer = new rs.mimic.UnitedRenderer(mimic, true);
const updateQueue = [];
const toastMessages = new Set();

// Set in MimicEdit.cshtml and MimicEditLang.cshtml
var rootPath = "/";
var mimicKey = "0";
var phrases = {};
var translation = {};

let splitter = null;
let propGrid = null;
let structTree = null;
let mimicWrapperElem = $();
let selectedElem = $();
let lastUpdateTime = 0;
let longAction = null;
let mimicModified = false;
let maxComponentId = null;

function bindEvents() {
    $(window)
        .on("resize", function () {
            updateLayout();
        })
        .on("beforeunload", function (event) {
            if (mimicModified) {
                event.preventDefault();
            }
        });

    $("#btnSave").on("click", async function () {
        await save();
    });

    $("#btnUndo").on("click", function () {
        undo();
    });

    $("#btnRedo").on("click", function () {
        redo();
    });

    $("#btnCut").on("click", function () {
        cut();
    });

    $("#btnCopy").on("click", function () {
        copy();
    });

    $("#btnPaste").on("click", function () {
        paste();
    });

    $("#btnRemove").on("click", function () {
        remove();
    });

    $("#btnPointer").on("click", function () {
        pointer();
    });

    $("#divComponents").on("click", ".component-item", function () {
        let typeName = $(this).data("type-name");
        longAction = LongAction.startAdding(typeName);
        mimicWrapperElem.css("cursor", longAction.getCursor());
    });

    mimicWrapperElem
        .on("mousedown", function (event) {
            if (longAction == null) {
                selectMimic();
            } else if (longAction.actionType = LongActionType.ADDING) {
                addComponent(longAction.componentTypeName, 0, getMimicPoint(event, mimicWrapperElem));
                clearLongAction();
            }
        })
        .on("mousedown", ".comp", function (event) {
            let compElem = $(this);

            if (longAction == null) {
                selectComponent(compElem);
                event.stopPropagation();
            } else if (longAction.actionType = LongActionType.ADDING) {
                if (compElem.hasClass("container")) {
                    addComponent(longAction.componentTypeName, compElem.data("id"), getMimicPoint(event, compElem));
                    clearLongAction();
                    event.stopPropagation();
                }
            }
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

    // translate mimic and faceplates
    translateObject(DescriptorSet.mimicDescriptor, translation.mimic);
    translateObject(DescriptorSet.faceplateDescriptor, translation.component);

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
        rs.mimic.DescriptorSet.mimicDescriptor.repair(mimic);
        mimicWrapperElem.append(unitedRenderer.createMimicDom());
        showFaceplates();
        showStructure();
        selectMimic();
    } else {
        selectNone();
        console.error(dto.msg);
        showToast(phrases.loadMimicError, MessageType.ERROR);
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
            mimicModified = false;
            console.log(phrases.mimicSaved);
            showToast(phrases.mimicSaved, MessageType.SUCCESS);
        } else {
            console.error(dto.msg);
            showToast(phrases.saveMimicError, MessageType.ERROR);
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

function remove() {
    let componentID = selectedElem.data("id");

    if (componentID > 0) {
        console.log(`Remove component with ID ${componentID}`);

        // remove component from mimic and DOM
        let component = mimic.removeComponent(componentID);

        if (component && component.dom) {
            component.dom.remove();
        }

        // update structure and properties
        structTree.removeComponent(componentID);
        selectMimic();

        // update server side
        pushChanges(Change.removeComponent(componentID));
    } else {
        console.log("Component is not selected.");
    }
}

function pointer() {
    clearLongAction();
}

function showFaceplates() {
    // create new faceplate group
    let newGroupElem;

    if (mimic.dependencies.length > 0) {
        newGroupElem = $("<div class='component-group faceplate-group'></div>");
        $("<div class='component-group-header'></div>")
            .text(phrases.faceplateGroup)
            .appendTo(newGroupElem);

        for (let faceplateMeta of mimic.dependencies) {
            if (!faceplateMeta.isTransitive) {
                let faceplateElem = $("<div class='component-item'></div>")
                    .attr("data-type-name", faceplateMeta.typeName)
                    .appendTo(newGroupElem);
                $("<img class='component-icon' />")
                    .attr("src", IMAGES_PATH + "faceplate-icon.png")
                    .appendTo(faceplateElem);
                $("<span class='component-name'></span>")
                    .text(faceplateMeta.typeName)
                    .appendTo(faceplateElem);
            }
        }
    } else {
        newGroupElem = $();
    }

    // replace existing faceplate group
    let oldGroupElem = $("#divComponents .faceplate-group");

    if (oldGroupElem.length > 0) {
        oldGroupElem.replaceWith(newGroupElem);
    } else {
        $("#divComponents").append(newGroupElem);
    }
}

function showStructure() {
    structTree ??= new StructTree("divStructure", mimic, phrases);
    structTree.prepare();
}

function selectMimic() {
    selectedElem.removeClass("selected");
    selectedElem = $();
    propGrid.selectedObject = mimic;
    console.log("Mimic selected");
}

function selectComponent(compElem) {
    if (!compElem) {
        selectNone();
        return;
    }

    let faceplateElem = compElem.parents(".comp.faceplate").last();

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
    selectedElem.removeClass("selected");
    selectedElem = $();
    propGrid.selectedObject = null;
}

function getMimicPoint(event, elem) {
    let offset = elem.offset();
    return new DOMPoint(
        parseInt(event.pageX - offset.left),
        parseInt(event.pageY - offset.top)
    );
}

function addComponent(typeName, parentID, point) {
    console.log(`Add ${typeName} component at ${point.x}, ${point.y}` +
        (parentID > 0 ? ` inside component ${parentID}` : ""));

    let factory;
    let descriptor;
    let renderer;
    let parent = parentID > 0 ? mimic.componentMap.get(parentID) : mimic;
    let faceplate = mimic.faceplateMap.get(typeName);

    if (faceplate) {
        factory = rs.mimic.FactorySet.getFaceplateFactory(faceplate);
        descriptor = rs.mimic.DescriptorSet.faceplateDescriptor;
        renderer = rs.mimic.RendererSet.faceplateRenderer;
    } else {
        factory = rs.mimic.FactorySet.componentFactories.get(typeName);
        descriptor = rs.mimic.DescriptorSet.componentDescriptors.get(typeName);
        renderer = rs.mimic.RendererSet.componentRenderers.get(typeName);
    }

    if (factory && descriptor && renderer && parent) {
        // create and render component
        let component = factory.createComponent();
        component.id = getNextComponentId();
        descriptor.repair(component);
        mimic.addComponent(component, parent, point.x, point.y);
        unitedRenderer.createComponentDom(component);

        // update structure and properties
        structTree.addComponent(component);
        selectComponent(component.dom);

        // update server side
        pushChanges(Change.addComponent(component));
    } else {
        if (!factory) {
            console.error("Component factory not found.");
        }

        if (!descriptor) {
            console.error("Component descriptor not found.");
        }

        if (!renderer) {
            console.error("Component renderer not found.");
        }

        if (!parent) {
            console.error("Component parent not found.");
        }

        showToast(phrases.unableAddComponent, MessageType.ERROR);
    }
}

function getNextComponentId() {
    maxComponentId ??= mimic.componentMap.size > 0 ? Math.max(...mimic.componentMap.keys()) : 0;
    return ++maxComponentId;
}

function clearLongAction() {
    if (longAction) {
        if (longAction.actionType === LongActionType.ADDING) {
            mimicWrapperElem.css("cursor", "");
        }

        longAction = null;
    }
}

function getLoaderUrl() {
    return rootPath + "Api/MimicEditor/Loader/";
}

function getUpdaterUrl() {
    return rootPath + "Api/MimicEditor/Updater/";
}

function pushChanges(...changes) {
    let updateDto = new UpdateDto(mimicKey, ...changes);
    updateQueue.push(updateDto);
}

async function postUpdates() {
    if (updateQueue.length > 0) {
        // send changes
        while (updateQueue.length > 0) {
            let updateDto = updateQueue.shift();
            let result = await postUpdate(updateDto);

            if (result) {
                mimicModified = true;
            } else {
                updateQueue.unshift(updateDto);
                break;
            }
        }
    } else if (Date.now() - lastUpdateTime >= KEEP_ALIVE_INTERVAL) {
        // heartbeat
        await postUpdate(new UpdateDto(mimicKey));
    }
}

async function postUpdate(updateDto) {
    try {
        let response = await fetch(getUpdaterUrl() + "UpdateMimic", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(updateDto)
        });

        if (response.ok) {
            let dto = await response.json();

            if (!dto.ok) {
                console.error(dto.msg);
                showPermanentToast(shortenMessage(dto.msg), MessageType.ERROR);
            }
        } else {
            showPermanentToast(phrases.postUpdateError, MessageType.ERROR);
        }

        lastUpdateTime = Date.now();
        return true;
    } catch {
        showPermanentToast(phrases.postUpdateError, MessageType.ERROR);
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

        // update selected element
        selectedElem = component.dom.addClass("selected");

        // update structure tree
        structTree.updateComponent(component);

        // update server side
        let change = Change.updateComponent(component.id).setProperty(propertyName, value);
        pushChanges(change);
    }
}

function shortenMessage(message) {
    return message && message.length > TOAST_MESSAGE_LENGTH
        ? message.substring(0, TOAST_MESSAGE_LENGTH) + "..."
        : message;
}

function showToast(message, opt_messageType, opt_toastOptions) {
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

    let contentsElem = $("<div class='d-flex'></div>").appendTo(toastElem);
    $("<div class='toast-body'></div>").text(message).appendTo(contentsElem);
    $("<button type='button' class='btn-close me-2 m-auto' data-bs-dismiss='toast'></button>").appendTo(contentsElem);
    $("#divToastContainer").prepend(toastElem);

    // show toast
    let toast = bootstrap.Toast.getOrCreateInstance(toastElem[0], opt_toastOptions);
    toast.show();

    // delete hidden toast
    toastElem.on("hidden.bs.toast", function () {
        if (toastElem.data("permanent")) {
            toastMessages.delete(message);
        }

        toastElem.remove();
    });

    return toastElem;
}

function showPermanentToast(message, opt_messageType) {
    if (!toastMessages.has(message)) {
        toastMessages.add(message);
        showToast(message, opt_messageType, { autohide: false }).data("permanent", true);
    }
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
