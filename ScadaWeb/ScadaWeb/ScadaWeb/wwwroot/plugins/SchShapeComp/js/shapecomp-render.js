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

/********** Static SVG Shape Renderer **********/
scada.scheme.addInfoTooltipToDiv = function (targetDiv, text) {

	if (targetDiv instanceof jQuery) {
		targetDiv = targetDiv[0];
	}

	if (!targetDiv) return;
	if (!targetDiv) return;

	// Create tooltip
	const tooltip = document.createElement('div');
	tooltip.style.position = 'absolute';
	tooltip.style.bottom = '45%';
	tooltip.style.left = '50%';
	tooltip.style.transform = 'translateX(-50%)';
	tooltip.style.padding = '10px';
	tooltip.style.backgroundColor = 'black';
	tooltip.style.color = 'white';
	tooltip.style.borderRadius = '5px';
	tooltip.style.zIndex = '10';
	tooltip.style.whiteSpace = 'nowrap';
	tooltip.style.marginBottom = '5px';

	tooltip.textContent = text;

	targetDiv.style.position = 'relative';
	targetDiv.appendChild(tooltip);
}


scada.scheme.SvgShapeRenderer = function () {
	scada.scheme.ComponentRenderer.call(this);
};

scada.scheme.SvgShapeRenderer.prototype = Object.create(
	scada.scheme.ComponentRenderer.prototype
);

scada.scheme.SvgShapeRenderer.constructor =
	scada.scheme.SvgShapeRenderer;


scada.scheme.SvgShapeRenderer.prototype.createSvgElement = function (shapeType, props) {
	var svgElement;
	var svgNamespace = "http://www.w3.org/2000/svg";

	switch (shapeType) {
		case "Polygon":
			svgElement = document.createElementNS(svgNamespace, "polygon");
			svgElement.setAttribute("points", "0,0 50,0 50,50 0,50");
			break;
		case "Triangle":
			svgElement = document.createElementNS(svgNamespace, "polygon");
			svgElement.setAttribute("points", "0,0 50,50 100,0");
			break;
		case "Rectangle":
			svgElement = document.createElementNS(svgNamespace, "rect");
			svgElement.setAttribute("width", "100");
			svgElement.setAttribute("height", "100");
			break;
		case "Circle":
			svgElement = document.createElementNS(svgNamespace, "circle");
			svgElement.setAttribute("cx", "50");
			svgElement.setAttribute("cy", "50");
			svgElement.setAttribute("r", "50");
			break;
		case "Line":
			svgElement = document.createElementNS(svgNamespace, "line");
			svgElement.setAttribute("x1", "0");
			svgElement.setAttribute("y1", "0");
			svgElement.setAttribute("x2", "100");
			svgElement.setAttribute("y2", "100");
			break;
		case "Polyline":
			svgElement = document.createElementNS(svgNamespace, "polyline");
			svgElement.setAttribute("points", "20,20 40,25 60,40 80,120 120,140 200,180");
			break;
		default:
			console.warn("Unrecognized shape type: " + shapeType);
			return null;
	}

	// Set SVG attributes for color and stroke width
	if (["Polygon", "Triangle", "Rectangle", "Circle", "Polyline"].includes(shapeType)) {
		svgElement.setAttribute('fill', props.backColor || 'none');
	}
	svgElement.setAttribute('stroke', props.borderColor || 'black');
	svgElement.setAttribute('stroke-width', props.borderWidth || '1');

	return svgElement;
};


scada.scheme.SvgShapeRenderer.prototype.createDom = function (
	component,
	renderContext,
) {
	var props = component.props;
	var shapeType = props.shapeType;

	var divComp = $("<div id='comp" + component.id + "'></div>");
	this.prepareComponent(divComp, component, false, true);


	var svgElement = this.createSvgElement(shapeType, props);

	var svgNamespace = "http://www.w3.org/2000/svg";
	var svgContainer = document.createElementNS(svgNamespace, "svg");
	svgContainer.appendChild(svgElement);
	svgContainer.setAttribute('viewBox', '0 0 ' + props.width + ' ' + props.height);
	svgContainer.style.width = "100%";
	svgContainer.style.height = "100%";

	var divSvgComp = $("<div class='svgcomp'> </div>")
	divSvgComp.append(svgContainer);
	divComp.css({ "overflow": "hidden" });

	divComp.append(divSvgComp);
	if (props.rotation && props.rotation > 0) {
		divComp.css({
			"transform": "rotate(" + props.rotation + "deg)",
		})
	}

	component.dom = divComp;
};


scada.scheme.SvgShapeRenderer.prototype.updateData = function (
	component,
	renderContext,
) {
	var props = component.props;

	if (props.inCnlNum > 0) {
		var divComp = component.dom;
		var cnlDataExt = renderContext.getCnlDataExt(props.inCnlNum);

		// choose and set colors of the component
		var statusColor = cnlDataExt.color;
		var isHovered = divComp.is(":hover");

		var backColor = this.chooseColor(
			isHovered,
			props.backColor,
			props.backColorOnHover,
		);
		var borderColor = this.chooseColor(
			isHovered,
			props.borderColor,
			props.borderColorOnHover,
		);


		var svgElement = divComp.find("svg > *");
		svgElement.attr("fill", backColor);
		svgElement.attr("stroke", borderColor);

		divComp.find(".svgcomp").css({
			"width": props.width + "px",
			"height": props.height + "px",
		})
		if (props.rotation && props.rotation > 0) {
			divComp.css({
				"transform": "rotate(" + props.rotation + "deg)",
			})
		}
		this.setBackColor(divComp, backColor, true, statusColor);
		this.setBorderColor(divComp, borderColor, true, statusColor);

		if (props.conditions && cnlDataExt.d.stat > 0) {
			var cnlVal = cnlDataExt.d.val;

			for (var cond of props.conditions) {
				if (scada.scheme.calc.conditionSatisfied(cond, cnlVal)) {
					// Set CSS properties based on Condition
					if (cond.color) {
						divComp.css("color", cond.color);
					}
					if (cond.backgroundColor) {
						divComp.css("background-color", cond.backgroundColor);
					}
					if (cond.textContent) {

						scada.scheme.addInfoTooltipToDiv(divComp[0], cond.textContent);
					}
					if (cond.rotation) {
						divComp.css(
							"transform", "rotate(" + props.rotation + "deg)",
						)
					}

					divComp.css("visibility", cond.isVisible ? "visible" : "hidden");
					divComp.css("width", cond.width);
					divComp.css("height", cond.height);

					// Handle Blinking
					if (cond.blinking == 1) {
						divComp.addClass("slow-blink");
					} else if (cond.blinking == 2) {
						divComp.addClass("fast-blink");
					} else {
						divComp.removeClass("slow-blink fast-blink");
					}

					break;
				}
			}
		}
	}
};
/******* Polygon shape */

scada.scheme.PolygonRenderer = function () {
	scada.scheme.ComponentRenderer.call(this);
};

scada.scheme.PolygonRenderer.prototype = Object.create(
	scada.scheme.ComponentRenderer.prototype,
);
scada.scheme.PolygonRenderer.constructor =
	scada.scheme.PolygonRenderer;

scada.scheme.PolygonRenderer.prototype.generatePolygonPath = function (
	numPoints,
) {

	// Check that numPoints is a valid value
	var validPoints = [3, 4, 5, 6, 8, 10];
	if (!validPoints.includes(numPoints)) {
		return "";
	}

	// Generate the points of the polygon
	var path = "";
	for (var i = 0; i < numPoints; i++) {
		var angle = (2 * Math.PI * i) / numPoints;
		var x = 50 + 50 * Math.cos(angle);
		var y = 50 + 50 * Math.sin(angle);
		path += x + "% " + y + "%, ";
	}

	// Remove trailing comma and space
	path = path.slice(0, -2);

	return "polygon(" + path + ")";
};


scada.scheme.PolygonRenderer.prototype.createDom = function (
	component,
	renderContext,
) {
	var props = component.props;

	var divComp = $("<div id='comp" + component.id + "'></div>");
	this.prepareComponent(divComp, component);


	divComp.css({
		width: "200px",
		height: "200px",
		background: props.backColor,
		"clip-path": this.generatePolygonPath(props.numberOfSides),
		"border-width": props.borderWidth + "px",
		"border-color": props.borderColor,
	});
	if (props.rotation && props.rotation > 0) {
		divComp.css({
			"transform": "rotate(" + props.rotation + "deg)",
		})
	}

	component.dom = divComp;
};

scada.scheme.PolygonRenderer.prototype.updateData = function (
	component,
	renderContext,
) {
	var props = component.props;

	if (props.inCnlNum > 0) {
		var divComp = component.dom;
		var cnlDataExt = renderContext.getCnlDataExt(props.inCnlNum);

		// choose and set colors of the component
		var statusColor = cnlDataExt.color;
		var isHovered = divComp.is(":hover");

		var backColor = this.chooseColor(
			isHovered,
			props.backColor,
			props.backColorOnHover,
		);
		var borderColor = this.chooseColor(
			isHovered,
			props.borderColor,
			props.borderColorOnHover,
		);

		if (props.rotation && props.rotation > 0) {
			divComp.css({
				"transform": "rotate(" + props.rotation + "deg)",
			})
		}

		this.setBackColor(divComp, backColor, true, statusColor);
		this.setBorderColor(divComp, borderColor, true, statusColor);

		// Conditions
		if (props.conditions && cnlDataExt.d.stat > 0) {
			var cnlVal = cnlDataExt.d.val;


			for (var cond of props.conditions) {
				if (scada.scheme.calc.conditionSatisfied(cond, cnlVal)) {
					// Set CSS properties based on Condition
					if (cond.color) {
						divComp.css("color", cond.color);
					}
					if (cond.backgroundColor) {
						divComp.css("background-color", cond.backgroundColor);
					}
					if (cond.textContent) {
						scada.scheme.addInfoTooltipToDiv(divComp[0], cond.textContent);
					}
					if (cond.rotation) {
						divComp.css(
							"transform", "rotate(" + props.rotation + "deg)",
						)
					}
					divComp.css("visibility", cond.isVisible ? "visible" : "hidden");
					divComp.css("width", cond.width);
					divComp.css("height", cond.height);

					// Handle Blinking
					if (cond.blinking == 1) {
						divComp.addClass("slow-blink");
					} else if (cond.blinking == 2) {
						divComp.addClass("fast-blink");
					} else {
						divComp.removeClass("slow-blink fast-blink");
					}

					break;
				}
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



scada.scheme.CustomSVGRenderer.prototype.generateSVG = function (
	codeSVG,
	strokeColor,
	fillColor,
	strokeWidth,
	viewBoxX,
	viewBoxY,
	viewBoxWidth,
	viewBoxHeight,
	width,
	height,
) {
	if (typeof codeSVG !== 'string' || codeSVG.trim().length === 0) {
		console.error("Invalid SVG code");
		return;
	}

	var parser = new DOMParser();
	var doc = parser.parseFromString(codeSVG, "image/svg+xml");

	if (doc.querySelector('parsererror')) {
		console.error("Error parsing SVG");
		return;
	}

	var svg = doc.querySelector("svg");
	if (!svg) {
		console.error("No svg element found in the SVG code");
		return;
	}

	svg.setAttribute("xmlns", "http://www.w3.org/2000/svg");
	svg.setAttribute("viewBox", `${viewBoxX} ${viewBoxY} ${viewBoxWidth} ${viewBoxHeight}`);
	svg.setAttribute("width", width + "%");
	svg.setAttribute("height", height + "%");
	svg.setAttribute("style", "cursor:move");

	var elements = svg.querySelectorAll("*");
	elements.forEach(el => {
		el.setAttribute("stroke", strokeColor);
		el.setAttribute("fill", fillColor);
		el.setAttribute("stroke-width", strokeWidth);

	});

	const serializer = new XMLSerializer();
	codeSVG = serializer.serializeToString(svg);

	return codeSVG;
};

scada.scheme.CustomSVGRenderer.prototype.createDom = function (
	component,
	renderContext,
) {
	var props = component.props;

	var divComp = $("<div id='comp" + component.id + "'></div>");
	this.prepareComponent(divComp, component, false, true);

	var svg = this.generateSVG(
		props.svgCode,
		props.borderColor,
		props.backColor,
		props.borderWidth,
		props.viewBoxX,
		props.viewBoxY,
		props.viewBoxWidth,
		props.viewBoxHeight,
		props.width,
		props.height);

	divComp.append(svg);
	if (props.rotation && props.rotation > 0) {
		divComp.css({
			"transform": "rotate(" + props.rotation + "deg)",
		})
	}
	component.dom = divComp;
};

scada.scheme.CustomSVGRenderer.prototype.updateData = function (
	component,
	renderContext,
) {
	var props = component.props;

	if (props.inCnlNum > 0) {
		var divComp = component.dom;
		var cnlDataExt = renderContext.getCnlDataExt(props.inCnlNum);

		// choose and set colors of the component
		var statusColor = cnlDataExt.Color;
		var isHovered = divComp.is(":hover");

		var backColor = this.chooseColor(
			isHovered,
			props.backColor,
			props.backColorOnHover,
		);
		var borderColor = this.chooseColor(
			isHovered,
			props.borderColor,
			props.borderColorOnHover,
		);
		if (props.rotation && props.rotation > 0) {
			divComp.css({
				"transform": "rotate(" + props.rotation + "deg)",
			})
		}
		this.setBackColor(divComp, backColor, true, statusColor);
		this.setBorderColor(divComp, borderColor, true, statusColor);

		divComp.removeClass("no-blink slow-blink fast-blink");
		if (props.conditions && cnlDataExt.d.stat > 0) {
			var cnlVal = cnlDataExt.d.val;

			for (var cond of props.conditions) {
				if (scada.scheme.calc.conditionSatisfied(cond, cnlVal)) {
					// Set CSS properties based on Condition
					if (cond.color) {
						divComp.css("color", cond.color);
					}
					if (cond.backgroundColor) {
						divComp.css("background-color", cond.backgroundColor);
					}
					if (cond.textContent) {
						scada.scheme.addInfoTooltipToDiv(divComp[0], cond.textContent);
					}
					if (cond.rotation) {
						divComp.css("transform", "rotate(" + cond.rotation + "deg)");
					}
					divComp.css("visibility", cond.isVisible ? "visible" : "hidden");
					if (cond.width) {
						divComp.css("width", cond.width);
					}
					if (cond.height) {
						divComp.css("height", cond.height);
					}

					// Handle Blinking
					switch (cond.blinking) {
						case 0:
							divComp.removeClass("slow-blink fast-blink");
							break;
						case 1:
							divComp.addClass("slow-blink");
							break;
						case 2:
							divComp.addClass("fast-blink");
							break;
					}

					break;
				}
			}

		}
	}
};

/**** BARGRAPH */
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


/********** Renderer Map **********/

// Add components to the renderer map
scada.scheme.rendererMap.set("Scada.Web.Plugins.PlgSchShapeComp.Code.SvgShape", new scada.scheme.SvgShapeRenderer);
scada.scheme.rendererMap.set("Scada.Web.Plugins.PlgSchShapeComp.Code.CustomSVG", new scada.scheme.CustomSVGRenderer);
scada.scheme.rendererMap.set("Scada.Web.Plugins.PlgSchShapeComp.Code.Polygon", new scada.scheme.PolygonRenderer);
scada.scheme.rendererMap.set("Scada.Web.Plugins.PlgSchShapeComp.Code.BarGraph", new scada.scheme.BarGraphRenderer);
