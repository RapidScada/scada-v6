
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
    this.setFont(btnComp, props.Font);
    this.setForeColor(btnComp, props.ForeColor);
    this.bindAction(btnComp, component, renderContext);

    if (!renderContext.editMode) {
        this._setState(btnComp, props.BoundProperty, false);
    }

    // set image
    if (props.ImageName) {
        var image = renderContext.getImage(props.ImageName);
        $("<img />")
            .css({
                "width": props.ImageSize.Width,
                "height": props.ImageSize.Height
            })
            .attr("src", this.imageToDataURL(image))
            .appendTo(btnContainer.find(".basic-button-icon"));
    }

    // set text
    btnContainer.find(".basic-button-label").text(props.Text);

    btnComp.append(btnContainer);
    component.dom = btnComp;
};

scada.scheme.ColoredButtonRenderer.prototype.refreshImages = function (component, renderContext, imageNames) {
    var props = component.props;

    if (Array.isArray(imageNames) && imageNames.includes(props.ImageName)) {
        var btnComp = component.dom;
        var image = renderContext.getImage(props.ImageName);
        btnComp.find("img").attr("src", this.imageToDataURL(image));
    }
};

scada.scheme.ColoredButtonRenderer.prototype.updateData = function (component, renderContext) {
    var props = component.props;

    if (props.InCnlNum > 0 && props.BoundProperty) {
        var btnComp = component.dom;
        var cnlDataExt = renderContext.getCnlDataExt(props.InCnlNum);
        var state = cnlDataExt.Val > 0 && cnlDataExt.Stat > 0;
        this._setState(btnComp, props.BoundProperty, state);
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

    var divComp = $("<div id='comp" + component.id + "'><i class='" + props.IconObj.Name + "' /></div>");
    this.prepareComponent(divComp, component);

    // set image
    switch (props.ImageStretch) {
        case ImageStretches.FILL:
            var borders = props.BorderWidth * 2;
            divComp.css("background-size", props.Size.Width - borders + "px " +
                (props.Size.Height - borders) + "px");
            break;
        case ImageStretches.ZOOM:
            divComp.css("background-size", "contain");
            break;
    }
    var iconSize = props.Size.Width > props.Size.Height ? props.Size.Height : props.Size.Width;
    divComp.find('i').css({ "font-size": iconSize + "px" });
    divComp.css({
        "background-repeat": "no-repeat",
        "background-position": "center",
        "background-clip": "padding-box"
    });

    var image = renderContext.getImage(props.ImageName);
    this.setBackgroundImage(divComp, image);

    component.dom = divComp;
};

scada.scheme.StaticSvgRenderer.prototype.refreshImages = function (component, renderContext, imageNames) {
    var props = component.props;

    if (Array.isArray(imageNames) && imageNames.includes(props.ImageName)) {
        var divComp = component.dom;
        var image = renderContext.getImage(props.ImageName);
        this.setBackgroundImage(divComp, image, true);
    }
};

scada.scheme.StaticSvgRenderer.prototype.setSize = function (component, width, height) {
    scada.scheme.ComponentRenderer.prototype.setSize.call(this, component, width, height);

    var ImageStretches = scada.scheme.ImageStretches;
    var props = component.props;
    var divComp = component.dom;

    if (props.ImageStretch === ImageStretches.FILL) {
        divComp.css("background-size", props.Size.Width + "px " + props.Size.Height + "px");
    }
    var iconSize = props.Size.Width > props.Size.Height ? props.Size.Height : props.Size.Width;
    divComp.find('i').css({ "font-size": iconSize + "px" });
};

scada.scheme.StaticSvgRenderer.prototype.updateData = function (component, renderContext) {
    var props = component.props;
    if (props.InCnlNum > 0) {
        var divComp = component.dom;
        var cnlDataExt = renderContext.getCnlDataExt(props.InCnlNum);
        var imageName = props.ImageName;

        // console.log('updateData',renderContext);
        console.log('updateData', props, cnlDataExt);
        // choose an image depending on the conditions
        if (cnlDataExt.Stat && props.Conditions) {
            var cnlVal = cnlDataExt.Val;

            for (var cond of props.Conditions) {
                if (scada.scheme.calc.conditionSatisfied(cond, cnlVal)) {
                    // imageName = cond.ImageName;
                    // console.log(cond.IconObj.Name);
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
        var statusColor = cnlDataExt.Color;
        var isHovered = divComp.is(":hover");

        var backColor = this.chooseColor(isHovered, props.BackColor, props.BackColorOnHover);
        var borderColor = this.chooseColor(isHovered, props.BorderColor, props.BorderColorOnHover);

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
    switch (props.ImageStretch) {
        case ImageStretches.FILL:
            var borders = props.BorderWidth * 2;
            divComp.css("background-size", props.Size.Width - borders + "px " +
                (props.Size.Height - borders) + "px");
            break;
        case ImageStretches.ZOOM:
            divComp.css("background-size", "contain");
            break;
    }
    var iconSize = props.Size.Width > props.Size.Height ? props.Size.Height : props.Size.Width;
    divComp.find('i').css({ "font-size": iconSize + "px" });
    divComp.css({
        "background-repeat": "no-repeat",
        "background-position": "center",
        "background-clip": "padding-box"
    });

    var image = renderContext.getImage(props.ImageName);
    this.setBackgroundImage(divComp, image);

    component.dom = divComp;

    this.bindAction(divComp, component, renderContext);

    // apply properties on hover
    var thisRenderer = this;
    var cnlNum = props.InCnlNum;

    divComp.hover(
        function () {
            thisRenderer.setDynamicBackColor(divComp, props.BackColorOnHover, cnlNum, renderContext);
            thisRenderer.setDynamicBorderColor(divComp, props.BorderColorOnHover, cnlNum, renderContext);

            if (cnlNum <= 0) {
                var image = renderContext.getImage(props.ImageOnHoverName);
                thisRenderer.setBackgroundImage(divComp, image);
            }
        },
        function () {
            thisRenderer.setDynamicBackColor(divComp, props.BackColor, cnlNum, renderContext, true);
            thisRenderer.setDynamicBorderColor(divComp, props.BorderColor, cnlNum, renderContext, true);

            if (cnlNum <= 0) {
                var image = renderContext.getImage(props.ImageName);
                thisRenderer.setBackgroundImage(divComp, image, true);
            }
        }
    );
};

scada.scheme.DynamicSvgRenderer.prototype.updateData = function (component, renderContext) {
    var props = component.props;

    if (props.InCnlNum > 0) {
        var divComp = component.dom;
        var cnlDataExt = renderContext.getCnlDataExt(props.InCnlNum);
        var imageName = props.ImageName;

        // choose an image depending on the conditions
        if (cnlDataExt.Stat && props.Conditions) {
            var cnlVal = cnlDataExt.Val;

            for (var cond of props.Conditions) {
                if (scada.scheme.calc.conditionSatisfied(cond, cnlVal)) {
                    imageName = cond.ImageName;
                    break;
                }
            }
        }

        // set the image
        var image = renderContext.imageMap.get(imageName);
        this.setBackgroundImage(divComp, image, true);

        // choose and set colors of the component
        var statusColor = cnlDataExt.Color;
        var isHovered = divComp.is(":hover");

        var backColor = this.chooseColor(isHovered, props.BackColor, props.BackColorOnHover);
        var borderColor = this.chooseColor(isHovered, props.BorderColor, props.BorderColorOnHover);

        this.setBackColor(divComp, backColor, true, statusColor);
        this.setBorderColor(divComp, borderColor, true, statusColor);
    }
};
/********** Renderer Map **********/

// Add components to the renderer map
scada.scheme.rendererMap.set("Scada.Web.Plugins.SchSvgExtComp.ColoredButton", new scada.scheme.ColoredButtonRenderer());
scada.scheme.rendererMap.set("Scada.Web.Plugins.SchSvgExtComp.StaticSvg", new scada.scheme.StaticSvgRenderer());
scada.scheme.rendererMap.set("Scada.Web.Plugins.SchSvgExtComp.DynamicSvg", new scada.scheme.DynamicSvgRenderer());