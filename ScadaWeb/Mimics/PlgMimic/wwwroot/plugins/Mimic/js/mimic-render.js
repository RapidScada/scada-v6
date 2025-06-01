// Contains classes: Renderer, MimicRenderer, ComponentRenderer,
//     TextRenderer, PictureRenderer, PanelRenderer, RenderContext, RendererSet, UnitedRenderer
// Depends on jquery, scada-common.js, mimic-common.js

// Represents a renderer of a mimic or component.
rs.mimic.Renderer = class {
    // Gets a value indocating whether the renderer supports DOM update.
    get canUpdateDom() {
        return false;
    }

    // Sets the left and top of the specified jQuery object.
    _setLocation(jqObj, location) {
        jqObj.css({
            "left": location.x + "px",
            "top": location.y + "px"
        });
    }

    // Sets the width and height of the specified jQuery object.
    _setSize(jqObj, size) {
        jqObj.css({
            "width": size.width + "px",
            "height": size.height + "px"
        });
    }

    // Sets the background image of the specified jQuery object.
    _setBackgroundImage(jqObj, image) {
        jqObj.css("background-image", this._imageToDataUrlCss(image));
    }

    // Returns a css property value for the image data URI.
    _imageToDataUrlCss(image) {
        return image ? "url('" + image.dataUrl + "')" : "";
    }

    // Creates a component DOM according to the component model. Returns a jQuery object.
    createDom(component, renderContext) {
        return null;
    }

    // Updates the existing component DOM according to the component model.
    updateDom(component, renderContext) {
    }

    // Updates the component view according to the current channel data.
    update(component, renderContext) {
    }

    // Sets the location of the component DOM.
    setLocation(component, x, y) {
        if (component.dom) {
            this._setLocation(component.dom, {
                x: x.toString(),
                y: y.toString()
            });
        }
    }

    // Gets the component location from its DOM.
    getLocation(component) {
        if (component.dom) {
            let position = component.dom.position();
            return {
                x: position.left.toString(),
                y: position.top.toString()
            };
        } else {
            return {
                x: "0",
                y: "0"
            };
        }
    }

    // Sets the size of the component DOM.
    setSize(component, width, height) {
        if (component.dom) {
            this._setSize(component.dom, {
                width: width.toString(),
                height: height.toString()
            });
        }
    }

    // Gets the component size from its DOM.
    getSize(component) {
        if (component.dom) {
            return {
                width: component.dom.outerWidth().toString(),
                height: component.dom.outerHeight().toString()
            };
        } else {
            return {
                width: "0",
                height: "0"
            };
        }
    }

    // Appends the child DOM into the parent DOM.
    static appendChild(parent, child) {
        if (parent.dom && child.dom) {
            parent.dom.append(child.dom);
        }
    }

    // Removes the component DOM from the mimic keeping data associated with the removed elements.
    static detach(component) {
        component.dom?.detach();
    }

    // Removes the component DOM from the mimic.
    static remove(component) {
        component.dom?.remove();
    }

    // Arranges the child components according to their order.
    static arrangeChildren(parent) {
        if (parent.children && parent.dom) {
            for (let component of parent.children) {
                component.dom?.detach();
            }

            for (let component of parent.children) {
                if (component.dom) {
                    parent.dom.append(component.dom);
                }
            }
        }
    }
}

// Represents a mimic renderer.
rs.mimic.MimicRenderer = class MimicRenderer extends rs.mimic.Renderer {
    static _GRID_COLOR = "#dee2e6"; // gray-300

    get canUpdateDom() {
        return true;
    }

    static _createGrid(gridSize, mimicSize) {
        // create canvas
        let canvasElem = $("<canvas class='grid'></canvas>");
        let canvas = canvasElem[0];
        let width = canvas.width = mimicSize.width;
        let height = canvas.height = mimicSize.height;

        // prepare drawing context
        let context = canvas.getContext("2d");
        context.lineWidth = 1;
        context.strokeStyle = MimicRenderer._GRID_COLOR;

        // draw grids with small and large cells
        MimicRenderer._drawGrid(context, width, height, gridSize, [1, 1]);
        MimicRenderer._drawGrid(context, width, height, gridSize * 10);
        return canvasElem;
    }

    static _drawGrid(context, width, height, gridStep, dashSegments = []) {
        const adj = 0.5; // adjustment for sharpness of lines

        // draw vertical lines
        for (let x = gridStep; x < width; x += gridStep) {
            context.beginPath();
            context.setLineDash(dashSegments);
            context.moveTo(x + adj, 0);
            context.lineTo(x + adj, height);
            context.stroke();
        }

        // draw horizontal lines
        for (let y = gridStep; y < height; y += gridStep) {
            context.beginPath();
            context.setLineDash(dashSegments);
            context.moveTo(0, y + adj);
            context.lineTo(width, y + adj);
            context.stroke();
        }
    }

    createDom(mimic, renderContext) {
        let mimicElem = $("<div class='mimic'></div>");
        mimic.dom = mimicElem;

        if (renderContext.editMode && renderContext.editorOptions &&
            renderContext.editorOptions.showGrid && renderContext.editorOptions.gridStep > 1) {
            mimicElem.append(MimicRenderer._createGrid(renderContext.editorOptions.gridStep, mimic.document.size));
        }

        this.updateDom(mimic, renderContext);
        return mimicElem;
    }

    updateDom(mimic, renderContext) {
        if (mimic.dom) {
            let mimicElem = mimic.dom.first();
            this._setSize(mimicElem, mimic.document.size);
        }
    }
}

// Represents a component renderer.
rs.mimic.ComponentRenderer = class extends rs.mimic.Renderer {
    createDom(component, renderContext) {
        let componentElem = $("<div class='comp'></div>")
            .attr("id", "comp" + renderContext.idPrefix + component.id)
            .attr("data-id", component.id);

        if (renderContext.editMode) {
            if (!renderContext.faceplateMode && component.isContainer) {
                componentElem.addClass("container")
            }

            if (component.isSelected) {
                componentElem.addClass("selected")
            }
        }

        component.dom = componentElem;
        return componentElem;
    }

    updateLocation(component) {
        if (component.dom) {
            this._setLocation(component.dom, component.properties.location);
        }
    }

    updateSelected(component) {
        if (component.dom) {
            component.dom.toggleClass("selected", component.isSelected);
        }
    }

    allowResizing(component) {
        return true;
    }
}

// Represents a text component renderer.
rs.mimic.TextRenderer = class extends rs.mimic.ComponentRenderer {
    createDom(component, renderContext) {
        let textElem = super.createDom(component, renderContext);
        let props = component.properties;
        textElem.addClass("text").text(props.text);
        this._setLocation(textElem, props.location);
        this._setSize(textElem, props.size);
        return textElem;
    }
}

// Represents a picture component renderer.
rs.mimic.PictureRenderer = class extends rs.mimic.ComponentRenderer {
    createDom(component, renderContext) {
        let pictureElem = super.createDom(component, renderContext);
        let props = component.properties;
        pictureElem.addClass("picture");
        this._setLocation(pictureElem, props.location);
        this._setSize(pictureElem, props.size);
        this._setBackgroundImage(pictureElem, renderContext.getImage(props.imageName));
        return pictureElem;
    }
}

// Represents a panel component renderer.
rs.mimic.PanelRenderer = class extends rs.mimic.ComponentRenderer {
    get canUpdateDom() {
        return true;
    }

    createDom(component, renderContext) {
        let panelElem = super.createDom(component, renderContext);
        panelElem.addClass("panel");
        this.updateDom(component, renderContext);
        return panelElem;
    }

    updateDom(component, renderContext) {
        if (component.dom) {
            let panelElem = component.dom.first();
            let props = component.properties;
            this._setLocation(panelElem, props.location);
            this._setSize(panelElem, props.size);
        }
    }
}

// Represents a faceplate renderer.
rs.mimic.FaceplateRenderer = class extends rs.mimic.ComponentRenderer {
    get canUpdateDom() {
        return true;
    }

    createDom(component, renderContext) {
        let faceplateElem = super.createDom(component, renderContext);
        faceplateElem.addClass("faceplate");
        this.updateDom(component, renderContext);
        return faceplateElem;
    }

    updateDom(component, renderContext) {
        if (component.dom) {
            let faceplateElem = component.dom.first();
            let props = component.properties;
            this._setLocation(faceplateElem, props.location);
            this._setSize(faceplateElem, props.size);
        }
    }
}

// Encapsulates information about a rendering operation.
rs.mimic.RenderContext = class {
    editMode = false;
    editorOptions = null;
    faceplateMode = false;
    imageMap = null;
    idPrefix = "";

    constructor(source) {
        Object.assign(this, source);
    }

    getImage(imageName) {
        return this.imageMap instanceof Map ? this.imageMap.get(imageName) : null;
    }
}

// Contains renderers for a mimic and its components.
rs.mimic.RendererSet = class {
    static mimicRenderer = new rs.mimic.MimicRenderer();
    static faceplateRenderer = new rs.mimic.FaceplateRenderer();
    static componentRenderers = new Map([
        ["Text", new rs.mimic.TextRenderer()],
        ["Picture", new rs.mimic.PictureRenderer()],
        ["Panel", new rs.mimic.PanelRenderer()]
    ]);
}

// Renders a mimic using appropriate renderers.
rs.mimic.UnitedRenderer = class {
    mimic;
    editMode;
    editorOptions;

    constructor(mimic, editMode) {
        this.mimic = mimic;
        this.editMode = editMode;
        this.editorOptions = null;
    }

    // Appends the component DOM to its parent.
    _appendToParent(component) {
        if (component.parent) {
            rs.mimic.Renderer.appendChild(component.parent, component);
        }
    }

    // Creates a component DOM.
    _createComponentDom(component, renderContext, opt_unknownTypes) {
        if (component.isFaceplate) {
            this._createFaceplateDom(component, renderContext, opt_unknownTypes);
        } else {
            let renderer = rs.mimic.RendererSet.componentRenderers.get(component.typeName);

            if (renderer) {
                component.renderer = renderer;
                renderer.createDom(component, renderContext);
                this._appendToParent(component);
            } else {
                opt_unknownTypes?.add(component.typeName);
            }
        }
    }

    // Creates a faceplate DOM.
    _createFaceplateDom(faceplateInstance, renderContext, opt_unknownTypes) {
        if (!faceplateInstance.model) {
            opt_unknownTypes?.add(faceplateInstance.typeName);
            return;
        }

        let faceplateContext = new rs.mimic.RenderContext({
            editMode: this.editMode,
            editorOptions: this.editorOptions,
            faceplateMode: true,
            imageMap: faceplateInstance.model.imageMap,
            idPrefix: renderContext.idPrefix
        });
        let renderer = rs.mimic.RendererSet.faceplateRenderer;
        faceplateInstance.renderer = renderer;
        renderer.createDom(faceplateInstance, faceplateContext);
        this._appendToParent(faceplateInstance);
        faceplateContext.idPrefix += faceplateInstance.id + "-";

        for (let component of faceplateInstance.components) {
            this._createComponentDom(component, faceplateContext, opt_unknownTypes);
        }
    }

    // Creates a mimic DOM according to the mimic model. Returns a jQuery object.
    createMimicDom() {
        let startTime = Date.now();
        let unknownTypes = new Set();
        let renderContext = new rs.mimic.RenderContext({
            editMode: this.editMode,
            editorOptions: this.editorOptions,
            imageMap: this.mimic.imageMap
        });
        let renderer = rs.mimic.RendererSet.mimicRenderer;
        this.mimic.renderer = renderer;
        renderer.createDom(this.mimic, renderContext);

        for (let component of this.mimic.components) {
            this._createComponentDom(component, renderContext, unknownTypes);
        }

        if (unknownTypes.size > 0) {
            console.warn("Unknown component types: " + Array.from(unknownTypes).sort().join(", "));
        }

        if (this.mimic.dom) {
            console.info("Mimic DOM created in " + (Date.now() - startTime) + " ms");
            return this.mimic.dom;
        } else {
            return $();
        }
    }

    // Updates a mimic DOM according to the mimic model.
    updateMimicDom() {
        rs.mimic.RendererSet.mimicRenderer.updateDom(this.mimic);
    }

    // Creates a component DOM according to the component model. Returns a jQuery object.
    createComponentDom(component) {
        this._createComponentDom(component, new rs.mimic.RenderContext({
            editMode: this.editMode,
            editorOptions: this.editorOptions,
            imageMap: this.mimic.imageMap
        }));
        return component.dom ?? $();
    }

    // Updates the component DOM according to the component model.
    updateComponentDom(component) {
        if (component.dom && component.renderer) {
            let renderContext = new rs.mimic.RenderContext({
                editMode: this.editMode,
                editorOptions: this.editorOptions
            });

            if (component.isFaceplate) {
                renderContext.faceplateMode = true;
                renderContext.imageMap = component.model.imageMap;
                component.renderer.updateDom(component, renderContext);
            } else {
                renderContext.imageMap = mimic.imageMap;

                if (component.renderer.canUpdateDom) {
                    component.renderer.updateDom(component, renderContext);
                } else {
                    let oldDom = component.dom;
                    component.renderer.createDom(component, renderContext);
                    oldDom.replaceWith(component.dom);
                }
            }
        }
    }

    // Arranges the child component DOMs according to the parent's model.
    arrangeChildren(parent) {
        if (parent.children) {
            // detach components
            for (let component of parent.children) {
                if (component.dom) {
                    component.dom.detach();
                }
            }

            // append components
            for (let component of parent.children) {
                this._appendToParent(component);
            }
        }
    }
}
