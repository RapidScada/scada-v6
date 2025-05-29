// Contains classes: PropertyDescriptor, BasicType, Descriptor, MimicDescriptor, ComponentDescriptor,
//     TextDescriptor, PictureDescriptor, PanelDescriptor, DescriptorSet, TranslationSet
// Depends on mimic-common.js

// Provides meta information about a property of a mimic or component.
rs.mimic.PropertyDescriptor = class {
    name = "";
    displayName = "";
    category = "";
    isReadOnly = false;
    isBrowsable = true;
    isKnown = false;
    type = rs.mimic.BasicType.UNDEFINED;
    defaultValue = undefined;
    format = {}; // Tweakpane binding options

    constructor(source) {
        Object.assign(this, source);
    }
}

// Specifies the known categories.
rs.mimic.KnownCategory = class {
    static APPEARANCE = "appearance";
    static BEHAVIOR = "behavior";
    static DATA = "data";
    static DESIGN = "design";
    static LAYOUT = "layout";
    static MISC = "misc";
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
rs.mimic.ObjectDescriptor = class {
    // Key is a property name. Value is a property descriptor.
    propertyDescriptors = new Map();

    add(propertyDescriptor) {
        this.propertyDescriptors.set(propertyDescriptor.name, propertyDescriptor);
    }

    get(propertyName) {
        return this.propertyDescriptors.get(propertyName);
    }

    delete(propertyName) {
        this.propertyDescriptors.delete(propertyName);
    }
}

// Represents a mimic descriptor.
rs.mimic.MimicDescriptor = class extends rs.mimic.ObjectDescriptor {
    constructor() {
        const PropertyDescriptor = rs.mimic.PropertyDescriptor;
        const KnownCategory = rs.mimic.KnownCategory;
        const BasicType = rs.mimic.BasicType;
        super();

        this.add(new PropertyDescriptor({
            name: "size",
            displayName: "Size",
            category: KnownCategory.LAYOUT,
            type: BasicType.SIZE
        }));
    }

    // Adds missing properties to the mimic document and components.
    repair(mimic) {
        mimic.document ??= {};

        for (let propertyDescriptor of this.propertyDescriptors.values()) {
            if (!propertyDescriptor.isKnown && !mimic.document.hasOwnProperty(propertyDescriptor.name)) {
                mimic.document[propertyDescriptor.name] = propertyDescriptor.defaultValue;
            }
        }

        if (Array.isArray(mimic.components)) {
            for (let component of mimic.components) {
                let componentDescriptor = rs.mimic.DescriptorSet.componentDescriptors.get(component.typeName);

                if (componentDescriptor) {
                    componentDescriptor.repair(component);
                }
            }
        }
    }
}

// Represents a component descriptor.
rs.mimic.ComponentDescriptor = class extends rs.mimic.ObjectDescriptor {
    constructor() {
        const PropertyDescriptor = rs.mimic.PropertyDescriptor;
        const KnownCategory = rs.mimic.KnownCategory;
        const BasicType = rs.mimic.BasicType;
        super();

        this.add(new PropertyDescriptor({
            name: "id",
            displayName: "ID",
            category: KnownCategory.DESIGN,
            isReadOnly: true,
            isKnown: true,
            type: BasicType.INT
        }));

        this.add(new PropertyDescriptor({
            name: "name",
            displayName: "Name",
            category: KnownCategory.DESIGN,
            isKnown: true,
            type: BasicType.STRING
        }));

        this.add(new PropertyDescriptor({
            name: "typeName",
            displayName: "Type name",
            category: KnownCategory.DESIGN,
            isReadOnly: true,
            isKnown: true,
            type: BasicType.STRING
        }));

        this.add(new PropertyDescriptor({
            name: "location",
            displayName: "Location",
            category: KnownCategory.LAYOUT,
            type: BasicType.POINT,
            defaultValue: { x: "0", y: "0"}
        }));

        this.add(new PropertyDescriptor({
            name: "size",
            displayName: "Size",
            category: KnownCategory.LAYOUT,
            type: BasicType.SIZE,
            defaultValue: { width: "0", height: "0" }
        }));
    }

    // Adds missing properties to the component.
    repair(component) {
        component.properties ??= {};

        for (let propertyDescriptor of this.propertyDescriptors.values()) {
            if (!propertyDescriptor.isKnown && !component.properties.hasOwnProperty(propertyDescriptor.name)) {
                component.properties[propertyDescriptor.name] = propertyDescriptor.defaultValue;
            }
        }
    }
}

// Represents a text component descriptor.
rs.mimic.TextDescriptor = class extends rs.mimic.ComponentDescriptor {
    constructor() {
        const PropertyDescriptor = rs.mimic.PropertyDescriptor;
        const KnownCategory = rs.mimic.KnownCategory;
        const BasicType = rs.mimic.BasicType;
        super();

        this.add(new PropertyDescriptor({
            name: "text",
            displayName: "Text",
            category: KnownCategory.APPEARANCE,
            type: BasicType.STRING,
            defaultValue: ""
        }));
    }
}

// Represents a picture component descriptor.
rs.mimic.PictureDescriptor = class extends rs.mimic.ComponentDescriptor {
    constructor() {
        const PropertyDescriptor = rs.mimic.PropertyDescriptor;
        const KnownCategory = rs.mimic.KnownCategory;
        const BasicType = rs.mimic.BasicType;
        super();

        this.add(new PropertyDescriptor({
            name: "imageName",
            displayName: "Image",
            category: KnownCategory.APPEARANCE,
            type: BasicType.STRING,
            defaultValue: ""
        }));
    }
}

// Represents a panel component descriptor.
rs.mimic.PanelDescriptor = class extends rs.mimic.ComponentDescriptor {
    repair(component) {
        super.repair(component);
        component.children ??= []; // accept child components
    }
}

// Represents a faceplate descriptor.
rs.mimic.FaceplateDescriptor = class extends rs.mimic.ComponentDescriptor {
}

// Contains descriptors for a mimic and its components.
rs.mimic.DescriptorSet = class {
    static mimicDescriptor = new rs.mimic.MimicDescriptor();
    static faceplateDescriptor = new rs.mimic.FaceplateDescriptor();
    static componentDescriptors = new Map([
        ["Text", new rs.mimic.TextDescriptor()],
        ["Picture", new rs.mimic.PictureDescriptor()],
        ["Panel", new rs.mimic.PanelDescriptor()]
    ]);
}
