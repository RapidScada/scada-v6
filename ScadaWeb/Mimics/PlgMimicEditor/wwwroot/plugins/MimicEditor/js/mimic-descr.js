// Contains classes: BasicType, KnownCategory, PropertyDescriptor, ObjectDescriptor, MimicDescriptor,
//     ComponentDescriptor, TextDescriptor, PictureDescriptor, PanelDescriptor, DescriptorSet
// Depends on mimic-common.js

// Specifies the basic types.
rs.mimic.BasicType = class BasicType {
    static UNDEFINED = "undefined";
    static BOOL = "bool";
    static COLOR = "color";
    static ENUM = "enum";
    static FLOAT = "float";
    static INT = "int";
    static OBJECT = "object";
    static POINT = "point";
    static SIZE = "size";
    static STRING = "string";

    static getDefaultValue(basicType) {
        switch (basicType) {
            case BasicType.BOOL:
                return false;

            case BasicType.COLOR:
                return "";

            case BasicType.ENUM:
                return 0;

            case BasicType.FLOAT:
                return 0;

            case BasicType.INT:
                return 0;

            case BasicType.OBJECT:
                return {};

            case BasicType.POINT:
                return { x: "0", y: "0" };

            case BasicType.SIZE:
                return { width: "0", height: "0" };

            case BasicType.STRING:
                return "";

            default:
                return undefined;
        }
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

    // Adds the missing property to the properties.
    repair(properties) {
        if (!this.isKnown && !properties.hasOwnProperty(this.name)) {
            properties[this.name] = this.defaultValue === undefined
                ? rs.mimic.BasicType.getDefaultValue(this.type)
                : this.defaultValue;
        }
    }
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
        const BasicType = rs.mimic.BasicType;
        const KnownCategory = rs.mimic.KnownCategory;
        const PropertyDescriptor = rs.mimic.PropertyDescriptor;
        super();

        // appearance
        this.add(new PropertyDescriptor({
            name: "backColor",
            displayName: "Background color",
            category: KnownCategory.APPEARANCE,
            type: BasicType.COLOR
        }));

        this.add(new PropertyDescriptor({
            name: "foreColor",
            displayName: "Foreground color",
            category: KnownCategory.APPEARANCE,
            type: BasicType.COLOR
        }));

        this.add(new PropertyDescriptor({
            name: "stylesheet",
            displayName: "Stylesheet",
            category: KnownCategory.APPEARANCE,
            type: BasicType.STRING
        }));

        // behavior
        this.add(new PropertyDescriptor({
            name: "script",
            displayName: "Script",
            category: KnownCategory.BEHAVIOR,
            type: BasicType.STRING
        }));

        // layout
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
            propertyDescriptor.repair(mimic.document);
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
        const BasicType = rs.mimic.BasicType;
        const KnownCategory = rs.mimic.KnownCategory;
        const PropertyDescriptor = rs.mimic.PropertyDescriptor;
        super();

        // appearance
        this.add(new PropertyDescriptor({
            name: "backColor",
            displayName: "Background color",
            category: KnownCategory.APPEARANCE,
            type: BasicType.COLOR
        }));

        this.add(new PropertyDescriptor({
            name: "border",
            displayName: "Border",
            category: KnownCategory.APPEARANCE,
            type: BasicType.OBJECT
        }));

        this.add(new PropertyDescriptor({
            name: "cornerRadius",
            displayName: "Corner radius",
            category: KnownCategory.APPEARANCE,
            type: BasicType.OBJECT
        }));

        this.add(new PropertyDescriptor({
            name: "cssClass",
            displayName: "CSS class",
            category: KnownCategory.APPEARANCE,
            type: BasicType.STRING
        }));

        this.add(new PropertyDescriptor({
            name: "cssStyle",
            displayName: "CSS style",
            category: KnownCategory.APPEARANCE,
            type: BasicType.STRING
        }));

        this.add(new PropertyDescriptor({
            name: "foreColor",
            displayName: "Foreground color",
            category: KnownCategory.APPEARANCE,
            type: BasicType.COLOR
        }));

        // behavior
        this.add(new PropertyDescriptor({
            name: "script",
            displayName: "Script",
            category: KnownCategory.BEHAVIOR,
            type: BasicType.STRING
        }));

        this.add(new PropertyDescriptor({
            name: "tooltip",
            displayName: "Tooltip",
            category: KnownCategory.BEHAVIOR,
            type: BasicType.STRING
        }));

        // design
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

        // layout
        this.add(new PropertyDescriptor({
            name: "location",
            displayName: "Location",
            category: KnownCategory.LAYOUT,
            type: BasicType.POINT
        }));

        this.add(new PropertyDescriptor({
            name: "size",
            displayName: "Size",
            category: KnownCategory.LAYOUT,
            type: BasicType.SIZE
        }));
    }

    // Adds missing properties to the component.
    repair(component) {
        component.properties ??= {};

        for (let propertyDescriptor of this.propertyDescriptors.values()) {
            propertyDescriptor.repair(component.properties);
        }
    }
}

// Represents a text component descriptor.
rs.mimic.TextDescriptor = class extends rs.mimic.ComponentDescriptor {
    constructor() {
        const BasicType = rs.mimic.BasicType;
        const KnownCategory = rs.mimic.KnownCategory;
        const PropertyDescriptor = rs.mimic.PropertyDescriptor;
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
        const BasicType = rs.mimic.BasicType;
        const KnownCategory = rs.mimic.KnownCategory;
        const PropertyDescriptor = rs.mimic.PropertyDescriptor;
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
