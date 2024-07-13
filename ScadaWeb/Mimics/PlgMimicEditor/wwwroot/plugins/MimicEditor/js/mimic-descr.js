// Contains classes: PropertyDescriptor, BasicType, TypeDescriptor, Descriptor, MimicDescriptor, ComponentDescriptor,
//     TextDescriptor, PictureDescriptor, PanelDescriptor, DescriptorSet, Translation
// Depends on mimic-common.js

// Provides meta information about a property of a mimic or component.
rs.mimic.PropertyDescriptor = class {
    name = "";
    displayName = "";
    isReadOnly = false;
    defaultValue = undefined;
    type = rs.mimic.TypeDescriptor.STRING;

    constructor(source) {
        Object.assign(this, source);
    }
}

// Specifies the basic types.
rs.mimic.BasicType = class {
    static UNDEFINED = 0;
    static BOOL = 1;
    static ENUM = 2;
    static FLOAT = 3;
    static INT = 4;
    static POINT = 5;
    static SIZE = 6;
    static STRING = 7;
}

// Provides meta information about a property type and format.
rs.mimic.TypeDescriptor = class {
    static BOOL;
    static FLOAT;
    static INT;
    static POINT;
    static SIZE;
    static STRING;

    basicType;
    format; // Tweakpane binding options

    static {
        const BasicType = rs.mimic.BasicType;
        const TypeDescriptor = this;

        TypeDescriptor.BOOL = new TypeDescriptor(BasicType.BOOL);
        TypeDescriptor.FLOAT = new TypeDescriptor(BasicType.FLOAT);
        TypeDescriptor.INT = new TypeDescriptor(BasicType.INT);
        TypeDescriptor.POINT = new TypeDescriptor(BasicType.POINT);
        TypeDescriptor.SIZE = new TypeDescriptor(BasicType.SIZE);
        TypeDescriptor.STRING = new TypeDescriptor(BasicType.STRING);
    }

    constructor(basicType, opt_format) {
        this.basicType = basicType;
        this.format = opt_format;
    }
}

// Represents a descriptor of a mimic or component.
rs.mimic.Descriptor = class {
    propertyDescriptors = [];
}

// Represents a mimic descriptor.
rs.mimic.MimicDescriptor = class extends rs.mimic.Descriptor {
    constructor() {
        const PropertyDescriptor = rs.mimic.PropertyDescriptor;
        super();

        this.propertyDescriptors.push(new PropertyDescriptor({
            name: "size",
            displayName: "Size",
            type: rs.mimic.TypeDescriptor.SIZE
        }));
    }
}

// Represents a component descriptor.
rs.mimic.ComponentDescriptor = class extends rs.mimic.Descriptor {
    constructor() {
        const PropertyDescriptor = rs.mimic.PropertyDescriptor;
        super();

        this.propertyDescriptors.push(new PropertyDescriptor({
            name: "id",
            displayName: "ID",
            isReadOnly: true,
            type: rs.mimic.TypeDescriptor.INT
        }));

        this.propertyDescriptors.push(new PropertyDescriptor({
            name: "name",
            displayName: "Name",
            type: rs.mimic.TypeDescriptor.STRING
        }));

        this.propertyDescriptors.push(new PropertyDescriptor({
            name: "location",
            displayName: "Location",
            type: rs.mimic.TypeDescriptor.POINT
        }));

        this.propertyDescriptors.push(new PropertyDescriptor({
            name: "size",
            displayName: "Size",
            type: rs.mimic.TypeDescriptor.SIZE
        }));
    }
}

// Represents a text component descriptor.
rs.mimic.TextDescriptor = class extends rs.mimic.ComponentDescriptor {
    constructor() {
        const PropertyDescriptor = rs.mimic.PropertyDescriptor;
        super();

        this.propertyDescriptors.push(new PropertyDescriptor({
            name: "text",
            displayName: "Text",
            type: rs.mimic.TypeDescriptor.STRING
        }));
    }
}

// Represents a picture component descriptor.
rs.mimic.PictureDescriptor = class extends rs.mimic.ComponentDescriptor {
    constructor() {
        const PropertyDescriptor = rs.mimic.PropertyDescriptor;
        super();

        this.propertyDescriptors.push(new PropertyDescriptor({
            name: "imageName",
            displayName: "Image name",
            type: rs.mimic.TypeDescriptor.STRING
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

// Collects component translations.
rs.mimic.Translation = class {
    // Key is a property name. Value is a display name.
    static mimicTranslation = new Map();
    // Key is a component type name. Value is a map containing property names and their display names.
    static componentTranslations = new Map();
}
