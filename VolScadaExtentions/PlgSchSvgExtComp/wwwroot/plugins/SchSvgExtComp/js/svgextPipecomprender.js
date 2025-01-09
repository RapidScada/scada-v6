/********** Static Picture Renderer **********/

// Static picture renderer type extends scada.scheme.ComponentRenderer
scada.scheme.BasePipeRenderer = function () {
    scada.scheme.ComponentRenderer.call(this);
};

scada.scheme.BasePipeRenderer.prototype = Object.create(scada.scheme.ComponentRenderer.prototype);
scada.scheme.BasePipeRenderer.constructor = scada.scheme.BasePipeRenderer;

scada.scheme.BasePipeRenderer.prototype.createDom = function (component, renderContext) {
    var props = component.props;
    var divComp = $("<div id='comp" + component.id + "'></div>");
    var compId = '#comp' + component.id;
    this.prepareComponent(divComp, component);

    var svgPic = $(pipeUtil.pipe1Svg);
    //pipe width
    component.dom = divComp;
    var strokeDasharray = props.pipeSize.solidWidth + " " + props.pipeSize.dashWidth
    var pipePadding = props.pipePadding * 2;
    setTimeout(function () {
        $(compId).append($('<div></div>').append(svgPic).html());
        svgPic = $(compId + " svg");
        if (props.pipeDirection == 0 || props.pipeDirection == 1) {
            var pipeWidth = props.size.height;
            var lineWidth = pipeWidth - pipePadding;
            if (pipeWidth < 15) svgPic.css({ "width": props.size.width + "px" });
            else svgPic.css({ "width": props.size.width + "px", "height": props.size.height + "px" });
            var startY = props.size.height / 2, endX = props.size.width, endY = props.size.height / 2;
            d3.select(compId + ' line[name=line]').attr({ x1: 0, y1: startY, x2: endX, y2: endY, 'stroke-width': lineWidth, "stroke-dasharray": strokeDasharray, "stroke": props.pipeColor });
            d3.select(compId + ' line[name=line_bg]').attr({ x1: 0, y1: startY, x2: endX, y2: endY, 'stroke-width': pipeWidth, "stroke": props.pipeBackColor });
        } else {
            var pipeWidth = props.size.width;
            if (props.size.height < 15) svgPic.css({ "width": props.size.width + "px" });
            else svgPic.css({ "width": props.size.width + "px", "height": props.size.height + "px" });
            var endX = props.size.width / 2, endY = props.size.height;
            var lineWidth = pipeWidth - pipePadding;
            d3.select(compId + ' line[name=line]').attr({ x1: endX, y1: 0, x2: endX, y2: endY, 'stroke-width': lineWidth, "stroke-dasharray": strokeDasharray, "stroke": props.pipeColor });
            d3.select(compId + ' line[name=line_bg]').attr({ x1: endX, y1: 0, x2: endX, y2: endY, 'stroke-width': pipeWidth, "stroke": props.pipeBackColor });
        }
    }, 10);
};

scada.scheme.BasePipeRenderer.prototype.refreshImages = function (component, renderContext, imageNames) {
    var props = component.props;

    /*if (Array.isArray(imageNames) && imageNames.includes(props.ImageName)) {
        var divComp = component.dom;
        var image = renderContext.getImage(props.ImageName);
        this.setBackgroundImage(divComp, image, true);
    }*/
};

scada.scheme.BasePipeRenderer.prototype.setSize = function (component, width, height) {
    scada.scheme.ComponentRenderer.prototype.setSize.call(this, component, width, height);
    var props = component.props;

    var compId = '#comp' + component.id;
    var svgPic = $(compId + " svg");
    // svgPic.css({ "width": props.size.width + "px", "height": props.size.height + "px" });

    var pipeWidth = props.size.width;
    var pipePadding = props.pipePadding * 2;
    var strokeDasharray = props.pipeSize.SolidWidth + " " + props.pipeSize.dashWidth
    if (props.pipeDirection == 0 || props.pipeDirection == 1) {
        var pipeWidth = props.size.height;
        if (pipeWidth < 15) svgPic.css({ "width": props.size.width + "px" });
        else svgPic.css({ "width": props.size.width + "px", "height": props.size.height + "px" });
        var startY = props.size.height / 2, endX = props.size.width, endY = props.size.height / 2;
        var lineWidth = pipeWidth - pipePadding;
        d3.select(compId + ' line[name=line]').attr({ x1: 0, y1: startY, x2: endX, y2: endY, 'stroke-width': lineWidth, "stroke-dasharray": strokeDasharray, "stroke": props.pipeColor });
        d3.select(compId + ' line[name=line_bg]').attr({ x1: 0, y1: startY, x2: endX, y2: endY, 'stroke-width': pipeWidth });
    } else {
        var pipeWidth = props.size.width;
        if (props.size.height < 15) svgPic.css({ "width": props.size.width + "px" });
        else svgPic.css({ "width": props.size.width + "px", "height": props.size.height + "px" });
        var endX = props.size.width / 2, endY = props.size.height;
        var lineWidth = pipeWidth - pipePadding;
        d3.select(compId + ' line[name=line]').attr({ x1: endX, y1: 0, x2: endX, y2: endY, 'stroke-width': lineWidth, "stroke-dasharray": strokeDasharray, "stroke": props.pipeColor });
        d3.select(compId + ' line[name=line_bg]').attr({ x1: endX, y1: 0, x2: endX, y2: endY, 'stroke-width': pipeWidth, "stroke": props.pipeBackColor });
    }
};

scada.scheme.BasePipeRenderer.prototype.updateData = function (component, renderContext) {
    var props = component.props;
    var compId = '#comp' + component.id;
    if (!props.offsetNum) props.offsetNum = 0;
    if (props.inCnlNum > 0) {
        var cnlDataExt = renderContext.getCnlDataExt(props.inCnlNum);
        if (cnlDataExt.d.stat) {
            if (props.tmrInterval) clearInterval(props.tmrInterval);
            var curVal = cnlDataExt.d.val * 100;
            if (curVal != 0) {
                if (Math.abs(curVal) < 1) return;
                var speed = (props.maxValue - props.minValue) * 1000 / Math.abs(curVal);
                props.tmrInterval = setInterval(function () {
                    if (props.offsetNum > 92233720368547758000 || props.offsetNum < -9223372036854775800) props.offsetNum = 0;
                    if (curVal > 0) {
                        (props.pipeDirection == 0 || props.pipeDirection == 2) ? props.offsetNum++ : props.offsetNum--;
                    } else {
                        (props.pipeDirection == 0 || props.pipeDirection == 2) ? props.offsetNum-- : props.offsetNum++;
                    }

                    d3.select(compId + ' line[name=line]').attr('stroke-dashoffset', props.offsetNum);
                }, speed)
            }
            if (curVal == 0) {
                d3.select(compId + ' line[name=line]').attr({ "stroke": "#ccc" });
                d3.select(compId + ' line[name=line_bg]').attr({ "stroke": "#ccc" });
            }
            else if (props.PipeColor == 'Status') {
                d3.select(compId + ' line[name=line]').attr({ "stroke": cnlDataExt.df.colors[0] });
                d3.select(compId + ' line[name=line_bg]').attr({ "stroke": props.pipeBackColor });
            } else {
                d3.select(compId + ' line[name=line]').attr({ "stroke": props.pipeColor });
                d3.select(compId + ' line[name=line_bg]').attr({ "stroke": props.pipeBackColor });
            }
        } else {
            if (props.tmrInterval) clearInterval(props.tmrInterval);
            d3.select(compId + ' line[name=line]').attr({ "stroke": "#ccc" });
            d3.select(compId + ' line[name=line_bg]').attr({ "stroke": "#ccc" });
        }
    }
};

/********** Renderer Map **********/
// EditablePipe renderer type extends scada.scheme.ComponentRenderer
scada.scheme.EditablePipeRenderer = function () {
    scada.scheme.ComponentRenderer.call(this);
};

scada.scheme.EditablePipeRenderer.prototype = Object.create(scada.scheme.ComponentRenderer.prototype);
scada.scheme.EditablePipeRenderer.constructor = scada.scheme.EditablePipeRenderer;

scada.scheme.EditablePipeRenderer.prototype.calculateAnimationDuration = function (minValue, maxValue, currentValue, minDuration, maxDuration) {
    // 计算当前值在最小值和最大值之间的比例
    const ratio = (currentValue - minValue) / (maxValue - minValue);
    // 根据比例计算动画持续时间
    const duration = (maxDuration - minDuration) * ratio;
    return maxDuration - duration;
}

scada.scheme.EditablePipeRenderer.prototype.createDom = function (component, renderContext) {
    var that = this;
    var props = component.props;
    console.log(props)
    var divComp = $("<div id='comp" + component.id + "'></div>");
    var compId = '#comp' + component.id;
    this.prepareComponent(divComp, component);

    var svgPic = $(pipeUtil.pipe2Svg);
    component.dom = divComp;
    setTimeout(function () {
        $(compId).append($('<div></div>').append(svgPic).html());
        svgPic = $(compId + " svg");
        var pipeSize = props.pipeSize;
        svgPic.css({ "width": props.size.width + "px", "height": props.size.height + "px" });
        var startY = 10, endX = props.size.width;
        var pipeLines = d3.select(compId + ' g[name=svgContainer]').append('g').attr("id", "lines");
        var pipeBack = pipeLines.append("path").attr({ "fill": "none", "class": "pipe-back path-round" }).attr("stroke", props.pipeBackColor || "#0a7ae2")
            .attr("stroke-width", pipeSize.solidWidth).attr("d", "M 0 " + startY + " L " + endX + ' ' + startY)
        if (props.pipeLineType == 1) {//管
            var pipeFlow = pipeLines.append("path").attr({ "fill": "none", "class": "pipe-flow path-round" }).attr("stroke", props.pipeFlowColor || "#119bfa")
                .attr("d", "M 0 " + startY + " L " + endX + ' ' + startY).attr("stroke-width", pipeSize.flowWidth)
                .attr("stroke-dasharray", pipeSize.dashWidth).attr("d", "M 0 " + startY + " L " + endX + ' ' + startY);
            var pipeAnimate = pipeFlow.append("animate").attr({ "attributeName": "stroke-dashoffset", "repeatCount": "indefinite", "fill": "freeze" })
            if (props.pipeDirection == 0) pipeAnimate.attr({ "from": "1000", "to": "0", "dur": "20s" })//正向
            else pipeAnimate.attr({ "from": "0", "to": "1000", "dur": "20s" })
            if (props.auxData) pipeFlow.attr("d", props.auxData)
        } else if (props.pipeLineType == 2) {//轨迹
            var pipeCircle = pipeLines.append("circle").attr({ "cx": "0", "cy": "0", "r": (pipeSize.solidWidth || 10) * 0.6, "fill": props.pipeFlowColor || "#119bfa" })
            var pipeCircleAnimate = pipeCircle.append("animateMotion").attr({ "repeatCount": "indefinite", "dur": "20s" }).attr("path", pipeBack.attr('d'));
            if (props.auxData) pipeCircleAnimate.attr("path", props.auxData)
        }
        if (props.auxData) {
            pipeBack.attr("d", props.auxData)
        }
    }, 10);
};
scada.scheme.EditablePipeRenderer.prototype.updateData = function (component, renderContext) {
    var props = component.props;
    var compId = '#comp' + component.id;
    if (props.inCnlNum > 0) {
        var cnlDataExt = renderContext.getCnlDataExt(props.inCnlNum);
        var svgPathGr = d3.selectAll(compId + ' g[id=lines]');
        var flowColor = props.pipeFlowColor;
        if (props.conditions && props.conditions.length > 0) {
            var cnlVal = cnlDataExt.d.val;
            for (var cond of props.conditions) {
                if (scada.scheme.calc.conditionSatisfied(cond, cnlVal)) {
                    flowColor = cond.color;
                    break;
                }
            }
        }
        if (props.pipeLineType == 1) {//管
            if (cnlDataExt.d.stat) {
                var curVal = cnlDataExt.d.val;
                var dur = "1000s";//1000s标识暂停
                if (curVal == 0) {
                    svgPathGr.select(".pipe-flow animate").attr("dur", "1000s");//1000s标识暂停
                } else if (curVal != props.preVal) {
                    //const ratio = (curVal - props.minValue) / (props.maxValue - props.minValue)
                    //var speed = (props.maxValue - props.minValue) / Math.abs(curVal) * 10;
                    let minDuration = 10, maxDuration = 30;
                    var speed = this.calculateAnimationDuration(props.minValue, props.maxValue, curVal, minDuration, maxDuration);
                    speed = speed <= minDuration ? minDuration : speed;
                    svgPathGr.select(".pipe-flow animate").attr("dur", speed + "s");
                    props.preVal = curVal;
                }
                svgPathGr.select('.pipe-back').attr({ "stroke": props.pipeBackColor });
                if (flowColor == 'Status') {
                    svgPathGr.select('.pipe-flow').attr({ "stroke": cnlDataExt.df.color });
                } else {
                    svgPathGr.select('.pipe-flow').attr({ "stroke": flowColor });
                }
            } else {
                svgPathGr.select('.pipe-flow').attr({ "stroke": "#ccc" });
                svgPathGr.select('.pipe-back').attr({ "stroke": "#ddd" });
            }
        } else if (props.pipeLineType == 2) {//轨迹

        } else {//线路

        }
    }
};

// Add components to the renderer map
scada.scheme.rendererMap.set("Scada.Web.Plugins.SchSvgExtComp.BasePipe", new scada.scheme.BasePipeRenderer());
scada.scheme.rendererMap.set("Scada.Web.Plugins.SchSvgExtComp.EditablePipe", new scada.scheme.EditablePipeRenderer());