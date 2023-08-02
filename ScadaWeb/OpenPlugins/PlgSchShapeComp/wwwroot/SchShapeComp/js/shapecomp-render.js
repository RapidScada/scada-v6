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
	svgElement.setAttribute('fill', props.backColor);
	svgElement.setAttribute('stroke', props.borderColor); props.borderColor
	svgElement.setAttribute('stroke-width', props.borderWidth);

	return svgElement;
};

scada.scheme.SvgShapeRenderer.prototype.createDom = function (
	component,
	renderContext,
) {
	var props = component.props;
	var shapeType = props.shapeType;

	var divComp = $("<div id='comp" + component.id + "'></div>");
	//this.prepareComponent(divComp, component);
	this.prepareComponent(divComp, component, false, true);

	var svgElement = this.createSvgElement(shapeType, props);

	var svgNamespace = "http://www.w3.org/2000/svg";
	var svgContainer = document.createElementNS(svgNamespace, "svg");
	svgContainer.appendChild(svgElement);
	svgContainer.style.width = "100%";
	svgContainer.style.height = "100%";

	divComp.append(svgContainer);
	component.dom = divComp;
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

	var polygonPath = this.generatePolygonPath(props.numberOfSides);

	divComp.css({
		width: "200px",
		height: "200px",
		background: props.backColor,
		"clip-path": this.generatePolygonPath(props.numberOfSides),
		"border-width": props.borderWidth + "px",
		"border-color": props.borderColor,
		"border-radius": props.boundedCorners ? props.bornerRadius + "%" : "0%",
	});

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

		this.setBackColor(divComp, backColor, true, statusColor);
		this.setBorderColor(divComp, borderColor, true, statusColor);
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

		this.setBackColor(divComp, backColor, true, statusColor);
		this.setBorderColor(divComp, borderColor, true, statusColor);
	}
};

/********** Renderer Map **********/

// Add components to the renderer map
scada.scheme.rendererMap.set("Scada.Web.Plugins.PlgSchShapeComp.Code.SvgShape", new scada.scheme.SvgShapeRenderer);
scada.scheme.rendererMap.set("Scada.Web.Plugins.PlgSchShapeComp.Code.CustomSVG", new scada.scheme.CustomSVGRenderer);
scada.scheme.rendererMap.set("Scada.Web.Plugins.PlgSchShapeComp.Code.Polygon", new scada.scheme.PolygonRenderer);
