// Contains classes: Renderer, MimicRenderer, ComponentRenderer, RegularComponentRenderer,
//     TextRenderer, PictureRenderer, PanelRenderer, RenderContext, RendererSet, UnitedRenderer
// Depends on jquery, scada-common.js, modal.js, mimic-common.js, mimic-model.js

// Represents a renderer of a mimic or component.
rs.mimic.Renderer = class {
    // Sets the background image of the specified jQuery object.
    _setBackgroundImage(jqObj, image) {
        jqObj.css("background-image", this._imageToDataUrlCss(image));
    }

    // Sets the background size of the specified jQuery object.
    _setBackgroundStretch(jqObj, imageStretch, innerWidth, innerHeight) {
        const ImageStretch = rs.mimic.ImageStretch;
        jqObj.css("background-position", "center center");

        switch (imageStretch) {
            case ImageStretch.NONE:
                jqObj.css("background-size", "");
                break;

            case ImageStretch.FILL:
                jqObj.css("background-size", `${innerWidth}px ${innerHeight}px`);
                break;

            case ImageStretch.ZOOM:
                jqObj.css("background-size", "contain");
                break;
        }
    }

    // Sets the border of the specified jQuery object.
    _setBorder(jqObj, border) {
        if (border && border.width > 0) {
            jqObj.css({
                "border-width": border.width,
                "border-style": "solid",
                "border-color": border.color
            });
        } else {
            jqObj.css("border", "");
        }
    }

    // Sets the corner radius of the specified jQuery object.
    _setCornerRadius(jqObj, cornerRadius) {
        if (cornerRadius) {
            jqObj.css({
                "border-top-left-radius": cornerRadius.topLeft,
                "border-top-right-radius": cornerRadius.topRight,
                "border-bottom-right-radius": cornerRadius.bottomRight,
                "border-bottom-left-radius": cornerRadius.bottomLeft
            });
        } else {
            jqObj.css("border-radius", "");
        }
    }

    // Sets the font of the specified jQuery object.
    _setFont(jqObj, font, fontMap) {
        if (!font || font.inherit) {
            jqObj.css({
                "font-family": "",
                "font-size": "",
                "font-weight": "",
                "font-style": "",
                "text-decoration": "" // not inherited
            });
        } else {
            jqObj.css({
                "font-family": fontMap?.get(font.name)?.family,
                "font-size": font.size,
                "font-weight": font.bold ? "bold" : "normal",
                "font-style": font.italic ? "italic" : "normal",
                "text-decoration": font.underline ? "underline" : "none"
            });
        }
    }

    // Sets the left and top of the specified jQuery object.
    _setLocation(jqObj, location) {
        jqObj.css({
            "left": location.x + "px",
            "top": location.y + "px"
        });
    }

    // Sets the padding of the specified jQuery object.
    _setPadding(jqObj, padding) {
        jqObj.css({
            "padding-top": padding.top,
            "padding-right": padding.right,
            "padding-bottom": padding.bottom,
            "padding-left": padding.left
        });
    }

    // Sets the rotation of the specified jQuery object.
    _setRotation(jqObj, rotation) {
        jqObj.css({
            "transform": `rotate(${rotation}deg)`,
            "transform-origin": "center"
        });
    }

    // Sets the width and height of the specified jQuery object.
    _setSize(jqObj, size) {
        jqObj.css({
            "width": size.width + "px",
            "height": size.height + "px"
        });
    }

    // Returns a css property value for the image data URI.
    _imageToDataUrlCss(image) {
        return image ? "url('" + image.dataUrl + "')" : "";
    }

    // Creates a component DOM according to the component model. Returns a jQuery object or null.
    createDom(component, renderContext) {
        return null;
    }

    // Updates the existing component DOM according to the component model. Returns a jQuery object or null.
    updateDom(component, renderContext) {
        return null;
    }

    // Sets the size of the component DOM.
    setSize(component, width, height) {
        if (component.dom) {
            this._setSize(component.dom, {
                width: width,
                height: height
            });
        }
    }

    // Gets the component size from its DOM.
    getSize(component) {
        if (component.dom) {
            return {
                width: parseInt(component.dom.outerWidth()),
                height: parseInt(component.dom.outerHeight())
            };
        } else {
            return {
                width: 0,
                height: 0
            };
        }
    }

    // Appends the child DOM into the parent DOM.
    appendChild(parent, child) {
        if (parent.dom && child.dom) {
            parent.dom.append(child.dom);
        }
    }

    // Removes the component DOM from the mimic keeping data associated with the removed elements.
    detach(component) {
        component.dom?.detach();
    }

    // Removes the component DOM from the mimic.
    remove(component) {
        component.dom?.remove();
    }

    // Arranges the child component DOMs according to their order.
    static arrangeChildren(parent) {
        if (parent && parent.children) {
            for (let component of parent.children) {
                component.renderer?.detach(component);
            }

            for (let component of parent.children) {
                parent.renderer?.appendChild(parent, component);
            }
        }
    }
};

// Represents a mimic renderer.
rs.mimic.MimicRenderer = class MimicRenderer extends rs.mimic.Renderer {
    static _GRID_COLOR = "#dee2e6"; // gray-300

    // Checks whether to display grid.
    static _gridVisible(renderContext) {
        return renderContext.editMode && renderContext.editorOptions &&
            renderContext.editorOptions.showGrid && renderContext.editorOptions.gridStep > 1;
    }

    // Creates a grid canvas and draws grid cells.
    static _createGrid(gridSize, mimicWidth, mimicHeight) {
        // create canvas
        let canvasElem = $("<canvas class='grid'></canvas>");
        let canvas = canvasElem[0];
        let width = canvas.width = mimicWidth;
        let height = canvas.height = mimicHeight;

        // prepare drawing context
        let context = canvas.getContext("2d");
        context.lineWidth = 1;
        context.strokeStyle = MimicRenderer._GRID_COLOR;

        // draw grids with small and large cells
        MimicRenderer._drawGrid(context, width, height, gridSize, [1, 1]);
        MimicRenderer._drawGrid(context, width, height, gridSize * 10);
        return canvasElem;
    }

    // Draws grid cells.
    static _drawGrid(context, width, height, gridStep, dashSegments = []) {
        const adj = 0.5; // adjustment for sharpness of lines

        // draw vertical lines
        for (let x = gridStep; x < width; x += gridStep) {
            context.beginPath();
            context.setLineDash(dashSegments);
            context.moveTo(x + adj, 0);
            context.lineTo(x + adj, height);
            context.stroke();
        }

        // draw horizontal lines
        for (let y = gridStep; y < height; y += gridStep) {
            context.beginPath();
            context.setLineDash(dashSegments);
            context.moveTo(0, y + adj);
            context.lineTo(width, y + adj);
            context.stroke();
        }
    }

    // Calculates the scale value depending on the scale type and mimic size.
    static _calcScaleValue(mimic, scale) {
        const ScaleType = rs.mimic.ScaleType;

        if (scale.type === ScaleType.NUMERIC) {
            if (scale.value >= 0) {
                return scale.value;
            }
        } else {
            let parentDom = mimic.dom?.parent();

            if (parentDom && mimic.width > 0 && mimic.height > 0) {
                let areaWidth = parentDom.innerWidth();
                let horScale = areaWidth / mimic.width;

                if (scale.type === ScaleType.FIT_WIDTH) {
                    return horScale;
                } else if (scale.type === ScaleType.FIT_SCREEN) {
                    let areaHeight = parentDom.innerHeight();
                    let vertScale = areaHeight / mimic.height;
                    return Math.min(horScale, vertScale);
                }
            }
        }

        return 1.0;
    }

    // Adds, replaces or removes a style element in the page head.
    static _updateStyleElem(stylesheet) {
        if (stylesheet) {
            let newStyleElem = $("<style id='mimic-style'></style>").html(stylesheet);
            let oldStyleElem = $("head").find("#mimic-style");

            if (oldStyleElem.length > 0) {
                oldStyleElem.replaceWith(newStyleElem);
            } else {
                $("head").append(newStyleElem);
            }
        } else {
            $("head").find("#mimic-style").remove();
        }
    }

    // Updates a style element according to the stylesheets of the mimic and faceplates.
    static _setStylesheet(mimic) {
        let stylesheet = mimic.document.stylesheet;

        for (let faceplate of mimic.faceplates) {
            stylesheet += faceplate.document.stylesheet;
        }

        MimicRenderer._updateStyleElem(stylesheet);
    }

    // Sets the body background color in edit mode.
    static _setBodyBackColor(mimic, renderContext) {
        if (!renderContext.editMode) {
            $("body").css("background-color", mimic.document.backColor);
        }
    }

    // Sets the CSS classes of the mimic element.
    _setClasses(mimicElem, mimic, renderContext) {
        mimicElem.removeClass(); // clear classes
        mimicElem.addClass("mimic");
        let props = mimic.document;

        if (props.cssClass) {
            mimicElem.addClass(props.cssClass);
        }
}

    // Sets the CSS properties of the mimic element.
    _setProps(mimicElem, mimic, renderContext) {
        let props = mimic.document;
        this._setFont(mimicElem, props.font, renderContext.fontMap);
        this._setSize(mimicElem, props.size);

        if (mimic.isFaceplate) {
            this._setBorder(mimicElem, props.border);
            this._setCornerRadius(mimicElem, props.cornerRadius);
        }

        mimicElem
            .attr("title", props.tooltip)
            .css({
                "background-color": props.backColor,
                "color": props.foreColor
            });

        if (props.backgroundImage) {
            let x = props.backgroundPadding.left;
            let y = props.backgroundPadding.top;
            let w = props.size.width - x - props.backgroundPadding.right;
            let h = props.size.height - y - props.backgroundPadding.bottom;

            mimicElem.css({
                "background-image": this._imageToDataUrlCss(renderContext.getImage(props.backgroundImage)),
                "background-position": `${x}px ${y}px`,
                "background-size": `${w}px ${h}px`
            });
        } else {
            mimicElem.css({
                "background-image": "",
                "background-position": "",
                "background-size": ""
            });
        }
    }

    // Creates a mimic DOM according to the mimic model.
    createDom(mimic, renderContext) {
        let mimicElem = $("<div></div>");
        MimicRenderer._setStylesheet(mimic);
        MimicRenderer._setBodyBackColor(mimic, renderContext);

        if (MimicRenderer._gridVisible(renderContext)) {
            mimicElem.append(MimicRenderer._createGrid(
                renderContext.editorOptions.gridStep, mimic.innerWidth, mimic.innerHeight));
        }

        this._setClasses(mimicElem, mimic, renderContext);
        this._setProps(mimicElem, mimic, renderContext);
        mimic.dom = mimicElem;
        return mimicElem;
    }

    // Updates the existing mimic DOM according to the mimic model.
    updateDom(mimic, renderContext) {
        let mimicElem = mimic.dom;
        MimicRenderer._setStylesheet(mimic);
        MimicRenderer._setBodyBackColor(mimic, renderContext);

        if (mimicElem) {
            if (MimicRenderer._gridVisible(renderContext)) {
                mimicElem.children(".grid:first").replaceWith(MimicRenderer._createGrid(
                    renderContext.editorOptions.gridStep, mimic.innerWidth, mimic.innerHeight));
            }

            this._setClasses(mimicElem, mimic, renderContext);
            this._setProps(mimicElem, mimic, renderContext);
        }

        return mimicElem;
    }

    // Sets the CSS properties of the mimic element.
    setMimicProps(mimicElem, mimic, renderContext) {
        this._setProps(mimicElem, mimic, renderContext);
    }

    // Sets the scale of the mimic DOM.
    setScale(mimic, scale) {
        if (mimic.dom) {
            let scaleValue = MimicRenderer._calcScaleValue(mimic, scale);

            if (scale.type !== rs.mimic.ScaleType.NUMERIC) {
                scale.setValue(scaleValue);
            }

            mimic.dom.css({
                "transform": `scale(${scale.value})`
            });
        }
    }
};

// Represents a component renderer.
rs.mimic.ComponentRenderer = class extends rs.mimic.Renderer {
    // Gets a value indicating whether the renderer can update the existing component DOM without recreating it.
    get canUpdateDom() {
        return true;
    }

    // Completes the creation of the component DOM.
    _completeDom(componentElem, component, renderContext) {
    }

    // Sets the CSS classes of the component element.
    _setClasses(componentElem, component, renderContext) {
        let unchangedClasses = this._keepClasses(componentElem);
        componentElem.removeClass(); // clear classes
        componentElem.addClass("comp");
        componentElem.addClass(unchangedClasses.join(" "));
        let props = component.properties;

        if (!props.enabled) {
            componentElem.addClass("disabled-state");
            componentElem.addClass("disabled"); // Bootstrap class
        }

        if (!props.visible) {
            componentElem.addClass("invisible-state");
        }

        if (this._hasAction(props, renderContext)) {
            componentElem.addClass("has-action");
        }

        if (renderContext.editMode) {
            if (!renderContext.faceplateMode && component.isContainer) {
                componentElem.addClass("container");
            }

            if (component.isSelected) {
                componentElem.addClass("selected");
            }
        }
    }

    // Collects classes that should remain unchanged during an update.
    _keepClasses(componentElem) {
        let classes = [];

        if (componentElem.hasClass("blink-on")) {
            classes.push("blink-on");
        }

        if (componentElem.hasClass("wait-action")) {
            classes.push("wait-action");
        }

        return classes;
    }

    // Sets the CSS properties of the component element.
    _setProps(componentElem, component, renderContext) {
        let props = component.properties;
        this._setLocation(componentElem, props.location);
        this._setSize(componentElem, props.size);
    }

    // Binds events to the component element.
    _bindEvents(componentElem, component, renderContext) {
        let props = component.properties;
        componentElem.off(".rs.mimic");

        if (props.enabled && this._hasAction(props, renderContext)) {
            componentElem.on("click.rs.mimic", () => {
                this._executeAction(componentElem, component, renderContext);
            });
        }
    }

    // Checks whether an action is set for the component.
    _hasAction(props, renderContext) {
        const ActionType = rs.mimic.ActionType;
        return !renderContext.editMode && props.enabled &&
            props.clickAction.actionType !== ActionType.NONE &&
            (props.clickAction.actionType !== ActionType.SEND_COMMAND || renderContext.controlRight);
    }

    // Executes a component action.
    _executeAction(componentElem, component, renderContext) {
        const ActionType = rs.mimic.ActionType;
        const ActionScriptArgs = rs.mimic.ActionScriptArgs;
        const LinkTarget = rs.mimic.LinkTarget;
        let props = component.properties;
        let action = props.clickAction;

        if (!renderContext.viewHub) {
            console.error("View hub is undefined.");
            return;
        }

        if (!renderContext.mainApi) {
            console.error("Main API is undefined.");
            return;
        }

        switch (action.actionType) {
            case ActionType.DRAW_CHART:
                if (props.inCnlNum > 0) {
                    renderContext.viewHub.features.chart.show(props.inCnlNum, null, action.chartArgs);
                } else {
                    console.warn("Input channel not specified.");
                }
                break;

            case ActionType.SEND_COMMAND:
                if (props.outCnlNum > 0) {
                    if (action.commandArgs.showDialog) {
                        renderContext.viewHub.features.command.show(props.outCnlNum);
                    } else {
                        this._showWait(componentElem);
                        let cmdVal = this._getCommandValue(component, action.commandArgs.cmdVal);

                        if (Number.isFinite(cmdVal)) {
                            console.log(`Send command ${cmdVal} to channel ${props.outCnlNum}`);
                            renderContext.mainApi.sendCommand(props.outCnlNum, cmdVal, false, null);
                        } else {
                            console.warn("Command cancelled.");
                        }
                    }
                } else {
                    console.warn("Output channel not specified.");
                }
                break;

            case ActionType.OPEN_LINK:
                let url;

                if (action.linkArgs.viewID > 0) {
                    url = viewHub.getViewUrl(action.linkArgs.viewID, action.linkArgs.target === LinkTarget.NEW_MODAL);
                } else if (action.linkArgs.urlParams.enabled) {
                    url = ScadaUtils.formatString(action.linkArgs.url, ...action.linkArgs.urlParams.toArray());
                } else {
                    url = action.linkArgs.url;
                }

                if (url) {
                    switch (action.linkArgs.target) {
                        case LinkTarget.SELF:
                            window.top.location = url;
                            break;
                        case LinkTarget.NEW_TAB:
                            window.open(url);
                            break;
                        case LinkTarget.NEW_MODAL:
                            renderContext.viewHub.modalManager.showModal(url, new ModalOptions({
                                size: action.linkArgs.getModalSize(),
                                height: action.linkArgs.modalHeight
                            }));
                            break;
                    }
                } else {
                    console.warn("URL is undefined.");
                }

                break;

            case ActionType.EXECUTE_SCRIPT:
                if (action.script) {
                    try {
                        let actionFunc = new Function("args", `const fn = ${action.script}; return fn(args);`);
                        actionFunc(new ActionScriptArgs({ component, renderContext }));
                    } catch (ex) {
                        console.error("Error executing action script: " + ex.message);
                    }
                } else {
                    console.warn("Script is undefined.");
                }
                break;
        }
    }

    // Shows a wait cursor over the component.
    _showWait(componentElem) {
        const WAIT_DURATION = 1000;
        componentElem.addClass("wait-action");
        setTimeout(() => { componentElem.removeClass("wait-action"); }, WAIT_DURATION);
    }

    // Gets a command value for the component action.
    _getCommandValue(component, defaultValue) {
        try {
            let cmdVal = component.getCommandValue();
            return Number.isFinite(cmdVal) ? cmdVal : defaultValue;
        } catch (ex) {
            console.error("Error getting command value: " + ex.message);
            return Number.NaN;
        }
    }

    // Sets the component colors and text decoration to the specified ones.
    _setVisualState(componentElem, visualState) {
        if (visualState.backColor) {
            componentElem.css("background-color", visualState.backColor);
        }

        if (visualState.foreColor) {
            componentElem.css("color", visualState.foreColor);
        }

        if (visualState.borderColor) {
            componentElem.css("border-color", visualState.borderColor);
        }

        if (visualState.underline) {
            componentElem.css("text-decoration", "underline");
        }
    }

    // Sets the component colors and text decoration according to its actual state.
    _restoreVisualState(componentElem, props) {
        let isBlinking = props.blinkingState.isSet && componentElem.hasClass("blink-on");
        let isHovered = props.hoverState.isSet && componentElem.is(":hover");

        if (isBlinking) {
            this._setVisualState(componentElem, props.blinkingState);
        } else if (isHovered) {
            this._setVisualState(componentElem, props.hoverState);
        } else {
            // original state
            componentElem.css({
                "background-color": props.backColor,
                "color": props.foreColor,
                "border-color": props.border.color,
                "text-decoration": props.font.inherit ? "" : (props.font.underline ? "underline" : "none")
            });
        }
    }

    // Binds the visual state events of the component.
    _bindVisualStates(componentElem, props) {
        const EventType = rs.mimic.EventType;

        if (props.blinkingState.isSet) {
            componentElem
                .on(EventType.BLINK_ON, () => { this._setVisualState(componentElem, props.blinkingState); })
                .on(EventType.BLINK_OFF, () => { this._restoreVisualState(componentElem, props); });
        }

        if (props.hoverState.isSet) {
            componentElem
                .on("mouseenter.rs.mimic", () => { this._setVisualState(componentElem, props.hoverState); })
                .on("mouseleave.rs.mimic", () => { this._restoreVisualState(componentElem, props); });
        }
    }

    // Creates a component DOM according to the component model.
    createDom(component, renderContext) {
        let componentElem = $("<div></div>")
            .attr("id", "comp" + renderContext.idPrefix + component.id)
            .attr("data-id", component.id)
            .attr("data-name", component.properties.name);
        this._completeDom(componentElem, component, renderContext);
        this._setClasses(componentElem, component, renderContext);
        this._setProps(componentElem, component, renderContext);
        this._bindEvents(componentElem, component, renderContext);
        component.dom = componentElem;
        return componentElem;
    }

    // Updates the existing component DOM according to the component model.
    updateDom(component, renderContext) {
        let componentElem = component.dom;

        if (componentElem) {
            this._setClasses(componentElem, component, renderContext);
            this._setProps(componentElem, component, renderContext);
            this._bindEvents(componentElem, component, renderContext);
        }

        return componentElem;
    }

    // Sets the location of the component DOM without changing the component model.
    setLocation(component, x, y) {
        if (component.dom) {
            this._setLocation(component.dom, {
                x: x,
                y: y
            });
        }
    }

    // Gets the component location from its DOM.
    getLocation(component) {
        if (component.dom) {
            let position = component.dom.position();
            return {
                x: parseInt(position.left),
                y: parseInt(position.top)
            };
        } else {
            return {
                x: 0,
                y: 0
            };
        }
    }

    // Updates the location of the component DOM according to the component model.
    updateLocation(component) {
        if (component.dom) {
            this._setLocation(component.dom, component.properties.location);
        }
    }

    // Visually selects or deselects the component.
    updateSelected(component) {
        if (component.dom) {
            component.dom.toggleClass("selected", component.isSelected);
        }
    }

    // Gets a value indicating whether the component can be resized by a user.
    allowResizing(component) {
        return true;
    }
};

// Represents a renderer for regular non-faceplate components.
rs.mimic.RegularComponentRenderer = class extends rs.mimic.ComponentRenderer {
    _setClasses(componentElem, component, renderContext) {
        super._setClasses(componentElem, component, renderContext);
        let props = component.properties;

        if (props.cssClass) {
            componentElem.addClass(props.cssClass);
        }
    }

    _setProps(componentElem, component, renderContext) {
        super._setProps(componentElem, component, renderContext);
        let props = component.properties;
        this._setBorder(componentElem, props.border);
        this._setCornerRadius(componentElem, props.cornerRadius);
        this._setFont(componentElem, props.font, renderContext.fontMap);
        componentElem.attr("title", props.tooltip);

        if (props.enabled) {
            this._restoreVisualState(componentElem, props);
        } else {
            this._setVisualState(componentElem, props.disabledState);
        }

        if (renderContext.editMode) {
            componentElem.css("--border-width", -props.border.width + "px");
        }
    }

    _bindEvents(componentElem, component, renderContext) {
        super._bindEvents(componentElem, component, renderContext);
        let props = component.properties;

        if (props.enabled) {
            this._bindVisualStates(componentElem, component.properties);
        }
    }
};

// Represents a text component renderer.
rs.mimic.TextRenderer = class extends rs.mimic.RegularComponentRenderer {
    _completeDom(componentElem, component, renderContext) {
        $("<div class='text-content'></div>").appendTo(componentElem);
    }

    _setClasses(componentElem, component, renderContext) {
        super._setClasses(componentElem, component, renderContext);
        componentElem.addClass("text");
    }

    _setProps(componentElem, component, renderContext) {
        super._setProps(componentElem, component, renderContext);
        let contentElem = componentElem.find(".text-content:first");
        let props = component.properties;
        this._setPadding(contentElem, props.padding);
        this._setTextDirection(contentElem, props.textDirection);
        contentElem.text(props.text || props.inCnlNum <= 0 ? props.text : `[${props.inCnlNum}]`);

        if (props.autoSize) {
            componentElem.css({
                "width": "",
                "height": ""
            });

            contentElem.css({
                "display": "inline",
                "white-space": "nowrap",
                "width": "",
                "height": ""
            });
        } else {
            contentElem
                .css({
                    "display": "flex",
                    "white-space": props.wordWrap ? "normal" : "nowrap",
                    "width": "100%",
                    "height": "100%"
                });
            this._setTextAlign(contentElem, props.textAlign);
        }
    }

    _setTextAlign(jqObj, contentAlignment) {
        const ContentAlignment = rs.mimic.ContentAlignment;

        switch (contentAlignment) {
            case ContentAlignment.TOP_LEFT:
                jqObj.css({
                    "align-items": "flex-start",
                    "justify-content": "flex-start",
                    "text-align": "left"
                });
                break;

            case ContentAlignment.TOP_CENTER:
                jqObj.css({
                    "align-items": "flex-start",
                    "justify-content": "center",
                    "text-align": "center"
                });
                break;

            case ContentAlignment.TOP_RIGHT:
                jqObj.css({
                    "align-items": "flex-start",
                    "justify-content": "flex-end",
                    "text-align": "right"
                });
                break;

            case ContentAlignment.MIDDLE_LEFT:
                jqObj.css({
                    "align-items": "center",
                    "justify-content": "flex-start",
                    "text-align": "left"
                });
                break;

            case ContentAlignment.MIDDLE_CENTER:
                jqObj.css({
                    "align-items": "center",
                    "justify-content": "center",
                    "text-align": "center"
                });
                break;

            case ContentAlignment.MIDDLE_RIGHT:
                jqObj.css({
                    "align-items": "center",
                    "justify-content": "flex-end",
                    "text-align": "right"
                });
                break;

            case ContentAlignment.BOTTOM_LEFT:
                jqObj.css({
                    "align-items": "flex-end",
                    "justify-content": "flex-start",
                    "text-align": "left"
                });
                break;

            case ContentAlignment.BOTTOM_CENTER:
                jqObj.css({
                    "align-items": "flex-end",
                    "justify-content": "center",
                    "text-align": "center"
                });
                break;

            case ContentAlignment.BOTTOM_RIGHT:
                jqObj.css({
                    "align-items": "flex-end",
                    "justify-content": "flex-end",
                    "text-align": "right"
                });
                break;

            default:
                jqObj.css({
                    "align-items": "",
                    "justify-content": "",
                    "text-align": ""
                });
                break;
        }
    }

    _setTextDirection(jqObj, textDirection) {
        const TextDirection = rs.mimic.TextDirection;

        switch (textDirection) {
            case TextDirection.VERTICAL90:
                jqObj.css("writing-mode", "vertical-rl");
                break;

            case TextDirection.VERTICAL270:
                jqObj.css("writing-mode", "sideways-lr");
                break;

            default:
                jqObj.css("writing-mode", "");
                break;
        }
    }

    allowResizing(component) {
        return !component.properties.autoSize;
    }
};

// Represents a picture component renderer.
rs.mimic.PictureRenderer = class extends rs.mimic.RegularComponentRenderer {
    _completeDom(componentElem, component, renderContext) {
        componentElem.append("<div class='picture-content'></div>");
    }

    _setClasses(componentElem, component, renderContext) {
        super._setClasses(componentElem, component, renderContext);
        componentElem.addClass("picture");

        if (renderContext.editMode && !component.properties.imageName) {
            componentElem.addClass("blank");
        }
    }

    _setProps(componentElem, component, renderContext) {
        super._setProps(componentElem, component, renderContext);
        let contentElem = componentElem.find(".picture-content:first");
        let props = component.properties;
        this._setPadding(componentElem, props.padding);
        this._setBackgroundImage(contentElem, renderContext.getImage(props.imageName));
        this._setBackgroundStretch(contentElem, props.imageStretch, component.innerWidth, component.innerHeight);
        this._setRotation(contentElem, props.rotation);
    }
};

// Represents a panel component renderer.
rs.mimic.PanelRenderer = class extends rs.mimic.RegularComponentRenderer {
    _setClasses(componentElem, component, renderContext) {
        super._setClasses(componentElem, component, renderContext);
        componentElem.addClass("panel");

        if (component.properties.border.width <= 0) {
            componentElem.addClass("fiction-border");
        }
    }
};

// Represents a faceplate renderer.
rs.mimic.FaceplateRenderer = class extends rs.mimic.ComponentRenderer {
    _setClasses(componentElem, component, renderContext) {
        super._setClasses(componentElem, component, renderContext);
        componentElem.addClass("faceplate");

        if (component.model) {
            let modelProps = component.model.document;

            if (modelProps.cssClass) {
                mimicElem.addClass(modelProps.cssClass);
            }
        }
    }

    _setProps(componentElem, component, renderContext) {
        super._setProps(componentElem, component, renderContext);
        let borderWidth = 0;

        if (component.model) {
            let compProps = component.properties;
            let modelProps = component.model.document;
            rs.mimic.RendererSet.mimicRenderer.setMimicProps(componentElem, component.model, renderContext);
            borderWidth = modelProps.border.width;

            if (compProps.enabled) {
                this._restoreVisualState(componentElem, modelProps);
            } else {
                this._setVisualState(componentElem, modelProps.disabledState);
            }
        }

        if (renderContext.editMode) {
            componentElem.css("--border-width", -borderWidth + "px");
        }
    }

    _bindEvents(componentElem, component, renderContext) {
        super._bindEvents(componentElem, component, renderContext);

        if (component.model && component.properties.enabled) {
            this._bindVisualStates(componentElem, component.model.document);
        }
    }
};

// Encapsulates information about a rendering operation.
rs.mimic.RenderContext = class {
    editMode = false;
    viewHub = null;
    mainApi = null;
    fontMap = null;
    editorOptions = null;
    controlRight = false;
    imageMap = null;
    faceplateMode = false;
    idPrefix = "";
    unknownTypes = null;

    constructor(source) {
        Object.assign(this, source);
    }

    getImage(imageName) {
        return this.imageMap instanceof Map ? this.imageMap.get(imageName) : null;
    }
};

// Contains renderers for a mimic and its components.
rs.mimic.RendererSet = class {
    static mimicRenderer = new rs.mimic.MimicRenderer();
    static faceplateRenderer = new rs.mimic.FaceplateRenderer();
    static componentRenderers = new Map([
        ["Text", new rs.mimic.TextRenderer()],
        ["Picture", new rs.mimic.PictureRenderer()],
        ["Panel", new rs.mimic.PanelRenderer()]
    ]);
};

// Renders a mimic using appropriate renderers.
rs.mimic.UnitedRenderer = class {
    mimic;
    editMode;
    viewHub = null;
    mainApi = null;
    fontMap = null;
    editorOptions = null;
    controlRight = false;

    constructor(mimic, editMode) {
        this.mimic = mimic;
        this.editMode = editMode;
    }

    // Creates a render context for regular components.
    _createRenderContext(opt_withUnknownTypes) {
        return new rs.mimic.RenderContext({
            editMode: this.editMode,
            viewHub: this.viewHub,
            mainApi: this.mainApi,
            fontMap: this.fontMap,
            editorOptions: this.editorOptions,
            controlRight: this.controlRight,
            imageMap: this.mimic.imageMap,
            unknownTypes: opt_withUnknownTypes ? new Set() : null
        });
    }

    // Creates a render context for the faceplate.
    _createFaceplateContext(faceplateInstance, renderContext) {
        return new rs.mimic.RenderContext({
            editMode: this.editMode,
            viewHub: this.viewHub,
            mainApi: this.mainApi,
            fontMap: this.fontMap,
            editorOptions: this.editorOptions,
            controlRight: this.controlRight,
            faceplateMode: true,
            imageMap: faceplateInstance.model?.imageMap,
            idPrefix: renderContext.idPrefix,
            unknownTypes: renderContext.unknownTypes
        });
    }

    // Appends the component DOM to its parent.
    _appendToParent(component) {
        if (component.parent?.renderer) {
            component.parent.renderer.appendChild(component.parent, component);
        }
    }

    // Creates a component DOM.
    _createComponentDom(component, renderContext) {
        if (component.isFaceplate) {
            this._createFaceplateDom(component, renderContext);
        } else {
            let renderer = rs.mimic.RendererSet.componentRenderers.get(component.typeName);

            if (renderer) {
                component.renderer = renderer;
                renderer.createDom(component, renderContext);
                component.onDomCreated(renderContext);
                this._appendToParent(component);
            } else {
                renderContext.unknownTypes?.add(component.typeName);
            }
        }
    }

    // Creates a faceplate DOM.
    _createFaceplateDom(faceplateInstance, renderContext) {
        if (!faceplateInstance.model) {
            renderContext.unknownTypes?.add(faceplateInstance.typeName);
            return;
        }

        let faceplateContext = this._createFaceplateContext(faceplateInstance, renderContext);
        let renderer = rs.mimic.RendererSet.faceplateRenderer;
        faceplateInstance.renderer = renderer;
        renderer.createDom(faceplateInstance, faceplateContext);
        faceplateInstance.onDomCreated(renderContext);
        this._appendToParent(faceplateInstance);
        faceplateContext.idPrefix += faceplateInstance.id + "-";

        for (let component of faceplateInstance.components) {
            this._createComponentDom(component, faceplateContext);
        }
    }

    // Updates the component DOM.
    _updateComponentDom(component, renderContext) {
        if (component.dom && component.renderer) {
            if (component.isFaceplate) {
                this._updateFaceplateDom(component, renderContext);
            } else {
                if (component.renderer.canUpdateDom) {
                    component.renderer.updateDom(component, renderContext);
                } else {
                    let oldDom = component.dom;
                    component.renderer.createDom(component, renderContext);
                    oldDom.replaceWith(component.dom);
                }

                component.onDomUpdated(renderContext);
            }
        }
    }

    // Updates the faceplate DOM.
    _updateFaceplateDom(faceplateInstance, renderContext) {
        if (faceplateInstance.model && faceplateInstance.dom && faceplateInstance.renderer) {
            let faceplateContext = this._createFaceplateContext(faceplateInstance, renderContext);
            faceplateInstance.renderer.updateDom(faceplateInstance, faceplateContext);
            faceplateInstance.onDomUpdated(renderContext);
            faceplateContext.idPrefix += faceplateInstance.id + "-";

            for (let component of faceplateInstance.components) {
                this._updateComponentDom(component, faceplateContext);
            }
        }
    }

    // Configures the renderer.
    configure({
        viewHub = null,
        mainApi = null,
        fonts = null,
        editorOptions = null,
        controlRight = false
    }) {
        this.viewHub = viewHub;
        this.mainApi = mainApi;
        this.fontMap = Array.isArray(fonts) ? new Map(fonts.map(font => [font.name, font])) : null;
        this.editorOptions = editorOptions;
        this.controlRight = controlRight;
    }

    // Creates a mimic DOM according to the mimic model. Returns a jQuery object.
    createMimicDom() {
        let startTime = Date.now();
        let renderContext = this._createRenderContext(true);
        let renderer = rs.mimic.RendererSet.mimicRenderer;
        this.mimic.renderer = renderer;
        renderer.createDom(this.mimic, renderContext);
        this.mimic.onDomCreated(renderContext);

        for (let component of this.mimic.components) {
            this._createComponentDom(component, renderContext);
        }

        if (renderContext.unknownTypes.size > 0) {
            console.warn("Unable to render components of types: " +
                Array.from(renderContext.unknownTypes).sort().join(", "));
        }

        if (this.mimic.dom) {
            console.info("Mimic DOM created in " + (Date.now() - startTime) + " ms");
            return this.mimic.dom;
        } else {
            console.warn("Unable to create mimic DOM");
            return $();
        }
    }

    // Updates a mimic DOM according to the mimic model.
    updateMimicDom() {
        let renderContext = this._createRenderContext();
        rs.mimic.RendererSet.mimicRenderer.updateDom(this.mimic, renderContext);
        this.mimic.onDomUpdated(renderContext);
    }

    // Creates a component DOM according to the component model. Returns a jQuery object.
    createComponentDom(component) {
        this._createComponentDom(component, this._createRenderContext());
        return component.dom ?? $();
    }

    // Updates the component DOM according to the component model.
    updateComponentDom(component) {
        this._updateComponentDom(component, this._createRenderContext());
    }

    // Updates the components according to the current data.
    updateData(dataProvider) {
        let renderContext = this._createRenderContext();

        for (let component of this.mimic.components) {
            try {
                let updateDomNeeded = false;

                if (component.updateData(dataProvider)) {
                    updateDomNeeded = true;
                }

                if (component.onDataUpdated(dataProvider)) {
                    updateDomNeeded = true;
                }

                if (updateDomNeeded) {
                    this._updateComponentDom(component, renderContext);
                }
            } catch (ex) {
                console.error(`Error updating data of the component with ID ${component.id}: ${ex.message}`);
            }
        }

        this.mimic.onDataUpdated(dataProvider);
    }
};
