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
    var strokeDasharray = props.PipeSize.SolidWidth + " " + props.PipeSize.DashWidth
    var pipePadding = props.PipePadding * 2;
    setTimeout(function () {
        $(compId).append($('<div></div>').append(svgPic).html());
        svgPic = $(compId + " svg");
        if (props.PipeDirection == 0 || props.PipeDirection == 1) {
            var pipeWidth = props.Size.Height;
            var lineWidth = pipeWidth - pipePadding;
            if (pipeWidth < 15) svgPic.css({ "width": props.Size.Width + "px" });
            else svgPic.css({ "width": props.Size.Width + "px", "height": props.Size.Height + "px" });
            var startY = props.Size.Height / 2, endX = props.Size.Width, endY = props.Size.Height / 2;
            d3.select(compId + ' line[name=line]').attr({ x1: 0, y1: startY, x2: endX, y2: endY, 'stroke-width': lineWidth, "stroke-dasharray": strokeDasharray, "stroke": props.PipeColor });
            d3.select(compId + ' line[name=line_bg]').attr({ x1: 0, y1: startY, x2: endX, y2: endY, 'stroke-width': pipeWidth, "stroke": props.PipeBackColor });
        } else {
            var pipeWidth = props.Size.Width;
            if (props.Size.Height < 15) svgPic.css({ "width": props.Size.Width + "px" });
            else svgPic.css({ "width": props.Size.Width + "px", "height": props.Size.Height + "px" });
            var endX = props.Size.Width / 2, endY = props.Size.Height;
            var lineWidth = pipeWidth - pipePadding;
            d3.select(compId + ' line[name=line]').attr({ x1: endX, y1: 0, x2: endX, y2: endY, 'stroke-width': lineWidth, "stroke-dasharray": strokeDasharray, "stroke": props.PipeColor });
            d3.select(compId + ' line[name=line_bg]').attr({ x1: endX, y1: 0, x2: endX, y2: endY, 'stroke-width': pipeWidth, "stroke": props.PipeBackColor });
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
    // svgPic.css({ "width": props.Size.Width + "px", "height": props.Size.Height + "px" });

    var pipeWidth = props.Size.Width;
    var pipePadding = props.PipePadding * 2;
    var strokeDasharray = props.PipeSize.SolidWidth + " " + props.PipeSize.DashWidth
    if (props.PipeDirection == 0 || props.PipeDirection == 1) {
        var pipeWidth = props.Size.Height;
        if (pipeWidth < 15) svgPic.css({ "width": props.Size.Width + "px" });
        else svgPic.css({ "width": props.Size.Width + "px", "height": props.Size.Height + "px" });
        var startY = props.Size.Height / 2, endX = props.Size.Width, endY = props.Size.Height / 2;
        var lineWidth = pipeWidth - pipePadding;
        d3.select(compId + ' line[name=line]').attr({ x1: 0, y1: startY, x2: endX, y2: endY, 'stroke-width': lineWidth, "stroke-dasharray": strokeDasharray, "stroke": props.PipeColor });
        d3.select(compId + ' line[name=line_bg]').attr({ x1: 0, y1: startY, x2: endX, y2: endY, 'stroke-width': pipeWidth });
    } else {
        var pipeWidth = props.Size.Width;
        if (props.Size.Height < 15) svgPic.css({ "width": props.Size.Width + "px" });
        else svgPic.css({ "width": props.Size.Width + "px", "height": props.Size.Height + "px" });
        var endX = props.Size.Width / 2, endY = props.Size.Height;
        var lineWidth = pipeWidth - pipePadding;
        d3.select(compId + ' line[name=line]').attr({ x1: endX, y1: 0, x2: endX, y2: endY, 'stroke-width': lineWidth, "stroke-dasharray": strokeDasharray, "stroke": props.PipeColor });
        d3.select(compId + ' line[name=line_bg]').attr({ x1: endX, y1: 0, x2: endX, y2: endY, 'stroke-width': pipeWidth, "stroke": props.PipeBackColor });
    }
};

scada.scheme.BasePipeRenderer.prototype.updateData = function (component, renderContext) {
    var props = component.props;
    var compId = '#comp' + component.id;
    if (!props.offsetNum) props.offsetNum = 0;
    if (props.InCnlNum > 0) {
        var cnlDataExt = renderContext.getCnlDataExt(props.InCnlNum);
        if (cnlDataExt.Stat) {
            if (props.tmrInterval) clearInterval(props.tmrInterval);
            var curVal = cnlDataExt.Val * 100;
            if (curVal != 0) {
                if (Math.abs(curVal) < 1) return;
                var speed = (props.MaxValue - props.MinValue) * 1000 / Math.abs(curVal);
                props.tmrInterval = setInterval(function () {
                    if (props.offsetNum > 92233720368547758000 || props.offsetNum < -9223372036854775800) props.offsetNum = 0;
                    if (curVal > 0) {
                        (props.PipeDirection == 0 || props.PipeDirection == 2) ? props.offsetNum++ : props.offsetNum--;
                    } else {
                        (props.PipeDirection == 0 || props.PipeDirection == 2) ? props.offsetNum-- : props.offsetNum++;
                    }

                    d3.select(compId + ' line[name=line]').attr('stroke-dashoffset', props.offsetNum);
                }, speed)
            }
            if (curVal == 0) {
                d3.select(compId + ' line[name=line]').attr({ "stroke": "#ccc" });
                d3.select(compId + ' line[name=line_bg]').attr({ "stroke": "#ccc" });
            }
            else if (props.PipeColor == 'Status') {
                d3.select(compId + ' line[name=line]').attr({ "stroke": cnlDataExt.Color });
                d3.select(compId + ' line[name=line_bg]').attr({ "stroke": props.PipeBackColor });
            } else {
                d3.select(compId + ' line[name=line]').attr({ "stroke": props.PipeColor });
                d3.select(compId + ' line[name=line_bg]').attr({ "stroke": props.PipeBackColor });
            }
        } else {
            if (props.tmrInterval) clearInterval(props.tmrInterval);
            d3.select(compId + ' line[name=line]').attr({ "stroke": "#ccc" });
            d3.select(compId + ' line[name=line_bg]').attr({ "stroke": "#ccc" });
        }
    }
};


var svgUtils = {
    /**
     * 将svg路径 转数组
    */
    PATH_COMMANDS: {
        M: ["x", "y"],
        m: ["dx", "dy"],
        H: ["x"],
        h: ["dx"],
        V: ["y"],
        v: ["dy"],
        L: ["x", "y"],
        l: ["dx", "dy"],
        Z: [],
        C: ["x1", "y1", "x2", "y2", "x", "y"],
        c: ["dx1", "dy1", "dx2", "dy2", "dx", "dy"],
        S: ["x2", "y2", "x", "y"],
        s: ["dx2", "dy2", "dx", "dy"],
        Q: ["x1", "y1", "x", "y"],
        q: ["dx1", "dy1", "dx", "dy"],
        T: ["x", "y"],
        t: ["dx", "dy"],
        A: ["rx", "ry", "rotation", "large-arc", "sweep", "x", "y"],
        a: ["rx", "ry", "rotation", "large-arc", "sweep", "dx", "dy"],
    },
    fromPathToArray: function (path) {
        const items = path
            .replace(/[\n\r]/g, "")
            .replace(/-/g, " -")
            .replace(/(\d*\.)(\d+)(?=\.)/g, "$1$2 ")
            .trim()
            .split(/\s*,|\s+/);
        const segments = [];
        let currentCommand = "";
        let currentElement = { type: "", x: 0, y: 0 };
        while (items.length > 0) {
            let it = items.shift();
            if (this.PATH_COMMANDS.hasOwnProperty(it)) {
                currentCommand = it;
            } else {
                items.unshift(it);
            }
            currentElement = { x: 0, y: 0, type: currentCommand };
            this.PATH_COMMANDS[currentCommand].forEach(prop => {
                it = items.shift();
                currentElement[prop] = it;
            });
            if (currentCommand === "M") {
                currentCommand = "L";
            } else if (currentCommand === "m") {
                currentCommand = "l";
            }
            segments.push(currentElement);
        }
        return segments.filter((item) => !["V", "H", "Q"].includes(item.type));
    },
    fromArrayToPath: function (pathList) {
        let nPath = "";
        pathList.forEach((item) => {
            if (item.type == "Q") {
                nPath += `${item.type} ${item.x1} ${item.y1} ${item.x} ${item.y} `;
            } else {
                nPath += `${item.type} ${item.x} ${item.y} `;
            }
        });
        return nPath;
    },
    areCollinear: function (a, b, c, lineWidth) {
        if (Math.abs(a.x - c.x) <= lineWidth && Math.abs(b.x - c.x) <= lineWidth) {
            return 0;
        }
        return Math.abs(this.slope(a, b) - this.slope(b, c));
    },
    slope: function (coor1, coor2) {
        if (coor2.x - coor1.x == 0) {
            return 0;
        }
        return (coor2.y - coor1.y) / (coor2.x - coor1.x);
    }
}


// EditablePipe renderer type extends scada.scheme.ComponentRenderer
scada.scheme.EditablePipeRenderer = function () {
    this.MIN_MOVING = 1;
    this.StartPos = {};
    this.PointPos = {};
    this.Moved = false;
    this.Mode = 0;
    this.draggedElem = $();
    this.component = $();
    scada.scheme.ComponentRenderer.call(this);
};

scada.scheme.EditablePipeRenderer.prototype = Object.create(scada.scheme.ComponentRenderer.prototype);
scada.scheme.EditablePipeRenderer.constructor = scada.scheme.EditablePipeRenderer;
scada.scheme.EditablePipeRenderer.prototype._startDragging = function (component, targetCircle, pageX, pageY) {
    var DragModes = scada.scheme.DragModes;
    //console.warn(targetCircle,pageX,pageY)
    this.Mode = DragModes.MOVE;
    this.Moved = false;
    this.StartPos.x = pageX;
    this.StartPos.y = pageY;
    this.PointPos.x = +targetCircle.attr("cx");
    this.PointPos.y = +targetCircle.attr("cy");
    this.draggedCircle = targetCircle;
    this.component = component;
}
scada.scheme.EditablePipeRenderer.prototype._continueDragging = function (pageX, pageY) {
    var DragModes = scada.scheme.DragModes;
    const startPos = this.StartPos;
    var dx = pageX - startPos.x;
    var dy = pageY - startPos.y;
    if (this.draggedCircle != $("") &&
        (this.Moved || Math.abs(dx) >= this.MIN_MOVING || Math.abs(dy) >= this.MIN_MOVING)) {
        if (this.Mode === DragModes.MOVE) {
            this.Moved = true;
            let targetX = dx + this.PointPos.x;
            let targetY = dy + this.PointPos.y;
            //设定点的位置
            d3.select("#" + this.draggedCircle.attr('id')).attr({ "cx": targetX, "cy": targetY })
            var pathElems = this.component.find("g[name='svgContainer'] path");
            var pathList = svgUtils.fromPathToArray(pathElems.eq(0).attr('d'));
            pathList[this.draggedCircle.index()].x = targetX;
            pathList[this.draggedCircle.index()].y = targetY;
            var newPath = svgUtils.fromArrayToPath(pathList);
            pathElems.each(function () {
                $(this).attr("d", newPath)
            })
        }
    }
}
scada.scheme.EditablePipeRenderer.prototype._stopDragging = function (callback) {
    this.Mode = scada.scheme.DragModes.NONE;
    //计算圆滑曲线

    // execute callback function
    if (this.Moved && typeof callback === "function") {
        callback(this.component.find("g[id='lines'] path").eq(0).attr("d"));
        //callback(this.lastDx, this.lastDy, this.lastW, this.lastH);
    }
}
/** 重新定义鼠标事件，避免影响顶层 */
scada.scheme.EditablePipeRenderer.prototype._resetMouseEvent = function (component, renderContext) {
    if (!renderContext.editMode) return;
    let pipeRender = this;
    var compId = '#comp' + component.id;
    var circleSelet = "svg g[id=linepoints] circle"
    $("#divSchWrapper").on('click mousedown', function (event) {
        setTimeout(() => {
            var allPointList = $('.comp-wrapper').not('.selected').find('g[id=linepoints]')
            allPointList.each(function () {
                $(this).css("display", "none")
            })
        }, 400);
    })
    $("#divSchWrapper " + compId)
        .on('mousedown', circleSelet, function (event) {
            if ($(compId).parent().hasClass("selected")) {
                pipeRender._startDragging($(compId), $(event.target), event.pageX, event.pageY);
                event.stopPropagation();
            }
        })
        .on('mousemove', function (event) {
            pipeRender._continueDragging(event.pageX, event.pageY);
        })
        .on("mouseup mouseleave", function (event) {
            //console.warn('mouseup mouseleave')
            if (pipeRender.Mode !== 0) {//DragModes.NONE
                pipeRender._stopDragging(function (svgData) {
                    // send changes to server under the assumption that the selection was not changed during dragging
                    $(compId).trigger("saveAuxData", { compId, svgData })
                });
            }
            pipeRender.Mode = 0;
        })
        .on('click', function (event) {
            if ($(compId).parent().hasClass("selected")) {
                $(compId + ' g[id=linepoints]').css("display", "block")
            }
        })
        .on('dblclick', "svg g[id=lines]", function (event) {
            var pathElem = $(this).children().eq(0);
            //添加点
            var oldPath = pathElem.attr('d');
            var strokeWidth = pathElem.attr('stroke-width');
            var pathList = svgUtils.fromPathToArray(oldPath);
            //计算位置

            const cx = event.pageX - $(compId).offset().left;
            const cy = event.pageY - $(compId).offset().top;
            const ratioList = [];
            for (let i = 0; i < pathList.length - 1; i++) {
                const ax = pathList[i].x || 0;
                const bx = pathList[i + 1].x || 0;
                const ay = pathList[i].y || 0;
                const by = pathList[i + 1].y || 0;
                //计算斜率差值
                ratioList[i] = 9999999;
                if (((cx >= ax && cx <= bx) || (cx <= ax && cx >= bx)) && ((cy >= ay && cy <= by) || (cy <= ay && cy >= by))) {
                    ratioList[i] = svgUtils.areCollinear({ x: ax, y: ay }, { x: cx, y: cy }, { x: bx, y: by }, strokeWidth);
                }
            }
            const minRatio = [...ratioList].sort((a, b) => a - b)[0];
            const index = ratioList.findIndex((item) => item == minRatio);
            pathList.splice(index + 1, 0, {
                type: "L",
                x: cx,
                y: cy,
            });
            var newPath = svgUtils.fromArrayToPath(pathList);

            var pathElems = $(compId + " g[id='lines'] path");
            pathElems.each(function () {
                $(this).attr("d", newPath)
            })
            pipeRender._refreshLinePoints(component);
            //event.stopPropagation();
        })
        .on('dblclick', "svg circle:not(:last)", function (event) {
            //移除点
            if ($(this).index() !== 0) {
                var pathElems = $(compId + " g[id='lines'] path");
                var oldPath = pathElems.eq(0).attr('d');
                var pathList = svgUtils.fromPathToArray(oldPath);
                pathList.splice($(this).index(), 1);
                var newPath = svgUtils.fromArrayToPath(pathList);
                pathElems.each(function () {
                    $(this).attr("d", newPath)
                })
                pipeRender._refreshLinePoints(component);
                event.stopPropagation();
            }
        })
        .on("selectstart", circleSelet, false)
        .on("dragstart", false);
}
/** 添加分段节点组 */
scada.scheme.EditablePipeRenderer.prototype._refreshLinePoints = function (component) {
    var props = component.props;
    var compId = '#comp' + component.id;
    var linePointSel = " g[id=linepoints]"
    if ($(compId + linePointSel).length == 0) {
        d3.select(compId + ' g[name=svgContainer]').append('g').attr("id", "linepoints")
            .attr("style", "display:none;");
    } else {
        $(compId + linePointSel).empty();
    }
    var pathList = svgUtils.fromPathToArray(d3.select(compId + ' path').attr('d'));
    for (let i = 0; i < pathList.length; i++) {
        const pathItem = pathList[i];
        //添加分段节点
        d3.select(compId + linePointSel).append('circle')
            .attr("id", 'c_' + component.id + "_" + i)
            .attr("cx", pathItem.x)
            .attr("cy", pathItem.y)
            .attr("r", "8")
            .attr("stroke", "#409EFF")
            .attr("fill", "#fff")
            .attr("cursor", "pointer");
        if ($(compId).parent().hasClass("selected")) {
            $(compId + linePointSel).css("display", "block")
        }
    }
}
scada.scheme.EditablePipeRenderer.prototype.createDom = function (component, renderContext) {
    var that = this;
    var props = component.props;
    var divComp = $("<div id='comp" + component.id + "'></div>");
    var compId = '#comp' + component.id;
    this.prepareComponent(divComp, component);

    var svgPic = $(pipeUtil.pipe2Svg);
    component.dom = divComp;
    setTimeout(function () {
        $(compId).append($('<div></div>').append(svgPic).html());
        svgPic = $(compId + " svg");
        var pipeSize = props.PipeSize;
        svgPic.css({ "width": props.Size.Width + "px", "height": props.Size.Height + "px" });
        var startY = 10, endX = props.Size.Width;
        var pipeLines = d3.select(compId + ' g[name=svgContainer]').append('g').attr("id", "lines");
        var pipeBack = pipeLines.append("path").attr({ "fill": "none", "class": "pipe-back path-round" }).attr("stroke", props.PipeBackColor || "#0a7ae2")
            .attr("stroke-width", pipeSize.SolidWidth).attr("d", "M 0 " + startY + " L " + endX + ' ' + startY)
        if (props.PipeLineType == 1) {//管
            var pipeFlow = pipeLines.append("path").attr({ "fill": "none", "class": "pipe-flow path-round" }).attr("stroke", props.PipeFlowColor || "#119bfa")
                .attr("d", "M 0 " + startY + " L " + endX + ' ' + startY).attr("stroke-width", pipeSize.FlowWidth)
                .attr("stroke-dasharray", pipeSize.DashWidth).attr("d", "M 0 " + startY + " L " + endX + ' ' + startY);
            var pipeAnimate = pipeFlow.append("animate").attr({ "attributeName": "stroke-dashoffset", "repeatCount": "indefinite", "fill": "freeze" })
            if (props.PipeDirection == 0) pipeAnimate.attr({ "from": "1000", "to": "0", "dur": "20s" })//正向
            else pipeAnimate.attr({ "from": "0", "to": "1000", "dur": "20s" })
            if (props.AuxData) {
                pipeBack.attr("d", props.AuxData)
                pipeFlow.attr("d", props.AuxData)
            }
        } else if (props.PipeLineType == 2) {//轨迹
            /**<circle cx="0" cy="0" r="30" fill="#0a7ae2"><animateMotion path="M 300 190 L 485.5 45.5 L 964 157" dur="20s" repeatCount="indefinite"></animateMotion></circle> */
            var pipeCircle = pipeLines.append("circle").attr({ "cx": "0", "cy": "0", "r": pipeSize.SolidWidth * 0.6, "fill": props.PipeFlowColor || "#119bfa" })
            var pipeCircleAnimate = pipeCircle.append("animateMotion").attr({ "repeatCount": "indefinite", "dur": "20s" }).attr("path", pipeBack.attr('d'));
            if (props.AuxData) {
                pipeBack.attr("d", props.AuxData)
                pipeCircleAnimate.attr("path", props.AuxData)
            }
        } else {
            if (props.AuxData) {
                pipeBack.attr("d", props.AuxData)
            }
        }


        if (renderContext.editMode) {
            that._refreshLinePoints(component);
        }
        that._resetMouseEvent(component, renderContext)
    }, 10);
};

scada.scheme.EditablePipeRenderer.prototype.setSize = function (component, width, height) {
    var compId = '#comp' + component.id;
    $(compId).css({ "width": width + "px", "height": height + "px" });
    $(compId + ' svg').css({ "width": width + "px", "height": height + "px" });
};

scada.scheme.EditablePipeRenderer.prototype.updateData = function (component, renderContext) {
};


/********** Renderer Map **********/
// Add components to the renderer map
scada.scheme.rendererMap.set("Scada.Web.Plugins.SchSvgExtComp.BasePipe", new scada.scheme.BasePipeRenderer());
scada.scheme.rendererMap.set("Scada.Web.Plugins.SchSvgExtComp.EditablePipe", new scada.scheme.EditablePipeRenderer());