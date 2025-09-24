// Contains classes: MimicFactory, ComponentFactory, RegularComponentFactory,
//     TextFactory, PictureScript, PictureFactory, PanelFactory, FaceplateFactory, FactorySet
// Depends on mimic-common.js, mimic-model.js, mimic-model-subtypes.js

// Create mimic properties.
rs.mimic.MimicFactory = class {
    // Parses the document properties from the specified source object.
    static parseProperties(sourceProps, isFaceplate) {
        const PropertyParser = rs.mimic.PropertyParser;
        sourceProps ??= {};
        let props = {
            // appearance
            backColor: PropertyParser.parseString(sourceProps.backColor),
            backgroundImage: PropertyParser.parseString(sourceProps.backgroundImage),
            backgroundPadding: new rs.mimic.Padding(),
            cssClass: PropertyParser.parseString(sourceProps.cssClass),
            font: rs.mimic.Font.parse(sourceProps.font),
            foreColor: PropertyParser.parseString(sourceProps.foreColor),
            stylesheet: PropertyParser.parseString(sourceProps.stylesheet),

            // behavior
            script: PropertyParser.parseString(sourceProps.script),
            tooltip: PropertyParser.parseString(sourceProps.tooltip),

            // layout
            size: rs.mimic.Size.parse(sourceProps.size, { width: 800, height: 600 })
        };

        // faceplate properties
        if (isFaceplate) {
            props.blinkingState = rs.mimic.VisualState.parse(sourceProps.blinkingState);
            props.border = rs.mimic.Border.parse(sourceProps.border);
            props.cornerRadius = rs.mimic.CornerRadius.parse(sourceProps.cornerRadius);
            props.disabledState = rs.mimic.VisualState.parse(sourceProps.disabledState);
            props.hoverState = rs.mimic.VisualState.parse(sourceProps.hoverState);
            props.propertyExports = rs.mimic.PropertyExportList.parse(sourceProps.propertyExports);
        }

        return props;
    }
}

// Represents an abstract component factory.
rs.mimic.ComponentFactory = class {
    // Copies the properties from the source object.
    _copyProperties(component, source) {
        component.id = source.id;
        component.typeName = source.typeName;
        component.properties = this.parseProperties(source.properties);
        component.properties.typeName = source.typeName;
        component.bindings = source.bindings;
        component.parentID = source.parentID;
    }

    // Creates and adds default property bindings.
    _addDefaultBindings(component) {
        if (component.bindings && component.bindings.inCnlNum > 0) {
            let defaultBindings = this._createDefaultBindings(component);

            if (defaultBindings) {
                let bindingExists = binding => {
                    return component.bindings.propertyBindings.some(pb => pb.propertyName === binding.propertyName);
                };

                for (let binding of defaultBindings) {
                    if (!bindingExists(binding)) {
                        component.bindings.propertyBindings.push(binding);
                    }
                }
            }
        }
    }

    // Creates an array of default property bindings for the component.
    _createDefaultBindings(component) {
        return null;
    }

    // Creates an object that implements additional component logic.
    _createExtraScript() {
        return null;
    }

    // Creates new component properties.
    createProperties() {
        return {
            // behavior
            blinking: false,
            clickAction: new rs.mimic.Action(),
            enabled: true,
            visible: true,

            // data
            checkRights: false,
            deviceNum: 0,
            inCnlNum: 0,
            objNum: 0,
            outCnlNum: 0,
            propertyBindings: new rs.mimic.PropertyBindingList(),

            // design
            id: 0,
            name: "",
            typeName: "",

            // layout
            location: new rs.mimic.Point(),
            size: new rs.mimic.Size()
        };
    }

    // Parses the component properties from the specified source object.
    parseProperties(sourceProps) {
        const PropertyParser = rs.mimic.PropertyParser;
        sourceProps ??= {};
        return {
            // behavior
            blinking: PropertyParser.parseBool(sourceProps.blinking),
            clickAction: rs.mimic.Action.parse(sourceProps.clickAction),
            enabled: PropertyParser.parseBool(sourceProps.enabled),
            visible: PropertyParser.parseBool(sourceProps.visible),

            // data
            checkRights: PropertyParser.parseBool(sourceProps.checkRights),
            deviceNum: PropertyParser.parseInt(sourceProps.deviceNum),
            inCnlNum: PropertyParser.parseInt(sourceProps.inCnlNum),
            objNum: PropertyParser.parseInt(sourceProps.objNum),
            outCnlNum: PropertyParser.parseInt(sourceProps.outCnlNum),
            propertyBindings: rs.mimic.PropertyBindingList.parse(sourceProps.propertyBindings),

            // design
            id: PropertyParser.parseInt(sourceProps.id),
            name: PropertyParser.parseString(sourceProps.name),
            typeName: PropertyParser.parseString(sourceProps.typeName),

            // layout
            location: rs.mimic.Point.parse(sourceProps.location),
            size: rs.mimic.Size.parse(sourceProps.size)
        };
    }

    // Creates a new component with the given type name.
    createComponent(typeName) {
        let component = new rs.mimic.Component();
        component.typeName = typeName;
        component.properties = this.createProperties(typeName);
        component.properties.typeName = typeName;
        component.extraScript = this._createExtraScript();
        return component;
    }

    // Creates a new component with the specified properties.
    createComponentFromSource(source) {
        let component = new rs.mimic.Component();
        this._copyProperties(component, source);
        this._addDefaultBindings(component);
        component.extraScript = this._createExtraScript();
        return component;
    }
};

// Represents an abstract factory for regular non-faceplate components.
rs.mimic.RegularComponentFactory = class extends rs.mimic.ComponentFactory {
    createProperties() {
        let props = super.createProperties();

        // appearance
        Object.assign(props, {
            backColor: "",
            border: new rs.mimic.Border(),
            cornerRadius: new rs.mimic.CornerRadius(),
            cssClass: "",
            font: new rs.mimic.Font({ inherit: true }),
            foreColor: ""
        });

        // behavior
        Object.assign(props, {
            blinkingState: new rs.mimic.VisualState(),
            disabledState: new rs.mimic.VisualState(),
            hoverState: new rs.mimic.VisualState(),
            script: "",
            tooltip: ""
        });

        return props;
    }

    parseProperties(sourceProps) {
        const PropertyParser = rs.mimic.PropertyParser;
        let props = super.parseProperties(sourceProps);
        sourceProps ??= {};

        // appearance
        Object.assign(props, {
            backColor: PropertyParser.parseString(sourceProps.backColor),
            border: rs.mimic.Border.parse(sourceProps.border),
            cornerRadius: rs.mimic.CornerRadius.parse(sourceProps.cornerRadius),
            cssClass: PropertyParser.parseString(sourceProps.cssClass),
            font: rs.mimic.Font.parse(sourceProps.font),
            foreColor: PropertyParser.parseString(sourceProps.foreColor)
        });

        // behavior
        Object.assign(props, {
            blinkingState: rs.mimic.VisualState.parse(sourceProps.blinkingState),
            disabledState: rs.mimic.VisualState.parse(sourceProps.disabledState),
            hoverState: rs.mimic.VisualState.parse(sourceProps.hoverState),
            script: PropertyParser.parseString(sourceProps.script),
            tooltip: PropertyParser.parseString(sourceProps.tooltip)
        });

        return props;
    }
};

// Creates components of the Text type.
rs.mimic.TextFactory = class extends rs.mimic.RegularComponentFactory {
    _createDefaultBindings(component) {
        const DataMember = rs.mimic.DataMember;
        let cnlNum = component.bindings.inCnlNum;
        let cnlProps = component.bindings.inCnlProps;
        let bindings = [{
            propertyName: "text",
            dataSource: String(cnlNum),
            dataMember: DataMember.DISPLAY_VALUE_WITH_UNIT,
            format: "",
            propertyChain: ["text"],
            cnlNum: cnlNum,
            cnlProps: cnlProps
        }];

        if (!component.properties.foreColor) {
            bindings.push({
                propertyName: "foreColor",
                dataSource: String(cnlNum),
                dataMember: DataMember.COLOR0,
                format: "",
                propertyChain: ["foreColor"],
                cnlNum: cnlNum,
                cnlProps: cnlProps
            });
        }

        return bindings;
    }

    createProperties() {
        let props = super.createProperties();

        // appearance
        Object.assign(props, {
            text: "Text",
            textAlign: rs.mimic.ContentAlignment.TOP_LEFT,
            textDirection: rs.mimic.TextDirection.HORIZONTAL,
            wordWrap: false
        });

        // layout
        Object.assign(props, {
            autoSize: true,
            padding: new rs.mimic.Padding()
        });

        return props;
    }

    parseProperties(sourceProps) {
        const PropertyParser = rs.mimic.PropertyParser;
        let props = super.parseProperties(sourceProps);
        sourceProps ??= {};

        // appearance
        Object.assign(props, {
            text: PropertyParser.parseString(sourceProps.text),
            textAlign: PropertyParser.parseString(sourceProps.textAlign, rs.mimic.ContentAlignment.TOP_LEFT),
            textDirection: PropertyParser.parseString(sourceProps.textDirection, rs.mimic.TextDirection.HORIZONTAL),
            wordWrap: PropertyParser.parseBool(sourceProps.wordWrap)
        });

        // layout
        Object.assign(props, {
            autoSize: PropertyParser.parseBool(sourceProps.autoSize),
            padding: rs.mimic.Padding.parse(sourceProps.padding)
        });

        return props;
    }

    createComponent() {
        return super.createComponent("Text");
    }
};

// Implements logic for Picture type components.
rs.mimic.PictureScript = class extends rs.mimic.ComponentScript {
    dataUpdated(args) {
        // select image according to conditions
        let cnlNum = args.component.bindings?.inCnlNum;
        let props = args.component.properties;
        let conditions = props.conditions;

        if (cnlNum > 0 && conditions.length > 0) {
            let curData = args.dataProvider.getCurData(cnlNum);
            let prevData = args.dataProvider.getPrevData(cnlNum);

            if (dataProvider.dataChanged(curData, prevData)) {
                let imageName = props.defaultImage;

                if (curData.d.stat > 0) {
                    for (let condition of conditions) {
                        if (condition.satisfied(curData.d.val)) {
                            imageName = condition.imageName;
                            break;
                        }
                    }
                }

                if (props.imageName !== imageName) {
                    props.imageName = imageName;
                    args.propertyChanged = true;
                }
            }
        }
    }
};

// Creates components of the Picture type.
rs.mimic.PictureFactory = class extends rs.mimic.RegularComponentFactory {
    _createExtraScript() {
        return new rs.mimic.PictureScript();
    }

    createProperties() {
        let props = super.createProperties();

        // appearance
        Object.assign(props, {
            imageName: "",
            imageStretch: rs.mimic.ImageStretch.NONE,
            rotation: 0
        });

        // behavior
        Object.assign(props, {
            conditions: new rs.mimic.ImageConditionList(),
            defaultImage: ""
        });

        // layout
        Object.assign(props, {
            padding: new rs.mimic.Padding()
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
            imageStretch: PropertyParser.parseString(sourceProps.imageStretch, rs.mimic.ImageStretch.NONE),
            rotation: PropertyParser.parseFloat(sourceProps.rotation)
        });

        // behavior
        Object.assign(props, {
            conditions: rs.mimic.ImageConditionList.parse(sourceProps.conditions),
            defaultImage: PropertyParser.parseString(sourceProps.defaultImage)
        });

        // layout
        Object.assign(props, {
            padding: rs.mimic.Padding.parse(sourceProps.padding)
        });

        return props;
    }

    createComponent() {
        return super.createComponent("Picture");
    }
};

// Creates components of the Panel type.
rs.mimic.PanelFactory = class extends rs.mimic.RegularComponentFactory {
    createComponent() {
        let component = super.createComponent("Panel");
        component.children = []; // accept child components
        return component;
    }

    createComponentFromSource(source) {
        let component = super.createComponentFromSource(source);
        component.children = [];
        return component;
    }
};

// Creates faceplate instances.
rs.mimic.FaceplateFactory = class extends rs.mimic.ComponentFactory {
    faceplate; // can be null

    constructor(faceplate) {
        super();
        this.faceplate = faceplate;
    }

    _updateSize(faceplateInstance) {
        faceplateInstance.properties.size = rs.mimic.Size.parse(this.faceplate.document.size);
    }

    _createComponents(faceplateInstance) {
        const FactorySet = rs.mimic.FactorySet;
        const MimicHelper = rs.mimic.MimicHelper;

        if (Array.isArray(this.faceplate.components)) {
            for (let sourceComponent of this.faceplate.components) {
                let factory = FactorySet.getComponentFactory(sourceComponent.typeName, this.faceplate.faceplateMap);

                if (factory) {
                    let componentCopy = factory.createComponentFromSource(sourceComponent);
                    faceplateInstance.components.push(componentCopy);

                    if (componentCopy.name) {
                        faceplateInstance.componentByName.set(componentCopy.name, componentCopy);
                    }
                }
            }

            MimicHelper.defineNesting(faceplateInstance, faceplateInstance.components);
        }
    }

    _createCustomProperties(faceplateInstance, sourceProps) {
        const ObjectHelper = rs.mimic.ObjectHelper;
        sourceProps ??= {};

        for (let propertyExport of this.faceplate.propertyExports) {
            let baseValue = faceplateInstance.getTargetPropertyValue(propertyExport) ?? propertyExport.defaultValue;
            let sourceValue = sourceProps[propertyExport.name];

            if (sourceValue === null || sourceValue === undefined) {
                faceplateInstance.properties[propertyExport.name] = baseValue;
            } else {
                let mergedValue = ObjectHelper.mergeValues(baseValue, sourceValue);
                faceplateInstance.properties[propertyExport.name] = mergedValue;
                faceplateInstance.setTargetPropertyValue(propertyExport, mergedValue);
            }
        }
    }

    _applyModel(faceplateInstance, source) {
        faceplateInstance.typeName = faceplateInstance.properties.typeName = this.faceplate.typeName;
        faceplateInstance.model = this.faceplate;
        this._createComponents(faceplateInstance);
        this._createCustomProperties(faceplateInstance, source?.properties);
    }

    createComponent() {
        let faceplateInstance = new rs.mimic.FaceplateInstance();
        faceplateInstance.properties = this.createProperties();

        if (this.faceplate) {
            this._updateSize(faceplateInstance);
            this._applyModel(faceplateInstance, null);
        }

        return faceplateInstance;
    }

    createComponentFromSource(source) {
        let faceplateInstance = new rs.mimic.FaceplateInstance();
        this._copyProperties(faceplateInstance, source);

        if (this.faceplate) {
            this._applyModel(faceplateInstance, source);
        }

        return faceplateInstance;
    }
};

// Contains factories for mimic components.
rs.mimic.FactorySet = class FactorySet {
    static componentFactories = new Map([
        ["Text", new rs.mimic.TextFactory()],
        ["Picture", new rs.mimic.PictureFactory()],
        ["Panel", new rs.mimic.PanelFactory()]
    ]);
    static getFaceplateFactory(faceplate) {
        return new rs.mimic.FaceplateFactory(faceplate);
    }
    static getComponentFactory(typeName, faceplateMap) {
        if (faceplateMap.has(typeName)) {
            let faceplate = faceplateMap.get(typeName); // can be null
            return FactorySet.getFaceplateFactory(faceplate);
        } else {
            return FactorySet.componentFactories.get(typeName);
        }
    }
};
