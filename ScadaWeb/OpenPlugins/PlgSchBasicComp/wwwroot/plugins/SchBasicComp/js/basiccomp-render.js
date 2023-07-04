/*
 * Basic components rendering
 *
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2022
 *
 * Requires:
 * - jquery
 * - modal.js
 * - scheme-common.js
 * - scheme-render.js
 */

/********** Button Renderer **********/

scada.scheme.ButtonRenderer = function () {
    scada.scheme.ComponentRenderer.call(this);
};

scada.scheme.ButtonRenderer.prototype = Object.create(scada.scheme.ComponentRenderer.prototype);
scada.scheme.ButtonRenderer.constructor = scada.scheme.ButtonRenderer;

// Set enabled or visibility of the button
scada.scheme.ButtonRenderer.prototype._setState = function (btnComp, boundProperty, state) {
    if (boundProperty === 1 /*Enabled*/) {
        if (state) {
            btnComp.removeClass("disabled").prop("disabled", false);
        } else {
            btnComp.addClass("disabled").prop("disabled", true);
        }
    } else if (boundProperty === 2 /*Visible*/) {
        if (state) {
            btnComp.removeClass("hidden");
        } else {
            btnComp.addClass("hidden");
        }
    }
};

scada.scheme.ButtonRenderer.prototype.createDom = function (component, renderContext) {
    var props = component.props;

    var btnComp = $("<button id='comp" + component.id + "' class='basic-button'></button>");
    var btnContainer = $("<div class='basic-button-container'><div class='basic-button-content'>" +
        "<div class='basic-button-icon'></div><div class='basic-button-label'></div></div></div>");

    this.prepareComponent(btnComp, component);
    this.setFont(btnComp, props.font);
    this.setForeColor(btnComp, props.foreColor);
    this.bindAction(btnComp, component, renderContext);

    if (!renderContext.editMode) {
        this._setState(btnComp, props.boundProperty, false);
    }

    // set image
    if (props.imageName) {
        var image = renderContext.getImage(props.imageName);
        $("<img />")
            .css({
                "width": props.imageSize.width,
                "height": props.imageSize.height
            })
            .attr("src", this.imageToDataURL(image))
            .appendTo(btnContainer.find(".basic-button-icon"));
    }

    // set text
    btnContainer.find(".basic-button-label").text(props.text);

    btnComp.append(btnContainer);
    component.dom = btnComp;
};

scada.scheme.ButtonRenderer.prototype.refreshImages = function (component, renderContext, imageNames) {
    var props = component.props;

    if (Array.isArray(imageNames) && imageNames.includes(props.imageName)) {
        var btnComp = component.dom;
        var image = renderContext.getImage(props.imageName);
        btnComp.find("img").attr("src", this.imageToDataURL(image));
    }
};

scada.scheme.ButtonRenderer.prototype.updateData = function (component, renderContext) {
    var props = component.props;

    if (props.inCnlNum > 0 && props.boundProperty) {
        var btnComp = component.dom;
        var cnlDataExt = renderContext.getCnlDataExt(props.inCnlNum);
        var state = cnlDataExt.d.val > 0 && cnlDataExt.d.stat > 0;
        this._setState(btnComp, props.boundProperty, state);
    }
};

/********** Led Renderer **********/

scada.scheme.LedRenderer = function () {
    scada.scheme.ComponentRenderer.call(this);
};

scada.scheme.LedRenderer.prototype = Object.create(scada.scheme.ComponentRenderer.prototype);
scada.scheme.LedRenderer.constructor = scada.scheme.LedRenderer;

scada.scheme.LedRenderer.prototype.createDom = function (component, renderContext) {
    var props = component.props;

    var divComp = $("<div id='comp" + component.id + "' class='basic-led'></div>");
    this.prepareComponent(divComp, component, false, true);
    this.setBackColor(divComp, props.backColor);
    this.bindAction(divComp, component, renderContext);

    if (props.borderWidth > 0) {
        var divBorder = $("<div class='basic-led-border'></div>").appendTo(divComp);
        this.setBorderColor(divBorder, props.borderColor);
        this.setBorderWidth(divBorder, props.borderWidth);

        var opacity = props.borderOpacity / 100;
        if (opacity < 0) {
            opacity = 0;
        } else if (opacity > 1) {
            opacity = 1;
        }

        divBorder.css("opacity", opacity);
        divComp.append(divBorder);
    }

    component.dom = divComp;
};

scada.scheme.LedRenderer.prototype.updateData = function (component, renderContext) {
    var props = component.props;

    if (props.inCnlNum > 0) {
        var divComp = component.dom;
        var cnlDataExt = renderContext.getCnlDataExt(props.inCnlNum);
        var backColor = props.backColor;

        // define background color according to the channel status
        if (backColor === this.STATUS_COLOR) {
            backColor = this._getStatusColor(cnlDataExt);
        }

        // define background color according to the led conditions and channel value
        if (cnlDataExt.d.stat > 0 && props.conditions) {
            var cnlVal = cnlDataExt.d.val;

            for (var cond of props.conditions) {
                if (scada.scheme.calc.conditionSatisfied(cond, cnlVal)) {
                    backColor = cond.color;
                    break;
                }
            }
        }

        // apply background color
        divComp.css("background-color", backColor);

        // set border color
        if (props.borderColor === this.STATUS_COLOR) {
            var divBorder = divComp.children(".basic-led-border");
            divBorder.css("border-color", this._getStatusColor(cnlDataExt));
        }
    }
};

/********** Link Renderer **********/

scada.scheme.LinkState = function () {
    this.cnlVals = [];
};

scada.scheme.LinkRenderer = function () {
    scada.scheme.StaticTextRenderer.call(this);
};

scada.scheme.LinkRenderer.prototype = Object.create(scada.scheme.StaticTextRenderer.prototype);
scada.scheme.LinkRenderer.constructor = scada.scheme.LinkRenderer;

scada.scheme.LinkRenderer.prototype._getLinkState = function (component) {
    if (!component.state) {
        component.state = new scada.scheme.LinkState();
    }
    return component.state;
};

scada.scheme.LinkRenderer.prototype._setUnderline = function (jqObj, underline) {
    // this method was copied from DynamicTextRenderer
    if (underline) {
        jqObj.css("text-decoration", "underline");
    }
};

scada.scheme.LinkRenderer.prototype._restoreUnderline = function (jqObj, font) {
    // this method was copied from DynamicTextRenderer
    jqObj.css("text-decoration", font && font.Underline ? "underline" : "none");
};

scada.scheme.LinkRenderer.prototype.createDom = function (component, renderContext) {
    scada.scheme.StaticTextRenderer.prototype.createDom.call(this, component, renderContext);

    var spanComp = component.dom.first();
    spanComp.addClass("basic-link");

    // apply properties on hover
    var props = component.props;
    var state = this._getLinkState(component);
    var thisRenderer = this;

    spanComp.hover(
        function () {
            thisRenderer.setBackColor(spanComp, props.backColorOnHover);
            thisRenderer.setBorderColor(spanComp, props.borderColorOnHover);
            thisRenderer.setForeColor(spanComp, props.foreColorOnHover);
            thisRenderer._setUnderline(spanComp, props.underlineOnHover);
        },
        function () {
            thisRenderer.setBackColor(spanComp, props.backColor, true);
            thisRenderer.setBorderColor(spanComp, props.borderColor, true);
            thisRenderer.setForeColor(spanComp, props.foreColor, true);
            thisRenderer._restoreUnderline(spanComp, props.font);
        }
    );

    // configure link
    if (props.url || props.viewID > 0) {
        spanComp.addClass("action");

        if (!renderContext.editMode) {
            spanComp.on("click", function () {
                let url = "";
                let viewHub = renderContext.schemeEnv.viewHub;

                if (props.viewID > 0) {
                    url = viewHub.getViewUrl(props.viewID, props.target === 2 /*Popup*/);
                } else {
                    url = props.url;

                    // insert input channel values into the URL
                    for (let i = 0, cnlCnt = props.cnlNums.length; i < cnlCnt; i++) {
                        let cnlVal = state.cnlVals[i];
                        url = url.replace("{" + i + "}", isNaN(cnlVal) ? "" : cnlVal);
                    }
                }

                if (url) {
                    switch (props.target) {
                        case 1: // Blank
                            window.open(url);
                            break;
                        case 2: // Popup
                            viewHub.modalManager.showModal(url,
                                new ModalOptions({ size: props.popupSize.width, height: props.popupSize.height }));
                            break;
                        default: // Self
                            window.top.location = url;
                            break;
                    }
                } else {
                    console.warn("URL is undefined");
                }
            });
        }
    }
};

scada.scheme.LinkRenderer.prototype.updateData = function (component, renderContext) {
    var props = component.props;
    var state = this._getLinkState(component);
    var cnlCnt = props.cnlNums.length;

    // get the current values of the input channels specified for the link
    if (cnlCnt > 0) {
        for (let i = 0; i < cnlCnt; i++) {
            var cnlDataExt = renderContext.getCnlDataExt(props.cnlNums[i]);
            state.cnlVals[i] = cnlDataExt.d.stat > 0 ? cnlDataExt.d.val : NaN;
        }
    }
};

/********** Toggle Renderer **********/

scada.scheme.ToggleRenderer = function () {
    scada.scheme.ComponentRenderer.call(this);
};

scada.scheme.ToggleRenderer.prototype = Object.create(scada.scheme.ComponentRenderer.prototype);
scada.scheme.ToggleRenderer.constructor = scada.scheme.ToggleRenderer;

scada.scheme.ToggleRenderer.prototype._applySize = function (divComp, divContainer, divLever, component) {
    var props = component.props;
    var borders = (props.borderWidth + props.padding) * 2;
    var minSize = Math.min(props.size.width, props.size.height);

    divComp.css({
        "border-radius": minSize / 2,
        "padding": props.padding
    });

    divContainer.css({
        "width": props.size.width - borders,
        "min-width": props.size.width - borders, // required for scaling
        "height": props.size.height - borders
    });

    divLever.css({
        "width": minSize - borders,
        "height": minSize - borders
    });
};

scada.scheme.ToggleRenderer.prototype.createDom = function (component, renderContext) {
    var props = component.props;

    var divComp = $("<div id='comp" + component.id + "' class='basic-toggle'></div>");
    var divContainer = $("<div class='basic-toggle-container'></div>");
    var divLever = $("<div class='basic-toggle-lever'></div>");

    this.prepareComponent(divComp, component, true);
    this.bindAction(divComp, component, renderContext);
    this.setBackColor(divLever, props.leverColor);
    this._applySize(divComp, divContainer, divLever, component);

    if (!renderContext.editMode) {
        divComp.addClass("undef");
    }

    divContainer.append(divLever);
    divComp.append(divContainer);
    component.dom = divComp;
};

scada.scheme.ToggleRenderer.prototype.setSize = function (component, width, height) {
    scada.scheme.ComponentRenderer.prototype.setSize.call(this, component, width, height);

    var divComp = component.dom;
    var divContainer = divComp.children(".basic-toggle-container");
    var divLever = divContainer.children(".basic-toggle-lever");
    this._applySize(divComp, divContainer, divLever, component);
};

scada.scheme.ToggleRenderer.prototype.updateData = function (component, renderContext) {
    var props = component.props;
    component.cmdVal = 0;

    if (props.inCnlNum > 0) {
        var divComp = component.dom;
        var cnlDataExt = renderContext.getCnlDataExt(props.inCnlNum);

        divComp.removeClass("undef");
        divComp.removeClass("on");
        divComp.removeClass("off");

        if (cnlDataExt.d.stat > 0) {
            if (cnlDataExt.d.val > 0) {
                divComp.addClass("on");
            } else {
                divComp.addClass("off");
                component.cmdVal = 1; // a command turns it on
            }
        } else {
            divComp.addClass("undef");
        }

        // set colors that depend on status
        var statusColor = this._getStatusColor(cnlDataExt);
        this.setBackColor(divComp, props.backColor, true, statusColor);
        this.setBorderColor(divComp, props.borderColor, true, statusColor);

        if (props.leverColor === this.STATUS_COLOR) {
            // execute the find method if the color depends on status
            divComp.find(".basic-toggle-lever").css("background-color", statusColor);
        }
    }
};

/********** Renderer Map **********/

// Add components to the renderer map
scada.scheme.rendererMap.set("Scada.Web.Plugins.PlgSchBasicComp.Code.Button", new scada.scheme.ButtonRenderer());
scada.scheme.rendererMap.set("Scada.Web.Plugins.PlgSchBasicComp.Code.Led", new scada.scheme.LedRenderer());
scada.scheme.rendererMap.set("Scada.Web.Plugins.PlgSchBasicComp.Code.Link", new scada.scheme.LinkRenderer());
scada.scheme.rendererMap.set("Scada.Web.Plugins.PlgSchBasicComp.Code.Toggle", new scada.scheme.ToggleRenderer());
