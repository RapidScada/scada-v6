// Contains descriptors for basic components.

rs.mimic.BasicSubtype = class {
    // Enumerations
    static TOGGLE_POSITION = "BasicTogglePosition";

    // Structures
    static COLOR_CONDITION = "BasicColorCondition";
};

rs.mimic.BasicButtonDescriptor = class extends rs.mimic.RegularComponentDescriptor {
    constructor() {
        super();
        const KnownCategory = rs.mimic.KnownCategory;
        const BasicType = rs.mimic.BasicType;
        const Subtype = rs.mimic.Subtype;
        const PropertyEditor = rs.mimic.PropertyEditor;
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
        const PropertyEditor = rs.mimic.PropertyEditor;
        const PropertyDescriptor = rs.mimic.PropertyDescriptor;

        // appearance
        this.add(new PropertyDescriptor({
            name: "borderOpacity",
            displayName: "Border opacity",
            category: KnownCategory.APPEARANCE,
            type: BasicType.INT
        }));

        this.add(new PropertyDescriptor({
            name: "isSquare",
            displayName: "Square",
            category: KnownCategory.APPEARANCE,
            type: BasicType.BOOL
        }));

        // behavior
        this.add(new PropertyDescriptor({
            name: "conditions",
            displayName: "Conditions",
            category: KnownCategory.BEHAVIOR,
            type: BasicType.LIST,
            subtype: BasicSubtype.COLOR_CONDITION
        }));

        this.add(new PropertyDescriptor({
            name: "defaultColor",
            displayName: "Default color",
            category: KnownCategory.BEHAVIOR,
            type: BasicType.STRING,
            editor: PropertyEditor.COLOR_DIALOG
        }));
    }
};

rs.mimic.BasicToggleDescriptor = class extends rs.mimic.RegularComponentDescriptor {
    constructor() {
        super();
        const KnownCategory = rs.mimic.KnownCategory;
        const BasicType = rs.mimic.BasicType;
        const Subtype = rs.mimic.Subtype;
        const BasicSubtype = rs.mimic.BasicSubtype;
        const PropertyDescriptor = rs.mimic.PropertyDescriptor;

        // appearance
        this.add(new PropertyDescriptor({
            name: "position",
            displayName: "Position",
            category: KnownCategory.APPEARANCE,
            type: BasicType.ENUM,
            subtype: BasicSubtype.TOGGLE_POSITION
        }));

        // layout
        this.add(new PropertyDescriptor({
            name: "padding",
            displayName: "Padding",
            category: KnownCategory.LAYOUT,
            type: BasicType.STRUCT,
            subtype: Subtype.PADDING
        }));
    }
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
