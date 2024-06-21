// Contains classes: Renderer, MimicRenderer, ComponentRenderer,
// TextRenderer, PictureRenderer, PanelRenderer, RenderContext, RendererSet
// Depends on jquery, mimic-common.js

// Represents a renderer of a mimic or component.
rs.mimic.Renderer = class {
    // Creates a DOM content of the component according to the model.
    createDom(component, renderContext) {
    }

    // Updates the component according to the current channel data.
    update(component, renderContext) {
    }
}

// Represents a mimic renderer.
rs.mimic.MimicRenderer = class extends rs.mimic.Renderer {
    createDom(component, renderContext) {
        component.dom = $("<div class='mimic'></div>");
        this.updateDom(component, renderContext);
    }

    updateDom(component, renderContext) {
        if (component.dom instanceof jQuery) {
            let mimicWidth = component.document.size.width;
            let mimicHeight = component.document.size.height;
            let mimicElem = component.dom.first();

            mimicElem.css({
                "width": mimicWidth,
                "height": mimicHeight
            });
        }
    }
}

// Represents a component renderer.
rs.mimic.ComponentRenderer = class extends rs.mimic.Renderer {
    createDom(component, renderContext) {
        component.dom = $("<div id='" + component.id + "' class='comp'></div>");
    }
}

// Represents a text component renderer.
rs.mimic.TextRenderer = class extends rs.mimic.ComponentRenderer {
    createDom(component, renderContext) {
        super.createDom(component, renderContext);
    }
}

// Represents a picture component renderer.
rs.mimic.PictureRenderer = class extends rs.mimic.ComponentRenderer {
    createDom(component, renderContext) {
        super.createDom(component, renderContext);
    }
}

// Represents a panel component renderer.
rs.mimic.PanelRenderer = class extends rs.mimic.ComponentRenderer {
    createDom(component, renderContext) {
        super.createDom(component, renderContext);
        this.updateDom(component, renderContext);
    }

    updateDom(component, renderContext) {
        if (component.dom instanceof jQuery) {
            let panelElem = component.dom.first();
            let props = component.properties;

            panelElem.css({
                "left": props.location.x,
                "top": props.location.y,
                "width": props.size.width,
                "height": props.size.height
            });
        }
    }
}

// Encapsulates information about a rendering operation.
rs.mimic.RenderContext = class {
    editMode = false;
}

// Contains renderers for a mimic and its components.
rs.mimic.RendererSet = class {
    mimicRenderer = new rs.mimic.MimicRenderer();
    componentRenderers = new Map([
        ["Text", new rs.mimic.TextRenderer()],
        ["Picture", new rs.mimic.PictureRenderer()],
        ["Panel", new rs.mimic.PanelRenderer()]
    ]);
}
