// Contains factories for basic components.

rs.mimic.BasicLedFactory = class extends rs.mimic.RegularComponentFactory {
};

// Registers the factories. The function name must be unique.
function registerBasicFactories() {
    let componentFactories = rs.mimic.FactorySet.componentFactories;
    componentFactories.set("BasicLed", new rs.mimic.BasicLedFactory());
}

registerBasicFactories();
