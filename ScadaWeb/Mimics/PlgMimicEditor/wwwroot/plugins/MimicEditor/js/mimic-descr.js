// Contains classes: PropertyDescriptor, BasicType, Descriptor, MimicDescriptor, ComponentDescriptor,
//     TextDescriptor, PictureDescriptor, PanelDescriptor, DescriptorSet, TranslationSet
// Depends on mimic-common.js

// Provides meta information about a property of a mimic or component.
rs.mimic.PropertyDescriptor = class {
    name = "";
    displayName = "";
    category = "";
    isReadOnly = false;
    defaultValue = undefined;
    type = rs.mimic.BasicType.UNDEFINED;
    format = {}; // Tweakpane binding options

    constructor(source) {
        Object.assign(this, source);
    }
}

// Specifies the known categories.
rs.mimic.KnownCategory = class {
    static APPEARANCE = "APPEARANCE";
    static BEHAVIOR = "BEHAVIOR";
    static DATA = "DATA";
    static DESIGN = "DESIGN";
    static LAYOUT = "LAYOUT";
    static MISC = "MISC";
}

// Specifies the basic types.
rs.mimic.BasicType = class {
    static UNDEFINED = 0;
    static BOOL = 1;
    static COLOR = 2;
    static ENUM = 3;
    static FLOAT = 4;
    static INT = 5;
    static POINT = 6;
    static SIZE = 7;
    static STRING = 8;
}

// Represents a descriptor of a mimic or component.
rs.mimic.Descriptor = class {
    // Key is a property name. Value is a property descriptor.
    propertyDescriptors = new Map();

    _add(propertyDescriptor) {
        this.propertyDescriptors.set(propertyDescriptor.name, propertyDescriptor);
    }

    get(propertyName) {
        return this.propertyDescriptors.get(propertyName);
    }
}

// Represents a mimic descriptor.
rs.mimic.MimicDescriptor = class extends rs.mimic.Descriptor {
    constructor() {
        const PropertyDescriptor = rs.mimic.PropertyDescriptor;
        super();

        this._add(new PropertyDescriptor({
            name: "size",
            displayName: "Size",
            category: rs.mimic.KnownCategory.LAYOUT,
            type: rs.mimic.BasicType.SIZE
        }));
    }
}

// Represents a component descriptor.
rs.mimic.ComponentDescriptor = class extends rs.mimic.Descriptor {
    constructor() {
        const PropertyDescriptor = rs.mimic.PropertyDescriptor;
        super();

        this._add(new PropertyDescriptor({
            name: "id",
            displayName: "ID",
            category: rs.mimic.KnownCategory.DESIGN,
            isReadOnly: true,
            type: rs.mimic.BasicType.INT
        }));

        this._add(new PropertyDescriptor({
            name: "name",
            displayName: "Name",
            category: rs.mimic.KnownCategory.DESIGN,
            type: rs.mimic.BasicType.STRING
        }));

        this._add(new PropertyDescriptor({
            name: "location",
            displayName: "Location",
            category: rs.mimic.KnownCategory.LAYOUT,
            type: rs.mimic.BasicType.POINT
        }));

        this._add(new PropertyDescriptor({
            name: "size",
            displayName: "Size",
            category: rs.mimic.KnownCategory.LAYOUT,
            type: rs.mimic.BasicType.SIZE
        }));
    }
}

// Represents a text component descriptor.
rs.mimic.TextDescriptor = class extends rs.mimic.ComponentDescriptor {
    constructor() {
        const PropertyDescriptor = rs.mimic.PropertyDescriptor;
        super();

        this._add(new PropertyDescriptor({
            name: "text",
            displayName: "Text",
            category: rs.mimic.KnownCategory.APPEARANCE,
            type: rs.mimic.BasicType.STRING
        }));
    }
}

// Represents a picture component descriptor.
rs.mimic.PictureDescriptor = class extends rs.mimic.ComponentDescriptor {
    constructor() {
        const PropertyDescriptor = rs.mimic.PropertyDescriptor;
        super();

        this._add(new PropertyDescriptor({
            name: "imageName",
            displayName: "Image name",
            category: rs.mimic.KnownCategory.APPEARANCE,
            type: rs.mimic.BasicType.STRING
        }));
    }
}

// Represents a panel component descriptor.
rs.mimic.PanelDescriptor = class extends rs.mimic.ComponentDescriptor {
}

// Contains descriptors for a mimic and its components.
rs.mimic.DescriptorSet = class {
    static mimicDescriptor = new rs.mimic.MimicDescriptor();
    static componentDescriptors = new Map([
        ["Text", new rs.mimic.TextDescriptor()],
        ["Picture", new rs.mimic.PictureDescriptor()],
        ["Panel", new rs.mimic.PanelDescriptor()]
    ]);
}

// Contains translations for a mimic and its components.
rs.mimic.TranslationSet = class {
    // Key is a property name. Value is a display name.
    static mimicTranslation = new Map();
    // Key is a component type name. Value is a map containing property names and their display names.
    static componentTranslations = new Map();
}
