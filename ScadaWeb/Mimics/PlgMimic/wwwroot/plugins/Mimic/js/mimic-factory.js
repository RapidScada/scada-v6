// Contains classes: ComponentFactory, RegularComponentFactory, TextFactory, PictureFactory, PanelFactory,
//     FaceplateFactory, FactorySet
// Depends on mimic-model.js, mimic-model-subtypes.js

// Represents an abstract component factory.
rs.mimic.ComponentFactory = class ComponentFactory {
    createComponent(typeName) {
        let component = new rs.mimic.Component();
        component.typeName = typeName;
        component.properties = {
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

            // layout
            location: new rs.mimic.Location(),
            size: new rs.mimic.Size()
        };
        return component;
    }

    createComponentFromSource(source) {
        const PropertyParser = rs.mimic.PropertyParser;
        let component = new rs.mimic.Component();
        let sourceProps = source.properties ?? {};
        component.typeName = source.typeName;
        component.properties = {
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

            // layout
            location: rs.mimic.Location.parse(sourceProps.location),
            size: rs.mimic.Size.parse(sourceProps.size)
        };
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
        return super.createComponent("Picture");
    }
};

// Creates components of the Panel type.
rs.mimic.PanelFactory = class extends rs.mimic.RegularComponentFactory {
    createComponent() {
        let component = super.createComponent("Panel");
        component.children = [];
        return component;
    }
};

// Creates faceplate instances.
rs.mimic.FaceplateFactory = class {
    static createComponent(faceplate) {
        let faceplateInstance = new rs.mimic.FaceplateInstance();
        faceplateInstance.typeName = faceplate.typeName;
        faceplateInstance.properties = {
            location: { x: "0", y: "0" }
        };
        faceplateInstance.applyModel(faceplate);
        return faceplateInstance;
    }
};

// Contains factories for mimic components.
rs.mimic.FactorySet = class {
    static componentFactories = new Map([
        ["Text", new rs.mimic.TextFactory()],
        ["Picture", new rs.mimic.PictureFactory()],
        ["Panel", new rs.mimic.PanelFactory()]
    ]);

    static getFaceplateFactory(faceplate) {
        return {
            createComponent: function () {
                return rs.mimic.FaceplateFactory.createComponent(faceplate);
            }
        };
    }
};
