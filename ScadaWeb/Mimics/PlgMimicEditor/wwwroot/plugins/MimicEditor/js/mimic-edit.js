// Handles the main page of the mimic editor.
// Depends on jquery, tweakpane, scada-common.js,
//     mimic-common.js, mimic-factory.js, mimic-model.js, mimic-render.js,
//     editor-common.js, mimic-descr.js, modals.js, prop-grid.js, struct-tree.js

const UPDATE_RATE = 1000; // ms
const KEEP_ALIVE_INTERVAL = 10000; // ms
const RESIZE_BORDER_WIDTH = 5;
const MIN_SIZE = RESIZE_BORDER_WIDTH * 3;
const MIN_MOVE = 10;
const TOAST_MESSAGE_LENGTH = 100;
const IMAGES_PATH = "../../plugins/MimicEditor/images/";
const mimic = new rs.mimic.Mimic();
const unitedRenderer = new rs.mimic.UnitedRenderer(mimic, true);
const updateQueue = [];
const toastMessages = new Set();
const mimicHistory = new MimicHistory();
const mimicClipboard = new MimicClipboard();

// Set in MimicEdit.cshtml and MimicEditLang.cshtml
var rootPath = "/";
var mimicKey = "0";
var fonts = [];
var editorOptions = {};
var translation = {};

let phrases = null;
let splitter = null;
let propGrid = null;
let structTree = null;
let faceplateModal = null;
let imageModal = null;
let mimicElem = $();
let selectedComponents = [];
let lastUpdateTime = 0;
let longAction = null;
let mimicModified = false;
let mimicReloadRequired = false;
let maxComponentID = null;

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

    $(document).on("keydown", function (event) {
        let targetElem = $(event.target);

        if ((targetElem.is("body") || targetElem.closest("#divToolbar").length > 0 ||
            event.code === "KeyS" && event.ctrlKey) &&
            handleKeyDown(event.code, event.ctrlKey, event.shiftKey)) {
            event.preventDefault();
        }
    });

    $(ToolbarButton.SAVE).on("click", async function () {
        await save();
    });

    $(ToolbarButton.UNDO).on("click", function () {
        undo();
    });

    $(ToolbarButton.REDO).on("click", function () {
        redo();
    });

    $(ToolbarButton.CUT).on("click", function () {
        cut();
    });

    $(ToolbarButton.COPY).on("click", function () {
        copy();
    });

    $(ToolbarButton.PASTE).on("click", function () {
        paste();
    });

    $(ToolbarButton.REMOVE).on("click", function () {
        remove();
    });

    $(ToolbarButton.POINTER).on("click", function () {
        pointer();
    });

    $(ToolbarButton.ALIGN).on("click", function () {
        align($(this).attr("data-action"));
    });

    $(ToolbarButton.ARRANGE).on("click", function () {
        arrange($(this).attr("data-action"));
    });

    $("#divComponents").on("click", ".component-item", function () {
        let typeName = $(this).data("type-name");
        startLongAction(LongAction.add(typeName));
    });

    $("#divMimicWrapper")
        .on("mousedown", function (event) {
            if (!longAction) {
                selectMimic();
            } else if (LongActionType.isPointing(longAction.actionType)) {
                finishPointing(0, getMimicPoint(event, mimicElem, true))
                clearLongAction();
            }
        })
        .on("mousedown", ".comp", function (event) {
            let thisElem = $(this);

            if (!longAction) {
                // select or deselect component, start dragging
                let compElem = closestCompElem(thisElem);
                let component = getComponentByDom(compElem);

                if (component) {
                    if (event.ctrlKey) {
                        if (component.isSelected) {
                            removeFromSelection(component);
                        } else {
                            addToSelection(component);
                        }
                    } else {
                        if (component.isSelected) {
                            startLongAction(LongAction.drag(
                                getDragType(event, compElem),
                                getMimicPoint(event, mimicElem)));
                        } else {
                            selectComponent(component);
                            startLongAction(LongAction.drag(DragType.MOVE, getMimicPoint(event, mimicElem)));
                        }
                    }
                }

                event.stopPropagation();
            } else if (LongActionType.isPointing(longAction.actionType)) {
                // add or paste component to container, arrange components
                let componentID = thisElem.data("id");

                if (thisElem.hasClass("container")) {
                    finishPointing(componentID, getMimicPoint(event, thisElem, true));
                    clearLongAction();
                    event.stopPropagation();
                } else if (longAction.actionType === LongActionType.ARRANGE) {
                    arrangeComponents(longAction.arrangeType, componentID);
                    clearLongAction();
                    event.stopPropagation();
                }
            }
        })
        .on("mousemove", function (event) {
            // continue dragging
            if (longAction?.actionType === LongActionType.DRAG) {
                continueDragging(getMimicPoint(event, mimicElem));
            }
        })
        .on("mousemove", ".comp", function (event) {
            // set cursor
            let compElem = closestCompElem($(this));

            if (longAction) {
                compElem.css("cursor", "");
            } else {
                let dragType = getDragType(event, compElem);
                compElem.css("cursor", DragType.getCursor(dragType));
            }
        })
        .on("mouseup mouseleave", function () {
            // finish dragging
            if (longAction?.actionType === LongActionType.DRAG) {
                finishDragging();
                clearLongAction();
            }
        })
        .on("selectstart", false)
        .on("dragstart", false);
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

function initStructTree() {
    structTree = new StructTree("divStructure", mimic, translation.structTree);

    // dependencies
    structTree.addEventListener(StructTreeEventType.ADD_DEPENDENCY_CLICK, function () {
        faceplateModal.show(null, function (context) {
            addDependency(context.newValue);
        });
    });

    structTree.addEventListener(StructTreeEventType.EDIT_DEPENDENCY_CLICK, function (event) {
        let eventData = event.detail;
        let faceplateMeta = mimic.dependencyMap.get(eventData.name);

        if (faceplateMeta) {
            faceplateModal.show(faceplateMeta, function (context) {
                addDependency(context.newValue, context.oldValue);
            });
        } else {
            console.error("Dependency not found.");
        }
    });

    structTree.addEventListener(StructTreeEventType.REMOVE_DEPENDENCY_CLICK, function (event) {
        if (confirm(phrases.confirmDeleteDependency)) {
            removeDependency(event.detail.name);
        }
    });

    // images
    structTree.addEventListener(StructTreeEventType.ADD_IMAGE_CLICK, function () {
        imageModal.show(null, function (context) {
            addImage(context.newValue);
        });
    });

    structTree.addEventListener(StructTreeEventType.EDIT_IMAGE_CLICK, function (event) {
        let image = mimic.imageMap.get(event.detail.name);

        if (image) {
            imageModal.show(image, function (context) {
                addImage(context.newValue, context.oldValue);
            });
        } else {
            console.error("Image not found.");
        }
    });

    structTree.addEventListener(StructTreeEventType.REMOVE_IMAGE_CLICK, function (event) {
        if (confirm(phrases.confirmDeleteImage)) {
            removeImage(event.detail.name);
        }
    });

    // mimic
    structTree.addEventListener(StructTreeEventType.MIMIC_CLICK, function () {
        selectMimic();
    });

    // components
    structTree.addEventListener(StructTreeEventType.COMPONENT_CLICK, function (event) {
        let eventData = event.detail;
        let component = mimic.componentMap.get(eventData.componentID);

        if (component) {
            if (eventData.ctrlKey) {
                if (eventData.isSelected) {
                    removeFromSelection(component);
                } else {
                    addToSelection(component);
                }
            } else if (!eventData.isSelected) {
                selectComponent(component);
            }
        } else {
            console.error("Component not found.");
        }
    });
}

function initPropGrid() {
    propGrid = new PropGrid("tweakpane", translation.propGrid);
    PropGridHelper.translateDescriptors(translation.model);

    propGrid.addEventListener(PropGridEventType.ERROR, function (event) {
        showToast(event.detail.message, MessageType.ERROR);
    });

    propGrid.addEventListener(PropGridEventType.PROPERTY_CHANGED, function (event) {
        handlePropertyChanged(event.detail);
    });
}

function initModals() {
    faceplateModal = new FaceplateModal("divFaceplateModal");
    imageModal = new ImageModal("divImageModal");
    PropGridDialogs.textEditor = new TextEditor("divTextEditor");
}

async function loadMimic() {
    showSpinner();
    let result = await mimic.load(getLoaderUrl(), mimicKey);
    hideSpinner();

    if (result.ok) {
        setButtonsEnabled();
        showFaceplates();
        showStructure();
        showMimic();
        selectMimic();

        if (result.warn) {
            showToast(phrases.loadMimicError, MessageType.WARNING);
        }
    } else {
        selectNone();
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
    restoreHistoryPoint(mimicHistory.getUndoPoint());
    setButtonsEnabled(EnabledDependsOn.HISTORY);
}

function redo() {
    restoreHistoryPoint(mimicHistory.getRedoPoint());
    setButtonsEnabled(EnabledDependsOn.HISTORY);
}

function cut() {
    if (copy()) {
        remove();
    }
}

async function copy() {
    if (selectedComponents.length === 0) {
        return false;
    }

    if (!rs.mimic.MimicHelper.areSiblings(selectedComponents)) {
        console.error(phrases.sameParentRequired);
        showToast(phrases.sameParentRequired, MessageType.ERROR);
        return false;
    }

    let componentIDs = selectedComponents.map(c => c.id);
    console.log("Copy components with IDs " + componentIDs.join(", "));

    await mimicClipboard.writeComponents(selectedComponents);
    setEnabled(ToolbarButton.PASTE, true);
    return true;
}

function paste() {
    if (!mimicClipboard.isEmpty) {
        startLongAction(LongAction.paste());
    }
}

function remove() {
    if (selectedComponents.length === 0) {
        return;
    }

    let componentIDs = selectedComponents.map(c => c.id);
    console.log("Remove components with IDs " + componentIDs.join(", "));

    for (let componentID of componentIDs) {
        // remove component from model and DOM
        let component = mimic.removeComponent(componentID);
        component.renderer?.remove(component);

        // update structure tree
        structTree.removeComponent(componentID);
    }

    // update selection and properties
    selectMimic();

    // update server side
    pushChanges(Change.removeComponent(componentIDs));
}

function pointer() {
    clearLongAction();
}

function align(actionType) {
    if (selectedComponents.length < 2) {
        return;
    }

    if (AlingActionType.sameParentRequired(actionType) && !rs.mimic.MimicHelper.areSiblings(selectedComponents)) {
        console.error(phrases.sameParentRequired);
        showToast(phrases.sameParentRequired, MessageType.ERROR);
        return;
    }

    // update model
    console.log("Align components");
    let firstComponent = selectedComponents[0];
    let updatedComponents = [];
    let changes = [];

    switch (actionType) {
        case AlingActionType.ALIGN_LEFTS: {
            let x = firstComponent.x;
            updatedComponents = selectedComponents.slice(1);

            for (let component of updatedComponents) {
                component.x = x;
                changes.push(Change.updateLocation(component));
            }

            break;
        }
        case AlingActionType.ALIGN_CENTERS: {
            let center = firstComponent.x + firstComponent.width / 2;
            updatedComponents = selectedComponents.slice(1);

            for (let component of updatedComponents) {
                component.x += Math.trunc(center - (component.x + component.width / 2));
                changes.push(Change.updateLocation(component));
            }

            break;
        }
        case AlingActionType.ALIGN_RIGHTS: {
            let right = firstComponent.x + firstComponent.width;
            updatedComponents = selectedComponents.slice(1);

            for (let component of updatedComponents) {
                component.x = right - component.width;
                changes.push(Change.updateLocation(component));
            }

            break;
        }
        case AlingActionType.ALIGN_TOPS: {
            let y = firstComponent.y;
            updatedComponents = selectedComponents.slice(1);

            for (let component of updatedComponents) {
                component.y = y;
                changes.push(Change.updateLocation(component));
            }

            break;
        }
        case AlingActionType.ALIGN_MIDDLES: {
            let middle = firstComponent.y + firstComponent.height / 2;
            updatedComponents = selectedComponents.slice(1);

            for (let component of updatedComponents) {
                component.y += Math.trunc(middle - (component.y + component.height / 2));
                changes.push(Change.updateLocation(component));
            }

            break;
        }
        case AlingActionType.ALIGN_BOTTOMS: {
            let bottom = firstComponent.y + firstComponent.height;
            updatedComponents = selectedComponents.slice(1);

            for (let component of updatedComponents) {
                component.y = bottom - component.height;
                changes.push(Change.updateLocation(component));
            }

            break;
        }
        case AlingActionType.SAME_WIDTH: {
            let width = firstComponent.width;
            updatedComponents = selectedComponents.slice(1);

            for (let component of updatedComponents) {
                component.width = width;
                changes.push(Change.updateSize(component));
            }

            break;
        }
        case AlingActionType.SAME_HEIGHT: {
            let height = firstComponent.height;
            updatedComponents = selectedComponents.slice(1);

            for (let component of updatedComponents) {
                component.height = height;
                changes.push(Change.updateSize(component));
            }

            break;
        }
        case AlingActionType.SAME_SIZE: {
            let width = firstComponent.width;
            let height = firstComponent.height;
            updatedComponents = selectedComponents.slice(1);

            for (let component of updatedComponents) {
                component.setSize(width, height);
                changes.push(Change.updateSize(component));
            }

            break;
        }
        case AlingActionType.HOR_SPACING: {
            let lastComponent = selectedComponents.at(-1);
            let spacing = lastComponent.x + lastComponent.width - firstComponent.x;
            selectedComponents.forEach(c => spacing -= c.width);
            spacing = Math.round(spacing / (selectedComponents.length - 1));
            let x = firstComponent.x + firstComponent.width + spacing;
            updatedComponents = selectedComponents.slice(1, -1);

            for (let component of updatedComponents) {
                component.x = x;
                changes.push(Change.updateLocation(component));
                x += component.width + spacing;
            }

            break;
        }
        case AlingActionType.VERT_SPACING: {
            let lastComponent = selectedComponents.at(-1);
            let spacing = lastComponent.y + lastComponent.height - firstComponent.y;
            selectedComponents.forEach(c => spacing -= c.height);
            spacing = Math.round(spacing / (selectedComponents.length - 1));
            let y = firstComponent.y + firstComponent.height + spacing;
            updatedComponents = selectedComponents.slice(1, -1);

            for (let component of updatedComponents) {
                component.y = y;
                changes.push(Change.updateLocation(component));
                y += component.height + spacing;
            }

            break;
        }
    }

    if (updatedComponents.length > 0) {
        // update DOM
        updatedComponents.forEach(c => unitedRenderer.updateComponentDom(c));

        // refresh properties
        propGrid.refresh();

        // update server side
        pushChanges(...changes);
    }
}

function arrange(actionType) {
    const MimicHelper = rs.mimic.MimicHelper;
    const Renderer = rs.mimic.Renderer;

    if (selectedComponents.length === 0) {
        return;
    }

    if (!MimicHelper.areSiblings(selectedComponents)) {
        console.error(phrases.sameParentRequired);
        showToast(phrases.sameParentRequired, MessageType.ERROR);
        return;
    }

    if (ArrangeActionType.longActionRequired(actionType)) {
        startLongAction(LongAction.arrange(actionType));
        return;
    }

    console.log("Arrange components");
    let parent = selectedComponents[0].parent;
    let getComponentIDs = () => selectedComponents.map(c => c.id);

    switch (actionType) {
        case ArrangeActionType.BRING_TO_FRONT:
            MimicHelper.bringToFront(parent, selectedComponents);
            Renderer.arrangeChildren(parent);
            structTree.refreshComponents(parent);
            pushChanges(Change.arrangeComponent(parent.id, getComponentIDs(), Change.MAX_SHIFT));
            break;

        case ArrangeActionType.BRING_FORWARD:
            MimicHelper.bringForward(parent, selectedComponents);
            Renderer.arrangeChildren(parent);
            structTree.refreshComponents(parent);
            pushChanges(Change.arrangeComponent(parent.id, getComponentIDs(), 1));
            break;

        case ArrangeActionType.SEND_BACKWARD:
            MimicHelper.sendBackward(parent, selectedComponents);
            Renderer.arrangeChildren(parent);
            structTree.refreshComponents(parent);
            pushChanges(Change.arrangeComponent(parent.id, getComponentIDs(), -1));
            break;

        case ArrangeActionType.SEND_TO_BACK:
            MimicHelper.sendToBack(parent, selectedComponents);
            Renderer.arrangeChildren(parent);
            structTree.refreshComponents(parent);
            pushChanges(Change.arrangeComponent(parent.id, getComponentIDs(), -Change.MAX_SHIFT));
            break;
    }
}

function showSpinner() {
    $("#divMimicWrapper").append("<div class='mimic-spinner fs-2 text-secondary'>" +
        "<i class='fa-solid fa-spinner fa-spin-pulse fa-3x'></i></div>");
}

function hideSpinner() {
    $("#divMimicWrapper .mimic-spinner").remove();
}

function setButtonsEnabled(opt_dependsOn) {
    let dependsOn = opt_dependsOn ?? EnabledDependsOn.NOT_SPECIFIED;

    if (dependsOn === EnabledDependsOn.NOT_SPECIFIED || dependsOn === EnabledDependsOn.SELECTION) {
        let oneSelected = selectedComponents.length >= 1;
        let twoSelected = selectedComponents.length >= 2;
        setEnabled(ToolbarButton.CUT, oneSelected);
        setEnabled(ToolbarButton.COPY, oneSelected);
        setEnabled(ToolbarButton.REMOVE, oneSelected);
        setEnabled(ToolbarButton.ALIGN, twoSelected);
        setEnabled(ToolbarButton.ARRANGE, oneSelected);
    }

    if (dependsOn === EnabledDependsOn.NOT_SPECIFIED || dependsOn === EnabledDependsOn.HISTORY) {
        setEnabled(ToolbarButton.UNDO, mimicHistory.canUndo);
        setEnabled(ToolbarButton.REDO, mimicHistory.canRedo);
    }

    if (dependsOn === EnabledDependsOn.NOT_SPECIFIED) {
        setEnabled(ToolbarButton.PASTE, !mimicClipboard.isEmpty);
        setEnabled(ToolbarButton.POINTER, LongActionType.isPointing(longAction?.actionType));
    }
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
            if (!faceplateMeta.isTransitive && !faceplateMeta.hasError) {
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
    structTree.build();
}

function showMimic() {
    mimicElem = unitedRenderer.createMimicDom();
    $("#divMimicWrapper").empty().append(mimicElem);
}

function addDependency(faceplateMeta, opt_oldfaceplateMeta) {
    console.log(`Add '${faceplateMeta.typeName}' dependency`);
    let changes = [];
    let oldTypeName = opt_oldfaceplateMeta?.typeName;

    if (oldTypeName && oldTypeName !== faceplateMeta.typeName) {
        mimic.removeDependency(oldTypeName);
        changes.push(Change.removeDependency(oldTypeName));
    }

    mimic.addDependency(faceplateMeta);
    structTree.refreshDependencies();
    changes.push(Change.addDependency(faceplateMeta));
    mimicReloadRequired = true;
    pushChanges(...changes);
}

function removeDependency(typeName) {
    console.log(`Remove '${typeName}' dependency`);
    mimic.removeDependency(typeName);
    structTree.refreshDependencies();
    mimicReloadRequired = true;
    pushChanges(Change.removeDependency(typeName));
}

function addImage(image, opt_oldImage) {
    console.log(`Add '${image.name}' image`);
    let changes = [];
    let oldImageName = opt_oldImage?.name;

    if (oldImageName && oldImageName !== image.name) {
        mimic.removeImage(oldImageName);
        changes.push(Change.removeImage(oldImageName));
    }

    mimic.addImage(image);
    structTree.refreshImages();
    showMimic();
    changes.push(Change.addImage(image));
    pushChanges(...changes);
}

function removeImage(imageName) {
    console.log(`Remove '${imageName}' image`);
    mimic.removeImage(imageName);
    structTree.refreshImages();
    showMimic();
    pushChanges(Change.removeImage(imageName));
}

function addToHistory(changes) {
    mimicHistory.addPoint(mimic, changes);
    setButtonsEnabled(EnabledDependsOn.HISTORY);
}

function clearHistory() {
    mimicHistory.clear();
    setButtonsEnabled(EnabledDependsOn.HISTORY);
}

function restoreHistoryPoint(historyPoint) {
    if (!historyPoint) {
        return;
    }

    console.log("Restore history point");
    const MimicFactory = rs.mimic.MimicFactory;
    const MimicHelper = rs.mimic.MimicHelper;
    const Renderer = rs.mimic.Renderer;

    let changes = [];
    let componentsToArrange = [];
    let componentIDs = [];
    let componentIndexes = [];
    let hasError = false;

    for (let historyChange of historyPoint.changes) {
        switch (historyChange.changeType) {
            case ChangeType.UPDATE_DOCUMENT: {
                let documentSource = historyChange.getNewObject();

                if (documentSource) {
                    Object.assign(mimic.document, MimicFactory.parseProperties(documentSource));
                    unitedRenderer.updateMimicDom();
                    changes.push(Change.updateDocument(mimic.document));
                } else {
                    hasError = true;
                }

                break;
            }
            case ChangeType.ADD_COMPONENT: {
                let componentSource = historyChange.getNewObject();
                let component = componentSource ? mimic.createComponent(componentSource) : null;
                let parent = component ? mimic.getComponentParent(component.parentID) : null;

                if (mimic.addComponent(component, parent)) {
                    componentsToArrange.push(component);
                    componentIDs.push(component.id);
                    componentIndexes.push(component.index);

                    unitedRenderer.createComponentDom(component);
                    structTree.addComponent(component);
                    changes.push(Change.addComponent(component));
                } else {
                    hasError = true;
                }

                break;
            }
            case ChangeType.UPDATE_COMPONENT: {
                let component = mimic.componentMap.get(historyChange.objectID);
                let componentSource = historyChange.getNewObject();
                let factory = mimic.getComponentFactory(componentSource?.typeName);

                if (component && componentSource && factory) {
                    Object.assign(component.properties, factory.parseProperties(componentSource.properties));
                    unitedRenderer.updateComponentDom(component);
                    structTree.updateComponent(component);
                    changes.push(Change.updateComponent(component.id, component.properties));
                } else {
                    hasError = true;
                }

                break;
            }
            case ChangeType.REMOVE_COMPONENT: {
                let componentID = historyChange.objectID;
                let component = mimic.removeComponent(componentID);

                if (component) {
                    if (component.isSelected) {
                        removeFromSelection(component);
                    }

                    component.renderer?.remove(component);
                    structTree.removeComponent(componentID);
                    changes.push(Change.removeComponent(componentID));
                } else {
                    hasError = true;
                }

                break;
            }
            case ChangeType.UPDATE_PARENT: {
                let component = mimic.componentMap.get(historyChange.objectID);
                let componentSource = historyChange.getNewObject();
    
                if (component && componentSource) {
                    let parent = mimic.getComponentParent(componentSource.parentID);
    
                    if (mimic.updateParent(component, parent)) {
                        componentsToArrange.push(component);
                        componentIDs.push(component.id);
                        componentIndexes.push(componentSource.index);

                        component.properties.location = componentSource.properties.location;
                        component.renderer?.detach(component);
                        component.renderer?.updateLocation(component);
                        parent.renderer?.appendChild(parent, component);

                        structTree.removeComponent(component.id);
                        changes.push(Change.updateParent(component));
                    } else {
                        hasError = true;
                    }
                } else {
                    hasError = true;
                }

                break;
            }
            case ChangeType.ARRANGE_COMPONENT: {
                let component = mimic.componentMap.get(historyChange.objectID);
                let newIndex = historyChange.newIndex;
    
                if (component && Number.isInteger(newIndex) && newIndex >= 0) {
                    componentsToArrange.push(component);
                    componentIDs.push(component.id);
                    componentIndexes.push(newIndex);
                } else {
                    hasError = true;
                }

                break;
            }
        }
    }

    // arrange components
    if (componentsToArrange.length > 0 && MimicHelper.areSiblings(componentsToArrange)) {
        let parent = componentsToArrange[0].parent;
        MimicHelper.arrange(parent, componentsToArrange, componentIndexes);
        Renderer.arrangeChildren(parent);
        structTree.refreshComponents(parent);
        changes.push(Change.arrangeByIndexes(parent.id, componentIDs, componentIndexes));
    }

    propGrid.refresh(true);
    pushChangesNoHistory(...changes);

    if (hasError) {
        console.error(phrases.unableRestoreHistory);
        showToast(phrases.unableRestoreHistory, MessageType.ERROR);
    }
}

function selectMimic() {
    clearSelection();
    structTree.selectMimic();
    propGrid.selectedObject = mimic;
    mimicHistory.rememberDocument(mimic, false);
    console.log("Mimic selected");
}

function selectNone() {
    clearSelection();
    structTree.selectNone();
    propGrid.selectedObject = null;
}

function clearSelection() {
    for (let component of selectedComponents) {
        component.isSelected = false;
        component.renderer?.updateSelected(component);
    }

    selectedComponents = [];
    setButtonsEnabled(EnabledDependsOn.SELECTION);
}

function selectComponent(component) {
    clearSelection();
    structTree.selectNone();
    addToSelection(component);
}

function addToSelection(component) {
    component.isSelected = true;
    component.renderer?.updateSelected(component);
    selectedComponents.push(component)
    setButtonsEnabled(EnabledDependsOn.SELECTION);
    structTree.addToSelection(component);
    propGrid.selectedObjects = selectedComponents;
    mimicHistory.rememberComponent(component, false);
    console.log(`Component with ID ${component.id} selected`);
}

function removeFromSelection(component) {
    component.isSelected = false;
    component.renderer?.updateSelected(component);
    let index = selectedComponents.indexOf(component);

    if (index >= 0) {
        selectedComponents.splice(index, 1);
    }

    setButtonsEnabled(EnabledDependsOn.SELECTION);
    structTree.removeFromSelection(component);
    propGrid.selectedObjects = selectedComponents;
    console.log(`Component with ID ${component.id} removed from selection`);
}

function selectComponents(components) {
    clearSelection();

    for (let component of components) {
        component.isSelected = true;
        component.renderer?.updateSelected(component);
    }

    selectedComponents = components;
    setButtonsEnabled(EnabledDependsOn.SELECTION);
    structTree.selectComponents(selectedComponents);
    propGrid.selectedObjects = selectedComponents;

    let componentIDs = selectedComponents.map(c => c.id);
    console.log(`Components with IDs ${componentIDs.join(", ")} selected`);
}

function closestCompElem(clickedElem) {
    let faceplateElem = clickedElem.parents(".comp.faceplate").last();
    return faceplateElem.length > 0 ? faceplateElem : clickedElem.closest(".comp");
}

function getComponentByDom(compElem) {
    if (compElem instanceof rs.mimic.Component) {
        return compElem;
    } else if (compElem instanceof jQuery) {
        let id = compElem.data("id");
        return mimic.componentMap.get(id);
    } else {
        return null;
    }
}

function getMimicPoint(event, elem, opt_alignToGrid) {
    let offset = elem.offset();
    let x = parseInt(event.pageX - offset.left);
    let y = parseInt(event.pageY - offset.top);

    if (opt_alignToGrid) {
        let gridStep = getGridStep();

        if (gridStep > 1) {
            x = alignValue(x, gridStep);
            y = alignValue(y, gridStep);
        }
    }

    return new DOMPoint(x, y);
}

function getDragType(event, compElem) {
    if (selectedComponents.length === 1) {
        let component = getComponentByDom(compElem);

        if (component?.renderer?.allowResizing(component)) {
            let compW = compElem.outerWidth();
            let compH = compElem.outerHeight();

            if (compW >= MIN_SIZE && compH >= MIN_SIZE) {
                // check if cursor is over component border
                let point = getMimicPoint(event, compElem);
                let onLeft = point.x < RESIZE_BORDER_WIDTH;
                let onRight = point.x >= compW - RESIZE_BORDER_WIDTH;
                let onTop = point.y < RESIZE_BORDER_WIDTH;
                let atBot = point.y >= compH - RESIZE_BORDER_WIDTH;

                if (onTop && onLeft) {
                    return DragType.RESIZE_TOP_LEFT;
                } else if (onTop && onRight) {
                    return DragType.RESIZE_TOP_RIGHT;
                } else if (atBot && onLeft) {
                    return DragType.RESIZE_BOT_LEFT;
                } else if (atBot && onRight) {
                    return DragType.RESIZE_BOT_RIGHT;
                } else if (onLeft) {
                    return DragType.RESIZE_LEFT;
                } else if (onRight) {
                    return DragType.RESIZE_RIGHT;
                } else if (onTop) {
                    return DragType.RESIZE_TOP;
                } else if (atBot) {
                    return DragType.RESIZE_BOT;
                }
            }
        }
    }

    return DragType.MOVE;
}

function getGridStep() {
    return editorOptions && editorOptions.useGrid && editorOptions.gridStep > 1
        ? editorOptions.gridStep
        : 1;
}

function alignValue(value, gridStep) {
    return Math.trunc(Math.round(value / gridStep) * gridStep);
}

function startLongAction(action) {
    if (action) {
        longAction = action;
        mimicElem.css("cursor", longAction.getCursor());
        setEnabled(ToolbarButton.POINTER, LongActionType.isPointing(longAction.actionType));
    }
}

function finishPointing(componentID, point) {
    if (!longAction) {
        return;
    } else if (longAction.actionType === LongActionType.ADD) {
        addComponent(longAction.componentTypeName, componentID, point);
    } else if (longAction.actionType === LongActionType.PASTE) {
        pasteComponents(componentID, point);
    } else if (longAction.actionType === LongActionType.ARRANGE) {
        arrangeComponents(longAction.arrangeType, componentID, point);
    }
}

function clearLongAction() {
    if (longAction) {
        longAction = null;
        mimicElem.css("cursor", "");
        setEnabled(ToolbarButton.POINTER, false);
    }
}

function addComponent(typeName, parentID, point) {
    console.log(`Add ${typeName} component at ${point.x}, ${point.y}` +
        (parentID > 0 ? ` inside component ${parentID}` : ""));

    let factory = mimic.getComponentFactory(typeName);
    let renderer = mimic.isFaceplate(typeName)
        ? rs.mimic.RendererSet.faceplateRenderer
        : rs.mimic.RendererSet.componentRenderers.get(typeName);

    if (factory && renderer) {
        // create and render component
        let component = factory.createComponent();
        let parent = mimic.getComponentParent(parentID);
        component.id = getNextComponentID();

        if (mimic.addComponent(component, parent, null, point.x, point.y)) {
            unitedRenderer.createComponentDom(component);
            structTree.addComponent(component);
            selectComponent(component);
            pushChanges(Change.addComponent(component));
        } else {
            console.error(phrases.unableAddComponent);
            showToast(phrases.unableAddComponent, MessageType.ERROR);
        }
    } else {
        if (!factory) {
            console.error("Component factory not found.");
        }

        if (!renderer) {
            console.error("Component renderer not found.");
        }

        showToast(phrases.unableAddComponent, MessageType.ERROR);
    }
}

async function pasteComponents(parentID, point) {
    console.log(`Paste components at ${point.x}, ${point.y}` +
        (parentID > 0 ? ` inside component ${parentID}` : ""));

    let parent = mimic.getComponentParent(parentID);
    let sourceComponents = await mimicClipboard.readComponents();

    if (!parent) {
        console.error("Parent not found.");
        return;
    }

    if (sourceComponents.length === 0) {
        console.error(phrases.clipboardEmpty);
        showToast(phrases.clipboardEmpty, MessageType.ERROR);
        return;
    }

    // separate top-level and child components
    let topComponents = [];
    let childComponents = [];

    for (let sourceComponent of sourceComponents) {
        let componentCopy = mimic.createComponent(sourceComponent);

        if (componentCopy) {
            if (componentCopy.parentID === mimicClipboard.rootID) {
                componentCopy.x -= mimicClipboard.offset.x;
                componentCopy.y -= mimicClipboard.offset.y;
                topComponents.push(componentCopy);
            } else {
                childComponents.push(componentCopy);
            }
        } else {
            console.error("Unable to create component of type " + sourceComponent.typeName);
        }
    }

    // add top-level components
    let idMap = new Map(); // key is old ID, value is new ID
    let componentsToSelect = [];
    let changes = [];

    for (let component of topComponents) {
        let newID = getNextComponentID();
        idMap.set(component.id, newID);
        component.id = newID;
        mimic.addComponent(component, parent, null, point.x + component.x, point.y + component.y);
        unitedRenderer.createComponentDom(component);

        structTree.addComponent(component);
        componentsToSelect.push(component);
        changes.push(Change.addComponent(component));
    }

    // add child components
    for (let component of childComponents) {
        let newParentID = idMap.get(component.parentID);
        parent = mimic.componentMap.get(newParentID);

        if (parent) {
            let newID = getNextComponentID();
            idMap.set(component.id, newID);
            component.id = newID;
            mimic.addComponent(component, parent, null, component.x, component.y);
            unitedRenderer.createComponentDom(component);

            structTree.addComponent(component);
            changes.push(Change.addComponent(component));
        }
    }

    // update selection
    if (componentsToSelect.length > 0) {
        selectComponents(componentsToSelect);
    } else {
        selectNone();
    }

    // update server side
    pushChanges(...changes);
}

function arrangeComponents(arrangeType, componentID, opt_point) {
    const MimicHelper = rs.mimic.MimicHelper;
    const Renderer = rs.mimic.Renderer;

    if (!MimicHelper.areSiblings(selectedComponents)) {
        return;
    }

    console.log("Arrange components");
    let errorMessage = "";

    if (arrangeType === ArrangeActionType.PLACE_BEFORE || arrangeType === ArrangeActionType.PLACE_AFTER) {
        let parent = selectedComponents[0].parent;
        let siblingID = componentID;
        let sibling = siblingID > 0 ? mimic.componentMap.get(siblingID) : null;

        if (sibling) {
            if (sibling.parent === parent) {
                let selectedIDs = selectedComponents.map(c => c.id);

                if (arrangeType === ArrangeActionType.PLACE_BEFORE) {
                    MimicHelper.placeBefore(parent, selectedComponents, sibling);
                    pushChanges(Change.arrangeComponent(parent.id, selectedIDs, -1, siblingID));
                } else {
                    MimicHelper.placeAfter(parent, selectedComponents, sibling);
                    pushChanges(Change.arrangeComponent(parent.id, selectedIDs, 1, siblingID));
                }

                Renderer.arrangeChildren(parent);
                structTree.refreshComponents(parent);
            } else {
                errorMessage = phrases.sameParentRequired;
            }
        } else {
            errorMessage = phrases.componentNotSpecified;
        }
    } else if (arrangeType === ArrangeActionType.SELECT_PARENT) {
        let parentID = componentID;
        let parent = mimic.getComponentParent(parentID);
        let minLocation = MimicHelper.getMinLocation(selectedComponents);
        let offset = opt_point ?? { x: 0, y: 0 };
        let changes = [];

        for (let component of selectedComponents) {
            let x = component.x - minLocation.x + offset.x;
            let y = component.y - minLocation.y + offset.y;

            if (mimic.updateParent(component, parent, null, x, y)) {
                component.renderer?.detach(component);
                component.renderer?.updateLocation(component);
                parent.renderer?.appendChild(parent, component);
                structTree.removeComponent(component.id);
                changes.push(Change.updateParent(component));
            } else {
                errorMessage = phrases.unableChangeParent;
                break;
            }
        }

        if (changes.length > 0) {
            structTree.refreshComponents(parent);
            propGrid.refresh();
            pushChanges(...changes);
        }
    }

    if (errorMessage) {
        console.error(errorMessage);
        showToast(errorMessage, MessageType.ERROR);
    }
}

function getNextComponentID() {
    maxComponentID ??= mimic.componentMap.size > 0 ? Math.max(...mimic.componentMap.keys()) : 0;
    return ++maxComponentID;
}

function continueDragging(point) {
    if (!longAction?.dragType) {
        return;
    }

    // calculate offset
    let offsetX = point.x - longAction.startPoint.x;
    let offsetY = point.y - longAction.startPoint.y;

    // align to grid
    let gridStep = getGridStep();

    if (gridStep > 1) {
        offsetX = alignValue(offsetX, gridStep);
        offsetY = alignValue(offsetY, gridStep);
    }

    if (longAction.dragType === DragType.MOVE) {
        // move
        if (longAction.moved || Math.abs(offsetX) >= MIN_MOVE || Math.abs(offsetY) >= MIN_MOVE) {
            longAction.moved = true;

            for (let component of selectedComponents) {
                component.renderer?.setLocation(component, component.x + offsetX, component.y + offsetY);
            }
        }
    } else {
        // resize
        let isResizeLeft = DragType.isResizeLeft(longAction.dragType);
        let isResizeRight = DragType.isResizeRight(longAction.dragType);
        let isResizeTop = DragType.isResizeTop(longAction.dragType);
        let isResizeBot = DragType.isResizeBot(longAction.dragType);

        for (let component of selectedComponents) {
            if (component.renderer) {
                let oldX = component.x;
                let oldY = component.y;
                let oldWidth = component.width;
                let oldHeight = component.height;
                let newX = oldX;
                let newY = oldY;
                let newWidth = oldWidth;
                let newHeight = oldHeight;

                if (isResizeLeft) {
                    newWidth = Math.max(oldWidth - offsetX, MIN_SIZE);
                    newX -= newWidth - oldWidth;
                } else if (isResizeRight) {
                    newWidth = Math.max(oldWidth + offsetX, MIN_SIZE);
                }

                if (isResizeTop) {
                    newHeight = Math.max(oldHeight - offsetY, MIN_SIZE);
                    newY -= newHeight - oldHeight;
                } else if (isResizeBot) {
                    newHeight = Math.max(oldHeight + offsetY, MIN_SIZE);
                }

                if (oldX !== newX || oldY !== newY) {
                    longAction.moved = true;
                    component.renderer.setLocation(component, newX, newY);
                }

                if (oldWidth !== newWidth || oldHeight !== newHeight) {
                    longAction.resized = true;
                    component.renderer.setSize(component, newWidth, newHeight);
                }
            }
        }
    }
}

function finishDragging() {
    if (!longAction) {
        return;
    } else if (longAction.resized) {
        console.log("Resize components");
    } else if (longAction.moved) {
        console.log("Move components");
    } else {
        return;
    }

    let changes = [];

    for (let component of selectedComponents) {
        if (component.renderer) {
            let change = Change.updateComponent(component.id);

            if (longAction.moved) {
                component.properties.location = component.renderer.getLocation(component);
                change.setProperty("location", component.properties.location);
            }

            if (longAction.resized) {
                component.properties.size = component.renderer.getSize(component);
                change.setProperty("size", component.properties.size);
            }

            changes.push(change);
        }
    }

    propGrid.refresh();
    pushChanges(...changes);
}

function moveComponents(offsetX, offsetY) {
    if (selectedComponents.length === 0) {
        return;
    }

    console.log("Move components");
    let changes = [];

    for (let component of selectedComponents) {
        component.setLocation(component.x + offsetX, component.y + offsetY);
        component.renderer?.setLocation(component, component.x, component.y);
        changes.push(Change.updateLocation(component));
    }

    propGrid.refresh();
    pushChanges(...changes);
}

function resizeComponents(offsetW, offsetH) {
    if (selectedComponents.length === 0) {
        return;
    }

    console.log("Resize components");
    let changes = [];

    for (let component of selectedComponents) {
        component.setSize(component.width + offsetW, component.height + offsetH);
        component.renderer?.setSize(component, component.width, component.height);
        changes.push(Change.updateSize(component));
    }

    propGrid.refresh();
    pushChanges(...changes);
}

function getLoaderUrl() {
    return rootPath + "Api/MimicEditor/Loader/";
}

function getUpdaterUrl() {
    return rootPath + "Api/MimicEditor/Updater/";
}

function pushChanges(...changes) {
    if (changes.length > 0) {
        updateQueue.push(new UpdateDto(mimicKey, changes));
        addToHistory(changes);
    }
}

function pushChangesNoHistory(...changes) {
    if (changes.length > 0) {
        updateQueue.push(new UpdateDto(mimicKey, changes));
    }
}

async function postUpdates() {
    if (updateQueue.length > 0) {
        // send changes
        while (updateQueue.length > 0) {
            let updateDto = updateQueue.shift();
            let result = await postUpdate(updateDto);

            if (result) {
                mimicModified = true;

                if (updateQueue.length === 0) {
                    await handleQueueEmpty();
                }
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
            body: updateDto.json
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

async function handleQueueEmpty() {
    if (mimicReloadRequired) {
        mimicReloadRequired = false;
        showToast(phrases.mimicReload, MessageType.WARNING);
        clearSelection();
        clearLongAction();
        clearHistory();
        await loadMimic();
    }
}

function handlePropertyChanged(eventData) {
    let changedObject = eventData.topObject;
    let propertyName = eventData.topPropertyName;
    let propertyValue = eventData.topPropertyValue;
    console.log(`Update ${changedObject.toString()}: ${propertyName} = ${JSON.stringify(propertyValue)}`);

    if (changedObject instanceof rs.mimic.Mimic) {
        // update mimic
        unitedRenderer.updateMimicDom();
        pushChanges(Change.updateDocument().setProperty(propertyName, propertyValue));
    } else if (changedObject instanceof rs.mimic.Component || changedObject instanceof UnionObject) {
        // update selected components
        let components = changedObject instanceof rs.mimic.Component
            ? [changedObject]
            : changedObject.targets.filter(t => t instanceof rs.mimic.Component);

        for (let component of components) {
            if (component.isFaceplate) {
                component.handlePropertyChanged(propertyName);
            }

            unitedRenderer.updateComponentDom(component);
            structTree.updateComponent(component);
        }

        pushChanges(Change
            .updateComponent(components.map(c => c.id))
            .setProperty(propertyName, propertyValue));
    } else {
        console.error("Unable to handle property change.");
    }
}

function handleKeyDown(code, ctrlKey, shiftKey) {
    // move or resize components
    if (code.startsWith("Arrow")) {
        if (longAction?.actionType === LongActionType.DRAG) {
            return false; // not handled
        } else {
            let step = ctrlKey || shiftKey ? 1 : getGridStep();
            let offsetX = 0;
            let offsetY = 0;

            switch (code) {
                case "ArrowLeft":
                    offsetX = -step;
                    break;

                case "ArrowRight":
                    offsetX = step;
                    break;

                case "ArrowUp":
                    offsetY = -step;
                    break;

                case "ArrowDown":
                    offsetY = step;
                    break;
            }

            if (shiftKey) {
                resizeComponents(offsetX, offsetY);
            } else {
                moveComponents(offsetX, offsetY);
            }

            return true; // handled
        }
    }

    // menu actions
    if (ctrlKey) {
        switch (code) {
            case "KeyS":
                save();
                return true;

            case "KeyZ":
                undo();
                return true;

            case "KeyY":
                redo();
                return true;

            case "KeyX":
                cut();
                return true;

            case "KeyC":
                copy();
                return true;

            case "KeyV":
                paste();
                return true;
        }
    } else {
        switch (code) {
            case "Delete":
                remove();
                return true;

            case "Escape":
                pointer();
                return true;
        }
    }

    return false;
}

function setEnabled(selector, enabled) {
    $(selector).prop("disabled", !enabled);
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
    unitedRenderer.configure({ fonts, editorOptions });
    phrases = translation.editor;
    splitter = new Splitter("divSplitter");

    bindEvents();
    updateLayout();
    initStructTree();
    initPropGrid();
    initModals();
    await mimicClipboard.defineEmptiness();
    await loadMimic();
    await startUpdatingBackend();
});
