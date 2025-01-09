
/********** Border Text Renderer **********/

// Border text renderer type extends scada.scheme.ComponentRenderer
scada.scheme.BorderTextRenderer = function () {
    scada.scheme.ComponentRenderer.call(this);
};

scada.scheme.BorderTextRenderer.prototype = Object.create(scada.scheme.ComponentRenderer.prototype);
scada.scheme.BorderTextRenderer.constructor = scada.scheme.BorderTextRenderer;

// Set horizontal algnment of the jQuery object
scada.scheme.BorderTextRenderer.prototype.setH = function (jqObj, hAlign) {
    var HorizontalAlignments = scada.scheme.HorizontalAlignments;

    switch (hAlign) {
        case HorizontalAlignments.CENTER:
            jqObj.css("justify-content", "center");
            break;
        case HorizontalAlignments.RIGHT:
            jqObj.css("justify-content", "flex-end");
            break;
        default:
            jqObj.css("justify-content", "flex-start");
            break;
    }
};

// Set vertical algnment of the jQuery object
scada.scheme.BorderTextRenderer.prototype.setV = function (jqObj, vAlign) {
    var VerticalAlignments = scada.scheme.VerticalAlignments;

    switch (vAlign) {
        case VerticalAlignments.CENTER:
            jqObj.css("align-items", "center");
            break;
        case VerticalAlignments.BOTTOM:
            jqObj.css("align-items", "flex-end");
            break;
        default:
            jqObj.css("align-items", "flex-start");
            break;
    }
};
scada.scheme.BorderTextRenderer.prototype.setWrap = function (jqObj, wordWrap) {
    jqObj.css("white-space", wordWrap ? "normal" : "nowrap");
    jqObj.css("word-wrap", wordWrap ? "break-word" : "normal");
};

scada.scheme.BorderTextRenderer.prototype.createDom = function (component, renderContext) {
    var props = component.props;
    var spanComp = $("<div class='text-container' id='comp" + component.id + "'></div>");
    spanComp.css({
        "width": props.size.width,
        "height": props.size.height
    });
    var spanText = $("<span></span>");

    this.prepareComponent(spanComp, component, true);
    this.setFont(spanComp, props.font);
    this.setForeColor(spanComp, props.foreColor);
    spanText.css({
        "display": "block",
        "overflow": "hidden"
    });

    this.setH(spanComp, props.hAlign);
    this.setV(spanComp, props.vAlign);
    this.setWrap(spanText, props.wordWrap);

    spanText.text(props.text);
    spanComp.append(spanText);
    component.dom = spanComp;
};

scada.scheme.BorderTextRenderer.prototype.setSize = function (component, width, height) {
    component.props.size = { width: width, height: height };
    component.dom.css({
        "width": width,
        "height": height
    });
};

scada.scheme.BorderTextRenderer.prototype.allowResizing = function (component) {
    return !component.props.autoSize;
};


/********** Renderer Map **********/

// Renderer map object
scada.scheme.rendererMap.set("Scada.Web.Plugins.SchVolExtentionsComp.BorderText", new scada.scheme.BorderTextRenderer());