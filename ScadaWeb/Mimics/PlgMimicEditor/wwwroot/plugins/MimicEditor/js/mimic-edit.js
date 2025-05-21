// Depends on jquery, tweakpane, scada-common.js,
//     mimic-common.js, mimic-model.js, mimic-render.js,
//     editor.js, mimic-descr.js, mimic-factory.js, prop-grid.js, struct-tree.js

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
const clipboard = new MimicClipboard();

// Set in MimicEdit.cshtml and MimicEditLang.cshtml
var rootPath = "/";
var mimicKey = "0";
var editorOptions = {};
var phrases = {};
var translation = {};

let splitter = null;
let propGrid = null;
let structTree = null;
let faceplateModal = null;
let imageModal = null;
let mimicWrapperElem = $();
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

    mimicWrapperElem
        .on("mousedown", function (event) {
            if (!longAction) {
                selectMimic();
            } else if (LongActionType.isPointing(longAction.actionType)) {
                finishPointing(0, getMimicPoint(event, mimicWrapperElem, true))
                clearLongAction();
            }
        })
        .on("mousedown", ".comp", function (event) {
            let thisElem = $(this);

            if (!longAction) {
                // select or deselect component, start dragging
                let compElem = closestCompElem(thisElem);
                let isSelected = compElem.hasClass("selected");

                if (event.ctrlKey) {
                    if (isSelected) {
                        removeFromSelection(compElem);
                    } else {
                        addToSelection(compElem);
                    }
                } else {
                    if (isSelected) {
                        startLongAction(LongAction.drag(
                            getDragType(event, compElem),
                            getMimicPoint(event, mimicWrapperElem)));
                    } else {
                        selectComponent(compElem);
                        startLongAction(LongAction.drag(DragType.MOVE, getMimicPoint(event, mimicWrapperElem)));
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
                continueDragging(getMimicPoint(event, mimicWrapperElem));
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
            if (longAction?.actionType == LongActionType.DRAG) {
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
    structTree = new StructTree("divStructure", mimic, phrases);

    // dependencies
    structTree.addEventListener(StructTreeEventType.ADD_DEPENDENCY_CLICK, function () {
        faceplateModal.show(null, function (context) {
            addDependency(context.newObject);
        });
    });

    structTree.addEventListener(StructTreeEventType.EDIT_DEPENDENCY_CLICK, function (event) {
        let eventData = event.detail;
        let faceplateMeta = mimic.dependencyMap.get(eventData.name);

        if (faceplateMeta) {
            faceplateModal.show(faceplateMeta, function (context) {
                addDependency(context.newObject, context.oldObject);
            });
        } else {
            console.error("Dependency not found.");
        }
    });

    structTree.addEventListener(StructTreeEventType.REMOVE_DEPENDENCY_CLICK, function (event) {
        removeDependency(event.detail.name);
    });

    // images
    structTree.addEventListener(StructTreeEventType.ADD_IMAGE_CLICK, function () {
        imageModal.show(null, function (context) {
            addImage(context.newObject);
        });
    });

    structTree.addEventListener(StructTreeEventType.EDIT_IMAGE_CLICK, function (event) {
        let image = mimic.imageMap.get(event.detail.name);

        if (image) {
            imageModal.show(image, function (context) {
                addImage(context.newObject, context.oldObject);
            });
        } else {
            console.error("Image not found.");
        }
    });

    structTree.addEventListener(StructTreeEventType.REMOVE_IMAGE_CLICK, function (event) {
        removeImage(event.detail.name);
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
    propGrid = new PropGrid("tweakpane");
    propGrid.addEventListener(PropGridEventType.PROPERTY_CHANGED, function (event) {
        handlePropertyChanged(event.detail);
    });
}

function initModals() {
    faceplateModal = new FaceplateModal("divFaceplateModal");
    imageModal = new ImageModal("divImageModal");
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
        setButtonsEnabled();
        showFaceplates();
        showStructure();
        showMimic();
        selectMimic();
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

}

function redo() {

}

function cut() {
    if (copy()) {
        remove();
    }
}

function copy() {
    if (selectedComponents.length === 0) {
        return false;
    }

    if (!siblingsSelected()) {
        console.error(phrases.sameParentRequired);
        showToast(phrases.sameParentRequired, MessageType.ERROR);
        return false;
    }

    // copy components
    let componentIDs = selectedComponents.map(c => c.id);
    console.log("Copy components with IDs " + componentIDs.join(", "));

    clipboard.clear();
    let minX = NaN;
    let minY = NaN;

    for (let component of selectedComponents) {
        let componentCopy = mimic.copyComponent(component);
        clipboard.selectedComponents.push(componentCopy);

        if (component.isContainer) {
            clipboard.childComponents.push(...component.getAllChildren().map(c => mimic.copyComponent(c)));
        }

        let x = component.x;
        let y = component.y;

        if (isNaN(minX) || minX > x) {
            minX = x;
        }

        if (isNaN(minY) || minY > y) {
            minY = y;
        }
    }

    // normalize locations
    for (let componentCopy of clipboard.selectedComponents) {
        componentCopy.x -= minX;
        componentCopy.y -= minY;
    }

    setEnabled(ToolbarButton.PASTE, true);
    return true;
}

function paste() {
    if (!clipboard.isEmpty) {
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
        // remove component from mimic and DOM
        let component = mimic.removeComponent(componentID);

        if (component && component.dom) {
            component.dom.remove();
        }

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

    if (AlingActionType.sameParentRequired(actionType) && !siblingsSelected()) {
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
        for (let component of updatedComponents) {
            unitedRenderer.updateComponentDom(component);
            component.dom?.addClass("selected");
        }

        // refresh properties
        propGrid.refresh();

        // update server side
        pushChanges(...changes);
    }
}

function arrange(actionType) {
    if (selectedComponents.length === 0) {
        return;
    }

    if (!siblingsSelected()) {
        console.error(phrases.sameParentRequired);
        showToast(phrases.sameParentRequired, MessageType.ERROR);
        return;
    }

    if (ArrangeActionType.longActionRequired(actionType)) {
        startLongAction(LongAction.arrange(actionType));
        return;
    }

    console.log("Arrange components");
    const MimicHelper = rs.mimic.MimicHelper;
    let parent = selectedComponents[0].parent;
    let getComponentIDs = () => selectedComponents.map(c => c.id);

    switch (actionType) {
        case ArrangeActionType.BRING_TO_FRONT:
            MimicHelper.bringToFront(parent, selectedComponents);
            unitedRenderer.arrangeChildren(parent);
            structTree.refreshComponents(parent, selectedComponents);
            pushChanges(Change.arrangeComponent(getComponentIDs(), Change.MAX_SHIFT));
            break;

        case ArrangeActionType.BRING_FORWARD:
            MimicHelper.bringForward(parent, selectedComponents);
            unitedRenderer.arrangeChildren(parent);
            structTree.refreshComponents(parent, selectedComponents);
            pushChanges(Change.arrangeComponent(getComponentIDs(), 1));
            break;

        case ArrangeActionType.SEND_BACKWARD:
            MimicHelper.sendBackward(parent, selectedComponents);
            unitedRenderer.arrangeChildren(parent);
            structTree.refreshComponents(parent, selectedComponents);
            pushChanges(Change.arrangeComponent(getComponentIDs(), -1));
            break;

        case ArrangeActionType.SEND_TO_BACK:
            MimicHelper.sendToBack(parent, selectedComponents);
            unitedRenderer.arrangeChildren(parent);
            structTree.refreshComponents(parent, selectedComponents);
            pushChanges(Change.arrangeComponent(getComponentIDs(), -Change.MAX_SHIFT));
            break;
    }
}

function setButtonsEnabled(opt_dependsOnSelection) {
    let oneSelected = selectedComponents.length >= 1;
    let twoSelected = selectedComponents.length >= 2;
    setEnabled(ToolbarButton.CUT, oneSelected);
    setEnabled(ToolbarButton.COPY, oneSelected);
    setEnabled(ToolbarButton.REMOVE, oneSelected);
    setEnabled(ToolbarButton.ALIGN, twoSelected);
    setEnabled(ToolbarButton.ARRANGE, oneSelected);

    if (!opt_dependsOnSelection) {
        setEnabled(ToolbarButton.PASTE, !clipboard.isEmpty);
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
    mimicWrapperElem.empty();
    mimicWrapperElem.append(unitedRenderer.createMimicDom());

    for (let component of selectedComponents) {
        component.dom?.addClass("selected");
    }
}

function addDependency(faceplateMeta, opt_oldfaceplateMeta) {
    console.log(`Add '${faceplateMeta.typeName}' dependency`);
    let changes = [];
    let oldTypeName = opt_oldfaceplateMeta?.typeName;

    if (oldTypeName && oldTypeName !== faceplateMeta.typeName) {
        mimic.removeDependency(oldTypeName);
        changes.push(Change.removeDependency(typeName));
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

function selectMimic() {
    clearSelection();
    structTree.selectMimic();
    propGrid.selectedObject = mimic;
    console.log("Mimic selected");
}

function selectNone() {
    clearSelection();
    structTree.selectNone();
    propGrid.selectedObject = null;
}

function clearSelection() {
    selectedComponents.forEach(c => c.dom?.removeClass("selected"));
    selectedComponents = [];
    setButtonsEnabled(true);
}

function selectComponent(compElem) {
    clearSelection();
    structTree.selectNone();
    addToSelection(compElem);
}

function addToSelection(compElem) {
    // compElem can be a component or jQuery object
    let component = getComponentByDom(compElem);

    if (component) {
        component.dom?.addClass("selected");
        selectedComponents.push(component)
        setButtonsEnabled(true);
        structTree.addToSelection(component);
        propGrid.selectedObjects = selectedComponents;
        console.log(`Component with ID ${component.id} selected`);
    }
}

function removeFromSelection(compElem) {
    let component = getComponentByDom(compElem);
    let index = selectedComponents.indexOf(component);

    if (index >= 0) {
        component.dom?.removeClass("selected");
        selectedComponents.splice(index, 1);
        setButtonsEnabled(true);
        structTree.removeFromSelection(component);
        propGrid.selectedObjects = selectedComponents;
        console.log(`Component with ID ${component.id} removed from selection`);
    }
}

function selectComponents(components) {
    clearSelection();
    components.forEach(c => c.dom?.addClass("selected"));
    selectedComponents = components;
    setButtonsEnabled(true);
    structTree.selectComponents(selectedComponents);
    propGrid.selectedObjects = selectedComponents;

    let componentIDs = selectedComponents.map(c => c.id);
    console.log(`Components with IDs ${componentIDs.join(", ")} selected`);
}

function siblingsSelected() {
    let parentIDs = new Set(selectedComponents.map(c => c.parentID));
    return parentIDs.size <= 1;
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
        let gridSize = getGridSize();

        if (gridSize > 1) {
            x = alignValue(x, gridSize);
            y = alignValue(y, gridSize);
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

function getGridSize() {
    return editorOptions && editorOptions.useGrid && editorOptions.gridSize > 1
        ? editorOptions.gridSize
        : 1;
}

function alignValue(value, gridSize) {
    return Math.trunc(Math.round(value / gridSize) * gridSize);
}

function startLongAction(action) {
    if (action) {
        longAction = action;
        mimicWrapperElem.css("cursor", longAction.getCursor());
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
        mimicWrapperElem.css("cursor", "");
        setEnabled(ToolbarButton.POINTER, false);
    }
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
        component.id = getNextComponentID();
        descriptor.repair(component);
        mimic.addComponent(component, parent, point.x, point.y);
        unitedRenderer.createComponentDom(component);

        // update structure and properties
        structTree.addComponent(component);
        selectComponent(component);

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

function pasteComponents(parentID, point) {
    console.log(`Paste components at ${point.x}, ${point.y}` +
        (parentID > 0 ? ` inside component ${parentID}` : ""));

    let parent = parentID > 0 ? mimic.componentMap.get(parentID) : mimic;
    let idMap = new Map(); // key is old ID, value is new ID
    let changes = [];
    let componentsToSelect = [];

    if (!parent) {
        console.error("Parent not found.");
        return;
    }

    // add top-level components
    for (let sourceComponent of clipboard.selectedComponents) {
        let component = mimic.copyComponent(sourceComponent);
        let newID = getNextComponentID();
        idMap.set(component.id, newID);
        component.id = newID;

        if (component.isFaceplate) {
            component.applyModel(mimic.faceplateMap.get(component.typeName));
        }

        mimic.addComponent(component, parent, point.x + component.x, point.y + component.y);
        unitedRenderer.createComponentDom(component);

        structTree.addComponent(component);
        componentsToSelect.push(component);
        changes.push(Change.addComponent(component));
    }

    // add child components
    for (let sourceComponent of clipboard.childComponents) {
        let newParentID = idMap.get(sourceComponent.parentID);
        parent = mimic.componentMap.get(newParentID);

        if (parent) {
            let component = mimic.copyComponent(sourceComponent);
            let newID = getNextComponentID();
            idMap.set(component.id, newID);
            component.id = newID;
            mimic.addComponent(component, parent, component.x, component.y);
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
    if (selectedComponents.length === 0 || !siblingsSelected()) {
        return;
    }

    console.log("Arrange components");
    let errorMessage = "";

    if (arrangeType == ArrangeActionType.PLACE_BEFORE || arrangeType == ArrangeActionType.PLACE_AFTER) {
        const MimicHelper = rs.mimic.MimicHelper;
        let parent = selectedComponents[0].parent;
        let sibling = componentID > 0 ? mimic.componentMap.get(componentID) : null;

        if (sibling) {
            if (sibling.parent === parent) {
                let selectedIDs = selectedComponents.map(c => c.id);

                if (arrangeType == ArrangeActionType.PLACE_BEFORE) {
                    MimicHelper.placeBefore(parent, sibling, selectedComponents);
                    pushChanges(Change.arrangeComponent(selectedIDs, -1, componentID));
                } else {
                    MimicHelper.placeAfter(parent, sibling, selectedComponents);
                    pushChanges(Change.arrangeComponent(selectedIDs, 1, componentID));
                }

                unitedRenderer.arrangeChildren(parent);
                structTree.refreshComponents(parent, selectedComponents);
            } else {
                errorMessage = phrases.sameParentRequired;
            }
        } else {
            errorMessage = phrases.componentNotSpecified;
        }
    } else if (arrangeType == ArrangeActionType.SELECT_PARENT) {

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
    let gridSize = getGridSize();

    if (gridSize > 1) {
        offsetX = alignValue(offsetX, gridSize);
        offsetY = alignValue(offsetY, gridSize);
    }

    if (longAction.dragType === DragType.MOVE) {
        // move
        if (longAction.moved || Math.abs(offsetX) >= MIN_MOVE || Math.abs(offsetY) >= MIN_MOVE) {
            longAction.moved = true;

            for (let component of selectedComponents) {
                if (component.renderer) {
                    component.renderer.setLocation(component, component.x + offsetX, component.y + offsetY);
                }
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
        changes.push(Change.updateLocation(component));

        if (component.renderer) {
            component.renderer.setLocation(component, component.x, component.y);
        }
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
        changes.push(Change.updateSize(component));

        if (component.renderer) {
            component.renderer.setSize(component, component.width, component.height);
        }
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
        let updateDto = new UpdateDto(mimicKey, ...changes);
        updateQueue.push(updateDto);
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

async function handleQueueEmpty() {
    if (mimicReloadRequired) {
        mimicReloadRequired = false;
        showToast(phrases.mimicReload, MessageType.WARNING);
        clearSelection();
        clearLongAction();
        await loadMimic();
    }
}

function handlePropertyChanged(eventData) {
    let selectedObject = eventData.selectedObject;
    let propertyName = eventData.propertyName;
    let value = eventData.value;
    console.log(`Update ${selectedObject.toString()}: ${propertyName} = ${JSON.stringify(value) }`);

    if (selectedObject instanceof rs.mimic.Mimic) {
        // update mimic
        unitedRenderer.updateMimicDom();
        pushChanges(Change.updateDocument().setProperty(propertyName, value));
    } else if (selectedObject instanceof rs.mimic.Component || selectedObject instanceof UnionObject) {
        let components = selectedObject instanceof rs.mimic.Component
            ? [selectedObject]
            : selectedObject.targets.filter(t => t instanceof rs.mimic.Component);

        for (let component of components) {
            // update client side
            unitedRenderer.updateComponentDom(component);
            component.dom?.addClass("selected");

            // update structure tree
            structTree.updateComponent(component);
        }

        // update server side
        pushChanges(Change
            .updateComponent(components.map(c => c.id))
            .setProperty(propertyName, value));
    }
}

function handleKeyDown(code, ctrlKey, shiftKey) {
    // move or resize components
    if (code.startsWith("Arrow")) {
        if (longAction?.actionType === LongActionType.DRAG) {
            return false; // not handled
        } else {
            let size = ctrlKey || shiftKey ? 1 : getGridSize();
            let offsetX = 0;
            let offsetY = 0;

            switch (code) {
                case "ArrowLeft":
                    offsetX = -size;
                    break;

                case "ArrowRight":
                    offsetX = size;
                    break;

                case "ArrowUp":
                    offsetY = -size;
                    break;

                case "ArrowDown":
                    offsetY = size;
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
    splitter = new Splitter("divSplitter");
    mimicWrapperElem = $("#divMimicWrapper");

    bindEvents();
    updateLayout();
    initStructTree();
    initPropGrid();
    initModals();
    translateProperties();
    await loadMimic();
    await startUpdatingBackend();
});
