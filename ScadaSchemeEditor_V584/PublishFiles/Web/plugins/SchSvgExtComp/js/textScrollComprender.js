/********** Text scroll Renderer **********/

// Text scroll renderer type extends scada.scheme.ComponentRenderer
scada.scheme.TextScrollRenderer = function () {
	scada.scheme.ComponentRenderer.call(this);
};

scada.scheme.TextScrollRenderer.prototype = Object.create(scada.scheme.ComponentRenderer.prototype);
scada.scheme.TextScrollRenderer.constructor = scada.scheme.TextScrollRenderer;

scada.scheme.TextScrollRenderer.prototype.createDom = function (component, renderContext) {
	var props = component.props;
	var divComp = $("<div id='comp" + component.id + "'></div>");
	var compId = '#comp' + component.id;
	var swiperId = "swiper_" + component.id;
	this.prepareComponent(divComp, component);

	console.log(props);
	component.dom = divComp;
	var swiperContainer = $("<div></div>").attr("id", swiperId).css({ "width": props.Size.Width + "px", "height": props.Size.Height + "px", "color": props.ForeColor, "overflow": "hidden" })
	var swiperDiv = $("<div></div>").attr("class", "swiper-wrapper");
	if (props.Conditions && props.Conditions.length > 0) {
		for (var i = 0; i < props.Conditions.length; i++) {
			var swiperItemDiv = $("<div></div>").attr("class", "swiper-slide");

			swiperItemDiv.append($("<div>" + props.Conditions[i].LabelName + "<span id='swiperval_" + component.id + "_" + i + "'></span></div>"))

			swiperDiv.append(swiperItemDiv);
		}
	}

	swiperContainer.append(swiperDiv);
	this.setFont(swiperContainer, props.Font, false)
	this.setBackColor(swiperContainer, props.BackColor, true);
	setTimeout(function () {
		$(compId).append($('<div class="swiper"></div>').append(swiperContainer).html());
		if (renderContext && !renderContext.editMode)
			var swiper = new Swiper("#" + swiperId, { autoplay: { disableOnInteraction: false }, direction: "vertical" });
	}, 100);
};

scada.scheme.TextScrollRenderer.prototype.setSize = function (component, width, height) {
	scada.scheme.ComponentRenderer.prototype.setSize.call(this, component, width, height);

	this._initDom(component, null, component.props.Size.Width, component.props.Size.Height);
};
// 初始化元素
scada.scheme.TextScrollRenderer.prototype._initDom = function (component, renderContext, width, height) {
	var props = component.props;
	var compId = '#comp' + component.id;
	var swiperId = "#swiper_" + component.id;
	$(component.dom).find(swiperId).css({ "width": width + "px", "height": height + "px", "color": props.ForeColor, "overflow": "hidden" })
};

scada.scheme.TextScrollRenderer.prototype.updateData = function (component, renderContext) {
	var props = component.props;
	if (props.Conditions && props.Conditions.length > 0) {
		var divComp = component.dom;

		if (props.ToggleCnlNum && props.ToggleCnlNum > 0) {
			var cnlDataExt = renderContext.getCnlDataExt(props.ToggleCnlNum);
			if (cnlDataExt.Val == 0) {
				$(divComp).hide();//隐藏元素
				return;
			}
			else {
				$(divComp).show();
			}
		}

		for (var i = 0; i < props.Conditions.length; i++) {
			var inCnl = props.Conditions[i].InCnlNum;
			if (inCnl > 0) {
				var cnlDataExt = renderContext.getCnlDataExt(inCnl);
				if (cnlDataExt.Stat) {
					var cnlVal = cnlDataExt.Val;
					var selector = "#swiperval_" + component.id + "_" + i;
					if (props.Conditions[i].Conditions && props.Conditions[i].Conditions.length > 0) {
						for (var cond of props.Conditions[i].Conditions) {
							if (scada.scheme.calc.conditionSatisfied(cond, cnlVal)) {
								$(selector).html(cond.ShowName);
								break;
							}
						}
					} else {
						$(selector).html(cnlDataExt.TextWithUnit);
					}

				}

			}
		}
	} else {
		/*svgAnimate.remove();*/
	}
};

/********** Text reflect Renderer **********/

// Text reflect renderer type extends scada.scheme.ComponentRenderer
scada.scheme.TextReflectRenderer = function () {
	scada.scheme.ComponentRenderer.call(this);
};

scada.scheme.TextReflectRenderer.prototype = Object.create(scada.scheme.ComponentRenderer.prototype);
scada.scheme.TextReflectRenderer.constructor = scada.scheme.TextReflectRenderer;

scada.scheme.TextReflectRenderer.prototype.createDom = function (component, renderContext) {
	var props = component.props;
	var divComp = $("<div id='comp" + component.id + "'></div>");
	var compId = '#comp' + component.id;
	var reflectId = "reflect_" + component.id;
	var reflectDataId = "reflect_data_" + component.id;
	this.prepareComponent(divComp, component);

	component.dom = divComp;
	var reflectContainer = $("<div class='reflect-container'></div>").attr("id", reflectId).css({ "width": props.Size.Width + "px", "height": props.Size.Height + "px", "color": props.ForeColor, "overflow": "hidden" })
	if (props.Marquee == 1) {
		var direction = props.MarqueeDirection == 0 ? "right" : "left";
		var marqueeEl = $('<marquee  direction="' + direction + '"  behavior="scroll"  scrollamount="' + props.MarqueeSpeed + '"  scrolldelay="10"></marquee>')
		var reflectDiv = $("<div></div>").attr("id", reflectDataId);
		marqueeEl.append(reflectDiv);
		reflectContainer.append(marqueeEl);
	} else {
		var reflectDiv = $("<div></div>").attr("id", reflectDataId);
		reflectContainer.append(reflectDiv);
	}

	this.setFont(reflectContainer, props.Font, false)
	this.setBackColor(reflectContainer, props.BackColor, true);
	setTimeout(function () {
		$(compId).append($('<div class="swiper"></div>').append(reflectContainer).html());
	}, 100);
};

scada.scheme.TextReflectRenderer.prototype.setSize = function (component, width, height) {
	scada.scheme.ComponentRenderer.prototype.setSize.call(this, component, width, height);

	this._initDom(component, null, component.props.Size.Width, component.props.Size.Height);
};
// 初始化元素
scada.scheme.TextReflectRenderer.prototype._initDom = function (component, renderContext, width, height) {
	var props = component.props;
	var compId = '#comp' + component.id;
	var reflectId = "#reflect_" + component.id;
	$(component.dom).find(reflectId).css({ "width": width + "px", "height": height + "px", "color": props.ForeColor, "overflow": "hidden" })
};

scada.scheme.TextReflectRenderer.prototype.updateData = function (component, renderContext) {
	var props = component.props;
	var selector = "#reflect_data_" + component.id;
	if (props.InCnlNum > 0) {
		var inCnl = props.InCnlNum;
		var cnlDataExt = renderContext.getCnlDataExt(inCnl);
		if (cnlDataExt.Stat == 1) {
			var cnlVal = cnlDataExt.Val;
			for (var cond of props.Conditions) {
				if (scada.scheme.calc.conditionSatisfied(cond, cnlVal)) {
					$(selector).css("color", cond.ForeColor).html(cond.ShowName);
					break;
				}
			}
		} else {
			$(selector).html("");
		}
	} else {
		$(selector).html("");
	}
};

/********** Signal Light ***********/
scada.scheme.SignalLightRenderer = function () {
	scada.scheme.ComponentRenderer.call(this);
};

scada.scheme.SignalLightRenderer.prototype = Object.create(scada.scheme.ComponentRenderer.prototype);
scada.scheme.SignalLightRenderer.constructor = scada.scheme.SignalLightRenderer;

scada.scheme.SignalLightRenderer.prototype.createDom = function (component, renderContext) {
	var props = component.props;
	var divComp = $("<div id='comp" + component.id + "'></div>");
	var compId = '#comp' + component.id;

	this.prepareComponent(divComp, component);

	var svgPic = $(SignalLightUtil.signalLightSvg1);
	var svgSize = props.Size.Width > props.Size.Height ? props.Size.Height : props.Size.Width;
	svgPic.css({ "width": svgSize + "px", "height": svgSize + "px" });
	var svgPath = svgPic.find('circle');

	svgPath.attr({ "stroke": props.ForeColor, "fill": props.ForeColor })
	component.dom = divComp;
	setTimeout(function () {
		$(compId).append($("<div />").append(svgPic).html());
	}, 10);
};

scada.scheme.SignalLightRenderer.prototype.refreshImages = function (component, renderContext, imageNames) {
	var props = component.props;

	/*if (Array.isArray(imageNames) && imageNames.includes(props.ImageName)) {
		var divComp = component.dom;
		var image = renderContext.getImage(props.ImageName);
		this.setBackgroundImage(divComp, image, true);
	}*/
};

scada.scheme.SignalLightRenderer.prototype.setSize = function (component, width, height) {
	scada.scheme.ComponentRenderer.prototype.setSize.call(this, component, width, height);

	var props = component.props;
	var divComp = component.dom;

	var svgSize = props.Size.Width > props.Size.Height ? props.Size.Height : props.Size.Width;
	divComp.find('svg').css({ "width": svgSize + "px", "height": svgSize + "px" });
};

scada.scheme.SignalLightRenderer.prototype.updateData = function (component, renderContext) {
	var props = component.props;
	if (props.InCnlNum > 0) {
		var divComp = component.dom;
		var cnlDataExt = renderContext.getCnlDataExt(props.InCnlNum);
		var cnlVal = cnlDataExt.Val;

		for (var cond of props.Conditions) {
			if (scada.scheme.calc.conditionSatisfied(cond, cnlVal)) {
				if (props.CurCond != cond) {
					var compId = '#comp' + component.id;
					var svgPic = $(SignalLightUtil.signalLightSvg1);
					if (cond.FlashingType == 1) {
						var value = [cond.ForeColor, cond.FlashingColor, cond.ForeColor, cond.ForeColor];
						var blinkAnimate = $('<animate attributeType="XML" attributeName="fill" values="' + value.join(';') + '" dur="0.8s" repeatCount="indefinite" />');
						blinkAnimate.appendTo(svgPic.find('circle')[0]);
					}
					var svgSize = props.Size.Width > props.Size.Height ? props.Size.Height : props.Size.Width;
					svgPic.css({ "width": svgSize + "px", "height": svgSize + "px" });
					svgPic.find('circle').attr("fill", cond.ForeColor);
					$(compId).empty();
					$(compId).append($("<div />").append(svgPic).html());
				}
				props.CurCond = cond;
				break;
			}
		}
	}
};

/********** Renderer Map **********/
scada.scheme.rendererMap.set("Scada.Web.Plugins.SchSvgExtComp.TextScroll", new scada.scheme.TextScrollRenderer());
scada.scheme.rendererMap.set("Scada.Web.Plugins.SchSvgExtComp.TextReflect", new scada.scheme.TextReflectRenderer());
scada.scheme.rendererMap.set("Scada.Web.Plugins.SchSvgExtComp.SignalLight", new scada.scheme.SignalLightRenderer());