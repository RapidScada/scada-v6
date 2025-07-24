// Contains renderers for basic components.

rs.mimic.BasicLedRenderer = class extends rs.mimic.RegularComponentRenderer {
    _setClasses(componentElem) {
        super._setClasses(componentElem);
        componentElem.addClass("basic-led");
    }

    _setProps(componentElem, component, renderContext) {
        super._setProps(componentElem, component, renderContext);
        componentElem.text("Led");
    }
}

function registerRenderers() {
    let componentRenderers = rs.mimic.RendererSet.componentRenderers;
    componentRenderers.set("BasicLed", new rs.mimic.BasicLedRenderer());
}

registerRenderers();
