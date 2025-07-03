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
    static ADD_DEPENDENCY = "AddDependency";
    static REMOVE_DEPENDENCY = "RemoveDependency";

    // Document
    static UPDATE_DOCUMENT = "UpdateDocument";

    // Components
    static ADD_COMPONENT = "AddComponent";
    static UPDATE_COMPONENT = "UpdateComponent";
    static UPDATE_PARENT = "UpdateParent";
    static ARRANGE_COMPONENT = "ArrangeComponent";
    static REMOVE_COMPONENT = "RemoveComponent";

    // Images
    static ADD_IMAGE = "AddImage";
    static REMOVE_IMAGE = "RemoveImage";
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
        return actionType === LongActionType.ADD ||
            actionType === LongActionType.PASTE ||
            actionType === LongActionType.ARRANGE;
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

// Represents a change in a mimic for data transfer.
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
    indexes = null;

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

    setProperty(name, value) {
        this.properties ??= {};
        this.properties[name] = value;
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

        if (opt_properties) {
            change.properties = Object.assign({}, opt_properties);
        }

        return change;
    }

    static addComponent(component) {
        let change = new Change(ChangeType.ADD_COMPONENT);
        change.objectID = component.id;
        change.properties = Object.assign({}, component.properties);
        change.properties.name = component.name;
        change.properties.typeName = component.typeName;
        change.parentID = component.parentID;
        change.index = component.index;
        return change;
    }

    static updateComponent(componentID, opt_properties) {
        let change = new Change(ChangeType.UPDATE_COMPONENT);
        change._setObjectID(componentID);

        if (opt_properties) {
            change.properties = Object.assign({}, opt_properties);
        }

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
        change.parentID = component.parentID;
        change.index = component.index;
        return change;
    }

    static arrangeComponent(parentID, componentID, shift, opt_siblingID) {
        let change = new Change(ChangeType.ARRANGE_COMPONENT);
        change._setObjectID(componentID);
        change.parentID = parentID;
        change.shift = shift;
        change.siblingID = opt_siblingID;
        return change;
    }

    static arrangeByIndexes(parentID, componentIDs, indexes) {
        let change = new Change(ChangeType.ARRANGE_COMPONENT);
        change.objectIDs = componentIDs;
        change.parentID = parentID;
        change.indexes = indexes;
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
    changes;
    json;

    constructor(mimicKey, opt_changes) {
        this.mimicKey = mimicKey;
        this.changes = opt_changes ?? [];
        this.json = JSON.stringify({
            mimicKey: this.mimicKey,
            changes: this.changes
        });
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
    _rootID;
    _offset;

    constructor() {
        this._clear();
    }

    get rootID() {
        return this._clipboardData ? this._clipboardData.rootID : this._rootID;
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
        this._rootID = 0;
        this._offset = { x: 0, y: 0 };
    }

    static _validate(clipboardData) {
        return clipboardData &&
            clipboardData.marker === MimicClipboard.MARKER &&
            Array.isArray(clipboardData.components) &&
            Number.isInteger(clipboardData.rootID) &&
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
            this._rootID = components[0].parentID; // assuming that parents are the same
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
                rootID: this._rootID,
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
class HistoryChange {
    changeType = "";
    objectID = null;
    oldObjectJson = null;
    newObjectJson = null;
    oldIndex = null;
    newIndex = null;

    constructor(source) {
        Object.assign(this, source);
    }

    getOldObject() {
        return this.oldObjectJson ? JSON.parse(this.oldObjectJson) : null;
    }

    getNewObject() {
        return this.newObjectJson ? JSON.parse(this.newObjectJson) : null;
    }
}

// Represents a single point in history.
class HistoryPoint {
    changes; // instances of HistoryChange class

    constructor(changes) {
        this.changes = changes ?? [];
    }

    toReversed() {
        let reversedChanges = [];

        for (let change of this.changes.toReversed()) {
            switch (change.changeType) {
                case ChangeType.UPDATE_DOCUMENT:
                    reversedChanges.push(new HistoryChange({
                        changeType: ChangeType.UPDATE_DOCUMENT,
                        oldObjectJson: change.newObjectJson,
                        newObjectJson: change.oldObjectJson
                    }));
                    break;

                case ChangeType.ADD_COMPONENT:
                    reversedChanges.push(new HistoryChange({
                        changeType: ChangeType.REMOVE_COMPONENT,
                        objectID: change.objectID,
                        oldObjectJson: change.newObjectJson
                    }));
                    break;

                case ChangeType.UPDATE_COMPONENT:
                    reversedChanges.push(new HistoryChange({
                        changeType: ChangeType.UPDATE_COMPONENT,
                        objectID: change.objectID,
                        oldObjectJson: change.newObjectJson,
                        newObjectJson: change.oldObjectJson
                    }));
                    break;

                case ChangeType.REMOVE_COMPONENT:
                    reversedChanges.push(new HistoryChange({
                        changeType: ChangeType.ADD_COMPONENT,
                        objectID: change.objectID,
                        newObjectJson: change.oldObjectJson
                    }));
                    break;

                case ChangeType.UPDATE_PARENT:
                    reversedChanges.push(new HistoryChange({
                        changeType: ChangeType.UPDATE_PARENT,
                        objectID: change.objectID,
                        oldObjectJson: change.newObjectJson,
                        newObjectJson: change.oldObjectJson
                    }));
                    break;

                case ChangeType.ARRANGE_COMPONENT:
                    reversedChanges.push(new HistoryChange({
                        changeType: ChangeType.ARRANGE_COMPONENT,
                        objectID: change.objectID,
                        oldIndex: change.newIndex,
                        newIndex: change.oldIndex
                    }));
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

    _createHistoryChanges(mimicChange, mimic) {
        let historyChanges = [];

        switch (mimicChange.changeType) {
            case ChangeType.UPDATE_DOCUMENT: {
                let oldDocumentJson = this._documentJson;
                let newDocumentJson = JSON.stringify(mimic.document);
                this._documentJson = newDocumentJson;

                historyChanges.push(new HistoryChange({
                    changeType: ChangeType.UPDATE_DOCUMENT,
                    oldObjectJson: oldDocumentJson,
                    newObjectJson: newDocumentJson
                }));

                break;
            }
            case ChangeType.ADD_COMPONENT: {
                let componentID = mimicChange.objectID;
                let componentJson = this._getComponentJsonFromMimic(componentID, mimic);
                this._componentJsonMap.set(componentID, componentJson);

                historyChanges.push(new HistoryChange({
                    changeType: ChangeType.ADD_COMPONENT,
                    objectID: componentID,
                    newObjectJson: componentJson
                }));

                break;
            }
            case ChangeType.UPDATE_COMPONENT: {
                for (let componentID of mimicChange.getObjectIDs()) {
                    let oldComponentJson = this._getComponentJsonFromCache(componentID);
                    let newComponentJson = this._getComponentJsonFromMimic(componentID, mimic);
                    this._componentJsonMap.set(componentID, newComponentJson);

                    historyChanges.push(new HistoryChange({
                        changeType: ChangeType.UPDATE_COMPONENT,
                        objectID: componentID,
                        oldObjectJson: oldComponentJson,
                        newObjectJson: newComponentJson
                    }));
                }

                break;
            }
            case ChangeType.REMOVE_COMPONENT: {
                for (let componentID of mimicChange.getObjectIDs()) {
                    historyChanges.push(new HistoryChange({
                        changeType: ChangeType.REMOVE_COMPONENT,
                        objectID: componentID,
                        oldObjectJson: this._getComponentJsonFromCache(componentID)
                    }));
                }

                break;
            }
            case ChangeType.UPDATE_PARENT: {
                let componentID = mimicChange.objectID;
                let oldComponentJson = this._getComponentJsonFromCache(componentID);
                let newComponentJson = this._getComponentJsonFromMimic(componentID, mimic);
                this._componentJsonMap.set(componentID, newComponentJson);

                historyChanges.push(new HistoryChange({
                    changeType: ChangeType.UPDATE_PARENT,
                    objectID: componentID,
                    oldObjectJson: oldComponentJson,
                    newObjectJson: newComponentJson
                }));

                break;
            }
            case ChangeType.ARRANGE_COMPONENT: {
                for (let componentID of mimicChange.getObjectIDs()) {
                    let oldComponentJson = this._getComponentJsonFromCache(componentID);
                    let newComponent = mimic.componentMap.get(componentID);
                    this._updateComponentCache(componentID, newComponent);

                    if (oldComponentJson && newComponent) {
                        let oldComponent = JSON.parse(oldComponentJson);
                        historyChanges.push(new HistoryChange({
                            changeType: ChangeType.ARRANGE_COMPONENT,
                            objectID: componentID,
                            oldIndex: oldComponent.index,
                            newIndex: newComponent.index
                        }));
                    }
                }

                break;
            }
        }

        return historyChanges;
    }

    _getComponentJsonFromCache(componentID) {
        return this._componentJsonMap.get(componentID);
    }

    _getComponentJsonFromMimic(componentID, mimic) {
        let component = mimic.componentMap.get(componentID);
        return component ? JSON.stringify(component.toPlainObject()) : null;
    }

    _updateComponentCache(componentID, component) {
        if (component) {
            this._componentJsonMap.set(componentID, null);
        } else {
            this._componentJsonMap.set(componentID, JSON.stringify(newComponent.toPlainObject()));
        }
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

    addPoint(mimic, mimicChanges) {
        // create history changes
        let historyChanges = [];

        for (let mimicChange of mimicChanges) {
            if (mimicChange instanceof Change) {
                historyChanges.push(...this._createHistoryChanges(mimicChange, mimic));
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

// Contains classes: ModalContext, ModalBase, FaceplateModal, ImageModal, TextEditor
// Depends on jquery, bootstrap, mimic-model.js

// Represents a context of a modal dialog.
class ModalContext {
    oldValue = null;
    newValue = null;
    result = false;
    callback = null;

    constructor(source) {
        Object.assign(this, source);
    }
}

// A base class for modal dialogs.
class ModalBase {
    _elem;
    _modal;
    _context;

    constructor(elemID) {
        this._elem = $("#" + elemID);
        this._modal = new bootstrap.Modal(this._elem[0]);
        this._context = new ModalContext();
    }

    _invokeCallback() {
        if (this._context.result && this._context.callback instanceof Function) {
            this._context.callback.call(this, this._context);
        }
    }
}

// Represents a modal dialog for editing a faceplate meta.
class FaceplateModal extends ModalBase {
    constructor(elemID) {
        super(elemID);
        this._bindEvents();
    }

    _bindEvents() {
        $("#frmFaceplateModal").on("submit", () => {
            $("#faceplateModal_btnOK").trigger("click");
            return false;
        });

        $("#faceplateModal_btnOK").on("click", () => {
            let formElem = $("#frmFaceplateModal");

            if (formElem[0].checkValidity()) {
                this._readFields();
                this._context.result = true;
                this._modal.hide();
            }

            formElem.addClass("was-validated");
        });

        this._elem
            .on("shown.bs.modal", () => {
                $("#faceplateModal_txtTypeName").focus();
            })
            .on("hidden.bs.modal", () => {
                this._invokeCallback();
            });
    }

    _readFields() {
        let obj = this._context.newValue;

        if (obj) {
            obj.typeName = $("#faceplateModal_txtTypeName").val();
            obj.path = $("#faceplateModal_txtPath").val();
        }
    }

    show(faceplateMeta, callback) {
        let obj = new rs.mimic.FaceplateMeta();
        Object.assign(obj, faceplateMeta);

        this._context = new ModalContext({
            oldValue: faceplateMeta,
            newValue: obj,
            callback: callback
        });

        $("#frmFaceplateModal").removeClass("was-validated")
        $("#faceplateModal_txtTypeName").val(obj.typeName);
        $("#faceplateModal_txtPath").val(obj.path);
        this._modal.show();
    }
}

// Represents a modal dialog for editing an image.
class ImageModal extends ModalBase {
    constructor(elemID) {
        super(elemID);
        this._bindEvents();
    }

    _bindEvents() {
        $("#frmImageModal").on("submit", () => {
            $("#imageModal_btnOK").trigger("click");
            return false;
        });

        $("#imageModal_btnOK").on("click", () => {
            let formElem = $("#frmImageModal");

            if (formElem[0].checkValidity()) {
                this._readFields();
                this._context.result = true;
                this._modal.hide();
            }

            formElem.addClass("was-validated");
        });

        $("#imageModal_btnUpload").on("click", () => {
            $("#imageModal_file").trigger("click");
        });

        $("#imageModal_btnDownload").on("click", (event) => {
            let linkElem = $(event.target);
            this._downloadImage(linkElem);
        });

        $("#imageModal_file").on("change", (event) => {
            let file = event.target.files[0];

            if (file) {
                this._uploadImage(file);
            }
        });

        this._elem
            .on("shown.bs.modal", () => {
                $("#imageModal_txtName").focus();
            })
            .on("hidden.bs.modal", () => {
                this._invokeCallback();
            });
    }

    _readFields() {
        let obj = this._context.newValue;

        if (obj) {
            obj.name = $("#imageModal_txtName").val();
            obj.dataUrl = $("#imageModal_imgPreview").attr("src");
        }
    }

    _showFileSize(size) {
        $("#imageModal_spnFileSize").text(size ? "(" + Math.round(size / 1024) + " KB)" : "");
    }

    _getFileSize(imageData) {
        return imageData ? atob(imageData).length : 0;
    }

    _showImage(dataUrl) {
        if (dataUrl) {
            $("#imageModal_imgPreview").attr("src", dataUrl).removeClass("d-none");
            $("#imageModal_divNoImage").addClass("d-none");
            $("#imageModal_btnDownload").prop("disabled", false);
        } else {
            $("#imageModal_imgPreview").attr("src", "").addClass("d-none");
            $("#imageModal_divNoImage").removeClass("d-none");
            $("#imageModal_btnDownload").prop("disabled", true);
        }
    }

    _uploadImage(file) {
        let reader = new FileReader();

        reader.onload = () => {
            let txtName = $("#imageModal_txtName");

            if (!txtName.val()) {
                txtName.val(file.name);
            }

            this._showFileSize(file.size);
            this._showImage(reader.result);
        };

        reader.onerror = () => {
            console.error("Error reading file.");
        };

        reader.readAsDataURL(file);
    }

    _downloadImage(linkElem) {
        let name = $("#imageModal_txtName").val();
        let dataUrl = $("#imageModal_imgPreview").attr("src");
        linkElem
            .attr("download", name)
            .attr("href", dataUrl);
    }

    show(image, callback) {
        let obj = new rs.mimic.Image();
        Object.assign(obj, image);

        this._context = new ModalContext({
            oldValue: image,
            newValue: obj,
            callback: callback
        });

        $("#frmImageModal").removeClass("was-validated")
        $("#imageModal_txtName").val(obj.name);
        $("#imageModal_file").val("");
        this._showFileSize(this._getFileSize(obj.data));
        this._showImage(obj.dataUrl);
        this._modal.show();
    }
}

// Represents a modal dialog for editing text.
class TextEditor extends ModalBase {
    static DEFAULT_OPTIONS = {
        language: "none"
    }

    _flask;

    constructor(elemID) {
        super(elemID);
        let editorElem = $("#textEditor_divEditor");
        this._flask = new CodeFlask(editorElem[0], TextEditor.DEFAULT_OPTIONS);
        this._bindEvents();
    }

    _bindEvents() {
        $("#textEditor_btnOK").on("click", () => {
            this._context.newValue = this._flask.getCode();
            this._context.result = true;
            this._modal.hide();
        });

        this._elem
            .on("shown.bs.modal", () => {
                $("#textEditor_divEditor textarea").focus();
            })
            .on("hidden.bs.modal", () => {
                this._invokeCallback();
            });
    }

    _showLanguage(language) {
        let lblLanguage = $("#textEditor_lblLanguage");

        if (language === "css") {
            lblLanguage.text("CSS").removeClass("d-none");
        } else if (language === "js") {
            lblLanguage.text("JavaScript").removeClass("d-none");
        } else {
            lblLanguage.text("Text").addClass("d-none");
        }
    }

    show(text, options, callback) {
        this._context = new ModalContext({
            oldValue: text,
            callback: callback
        });

        options ??= TextEditor.DEFAULT_OPTIONS;
        this._showLanguage(options.language);
        this._flask.updateLanguage(options.language);
        this._flask.updateCode(text);
        this._modal.show();
    }
}

// Contains classes: PropGrid, PropGridEventType, PropGridHelper, PropGridDialogs,
//     ProxyObject, PointProxy, SizeProxy, UnionObject
// Depends on jquery, tweakpane, tweakpane-plugin-essentials, scada-common.js, mimic-model.js, mimic-descr.js

// Interacts with Tweakpane to provide property grid functionality.
class PropGrid {
    _tweakpaneElem;
    _tweakpane;
    _phrases;
    _eventSource = document.createElement("prop-grid");
    _selectedObject = null;
    _topObject = null;
    _topPropertyName = "";
    _parentStack = [];

    constructor(elemID, phrases) {
        this._tweakpaneElem = $("#" + elemID);
        this._tweakpane = new Tweakpane({
            container: this._tweakpaneElem[0]
        });
        this._tweakpane.registerPlugin(TweakpaneEssentialsPlugin);
        this._phrases = phrases ?? {};
        this._bindEvents();
    }

    _bindEvents() {
        this._tweakpaneElem.on("click", ".rs-array-item", (event) => {
            // select the clicked array item
            this._tweakpaneElem.find(".rs-array-item").removeClass("rs-selected");
            $(event.currentTarget).addClass("rs-selected");
        });
    }

    _selectObject(obj) {
        this._selectedObject = obj;
        this._parentStack = [];
        this._topObject = obj;
        this._topPropertyName = "";
        this._showObjectProperties(obj, false);
    }

    _selectChildObject(propertyName, obj) {
        let parent = this._selectedObject;

        if (parent === this._topObject) {
            this._topPropertyName = propertyName;
        }

        this._selectedObject = obj;
        this._parentStack.push(parent);
        this._showObjectProperties(obj, true);
    }

    _selectParentObject() {
        let parent = this._parentStack.pop();
        let isChild = this._parentStack.length > 0;

        if (!isChild) {
            this._topPropertyName = "";
        }

        this._selectedObject = parent;
        this._showObjectProperties(parent, isChild);
    }

    _showObjectProperties(obj, isChild) {
        this._clearPane();
        let targetObject = PropGridHelper.getTargetObject(obj);
        let descriptor = PropGridHelper.getObjectDescriptor(obj);
        let folderMap = this._addFolders(descriptor);
        this._addBlades(folderMap, targetObject, isChild, descriptor);
    }

    _clearPane() {
        for (let child of this._tweakpane.children) {
            child.dispose();
        }
    }

    _addBlades(folderMap, targetObject, isChild, objectDescriptor) {
        if (targetObject) {
            if (Array.isArray(targetObject)) {
                // show array elements
                this._addArrayToolbar(targetObject);
                let index = 0;

                for (let [name, value] of Object.entries(targetObject)) {
                    let blade = this._addBlade(folderMap, targetObject, name, value, objectDescriptor);
                    this._prepareArrayBlade(blade, value, index);
                    index++;
                }
            } else {
                // show object properties
                let entries = Object.entries(targetObject);

                if (objectDescriptor && objectDescriptor.sorted) {
                    entries.sort(([nameA], [nameB]) => {
                        let displayNameA = objectDescriptor.get(nameA)?.displayName ?? nameA;
                        let displayNameB = objectDescriptor.get(nameB)?.displayName ?? nameB;
                        return displayNameA.localeCompare(displayNameB);
                    });
                }

                for (let [name, value] of entries) {
                    this._addBlade(folderMap, targetObject, name, value, objectDescriptor);
                }
            }
        }

        // add the Back button
        if (isChild) {
            this._tweakpane
                .addButton({
                    title: this._phrases.backButton
                })
                .on("click", () => {
                    this._selectParentObject();
                });
        }
    }

    _addBlade(folderMap, targetObject, propertyName, propertyValue, objectDescriptor) {
        let propertyDescriptor = objectDescriptor?.get(propertyName);

        if (propertyDescriptor && !propertyDescriptor.isBrowsable) {
            return;
        }

        let blade = null;
        let container = this._selectContainer(folderMap, propertyDescriptor);

        if (PropGridDialogs.editorSupported(propertyDescriptor)) {
            // property editor called by button click
            blade = container
                .addButton({
                    label: propertyDescriptor.displayName,
                    title: this._getEditButtonText(propertyValue)
                })
                .on("click", () => {
                    PropGridDialogs.showEditor(propertyValue, propertyDescriptor, (newPropertyValue) => {
                        propertyValue = newPropertyValue;
                        blade.title = this._getEditButtonText(propertyValue);
                        targetObject[propertyName] = propertyValue;
                        this._handleBindingChange(targetObject, propertyName, propertyValue);
                    });
                });
        } else if (typeof propertyValue === "number" ||
            typeof propertyValue === "string" ||
            typeof propertyValue === "boolean") {
            // simple property is editable in row
            blade = container
                .addBinding(targetObject, propertyName, this._getBindingOptions(propertyDescriptor))
                .on("change", (event) => {
                    if (event.last) {
                        this._handleBindingChange(targetObject, propertyName, event.value);
                    }
                });
        } else if (propertyValue instanceof Object) {
            let proxyObject = this._createProxyObject(propertyValue, propertyDescriptor);

            if (proxyObject) {
                // use proxy object
                blade = container
                    .addBinding({ [propertyName]: proxyObject }, propertyName,
                        this._getBindingOptions(propertyDescriptor))
                    .on("change", (event) => {
                        if (event.last) {
                            this._handleBindingChange(targetObject, propertyName, event.value);
                        }
                    });
            } else {
                // complex property requires braking into simple properties
                blade = container
                    .addButton({
                        label: propertyDescriptor ? propertyDescriptor.displayName : propertyName,
                        title: this._phrases.editButton
                    })
                    .on("click", () => {
                        this._selectChildObject(propertyName, propertyValue);
                    });
            }
        }

        return blade;
    }

    _addFolders(objectDescriptor) {
        let folderMap = new Map();

        if (objectDescriptor) {
            // get distinct categories
            let categorySet = new Set();

            for (let propertyDescriptor of objectDescriptor.propertyDescriptors.values()) {
                if (propertyDescriptor.isBrowsable && propertyDescriptor.category) {
                    categorySet.add(propertyDescriptor.category);
                }
            }

            // create folders
            for (let category of Array.from(categorySet).sort()) {
                folderMap.set(category, this._tweakpane.addFolder({
                    title: category
                }));
            }
        }

        return folderMap;
    }

    _selectContainer(folderMap, propertyDescriptor) {
        return propertyDescriptor && propertyDescriptor.category
            ? folderMap.get(propertyDescriptor.category) ?? this._tweakpane
            : this._tweakpane;
    }

    _getEditButtonText(propertyValue) {
        const MaxLength = 20;
        let text = propertyValue ? propertyValue.toString().trimStart() : "";

        if (text) {
            return text.length > MaxLength
                ? text.substring(0, MaxLength) + "..."
                : text;
        }

        return this._phrases.editButton;
    }

    _createProxyObject(propertyValue, propertyDescriptor) {
        if (!propertyDescriptor) {
            return null;
        }

        const BasicType = rs.mimic.BasicType;
        const Subtype = rs.mimic.Subtype;
        let proxy = null;

        if (propertyDescriptor.type === BasicType.STRUCT) {
            switch (propertyDescriptor.subtype) {
                case Subtype.POINT:
                    proxy = new PointProxy(propertyValue);
                    break;

                case Subtype.SIZE:
                    proxy = new SizeProxy(propertyValue);
                    break;
            }
        }

        return proxy;
    }

    _getBindingOptions(propertyDescriptor) {
        if (!propertyDescriptor) {
            return null;
        }

        const BasicType = rs.mimic.BasicType;
        const Subtype = rs.mimic.Subtype;

        let bindingOptions = {
            label: propertyDescriptor.displayName
        };

        if (propertyDescriptor.isReadOnly) {
            bindingOptions.readonly = true;
            bindingOptions.interval = ScadaUtils.MS_PER_DAY;
        }

        switch (propertyDescriptor.type) {
            case BasicType.INT:
                bindingOptions.format = (v) => v.toFixed();
                bindingOptions.step = 1;
                break;

            case BasicType.STRING:
                bindingOptions.view = "text";
                break;

            case BasicType.STRUCT:
                if (propertyDescriptor.subtype === Subtype.POINT ||
                    propertyDescriptor.subtype === Subtype.SIZE) {
                    bindingOptions.x = { step: 1 };
                    bindingOptions.y = { step: 1 };
                }
                break;
        }

        Object.assign(bindingOptions, propertyDescriptor.tweakpaneOptions);
        return bindingOptions;
    }

    _handleBindingChange(targetObject, propertyName, propertyValue) {
        // get value from proxy object
        if (propertyValue instanceof ProxyObject) {
            propertyValue = propertyValue.target;
        }

        // get top property name and value
        let topTargetObject = PropGridHelper.getTargetObject(this._topObject);
        let topPropertyName = "";
        let topPropertyValue = null;

        if (topTargetObject === targetObject) {
            topPropertyName = propertyName;
            topPropertyValue = propertyValue;
        } else if (topTargetObject) {
            topPropertyName = this._topPropertyName;
            topPropertyValue = topTargetObject[this._topPropertyName];
        }

        // update union object
        if (this._topObject instanceof UnionObject) {
            this._topObject.setProperty(topPropertyName, topPropertyValue);
        }

        // call event
        this._eventSource.dispatchEvent(new CustomEvent(PropGridEventType.PROPERTY_CHANGED, {
            detail: {
                selectedObject: this._selectedObject,
                topObject: this._topObject,
                targetObject: targetObject,
                propertyName: propertyName,
                propertyValue: propertyValue,
                topPropertyName: topPropertyName,
                topPropertyValue: topPropertyValue
            }
        }));
    }

    _handleError(message) {
        console.error(message);

        this._eventSource.dispatchEvent(new CustomEvent(PropGridEventType.ERROR, {
            detail: {
                message: message
            }
        }));
    }

    _addArrayToolbar(array) {
        this._tweakpane.addBlade({
            view: "buttongrid",
            size: [4, 1],
            cells: (x, y) => ({
                title: [[
                    this._phrases.addButton,
                    this._phrases.upButton,
                    this._phrases.downButton,
                    this._phrases.deleteButton
                ]][y][x]
            })
        }).on("click", (event) => {
            switch (event.index[0]) {
                case 0:
                    this._addArrayItem(array);
                    break;
                case 1:
                    this._moveUpArrayItem(array);
                    break;
                case 2:
                    this._moveDownArrayItem(array);
                    break;
                case 3:
                    this._deleteArrayItem(array);
                    break;
            }
        });
    }

    _prepareArrayBlade(blade, item, index) {
        if (blade) {
            blade.label = item.displayName || this._phrases.arrayItem + index;
            $(blade.element).addClass("rs-array-item").attr("data-rs-index", index);
        }
    }

    _getSelectedIndex() {
        let index = this._tweakpaneElem.find(".rs-array-item.rs-selected:first").data("rs-index");
        return index >= 0 ? index : -1;
    }

    _setSelectedIndex(index) {
        let itemElems = this._tweakpaneElem.find(".rs-array-item");
        itemElems.removeClass("rs-selected");
        itemElems.filter(`[data-rs-index="${index}"]`).addClass("rs-selected");
    }

    _addArrayItem(array) {
        let itemAdded = false;

        if (array.createItem instanceof Function) {
            let item = array.createItem();

            if (item !== undefined && item !== null) {
                let index = this._getSelectedIndex();

                if (index < 0) {
                    index = array.length;
                    array.push(item);
                } else {
                    index++;
                    array.splice(index, 0, item);
                }

                this._handleArrayChange(array);
                this.refresh();
                this._setSelectedIndex(index);
                itemAdded = true;
            }
        }

        if (!itemAdded) {
            this._handleError(this._phrases.unableAddItem);
        }
    }

    _moveUpArrayItem(array) {
        let index = this._getSelectedIndex();

        if (index > 0) {
            [array[index - 1], array[index]] = [array[index], array[index - 1]];
            this._handleArrayChange(array);
            this.refresh();
            this._setSelectedIndex(index - 1);
        } else {
            this._handleError(this._phrases.unableMoveItem);
        }
    }

    _moveDownArrayItem(array) {
        let index = this._getSelectedIndex();

        if (0 <= index && index < array.length - 1) {
            [array[index], array[index + 1]] = [array[index + 1], array[index]];
            this._handleArrayChange(array);
            this.refresh();
            this._setSelectedIndex(index + 1);
        } else {
            this._handleError(this._phrases.unableMoveItem);
        }
    }

    _deleteArrayItem(array) {
        let index = this._getSelectedIndex();

        if (index >= 0) {
            array.splice(index, 1);
            this._handleArrayChange(array);
            this.refresh();
        } else {
            this._handleError(this._phrases.unableDeleteItem);
        }
    }

    _handleArrayChange(array) {
        this._handleBindingChange(array, "", null);
    }

    get selectedObject() {
        return this._selectedObject;
    }

    set selectedObject(value) {
        if (this._selectedObject !== value) {
            this._selectObject(value);
        }
    }

    get selectedObjects() {
        return this._selectedObject instanceof UnionObject
            ? this._selectedObject.targets
            : [this._selectedObject];
    }

    set selectedObjects(value) {
        if (Array.isArray(value)) {
            if (value.length === 0) {
                this.selectedObject = null;
            } else if (value.length === 1) {
                this.selectedObject = value[0];
            } else {
                this.selectedObject = new UnionObject(value);
            }
        }
    }

    addEventListener(type, listener) {
        this._eventSource.addEventListener(type, listener);
    }

    refreshProperty(propertyName) {
        for (let folder of this._tweakpane.children) {
            for (let binding of folder.children) {
                if (binding.key === propertyName) {
                    binding.refresh();
                    return;
                }
            }
        }
    }

    refresh(opt_backToTop) {
        if (this._selectedObject instanceof UnionObject) {
            let newUnion = new UnionObject(this._selectedObject.targets);
            this._selectObject(newUnion);
        } else if (opt_backToTop) {
            this._selectObject(this._topObject);
        } else {
            let isChild = parent !== this._topObject;
            this._showObjectProperties(this._selectedObject, isChild);
        }
    }
}

// Specifies the event types for property grid.
class PropGridEventType {
    static ERROR = "error";
    static PROPERTY_CHANGED = "propertyChanged";
}

// Provides helper methods for property grid.
class PropGridHelper {
    static _translateObject(objectDescriptor, translation, objectDict, opt_fallbackDict) {
        if (!objectDict) {
            return;
        }

        const BasicType = rs.mimic.BasicType;

        for (let propertyDescriptor of objectDescriptor.propertyDescriptors.values()) {
            // translate display name and category
            let displayName = objectDict[propertyDescriptor.name] ??
                (opt_fallbackDict ? opt_fallbackDict[propertyDescriptor.name] : "");
            let category = translation.category[propertyDescriptor.category];

            if (displayName) {
                propertyDescriptor.displayName = displayName;
            }

            if (category) {
                propertyDescriptor.category = category;
            }

            // translate enumeration
            if (propertyDescriptor.type === BasicType.ENUM) {
                let enumDict = translation.enumerations.get(propertyDescriptor.subtype);

                if (enumDict) {
                    propertyDescriptor.tweakpaneOptions ??= {};
                    propertyDescriptor.tweakpaneOptions.options ??= enumDict;
                }
            }
        }
    }

    static translateDescriptors(translation) {
        const DescriptorSet = rs.mimic.DescriptorSet;

        // translate mimic and faceplates
        PropGridHelper._translateObject(DescriptorSet.mimicDescriptor, translation, translation.mimic);
        PropGridHelper._translateObject(DescriptorSet.faceplateDescriptor, translation, translation.component);

        // translate components
        for (let [typeName, descriptor] of DescriptorSet.componentDescriptors) {
            PropGridHelper._translateObject(descriptor, translation,
                translation.components.get(typeName), translation.component);
        }

        // translate structures
        for (let [typeName, descriptor] of DescriptorSet.structureDescriptors) {
            PropGridHelper._translateObject(descriptor, translation, translation.structures.get(typeName));
        }
    }

    static getTargetObject(obj) {
        if (obj instanceof rs.mimic.Mimic) {
            return obj.document;
        } else if (obj instanceof rs.mimic.Component) {
            return obj.properties;
        } else if (obj instanceof UnionObject) {
            return obj.properties;
        } else if (obj instanceof Object) {
            return obj;
        } else {
            return null;
        }
    }

    static getObjectDescriptor(obj) {
        const DescriptorSet = rs.mimic.DescriptorSet;

        if (obj instanceof rs.mimic.FaceplateInstance) {
            return DescriptorSet.faceplateDescriptor;
        } else if (obj instanceof rs.mimic.Component) {
            return DescriptorSet.componentDescriptors.get(obj.typeName);
        } else if (obj instanceof rs.mimic.Mimic) {
            return DescriptorSet.mimicDescriptor;
        } else if (obj instanceof UnionObject) {
            return obj.descriptor;
        } else if (obj instanceof Object && obj.typeName) {
            return DescriptorSet.structureDescriptors.get(obj.typeName);
        } else {
            return null;
        }
    }
}

// Calls property editors implemented as modal dialogs.
class PropGridDialogs {
    static colorDialog = null;
    static fontDialog = null;
    static imageDialog = null;
    static propertyDialog = null;
    static textEditor = null;

    // Shows the color dialog.
    static _showColorDialog(value, options, callback) {
    }

    // Shows the font dialog.
    static _showFontDialog(value, options, callback) {
    }

    // Shows the image dialog.
    static _showImageDialog(value, options, callback) {
    }

    // Shows the property dialog.
    static _showPropertyDialog(value, options, callback) {
    }

    // Shows the text editor.
    static _showTextEditor(value, options, callback) {
        PropGridDialogs.textEditor.show(value, options, (modalContext) => {
            PropGridDialogs._invokeCallback(modalContext, callback);
        });
    }

    // Invokes the callback function.
    static _invokeCallback(modalContext, callback) {
        if (modalContext.result && callback instanceof Function) {
            callback(modalContext.newValue);
        }
    }

    // Checks whether an editor for the specified property is supported.
    static editorSupported(propertyDescriptor) {
        const PropertyEditor = rs.mimic.PropertyEditor;
        let editor = propertyDescriptor?.editor;
        return editor &&
            editor === PropertyEditor.COLOR_DIALOG && PropGridDialogs.colorDialog ||
            editor === PropertyEditor.FONT_DIALOG && PropGridDialogs.fontDialog ||
            editor === PropertyEditor.IMAGE_DIALOG && PropGridDialogs.imageDialog ||
            editor === PropertyEditor.PROPERTY_DIALOG && PropGridDialogs.propertyDialog ||
            editor === PropertyEditor.TEXT_EDITOR && PropGridDialogs.textEditor;
    }

    // Shows an editor as a modal dialog.
    // callback is a function (newPropertyValue)
    static showEditor(propertyValue, propertyDescriptor, callback) {
        if (propertyDescriptor) {
            const PropertyEditor = rs.mimic.PropertyEditor;
            let editor = propertyDescriptor.editor;
            let options = propertyDescriptor.editorOptions;

            if (editor === PropertyEditor.COLOR_DIALOG) {
                if (PropGridDialogs.colorDialog) {
                    PropGridDialogs._showColorDialog(propertyValue, options, callback);
                }
            } else if (editor === PropertyEditor.FONT_DIALOG) {
                if (PropGridDialogs.fontDialog) {
                    PropGridDialogs._showFontDialog(propertyValue, options, callback);
                }
            } else if (editor === PropertyEditor.IMAGE_DIALOG) {
                if (PropGridDialogs.imageDialog) {
                    PropGridDialogs._showImageDialog(propertyValue, options, callback);
                }
            } else if (editor === PropertyEditor.PROPERTY_DIALOG) {
                if (PropGridDialogs.propertyDialog) {
                    PropGridDialogs._showPropertyDialog(propertyValue, options, callback);
                }
            } else if (editor === PropertyEditor.TEXT_EDITOR) {
                if (PropGridDialogs.textEditor) {
                    PropGridDialogs._showTextEditor(propertyValue, options, callback);
                }
            }
        }
    }
}

// Represents an intermediate object intended for editing.
class ProxyObject {
    target;

    constructor(target) {
        if (target) {
            this.target = target;
        } else {
            throw new Error("Target must not be null.");
        }
    }
}

// Represents a proxy object for editing point as a Point2d.
class PointProxy extends ProxyObject {
    get x() {
        return parseInt(this.target.x) || 0;
    }

    set x(value) {
        this.target.x = value;
    }

    get y() {
        return parseInt(this.target.y) || 0;
    }

    set y(value) {
        this.target.y = value;
    }
}

// Represents a proxy object for editing size as a Point2d.
class SizeProxy extends ProxyObject {
    get x() {
        return parseInt(this.target.width) || 0;
    }

    set x(value) {
        this.target.width = value;
    }

    get y() {
        return parseInt(this.target.height) || 0;
    }

    set y(value) {
        this.target.height = value;
    }
}

// Represents an intermediate object for editing multiple objects.
class UnionObject {
    targets;    // objects included in the union
    properties; // common properties
    descriptor; // describes the union properties

    constructor(targets) {
        if (Array.isArray(targets)) {
            this.targets = targets;
        } else {
            throw new Error("Targets must be an array.");
        }

        this._buildProperties();
    }

    _buildProperties() {
        this.properties = {};
        this.descriptor = new rs.mimic.ObjectDescriptor();
        let index = 0;

        for (let target of this.targets) {
            let targetDescriptor = PropGridHelper.getObjectDescriptor(target);
            let editableObj = this._getEditableObject(target);

            if (index === 0) {
                // add properties of the 1st object
                for (let [name, value] of Object.entries(editableObj)) {
                    this.properties[name] = ScadaUtils.deepClone(value, true);
                    let propertyDescriptor = targetDescriptor.get(name);

                    if (propertyDescriptor && propertyDescriptor.type !== rs.mimic.BasicType.LIST) {
                        this.descriptor.add(propertyDescriptor);
                    }
                }
            } else {
                // intersect with properties of other objects
                for (let [name, value] of Object.entries(this.properties)) {
                    let descriptor1 = this.descriptor.get(name);
                    let descriptor2 = targetDescriptor.get(name);

                    if (editableObj.hasOwnProperty(name) && this._sameProperties(descriptor1, descriptor2)) {
                        let value2 = editableObj[name];

                        if (!this._sameValues(value, value2)) {
                            // objects have the same property with different values
                            this.properties[name] = this._mergeValues(value, value2);
                        }
                    } else {
                        delete this.properties[name];
                        this.descriptor.delete(name);
                    }
                }
            }

            index++;
        }
    }

    _getEditableObject(target) {
        if (target instanceof rs.mimic.Mimic) {
            return target.document;
        } else if (target instanceof rs.mimic.Component) {
            return target.properties;
        } else if (target instanceof Object) {
            return target;
        } else {
            return {};
        }
    }

    _sameProperties(descriptor1, descriptor2) {
        return descriptor1 === descriptor2 ||
            descriptor1 && descriptor2 && descriptor1.type === descriptor2.type &&
            descriptor1.subtype === descriptor2.subtype;
    }

    _sameValues(value1, value2) {
        let json1 = JSON.stringify(value1);
        let json2 = JSON.stringify(value2);
        return json1 === json2;
    }

    _mergeValues(value1, value2) {
        if (Array.isArray(value1)) {
            return null; // do not display array properties
        } else if (typeof value1 === "number") {
            return value1 === value2 ? value1 : 0;
        } else if (typeof value1 === "string") {
            return value1 === value2 ? value1 : "";
        } else if (typeof value1 === "boolean") {
            return value1 === value2 ? value1 : false;
        } else if (value1 instanceof Object) {
            let result = {};

            for (let [name, value] of Object.entries(value1)) {
                result[name] = this._mergeValues(value, value2[name]);
            }

            return result;
        } else {
            return null; // property is not displayed
        }
    }

    setProperty(name, value) {
        for (let target of this.targets) {
            let editableObj = this._getEditableObject(target);
            editableObj[name] = ScadaUtils.deepClone(value, true);
        }
    }

    toString() {
        return "Union";
    }
}

// Contains classes: StructTree, StructTreeEventType
// Depends on jquery, bootstrap, mimic-model.js

// Represents a component for displaying mimic structure.
class StructTree {
    _eventSource = document.createElement("struct-tree");

    structElem;
    mimic;
    phrases;

    constructor(elemID, mimic, phrases) {
        this.structElem = $("#" + elemID);
        this.mimic = mimic;
        this.phrases = phrases ?? {};
    }

    _prepareDependencies(listElem) {
        let dependenciesNode = $("<span class='node node-dependencies'></span>");
        $("<span class='node-text'></span>").text(this.phrases.dependenciesNode).appendTo(dependenciesNode);
        $("<span class='node-btn add-btn'><i class='fa-solid fa-plus'></i></span>").appendTo(dependenciesNode);

        let dependenciesItem = $("<li class='item-dependencies'></li>").append(dependenciesNode).appendTo(listElem);
        let dependenciesList = $("<ul class='list-dependencies'></ul>").appendTo(dependenciesItem);
        this._appendDependencies(dependenciesList);
    }

    _appendDependencies(listElem) {
        for (let dependency of this.mimic.dependencies) {
            if (!dependency.isTransitive) {
                let dependencyNode = $("<span class='node node-dependency'></span>");
                $("<span class='node-text'></span>").text(dependency.typeName)
                    .appendTo(dependencyNode);
                $("<span class='node-btn edit-btn'><i class='fa-solid fa-pen-to-square'></i></span>")
                    .appendTo(dependencyNode);
                $("<span class='node-btn remove-btn'><i class='fa-regular fa-trash-can'></i></span>")
                    .appendTo(dependencyNode);
                $("<li class='item-dependency'></li>")
                    .attr("data-name", dependency.typeName)
                    .addClass(dependency.hasError ? "has-error" : "")
                    .append(dependencyNode).appendTo(listElem);
            }
        }
    }

    _prepareImages(listElem) {
        let imagesNode = $("<span class='node node-images'></span>");
        $("<span class='node-text'></span>").text(this.phrases.imagesNode).appendTo(imagesNode);
        $("<span class='node-btn add-btn'><i class='fa-solid fa-plus'></i></span>").appendTo(imagesNode);

        let imagesItem = $("<li class='item-images'></li>").append(imagesNode).appendTo(listElem);
        let imagesList = $("<ul class='list-images'></ul>").appendTo(imagesItem);
        this._appendImages(imagesList);
    }

    _appendImages(listElem) {
        for (let image of this.mimic.images) {
            let imageNode = $("<span class='node node-image'></span>");
            $("<span class='node-text'></span>").text(image.name)
                .appendTo(imageNode);
            let viewBtn = $("<span class='node-btn view-btn'><i class='fa-regular fa-eye'></i></span>")
                .appendTo(imageNode);
            $("<span class='node-btn edit-btn'><i class='fa-solid fa-pen-to-square'></i></span>")
                .appendTo(imageNode);
            $("<span class='node-btn remove-btn'><i class='fa-regular fa-trash-can'></i></span>")
                .appendTo(imageNode);
            $("<li class='item-image'></li>").attr("data-name", image.name)
                .append(imageNode).appendTo(listElem);
            this._initImagePopover(viewBtn, image.name);
        }
    }

    _initImagePopover(buttonElem, imageName) {
        const thisObj = this;

        bootstrap.Popover.getOrCreateInstance(buttonElem[0], {
            html: true,
            placement: "bottom",
            content: function () {
                // called twice by Bootstrap on each show
                let popoverContent = buttonElem.data("popoverContent");

                if (!popoverContent) {
                    let dataUrl = thisObj.mimic.imageMap.get(imageName)?.dataUrl;
                    popoverContent = dataUrl
                        ? `<img class="image-preview" src="${dataUrl}" />`
                        : thisObj.phrases.noImagePreview;
                    buttonElem.data("popoverContent", popoverContent);
                }

                return popoverContent;
            }
        });
    }

    _prepareComponents(listElem) {
        let mimicNode = $("<span class='node node-mimic'></span>").text(this.phrases.mimicNode);
        let mimicItem = $("<li class='item-mimic'></li>").append(mimicNode).appendTo(listElem);
        let componentsList = $("<ul class='list-components'></ul>").appendTo(mimicItem);
        this._appendComponents(componentsList);
    }

    _appendComponents(listElem) {
        for (let component of this.mimic.children) {
            this._appendComponent(listElem, component);
        }
    }

    _appendComponent(listElem, component) {
        let componentNode = $("<span class='node node-comp'></span>").text(component.displayName);
        let componentItem = $("<li class='item-comp'></li>")
            .attr("id", "struct-comp-item" + component.id)
            .attr("data-id", component.id)
            .append(componentNode).appendTo(listElem);

        if (component.isContainer) {
            let childList = $("<ul></ul>").appendTo(componentItem);

            for (let childComponent of component.children) {
                this._appendComponent(childList, childComponent);
            }
        }

        if (component.isSelected) {
            componentNode.addClass("selected");
        }
    }

    _bindEvents(listElem) {
        const thisObj = this;

        // dependencies
        listElem.find(".item-dependencies")
            .on("click", ".add-btn", function () {
                thisObj._eventSource.dispatchEvent(new CustomEvent(StructTreeEventType.ADD_DEPENDENCY_CLICK));
            })
            .on("click", ".edit-btn", function () {
                thisObj._eventSource.dispatchEvent(new CustomEvent(StructTreeEventType.EDIT_DEPENDENCY_CLICK, {
                    detail: { name: $(this).closest(".item-dependency").data("name") }
                }));
            })
            .on("click", ".remove-btn", function () {
                thisObj._eventSource.dispatchEvent(new CustomEvent(StructTreeEventType.REMOVE_DEPENDENCY_CLICK, {
                    detail: { name: $(this).closest(".item-dependency").data("name") }
                }));
            });

        // images
        listElem.find(".item-images")
            .on("click", ".add-btn", function () {
                thisObj._eventSource.dispatchEvent(new CustomEvent(StructTreeEventType.ADD_IMAGE_CLICK));
            })
            .on("click", ".edit-btn", function () {
                thisObj._eventSource.dispatchEvent(new CustomEvent(StructTreeEventType.EDIT_IMAGE_CLICK, {
                    detail: { name: $(this).closest(".item-image").data("name") }
                }));
            })
            .on("click", ".remove-btn", function () {
                thisObj._eventSource.dispatchEvent(new CustomEvent(StructTreeEventType.REMOVE_IMAGE_CLICK, {
                    detail: { name: $(this).closest(".item-image").data("name") }
                }));
            });

        // mimic and components
        listElem.find(".item-mimic")
            .on("click", ".node-mimic", function () {
                thisObj._eventSource.dispatchEvent(new CustomEvent(StructTreeEventType.MIMIC_CLICK));
            })
            .on("click", ".node-comp", function (event) {
                thisObj._eventSource.dispatchEvent(new CustomEvent(StructTreeEventType.COMPONENT_CLICK, {
                    detail: {
                        componentID: $(this).parent().data("id"),
                        isSelected: $(this).hasClass("selected"),
                        ctrlKey: event.ctrlKey
                    }
                }));
            });
    }

    _findMimicItem() {
        return this.structElem.find(".item-mimic");
    }

    _findComponentItem(component) {
        return this.structElem.find("#struct-comp-item" + component.id);
    }

    _findComponentNode(component) {
        return this._findComponentItem(component).children(".node");
    }

    build() {
        let listElem = $("<ul class='list-top'></ul>");
        this._prepareDependencies(listElem);
        this._prepareImages(listElem);
        this._prepareComponents(listElem);
        this._bindEvents(listElem);

        // add new or replace existing element
        let oldListElem = this.structElem.find(".list-top:first");

        if (oldListElem.length > 0) {
            oldListElem.replaceWith(listElem);
        } else {
            this.structElem.append(listElem);
        }
    }

    refreshDependencies() {
        let oldDependenciesList = this.structElem.find(".list-dependencies:first");
        let newDependenciesList = $("<ul class='list-dependencies'></ul>");
        this._appendDependencies(newDependenciesList);
        oldDependenciesList.replaceWith(newDependenciesList);
    }

    refreshImages() {
        let oldImagesList = this.structElem.find(".list-images:first");
        let newImagesList = $("<ul class='list-images'></ul>");
        this._appendImages(newImagesList);
        oldImagesList.replaceWith(newImagesList);
    }

    refreshComponents(parent) {
        if (parent instanceof rs.mimic.Mimic) {
            // refresh all components
            let oldListElem = this.structElem.find(".list-components:first");
            let newListElem = $("<ul class='list-components'></ul>");
            this._appendComponents(newListElem)
            oldListElem.replaceWith(newListElem);
        } else if (parent instanceof rs.mimic.Component) {
            // refresh components belongs to the parent
            let parentItem = this._findComponentItem(parent);
            let oldListElem = parentItem.children("ul:first");
            let newListElem = $("<ul></ul>");

            for (let childComponent of parent.children) {
                this._appendComponent(newListElem, childComponent);
            }

            oldListElem.replaceWith(newListElem);
        }
    }

    addComponent(component) {
        let listElem = component.parentID > 0
            ? this.structElem.find(`#struct-comp-item${component.parentID}>ul`) 
            : this.structElem.find(".item-mimic>ul");

        if (listElem.length > 0) {
            this._appendComponent(listElem, component);
        }
    }

    updateComponent(component) {
        this._findComponentNode(component).text(component.displayName);
    }

    removeComponent(componentID) {
        this.structElem.find("#struct-comp-item" + componentID).remove();
    }

    selectMimic() {
        let mimicItem = this._findMimicItem();
        mimicItem.children(".node").addClass("selected");
        mimicItem.children("ul").find(".node").removeClass("selected");
    }

    selectNone() {
        this._findMimicItem().find(".node").removeClass("selected");
    }

    selectComponents(components) {
        this.selectNone();

        if (Array.isArray(components)) {
            for (let component of components) {
                this._findComponentNode(component).addClass("selected");
            }
        }
    }

    addToSelection(component) {
        this._findMimicItem().children(".node").removeClass("selected");
        this._findComponentNode(component).addClass("selected");
    }

    removeFromSelection(component) {
        this._findComponentNode(component).removeClass("selected");
    }

    addEventListener(type, listener) {
        this._eventSource.addEventListener(type, listener);
    }
}

// Specifies the event types for a mimic structure component.
class StructTreeEventType {
    static ADD_DEPENDENCY_CLICK = "addDependencyClick";
    static EDIT_DEPENDENCY_CLICK = "editDependencyClick";
    static REMOVE_DEPENDENCY_CLICK = "removeDependencyClick";

    static ADD_IMAGE_CLICK = "addImageClick";
    static EDIT_IMAGE_CLICK = "editImageClick";
    static REMOVE_IMAGE_CLICK = "removeImageClick";

    static MIMIC_CLICK = "mimicClick";
    static COMPONENT_CLICK = "componentClick";
}
