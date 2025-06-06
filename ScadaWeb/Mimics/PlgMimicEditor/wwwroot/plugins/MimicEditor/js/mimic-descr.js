// Contains classes: BasicType, KnownCategory, CornerRadiusStruct, BorderStruct, PaddingStruct, VisualStateStruct,
//     PropertyDescriptor, ObjectDescriptor, MimicDescriptor, ComponentDescriptorBase, ComponentDescriptor,
//     TextDescriptor, PictureDescriptor, PanelDescriptor, DescriptorSet
// Depends on scada-common.js, mimic-common.js

// Specifies the basic types.
rs.mimic.BasicType = class BasicType {
    static UNDEFINED = "undefined";
    static BOOL = "bool";
    static COLOR = "color";
    static ENUM = "enum";
    static FLOAT = "float";
    static INT = "int";
    static LIST = "list";
    static POINT = "point";
    static SIZE = "size";
    static STRING = "string";
    static STRUCT = "struct";

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

            case BasicType.LIST:
                return [];

            case BasicType.POINT:
                return { x: "0", y: "0" };

            case BasicType.SIZE:
                return { width: "0", height: "0" };

            case BasicType.STRING:
                return "";

            case BasicType.STRUCT:
                return {};

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

// Represents a corner radius structure.
rs.mimic.CornerRadiusStruct = class {
    topLeft = 0;
    topRight = 0;
    bottomRight = 0;
    bottomLeft = 0;
}

// Represents a border structure.
rs.mimic.BorderStruct = class {
    width = 0;
    color = ""
}

// Represents a padding structure.
rs.mimic.PaddingStruct = class {
    top = 0;
    right = 0;
    bottom = 0;
    left = 0;
}

// Represents a visual state structure.
rs.mimic.VisualStateStruct = class {
    backColor = "";
    foreColor = "";
    borderColor = "";
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

    getDefaultValue() {
        return this.defaultValue === undefined
            ? rs.mimic.BasicType.getDefaultValue(this.type)
            : ScadaUtils.deepClone(this.defaultValue);
    }
}

// Represents an object descriptor.
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

    // Adds missing properties to the object.
    repair(obj) {
        for (let propertyDescriptor of this.propertyDescriptors.values()) {
            if (!propertyDescriptor.isKnown && !obj.hasOwnProperty(propertyDescriptor.name)) {
                obj[propertyDescriptor.name] = propertyDescriptor.getDefaultValue();
            }
        }
    }
}

// Represents a mimic descriptor.
rs.mimic.MimicDescriptor = class extends rs.mimic.ObjectDescriptor {
    constructor() {
        super();
        const BasicType = rs.mimic.BasicType;
        const KnownCategory = rs.mimic.KnownCategory;
        const PropertyDescriptor = rs.mimic.PropertyDescriptor;

        // appearance
        this.add(new PropertyDescriptor({
            name: "backColor",
            displayName: "Background color",
            category: KnownCategory.APPEARANCE,
            type: BasicType.COLOR
        }));

        this.add(new PropertyDescriptor({
            name: "backgroundImage",
            displayName: "Background image",
            category: KnownCategory.APPEARANCE,
            type: BasicType.STRUCT
        }));

        this.add(new PropertyDescriptor({
            name: "font",
            displayName: "Font",
            category: KnownCategory.APPEARANCE,
            type: BasicType.STRUCT
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

        this.add(new PropertyDescriptor({
            name: "tooltip",
            displayName: "Tooltip",
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
        super.repair(mimic.document);

        if (mimic.components) {
            for (let component of mimic.components) {
                let componentDescriptor = component.isFaceplate
                    ? rs.mimic.DescriptorSet.faceplateDescriptor
                    : rs.mimic.DescriptorSet.componentDescriptors.get(component.typeName);

                if (componentDescriptor) {
                    componentDescriptor.repair(component);
                }
            }
        }
    }
}

// Represents a basic component descriptor that contains common properties for components and faceplates.
rs.mimic.ComponentDescriptorBase = class extends rs.mimic.ObjectDescriptor {
    constructor() {
        super();
        const BasicType = rs.mimic.BasicType;
        const KnownCategory = rs.mimic.KnownCategory;
        const PropertyDescriptor = rs.mimic.PropertyDescriptor;

        // behavior
        this.add(new PropertyDescriptor({
            name: "blinking",
            displayName: "Blinking",
            category: KnownCategory.BEHAVIOR,
            type: BasicType.BOOL
        }));

        this.add(new PropertyDescriptor({
            name: "enabled",
            displayName: "Enabled",
            category: KnownCategory.BEHAVIOR,
            type: BasicType.BOOL
        }));

        this.add(new PropertyDescriptor({
            name: "visible",
            displayName: "Visible",
            category: KnownCategory.BEHAVIOR,
            type: BasicType.BOOL
        }));

        // data
        this.add(new PropertyDescriptor({
            name: "deviceNum",
            displayName: "Device number",
            category: KnownCategory.DATA,
            type: BasicType.INT
        }));

        this.add(new PropertyDescriptor({
            name: "inCnlNum",
            displayName: "Input channel",
            category: KnownCategory.DATA,
            type: BasicType.INT
        }));

        this.add(new PropertyDescriptor({
            name: "objNum",
            displayName: "Object number",
            category: KnownCategory.DATA,
            type: BasicType.INT
        }));

        this.add(new PropertyDescriptor({
            name: "outCnlNum",
            displayName: "Output channel",
            category: KnownCategory.DATA,
            type: BasicType.INT
        }));

        this.add(new PropertyDescriptor({
            name: "propertyBindings",
            displayName: "Property bindings",
            category: KnownCategory.DATA,
            type: BasicType.LIST
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
        super.repair(component.properties);
    }
}

// Represents a component descriptor.
rs.mimic.ComponentDescriptor = class extends rs.mimic.ComponentDescriptorBase {
    constructor() {
        super();
        const BasicType = rs.mimic.BasicType;
        const KnownCategory = rs.mimic.KnownCategory;
        const PropertyDescriptor = rs.mimic.PropertyDescriptor;

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
            type: BasicType.STRUCT,
            defaultValue: new rs.mimic.BorderStruct()
        }));

        this.add(new PropertyDescriptor({
            name: "cornerRadius",
            displayName: "Corner radius",
            category: KnownCategory.APPEARANCE,
            type: BasicType.STRUCT,
            defaultValue: new rs.mimic.CornerRadiusStruct()
        }));

        this.add(new PropertyDescriptor({
            name: "cssClass",
            displayName: "CSS class",
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
            name: "blinkingState",
            displayName: "When blinking",
            category: KnownCategory.BEHAVIOR,
            type: BasicType.STRUCT,
            defaultValue: new rs.mimic.VisualStateStruct()
        }));

        this.add(new PropertyDescriptor({
            name: "clickAction",
            displayName: "On click",
            category: KnownCategory.BEHAVIOR,
            type: BasicType.STRUCT
        }));

        this.add(new PropertyDescriptor({
            name: "disabledState",
            displayName: "On disabled",
            category: KnownCategory.BEHAVIOR,
            type: BasicType.STRUCT,
            defaultValue: new rs.mimic.VisualStateStruct()
        }));

        this.add(new PropertyDescriptor({
            name: "hoverState",
            displayName: "On hover",
            category: KnownCategory.BEHAVIOR,
            type: BasicType.STRUCT,
            defaultValue: new rs.mimic.VisualStateStruct()
        }));

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
    }
}

// Represents a text component descriptor.
rs.mimic.TextDescriptor = class extends rs.mimic.ComponentDescriptor {
    constructor() {
        super();
        const BasicType = rs.mimic.BasicType;
        const KnownCategory = rs.mimic.KnownCategory;
        const PropertyDescriptor = rs.mimic.PropertyDescriptor;

        // appearance
        this.add(new PropertyDescriptor({
            name: "font",
            displayName: "Font",
            category: KnownCategory.APPEARANCE,
            type: BasicType.STRUCT
        }));

        this.add(new PropertyDescriptor({
            name: "text",
            displayName: "Text",
            category: KnownCategory.APPEARANCE,
            type: BasicType.STRING
        }));

        this.add(new PropertyDescriptor({
            name: "textAlign",
            displayName: "Text alignment",
            category: KnownCategory.APPEARANCE,
            type: BasicType.ENUM
        }));

        this.add(new PropertyDescriptor({
            name: "wordWrap",
            displayName: "Word wrap",
            category: KnownCategory.APPEARANCE,
            type: BasicType.BOOL
        }));

        // layout
        this.add(new PropertyDescriptor({
            name: "autoSize",
            displayName: "Auto size",
            category: KnownCategory.LAYOUT,
            type: BasicType.BOOL
        }));

        this.add(new PropertyDescriptor({
            name: "padding",
            displayName: "Padding",
            category: KnownCategory.LAYOUT,
            type: BasicType.STRUCT,
            defaultValue: new rs.mimic.PaddingStruct()
        }));
    }
}

// Represents a picture component descriptor.
rs.mimic.PictureDescriptor = class extends rs.mimic.ComponentDescriptor {
    constructor() {
        super();
        const BasicType = rs.mimic.BasicType;
        const KnownCategory = rs.mimic.KnownCategory;
        const PropertyDescriptor = rs.mimic.PropertyDescriptor;

        // appearance
        this.add(new PropertyDescriptor({
            name: "imageName",
            displayName: "Image",
            category: KnownCategory.APPEARANCE,
            type: BasicType.STRING
        }));

        // behavior
        this.add(new PropertyDescriptor({
            name: "conditions",
            displayName: "Conditions",
            category: KnownCategory.BEHAVIOR,
            type: BasicType.LIST
        }));

        this.add(new PropertyDescriptor({
            name: "sizeMode",
            displayName: "Size mode",
            category: KnownCategory.BEHAVIOR,
            type: BasicType.ENUM
        }));

        // layout
        this.add(new PropertyDescriptor({
            name: "padding",
            displayName: "Padding",
            category: KnownCategory.LAYOUT,
            type: BasicType.STRUCT,
            defaultValue: new rs.mimic.PaddingStruct()
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
rs.mimic.FaceplateDescriptor = class extends rs.mimic.ComponentDescriptorBase {
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
