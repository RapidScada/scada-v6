// Contains descriptors for basic components.

rs.mimic.BasicSubtype = class {
    static COLOR_CONDITION = "BasicColorCondition";
};

rs.mimic.BasicButtonDescriptor = class extends rs.mimic.RegularComponentDescriptor {
    constructor() {
        super();
        const KnownCategory = rs.mimic.KnownCategory;
        const BasicType = rs.mimic.BasicType;
        const PropertyDescriptor = rs.mimic.PropertyDescriptor;

        // appearance
        this.add(new PropertyDescriptor({
            name: "imageName",
            displayName: "Image",
            category: KnownCategory.APPEARANCE,
            type: BasicType.STRING,
            editor: PropertyEditor.IMAGE_DIALOG
        }));

        this.add(new PropertyDescriptor({
            name: "imageSize",
            displayName: "Image size",
            category: KnownCategory.APPEARANCE,
            type: BasicType.STRUCT,
            subtype: Subtype.SIZE
        }));

        this.add(new PropertyDescriptor({
            name: "text",
            displayName: "Text",
            category: KnownCategory.APPEARANCE,
            type: BasicType.STRING
        }));
    }
};

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

rs.mimic.BasicToggleDescriptor = class extends rs.mimic.RegularComponentDescriptor {
};

rs.mimic.BasicColorConditionDescriptor = class extends rs.mimic.ConditionDescriptor {
    constructor() {
        super();
        const BasicType = rs.mimic.BasicType;
        const PropertyEditor = rs.mimic.PropertyEditor;
        const PropertyDescriptor = rs.mimic.PropertyDescriptor;

        this.add(new PropertyDescriptor({
            name: "color",
            displayName: "Color",
            type: BasicType.STRING,
            editor: PropertyEditor.COLOR_DIALOG
        }));
    }
}

// Registers the descriptors. The function name must be unique.
function registerBasicDescriptors() {
    let componentDescriptors = rs.mimic.DescriptorSet.componentDescriptors;
    componentDescriptors.set("BasicButton", new rs.mimic.BasicButtonDescriptor());
    componentDescriptors.set("BasicLed", new rs.mimic.BasicLedDescriptor());
    componentDescriptors.set("BasicToggle", new rs.mimic.BasicToggleDescriptor());

    let structureDescriptors = rs.mimic.DescriptorSet.structureDescriptors;
    structureDescriptors.set("BasicColorCondition", new rs.mimic.BasicColorConditionDescriptor);
}

registerBasicDescriptors();
