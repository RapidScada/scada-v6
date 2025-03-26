﻿// Contains classes: PropGrid, ProxyObject, PointProxy
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
        let descriptor = this._getObjectDescriptor(obj);
        let folderMap = this._addFolders(descriptor);

        if (obj instanceof rs.mimic.Component) {
            this._addBlade(folderMap, obj, "id", obj.id, descriptor);
            this._addBlade(folderMap, obj, "name", obj.name, descriptor);
            this._addBlade(folderMap, obj, "typeName", obj.typeName, descriptor);
            this._addBlades(folderMap, obj.properties, null, descriptor);
        } else if (obj instanceof rs.mimic.Mimic) {
            this._addBlades(folderMap, obj.document, null, descriptor);
        } else if (obj) {
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

    _getObjectDescriptor(obj) {
        const DescriptorSet = rs.mimic.DescriptorSet;

        if (obj instanceof rs.mimic.Component) {
            return DescriptorSet.componentDescriptors.get(obj.typeName);
        } else if (obj instanceof rs.mimic.Mimic) {
            return DescriptorSet.mimicDescriptor;
        } else {
            return null;
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
        this._elem.dispatchEvent(new CustomEvent("propertyChanged", {
            detail: {
                selectedObject: selectedObject,
                changedObject: changedObject,
                propertyName: propertyName,
                value: value instanceof ProxyObject ? value.target : value
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
        return parseInt(this.target.x);
    }

    set x(value) {
        this.target.x = value.toString();
    }

    get y() {
        return parseInt(this.target.y);
    }

    set y(value) {
        this.target.y = value.toString();
    }
}

// Represents a proxy object for editing size as a Point2d.
class SizeProxy extends ProxyObject {
    get x() {
        return parseInt(this.target.width);
    }

    set x(value) {
        this.target.width = value.toString();
    }

    get y() {
        return parseInt(this.target.height);
    }

    set y(value) {
        this.target.height = value.toString();
    }
}
