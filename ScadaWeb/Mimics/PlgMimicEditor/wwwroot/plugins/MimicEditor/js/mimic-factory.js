// Contains classes: ComponentFactory
// Depends on mimic-model.js

// Represents an abstract component factory.
rs.mimic.ComponentFactory = class {
    createComponent(typeName) {
        let component = new rs.mimic.Component();
        component.typeName = typeName;
        component.properties = {
            location: { x: "0", y: "0" },
            size: { width: "100", height: "100" }
        };
        return component;
    }
}

// Creates components of the Text type.
rs.mimic.TextFactory = class extends rs.mimic.ComponentFactory {
    createComponent() {
        let component = super.createComponent("Text");
        component.properties.text = "Text";
        return component;
    }
}

// Creates components of the Picture type.
rs.mimic.PictureFactory = class extends rs.mimic.ComponentFactory {
    createComponent() {
        return super.createComponent("Picture");
    }
}

// Creates components of the Panel type.
rs.mimic.PanelFactory = class extends rs.mimic.ComponentFactory {
    createComponent() {
        let component = super.createComponent("Panel");
        component.children = [];
        return component;
    }
}

// Creates faceplates.
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
}

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
}
