// Contains classes: PropGrid
// Depends on jquery, tweakpane

// Interacts with Tweakpane to provide property grid functionality.
class PropGrid {
    _pane; // Tweakpane
    _selectedObject = null;
    _parentStack = [];

    constructor(pane) {
        this._pane = pane;
    }

    _clearPane() {
        for (let child of this._pane.children) {
            child.dispose();
        }
    }

    _addBlades(obj, parent) {
        const thisObj = this;

        for (let [name, value] of Object.entries(obj)) {
            if (typeof value === "number" ||
                typeof value === "string") {
                // simple property is editable in row
                this._pane.addBinding(obj, name);
            } else if (value instanceof Object) {
                // complex property requires braking into simple properties
                this._pane.addButton({
                    label: name,
                    title: "Edit"
                }).on("click", function () {
                    thisObj._selectedObject = value;
                    thisObj._parentStack.push(obj);
                    thisObj._clearPane();
                    thisObj._addBlades(value, obj);
                });
            }
        }

        if (parent) {
            this._pane.addButton({
                title: "Return to Parent"
            }).on("click", function () {
                let parent1 = thisObj._parentStack.pop();
                let parent2 = thisObj._parentStack.at(-1); // last
                thisObj._clearPane();
                thisObj._addBlades(parent1, parent2);
            });
        }
    }

    get selectedObject() {
        return this._selectedObject;
    }

    set selectedObject(value) {
        this._selectedObject = value;
        this._parentStack = [];
        this._clearPane();
        this._addBlades(value, null);
    }
}
