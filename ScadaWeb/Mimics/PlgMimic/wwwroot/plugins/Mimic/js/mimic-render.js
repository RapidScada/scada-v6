﻿// Contains classes: Renderer, MimicRenderer, ComponentRenderer,
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
        component.dom = $("<div class='comp'></div>")
            .attr("id", "comp" + renderContext.idPrefix + component.id)
            .attr("data-id", component.id);
        return component.dom;
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
            this._setLocation(faceplateElem, component.properties.location);
            this._setSize(faceplateElem, component.model.document.size);
        }
    }
}

// Encapsulates information about a rendering operation.
rs.mimic.RenderContext = class {
    editMode = false;
    idPrefix = "";
    imageMap = null;

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

    // Creates a faceplate DOM content.
    _createFaceplateDom(faceplateInstance, unknownTypes) {
        if (!faceplateInstance.model) {
            unknownTypes.add(faceplateInstance.typeName);
            return;
        }

        let renderContext = new rs.mimic.RenderContext();
        renderContext.editMode = this.editMode;
        renderContext.imageMap = faceplateInstance.model.imageMap;

        const RendererSet = rs.mimic.RendererSet;
        faceplateInstance.renderer = RendererSet.faceplateRenderer;
        RendererSet.faceplateRenderer.createDom(faceplateInstance, renderContext);
        renderContext.idPrefix = faceplateInstance.id + "-";

        for (let component of faceplateInstance.components) {
            let renderer = RendererSet.componentRenderers.get(component.typeName);

            if (renderer) {
                component.renderer = renderer;
                renderer.createDom(component, renderContext);

                if (component.dom && component.parent?.dom) {
                    component.parent.dom.append(component.dom);
                }
            } else {
                unknownTypes.add(component.typeName);
            }
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
            if (component.isFaceplate) {
                this._createFaceplateDom(component, unknownTypes);
            } else {
                let renderer = RendererSet.componentRenderers.get(component.typeName);

                if (renderer) {
                    component.renderer = renderer;
                    renderer.createDom(component, renderContext);
                } else {
                    unknownTypes.add(component.typeName);
                }
            }

            if (component.dom && component.parent?.dom) {
                component.parent.dom.append(component.dom);
            }
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

    // Update the component DOM content according to the component model.
    updateComponentDom(component) {
        if (component.dom && component.renderer) {
            let renderContext = new rs.mimic.RenderContext();
            renderContext.editMode = this.editMode;

            if (component.isFaceplate) {
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
