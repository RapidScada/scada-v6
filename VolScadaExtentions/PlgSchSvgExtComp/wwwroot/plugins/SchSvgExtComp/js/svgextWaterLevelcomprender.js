/********** WaterLevel Renderer **********/

scada.scheme.WaterLevelRenderer = function () {
    scada.scheme.ComponentRenderer.call(this);
};

scada.scheme.WaterLevelRenderer.prototype = Object.create(scada.scheme.ComponentRenderer.prototype);
scada.scheme.WaterLevelRenderer.constructor = scada.scheme.WaterLevelRenderer;

// levelNum  0-100
scada.scheme.WaterLevelRenderer.prototype._setWaterLevel = function (component, renderContext, levelNum, color) {
    var props = component.props;
    props.curVal = levelNum;
    var compId = '#comp' + component.id;
    var baseVal = props.size.height;
    var canFlow = props.Flow;
    if (canFlow) {
        var levelMapper = baseVal / (props.maxValue - props.minValue) * levelNum;
        levelMapper = baseVal - (levelMapper > baseVal ? 5 : levelMapper);
        if (props.ForeColor == "Status") {
            d3.select(compId + ' svg g').attr({ "transform": "translate(0," + levelMapper + ")", "fill": color })
        } else {
            d3.select(compId + ' svg g').attr({ "transform": "translate(0," + levelMapper + ")" })
        }
    } else {
        var levelMapper = baseVal / (props.maxValue - props.minValue) * levelNum;
        levelMapper = baseVal - (levelMapper > baseVal ? 5 : levelMapper);
        if (props.ForeColor == "Status") {
            d3.select(compId + ' svg rect').attr({ "x": 0, "y": levelMapper, "fill": color })
        } else {
            d3.select(compId + ' svg rect').attr({ "x": 0, "y": levelMapper })
        }
    }
};

scada.scheme.WaterLevelRenderer.prototype.createDom = function (component, renderContext) {
    var that = this;
    var props = component.props;
    var divComp = $("<div id='comp" + component.id + "'></div>");
    var compId = '#comp' + component.id;

    this.prepareComponent(divComp, component);
    var canFlow = props.Flow; //流动

    var svgPic = canFlow ? $(waterLevelUtil["svg0"]) : $(waterLevelUtil["svgStatic"]);
    svgPic.attr({ "width": props.size.width - props.borderWidth, "height": props.size.height });
    if (canFlow) {
        svgPic.find('g').attr({ "fill": props.foreColor });
    } else {
        svgPic.find('rect').attr({ "fill": props.foreColor, "width": props.size.width - props.borderWidth * 2, "height": props.size.height });
    }

    component.dom = divComp;
    setTimeout(function () {
        $(compId).append($('<div></div>').append(svgPic).html());
        that._setWaterLevel(component, renderContext, 50, "oriange")
    }, 10);
};

scada.scheme.WaterLevelRenderer.prototype.refreshImages = function (component, renderContext, imageNames) {
    var props = component.props;

    /*if (Array.isArray(imageNames) && imageNames.includes(props.ImageName)) {
        var divComp = component.dom;
        var image = renderContext.getImage(props.ImageName);
        this.setBackgroundImage(divComp, image, true);
    }*/
};

scada.scheme.WaterLevelRenderer.prototype.setSize = function (component, width, height) {
    scada.scheme.ComponentRenderer.prototype.setSize.call(this, component, width, height);

    var props = component.props;
    var canFlow = props.Flow;
    var compId = '#comp' + component.id;
    var svgPic = $(compId + " svg");
    var svgPic = canFlow ? $(waterLevelUtil["svg0"]) : $(waterLevelUtil["svgStatic"]);

    svgPic.css({ "width": props.size.width + "px", "height": props.size.height + "px" });
    if (canFlow) {
        svgPic.find('g').attr({ "fill": props.foreColor });
    } else {
        svgPic.find('rect').attr({ "fill": props.foreColor, "width": props.size.width - props.borderWidth * 2, "height": props.size.height });
    }
    $(compId).empty();
    $(compId).append($('<div></div>').append(svgPic).html());

    this._setWaterLevel(component, null, 50, "oriange");
};

scada.scheme.WaterLevelRenderer.prototype.updateData = function (component, renderContext) {
    var that = this;
    var props = component.props;
    if (!props.offsetNum) props.offsetNum = 0;
    if (props.inCnlNum > 0) {
        var cnlDataExt = renderContext.getCnlDataExt(props.inCnlNum);
        if (cnlDataExt.d.stat == 1) {
            that._setWaterLevel(component, renderContext, cnlDataExt.d.val, cnlDataExt.df.colors[0])
        } else {
            that._setWaterLevel(component, renderContext, 0, cnlDataExt.df.colors[0])
        }
    }
};

/********** Renderer Map **********/

// Add components to the renderer map
scada.scheme.rendererMap.set("Scada.Web.Plugins.SchSvgExtComp.WaterLevel", new scada.scheme.WaterLevelRenderer());