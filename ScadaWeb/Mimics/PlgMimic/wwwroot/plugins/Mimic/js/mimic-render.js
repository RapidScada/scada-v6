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
}

// Represents a component renderer.
rs.mimic.ComponentRenderer = class extends rs.mimic.Renderer {
}

// Represents a text component renderer.
rs.mimic.TextRenderer = class extends rs.mimic.ComponentRenderer {
}

// Represents a picture component renderer.
rs.mimic.PictureRenderer = class extends rs.mimic.ComponentRenderer {
}

// Represents a panel component renderer.
rs.mimic.PanelRenderer = class extends rs.mimic.ComponentRenderer {
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
