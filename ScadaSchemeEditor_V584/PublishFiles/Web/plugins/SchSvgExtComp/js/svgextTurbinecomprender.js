/********** Turbine Renderer **********/

// Turbine renderer type extends scada.scheme.ComponentRenderer
scada.scheme.TurbineRenderer = function () {
    scada.scheme.ComponentRenderer.call(this);
};

scada.scheme.TurbineRenderer.prototype = Object.create(scada.scheme.ComponentRenderer.prototype);
scada.scheme.TurbineRenderer.constructor = scada.scheme.TurbineRenderer;

scada.scheme.TurbineRenderer.prototype.createDom = function (component, renderContext) {
    var props = component.props;
    var divComp = $("<div id='comp" + component.id + "'></div>");
    var compId = '#comp' + component.id;
    this.prepareComponent(divComp, component);
    if (props.TurbineName) {
        var svgName = props.TurbineName.substr(0, props.TurbineName.indexOf('.'));
        var svgPic = $(turbineUtil["turbine" + svgName]);
        var iconSize = props.Size.Width > props.Size.Height ? props.Size.Height : props.Size.Width;
        svgPic.css({ "width": iconSize + "px", "height": iconSize + "px" });
        var svgPath = svgPic.find('path');
        svgPath.attr({ "stroke": props.ForeColor, "fill": props.ForeColor })
        component.dom = divComp;
        setTimeout(function () {
            $(compId).append($("<div />").append(svgPic).html());
        }, 10);
    } else {
        divComp.append($("<span>请选择组件</span>"));
        component.dom = divComp;
    }
};

scada.scheme.TurbineRenderer.prototype.refreshImages = function (component, renderContext, imageNames) {
    var props = component.props;

    /*if (Array.isArray(imageNames) && imageNames.includes(props.ImageName)) {
        var divComp = component.dom;
        var image = renderContext.getImage(props.ImageName);
        this.setBackgroundImage(divComp, image, true);
    }*/
};

scada.scheme.TurbineRenderer.prototype.setSize = function (component, width, height) {
    scada.scheme.ComponentRenderer.prototype.setSize.call(this, component, width, height);

    var ImageStretches = scada.scheme.ImageStretches;
    var props = component.props;
    var divComp = component.dom;

    if (props.ImageStretch === ImageStretches.FILL) {
        divComp.css("background-size", props.Size.Width + "px " + props.Size.Height + "px");
    }
    var iconSize = props.Size.Width > props.Size.Height ? props.Size.Height : props.Size.Width;
    divComp.find('svg').css({ "width": iconSize + "px", "height": iconSize + "px" });
    var svgPath = divComp.find('svg path');
    svgPath.attr({ "stroke": props.ForeColor, "fill": props.ForeColor })
};

scada.scheme.TurbineRenderer.prototype.updateData = function (component, renderContext) {
    var props = component.props;
    if (props.InCnlNum > 0 && props.TurbineName) {
        var divComp = component.dom;
        var cnlDataExt = renderContext.getCnlDataExt(props.InCnlNum);
        var svgAnimate = divComp.find('svg g animateTransform');
        if (cnlDataExt.Stat == 1 && cnlDataExt.Val > 0) {
            if (svgAnimate.length == 0) {
                var svgName = props.TurbineName.substr(0, props.TurbineName.indexOf('.'));
                var svgCenter = turbineUtil["turbine" + svgName + "Center"];
                var svgPicAnimateEle = $('<animateTransform attributeName="transform" begin="0s" dur="6s" type="rotate" from="360 0 0" to="0 0 0" repeatCount="indefinite" additive ="sum"/>')
                if (props.TurbineDirection == 0) {
                    svgPicAnimateEle.attr({ "from": "0 " + svgCenter, "to": "360 " + svgCenter });
                } else {
                    svgPicAnimateEle.attr({ "from": "360 " + svgCenter, "to": "0 " + svgCenter });
                }

                var svgPic = $(turbineUtil["turbine" + svgName]);
                svgPicAnimateEle.appendTo(svgPic.find('g')[0]);
                var compId = '#comp' + component.id;
                var iconSize = props.Size.Width > props.Size.Height ? props.Size.Height : props.Size.Width;
                svgPic.css({ "width": iconSize + "px", "height": iconSize + "px" });
                var svgPath = svgPic.find('path');
                svgPath.attr({ "stroke": props.ForeColor, "fill": props.ForeColor })
                $(compId).empty();
                $(compId).append($("<div />").append(svgPic).html());
            }
        } else {
            svgAnimate.remove();
        }
    }
};

/********** Renderer Map **********/

// Add components to the renderer map
scada.scheme.rendererMap.set("Scada.Web.Plugins.SchSvgExtComp.Turbine", new scada.scheme.TurbineRenderer());