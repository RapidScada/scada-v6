// Contains classes: ComponentFactory
// Depends on mimic-mode.js

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

// Creates text components.
rs.mimic.TextFactory = class extends rs.mimic.ComponentFactory {
    createComponent() {
        let component = super.createComponent("Text");
        component.properties.text = "Text";
        return component;
    }
}

// Contains factories for mimic components.
rs.mimic.FactorySet = class {
    static componentFactories = new Map([
        ["Text", new rs.mimic.TextFactory()],
    ]);
}
