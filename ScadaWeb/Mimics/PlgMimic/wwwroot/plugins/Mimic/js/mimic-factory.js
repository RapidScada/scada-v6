// Contains classes: MimicFactory, ComponentFactory, RegularComponentFactory,
//     TextFactory, PictureFactory, PanelFactory, FaceplateFactory, FactorySet
// Depends on mimic-common.js, mimic-model.js, mimic-model-subtypes.js

// Create mimic properties.
rs.mimic.MimicFactory = class {
    // Parses the document properties from the specified source object.
    static parseProperties(sourceProps) {
        const PropertyParser = rs.mimic.PropertyParser;
        sourceProps ??= {};
        return {
            // appearance
            backColor: PropertyParser.parseString(sourceProps.backColor),
            backgroundImage: PropertyParser.parseString(sourceProps.backgroundImage),
            font: rs.mimic.Font.parse(sourceProps.font),
            foreColor: PropertyParser.parseString(sourceProps.foreColor),
            stylesheet: PropertyParser.parseString(sourceProps.stylesheet),

            // behavior
            script: PropertyParser.parseString(sourceProps.script),
            tooltip: PropertyParser.parseString(sourceProps.tooltip),

            // data
            propertyExports: rs.mimic.PropertyExportList.parse(sourceProps.propertyExports),

            // layout
            size: rs.mimic.Size.parse(sourceProps.size)
        };
    }
}

// Represents an abstract component factory.
rs.mimic.ComponentFactory = class {
    // Creates new component properties.
    _createProperties() {
        return {
            // behavior
            blinking: false,
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
    _parseProperties(sourceProps) {
        const PropertyParser = rs.mimic.PropertyParser;
        sourceProps ??= {};
        return {
            // behavior
            blinking: PropertyParser.parseBool(sourceProps.blinking),
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

    // Copies the properties from the source object.
    _copyProperties(component, source) {
        component.id = source.id;
        component.typeName = source.typeName;
        component.properties = this._parseProperties(source.properties);
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
                    return component.bindings.propertyBindings.some(pb => pb.dataMember === binding.dataMember);
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

    // Creates a new component with the given type name.
    createComponent(typeName) {
        let component = new rs.mimic.Component();
        component.typeName = typeName;
        component.properties = this._createProperties(typeName);
        component.properties.typeName = typeName;
        return component;
    }

    // Creates a new component with the specified properties, making deep copies of the source properties.
    createComponentFromSource(source) {
        let component = new rs.mimic.Component();
        this._copyProperties(component, source);
        this._addDefaultBindings(component);
        return component;
    }
};

// Represents an abstract factory for regular non-faceplate components.
rs.mimic.RegularComponentFactory = class extends rs.mimic.ComponentFactory {
    _createProperties() {
        let properties = super._createProperties();

        // appearance
        Object.assign(properties, {
            backColor: "",
            border: new rs.mimic.Border(),
            cornerRadius: new rs.mimic.CornerRadius(),
            cssClass: "",
            font: new rs.mimic.Font({ inherit: true }),
            foreColor: ""
        });

        // behavior
        Object.assign(properties, {
            blinkingState: new rs.mimic.VisualState(),
            clickAction: new rs.mimic.Action(),
            disabledState: new rs.mimic.VisualState(),
            hoverState: new rs.mimic.VisualState(),
            script: "",
            tooltip: ""
        });

        return properties;
    }

    _parseProperties(sourceProps) {
        const PropertyParser = rs.mimic.PropertyParser;
        let properties = super._parseProperties(sourceProps);
        sourceProps ??= {};

        // appearance
        Object.assign(properties, {
            backColor: PropertyParser.parseString(sourceProps.backColor),
            border: rs.mimic.Border.parse(sourceProps.border),
            cornerRadius: rs.mimic.CornerRadius.parse(sourceProps.cornerRadius),
            cssClass: PropertyParser.parseString(sourceProps.cssClass),
            font: rs.mimic.Font.parse(sourceProps.font),
            foreColor: PropertyParser.parseString(sourceProps.foreColor)
        });

        // behavior
        Object.assign(properties, {
            blinkingState: rs.mimic.VisualState.parse(sourceProps.blinkingState),
            clickAction: rs.mimic.Action.parse(sourceProps.clickAction),
            disabledState: rs.mimic.VisualState.parse(sourceProps.disabledState),
            hoverState: rs.mimic.VisualState.parse(sourceProps.hoverState),
            script: PropertyParser.parseString(sourceProps.script),
            tooltip: PropertyParser.parseString(sourceProps.tooltip)
        });

        return properties;
    }
};

// Creates components of the Text type.
rs.mimic.TextFactory = class extends rs.mimic.RegularComponentFactory {
    _createProperties() {
        let properties = super._createProperties();

        // appearance
        Object.assign(properties, {
            text: "Text",
            textAlign: rs.mimic.ContentAlignment.TOP_LEFT,
            textDirection: rs.mimic.TextDirection.HORIZONTAL,
            wordWrap: false
        });

        // layout
        Object.assign(properties, {
            autoSize: true,
            padding: new rs.mimic.Padding()
        });

        return properties;
    }

    _parseProperties(sourceProps) {
        const PropertyParser = rs.mimic.PropertyParser;
        let properties = super._parseProperties(sourceProps);
        sourceProps ??= {};

        // appearance
        Object.assign(properties, {
            text: PropertyParser.parseString(sourceProps.text),
            textAlign: PropertyParser.parseString(sourceProps.textAlign, rs.mimic.ContentAlignment.TOP_LEFT),
            textDirection: PropertyParser.parseString(sourceProps.textDirection, rs.mimic.TextDirection.HORIZONTAL),
            wordWrap: PropertyParser.parseBool(sourceProps.wordWrap)
        });

        // layout
        Object.assign(properties, {
            autoSize: PropertyParser.parseBool(sourceProps.autoSize),
            padding: rs.mimic.Padding.parse(sourceProps.padding)
        });

        return properties;
    }

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

    createComponent() {
        return super.createComponent("Text");
    }
};

// Creates components of the Picture type.
rs.mimic.PictureFactory = class extends rs.mimic.RegularComponentFactory {
    _createProperties() {
        let properties = super._createProperties();

        // appearance
        Object.assign(properties, {
            imageName: "",
            rotation: 0
        });

        // behavior
        Object.assign(properties, {
            conditions: new rs.mimic.ImageConditionList(),
            sizeMode: rs.mimic.ImageSizeMode.NORMAL
        });

        // layout
        Object.assign(properties, {
            padding: new rs.mimic.Padding()
        });

        return properties;
    }

    _parseProperties(sourceProps) {
        const PropertyParser = rs.mimic.PropertyParser;
        let properties = super._parseProperties(sourceProps);
        sourceProps ??= {};

        // appearance
        Object.assign(properties, {
            imageName: PropertyParser.parseString(sourceProps.imageName),
            rotation: PropertyParser.parseFloat(sourceProps.rotation)
        });

        // behavior
        Object.assign(properties, {
            conditions: rs.mimic.ImageConditionList.parse(sourceProps.conditions),
            sizeMode: PropertyParser.parseString(sourceProps.sizeMode, rs.mimic.ImageSizeMode.NORMAL)
        });

        // layout
        Object.assign(properties, {
            padding: rs.mimic.Padding.parse(sourceProps.padding)
        });

        return properties;
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
            let baseValue = faceplateInstance.getTargetPropertyValue(propertyExport);
            let sourceValue = sourceProps[propertyExport.name];

            if (sourceValue === undefined) {
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
        faceplateInstance.properties = this._createProperties();

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
