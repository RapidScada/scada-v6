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
        $("<div class='basic-button-content'>" +
            "<div class='basic-button-icon'></div>" +
            "<div class='basic-button-text'></div></div>").appendTo(componentElem);
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
    _setBorder(jqObj, border) {
        // do nothing
    }

    _setCornerRadius(jqObj, cornerRadius) {
        // do nothing
    }

    _setLedBorder(componentElem, borderElem, border) {
        super._setBorder(componentElem, null);
        super._setBorder(borderElem, border);
    }

    _setLedCornerRadius(componentElem, borderElem, cornerRadius) {
        super._setCornerRadius(componentElem, cornerRadius);
        super._setCornerRadius(borderElem, cornerRadius);
    }

    _completeDom(componentElem, component, renderContext) {
        $("<div class='basic-led-border'></div>").appendTo(componentElem);
    }

    _setClasses(componentElem, component, renderContext) {
        super._setClasses(componentElem, component, renderContext);
        componentElem.addClass("basic-led");

        if (!component.properties.isSquare) {
            componentElem.addClass("circle");
        }
    }

    _setProps(componentElem, component, renderContext) {
        super._setProps(componentElem, component, renderContext);
        let borderElem = componentElem.children().first();
        let props = component.properties;
        this._setLedBorder(componentElem, borderElem, props.border);
        this._setLedCornerRadius(componentElem, borderElem, props.isSquare ? props.cornerRadius : null);
        borderElem.css("opacity", props.borderOpacity / 100);
    }
};

rs.mimic.BasicToggleRenderer = class extends rs.mimic.RegularComponentRenderer {
    _completeDom(componentElem, component, renderContext) {
        $("<div class='basic-toggle-lever'></div>").appendTo(componentElem);
    }

    _setClasses(componentElem, component, renderContext) {
        super._setClasses(componentElem, component, renderContext);
        componentElem.addClass("basic-toggle");

        const BasicTogglePosition = rs.mimic.BasicTogglePosition;
        let position = component.properties.position;
        componentElem.toggleClass("position-not-set", position === BasicTogglePosition.NOT_SET);
        componentElem.toggleClass("position-off", position === BasicTogglePosition.OFF);
        componentElem.toggleClass("position-on", position === BasicTogglePosition.ON);
    }

    _setProps(componentElem, component, renderContext) {
        super._setProps(componentElem, component, renderContext);
        let props = component.properties;
        this._setPadding(componentElem, props.padding);

        let leverElem = componentElem.children().first();
        let minSize = Math.min(component.width, component.height);
        let minInnerSize = Math.min(component.innerWidth, component.innerHeight);
        componentElem.css("border-radius", minSize / 2);

        leverElem.css({
            "background-color": props.foreColor,
            "width": minInnerSize,
            "height": minInnerSize
        });
    }
};

// Registers the renderers. The function name must be unique.
function registerBasicRenderers() {
    let componentRenderers = rs.mimic.RendererSet.componentRenderers;
    componentRenderers.set("BasicButton", new rs.mimic.BasicButtonRenderer());
    componentRenderers.set("BasicLed", new rs.mimic.BasicLedRenderer());
    componentRenderers.set("BasicToggle", new rs.mimic.BasicToggleRenderer());
}

registerBasicRenderers();
