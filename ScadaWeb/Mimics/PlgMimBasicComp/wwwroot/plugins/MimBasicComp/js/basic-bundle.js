// Contains descriptors for basic components.

rs.mimic.BasicSubtype = class {
    // Enumerations
    static TOGGLE_POSITION = "BasicTogglePosition";

    // Structures
    static COLOR_CONDITION = "BasicColorCondition";
};

rs.mimic.BasicButtonDescriptor = class extends rs.mimic.RegularComponentDescriptor {
    constructor() {
        super();
        const KnownCategory = rs.mimic.KnownCategory;
        const BasicType = rs.mimic.BasicType;
        const Subtype = rs.mimic.Subtype;
        const PropertyEditor = rs.mimic.PropertyEditor;
        const PropertyDescriptor = rs.mimic.PropertyDescriptor;

        // appearance
        this.add(new PropertyDescriptor({
            name: "imageName",
            displayName: "Image",
            category: KnownCategory.APPEARANCE,
            type: BasicType.STRING,
            editor: PropertyEditor.IMAGE_DIALOG
        }));

        this.add(new PropertyDescriptor({
            name: "imageSize",
            displayName: "Image size",
            category: KnownCategory.APPEARANCE,
            type: BasicType.STRUCT,
            subtype: Subtype.SIZE
        }));

        this.add(new PropertyDescriptor({
            name: "text",
            displayName: "Text",
            category: KnownCategory.APPEARANCE,
            type: BasicType.STRING
        }));
    }
};

rs.mimic.BasicLedDescriptor = class extends rs.mimic.RegularComponentDescriptor {
    constructor() {
        super();
        const KnownCategory = rs.mimic.KnownCategory;
        const BasicType = rs.mimic.BasicType;
        const BasicSubtype = rs.mimic.BasicSubtype;
        const PropertyEditor = rs.mimic.PropertyEditor;
        const PropertyDescriptor = rs.mimic.PropertyDescriptor;

        // appearance
        this.add(new PropertyDescriptor({
            name: "borderOpacity",
            displayName: "Border opacity",
            category: KnownCategory.APPEARANCE,
            type: BasicType.INT
        }));

        this.add(new PropertyDescriptor({
            name: "isSquare",
            displayName: "Square",
            category: KnownCategory.APPEARANCE,
            type: BasicType.BOOL
        }));

        // behavior
        this.add(new PropertyDescriptor({
            name: "conditions",
            displayName: "Conditions",
            category: KnownCategory.BEHAVIOR,
            type: BasicType.LIST,
            subtype: BasicSubtype.COLOR_CONDITION
        }));

        this.add(new PropertyDescriptor({
            name: "defaultColor",
            displayName: "Default color",
            category: KnownCategory.BEHAVIOR,
            type: BasicType.STRING,
            editor: PropertyEditor.COLOR_DIALOG
        }));
    }
};

rs.mimic.BasicToggleDescriptor = class extends rs.mimic.RegularComponentDescriptor {
    constructor() {
        super();
        const KnownCategory = rs.mimic.KnownCategory;
        const BasicType = rs.mimic.BasicType;
        const Subtype = rs.mimic.Subtype;
        const BasicSubtype = rs.mimic.BasicSubtype;
        const PropertyDescriptor = rs.mimic.PropertyDescriptor;

        // appearance
        this.add(new PropertyDescriptor({
            name: "position",
            displayName: "Position",
            category: KnownCategory.APPEARANCE,
            type: BasicType.ENUM,
            subtype: BasicSubtype.TOGGLE_POSITION
        }));

        // layout
        this.add(new PropertyDescriptor({
            name: "padding",
            displayName: "Padding",
            category: KnownCategory.LAYOUT,
            type: BasicType.STRUCT,
            subtype: Subtype.PADDING
        }));
    }
};

rs.mimic.BasicColorConditionDescriptor = class extends rs.mimic.ConditionDescriptor {
    constructor() {
        super();
        const BasicType = rs.mimic.BasicType;
        const PropertyEditor = rs.mimic.PropertyEditor;
        const PropertyDescriptor = rs.mimic.PropertyDescriptor;

        this.add(new PropertyDescriptor({
            name: "color",
            displayName: "Color",
            type: BasicType.STRING,
            editor: PropertyEditor.COLOR_DIALOG
        }));
    }
}

// Registers the descriptors. The function name must be unique.
function registerBasicDescriptors() {
    let componentDescriptors = rs.mimic.DescriptorSet.componentDescriptors;
    componentDescriptors.set("BasicButton", new rs.mimic.BasicButtonDescriptor());
    componentDescriptors.set("BasicLed", new rs.mimic.BasicLedDescriptor());
    componentDescriptors.set("BasicToggle", new rs.mimic.BasicToggleDescriptor());

    let structureDescriptors = rs.mimic.DescriptorSet.structureDescriptors;
    structureDescriptors.set("BasicColorCondition", new rs.mimic.BasicColorConditionDescriptor);
}

registerBasicDescriptors();

// Contains factories for basic components.

rs.mimic.BasicButtonFactory = class extends rs.mimic.RegularComponentFactory {
    createProperties() {
        let props = super.createProperties();

        // change inherited properties
        props.size.width = 100;
        props.size.height = 30;

        // appearance
        Object.assign(props, {
            imageName: "",
            imageSize: new rs.mimic.Size({ width: 16, height: 16 }),
            text: "Button",
        });

        return props;
    }

    parseProperties(sourceProps) {
        const PropertyParser = rs.mimic.PropertyParser;
        let props = super.parseProperties(sourceProps);
        sourceProps ??= {};

        // appearance
        Object.assign(props, {
            imageName: PropertyParser.parseString(sourceProps.imageName),
            imageSize: rs.mimic.Size.parse(sourceProps.imageSize),
            text: PropertyParser.parseString(sourceProps.text),
        });

        return props;
    }

    createComponent() {
        return super.createComponent("BasicButton");
    }
};

rs.mimic.BasicLedScript = class extends rs.mimic.ComponentScript {
    dataUpdated(args) {
        // select background color according to conditions
        let cnlNum = args.component.bindings?.inCnlNum;
        let props = args.component.properties;
        let conditions = props.conditions;

        if (cnlNum > 0 && conditions.length > 0) {
            let curData = args.dataProvider.getCurData(cnlNum);
            let prevData = args.dataProvider.getPrevData(cnlNum);

            if (dataProvider.dataChanged(curData, prevData)) {
                let color = props.defaultColor;

                if (curData.d.stat > 0) {
                    for (let condition of conditions) {
                        if (condition.satisfied(curData.d.val)) {
                            color = condition.color;
                            break;
                        }
                    }
                }

                if (props.backColor !== color) {
                    props.backColor = color;
                    args.propertyChanged = true;
                }
            }
        }
    }
};

rs.mimic.BasicLedFactory = class extends rs.mimic.RegularComponentFactory {
    _createExtraScript() {
        return new rs.mimic.BasicLedScript();
    }

    _addDefaultConditions(conditions) {
        const BasicColorCondition = rs.mimic.BasicColorCondition;
        const ComparisonOperator = rs.mimic.ComparisonOperator;

        conditions.push(new BasicColorCondition({
            comparisonOper1: ComparisonOperator.LESS_THAN_EQUAL,
            color: "Red"
        }));

        conditions.push(new BasicColorCondition({
            comparisonOper1: ComparisonOperator.GREATER_THAN,
            color: "Green"
        }));
    }

    createProperties() {
        let props = super.createProperties();

        // change inherited properties
        props.backColor = "Silver";
        props.border.color = "Black";
        props.border.width = 3;
        props.size.width = 30;
        props.size.height = 30;

        // appearance
        props.borderOpacity = 30;
        props.isSquare = false;

        // behavior
        props.conditions = new rs.mimic.BasicColorConditionList();
        props.defaultColor = "Silver";

        this._addDefaultConditions(props.conditions);
        return props;
    }

    parseProperties(sourceProps) {
        const PropertyParser = rs.mimic.PropertyParser;
        let props = super.parseProperties(sourceProps);
        sourceProps ??= {};

        // appearance
        props.borderOpacity = PropertyParser.parseInt(sourceProps.borderOpacity);
        props.isSquare = PropertyParser.parseBool(sourceProps.isSquare);

        // behavior
        props.conditions = rs.mimic.BasicColorConditionList.parse(sourceProps.conditions);
        props.defaultColor = PropertyParser.parseString(sourceProps.defaultColor);
        return props;
    }

    createComponent() {
        return super.createComponent("BasicLed");
    }
};

rs.mimic.BasicToggleScript = class extends rs.mimic.ComponentScript {
    dataUpdated(args) {
        // set toggle position
        const BasicTogglePosition = rs.mimic.BasicTogglePosition;
        let cnlNum = args.component.bindings?.inCnlNum;

        if (cnlNum > 0) {
            let curData = args.dataProvider.getCurData(cnlNum);
            let prevData = args.dataProvider.getPrevData(cnlNum);

            if (dataProvider.dataChanged(curData, prevData)) {
                let props = args.component.properties;
                let position = props.position;

                if (curData.d.stat > 0) {
                    position = curData.d.val > 0
                        ? BasicTogglePosition.ON
                        : BasicTogglePosition.OFF;
                } else {
                    position = BasicTogglePosition.NOT_SET;
                }

                if (props.position !== position) {
                    props.position = position;
                    args.propertyChanged = true;
                }
            }
        }
    }

    getCommandValue(args) {
        return args.component.properties.position === rs.mimic.BasicTogglePosition.ON ? 0 : 1;
    }
};

rs.mimic.BasicToggleFactory = class extends rs.mimic.RegularComponentFactory {
    _createDefaultBindings(component) {
        const DataMember = rs.mimic.DataMember;
        let cnlNum = component.bindings.inCnlNum;
        let cnlProps = component.bindings.inCnlProps;
        let bindings = [];

        if (!component.properties.backColor) {
            bindings.push({
                propertyName: "backColor",
                dataSource: String(cnlNum),
                dataMember: DataMember.COLOR0,
                format: "",
                propertyChain: ["backColor"],
                cnlNum: cnlNum,
                cnlProps: cnlProps
            });
        }

        if (!component.properties.border.color) {
            bindings.push({
                propertyName: "border.color",
                dataSource: String(cnlNum),
                dataMember: DataMember.COLOR0,
                format: "",
                propertyChain: ["border", "color"],
                cnlNum: cnlNum,
                cnlProps: cnlProps
            });
        }

        return bindings;
    }

    _createExtraScript() {
        return new rs.mimic.BasicToggleScript();
    }

    createProperties() {
        let props = super.createProperties();

        // change inherited properties
        props.border.width = 2;
        props.foreColor = "White";
        props.size.width = 60;
        props.size.height = 30;

        // appearance
        props.position = rs.mimic.BasicTogglePosition.ON;

        // layout
        props.padding = new rs.mimic.Padding();

        return props;
    }

    parseProperties(sourceProps) {
        const PropertyParser = rs.mimic.PropertyParser;
        let props = super.parseProperties(sourceProps);
        sourceProps ??= {};
        props.position = PropertyParser.parseString(sourceProps.position, rs.mimic.BasicTogglePosition.NOT_SET);
        props.padding = rs.mimic.Padding.parse(sourceProps.padding);
        return props;
    }

    createComponent() {
        return super.createComponent("BasicToggle");
    }
};

// Registers the factories. The function name must be unique.
function registerBasicFactories() {
    let componentFactories = rs.mimic.FactorySet.componentFactories;
    componentFactories.set("BasicButton", new rs.mimic.BasicButtonFactory());
    componentFactories.set("BasicLed", new rs.mimic.BasicLedFactory());
    componentFactories.set("BasicToggle", new rs.mimic.BasicToggleFactory());
}

registerBasicFactories();

// Contains renderers for basic components.

rs.mimic.BasicButtonRenderer = class extends rs.mimic.RegularComponentRenderer {
    _setBorder(jqObj, border) {
        if (border.width > 0) {
            super._setBorder(jqObj, border);
        } else {
            jqObj.css("border", ""); // default border
        }
    }

    _setCornerRadius(jqObj, cornerRadius) {
        if (cornerRadius.isSet) {
            super._setCornerRadius(jqObj, cornerRadius);
        } else {
            jqObj.css("border-radius", ""); // default radius
        }
    }

    _completeDom(componentElem, component, renderContext) {
        componentElem.append("<div class='basic-button-content'>" +
            "<div class='basic-button-icon'></div>" +
            "<div class='basic-button-text'></div></div>");
    }

    _setClasses(componentElem, component, renderContext) {
        super._setClasses(componentElem, component, renderContext);
        componentElem.addClass("basic-button");
    }

    _setProps(componentElem, component, renderContext) {
        super._setProps(componentElem, component, renderContext);
        let contentElem = componentElem.children(".basic-button-content:first");
        let iconElem = contentElem.children(".basic-button-icon:first");
        let textElem = contentElem.children(".basic-button-text:first");
        let props = component.properties;

        if (props.imageName) {
            this._setBackgroundImage(iconElem, renderContext.getImage(props.imageName));
            this._setSize(iconElem, props.imageSize);
        } else {
            this._setBackgroundImage(iconElem, null);
            this._setSize(iconElem, { width: 0, height: 0 });
        }

        textElem.text(props.text);
    }

    createDom(component, renderContext) {
        let buttonElem = $("<button type='button'></button>")
            .attr("id", "comp" + renderContext.idPrefix + component.id)
            .attr("data-id", component.id);
        this._completeDom(buttonElem, component, renderContext);
        this._setClasses(buttonElem, component, renderContext);
        this._setProps(buttonElem, component, renderContext);
        component.dom = buttonElem;
        return buttonElem;
    }
};

rs.mimic.BasicLedRenderer = class extends rs.mimic.RegularComponentRenderer {
    _setBorder(jqObj, border) {
        // do nothing
    }

    _setCornerRadius(jqObj, cornerRadius) {
        // do nothing
    }

    _setLedBorder(componentElem, borderElem, border) {
        super._setBorder(componentElem, null);
        super._setBorder(borderElem, border);
    }

    _setLedCornerRadius(componentElem, borderElem, cornerRadius) {
        super._setCornerRadius(componentElem, cornerRadius);
        super._setCornerRadius(borderElem, cornerRadius);
    }

    _completeDom(componentElem, component, renderContext) {
        $("<div class='basic-led-border'></div>").appendTo(componentElem);
    }

    _setClasses(componentElem, component, renderContext) {
        super._setClasses(componentElem, component, renderContext);
        componentElem.addClass("basic-led");

        if (!component.properties.isSquare) {
            componentElem.addClass("circle");
        }
    }

    _setProps(componentElem, component, renderContext) {
        super._setProps(componentElem, component, renderContext);
        let borderElem = componentElem.children(".basic-led-border:first");
        let props = component.properties;
        this._setLedBorder(componentElem, borderElem, props.border);
        this._setLedCornerRadius(componentElem, borderElem, props.isSquare ? props.cornerRadius : null);
        borderElem.css("opacity", props.borderOpacity / 100);

        // configure area outside rounded corners
        if (renderContext.editMode) {
            componentElem.css("--border-width", 0);
        }
    }
};

rs.mimic.BasicToggleRenderer = class extends rs.mimic.RegularComponentRenderer {
    _completeDom(componentElem, component, renderContext) {
        componentElem.append("<div class='basic-toggle-lever'></div>");
    }

    _setClasses(componentElem, component, renderContext) {
        super._setClasses(componentElem, component, renderContext);
        componentElem.addClass("basic-toggle");

        const BasicTogglePosition = rs.mimic.BasicTogglePosition;
        let position = component.properties.position;
        componentElem.toggleClass("position-not-set", position === BasicTogglePosition.NOT_SET);
        componentElem.toggleClass("position-off", position === BasicTogglePosition.OFF);
        componentElem.toggleClass("position-on", position === BasicTogglePosition.ON);
    }

    _setProps(componentElem, component, renderContext) {
        super._setProps(componentElem, component, renderContext);
        let props = component.properties;
        this._setPadding(componentElem, props.padding);

        let leverElem = componentElem.children().first();
        let minSize = Math.min(component.width, component.height);
        let minInnerSize = Math.min(component.innerWidth, component.innerHeight);
        componentElem.css("border-radius", minSize / 2);

        leverElem.css({
            "background-color": props.foreColor,
            "width": minInnerSize,
            "height": minInnerSize
        });
    }
};

// Registers the renderers. The function name must be unique.
function registerBasicRenderers() {
    let componentRenderers = rs.mimic.RendererSet.componentRenderers;
    componentRenderers.set("BasicButton", new rs.mimic.BasicButtonRenderer());
    componentRenderers.set("BasicLed", new rs.mimic.BasicLedRenderer());
    componentRenderers.set("BasicToggle", new rs.mimic.BasicToggleRenderer());
}

registerBasicRenderers();

// Contains subtypes for basic components.

rs.mimic.BasicTogglePosition = class {
    static NOT_SET = "NotSet";
    static OFF = "Off";
    static ON = "On";
}

rs.mimic.BasicColorCondition = class extends rs.mimic.Condition {
    color = "";

    constructor(source) {
        super();
        Object.assign(this, source);
    }

    get typeName() {
        return "BasicColorCondition";
    }

    static parse(source) {
        const PropertyParser = rs.mimic.PropertyParser;
        let colorCondition = new rs.mimic.BasicColorCondition();

        if (source) {
            colorCondition.color = PropertyParser.parseString(source.color);
            colorCondition._copyFrom(source);
        }

        return colorCondition;
    }
};

rs.mimic.BasicColorConditionList = class extends rs.mimic.List {
    constructor() {
        super(() => {
            return new rs.mimic.BasicColorCondition();
        });
    }

    static parse(source) {
        const BasicColorCondition = rs.mimic.BasicColorCondition;
        let colorConditions = new rs.mimic.BasicColorConditionList();

        if (Array.isArray(source)) {
            for (let sourceItem of source) {
                colorConditions.push(BasicColorCondition.parse(sourceItem));
            }
        }

        return colorConditions;
    }
}
