/********** ConveyerBelt Renderer **********/

// ConveyerBelt renderer type extends scada.scheme.ComponentRenderer
scada.scheme.ConveyerBeltRenderer = function () {
    scada.scheme.ComponentRenderer.call(this);
};

scada.scheme.ConveyerBeltRenderer.prototype = Object.create(scada.scheme.ComponentRenderer.prototype);
scada.scheme.ConveyerBeltRenderer.constructor = scada.scheme.ConveyerBeltRenderer;
scada.scheme.ConveyerBeltRenderer.prototype.clickTimeout = {
    _timeout: null,
    set: function (fn) {
        var that = this
        that.clear()
        that._timeout = setTimeout(fn, 300)
    },
    clear: function () {
        var that = this
        if (that._timeout) {
            clearTimeout(that._timeout)
        }
    }
}
scada.scheme.ConveyerBeltRenderer.prototype.bindDbClick = function (jqObj, component, renderContext) {
    var Actions = scada.scheme.Actions;
    var props = component.props;
    var action = props.action;
    var actionIsBound =
        action === Actions.DRAW_DIAGRAM && props.inCnlNum > 0 ||
        (action === Actions.SEND_COMMAND || action === Actions.SEND_COMMAND_NOW) &&
        props.ctrlCnlNum > 0 && renderContext.controlRight;

    var that = this;
    if (actionIsBound) {
        jqObj.addClass("action");
        if (!renderContext.editMode) {
            var viewHub = renderContext.schemeEnv.viewHub;
            jqObj.on("dblclick", function () {
                that.clickTimeout.clear();
                switch (props.action) {
                    case Actions.DRAW_DIAGRAM:
                        viewHub.features.chart.show(props.inCnlNum, new Date().toISOString().slice(0, 10));
                        break;

                    case Actions.SEND_COMMAND:
                        viewHub.features.command.show(props.ctrlCnlNum);
                        break;

                    case Actions.SEND_COMMAND_NOW:
                        if (renderContext.schemeEnv) {
                            renderContext.schemeEnv.sendCommand(props.ctrlCnlNum, component.cmdVal,
                                renderContext.viewID, component.id);
                        } else {
                            console.warn("Scheme environment object is undefined");
                        }
                        break;
                }
            });
        }
    }
};
scada.scheme.ConveyerBeltRenderer.prototype.createDom = function (component, renderContext) {
    var props = component.props;
    var divComp = $("<div id='comp" + component.id + "'></div>");
    var compId = '#comp' + component.id;
    var svgId = 'svg_' + component.id;
    this.prepareComponent(divComp, component);
    //command
    component.cmdVal = props.actionValue;
    //this.bindAction(divComp, component, renderContext);
    if (props.conveyerName) {
        setTimeout(function () {
            var conveyerContent = props.conveyerContent.replaceAll('.cls', '#' + svgId + ' .cls')
            $(compId).append($("<div />").append(conveyerContent).html());
            var svgPic = $(compId + " svg");
            svgPic.attr('id', svgId)
            svgPic.attr({ "width": "100%", "height": "100%" });
            svgPic.find('.cls-100').css('fill', 'Gray');
        }, 10);
    } else {
        $(compId).append($("<div />").append("请选择传送带").html());
    }
    component.dom = divComp;

    var that = this;
    var spanComp = component.dom.first();
    this.bindDbClick(spanComp, component, renderContext);
    if (props.url || props.viewID > 0) {
        spanComp.addClass("action");

        if (!renderContext.editMode) {
            spanComp.on("click", function () {
                that.clickTimeout.set(function () {
                    let url = "";
                    let viewHub = renderContext.schemeEnv.viewHub;

                    if (props.viewID > 0) {
                        let popViewId = props.viewID + renderContext.viewIdOffset;
                        console.log('ConveyerBelt viewid && offset:', props.viewID, renderContext.viewIdOffset)
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
                })
            });
        }
    }
};

scada.scheme.ConveyerBeltRenderer.prototype.refreshImages = function (component, renderContext, imageNames) {
    var props = component.props;

};

scada.scheme.ConveyerBeltRenderer.prototype.setSize = function (component, width, height) {
    scada.scheme.ComponentRenderer.prototype.setSize.call(this, component, width, height);
    var ImageStretches = scada.scheme.ImageStretches;
    var props = component.props;
    var divComp = component.dom;

    //divComp.find('svg').attr({ "width": props.size.width + "px", "height": props.size.height + "px" });
    divComp.find('svg').attr({ "width": "100%", "height": "100%" });
};

scada.scheme.ConveyerBeltRenderer.prototype.updateData = function (component, renderContext) {
    var props = component.props;
    var divComp = component.dom;
    var svgTarget = divComp.find('svg .cls-100');
    if (props.inCnlNum > 0) {
        var cnlDataExt = renderContext.getCnlDataExt(props.inCnlNum);
        for (var cond of props.conditions) {
            if (scada.scheme.calc.conditionSatisfied(cond, cnlDataExt.d.val)) {
                if (props.conveyerColor == 'Status') {
                    svgTarget.css('fill', cnlDataExt.df.colors[0])
                } else {
                    svgTarget.css('fill', cond.conveyerColor)
                }
                break;
            }
        }
    } else {
        svgTarget.css('fill', "Gray")
    }
};

/********** Renderer Map **********/

// Add components to the renderer map
scada.scheme.rendererMap.set("Scada.Web.Plugins.SchSvgExtComp.ConveyerBelt", new scada.scheme.ConveyerBeltRenderer());