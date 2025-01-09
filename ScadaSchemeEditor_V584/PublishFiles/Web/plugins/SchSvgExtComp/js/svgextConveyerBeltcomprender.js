/********** ConveyerBelt Renderer **********/

// ConveyerBelt renderer type extends scada.scheme.ComponentRenderer
scada.scheme.ConveyerBeltRenderer = function () {
    scada.scheme.ComponentRenderer.call(this);
};

scada.scheme.ConveyerBeltRenderer.prototype = Object.create(scada.scheme.ComponentRenderer.prototype);
scada.scheme.ConveyerBeltRenderer.constructor = scada.scheme.ConveyerBeltRenderer;

scada.scheme.ConveyerBeltRenderer.prototype.createDom = function (component, renderContext) {
    var props = component.props;
    var divComp = $("<div id='comp" + component.id + "'></div>");
    var compId = '#comp' + component.id;
    var svgId = 'svg_' + component.id;
    this.prepareComponent(divComp, component);
    if (props.ConveyerName) {
        setTimeout(function () {
            var conveyerContent = props.ConveyerContent.replaceAll('.cls', '#' + svgId + ' .cls')
            $(compId).append($("<div />").append(conveyerContent).html());
            var svgPic = $(compId + " svg");
            svgPic.attr('id', svgId)
            svgPic.attr({ "width": "100%", "height": "100%" });
            svgPic.find('.cls-100').css('fill', 'Gray')
        }, 10);
    } else {
        $(compId).append($("<div />").append("请选择传送带").html());
    }
    component.dom = divComp;
};

scada.scheme.ConveyerBeltRenderer.prototype.refreshImages = function (component, renderContext, imageNames) {
    var props = component.props;
};

scada.scheme.ConveyerBeltRenderer.prototype.setSize = function (component, width, height) {
    scada.scheme.ComponentRenderer.prototype.setSize.call(this, component, width, height);

    var ImageStretches = scada.scheme.ImageStretches;
    var props = component.props;
    var divComp = component.dom;

    //var iconSize = props.Size.Width > props.Size.Height ? props.Size.Height : props.Size.Width;
    //divComp.find('svg').attr({ "width": iconSize + "px", "height": props.Size.Height + "px" });
    divComp.find('svg').attr({ "width": "100%", "height": "100%" });
};

scada.scheme.ConveyerBeltRenderer.prototype.updateData = function (component, renderContext) {
    var props = component.props;
    if (props.InCnlNum > 0) {
    }
};

/********** Renderer Map **********/

// Add components to the renderer map
scada.scheme.rendererMap.set("Scada.Web.Plugins.SchSvgExtComp.ConveyerBelt", new scada.scheme.ConveyerBeltRenderer());