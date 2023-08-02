// Renders a scheme.
// Depends on jquery, scada-common.js, main-api.js, scheme-common.js
// Contains classes:
//     Renderer
//         SchemeRenderer
//         ComponentRenderer
//             StaticTextRenderer
//                 DynamicTextRenderer
//             StaticPictureRenderer
//                 DynamicPictureRenderer
//             StaticPolygonRenderer
//                 DynamicPolygonRenderer

// Namespaces
var scada = scada || {};
scada.scheme = scada.scheme || {};

/********** Renderer **********/

// Abstract parent type for renderers
scada.scheme.Renderer = function () {
    // Color bound to an input channel status
    this.STATUS_COLOR = "Status";
    // Color displayed in edit mode for the Status color
    this.STATUS_DISPLAY_COLOR = "Silver";
    // Width of the wrapper frame of a component in edit mode
    this.COMP_FRAME_WIDTH = 1;
};

// Set background color of the jQuery object
scada.scheme.Renderer.prototype.setBackColor = function (jqObj, backColor, opt_removeIfEmpty, opt_statusColor) {
    if (backColor) {
        if (backColor === this.STATUS_COLOR) {
            jqObj.css("background-color", opt_statusColor ? opt_statusColor : this.STATUS_DISPLAY_COLOR);
        } else if (typeof opt_statusColor === "undefined") {
            jqObj.css("background-color", backColor);
        }
    } else if (opt_removeIfEmpty) {
        jqObj.css("background-color", "");
    }
};

// Set border color of the jQuery object
scada.scheme.Renderer.prototype.setBorderColor = function (jqObj, borderColor, opt_removeIfEmpty, opt_statusColor) {
    if (borderColor) {
        if (borderColor === this.STATUS_COLOR) {
            jqObj.css("border-color", opt_statusColor ? opt_statusColor : this.STATUS_DISPLAY_COLOR);
        } else if (typeof opt_statusColor === "undefined") {
            jqObj.css("border-color", borderColor);
        }
    } else if (opt_removeIfEmpty) {
        jqObj.css("border-color", "transparent");
    }
};

// Set border width and style of the jQuery object
scada.scheme.Renderer.prototype.setBorderWidth = function (jqObj, borderWidth, opt_removeIfEmpty) {
    if (Number.isInteger(borderWidth) && borderWidth > 0) {
        jqObj.css({
            "border-style": "solid",
            "border-width": borderWidth
        });
    } else if (opt_removeIfEmpty) {
        jqObj.css({
            "border-style": "none",
            "border-width": ""
        });
    }
};

// Set fore color of the jQuery object
scada.scheme.Renderer.prototype.setForeColor = function (jqObj, foreColor, opt_removeIfEmpty, opt_statusColor) {
    if (foreColor) {
        if (foreColor === this.STATUS_COLOR) {
            jqObj.css("color", opt_statusColor ? opt_statusColor : this.STATUS_DISPLAY_COLOR);
        } else if (typeof opt_statusColor === "undefined") {
            jqObj.css("color", foreColor);
        }
    } else if (opt_removeIfEmpty) {
        jqObj.css("color", "");
    }
};

// Set font of the jQuery object
scada.scheme.Renderer.prototype.setFont = function (jqObj, font, opt_removeIfEmpty) {
    if (font) {
        jqObj.css({
            "font-family": font.name,
            "font-size": font.size + "px",
            "font-weight": font.bold ? "bold" : "normal",
            "font-style": font.italic ? "italic" : "normal",
            "text-decoration": font.underline ? "underline" : "none"
        });
    } else if (opt_removeIfEmpty) {
        jqObj.css({
            "font-family": "",
            "font-size": "",
            "font-weight": "",
            "font-style": "",
            "text-decoration": ""
        });
    }
};

// Set background image of the jQuery object
scada.scheme.Renderer.prototype.setBackgroundImage = function (jqObj, image, opt_removeIfEmpty) {
    if (image) {
        jqObj.css("background-image", this.imageToDataUrlCss(image));
    } else if (opt_removeIfEmpty) {
        jqObj.css("background-image", "");
    }
};

// Returns a data URI containing a representation of the image
scada.scheme.Renderer.prototype.imageToDataURL = function (image) {
    return image ?
        "data:" + (image.mediaType ? image.mediaType : "image/png") + ";base64," + image.data :
        "";
};

// Returns a css property value for the image data URI
scada.scheme.Renderer.prototype.imageToDataUrlCss = function (image) {
    return "url('" + this.imageToDataURL(image) + "')";
};

// Create DOM content of the component according to the component properties.
// If component properties are incorrect, no DOM content is created
scada.scheme.Renderer.prototype.createDom = function (component, renderContext) {
};

// Refresh the component if it contains the specified images
scada.scheme.Renderer.prototype.refreshImages = function (component, renderContext, imageNames) {
};

// Update the component according to the current input channel data
scada.scheme.Renderer.prototype.updateData = function (component, renderContext) {
};

/********** Scheme Renderer **********/

// Scheme renderer type extends scada.scheme.Renderer
scada.scheme.SchemeRenderer = function () {
    scada.scheme.Renderer.call(this);
};

scada.scheme.SchemeRenderer.prototype = Object.create(scada.scheme.Renderer.prototype);
scada.scheme.SchemeRenderer.constructor = scada.scheme.SchemeRenderer;

scada.scheme.SchemeRenderer.prototype._setTitle = function (title, renderContext) {
    let viewHub = renderContext.schemeEnv.viewHub;

    if (title && viewHub) {
        // set the document title
        document.title = title + " - " + viewHub.appEnv.productName;

        // set title of a popup in case the scheme is in the popup
        if (viewHub.modalManager.isModal(window)) {
            viewHub.modalManager.setTitle(window, document.title);
        }

        // send notification about title change
        viewHub.notifyMainWindow(ScadaEventType.UPDATE_TITLE);
    }
};

scada.scheme.SchemeRenderer.prototype._calcScale = function (component, scaleType, scaleValue) {
    var ScaleTypes = scada.scheme.ScaleTypes;

    if (scaleType === ScaleTypes.NUMERIC) {
        if (!isNaN(scaleValue) && scaleValue >= 0) {
            return scaleValue;
        }
    }
    else if (component.parentDomElem) {
        var areaWidth = component.parentDomElem.innerWidth();
        var schemeWidth = component.props.size.width;
        var horScale = areaWidth / schemeWidth;

        if (scaleType === ScaleTypes.FIT_SCREEN) {
            var schemeHeight = component.props.size.height;
            var areaHeight = component.parentDomElem.innerHeight();
            var vertScale = areaHeight / schemeHeight;
            return Math.min(horScale, vertScale);
        } else if (scaleType === ScaleTypes.FIT_WIDTH) {
            return horScale;
        }
    }

    return 1.0;
};

scada.scheme.SchemeRenderer.prototype.createDom = function (component, renderContext) {
    var divScheme = $("<div class='scheme'></div>");
    component.dom = divScheme;
    this.updateDom(component, renderContext);
};

scada.scheme.SchemeRenderer.prototype.refreshImages = function (component, renderContext, imageNames) {
    if (component.dom) {
        var props = component.props;

        if (Array.isArray(imageNames) && imageNames.includes(props.backImageName)) {
            var divSchemeBack = component.dom.find(".scheme-back:first");
            var backImage = renderContext.getImage(props.backImageName);
            this.setBackgroundImage(divSchemeBack, backImage, true);
        }
    }
};

// Update existing DOM content of the scheme according to the scheme properties
scada.scheme.SchemeRenderer.prototype.updateDom = function (component, renderContext) {
    if (component.dom) {
        var props = component.props; // scheme document properties
        var schemeWidth = props.size.width;
        var schemeHeight = props.size.height;

        var divScheme = component.dom.first();
        divScheme.css({
            "width": schemeWidth,
            "height": schemeHeight,
        });

        if (!renderContext.editMode) {
            this.setBackColor($("body"), props.backColor, true);
        }

        this.setBackColor(divScheme, props.backColor, true);
        this.setFont(divScheme, props.font, true);
        this.setForeColor(divScheme, props.foreColor, true);

        // set background image if presents,
        // the additional div is required for correct scaling
        var divSchemeBack = divScheme.children(".scheme-back:first");

        if (divSchemeBack.length === 0) {
            divSchemeBack = $("<div class='scheme-back'></div>");
            divScheme.append(divSchemeBack);
        }

        divSchemeBack.css({
            "width": schemeWidth,
            "height": schemeHeight,
            "background-size": schemeWidth + "px " + schemeHeight + "px",
            "background-repeat": "no-repeat"
        });

        var backImage = renderContext.getImage(props.backImageName);
        this.setBackgroundImage(divSchemeBack, backImage, true);

        // set title
        if (!renderContext.editMode) {
            this._setTitle(props.title, renderContext);
        }
    }
};

// Set the scheme scale
scada.scheme.SchemeRenderer.prototype.setScale = function (component, scaleType, scaleValue) {
    if (component.dom) {
        var scale = this._calcScale(component, scaleType, scaleValue);
        //var sizeCoef = Math.min(scale, 1);

        component.dom.css({
            "transform": "scale(" + scale + ", " + scale + ")",
            //"width": component.props.size.Width * sizeCoef,
            //"height": component.props.size.Height * sizeCoef
        });

        return scale;
    }
};

/********** Component Renderer **********/

// Abstract component renderer type extends scada.scheme.Renderer
scada.scheme.ComponentRenderer = function () {
    scada.scheme.Renderer.call(this);
};

scada.scheme.ComponentRenderer.prototype = Object.create(scada.scheme.Renderer.prototype);
scada.scheme.ComponentRenderer.constructor = scada.scheme.ComponentRenderer;

// Get a status color from the extended channel data
scada.scheme.ComponentRenderer.prototype._getStatusColor = function (cnlDataExt) {
    return MainApi.getColor(cnlDataExt, ColorIndex.MAIN_COLOR, this.STATUS_DISPLAY_COLOR);
};

// Get dynamic color that may depend on input channel status
scada.scheme.ComponentRenderer.prototype._getDynamicColor = function (color, cnlNum, renderContext) {
    if (color) {
        if (color === this.STATUS_COLOR) {
            return renderContext.curCnlDataMap
                ? this._getStatusColor(renderContext.curCnlDataMap.get(cnlNum))
                : this.STATUS_DISPLAY_COLOR;
        } else {
            return color;
        }
    } else {
        return "";
    }
};

// Set fore color of the jQuery object that may depend on input channel status
scada.scheme.ComponentRenderer.prototype.setDynamicForeColor = function (jqObj, foreColor,
    cnlNum, renderContext, opt_removeIfEmpty) {
    this.setForeColor(jqObj, this._getDynamicColor(foreColor, cnlNum, renderContext), opt_removeIfEmpty);
};

// Set background color of the jQuery object that may depend on input channel status
scada.scheme.ComponentRenderer.prototype.setDynamicBackColor = function (jqObj, backColor,
    cnlNum, renderContext, opt_removeIfEmpty) {
    this.setBackColor(jqObj, this._getDynamicColor(backColor, cnlNum, renderContext), opt_removeIfEmpty);
};

// Set border color of the jQuery object that may depend on input channel status
scada.scheme.ComponentRenderer.prototype.setDynamicBorderColor = function (jqObj, borderColor,
    cnlNum, renderContext, opt_removeIfEmpty) {
    this.setBorderColor(jqObj, this._getDynamicColor(borderColor, cnlNum, renderContext), opt_removeIfEmpty);
};

// Choose a color according to hover state
scada.scheme.ComponentRenderer.prototype.chooseColor = function (isHovered, color, colorOnHover) {
    return isHovered && colorOnHover ? colorOnHover : color;
};

// Set text wrapping of the jQuery object
scada.scheme.ComponentRenderer.prototype.setWordWrap = function (jqObj, wordWrap) {
    jqObj.css("white-space", wordWrap ? "normal" : "nowrap");
};

// Set horizontal algnment of the jQuery object
scada.scheme.ComponentRenderer.prototype.setHAlign = function (jqObj, hAlign) {
    var HorizontalAlignments = scada.scheme.HorizontalAlignments;

    switch (hAlign) {
        case HorizontalAlignments.CENTER:
            jqObj.css("text-align", "center");
            break;
        case HorizontalAlignments.RIGHT:
            jqObj.css("text-align", "right");
            break;
        default:
            jqObj.css("text-align", "left");
            break;
    }
};

// Set vertical algnment of the jQuery object
scada.scheme.ComponentRenderer.prototype.setVAlign = function (jqObj, vAlign) {
    var VerticalAlignments = scada.scheme.VerticalAlignments;

    switch (vAlign) {
        case VerticalAlignments.CENTER:
            jqObj.css("vertical-align", "middle");
            break;
        case VerticalAlignments.BOTTOM:
            jqObj.css("vertical-align", "bottom");
            break;
        default:
            jqObj.css("vertical-align", "top");
            break;
    }
};

// Set tooltip (title) of the jQuery object
scada.scheme.ComponentRenderer.prototype.setToolTip = function (jqObj, toolTip) {
    if (toolTip) {
        jqObj.prop("title", toolTip);
    }
};

// Set primary css properties of the jQuery object of the scheme component
scada.scheme.ComponentRenderer.prototype.prepareComponent = function (jqObj, component, opt_skipSize, opt_skipColors) {
    var props = component.props;
    jqObj
        .addClass("comp")
        .data("id", props.id)
        .css({
            "left": props.location.x,
            "top": props.location.y,
            "z-index": props.zIndex
        });

    if (!opt_skipSize) {
        jqObj.css({
            "width": props.size.width,
            "height": props.size.height
        });
    }

    if (!opt_skipColors) {
        this.setBackColor(jqObj, props.backColor);
        this.setBorderColor(jqObj, props.borderColor);
        this.setBorderWidth(jqObj, props.borderWidth);
    }

    this.setToolTip(jqObj, props.toolTip);
};

// Bind user action to the component
scada.scheme.ComponentRenderer.prototype.bindAction = function (jqObj, component, renderContext) {
    var Actions = scada.scheme.Actions;
    var props = component.props;
    var action = props.action;
    var actionIsBound =
        action === Actions.DRAW_DIAGRAM && props.inCnlNum > 0 ||
        (action === Actions.SEND_COMMAND || action === Actions.SEND_COMMAND_NOW) &&
        props.ctrlCnlNum > 0 && renderContext.controlRight;

    if (actionIsBound) {
        jqObj.addClass("action");

        if (!renderContext.editMode) {
            var viewHub = renderContext.schemeEnv.viewHub;

            jqObj.on("click", function () {
                switch (props.action) {
                    case Actions.DRAW_DIAGRAM:
                        viewHub.features.chart.show(props.inCnlNum, new Date().toISOString().slice(0, 10));
                        break;

                    case Actions.SEND_COMMAND:
                        viewHub.features.command.show(props.ctrlCnlNum);
                        break;

                    case Actions.SEND_COMMAND_NOW:
                        if (renderContext.schemeEnv) {
                            renderContext.schemeEnv.sendCommand(props.ctrlCnlNum, component.cmdVal,
                                renderContext.viewID, component.id);
                        } else {
                            console.warn("Scheme environment object is undefined");
                        }
                        break;
                }
            });
        }
    }
};

// Get location of the component. Returns an object containing the properties x and y
scada.scheme.ComponentRenderer.prototype.getLocation = function (component) {
    if (component.props && component.props.location) {
        return {
            x: component.props.location.x,
            y: component.props.location.y
        };
    } else {
        return {
            x: 0,
            y: 0
        };
    }
};

// Set location of the component
scada.scheme.ComponentRenderer.prototype.setLocation = function (component, x, y) {
    component.props.location = { x: x, y: y };

    if (component.dom) {
        var compParent = component.dom.parent();

        if (compParent.is(".comp-wrapper")) {
            compParent.css({
                "left": x - this.COMP_FRAME_WIDTH,
                "top": y - this.COMP_FRAME_WIDTH,
                "z-index": component.props.zIndex
            });

            component.dom.css({
                "left": "",
                "top": ""
            });
        } else {
            component.dom.css({
                "left": x,
                "top": y
            });
        }
    }
};

// Set properties of the component wrapper in edit mode
scada.scheme.ComponentRenderer.prototype.setWrapperProps = function (component) {
    // set location
    var location = this.getLocation(component);
    this.setLocation(component, location.x, location.y);

    // set font
    if (component.props && component.dom) {
        var wrapper = component.dom.parent(".comp-wrapper");
        this.setFont(wrapper, component.props.font, true);
    }
};

// Get size of the component. Returns an object containing the properties width and height
scada.scheme.ComponentRenderer.prototype.getSize = function (component) {
    if (component.props && component.props.size) {
        return {
            width: component.props.size.width,
            height: component.props.size.height
        };
    } else {
        return {
            width: 0,
            height: 0
        };
    }
};

// Set size of the component
scada.scheme.ComponentRenderer.prototype.setSize = function (component, width, height) {
    component.props.size = { width: width, height: height };

    if (component.dom) {
        component.dom.css({
            "width": width,
            "height": height
        });
    }
};

// Check the possibility of resizing in edit mode
scada.scheme.ComponentRenderer.prototype.allowResizing = function (component) {
    return true;
};

// Wrap the component with a frame needed in edit mode
scada.scheme.ComponentRenderer.prototype.wrap = function (component) {
    var compWrapper = $("<div class='comp-wrapper'></div>").append(component.dom);
    this.setWrapperProps(component);
    return compWrapper;
};

/********** Static Text Renderer **********/

// Static text renderer type extends scada.scheme.ComponentRenderer
scada.scheme.StaticTextRenderer = function () {
    scada.scheme.ComponentRenderer.call(this);
};

scada.scheme.StaticTextRenderer.prototype = Object.create(scada.scheme.ComponentRenderer.prototype);
scada.scheme.StaticTextRenderer.constructor = scada.scheme.StaticTextRenderer;

scada.scheme.StaticTextRenderer.prototype.createDom = function (component, renderContext) {
    var props = component.props;

    var spanComp = $("<span id='comp" + component.id + "'></span>");
    var spanText = $("<span></span>");

    this.prepareComponent(spanComp, component, true);
    this.setFont(spanComp, props.font);
    this.setForeColor(spanComp, props.foreColor);

    if (props.autoSize) {
        spanComp.css("display", "inline-block");
        this.setWordWrap(spanText, false);
    } else {
        spanComp.css("display", "table");
        var borders = props.borderWidth * 2;

        spanText.css({
            "display": "table-cell",
            "overflow": "hidden",
            "max-width": props.size.width - borders,
            "width": props.size.width - borders,
            "height": props.size.height - borders
        });

        this.setHAlign(spanComp, props.hAlign);
        this.setVAlign(spanText, props.vAlign);
        this.setWordWrap(spanText, props.wordWrap);
    }

    spanText.text(props.text);
    spanComp.append(spanText);
    component.dom = spanComp;
};

scada.scheme.StaticTextRenderer.prototype.setSize = function (component, width, height) {
    component.props.size = { width: width, height: height };

    var spanText = component.dom.children();
    spanText.css({
        "max-width": width,
        "width": width,
        "height": height
    });
};

scada.scheme.StaticTextRenderer.prototype.allowResizing = function (component) {
    return !component.props.autoSize;
};

/********** Dynamic Text Renderer **********/

// Dynamic text renderer type extends scada.scheme.StaticTextRenderer
scada.scheme.DynamicTextRenderer = function () {
    scada.scheme.StaticTextRenderer.call(this);
};

scada.scheme.DynamicTextRenderer.prototype = Object.create(scada.scheme.StaticTextRenderer.prototype);
scada.scheme.DynamicTextRenderer.constructor = scada.scheme.DynamicTextRenderer;

scada.scheme.DynamicTextRenderer.prototype._setUnderline = function (jqObj, underline) {
    if (underline) {
        jqObj.css("text-decoration", "underline");
    }
};

scada.scheme.DynamicTextRenderer.prototype._restoreUnderline = function (jqObj, font) {
    jqObj.css("text-decoration", font && font.Underline ? "underline" : "none");
};

scada.scheme.DynamicTextRenderer.prototype.createDom = function (component, renderContext) {
    scada.scheme.StaticTextRenderer.prototype.createDom.call(this, component, renderContext);

    var ShowValueKinds = scada.scheme.ShowValueKinds;
    var props = component.props;
    var spanComp = component.dom.first();
    var spanText = component.dom.children();
    var cnlNum = props.inCnlNum;

    if (props.showValue > ShowValueKinds.NOT_SHOW && !props.text) {
        spanText.text("[" + cnlNum + "]");
    }

    this.bindAction(spanComp, component, renderContext);

    // apply properties on hover
    var thisRenderer = this;

    spanComp.hover(
        function () {
            thisRenderer.setDynamicBackColor(spanComp, props.backColorOnHover, cnlNum, renderContext);
            thisRenderer.setDynamicBorderColor(spanComp, props.borderColorOnHover, cnlNum, renderContext);
            thisRenderer.setDynamicForeColor(spanComp, props.foreColorOnHover, cnlNum, renderContext);
            thisRenderer._setUnderline(spanComp, props.underlineOnHover);
        },
        function () {
            thisRenderer.setDynamicBackColor(spanComp, props.backColor, cnlNum, renderContext, true);
            thisRenderer.setDynamicBorderColor(spanComp, props.borderColor, cnlNum, renderContext, true);
            thisRenderer.setDynamicForeColor(spanComp, props.foreColor, cnlNum, renderContext, true);
            thisRenderer._restoreUnderline(spanComp, props.font);
        }
    );
};

scada.scheme.DynamicTextRenderer.prototype.updateData = function (component, renderContext) {
    var props = component.props;

    if (props.inCnlNum > 0) {
        var ShowValueKinds = scada.scheme.ShowValueKinds;
        var spanComp = component.dom;
        var spanText = spanComp.children();
        var cnlProps = renderContext.getCnlProps(props.inCnlNum);
        var cnlDataExt = renderContext.getCnlDataExt(props.inCnlNum, cnlProps.joinLen);

        // show value of the appropriate input channel
        switch (props.showValue) {
            case ShowValueKinds.SHOW_WITH_UNIT:
                spanText.text(MainApi.getDisplayValue(cnlDataExt, cnlProps.unit));
                break;
            case ShowValueKinds.SHOW_WITHOUT_UNIT:
                spanText.text(cnlDataExt.df.dispVal);
                break;
        }

        // choose and set colors of the component
        var isHovered = spanComp.is(":hover");
        var backColor = this.chooseColor(isHovered, props.backColor, props.backColorOnHover);
        var borderColor = this.chooseColor(isHovered, props.borderColor, props.borderColorOnHover);
        var foreColor = this.chooseColor(isHovered, props.foreColor, props.foreColorOnHover);
        var statusColor = this._getStatusColor(cnlDataExt);

        this.setBackColor(spanComp, backColor, true, statusColor);
        this.setBorderColor(spanComp, borderColor, true, statusColor);
        this.setForeColor(spanComp, foreColor, true, statusColor);
    }
};

/********** Static Picture Renderer **********/

// Static picture renderer type extends scada.scheme.ComponentRenderer
scada.scheme.StaticPictureRenderer = function () {
    scada.scheme.ComponentRenderer.call(this);
};

scada.scheme.StaticPictureRenderer.prototype = Object.create(scada.scheme.ComponentRenderer.prototype);
scada.scheme.StaticPictureRenderer.constructor = scada.scheme.StaticPictureRenderer;

scada.scheme.StaticPictureRenderer.prototype.createDom = function (component, renderContext) {
    var ImageStretches = scada.scheme.ImageStretches;
    var props = component.props;

    var divComp = $("<div id='comp" + component.id + "'></div>");
    this.prepareComponent(divComp, component);

    // set image
    switch (props.imageStretch) {
        case ImageStretches.FILL:
            var borders = props.borderWidth * 2;
            divComp.css("background-size", props.size.width - borders + "px " +
                (props.size.height - borders) + "px");
            break;
        case ImageStretches.ZOOM:
            divComp.css("background-size", "contain");
            break;
    }

    divComp.css({
        "background-repeat": "no-repeat",
        "background-position": "center",
        "background-clip": "padding-box"
    });

    var image = renderContext.getImage(props.imageName);
    this.setBackgroundImage(divComp, image);

    component.dom = divComp;
};

scada.scheme.StaticPictureRenderer.prototype.refreshImages = function (component, renderContext, imageNames) {
    var props = component.props;

    if (Array.isArray(imageNames) && imageNames.includes(props.imageName)) {
        var divComp = component.dom;
        var image = renderContext.getImage(props.imageName);
        this.setBackgroundImage(divComp, image, true);
    }
};

scada.scheme.StaticPictureRenderer.prototype.setSize = function (component, width, height) {
    scada.scheme.ComponentRenderer.prototype.setSize.call(this, component, width, height);

    var ImageStretches = scada.scheme.ImageStretches;
    var props = component.props;

    if (props.imageStretch === ImageStretches.FILL) {
        var divComp = component.dom;
        divComp.css("background-size", props.size.width + "px " + props.size.height + "px");
    }
};

/********** Dynamic Picture Renderer **********/

// Dynamic picture renderer type extends scada.scheme.StaticPictureRenderer
scada.scheme.DynamicPictureRenderer = function () {
    scada.scheme.StaticPictureRenderer.call(this);
};

scada.scheme.DynamicPictureRenderer.prototype = Object.create(scada.scheme.StaticPictureRenderer.prototype);
scada.scheme.DynamicPictureRenderer.constructor = scada.scheme.DynamicPictureRenderer;

scada.scheme.DynamicPictureRenderer.prototype.createDom = function (component, renderContext) {
    scada.scheme.StaticPictureRenderer.prototype.createDom.call(this, component, renderContext);

    var props = component.props;
    var divComp = component.dom;

    this.bindAction(divComp, component, renderContext);

    // apply properties on hover
    var thisRenderer = this;
    var cnlNum = props.inCnlNum;

    divComp.hover(
        function () {
            thisRenderer.setDynamicBackColor(divComp, props.backColorOnHover, cnlNum, renderContext);
            thisRenderer.setDynamicBorderColor(divComp, props.borderColorOnHover, cnlNum, renderContext);

            if (cnlNum <= 0) {
                var image = renderContext.getImage(props.imageOnHoverName);
                thisRenderer.setBackgroundImage(divComp, image);
            }
        },
        function () {
            thisRenderer.setDynamicBackColor(divComp, props.backColor, cnlNum, renderContext, true);
            thisRenderer.setDynamicBorderColor(divComp, props.borderColor, cnlNum, renderContext, true);

            if (cnlNum <= 0) {
                var image = renderContext.getImage(props.imageName);
                thisRenderer.setBackgroundImage(divComp, image, true);
            }
        }
    );
};

scada.scheme.DynamicPictureRenderer.prototype.updateData = function (component, renderContext) {
    var props = component.props;

    if (props.inCnlNum > 0) {
        var divComp = component.dom;
        var cnlDataExt = renderContext.getCnlDataExt(props.inCnlNum);
        var imageName = props.imageName;

        // choose an image depending on the conditions
        if (cnlDataExt.d.stat > 0 && props.conditions) {
            var cnlVal = cnlDataExt.d.val;

            for (var cond of props.conditions) {
                if (scada.scheme.calc.conditionSatisfied(cond, cnlVal)) {
                    imageName = cond.imageName;
                    break;
                }
            }
        }

        // set the image
        var image = renderContext.imageMap.get(imageName);
        this.setBackgroundImage(divComp, image, true);

        // choose and set colors of the component
        var isHovered = divComp.is(":hover");
        var backColor = this.chooseColor(isHovered, props.backColor, props.backColorOnHover);
        var borderColor = this.chooseColor(isHovered, props.borderColor, props.borderColorOnHover);
        var statusColor = this._getStatusColor(cnlDataExt);

        this.setBackColor(divComp, backColor, true, statusColor);
        this.setBorderColor(divComp, borderColor, true, statusColor);
    }
};

/********** Unknown Component Renderer **********/

scada.scheme.UnknownComponentRenderer = function () {
    scada.scheme.Renderer.call(this);
};

scada.scheme.UnknownComponentRenderer.prototype = Object.create(scada.scheme.Renderer.prototype);
scada.scheme.UnknownComponentRenderer.constructor = scada.scheme.UnknownComponentRenderer;

/********** Render Context **********/

// Render context type
scada.scheme.RenderContext = function () {
    this.curCnlDataMap = null;
    this.cnlPropsMap = null;
    this.editMode = false;
    this.schemeEnv = null;
    this.viewID = 0;
    this.imageMap = null;
    this.controlRight = true;
};

// Get a non-null channel data by the specified channel number
scada.scheme.RenderContext.prototype.getCnlDataExt = function (cnlNum, opt_joinLen) {
    return MainApi.getCurDataFromMap(this.curCnlDataMap, cnlNum, opt_joinLen);
};

// Get a non-null channel properties by the specified channel number
scada.scheme.RenderContext.prototype.getCnlProps = function (cnlNum) {
    let cnlProps = this.cnlPropsMap ? this.cnlPropsMap.get(cnlNum) : null;

    if (!cnlProps) {
        cnlProps = {
            cnlNum: 0,
            joinLen: 1,
            unit: ""
        }
    }

    return cnlProps;
};

// Get scheme image object by the image name
scada.scheme.RenderContext.prototype.getImage = function (imageName) {
    return this.imageMap ? this.imageMap.get(imageName) : null;
};

/********** Renderer Map **********/

// Renderer map object
scada.scheme.rendererMap = new Map([
    ["Scada.Web.Plugins.PlgScheme.Model.StaticText", new scada.scheme.StaticTextRenderer()],
    ["Scada.Web.Plugins.PlgScheme.Model.DynamicText", new scada.scheme.DynamicTextRenderer()],
    ["Scada.Web.Plugins.PlgScheme.Model.StaticPicture", new scada.scheme.StaticPictureRenderer()],
    ["Scada.Web.Plugins.PlgScheme.Model.DynamicPicture", new scada.scheme.DynamicPictureRenderer()],
    ["Scada.Web.Plugins.PlgScheme.Model.UnknownComponent", new scada.scheme.UnknownComponentRenderer()]
]);
