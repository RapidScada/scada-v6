// Contains classes: AlingActionType, ArrangeActionType, ChangeType, DragType, LongActionType, MessageType,
//     ToolbarButton, Change, UpdateDto, LongAction
// No dependencies

// Specifies the action types for component alignment.
class AlingActionType {
    static ALIGN_LEFTS = "align-lefts";
    static ALIGN_CENTERS = "align-centers";
    static ALIGN_RIGHTS = "align-rights";
    static ALIGN_TOPS = "align-tops";
    static ALIGN_MIDDLES = "align-middles";
    static ALIGN_BOTTOMS = "align-bottoms";
    static SAME_WIDTH = "same-width";
    static SAME_HEIGHT = "same-height";
    static SAME_SIZE = "same-size";
    static HOR_SPACING = "hor-spacing";
    static VERT_SPACING = "vert-spacing";

    static sameParentRequired(actionType) {
        return actionType === AlingActionType.ALIGN_LEFTS ||
            actionType === AlingActionType.ALIGN_CENTERS ||
            actionType === AlingActionType.ALIGN_RIGHTS ||
            actionType === AlingActionType.ALIGN_TOPS ||
            actionType === AlingActionType.ALIGN_MIDDLES ||
            actionType === AlingActionType.ALIGN_BOTTOMS ||
            actionType === AlingActionType.HOR_SPACING ||
            actionType === AlingActionType.VERT_SPACING;
    }
}

// Specifies the action types for component arrangement.
class ArrangeActionType {
    static BRING_TO_FRONT = "bring-to-front";
    static BRING_FORWARD = "bring-forward";
    static SEND_BACKWARD = "send-backward";
    static SEND_TO_BACK = "send-to-back";
    static PLACE_BEFORE = "place-before";
    static PLACE_AFTER = "place-after";
    static SELECT_PARENT = "select-parent";
}

// Specifies the change types.
class ChangeType {
    static NONE = 0;

    // Dependencies
    static ADD_DEPENDENCY = 1;
    static REMOVE_DEPENDENCY = 2;

    // Document
    static UPDATE_DOCUMENT = 3;

    // Components
    static ADD_COMPONENT = 4;
    static UPDATE_COMPONENT = 5;
    static ARRANGE_COMPONENT = 6;
    static REMOVE_COMPONENT = 7;

    // Images
    static ADD_IMAGE = 8;
    static REMOVE_IMAGE = 9;
}

// Specifies the drag types.
class DragType {
    static NONE = 0;
    static MOVE = 1;
    static RESIZE_LEFT = 2;
    static RESIZE_RIGHT = 3;
    static RESIZE_TOP = 4;
    static RESIZE_BOT = 5;
    static RESIZE_TOP_LEFT = 6;
    static RESIZE_TOP_RIGHT = 7;
    static RESIZE_BOT_LEFT = 8;
    static RESIZE_BOT_RIGHT = 9;

    static getCursor(dragType) {
        if (dragType) {
            switch (dragType) {
                case DragType.MOVE:
                    return "move";

                case DragType.RESIZE_LEFT:
                case DragType.RESIZE_RIGHT:
                    return "ew-resize";

                case DragType.RESIZE_TOP:
                case DragType.RESIZE_BOT:
                    return "ns-resize";

                case DragType.RESIZE_TOP_LEFT:
                case DragType.RESIZE_BOT_RIGHT:
                    return "nwse-resize";

                case DragType.RESIZE_TOP_RIGHT:
                case DragType.RESIZE_BOT_LEFT:
                    return "nesw-resize";
            }
        }

        return "";
    }

    static isResizeLeft(dragType) {
        return dragType === DragType.RESIZE_LEFT ||
            dragType === DragType.RESIZE_TOP_LEFT ||
            dragType === DragType.RESIZE_BOT_LEFT;
    }

    static isResizeRight(dragType) {
        return dragType === DragType.RESIZE_RIGHT ||
            dragType === DragType.RESIZE_TOP_RIGHT ||
            dragType === DragType.RESIZE_BOT_RIGHT;
    }

    static isResizeTop(dragType) {
        return dragType === DragType.RESIZE_TOP ||
            dragType === DragType.RESIZE_TOP_LEFT ||
            dragType === DragType.RESIZE_TOP_RIGHT;
    }

    static isResizeBot(dragType) {
        return dragType === DragType.RESIZE_BOT ||
            dragType === DragType.RESIZE_BOT_LEFT ||
            dragType === DragType.RESIZE_BOT_RIGHT;
    }
}

// Specifies the long action types.
class LongActionType {
    static NONE = 0;
    static ADD = 1;
    static PASTE = 2;
    static DRAG = 3;
}

// Specifies the message types for toasts.
class MessageType {
    static INFO = 0;
    static SUCCESS = 1;
    static WARNING = 2;
    static ERROR = 3;
}

// Specifies the toolbar button selectors.
class ToolbarButton {
    static SAVE = "#btnSave";
    static UNDO = "#btnUndo";
    static REDO = "#btnRedo";
    static CUT = "#btnCut";
    static COPY = "#btnCopy";
    static PASTE = "#btnPaste";
    static REMOVE = "#btnRemove";
    static POINTER = "#btnPointer";
    static ALIGN = "button.rs-btn-align";
    static ARRANGE = "button.rs-btn-arrange";
}

// Represents a change in a mimic.
class Change {
    static MAX_SHIFT = 1000000;

    changeType = ChangeType.NONE;
    objectID = 0;
    objectIDs = null;
    objectName = "";
    properties = null;
    shift = 0;
    siblingID = 0;

    constructor(changeType) {
        this.changeType = changeType ?? ChangeType.NONE;
    }

    _setObjectID(objectID) {
        if (Array.isArray(objectID)) {
            if (objectID.length === 1) {
                this.objectID = objectID[0];
            } else {
                this.objectIDs = objectID;
            }
        } else {
            this.objectID = objectID;
        }
    }

    setProperty(propertyName, value) {
        if (typeof this.properties === "undefined") {
            this.properties = {};
        }

        this.properties[propertyName] = value;
        return this;
    }

    static addDependency(dependency) {
        let change = new Change(ChangeType.ADD_DEPENDENCY);
        change.objectName = dependency.typeName;
        change.properties = Object.assign({}, dependency);
        return change;
    }

    static removeDependency(typeName) {
        let change = new Change(ChangeType.REMOVE_DEPENDENCY);
        change.objectName = typeName;
        return change;
    }

    static updateDocument(opt_properties) {
        let change = new Change(ChangeType.UPDATE_DOCUMENT);
        change.properties = opt_properties;
        return change;
    }

    static addComponent(component) {
        let change = new Change(ChangeType.ADD_COMPONENT);
        change.objectID = component.id;
        change.properties = Object.assign({}, component.properties);
        change.properties.name = component.name;
        change.properties.typeName = component.typeName;
        change.properties.parentID = component.parentID;
        return change;
    }

    static updateComponent(componentID, opt_properties) {
        let change = new Change(ChangeType.UPDATE_COMPONENT);
        change._setObjectID(componentID);
        change.properties = opt_properties;
        return change;
    }

    static updateLocation(component) {
        return Change.updateComponent(component.id, {
            location: component.properties.location
        });
    }

    static updateSize(component) {
        return Change.updateComponent(component.id, {
            size: component.properties.size
        });
    }

    static arrangeComponent(componentID, shift, opt_siblingID) {
        let change = new Change(ChangeType.ARRANGE_COMPONENT);
        change._setObjectID(componentID);
        change.shift = shift;
        change.siblingID = opt_siblingID;
        return change;
    }

    static removeComponent(componentID) {
        let change = new Change(ChangeType.REMOVE_COMPONENT);
        change._setObjectID(componentID);
        return change;
    }

    static addImage(image) {
        let change = new Change(ChangeType.ADD_IMAGE);
        change.objectName = image.name;
        change.properties = Object.assign({}, image);
        return change;
    }

    static removeImage(imageName) {
        let change = new Change(ChangeType.REMOVE_IMAGE);
        change.objectName = imageName;
        return change;
    }
}

// Represents a data transfer object containing mimic changes.
class UpdateDto {
    mimicKey;
    changes = [];

    constructor(mimicKey, ...changes) {
        this.mimicKey = mimicKey;

        if (changes) {
            this.changes.push(...changes);
        }
    }
}

// Represents a continuous user action based on multiple mouse events.
class LongAction {
    actionType;        // action type
    componentTypeName; // component to add
    dragType;          // drag type, move or resize
    startPoint;        // start point of drag
    moved;             // components were moved during drag operation
    resized;           // components were resized during drag operation

    constructor(actionType) {
        this.actionType = actionType ?? LongActionType.NONE;
    }

    getCursor() {
        if (this.actionType === LongActionType.ADD ||
            this.actionType === LongActionType.PASTE) {
            return "crosshair";
        } else if (this.actionType === LongActionType.DRAG) {
            return DragType.getCursor(this.dragType);
        } else {
            return "";
        }
    }

    actionTypeIs(...actionTypes) {
        return actionTypes.indexOf(this.actionType) >= 0;
    }

    static add(componentTypeName) {
        let action = new LongAction(LongActionType.ADD);
        action.componentTypeName = componentTypeName;
        return action;
    }

    static paste() {
        return new LongAction(LongActionType.PASTE);
    }

    static drag(dragType, startPoint) {
        let action = new LongAction(LongActionType.DRAG);
        action.dragType = dragType;
        action.startPoint = startPoint;
        action.moved = false;
        action.resized = false;
        return action;
    }
}

// Represents a clipboard for copying and pasting components.
class MimicClipboard {
    selectedComponents = [];
    childComponents = [];

    get isEmpty() {
        return !(Array.isArray(this.selectedComponents) && this.selectedComponents.length > 0);
    }

    clear() {
        this.selectedComponents = [];
        this.childComponents = [];
    }
}
