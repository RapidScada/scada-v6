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
    var svgName = props.turbineName.substr(0, props.turbineName.indexOf('.'));
    var svgPic = $(turbineUtil["turbine" + svgName]);
    var iconSize = props.size.width > props.size.height ? props.size.height : props.size.width;
    svgPic.css({ "width": iconSize + "px", "height": iconSize + "px" });
    var svgPath = svgPic.find('path');
    svgPath.attr({ "stroke": props.foreColor, "fill": props.foreColor })
    component.dom = divComp;
    setTimeout(function () {
        $(compId).append($("<div />").append(svgPic).html());
    }, 10);
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
        divComp.css("background-size", props.size.width + "px " + props.size.height + "px");
    }
    var iconSize = props.size.width > props.size.height ? props.size.height : props.size.width;
    divComp.find('svg').css({ "width": iconSize + "px", "height": iconSize + "px" });
    var svgPath = divComp.find('svg path');
    svgPath.attr({ "stroke": props.foreColor, "fill": props.foreColor })
};

scada.scheme.TurbineRenderer.prototype.updateData = function (component, renderContext) {
    var props = component.props;
    if (props.inCnlNum > 0) {
        var divComp = component.dom;
        var cnlDataExt = renderContext.getCnlDataExt(props.inCnlNum);
        var svgAnimate = divComp.find('svg g animateTransform');
        if (cnlDataExt.d.stat == 1 && cnlDataExt.d.val > 0) {
            if (svgAnimate.length == 0) {
                var svgName = props.turbineName.substr(0, props.turbineName.indexOf('.'));
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
                var iconSize = props.size.width > props.size.height ? props.size.height : props.size.width;
                svgPic.css({ "width": iconSize + "px", "height": iconSize + "px" });
                var svgPath = svgPic.find('path');
                svgPath.attr({ "stroke": props.foreColor, "fill": props.foreColor })
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