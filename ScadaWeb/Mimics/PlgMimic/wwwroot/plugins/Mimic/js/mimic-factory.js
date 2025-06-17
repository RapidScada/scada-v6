// Contains classes: ComponentFactory, TextFactory, PictureFactory, PanelFactory, FaceplateFactory, FactorySet
// Depends on mimic-model.js

// Parses property values ​​from strings.
rs.mimic.PropertyParser = class {

}

// Represents an abstract component factory.
rs.mimic.ComponentFactory = class ComponentFactory {
    static _parseBool(string, defaultValue = false) {
        return !string
            ? defaultValue
            : string === "true" || string === "True";
    }

    static _parseInt(string, defaultValue = 0) {
        let number = Number.parseInt(string);
        return Number.isFinite(number) ? number : defaultValue;
    }

    createComponent(typeName) {
        let component = new rs.mimic.Component();
        component.typeName = typeName;
        component.properties = {
            enabled: true,
            visible: true,
            location: { x: "0", y: "0" },
            size: { width: "100", height: "100" }
        };
        return component;
    }

    createComponent2(typeName) {
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
        let component = new rs.mimic.Component();
        let sourceProps = source.properties ?? {};
        component.typeName = source.typeName;
        component.properties = {
            // behavior
            blinking: ComponentFactory._parseBool(sourceProps.blinking),
            enabled: ComponentFactory._parseBool(sourceProps.enabled),
            visible: ComponentFactory._parseBool(sourceProps.visible),

            // data
            deviceNum: ComponentFactory._parseInt(sourceProps.deviceNum),
            inCnlNum: ComponentFactory._parseInt(sourceProps.inCnlNum),
            objNum: ComponentFactory._parseInt(sourceProps.objNum),
            outCnlNum: ComponentFactory._parseInt(sourceProps.outCnlNum),
            propertyBindings: [],

            // layout
            location: new rs.mimic.Location(),
            size: new rs.mimic.Size()
        };
        return component;
    }
};

// Represents an abstract factory for regular non-faceplate components.
rs.mimic.RegularComponentFactory = class extends rs.mimic.ComponentFactory {

};

// Creates components of the Text type.
rs.mimic.TextFactory = class extends rs.mimic.ComponentFactory {
    createComponent() {
        let component = super.createComponent("Text");
        component.properties.text = "Text";
        return component;
    }

    createComponent2() {
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
            padding: rs.mimic.Padding()
        });

        return component;
    }
};

// Creates components of the Picture type.
rs.mimic.PictureFactory = class extends rs.mimic.ComponentFactory {
    createComponent() {
        return super.createComponent("Picture");
    }
};

// Creates components of the Panel type.
rs.mimic.PanelFactory = class extends rs.mimic.ComponentFactory {
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
