// Contains factories for basic components.

rs.mimic.BasicButtonFactory = class extends rs.mimic.RegularComponentFactory {
    createProperties() {
        let properties = super.createProperties();

        // appearance
        Object.assign(properties, {
            imageName: "",
            imageSize: new rs.mimic.Size({ width: 16, height: 16 }),
            text: "Button",
        });

        return properties;
    }

    parseProperties(sourceProps) {
        const PropertyParser = rs.mimic.PropertyParser;
        let properties = super.parseProperties(sourceProps);
        sourceProps ??= {};

        // appearance
        Object.assign(properties, {
            imageName: PropertyParser.parseString(sourceProps.imageName),
            imageSize: rs.mimic.Size.parse(sourceProps.imageSize),
            text: PropertyParser.parseString(sourceProps.text),
        });

        return properties;
    }
};

rs.mimic.BasicLedFactory = class extends rs.mimic.RegularComponentFactory {
    createProperties() {
        let properties = super._createProperties();

        // behavior
        Object.assign(properties, {
            conditions: new rs.mimic.BasicColorConditionList()
        });

        return properties;
    }

    parseProperties(sourceProps) {
        let properties = super._parseProperties(sourceProps);
        sourceProps ??= {};

        // behavior
        Object.assign(properties, {
            conditions: rs.mimic.BasicColorConditionList.parse(sourceProps.conditions)
        });

        return properties;
    }

    createComponent() {
        return super.createComponent("BasicLed");
    }
};

rs.mimic.BasicToggleFactory = class extends rs.mimic.RegularComponentFactory {
};

// Registers the factories. The function name must be unique.
function registerBasicFactories() {
    let componentFactories = rs.mimic.FactorySet.componentFactories;
    componentFactories.set("BasicButton", new rs.mimic.BasicButtonFactory());
    componentFactories.set("BasicLed", new rs.mimic.BasicLedFactory());
    componentFactories.set("BasicToggle", new rs.mimic.BasicToggleFactory());
}

registerBasicFactories();
