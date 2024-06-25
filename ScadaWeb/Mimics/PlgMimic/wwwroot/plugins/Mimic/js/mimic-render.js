// Contains classes: Renderer, MimicRenderer, ComponentRenderer,
// TextRenderer, PictureRenderer, PanelRenderer, RenderContext, RendererSet
// Depends on jquery, mimic-common.js

// Represents a renderer of a mimic or component.
rs.mimic.Renderer = class {
    canUpdateDom = false; // the renderer supports DOM update

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
    constructor() {
        super();
        this.canUpdateDom = true;
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
        component.dom = $("<div id='comp" + renderContext.idPrefix + component.id + "' class='comp'></div>");
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
    constructor() {
        super();
        this.canUpdateDom = true;
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
    constructor() {
        super();
        this.canUpdateDom = true;
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
    mimicRenderer = new rs.mimic.MimicRenderer();
    faceplateRenderer = new rs.mimic.FaceplateRenderer();
    componentRenderers = new Map([
        ["Text", new rs.mimic.TextRenderer()],
        ["Picture", new rs.mimic.PictureRenderer()],
        ["Panel", new rs.mimic.PanelRenderer()]
    ]);
}
