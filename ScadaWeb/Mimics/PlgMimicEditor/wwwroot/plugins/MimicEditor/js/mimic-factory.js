// Contains classes: ComponentFactory
// Depends on mimic-model.js

// Represents an abstract component factory.
rs.mimic.ComponentFactory = class {
    createComponent(typeName) {
        let component = new rs.mimic.Component();
        component.typeName = typeName;
        this.setProperties(component);
        return component;
    }

    setProperties(component) {
        component.properties = {
            location: { x: "0", y: "0" },
            size: { width: "100", height: "100" }
        };
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
        let component = super.createComponent("Picture");
        component.properties.imageName = "";
        return component;
    }
}

// Creates components of the Panel type.
rs.mimic.PanelFactory = class extends rs.mimic.ComponentFactory {
    createComponent() {
        let component = new rs.mimic.Panel();
        component.typeName = "Panel";
        this.setProperties(component);
        return component;
    }
}

// Contains factories for mimic components.
rs.mimic.FactorySet = class {
    static componentFactories = new Map([
        ["Text", new rs.mimic.TextFactory()],
        ["Picture", new rs.mimic.PictureFactory()],
        ["Panel", new rs.mimic.PanelFactory()]
    ]);
}
