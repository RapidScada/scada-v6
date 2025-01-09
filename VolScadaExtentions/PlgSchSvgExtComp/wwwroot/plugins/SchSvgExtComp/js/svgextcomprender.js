

/********** Button Renderer **********/

scada.scheme.ColoredButtonRenderer = function () {
    scada.scheme.ComponentRenderer.call(this);
};

scada.scheme.ColoredButtonRenderer.prototype = Object.create(scada.scheme.ComponentRenderer.prototype);
scada.scheme.ColoredButtonRenderer.constructor = scada.scheme.ColoredButtonRenderer;

// Set enabled or visibility of the button
scada.scheme.ColoredButtonRenderer.prototype._setState = function (btnComp, boundProperty, state) {
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

scada.scheme.ColoredButtonRenderer.prototype.createDom = function (component, renderContext) {
    var props = component.props;

    var btnComp = $("<button id='comp" + component.id + "' class='basic-button'></button>");
    var btnContainer = $("<div class='basic-button-container'><div class='basic-button-content'>" +
        "<div class='basic-button-icon'></div><div class='basic-button-label'></div></div></div>");

    this.prepareComponent(btnComp, component);
    this.setFont(btnComp, props.font);
    this.setForeColor(btnComp, props.foreColor);
    this.bindAction(btnComp, component, renderContext);
    component.cmdVal = props.actionValue;

    if (!renderContext.editMode) {
        if (!props.conditions || props.conditions.length == 0) this._setState(btnComp, props.boundProperty, false);
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
    btnComp.css("border-radius", "5px");//加圆角
    btnComp.append(btnContainer);
    component.dom = btnComp;

    // configure popup
    if (props.action == 0 && (props.url || props.viewID > 0)) {
        btnComp.addClass("action");

        if (!renderContext.editMode) {
            btnComp.on("click", function () {
                let url = "";
                let viewHub = renderContext.schemeEnv.viewHub;

                if (props.viewID > 0) {
                    let popViewId = props.viewID + renderContext.viewIdOffset;
                    console.log('ColorBtn viewid && offset:', props.viewID, renderContext.viewIdOffset)
                    url = viewHub.getViewUrl(popViewId, props.target === 2 /*Popup*/);
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

scada.scheme.ColoredButtonRenderer.prototype.refreshImages = function (component, renderContext, imageNames) {
    var props = component.props;

    if (Array.isArray(imageNames) && imageNames.includes(props.imageName)) {
        var btnComp = component.dom;
        var image = renderContext.getImage(props.imageName);
        btnComp.find("img").attr("src", this.imageToDataURL(image));
    }
};

scada.scheme.ColoredButtonRenderer.prototype.updateData = function (component, renderContext) {
    var props = component.props;

    if (props.inCnlNum > 0 && props.boundProperty) {
        var btnComp = component.dom;
        var cnlDataExt = renderContext.getCnlDataExt(props.inCnlNum);
        if (props.conditions && props.conditions.length > 0) {
            var cnlVal = cnlDataExt.d.val;
            for (var cond of props.conditions) {
                if (scada.scheme.calc.conditionSatisfied(cond, cnlVal)) {
                    this.setBackColor(btnComp, cond.color);
                    break;
                }
            }
        } else {
            var state = cnlDataExt.d.val > 0 && cnlDataExt.d.stat > 0;
            this._setState(btnComp, props.boundProperty, state);
        }
    }
};

/********** Static Picture Renderer **********/

// Static picture renderer type extends scada.scheme.ComponentRenderer
scada.scheme.StaticSvgRenderer = function () {
    scada.scheme.ComponentRenderer.call(this);
};

scada.scheme.StaticSvgRenderer.prototype = Object.create(scada.scheme.ComponentRenderer.prototype);
scada.scheme.StaticSvgRenderer.constructor = scada.scheme.StaticSvgRenderer;

scada.scheme.StaticSvgRenderer.prototype.createDom = function (component, renderContext) {
    var ImageStretches = scada.scheme.ImageStretches;
    var props = component.props;

    var divComp = $("<div id='comp" + component.id + "'><i class='" + props.iconObj.name + "' /></div>");
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
    var iconSize = props.size.width > props.size.height ? props.size.height : props.size.width;
    divComp.find('i').css({ "font-size": iconSize + "px" });
    divComp.css({
        "background-repeat": "no-repeat",
        "background-position": "center",
        "background-clip": "padding-box"
    });

    var image = renderContext.getImage(props.imageName);
    this.setBackgroundImage(divComp, image);

    component.dom = divComp;
};

scada.scheme.StaticSvgRenderer.prototype.refreshImages = function (component, renderContext, imageNames) {
    var props = component.props;

    if (Array.isArray(imageNames) && imageNames.includes(props.imageName)) {
        var divComp = component.dom;
        var image = renderContext.getImage(props.imageName);
        this.setBackgroundImage(divComp, image, true);
    }
};

scada.scheme.StaticSvgRenderer.prototype.setSize = function (component, width, height) {
    scada.scheme.ComponentRenderer.prototype.setSize.call(this, component, width, height);

    var ImageStretches = scada.scheme.ImageStretches;
    var props = component.props;
    var divComp = component.dom;

    if (props.ImageStretch === ImageStretches.FILL) {
        divComp.css("background-size", props.size.width + "px " + props.size.height + "px");
    }
    var iconSize = props.size.width > props.size.height ? props.size.height : props.size.width;
    divComp.find('i').css({ "font-size": iconSize + "px" });
};

scada.scheme.StaticSvgRenderer.prototype.updateData = function (component, renderContext) {
    var props = component.props;
    if (props.inCnlNum > 0) {
        var divComp = component.dom;
        var cnlDataExt = renderContext.getCnlDataExt(props.inCnlNum);
        var imageName = props.imageName;

        // choose an image depending on the conditions
        if (cnlDataExt.d.stat && props.conditions) {
            var cnlVal = cnlDataExt.d.val;

            for (var cond of props.conditions) {
                if (scada.scheme.calc.conditionSatisfied(cond, cnlVal)) {
                    // imageName = cond.ImageName;
                    divComp.find('i').removeAttr("class");
                    divComp.find('i').addClass(cond.IconObj.Name);
                    break;
                }
            }
        }

        // set the image
        var image = renderContext.imageMap.get(imageName);
        this.setBackgroundImage(divComp, image, true);

        // choose and set colors of the component
        var statusColor = cnlDataExt.df.colors[0];
        var isHovered = divComp.is(":hover");

        var backColor = this.chooseColor(isHovered, props.backColor, props.backColorOnHover);
        var borderColor = this.chooseColor(isHovered, props.borderColor, props.borderColorOnHover);

        this.setBackColor(divComp, backColor, true, statusColor);
        this.setBorderColor(divComp, borderColor, true, statusColor);
    }
};
/********** Dynamic Picture Renderer **********/

// Dynamic picture renderer type extends scada.scheme.StaticSvgRenderer
scada.scheme.DynamicSvgRenderer = function () {
    scada.scheme.ComponentRenderer.call(this);
};

scada.scheme.DynamicSvgRenderer.prototype = Object.create(scada.scheme.ComponentRenderer.prototype);
scada.scheme.DynamicSvgRenderer.constructor = scada.scheme.DynamicSvgRenderer;

scada.scheme.DynamicSvgRenderer.prototype.createDom = function (component, renderContext) {
    var props = component.props;
    var ImageStretches = scada.scheme.ImageStretches;
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
    var iconSize = props.size.width > props.size.height ? props.size.height : props.size.width;
    divComp.find('i').css({ "font-size": iconSize + "px" });
    divComp.css({
        "background-repeat": "no-repeat",
        "background-position": "center",
        "background-clip": "padding-box"
    });

    var image = renderContext.getImage(props.imageName);
    this.setBackgroundImage(divComp, image);

    component.dom = divComp;

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

scada.scheme.DynamicSvgRenderer.prototype.updateData = function (component, renderContext) {
    var props = component.props;

    if (props.inCnlNum > 0) {
        var divComp = component.dom;
        var cnlDataExt = renderContext.getCnlDataExt(props.inCnlNum);
        var imageName = props.imageName;
        // choose an image depending on the conditions
        if (cnlDataExt.d.stat && props.conditions) {
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
        var statusColor = cnlDataExt.df.colors[0];
        var isHovered = divComp.is(":hover");

        var backColor = this.chooseColor(isHovered, props.backColor, props.backColorOnHover);
        var borderColor = this.chooseColor(isHovered, props.borderColor, props.borderColorOnHover);

        this.setBackColor(divComp, backColor, true, statusColor);
        this.setBorderColor(divComp, borderColor, true, statusColor);
    }
};
/********** Renderer Map **********/

// Add components to the renderer map
scada.scheme.rendererMap.set("Scada.Web.Plugins.SchSvgExtComp.ColoredButton", new scada.scheme.ColoredButtonRenderer());
scada.scheme.rendererMap.set("Scada.Web.Plugins.SchSvgExtComp.StaticSvg", new scada.scheme.StaticSvgRenderer());
scada.scheme.rendererMap.set("Scada.Web.Plugins.SchSvgExtComp.DynamicSvg", new scada.scheme.DynamicSvgRenderer());