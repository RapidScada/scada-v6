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

    _addBlades(obj, parent, opt_descriptor) {
        const thisObj = this;

        if (obj) {
            for (let [name, value] of Object.entries(obj)) {
                let propDescr = opt_descriptor?.get(name);

                if (typeof value === "number" ||
                    typeof value === "string") {
                    // simple property is editable in row
                    this._pane
                        .addBinding(obj, name, {
                            label: propDescr?.displayName ?? name
                        })
                        .on("change", function (event) {
                            if (event.last) {
                                thisObj._handleBindingChange(obj, name, event.value);
                            }
                        });
                } else if (name === "location") {
                    // temporary
                    this._pane.addBinding({
                        location: new LocationProxy(value)
                    }, name, {
                        x: { step: 1 },
                        y: { step: 1 }
                    });
                } else if (value instanceof Object) {
                    // complex property requires braking into simple properties
                    this._pane
                        .addButton({
                            label: name,
                            title: "Edit"
                        })
                        .on("click", function () {
                            thisObj._selectChildObject(value, obj);
                        });
                }
            }
        }

        if (parent) {
            this._pane.addButton({
                title: "Return to Parent"
            }).on("click", function () {
                thisObj._selectParentObject();
            });
        }
    }

    _selectObject(obj) {
        let descriptor = this._getDescriptor(obj);
        this._selectedObject = obj;
        this._parentStack = [];
        this._clearPane();
        this._addBlades(obj, null, descriptor);
    }

    _selectChildObject(obj, parent) {
        this._selectedObject = obj;
        this._parentStack.push(parent);
        this._clearPane();
        this._addBlades(obj, parent);
    }

    _selectParentObject() {
        let parent = this._parentStack.pop();
        let grandParent = this._parentStack.at(-1); // last
        this._clearPane();
        this._addBlades(parent, grandParent);
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

    _handleBindingChange(obj, propName, value) {
        console.log(propName + " = " + value);
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

// Represents a proxy object for editing location as a Point2d.
class LocationProxy extends ProxyObject {
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
