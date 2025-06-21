// Contains classes: MimicFactory, ComponentFactory, RegularComponentFactory,
//     TextFactory, PictureFactory, PanelFactory, FaceplateFactory, FactorySet
// Depends on mimic-model.js, mimic-model-subtypes.js

// Create mimic properties.
rs.mimic.MimicFactory = class {
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
            propertyExports: PropertyParser.parsePropertyExports(sourceProps.propertyExports),

            // layout
            size: rs.mimic.Size.parse(sourceProps.size)
        };
    }
}

// Represents an abstract component factory.
rs.mimic.ComponentFactory = class ComponentFactory {
    static _createProperties() {
        return {
            // behavior
            blinking: false,
            enabled: true,
            visible: true,

            // data
            deviceNum: 0,
            inCnlNum: 0,
            objNum: 0,
            outCnlNum: 0,
            propertyBindings: [],

            // design
            id: 0,
            name: "",
            typeName: "",

            // layout
            location: new rs.mimic.Point(),
            size: new rs.mimic.Size()
        };
    }

    static _parseProperties(sourceProps) {
        const PropertyParser = rs.mimic.PropertyParser;
        sourceProps ??= {};
        return {
            // behavior
            blinking: PropertyParser.parseBool(sourceProps.blinking),
            enabled: PropertyParser.parseBool(sourceProps.enabled),
            visible: PropertyParser.parseBool(sourceProps.visible),

            // data
            deviceNum: PropertyParser.parseInt(sourceProps.deviceNum),
            inCnlNum: PropertyParser.parseInt(sourceProps.inCnlNum),
            objNum: PropertyParser.parseInt(sourceProps.objNum),
            outCnlNum: PropertyParser.parseInt(sourceProps.outCnlNum),
            propertyBindings: PropertyParser.parsePropertyBindings(sourceProps.propertyBindings),

            // design
            id: PropertyParser.parseInt(sourceProps.id),
            name: PropertyParser.parseString(sourceProps.name),
            typeName: "",

            // layout
            location: rs.mimic.Point.parse(sourceProps.location),
            size: rs.mimic.Size.parse(sourceProps.size)
        };
    }

    static _copyProperties(component, source) {
        component.id = source.id;
        component.typeName = source.typeName;
        component.properties = ComponentFactory._parseProperties(source.properties);
        component.properties.typeName = source.typeName;
        component.parentID = source.parentID;
    }

    // Creates a new component with the given type name.
    createComponent(typeName) {
        let component = new rs.mimic.Component();
        component.typeName = typeName;
        component.properties = ComponentFactory._createProperties(typeName);
        component.properties.typeName = typeName;
        return component;
    }

    // Creates a new component with the specified properties, making deep copies of the source properties.
    createComponentFromSource(source) {
        let component = new rs.mimic.Component();
        ComponentFactory._copyProperties(component, source);
        return component;
    }
};

// Represents an abstract factory for regular non-faceplate components.
rs.mimic.RegularComponentFactory = class extends rs.mimic.ComponentFactory {
    createComponent(typeName) {
        let component = super.createComponent(typeName);

        // appearance
        Object.assign(component.properties, {
            backColor: "",
            border: new rs.mimic.Border(),
            cornerRadius: new rs.mimic.CornerRadius(),
            cssClass: "",
            foreColor: ""
        });

        // behavior
        Object.assign(component.properties, {
            blinkingState: new rs.mimic.VisualState(),
            clickAction: new rs.mimic.Action(),
            disabledState: new rs.mimic.VisualState(),
            hoverState: new rs.mimic.VisualState(),
            script: "",
            tooltip: ""
        });

        return component;
    }

    createComponentFromSource(source) {
        const PropertyParser = rs.mimic.PropertyParser;
        let component = super.createComponentFromSource(source);
        let sourceProps = source.properties ?? {};

        // appearance
        Object.assign(component.properties, {
            backColor: PropertyParser.parseString(sourceProps.backColor),
            border: rs.mimic.Border.parse(sourceProps.border),
            cornerRadius: rs.mimic.CornerRadius.parse(sourceProps.cornerRadius),
            cssClass: PropertyParser.parseString(sourceProps.cssClass),
            foreColor: PropertyParser.parseString(sourceProps.foreColor)
        });

        // behavior
        Object.assign(component.properties, {
            blinkingState: rs.mimic.VisualState.parse(sourceProps.blinkingState),
            clickAction: rs.mimic.Action.parse(sourceProps.clickAction),
            disabledState: rs.mimic.VisualState.parse(sourceProps.disabledState),
            hoverState: rs.mimic.VisualState.parse(sourceProps.hoverState),
            script: PropertyParser.parseString(sourceProps.script),
            tooltip: PropertyParser.parseString(sourceProps.tooltip)
        });

        return component;
    }
};

// Creates components of the Text type.
rs.mimic.TextFactory = class extends rs.mimic.RegularComponentFactory {
    createComponent() {
        let component = super.createComponent("Text");

        // appearance
        Object.assign(component.properties, {
            font: new rs.mimic.Font(),
            text: "Text",
            textAlign: rs.mimic.ContentAlignment.TOP_LEFT,
            wordWrap: false
        });

        // layout
        Object.assign(component.properties, {
            autoSize: false,
            padding: new rs.mimic.Padding()
        });

        return component;
    }

    createComponentFromSource(source) {
        const PropertyParser = rs.mimic.PropertyParser;
        let component = super.createComponentFromSource(source);
        let sourceProps = source.properties ?? {};

        // appearance
        Object.assign(component.properties, {
            font: rs.mimic.Font.parse(sourceProps.font),
            text: PropertyParser.parseString(sourceProps.text, "Text"),
            textAlign: PropertyParser.parseString(sourceProps.textAlign, rs.mimic.ContentAlignment.TOP_LEFT),
            wordWrap: PropertyParser.parseBool(sourceProps.wordWrap)
        });

        // layout
        Object.assign(component.properties, {
            autoSize: PropertyParser.parseBool(sourceProps.autoSize),
            padding: rs.mimic.Padding.parse(sourceProps.padding)
        });

        return component;
    }
};

// Creates components of the Picture type.
rs.mimic.PictureFactory = class extends rs.mimic.RegularComponentFactory {
    createComponent() {
        let component = super.createComponent("Picture");

        // appearance
        Object.assign(component.properties, {
            imageName: ""
        });

        // behavior
        Object.assign(component.properties, {
            conditions: [],
            sizeMode: rs.mimic.ImageSizeMode.NORMAL
        });

        // layout
        Object.assign(component.properties, {
            padding: new rs.mimic.Padding()
        });

        return component;
    }

    createComponentFromSource(source) {
        const PropertyParser = rs.mimic.PropertyParser;
        let component = super.createComponentFromSource(source);
        let sourceProps = source.properties ?? {};

        // appearance
        Object.assign(component.properties, {
            imageName: PropertyParser.parseString(sourceProps.imageName)
        });

        // behavior
        Object.assign(component.properties, {
            conditions: PropertyParser.parseImageConditions(sourceProps.conditions),
            sizeMode: PropertyParser.parseString(sourceProps.sizeMode, rs.mimic.ImageSizeMode.NORMAL)
        });

        // layout
        Object.assign(component.properties, {
            padding: rs.mimic.Padding.parse(sourceProps.padding)
        });

        return component;
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
    createComponent(faceplate) {
        let component = new rs.mimic.FaceplateInstance();
        component.properties = this._createProperties();

        if (faceplate) {
            component.typeName = component.properties.typeName = faceplate.typeName;
            component.applyModel(faceplate);
        }

        return component;
    }

    createComponentFromSource(source, faceplate) {
        let component = new rs.mimic.FaceplateInstance();
        rs.mimic.ComponentFactory._copyProperties(component, source);

        if (faceplate) {
            component.applyModel(faceplate);
        }

        return component;
    }
};

// Contains factories for mimic components.
rs.mimic.FactorySet = class FactorySet {
    static faceplateFactory = new rs.mimic.FaceplateFactory();
    static componentFactories = new Map([
        ["Text", new rs.mimic.TextFactory()],
        ["Picture", new rs.mimic.PictureFactory()],
        ["Panel", new rs.mimic.PanelFactory()]
    ]);
    static getFaceplateFactory(faceplate) {
        return {
            createComponent: () =>
                FactorySet.faceplateFactory.createComponent(faceplate),
            createComponentFromSource: (source) =>
                FactorySet.faceplateFactory.createComponentFromSource(source, faceplate)
        };
    }
};
