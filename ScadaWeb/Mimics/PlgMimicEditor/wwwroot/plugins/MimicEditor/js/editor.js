// Contains classes: ChangeType, LongActionType, DragMode, MessageType, Change, UpdateDto, LongAction
// No dependencies

// Specifies the change types.
class ChangeType {
    static NONE = 0;

    static UPDATE_DOCUMENT = 1;
    static ADD_DEPENDENCY = 2;
    static DELETE_DEPENDENCY = 3;

    static ADD_COMPONENT = 4;
    static UPDATE_COMPONENT = 5;
    static UPDATE_COMPONENT_PARENT = 6;
    static UPDATE_COMPONENT_BINDINGS = 7;
    static UPDATE_COMPONENT_ACCESS = 8;
    static DELETE_COMPONENT = 9;

    static ADD_IMAGE = 10;
    static RENAME_IMAGE = 11;
    static DELETE_IMAGE = 12;
}

// Specifies the long action types.
class LongActionType {
    static NONE = 0;
    static ADDING = 1;
    static DRAGGING = 2;
}

// Specifies the drag modes.
class DragMode {
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
}

// Specifies the message types for toasts.
class MessageType {
    static INFO = 0;
    static SUCCESS = 1;
    static WARNING = 2;
    static ERROR = 3;
}

// Represents a change in a mimic.
class Change {
    changeType;
    objectID;
    objectName;
    properties;

    constructor(changeType) {
        this.changeType = changeType ?? ChangeType.NONE;
    }

    setProperty(propertyName, value) {
        if (typeof this.properties === "undefined") {
            this.properties = {};
        }

        this.properties[propertyName] = value;
        return this;
    }

    static addComponent(component) {
        let change = new Change(ChangeType.ADD_COMPONENT);
        change.objectID = component.id;
        change.properties = component.properties;
        change.properties.name = component.name;
        change.properties.typeName = component.typeName;
        change.properties.parentID = component.parentID;
        return change;
    }

    static updateComponent(componentID, opt_properties) {
        let change = new Change(ChangeType.UPDATE_COMPONENT);
        change.objectID = componentID;
        change.properties = opt_properties;
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
    actionType;
    componentTypeName;

    constructor(actionType) {
        this.actionType = actionType ?? LongActionType.NONE;
    }

    static startAdding(componentTypeName) {
        let action = new LongAction(LongActionType.ADDING);
        action.componentTypeName = componentTypeName;
        return action;
    }

    getCursor() {
        if (this.actionType === LongActionType.ADDING) {
            return "crosshair";
        }

        return "";
    }
}
