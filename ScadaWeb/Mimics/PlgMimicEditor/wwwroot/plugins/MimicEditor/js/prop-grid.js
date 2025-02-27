// Contains classes: PropGrid, ProxyObject
// Depends on jquery, tweakpane, mimic-model.js, mimic-descr.js

// Interacts with Tweakpane to provide property grid functionality.
class PropGrid {
    _pane; // Tweakpane
    _selectedObject = null;
    _parentStack = [];
    _descriptorSet = new rs.mimic.DescriptorSet();

    constructor(pane) {
        this._pane = pane;
    }

    _clearPane() {
        for (let child of this._pane.children) {
            child.dispose();
        }
    }

    _addBlades(container, target, parent, descriptor) {
        const thisObj = this;

        if (target) {
            for (let [name, value] of Object.entries(target)) {
                this._addBlade(container, target, name, value, descriptor);
            }
        }

        if (parent) {
            container
                .addButton({
                    title: "Return to Parent"
                })
                .on("click", function () {
                    thisObj._selectParentObject();
                });
        }
    }

    _addBlade(container, target, propertyName, propertyValue, objectDescriptor) {
        const thisObj = this;
        let propertyDescriptor = objectDescriptor?.get(propertyName);
        let displayName = propertyDescriptor?.displayName ?? propertyName;

        if (typeof propertyValue === "number" ||
            typeof propertyValue === "string") {
            // simple property is editable in row
            container
                .addBinding(target, propertyName, {
                    label: displayName
                })
                .on("change", function (event) {
                    if (event.last) {
                        thisObj._handleBindingChange(target, propertyName, event.value);
                    }
                });
        } else if (propertyValue instanceof Object) {
            let proxyObject = this._createProxyObject(propertyValue, propertyDescriptor);

            if (proxyObject) {
                // use proxy object
                container
                    .addBinding({ proxy: proxyObject }, "proxy", this._getBindingOptions(propertyDescriptor))
                    .on("change", function (event) {
                        if (event.last) {
                            thisObj._handleBindingChange(target, propertyName, event.value);
                        }
                    });
            } else {
                // complex property requires braking into simple properties
                container
                    .addButton({
                        label: displayName,
                        title: "Edit"
                    })
                    .on("click", function () {
                        thisObj._selectChildObject(propertyValue, target);
                    });
            }
        }
    }

    _selectObject(obj) {
        let descriptor = this._getDescriptor(obj);
        this._selectedObject = obj;
        this._parentStack = [];
        this._clearPane();

        if (obj instanceof rs.mimic.Component) {
            this._addBlade(this._pane, obj, "id", obj.id, descriptor);
            this._addBlade(this._pane, obj, "name", obj.name, descriptor);
            this._addBlade(this._pane, obj, "typeName", obj.typeName, descriptor);
            this._addBlades(this._pane, obj.properties, null, descriptor);
        } else if (obj instanceof rs.mimic.Mimic) {

        } else {
            this._addBlades(this._pane, obj, null, descriptor);
        }
    }

    _selectChildObject(obj, parent) {
        this._selectedObject = obj;
        this._parentStack.push(parent);
        this._clearPane();
        this._addBlades(this._pane, obj, parent, null);
    }

    _selectParentObject() {
        let parent = this._parentStack.pop();
        let grandParent = this._parentStack.at(-1); // last
        this._clearPane();
        this._addBlades(this._pane, parent, grandParent, null);
    }

    _getDescriptor(obj) {
        const DescriptorSet = rs.mimic.DescriptorSet;

        if (obj instanceof rs.mimic.Component) {
            return DescriptorSet.componentDescriptors.get(obj.typeName);
        } else if (obj instanceof rs.mimic.Mimic) {
            return DescriptorSet.mimicDescriptor;
        } else {
            return null;
        }
    }

    _createProxyObject(target, propertyDescriptor) {
        const BasicType = rs.mimic.BasicType;
        let proxy = null;

        if (propertyDescriptor) {
            if (propertyDescriptor.type === BasicType.POINT) {
                proxy = new PointProxy(target);
            }
        }

        return proxy;
    }

    _getBindingOptions(propertyDescriptor) {
        const BasicType = rs.mimic.BasicType;
        let bindingOptions = null;

        if (propertyDescriptor) {
            if (propertyDescriptor.type === BasicType.POINT) {
                bindingOptions = {
                    label: propertyDescriptor.displayName,
                    x: { step: 1 },
                    y: { step: 1 }
                };
            }
        }

        return bindingOptions;
    }

    _handleBindingChange(target, propertyName, value) {
        console.log(propertyName + " = " + JSON.stringify(value));
    }

    get selectedObject() {
        return this._selectedObject;
    }

    set selectedObject(value) {
        this._selectObject(value);
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
