// Contains descriptors for basic components.

rs.mimic.BasicLedDescriptor = class extends rs.mimic.RegularComponentDescriptor {
    constructor() {
        super();
        const KnownCategory = rs.mimic.KnownCategory;
        const BasicType = rs.mimic.BasicType;
        const BasicSubtype = rs.mimic.BasicSubtype;
        const PropertyDescriptor = rs.mimic.PropertyDescriptor;

        // behavior
        this.add(new PropertyDescriptor({
            name: "conditions",
            displayName: "Conditions",
            category: KnownCategory.BEHAVIOR,
            type: BasicType.LIST,
            subtype: BasicSubtype.COLOR_CONDITION
        }));
    }
};

// Registers the descriptors. The function name must be unique.
function registerBasicDescriptors() {
    let componentDescriptors = rs.mimic.DescriptorSet.componentDescriptors;
    componentDescriptors.set("BasicLed", new rs.mimic.BasicLedDescriptor());
}

registerBasicDescriptors();
