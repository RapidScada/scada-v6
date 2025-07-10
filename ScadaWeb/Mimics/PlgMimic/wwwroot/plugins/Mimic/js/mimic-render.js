// Contains classes: Renderer, MimicRenderer, ComponentRenderer, RegularComponentRenderer,
//     TextRenderer, PictureRenderer, PanelRenderer, RenderContext, DataProvider, RendererSet, UnitedRenderer
// Depends on jquery, scada-common.js, mimic-common.js

// Represents a renderer of a mimic or component.
rs.mimic.Renderer = class {
    // Sets the background image of the specified jQuery object.
    _setBackgroundImage(jqObj, image) {
        jqObj.css("background-image", this._imageToDataUrlCss(image));
    }

    // Sets the border of the specified jQuery object.
    _setBorder(jqObj, border) {
        if (border.width > 0) {
            jqObj.css({
                "border-width": border.width,
                "border-style": "solid",
                "border-color": border.color
            });
        } else {
            jqObj.css("border", "none");
        }
    }

    // Sets the corner radius of the specified jQuery object.
    _setCornerRadius(jqObj, cornerRadius) {
        jqObj.css({
            "border-top-left-radius": cornerRadius.topLeft,
            "border-top-right-radius": cornerRadius.topRight,
            "border-bottom-right-radius": cornerRadius.bottomRight,
            "border-bottom-left-radius": cornerRadius.bottomLeft
        });
    }

    // Sets the font of the specified jQuery object.
    _setFont(jqObj, font) {
        if (font.inherit) {
            jqObj.css({
                "font-family": "",
                "font-size": "",
                "font-weight": "",
                "font-style": "",
                "text-decoration": "" // not inherited
            });
        } else {
            jqObj.css({
                //"font-family": font.name,
                "font-size": font.size,
                "font-weight": font.bold ? "bold" : "normal",
                "font-style": font.italic ? "italic" : "normal",
                "text-decoration": font.underline ? "underline" : "none"
            });
        }
    }

    // Sets the left and top of the specified jQuery object.
    _setLocation(jqObj, location) {
        jqObj.css({
            "left": location.x + "px",
            "top": location.y + "px"
        });
    }

    // Sets the padding of the specified jQuery object.
    _setPadding(jqObj, padding) {
        jqObj.css({
            "padding-top": padding.top,
            "padding-right": padding.right,
            "padding-bottom": padding.bottom,
            "padding-left": padding.left
        });
    }

    // Sets the width and height of the specified jQuery object.
    _setSize(jqObj, size) {
        jqObj.css({
            "width": size.width + "px",
            "height": size.height + "px"
        });
    }

    // Returns a css property value for the image data URI.
    _imageToDataUrlCss(image) {
        return image ? "url('" + image.dataUrl + "')" : "";
    }

    // Creates a component DOM according to the component model. Returns a jQuery object or null.
    createDom(component, renderContext) {
        return null;
    }

    // Updates the existing component DOM according to the component model. Returns a jQuery object or null.
    updateDom(component, renderContext) {
        return null;
    }

    // Sets the size of the component DOM.
    setSize(component, width, height) {
        if (component.dom) {
            this._setSize(component.dom, {
                width: width,
                height: height
            });
        }
    }

    // Gets the component size from its DOM.
    getSize(component) {
        if (component.dom) {
            return {
                width: parseInt(component.dom.outerWidth()),
                height: parseInt(component.dom.outerHeight())
            };
        } else {
            return {
                width: 0,
                height: 0
            };
        }
    }

    // Appends the child DOM into the parent DOM.
    static appendChild(parent, child) {
        if (parent.dom && child.dom) {
            parent.dom.append(child.dom);
        }
    }

    // Removes the component DOM from the mimic keeping data associated with the removed elements.
    static detach(component) {
        component.dom?.detach();
    }

    // Removes the component DOM from the mimic.
    static remove(component) {
        component.dom?.remove();
    }

    // Arranges the child components according to their order.
    static arrangeChildren(parent) {
        if (parent.children && parent.dom) {
            for (let component of parent.children) {
                component.dom?.detach();
            }

            for (let component of parent.children) {
                if (component.dom) {
                    parent.dom.append(component.dom);
                }
            }
        }
    }
};

// Represents a mimic renderer.
rs.mimic.MimicRenderer = class MimicRenderer extends rs.mimic.Renderer {
    static _GRID_COLOR = "#dee2e6"; // gray-300

    // Creates a grid canvas and draws grid cells.
    static _createGrid(gridSize, mimicSize) {
        // create canvas
        let canvasElem = $("<canvas class='grid'></canvas>");
        let canvas = canvasElem[0];
        let width = canvas.width = mimicSize.width;
        let height = canvas.height = mimicSize.height;

        // prepare drawing context
        let context = canvas.getContext("2d");
        context.lineWidth = 1;
        context.strokeStyle = MimicRenderer._GRID_COLOR;

        // draw grids with small and large cells
        MimicRenderer._drawGrid(context, width, height, gridSize, [1, 1]);
        MimicRenderer._drawGrid(context, width, height, gridSize * 10);
        return canvasElem;
    }

    // Draws grid cells.
    static _drawGrid(context, width, height, gridStep, dashSegments = []) {
        const adj = 0.5; // adjustment for sharpness of lines

        // draw vertical lines
        for (let x = gridStep; x < width; x += gridStep) {
            context.beginPath();
            context.setLineDash(dashSegments);
            context.moveTo(x + adj, 0);
            context.lineTo(x + adj, height);
            context.stroke();
        }

        // draw horizontal lines
        for (let y = gridStep; y < height; y += gridStep) {
            context.beginPath();
            context.setLineDash(dashSegments);
            context.moveTo(0, y + adj);
            context.lineTo(width, y + adj);
            context.stroke();
        }
    }

    // Sets the CSS properties of the mimic element.
    _setProps(mimicElem, mimic, renderContext) {
        let props = mimic.document;
        this._setBackgroundImage(mimicElem, renderContext.getImage(props.backgroundImage));
        this._setFont(mimicElem, props.font);
        this._setSize(mimicElem, props.size);

        mimicElem
            .attr("title", props.tooltip)
            .css({
                "background-color": props.backColor,
                "background-repeat": "no-repeat",
                "background-size": props.size.width + "px " + props.size.height + "px",
                "color": props.foreColor
            });
    }

    // Creates a mimic DOM according to the mimic model.
    createDom(mimic, renderContext) {
        let mimicElem = $("<div class='mimic'></div>");

        if (renderContext.editMode && renderContext.editorOptions &&
            renderContext.editorOptions.showGrid && renderContext.editorOptions.gridStep > 1) {
            mimicElem.append(MimicRenderer._createGrid(renderContext.editorOptions.gridStep, mimic.document.size));
        }

        this._setProps(mimicElem, mimic, renderContext);
        mimic.dom = mimicElem;
        return mimicElem;
    }

    // Updates the existing mimic DOM according to the mimic model.
    updateDom(mimic, renderContext) {
        let mimicElem = mimic.dom;

        if (mimicElem) {
            this._setProps(mimicElem, mimic, renderContext);
        }

        return mimicElem;
    }
};

// Represents a component renderer.
rs.mimic.ComponentRenderer = class extends rs.mimic.Renderer {
    get canUpdateDom() {
        return false;
    }

    // Sets the CSS classes of the component element.
    _setClasses(componentElem) {
        componentElem.removeClass(); // clear classes
        componentElem.addClass("comp");
    }

    // Sets the CSS properties of the component element.
    _setProps(componentElem, component, renderContext) {
        let props = component.properties;
        this._setLocation(componentElem, props.location);
        this._setSize(componentElem, props.size);

        componentElem.css({
            "background-color": props.backColor,
            "color": props.foreColor
        });

        if (renderContext.editMode) {
            if (!renderContext.faceplateMode && component.isContainer) {
                componentElem.addClass("container")
            }

            if (component.isSelected) {
                componentElem.addClass("selected")
            }
        }
    }

    // Creates a component DOM according to the component model.
    createDom(component, renderContext) {
        let componentElem = $("<div></div>")
            .attr("id", "comp" + renderContext.idPrefix + component.id)
            .attr("data-id", component.id);
        this._setClasses(componentElem);
        this._setProps(componentElem, component, renderContext);
        component.dom = componentElem;
        return componentElem;
    }

    // Updates the existing component DOM according to the component model.
    updateDom(component, renderContext) {
        let componentElem = component.dom;

        if (componentElem) {
            this._setClasses(componentElem);
            this._setProps(componentElem, component, renderContext);
        }

        return componentElem;
    }

    // Updates the component view according to the current channel data.
    updateData(component, renderContext) {
    }

    // Sets the location of the component DOM without changing the component model.
    setLocation(component, x, y) {
        if (component.dom) {
            this._setLocation(component.dom, {
                x: x,
                y: y
            });
        }
    }

    // Gets the component location from its DOM.
    getLocation(component) {
        if (component.dom) {
            let position = component.dom.position();
            return {
                x: parseInt(position.left),
                y: parseInt(position.top)
            };
        } else {
            return {
                x: 0,
                y: 0
            };
        }
    }

    // Updates the location of the component DOM according to the component model.
    updateLocation(component) {
        if (component.dom) {
            this._setLocation(component.dom, component.properties.location);
        }
    }

    // Visually selects or deselects the component.
    updateSelected(component) {
        if (component.dom) {
            component.dom.toggleClass("selected", component.isSelected);
        }
    }

    // Gets a value indicating whether the component can be resized by a user.
    allowResizing(component) {
        return true;
    }
};

// Represents a renderer for regular non-faceplate components.
rs.mimic.RegularComponentRenderer = class extends rs.mimic.ComponentRenderer {
    _setProps(componentElem, component, renderContext) {
        super._setProps(componentElem, component, renderContext);
        let props = component.properties;
        this._setBorder(componentElem, props.border);
        this._setCornerRadius(componentElem, props.cornerRadius);
        componentElem.attr("title", props.tooltip);

        if (props.cssClass) {
            componentElem.addClass(props.cssClass);
        }
    }

    _setPropertyValue(obj, binding, curData) {

    }

    updateData(component, renderContext) {
        // update properties
        if (Array.isArray(component.bindings)) {
            let dataProvider = renderContext.getDataProvider();

            for (let binding of component.bindings) {
                let cnlNum = binding.cnlNum;

                if (binding.propertyName && cnlNum > 0) {
                    let cnlProps = dataProvider.getCnlProps(cnlNum);
                    let curData = dataProvider.getCurData(cnlNum, cnlProps.joinLen);
                    let prevData = dataProvider.getPrevData(cnlNum, cnlProps.joinLen);

                    if (!(curData.d.val === prevData.d.val && curData.d.stat === prevData.d.stat) ||
                        !dataProvider.prevCnlDataMap) {
                        this._setPropertyValue(component.properties, binding, curData);
                    }
                }
            }
        }

        // update DOM
        if (component.dom) {
            if (this.canUpdateDom) {
                this.updateDom(component, renderContext);
            } else {
                let oldDom = component.dom;
                this.createDom(component, renderContext);
                oldDom.replaceWith(component.dom);
            }
        }
    }
};

// Represents a text component renderer.
rs.mimic.TextRenderer = class extends rs.mimic.RegularComponentRenderer {
    _setClasses(componentElem) {
        super._setClasses(componentElem);
        componentElem.addClass("text");
    }

    _setProps(componentElem, component, renderContext) {
        super._setProps(componentElem, component, renderContext);
        let props = component.properties;
        this._setFont(componentElem, props.font);
        this._setPadding(componentElem, props.padding);
        componentElem.text(props.text);
    }
};

// Represents a picture component renderer.
rs.mimic.PictureRenderer = class extends rs.mimic.RegularComponentRenderer {
    _setClasses(componentElem) {
        super._setClasses(componentElem);
        componentElem.addClass("picture");
    }

    _setProps(componentElem, component, renderContext) {
        super._setProps(componentElem, component, renderContext);
        let props = component.properties;
        this._setBackgroundImage(componentElem, renderContext.getImage(props.imageName));
    }
};

// Represents a panel component renderer.
rs.mimic.PanelRenderer = class extends rs.mimic.RegularComponentRenderer {
    get canUpdateDom() {
        return true;
    }

    _setClasses(componentElem) {
        super._setClasses(componentElem);
        componentElem.addClass("panel");
    }
};

// Represents a faceplate renderer.
rs.mimic.FaceplateRenderer = class extends rs.mimic.ComponentRenderer {
    get canUpdateDom() {
        return true;
    }

    _setClasses(componentElem) {
        super._setClasses(componentElem);
        componentElem.addClass("faceplate");
    }
};

// Encapsulates information about a rendering operation.
rs.mimic.RenderContext = class {
    editMode = false;
    editorOptions = null;
    faceplateMode = false;
    imageMap = null;
    idPrefix = "";
    unknownTypes = null;
    dataProvider = null;

    constructor(source) {
        Object.assign(this, source);
    }

    getImage(imageName) {
        return this.imageMap instanceof Map ? this.imageMap.get(imageName) : null;
    }

    getDataProvider() {
        return this.dataProvider ?? rs.mimic.DataProvider.STUB;
    }
};

// Represents a provider of channel data and channel properties.
rs.mimic.DataProvider = class DataProvider {
    static EMPTY_DATA = {
        d: { cnlNum: 0, val: 0.0, stat: 0 },
        df: { dispVal: "", colors: [] }
    };
    static EMPTY_CNL_PROPS = { cnlNum: 0, joinLen: 1, unit: "" };
    static STUB = new DataProvider();

    curCnlDataMap = null;
    prevCnlDataMap = null;
    cnlPropsMap = null;

    getCurData(cnlNum, opt_joinLen) {
        return DataProvider.EMPTY_DATA;
    }

    getPrevData(cnlNum, opt_joinLen) {
        return DataProvider.EMPTY_DATA;
    }

    getCnlProps(cnlNum) {
        return DataProvider.EMPTY_CNL_PROPS;
    }
};

// Contains renderers for a mimic and its components.
rs.mimic.RendererSet = class {
    static mimicRenderer = new rs.mimic.MimicRenderer();
    static faceplateRenderer = new rs.mimic.FaceplateRenderer();
    static componentRenderers = new Map([
        ["Text", new rs.mimic.TextRenderer()],
        ["Picture", new rs.mimic.PictureRenderer()],
        ["Panel", new rs.mimic.PanelRenderer()]
    ]);
};

// Renders a mimic using appropriate renderers.
rs.mimic.UnitedRenderer = class {
    mimic;
    editMode;
    editorOptions;

    constructor(mimic, editMode) {
        this.mimic = mimic;
        this.editMode = editMode;
        this.editorOptions = null;
    }

    // Appends the component DOM to its parent.
    _appendToParent(component) {
        if (component.parent) {
            rs.mimic.Renderer.appendChild(component.parent, component);
        }
    }

    // Creates a component DOM.
    _createComponentDom(component, renderContext) {
        if (component.isFaceplate) {
            this._createFaceplateDom(component, renderContext);
        } else {
            let renderer = rs.mimic.RendererSet.componentRenderers.get(component.typeName);

            if (renderer) {
                component.renderer = renderer;
                renderer.createDom(component, renderContext);
                this._appendToParent(component);
            } else {
                renderContext.unknownTypes?.add(component.typeName);
            }
        }
    }

    // Creates a faceplate DOM.
    _createFaceplateDom(faceplateInstance, renderContext) {
        if (!faceplateInstance.model) {
            renderContext.unknownTypes?.add(faceplateInstance.typeName);
            return;
        }

        let faceplateContext = new rs.mimic.RenderContext({
            editMode: this.editMode,
            editorOptions: this.editorOptions,
            faceplateMode: true,
            imageMap: faceplateInstance.model.imageMap,
            idPrefix: renderContext.idPrefix,
            unknownTypes: renderContext.unknownTypes
        });
        let renderer = rs.mimic.RendererSet.faceplateRenderer;
        faceplateInstance.renderer = renderer;
        renderer.createDom(faceplateInstance, faceplateContext);
        this._appendToParent(faceplateInstance);
        faceplateContext.idPrefix += faceplateInstance.id + "-";

        for (let component of faceplateInstance.components) {
            this._createComponentDom(component, faceplateContext);
        }
    }

    // Creates a mimic DOM according to the mimic model. Returns a jQuery object.
    createMimicDom() {
        let startTime = Date.now();
        let renderContext = new rs.mimic.RenderContext({
            editMode: this.editMode,
            editorOptions: this.editorOptions,
            imageMap: this.mimic.imageMap,
            unknownTypes: new Set()
        });
        let renderer = rs.mimic.RendererSet.mimicRenderer;
        this.mimic.renderer = renderer;
        renderer.createDom(this.mimic, renderContext);

        for (let component of this.mimic.components) {
            this._createComponentDom(component, renderContext);
        }

        if (renderContext.unknownTypes.size > 0) {
            console.warn("Unable to render components of types: " +
                Array.from(renderContext.unknownTypes).sort().join(", "));
        }

        if (this.mimic.dom) {
            console.info("Mimic DOM created in " + (Date.now() - startTime) + " ms");
            return this.mimic.dom;
        } else {
            return $();
        }
    }

    // Updates a mimic DOM according to the mimic model.
    updateMimicDom() {
        rs.mimic.RendererSet.mimicRenderer.updateDom(this.mimic, new rs.mimic.RenderContext({
            editMode: this.editMode,
            editorOptions: this.editorOptions,
            imageMap: this.mimic.imageMap,
        }));
    }

    // Creates a component DOM according to the component model. Returns a jQuery object.
    createComponentDom(component) {
        this._createComponentDom(component, new rs.mimic.RenderContext({
            editMode: this.editMode,
            editorOptions: this.editorOptions,
            imageMap: this.mimic.imageMap
        }));
        return component.dom ?? $();
    }

    // Updates the component DOM according to the component model.
    updateComponentDom(component) {
        if (component.dom && component.renderer) {
            let renderContext = new rs.mimic.RenderContext({
                editMode: this.editMode,
                editorOptions: this.editorOptions
            });

            if (component.isFaceplate) {
                renderContext.faceplateMode = true;
                renderContext.imageMap = component.model.imageMap;
                component.renderer.updateDom(component, renderContext);
            } else {
                renderContext.imageMap = mimic.imageMap;

                if (component.renderer.canUpdateDom) {
                    component.renderer.updateDom(component, renderContext);
                } else {
                    let oldDom = component.dom;
                    component.renderer.createDom(component, renderContext);
                    oldDom.replaceWith(component.dom);
                }
            }
        }
    }

    // Arranges the child component DOMs according to the parent's model.
    arrangeChildren(parent) {
        if (parent.children) {
            // detach components
            for (let component of parent.children) {
                if (component.dom) {
                    component.dom.detach();
                }
            }

            // append components
            for (let component of parent.children) {
                this._appendToParent(component);
            }
        }
    }

    // Updates the components according to the current channel data.
    updateData(dataProvider) {
        let renderContext = new rs.mimic.RenderContext({
            imageMap: this.mimic.imageMap,
            dataProvider: dataProvider
        });

        for (let component of this.mimic.components) {
            if (component.renderer) {
                component.renderer.updateData(component, renderContext);
            }
        }
    }
};
