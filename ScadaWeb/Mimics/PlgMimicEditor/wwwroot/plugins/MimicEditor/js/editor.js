// Contains classes: AlingActionType, ArrangeActionType, ChangeType, DragType, EnabledDependsOn, LongActionType,
//     MessageType, ToolbarButton, Change, UpdateDto, LongAction, MimicClipboard, 
//     HistoryChange, HistoryPoint, MimicHistory
// Depends on mimic-model.js

// Specifies the action types for component alignment.
// Readable values are used in data-* attributes.
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
// Readable values are used in data-* attributes.
class ArrangeActionType {
    static SEND_TO_BACK = "send-to-back";
    static SEND_BACKWARD = "send-backward";
    static BRING_FORWARD = "bring-forward";
    static BRING_TO_FRONT = "bring-to-front";
    static PLACE_BEFORE = "place-before";
    static PLACE_AFTER = "place-after";
    static SELECT_PARENT = "select-parent";

    static longActionRequired(actionType) {
        return actionType === ArrangeActionType.PLACE_BEFORE ||
            actionType === ArrangeActionType.PLACE_AFTER ||
            actionType === ArrangeActionType.SELECT_PARENT;
    }
}

// Specifies the change types.
// Readable values are sent in HTTP requests.
class ChangeType {
    // Dependencies
    static ADD_DEPENDENCY = "add-dependency";
    static REMOVE_DEPENDENCY = "remove-dependency";

    // Document
    static UPDATE_DOCUMENT = "update-document";

    // Components
    static ADD_COMPONENT = "add-component";
    static UPDATE_COMPONENT = "update-component";
    static UPDATE_PARENT = "update-parent";
    static ARRANGE_COMPONENT = "arrange-component";
    static REMOVE_COMPONENT = "remove-component";

    // Images
    static ADD_IMAGE = "add-image";
    static REMOVE_IMAGE = "remove-image";
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

// Specifies the categories on which the availability of the toolbar buttons depends.
class EnabledDependsOn {
    static NOT_SPECIFIED = 0;
    static SELECTION = 1;
    static HISTORY = 2;
}

// Specifies the long action types.
class LongActionType {
    static NONE = 0;
    static ADD = 1;
    static PASTE = 2;
    static DRAG = 3;
    static ARRANGE = 4;

    static isPointing(actionType) {
        return actionType == LongActionType.ADD ||
            actionType == LongActionType.PASTE ||
            actionType == LongActionType.ARRANGE;
    }
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

    changeType = "";
    objectID = 0;
    objectIDs = null;
    objectName = "";
    properties = null;
    parentID = 0;
    shift = 0;
    siblingID = 0;

    constructor(changeType) {
        this.changeType = changeType;
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

    getObjectIDs() {
        return this.objectID > 0
            ? [this.objectID]
            : (this.objectIDs ?? []);
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

    static updateParent(component) {
        let change = new Change(ChangeType.UPDATE_PARENT);
        change.objectID = component.id;
        change.properties = {
            location: component.properties.location
        };
        change.parentID = component.parent?.id ?? 0;
        return change;
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

    constructor(mimicKey, opt_changes) {
        this.mimicKey = mimicKey;

        if (opt_changes) {
            this.changes.push(...opt_changes);
        }
    }
}

// Represents a continuous user action based on multiple mouse events.
class LongAction {
    actionType;        // action type
    componentTypeName; // component to add
    dragType;          // drag type: move or resize
    startPoint;        // start point of drag
    moved;             // components were moved during drag operation
    resized;           // components were resized during drag operation
    arrangeType;       // arrange type: before, after or parent

    constructor(actionType) {
        this.actionType = actionType ?? LongActionType.NONE;
    }

    getCursor() {
        if (this.actionType === LongActionType.ADD ||
            this.actionType === LongActionType.PASTE) {
            return "crosshair";
        } else if (this.actionType === LongActionType.DRAG) {
            return DragType.getCursor(this.dragType);
        } else if (this.actionType === LongActionType.ARRANGE) {
            return "pointer";
        } else {
            return "";
        }
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

    static arrange(arrangeType) {
        let action = new LongAction(LongActionType.ARRANGE);
        action.arrangeType = arrangeType;
        return action;
    }
}

// Represents a clipboard for copying and pasting components.
class MimicClipboard {
    static MARKER = "MimicEditor";

    _isEmpty;
    _clipboardData;
    _componentJsons;
    _parentID;
    _offset;

    constructor() {
        this._clear();
    }

    get parentID() {
        return this._clipboardData ? this._clipboardData.parentID : this._parentID;
    }

    get offset() {
        return this._clipboardData ? this._clipboardData.offset : this._offset;
    }

    get isEmpty() {
        return this._isEmpty;
    }

    _clear() {
        this._isEmpty = true;
        this._clipboardData = null;
        this._componentJsons = [];
        this._parentID = 0;
        this._offset = { x: 0, y: 0 };
    }

    static _validate(clipboardData) {
        return clipboardData &&
            clipboardData.marker === MimicClipboard.MARKER &&
            Array.isArray(clipboardData.components) &&
            Number.isInteger(clipboardData.parentID) &&
            clipboardData.offset instanceof Object;
    }

    async defineEmptiness() {
        if (this._componentJsons.length > 0) {
            this._isEmpty = false;
        } else {
            try { this._isEmpty = !await navigator.clipboard.readText(); }
            catch { this._isEmpty = true; }
        }
    }

    async writeComponents(components) {
        // extract information from components
        let plainObjects = [];
        this._clear();

        if (Array.isArray(components) && components.length > 0) {
            this._isEmpty = false;
            this._parentID = components[0].parentID; // assuming that parents are the same
            this._offset = rs.mimic.MimicHelper.getMinLocation(components);

            for (let component of components) {
                plainObjects.push(component.toPlainObject())

                if (component.isContainer) {
                    plainObjects.push(...component.getAllChildren().map(c => c.toPlainObject()));
                }
            }

            this._componentJsons = plainObjects.map(o => JSON.stringify(o));
        }

        // write to system buffer
        try {
            await navigator.clipboard.writeText(JSON.stringify({
                marker: MimicClipboard.MARKER,
                components: plainObjects,
                parentID: this._parentID,
                offset: this._offset
            }));
        } catch (ex) {
            console.error("Error writing to clipboard: " + ex.message);
        }
    }

    async readComponents() {
        // read from system buffer first
        try {
            let text = await navigator.clipboard.readText();
            let data;
            try { data = JSON.parse(text); }
            catch { data = null; }

            if (MimicClipboard._validate(data)) {
                this._clipboardData = data;
                return data.components;
            }
        } catch (ex) {
            console.error("Error reading from clipboard: " + ex.message);
        }

        // return plain objects that are not instances of Component
        return this._componentJsons.map(j => JSON.parse(j));
    }
}

// Represents a change in history.
class HistoryChange extends Change {
    oldObjectJsons = null;
    newObjectJsons = null;
    oldChildIndexes = null;
    newChildIndexes = null;

    constructor(source) {
        super("");
        Object.assign(this, source);
    }
}

// Represents a single point in history.
class HistoryPoint {
    changes = []; // instances of HistoryChange class

    constructor(changes) {
        if (changes) {
            this.changes.push(...changes);
        }
    }

    toReversed() {
        let reversedChanges = [];

        for (let change of this.changes) {
            switch (change.changeType) {
                case ChangeType.UPDATE_DOCUMENT:
                    reversedChanges.push(new HistoryChange({
                        changeType: ChangeType.UPDATE_DOCUMENT,
                        oldObjectJsons: change.newObjectJsons,
                        newObjectJsons: change.oldObjectJsons
                    }));
                    break;

                case ChangeType.ADD_COMPONENT:
                    reversedChanges.push(new HistoryChange({
                        changeType: ChangeType.REMOVE_COMPONENT,
                        objectIDs: change.objectIDs,
                        oldObjectJsons: change.newObjectJsons
                    }));
                    break;

                case ChangeType.UPDATE_COMPONENT:
                    reversedChanges.push(new HistoryChange({
                        changeType: ChangeType.UPDATE_COMPONENT,
                        objectIDs: change.objectIDs,
                        oldObjectJsons: change.newObjectJsons,
                        newObjectJsons: change.oldObjectJsons
                    }));
                    break;

                case ChangeType.REMOVE_COMPONENT:
                    reversedChanges.push(new HistoryChange({
                        changeType: ChangeType.ADD_COMPONENT,
                        objectIDs: change.objectIDs,
                        newObjectJsons: change.oldObjectJsons
                    }));
                    break;

                case ChangeType.UPDATE_PARENT:
                    break;

                case ChangeType.ARRANGE_COMPONENT:
                    break;
            }
        }

        return reversedChanges.length > 0
            ? new HistoryPoint(reversedChanges)
            : null;
    }
}

// Contains the history of mimic changes.
class MimicHistory {
    static MAX_SIZE = 20; // maximum number of history points

    _points;           // history points
    _headIndex;        // index to add new points
    _documentJson;     // chached mimic document as a JSON string
    _componentJsonMap; // cached components as JSON strings accessible by ID

    constructor() {
        this.clear();
    }

    get canUndo() {
        return this._headIndex > 0;
    }

    get canRedo() {
        return this._headIndex < this._points.length;
    }

    _fillChangeData(change, mimic) {
        if (!change.objectIDs) {
            change.objectIDs = change.getObjectIDs();
        }

        switch (change.changeType) {
            case ChangeType.UPDATE_DOCUMENT:
                let documentJson = JSON.stringify(mimic.document);
                change.newObjectJsons = [documentJson];
                change.oldObjectJsons = [this._documentJson];
                this._documentJson = documentJson;
                break;

            case ChangeType.ADD_COMPONENT:
                change.newObjectJsons = this._getComponentJsonsFromMimic(change.objectIDs, mimic);
                this._updateComponentCache(change.objectIDs, change.newObjectJsons);
                break;

            case ChangeType.UPDATE_COMPONENT:
                change.oldObjectJsons = this._getComponentJsonsFromCache(change.objectIDs);
                change.newObjectJsons = this._getComponentJsonsFromMimic(change.objectIDs, mimic);
                this._updateComponentCache(change.objectIDs, change.newObjectJsons);
                break;

            case ChangeType.REMOVE_COMPONENT:
                change.oldObjectJsons = this._getComponentJsonsFromCache(change.objectIDs);
                break;

            case ChangeType.UPDATE_PARENT:
                break;

            case ChangeType.ARRANGE_COMPONENT:
                break;
        }
    }

    _getComponentJsonsFromCache(componentIDs) {
        return componentIDs.map(id => this._componentJsonMap.get(id));
    }

    _getComponentJsonsFromMimic(componentIDs, mimic) {
        return componentIDs.map(id => JSON.stringify(mimic.componentMap.get(id).toPlainObject()));
    }

    _updateComponentCache(componentIDs, componentJsons) {
        componentIDs.forEach((id, index) => {
            this._componentJsonMap.set(id, componentJsons[index]);
        });
    }

    clear() {
        this._points = [];
        this._headIndex = 0;
        this._documentJson = null;
        this._componentJsonMap = new Map();
    }

    rememberDocument(mimic, overwriteExisting) {
        if (overwriteExisting || !this._documentJson) {
            this._documentJson = JSON.stringify(mimic.document);
        }
    }

    rememberComponent(component, overwriteExisting) {
        if (overwriteExisting || !this._componentJsonMap.has(component.id)) {
            this._componentJsonMap.set(component.id, JSON.stringify(component.toPlainObject()));
        }
    }

    addPoint(mimic, changes) {
        // prepare history changes
        let historyChanges = [];

        for (let change of changes) {
            if (change instanceof HistoryChange) {
                historyChanges.push(change);
            } else if (change instanceof Change) {
                let historyChange = new HistoryChange(change);
                this._fillChangeData(historyChange, mimic);
                historyChanges.push(historyChange);
            }
        }

        if (historyChanges.length > 0) {
            // remove history points after head index
            if (this._headIndex < this._points.length) {
                this._points.length = this._headIndex;
            }

            // add history point
            let point = new HistoryPoint(historyChanges);
            this._points.push(point);

            // remove history points if history size exceeded
            let removeCount = this._points.length - MimicHistory.MAX_SIZE;

            if (removeCount > 0) {
                this._points.splice(0, removeCount);
            }

            // update head index
            this._headIndex = this._points.length;
        }
    }

    getUndoPoint() {
        if (this._headIndex > 0) {
            this._headIndex--;
            let point = this._points[this._headIndex];
            return point.toReversed();
        } else {
            return null;
        }
    }

    getRedoPoint() {
        if (this._headIndex < this._points.length) {
            let point = this._points[this._headIndex];
            this._headIndex++;
            return point;
        } else {
            return null;
        }
    }
}
