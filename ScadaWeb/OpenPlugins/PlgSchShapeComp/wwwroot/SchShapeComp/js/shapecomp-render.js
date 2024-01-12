/*
 * SVG components rendering
 *
 * Author   : Messie MOUKIMOU
 * Created  : 2023
 * Modified : 
 *
 * Requires:
 * - jquery
 * - schemecommon.js
 * - schemerender.js
 */

/********** Shape Renderer **********/
scada.scheme.addInfoTooltipToDiv = function (targetDiv, text) {
    if (targetDiv instanceof jQuery) {
        targetDiv = targetDiv[0];
    }

    if (!targetDiv) return;
    if (!targetDiv) return;

    // Create tooltip
    const tooltip = document.createElement("div");
    tooltip.style.position = "absolute";
    tooltip.style.bottom = "45%";
    tooltip.style.left = "50%";
    tooltip.style.transform = "translateX(-50%)";
    tooltip.style.padding = "10px";
    tooltip.style.backgroundColor = "black";
    tooltip.style.color = "white";
    tooltip.style.borderRadius = "5px";
    tooltip.style.zIndex = "10";
    tooltip.style.whiteSpace = "nowrap";
    tooltip.style.marginBottom = "5px";

    tooltip.textContent = text;

    targetDiv.style.position = "relative";
    targetDiv.appendChild(tooltip);
};

scada.scheme.handleBlinking = function (divComp, blinking,bool) {
    if(bool)
    {
        divComp.removeClass("no-blink slow-blink fast-blink");
    switch (blinking) {
        case 0:
            break;
        case 1:
            divComp.addClass("slow-blink");
            break;
        case 2:
            divComp.addClass("fast-blink");
            break;
    }
}else{
    divComp.removeClass("no-blink slow-blink fast-blink");
}
};
scada.scheme.updateStyles = function (divComp, cond, bool) {
    if (cond.color && bool ){
        divComp.css("color", cond.color);
    }else if (cond.color && !bool ){
        divComp.css("color",cond.color)
    }
    if (cond.backgroundColor && bool ){
        divComp.css("background-color", cond.backgroundColor);
    }else {
        
        divComp.css("background-color", String(cond.backColor));
    }
        
    if (cond.rotation && bool ){
        divComp.css("transform", "rotate(" + cond.rotation + "deg)");
    }else{
        divComp.css("transform", "rotate(" + cond.rotation + "deg)");
    }

    if (cond.isVisible !== undefined && bool)
    {
        divComp.css("visibility", cond.isVisible ? "visible" : "hidden");
    }else{
        divComp.css("visibility",  "visible");
    }

    if (cond.width && bool )
    {
      divComp.css("width", cond.width);
    }else{
      divComp.css("width", "100px");
    }
        
    if (cond.height && bool){
        divComp.css("height", cond.height);
    }else {
        divComp.css("height", "100px");
    }
        
};

scada.scheme.applyRotation = function (divComp, props) {
    if (props.rotation && props.rotation > 0) {
        divComp.css({
            transform: "rotate(" + props.rotation + "deg)",
        });
    }
};
scada.scheme.updateColors = function (divComp, cnlDataExt, isHovered, props) {
    var statusColor = cnlDataExt.color;

    var backColor = chooseColor(
        isHovered,
        props.backColor,
        props.backColorOnHover,
    );
    var borderColor = chooseColor(
        isHovered,
        props.borderColor,
        props.borderColorOnHover,
    );

    setBackColor(divComp, backColor, true, statusColor);
    setBorderColor(divComp, borderColor, true, statusColor);
};

scada.scheme.updateComponentData = function (component, renderContext) {
    var props = component.props;
    if (props.inCnlNum <= 0) {
        return;
    }
    var divComp = component.dom;
    var cnlDataExt = renderContext.getCnlDataExt(props.inCnlNum);

    if (props.conditions && cnlDataExt.d.stat > 0) {
        var cnlVal = cnlDataExt.d.val;

        for (var cond of props.conditions) {
            if (scada.scheme.calc.conditionSatisfied(cond, cnlVal)) {
                scada.scheme.updateStyles(divComp, cond,true);
                scada.scheme.handleBlinking(divComp, cond.blinking, true);
                break;
            }else{
                scada.scheme.updateStyles(divComp,props,false);
                scada.scheme.handleBlinking(divComp, cond.blinking, false);

            }
        }
    }
};


/**************** Custom SVG *********************/
scada.scheme.CustomSVGRenderer = function () {
    scada.scheme.ComponentRenderer.call(this);
};

scada.scheme.CustomSVGRenderer.prototype = Object.create(
    scada.scheme.ComponentRenderer.prototype,
);
scada.scheme.CustomSVGRenderer.constructor =
    scada.scheme.CustomSVGRenderer;

scada.scheme.CustomSVGRenderer.prototype.createDom = function (
    component,
    renderContext,
) {
    var props = component.props;

    var divComp = $("<div id='comp" + component.id + "'></div>");
    this.prepareComponent(divComp, component, false, true);
    scada.scheme.applyRotation(divComp, props);

    if (props.svgCode && props.svgCode.includes("width") || props.svgCode.includes("height")) {
        props.svgCode = props.svgCode.replace(
            /<svg[^>]*?(\s+width\s*=\s*["'][^"']*["'])/g,
            "<svg",
        );
        props.svgCode = props.svgCode.replace(
            /<svg[^>]*?(\s+height\s*=\s*["'][^"']*["'])/g,
            "<svg",
        );
        props.svgCode = props.svgCode.replace(
            /<svg/g,
            "<svg height='100%' width='100%' preserveAspectRatio='none'"
        );
    }
    divComp.append(props.svgCode);
    component.dom = divComp;
};

scada.scheme.CustomSVGRenderer.prototype.updateData = function (
    component,
    renderContext,
) {
   // scada.scheme.applyRotation(component.dom, component.props);
    scada.scheme.updateComponentData(component, renderContext);
};

/**
 * Basic shape renderer
 */
scada.scheme.BasicShapeRenderer = function () {
    scada.scheme.ComponentRenderer.call(this);
};

scada.scheme.BasicShapeRenderer.prototype = Object.create(
    scada.scheme.ComponentRenderer.prototype
);

scada.scheme.BasicShapeRenderer.constructor =
    scada.scheme.BasicShapeRenderer;


scada.scheme.BasicShapeRenderer.prototype.createDom = function (
    component,
    renderContext,
) {
    var props = component.props;
    var shapeType = props.shapeType;

    var divComp = $("<div id='comp" + component.id + "'></div>");
    var shape = $("<div class='shape '></div>");
    this.prepareComponent(divComp, component, false, true);
    if (shapeType == "Line") {
        shape.addClass(shapeType.toLowerCase());
        shape.css({
            "border-color": props.borderColor,
            "border-width": props.borderWidth,
            "border-style": "solid",
            "background-color": props.backColor,
        });

        divComp.css({
            display: "flex",
            "align-items": "center",
            "justify-content": "center",
        });

        divComp.append(shape);
    } else {
        divComp.addClass(shapeType.toLowerCase());
        this.setBackColor(divComp, props.backColor);
        this.setBorderColor(divComp, props.borderColor);
        if (props.borderWidth > 0) {
            this.setBorderWidth(divComp, props.borderWidth);
        }
    }

    scada.scheme.applyRotation(divComp, props);

    component.dom = divComp;
};

scada.scheme.BasicShapeRenderer.prototype.updateData = function (
    component,
    renderContext,
) {
   // scada.scheme.applyRotation(component.dom, component.props);
    scada.scheme.updateComponentData(component, renderContext);
};


/**** 
 * BARGRAPH
 *  */
scada.scheme.BarGraphRenderer = function () {
    scada.scheme.ComponentRenderer.call(this);
};

scada.scheme.BarGraphRenderer.prototype = Object.create(
    scada.scheme.ComponentRenderer.prototype
);
scada.scheme.BarGraphRenderer.constructor = scada.scheme.BarGraphRenderer;

scada.scheme.BarGraphRenderer.prototype.createDom = function (component, renderContext) {
    var props = component.props;

    var divComp = $("<div id='comp" + component.id + "'></div>");

    var bar = $("<div class='bar' style='height:" + props.value + "%" + ";background-color:" + props.barColor + "' data-value='" + parseInt(props.Value) + "'></div>");

    divComp.append(bar);

    this.prepareComponent(divComp, component);

    divComp.css({
        "border": props.borderWidth + "px solid " + props.borderColor,
        "display": "flex",
        "align-items": "flex-end",
        "justify-content": "center"
    });

    component.dom = divComp;
};


scada.scheme.BarGraphRenderer.prototype.updateData = function (component, renderContext) {
    var props = component.props;
    if (props.inCnlNum > 0) {
        var divComp = component.dom;
        var cnlDataExt = renderContext.getCnlDataExt(props.inCnlNum);

        divComp.css({
            "border": props.borderWidth + "px solid " + props.borderColor,
            "background-color": props.backColor,
        })
        divComp.find('.bar').css({
            "background-color": props.barColor,
            "height": props.value + "%",
        });
        divComp.find('.bar').attr('data-value', parseInt(props.value));

    }

    if (props.conditions && cnlDataExt.d.stat > 0) {
        var cnlVal = cnlDataExt.d.val;

        for (var condition of props.conditions) {
            if (scada.scheme.calc.conditionSatisfied(condition, cnlVal)) {
                if (scada.scheme.calc.conditionSatisfied(condition, cnlVal)) {
                    var barStyles = {};

                    if (condition.level === "Min") {
                        barStyles.height = "10%";
                    } else if (condition.Level === "Low") {
                        barStyles.height = "30%";
                    } else if (condition.level === "Medium") {
                        barStyles.height = "50%";
                    } else if (condition.level === "High") {
                        barStyles.height = "70%";
                    } else if (condition.level === "Max") {
                        barStyles.height = "100%";
                    }
                    if (condition.fillColor) {
                        barStyles['background-color'] = condition.fillColor;
                    }

                    divComp.find('.bar').css(barStyles);

                    if (condition.textContent) {
                        scada.scheme.addInfoTooltipToDiv(divComp[0], condition.textContent);
                    }
                    // Set other CSS properties based on Condition
                    if (condition.color) {
                        divComp.css("color", condition.color);
                    }
                    if (condition.backgroundColor) {
                        divComp.css("background-color", condition.backgroundColor);
                    }
                    if (condition.textContent) {
                        divComp.text(condition.textContent);
                    }
                    divComp.css("visibility", condition.isVisible ? "visible" : "hidden");
                    if (condition.width) {
                        divComp.css("width", condition.width);
                    }
                    if (condition.height) {
                        divComp.css("height", condition.height);
                    }

                    // Handle Blinking
                    if (condition.blinking == 1) {
                        divComp.addClass("slow-blink");
                    } else if (condition.blinking == 2) {
                        divComp.addClass("fast-blink");
                    } else {
                        divComp.removeClass("slow-blink fast-blink");
                    }
                }
            }
        }
    }

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

scada.scheme.DynamicTextRenderer.prototype.setFontSize = function (jqObj, fontSize) {
    jqObj.css("font-size", fontSize + "px");
};

scada.scheme.DynamicTextRenderer.prototype.createDom = function (component, renderContext) {
    scada.scheme.StaticTextRenderer.prototype.createDom.call(this, component, renderContext);

    var ShowValueKinds = scada.scheme.ShowValueKinds;
    var props = component.props;
    var spanComp = component.dom.first();
    var spanText = component.dom.children();
    var cnlNum = props.inCnlNum;

    //Apply rotation
    scada.scheme.applyRotation(spanComp, props);

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
                let unit = cnlDataExt.d.stat > 0 && cnlProps.unit ? " " + cnlProps.unit : "";
                spanText.text(cnlDataExt.df.dispVal + unit);
                break;
            case ShowValueKinds.SHOW_WITHOUT_UNIT:
                spanText.text(cnlDataExt.df.dispVal);
                break;
        }
         //add condition textContente to set spanText
         if (props.conditions && cnlDataExt.d.stat > 0) {
            var cnlVal = cnlDataExt.d.val;
            for (var cond of props.conditions) {
                if (scada.scheme.calc.conditionSatisfied(cond, cnlVal)) {
                    if (cond.textContent) {
                        spanText.text(cond.textContent);
                    }
                    
                }
                //update font size
                if (scada.scheme.calc.conditionSatisfied(cond, cnlVal)) {
                    this.setFontSize(spanComp, cond.fontSize);
                }
                break;
            }
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

        

        //update component data conditions
        scada.scheme.updateComponentData(component, renderContext);

        //apply rotation
      //  scada.scheme.applyRotation(spanComp, props);
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

    //apply rotation
    scada.scheme.applyRotation(divComp, props);

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
        scada.scheme.updateComponentData(component, renderContext);

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

/********** Renderer Map **********/

// Add components to the renderer map
scada.scheme.rendererMap.set("Scada.Web.Plugins.PlgSchShapeComp.Code.BasicShape", new scada.scheme.BasicShapeRenderer);
scada.scheme.rendererMap.set("Scada.Web.Plugins.PlgSchShapeComp.Code.CustomSVG", new scada.scheme.CustomSVGRenderer);
scada.scheme.rendererMap.set("Scada.Web.Plugins.PlgSchShapeComp.Code.BarGraph", new scada.scheme.BarGraphRenderer);
scada.scheme.rendererMap.set("Scada.Web.Plugins.PlgSchShapeComp.Code.DynamicText", new scada.scheme.DynamicTextRenderer);
scada.scheme.rendererMap.set("Scada.Web.Plugins.PlgSchShapeComp.Code.DynamicPicture", new scada.scheme.DynamicPictureRenderer);
