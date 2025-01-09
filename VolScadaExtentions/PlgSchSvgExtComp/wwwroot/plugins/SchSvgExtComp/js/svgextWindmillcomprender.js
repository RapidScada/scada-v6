/********** Static Picture Renderer **********/

// Static picture renderer type extends scada.scheme.ComponentRenderer
scada.scheme.Windmill1Renderer = function () {
    scada.scheme.ComponentRenderer.call(this);
};

scada.scheme.Windmill1Renderer.prototype = Object.create(scada.scheme.ComponentRenderer.prototype);
scada.scheme.Windmill1Renderer.constructor = scada.scheme.Windmill1Renderer;

scada.scheme.Windmill1Renderer.prototype.createDom = function (component, renderContext) {
    var props = component.props;
    var divComp = $("<div id='comp" + component.id + "'><i class='" + props.IconObj.Name + "' /></div>");
    var compId = '#comp' + component.id;

    this.prepareComponent(divComp, component);

    var svgPic = $(windmillUtil.windmill1Svg);
    var iconSize = props.size.width > props.size.height ? props.size.height : props.size.width;
    svgPic.css({ "width": iconSize + "px", "height": iconSize + "px" });
    component.dom = divComp;
    setTimeout(function () {
        $(compId).append($("<div />").append(svgPic).html());
    }, 10);
};

scada.scheme.Windmill1Renderer.prototype.refreshImages = function (component, renderContext, imageNames) {
    var props = component.props;

    /*if (Array.isArray(imageNames) && imageNames.includes(props.ImageName)) {
        var divComp = component.dom;
        var image = renderContext.getImage(props.ImageName);
        this.setBackgroundImage(divComp, image, true);
    }*/
};

scada.scheme.Windmill1Renderer.prototype.setSize = function (component, width, height) {
    scada.scheme.ComponentRenderer.prototype.setSize.call(this, component, width, height);

    var ImageStretches = scada.scheme.ImageStretches;
    var props = component.props;
    var divComp = component.dom;

    if (props.ImageStretch === ImageStretches.FILL) {
        divComp.css("background-size", props.size.width + "px " + props.size.height + "px");
    }
    var iconSize = props.size.width > props.size.height ? props.size.height : props.size.width;
    divComp.find('svg').css({ "width": iconSize + "px", "height": iconSize + "px" });
};

scada.scheme.Windmill1Renderer.prototype.updateData = function (component, renderContext) {
    var props = component.props;
    if (props.inCnlNum > 0) {
        var divComp = component.dom;
        var cnlDataExt = renderContext.getCnlDataExt(props.inCnlNum);

        // choose an image depending on the conditions
        var svgAnimate = divComp.find('svg g animateTransform');
        if (cnlDataExt.d.stat == 1) {
            if (svgAnimate.length == 0) {
                var svgPic = $(windmillUtil.windmill1Svg);
                var svgPicAnimate = windmillUtil.windmill1Animate;
                $(svgPicAnimate).appendTo(svgPic.find('g'));
                var compId = '#comp' + component.id;
                var iconSize = props.size.width > props.size.height ? props.size.height : props.size.width;
                svgPic.css({ "width": iconSize + "px", "height": iconSize + "px" });
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
scada.scheme.rendererMap.set("Scada.Web.Plugins.SchSvgExtComp.Windmill1", new scada.scheme.Windmill1Renderer());