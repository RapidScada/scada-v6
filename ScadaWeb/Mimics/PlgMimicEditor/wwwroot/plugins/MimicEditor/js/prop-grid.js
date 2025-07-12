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
    static _translationRef = null;

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
        PropGridHelper._translationRef = translation;

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
            let descriptor = DescriptorSet.getFaceplateDescriptor(obj.model);
            let translation = PropGridHelper._translationRef;

            if (translation) {
                PropGridHelper._translateObject(descriptor, translation, translation.component);
            }

            return descriptor;
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
