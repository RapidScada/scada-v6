// Contains renderers for basic components.

rs.mimic.BasicButtonRenderer = class extends rs.mimic.RegularComponentRenderer {
    _setBorder(jqObj, border) {
        if (border.width > 0) {
            super._setBorder(jqObj, border);
        } else {
            jqObj.css("border", ""); // default border
        }
    }

    _setCornerRadius(jqObj, cornerRadius) {
        if (cornerRadius.isSet) {
            super._setCornerRadius(jqObj, cornerRadius);
        } else {
            jqObj.css("border-radius", ""); // default radius
        }
    }

    _completeDom(componentElem, component, renderContext) {
        let contentElem = $("<div class='basic-button-content'>" +
            "<div class='basic-button-icon'></div><div class='basic-button-text'></div></div>");
        componentElem.append(contentElem);
    }

    _setClasses(componentElem, component, renderContext) {
        super._setClasses(componentElem, component, renderContext);
        componentElem.addClass("basic-button");
    }

    _setProps(componentElem, component, renderContext) {
        super._setProps(componentElem, component, renderContext);
        let contentElem = componentElem.children(".basic-button-content:first");
        let iconElem = contentElem.children(".basic-button-icon:first");
        let textElem = contentElem.children(".basic-button-text:first");
        let props = component.properties;

        if (props.imageName) {
            this._setBackgroundImage(iconElem, renderContext.getImage(props.imageName));
            this._setSize(iconElem, props.imageSize);
        } else {
            this._setBackgroundImage(iconElem, null);
            this._setSize(iconElem, { width: 0, height: 0 });
        }

        textElem.text(props.text);
    }

    createDom(component, renderContext) {
        let buttonElem = $("<button type='button'></button>")
            .attr("id", "comp" + renderContext.idPrefix + component.id)
            .attr("data-id", component.id);
        this._completeDom(buttonElem, component, renderContext);
        this._setClasses(buttonElem, component, renderContext);
        this._setProps(buttonElem, component, renderContext);
        component.dom = buttonElem;
        return buttonElem;
    }
};

rs.mimic.BasicLedRenderer = class extends rs.mimic.RegularComponentRenderer {
    _setClasses(componentElem, component, renderContext) {
        super._setClasses(componentElem, component, renderContext);
        componentElem.addClass("basic-led");
    }

    _setProps(componentElem, component, renderContext) {
        super._setProps(componentElem, component, renderContext);
        componentElem.text("Led");
    }
};

rs.mimic.BasicToggleRenderer = class extends rs.mimic.RegularComponentRenderer {
};

// Registers the renderers. The function name must be unique.
function registerBasicRenderers() {
    let componentRenderers = rs.mimic.RendererSet.componentRenderers;
    componentRenderers.set("BasicButton", new rs.mimic.BasicButtonRenderer());
    componentRenderers.set("BasicLed", new rs.mimic.BasicLedRenderer());
    componentRenderers.set("BasicToggle", new rs.mimic.BasicToggleRenderer());
}

registerBasicRenderers();
