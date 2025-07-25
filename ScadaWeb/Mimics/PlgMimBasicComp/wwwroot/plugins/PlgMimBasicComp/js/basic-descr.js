// Contains descriptors for basic components.

rs.mimic.BasicLedDescriptor = class extends rs.mimic.RegularComponentDescriptor {
};

// Registers the descriptors. The function name must be unique.
function registerBasicDescriptors() {
    let componentDescriptors = rs.mimic.DescriptorSet.componentDescriptors;
    componentDescriptors.set("BasicLed", new rs.mimic.BasicLedDescriptor());
}

registerBasicDescriptors();
