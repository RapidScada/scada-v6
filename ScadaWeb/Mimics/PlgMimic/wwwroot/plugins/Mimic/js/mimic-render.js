// Contains classes: Renderer, MimicRenderer, ComponentRenderer, RenderContext
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

// Contains renderers for a mimic and its components.
rs.mimic.RendererSet = class {
    mimicRenderer = new rs.mimic.MimicRenderer();
    componentRenderers = new Map();
}

// Encapsulates information about a rendering operation.
rs.mimic.RenderContext = class {
    editMode = false;
}
