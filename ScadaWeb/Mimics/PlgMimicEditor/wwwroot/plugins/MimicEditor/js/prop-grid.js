// Contains classes: PropGrid, PropGridEventType, PropGridHelper, ProxyObject, PointProxy, SizeProxy, UnionObject
// Depends on tweakpane, scada-common.js, mimic-model.js, mimic-descr.js

// Interacts with Tweakpane to provide property grid functionality.
class PropGrid {
    _pane; // Tweakpane
    _elem = document.createElement("propgrid");
    _selectedObject = null;
    _parentStack = [];

    constructor(pane) {
        this._pane = pane;
    }

    _selectObject(obj) {
        this._selectedObject = obj;
        this._parentStack = [];
        this._showObjectProperties(obj, null);
    }

    _selectChildObject(obj, parent) {
        this._selectedObject = obj;
        this._parentStack.push(parent);
        this._showObjectProperties(obj, parent);
    }

    _selectParentObject() {
        let parent = this._parentStack.pop();
        let grandParent = this._parentStack.at(-1); // last
        this._selectedObject = parent;
        this._showObjectProperties(parent, grandParent);
    }

    _showObjectProperties(obj, parent) {
        this._clearPane();
        let descriptor = PropGridHelper.getObjectDescriptor(obj);
        let folderMap = this._addFolders(descriptor);

        if (obj instanceof rs.mimic.Mimic) {
            this._addBlades(folderMap, obj.document, parent, descriptor);
        } else if (obj instanceof rs.mimic.Component) {
            this._addBlade(folderMap, obj, "id", obj.id, descriptor);
            this._addBlade(folderMap, obj, "name", obj.name, descriptor);
            this._addBlade(folderMap, obj, "typeName", obj.typeName, descriptor);
            this._addBlades(folderMap, obj.properties, parent, descriptor);
        } else if (obj instanceof UnionObject) {
            this._addBlades(folderMap, obj.properties, parent, descriptor);
        } else if (obj instanceof Object) {
            this._addBlades(folderMap, obj, parent, descriptor);
        }
    }

    _clearPane() {
        for (let child of this._pane.children) {
            child.dispose();
        }
    }

    _addBlades(folderMap, target, parent, objectDescriptor) {
        const thisObj = this;

        if (target) {
            for (let [name, value] of Object.entries(target)) {
                this._addBlade(folderMap, target, name, value, objectDescriptor);
            }
        }

        if (parent) {
            this._pane
                .addButton({
                    title: "Return to Parent"
                })
                .on("click", function () {
                    thisObj._selectParentObject();
                });
        }
    }

    _addBlade(folderMap, target, propertyName, propertyValue, objectDescriptor) {
        let propertyDescriptor = objectDescriptor?.get(propertyName);

        if (propertyDescriptor && !propertyDescriptor.isBrowsable) {
            return;
        }

        const thisObj = this;
        const selObj = this._selectedObject;
        let container = this._selectContainer(folderMap, propertyDescriptor);

        if (typeof propertyValue === "number" ||
            typeof propertyValue === "string") {
            // simple property is editable in row
            container
                .addBinding(target, propertyName, this._getBindingOptions(propertyDescriptor))
                .on("change", function (event) {
                    if (event.last) {
                        thisObj._handleBindingChange(selObj, target, propertyName, event.value);
                    }
                });
        } else if (propertyValue instanceof Object) {
            let proxyObject = this._createProxyObject(propertyValue, propertyDescriptor);

            if (proxyObject) {
                // use proxy object
                container
                    .addBinding({ [propertyName]: proxyObject }, propertyName,
                        this._getBindingOptions(propertyDescriptor))
                    .on("change", function (event) {
                        if (event.last) {
                            thisObj._handleBindingChange(selObj, target, propertyName, event.value);
                        }
                    });
            } else {
                // complex property requires braking into simple properties
                container
                    .addButton({
                        label: propertyDescriptor?.displayName ?? propertyName,
                        title: "Edit"
                    })
                    .on("click", function () {
                        thisObj._selectChildObject(propertyValue, selObj);
                    });
            }
        }
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
                folderMap.set(category, this._pane.addFolder({
                    title: category
                }));
            }
        }

        return folderMap;
    }

    _selectContainer(folderMap, propertyDescriptor) {
        return propertyDescriptor && propertyDescriptor.category
            ? folderMap.get(propertyDescriptor.category) ?? this._pane
            : this._pane;
    }

    _createProxyObject(target, propertyDescriptor) {
        const BasicType = rs.mimic.BasicType;
        let proxy = null;

        if (propertyDescriptor) {
            switch (propertyDescriptor.type) {
                case BasicType.POINT:
                    proxy = new PointProxy(target);
                    break;

                case BasicType.SIZE:
                    proxy = new SizeProxy(target);
                    break;
            }
        }

        return proxy;
    }

    _getBindingOptions(propertyDescriptor) {
        const BasicType = rs.mimic.BasicType;
        let bindingOptions = null;

        if (propertyDescriptor) {
            bindingOptions = {
                label: propertyDescriptor.displayName
            };

            if (propertyDescriptor.isReadOnly) {
                bindingOptions.readonly = true;
                bindingOptions.interval = ScadaUtils.MS_PER_DAY;
            }

            switch (propertyDescriptor.type) {
                case BasicType.INT:
                    bindingOptions.format = (v) => v.toFixed();
                    break;

                case BasicType.POINT:
                case BasicType.SIZE:
                    bindingOptions.x = { step: 1 };
                    bindingOptions.y = { step: 1 };
                    break;
            }

            if (propertyDescriptor.format instanceof Object) {
                Object.assign(bindingOptions, propertyDescriptor.format);
            }
        }

        return bindingOptions;
    }

    _handleBindingChange(selectedObject, changedObject, propertyName, value) {
        let targetValue = value instanceof ProxyObject ? value.target : value;

        if (selectedObject instanceof UnionObject) {
            selectedObject.setProperty(propertyName, targetValue);
        }

        this._elem.dispatchEvent(new CustomEvent(PropGridEventType.PROPERTY_CHANGED, {
            detail: {
                selectedObject: selectedObject,
                changedObject: changedObject,
                propertyName: propertyName,
                value: targetValue
            }
        }));
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
        this._elem.addEventListener(type, listener);
    }

    refreshProperty(propertyName) {
        for (let folder of this._pane.children) {
            for (let binding of folder.children) {
                if (binding.key === propertyName) {
                    binding.refresh();
                    return;
                }
            }
        }
    }

    refresh() {
        if (this._selectedObject instanceof UnionObject) {
            let newUnion = new UnionObject(this._selectedObject.targets);
            this._selectObject(newUnion);
        } else {
            this._selectObject(this._selectedObject);
        }
    }
}

// Specifies the event types for property grid.
class PropGridEventType {
    static PROPERTY_CHANGED = "propertyChanged";
}

// Provides helper methods for property grid.
class PropGridHelper {
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
        } else {
            return null;
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
        this.target.x = value.toString();
    }

    get y() {
        return parseInt(this.target.y) || 0;
    }

    set y(value) {
        this.target.y = value.toString();
    }
}

// Represents a proxy object for editing size as a Point2d.
class SizeProxy extends ProxyObject {
    get x() {
        return parseInt(this.target.width) || 0;
    }

    set x(value) {
        this.target.width = value.toString();
    }

    get y() {
        return parseInt(this.target.height) || 0;
    }

    set y(value) {
        this.target.height = value.toString();
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
                    this.properties[name] = ScadaUtils.deepClone(value);
                    let propertyDescriptor = targetDescriptor.get(name);

                    if (propertyDescriptor) {
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
        if (target instanceof rs.mimic.Component) {
            return target.properties;
        } else if (target instanceof rs.mimic.Mimic) {
            return target.document;
        } else if (target instanceof Object) {
            return target;
        } else {
            return null;
        }
    }

    _sameProperties(descriptor1, descriptor2) {
        return descriptor1 === descriptor2 ||
            descriptor1 && descriptor2 && descriptor1.type === descriptor2.type;
    }

    _sameValues(value1, value2) {
        let json1 = JSON.stringify(value1);
        let json2 = JSON.stringify(value2);
        return json1 === json2;
    }

    _mergeValues(value1, value2) {
        if (typeof value1 === "number") {
            return value1 === value2 ? value1 : 0;
        } else if (typeof value1 === "string") {
            return value1 === value2 ? value1 : "";
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
            editableObj[name] = ScadaUtils.deepClone(value);
        }
    }

    toString() {
        return "Union";
    }
}
