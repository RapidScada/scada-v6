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
        return image ? "url('" + this._imageToDataUrl(image) + "')" : "";
    }

    // Returns a data URI containing a representation of the Image object.
    _imageToDataUrl(image) {
        return image ? "data:;base64," + image.data : "";
    }

    // Creates a DOM content of the component according to the component model. Returns a jQuery object.
    createDom(component, renderContext) {
        return null;
    }

    // Update the existing DOM content of the component according to the component model.
    updateDom(component, renderContext) {
    }

    // Updates the component according to the current channel data.
    update(component, renderContext) {
    }

    // Sets the location of the component DOM content.
    setLocation(component, x, y) {
        if (component.dom instanceof jQuery) {
            this._setLocation(component.dom, {
                x: x.toString(),
                y: y.toString()
            });
        }
    }

    // Gets the component location from its DOM content.
    getLocation(component) {
        if (component.dom instanceof jQuery) {
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

    // Sets the size of the component DOM content.
    setSize(component, width, height) {
        if (component.dom instanceof jQuery) {
            this._setSize(component.dom, {
                width: width.toString(),
                height: height.toString()
            });
        }
    }

    // Gets the component size from its DOM content.
    getSize(component) {
        if (component.dom instanceof jQuery) {
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
}

// Represents a mimic renderer.
rs.mimic.MimicRenderer = class extends rs.mimic.Renderer {
    get canUpdateDom() {
        return true;
    }

    createDom(component, renderContext) {
        component.dom = $("<div class='mimic'></div>");
        this.updateDom(component, renderContext);
        return component.dom;
    }

    updateDom(component, renderContext) {
        if (component.dom instanceof jQuery) {
            let mimicElem = component.dom.first();
            this._setSize(mimicElem, component.document.size);
        }
    }
}

// Represents a component renderer.
rs.mimic.ComponentRenderer = class extends rs.mimic.Renderer {
    createDom(component, renderContext) {
        let componentElem = $("<div class='comp'></div>")
            .attr("id", "comp" + renderContext.idPrefix + component.id)
            .attr("data-id", component.id);

        if (renderContext.editMode && !renderContext.faceplateMode && component.isContainer) {
            componentElem.addClass("container")
        }

        component.dom = componentElem;
        return componentElem;
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
        if (component.dom instanceof jQuery) {
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
        if (component.dom instanceof jQuery) {
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
    faceplateMode = false;
    imageMap = null;
    idPrefix = "";

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

    constructor(mimic, editMode) {
        this.mimic = mimic;
        this.editMode = editMode;
    }

    // Appends the component DOM content to its parent.
    _appendToParent(component) {
        if (component.dom && component.parent?.dom) {
            component.parent.dom.append(component.dom);
        }
    }

    // Creates a component DOM content.
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

    // Creates a faceplate DOM content.
    _createFaceplateDom(faceplateInstance, renderContext, opt_unknownTypes) {
        if (!faceplateInstance.model) {
            opt_unknownTypes?.add(faceplateInstance.typeName);
            return;
        }

        let faceplateContext = new rs.mimic.RenderContext();
        faceplateContext.editMode = this.editMode;
        faceplateContext.faceplateMode = true;
        faceplateContext.imageMap = faceplateInstance.model.imageMap;
        faceplateContext.idPrefix = renderContext.idPrefix;

        let renderer = rs.mimic.RendererSet.faceplateRenderer;
        faceplateInstance.renderer = renderer;
        renderer.createDom(faceplateInstance, faceplateContext);
        this._appendToParent(faceplateInstance);
        faceplateContext.idPrefix += faceplateInstance.id + "-";

        for (let component of faceplateInstance.components) {
            this._createComponentDom(component, faceplateContext, opt_unknownTypes);
        }
    }

    // Creates a mimic DOM content according to the mimic model. Returns a jQuery object.
    createMimicDom() {
        const RendererSet = rs.mimic.RendererSet;
        let startTime = Date.now();
        let unknownTypes = new Set();

        let renderContext = new rs.mimic.RenderContext();
        renderContext.editMode = this.editMode;
        renderContext.imageMap = this.mimic.imageMap;
        RendererSet.mimicRenderer.createDom(this.mimic, renderContext);

        for (let component of this.mimic.components) {
            this._createComponentDom(component, renderContext, unknownTypes);
        }

        if (unknownTypes.size > 0) {
            console.warn("Unknown component types: " + Array.from(unknownTypes).sort().join(", "));
        }

        if (this.mimic.dom) {
            console.info(ScadaUtils.getCurrentTime() + " Mimic DOM created in " + (Date.now() - startTime) + " ms");
            return this.mimic.dom;
        } else {
            return $();
        }
    }

    // Creates a component DOM content according to the component model.
    createComponentDom(component) {
        let renderContext = new rs.mimic.RenderContext();
        renderContext.editMode = this.editMode;
        renderContext.imageMap = this.mimic.imageMap;
        this._createComponentDom(component, renderContext);
    }

    // Updates the component DOM content according to the component model.
    updateComponentDom(component) {
        if (component.dom && component.renderer) {
            let renderContext = new rs.mimic.RenderContext();
            renderContext.editMode = this.editMode;

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
}
