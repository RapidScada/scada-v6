// Contains factories for basic components.

rs.mimic.BasicLedFactory = class extends rs.mimic.RegularComponentFactory {
    createProperties() {
        let properties = super.createProperties();

        // behavior
        Object.assign(properties, {
            conditions: new rs.mimic.BasicColorConditionList()
        });

        return properties;
    }

    parseProperties(sourceProps) {
        let properties = super.parseProperties(sourceProps);
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

// Registers the factories. The function name must be unique.
function registerBasicFactories() {
    let componentFactories = rs.mimic.FactorySet.componentFactories;
    componentFactories.set("BasicLed", new rs.mimic.BasicLedFactory());
}

registerBasicFactories();
