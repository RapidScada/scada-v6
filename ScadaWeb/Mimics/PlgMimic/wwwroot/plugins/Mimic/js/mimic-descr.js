// Contains classes:
//     KnownCategory, BasicType, Subtype, PropertyEditor,
//     PropertyDescriptor, ObjectDescriptor, MimicDescriptor, ComponentDescriptor, RegularComponentDescriptor,
//     TextDescriptor, PictureDescriptor, PanelDescriptor, FaceplateDescriptor,
//     StructureDescriptor, ActionDescriptor,
//     DescriptorSet
// Depends on scada-common.js, mimic-common.js

// Specifies the known categories.
// Values ​​must be specified in camel case since they are used as property names.
rs.mimic.KnownCategory = class {
    static APPEARANCE = "appearance";
    static BEHAVIOR = "behavior";
    static DATA = "data";
    static DESIGN = "design";
    static LAYOUT = "layout";
    static MISC = "misc";
};

// Specifies the basic types.
rs.mimic.BasicType = class {
    static BOOL = "bool";
    static ENUM = "enum";
    static FLOAT = "float";
    static INT = "int";
    static LIST = "list";
    static STRING = "string";
    static STRUCT = "struct";
};

// Specifies the subtypes.
rs.mimic.Subtype = class {
    // Enumerations
    static ACTION_TYPE = "ActionType";
    static COMPARISON_OPERATOR = "ComparisonOperator";
    static IMAGE_SIZE_MODE = "ImageSizeMode";
    static LOGICAL_OPERATOR = "LogicalOperator";
    static LINK_TARGET = "LinkTarget";
    static MODAL_WIDTH = "ModalWidth";
    static CONTENT_ALIGNMENT = "ContentAlignment";

    // Structures
    static ACTION = "Action";
    static BORDER = "Border";
    static COMMAND_ARGS = "CommandArgs";
    static CONDITION = "Condition";
    static CORNER_RADIUS = "CornerRadius";
    static FONT = "Font";
    static IMAGE_CONDITION = "ImageCondition";
    static LINK_ARGS = "LinkArgs";
    static PADDING = "Padding";
    static POINT = "Point";
    static PROPERTY_BINDING = "PropertyBinding";
    static PROPERTY_EXPORT = "PropertyExport";
    static SIZE = "Size";
    static VISUAL_STATE = "VisualState";
}

// Specifies the property editors.
rs.mimic.PropertyEditor = class {
    static COLOR_PICKER = "ColorPicker";
    static IMAGE_PICKER = "ImagePicker";
    static PROPERTY_PICKER = "PropertyPicker";
    static TEXT_EDITOR = "TextEditor";
}

// Provides meta information about a property of a mimic or component.
rs.mimic.PropertyDescriptor = class {
    name = "";
    displayName = "";
    category = "";
    isReadOnly = false;
    isBrowsable = true;
    type = "";
    subtype = "";
    editor = "";
    tweakpaneOptions = null;

    constructor(source) {
        Object.assign(this, source);
    }
};

// Represents an object descriptor.
rs.mimic.ObjectDescriptor = class {
    propertyDescriptors = new Map(); // property descriptors accessible by property name
    sorted = true;

    add(propertyDescriptor) {
        this.propertyDescriptors.set(propertyDescriptor.name, propertyDescriptor);
    }

    get(propertyName) {
        return this.propertyDescriptors.get(propertyName);
    }

    delete(propertyName) {
        this.propertyDescriptors.delete(propertyName);
    }
};

// Represents a mimic descriptor.
rs.mimic.MimicDescriptor = class extends rs.mimic.ObjectDescriptor {
    constructor() {
        super();
        const KnownCategory = rs.mimic.KnownCategory;
        const BasicType = rs.mimic.BasicType;
        const Subtype = rs.mimic.Subtype;
        const PropertyEditor = rs.mimic.PropertyEditor;
        const PropertyDescriptor = rs.mimic.PropertyDescriptor;

        // appearance
        this.add(new PropertyDescriptor({
            name: "backColor",
            displayName: "Background color",
            category: KnownCategory.APPEARANCE,
            type: BasicType.STRING,
            editor: PropertyEditor.COLOR_PICKER
        }));

        this.add(new PropertyDescriptor({
            name: "backgroundImage",
            displayName: "Background image",
            category: KnownCategory.APPEARANCE,
            type: BasicType.STRING,
            editor: PropertyEditor.IMAGE_PICKER
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
            type: BasicType.STRING,
            editor: PropertyEditor.COLOR_PICKER
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

        // data
        this.add(new PropertyDescriptor({
            name: "propertyExports",
            displayName: "Exported properties",
            category: KnownCategory.DATA,
            type: BasicType.LIST
        }));

        // layout
        this.add(new PropertyDescriptor({
            name: "size",
            displayName: "Size",
            category: KnownCategory.LAYOUT,
            type: BasicType.STRUCT,
            subtype: Subtype.SIZE
        }));
    }
};

// Represents a basic component descriptor.
rs.mimic.ComponentDescriptor = class extends rs.mimic.ObjectDescriptor {
    constructor() {
        super();
        const KnownCategory = rs.mimic.KnownCategory;
        const BasicType = rs.mimic.BasicType;
        const Subtype = rs.mimic.Subtype;
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
            type: BasicType.INT
        }));

        this.add(new PropertyDescriptor({
            name: "name",
            displayName: "Name",
            category: KnownCategory.DESIGN,
            type: BasicType.STRING
        }));

        this.add(new PropertyDescriptor({
            name: "typeName",
            displayName: "Type name",
            category: KnownCategory.DESIGN,
            isReadOnly: true,
            type: BasicType.STRING
        }));

        // layout
        this.add(new PropertyDescriptor({
            name: "location",
            displayName: "Location",
            category: KnownCategory.LAYOUT,
            type: BasicType.STRUCT,
            subtype: Subtype.POINT
        }));

        this.add(new PropertyDescriptor({
            name: "size",
            displayName: "Size",
            category: KnownCategory.LAYOUT,
            type: BasicType.STRUCT,
            subtype: Subtype.SIZE
        }));
    }
};

// Represents a descriptor for regular non-faceplate components.
rs.mimic.RegularComponentDescriptor = class extends rs.mimic.ComponentDescriptor {
    constructor() {
        super();
        const KnownCategory = rs.mimic.KnownCategory;
        const BasicType = rs.mimic.BasicType;
        const PropertyEditor = rs.mimic.PropertyEditor;
        const PropertyDescriptor = rs.mimic.PropertyDescriptor;

        // appearance
        this.add(new PropertyDescriptor({
            name: "backColor",
            displayName: "Background color",
            category: KnownCategory.APPEARANCE,
            type: BasicType.STRING,
            editor: PropertyEditor.COLOR_PICKER
        }));

        this.add(new PropertyDescriptor({
            name: "border",
            displayName: "Border",
            category: KnownCategory.APPEARANCE,
            type: BasicType.STRUCT
        }));

        this.add(new PropertyDescriptor({
            name: "cornerRadius",
            displayName: "Corner radius",
            category: KnownCategory.APPEARANCE,
            type: BasicType.STRUCT
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
            type: BasicType.STRING,
            editor: PropertyEditor.COLOR_PICKER
        }));

        // behavior
        this.add(new PropertyDescriptor({
            name: "blinkingState",
            displayName: "When blinking",
            category: KnownCategory.BEHAVIOR,
            type: BasicType.STRUCT
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
            type: BasicType.STRUCT
        }));

        this.add(new PropertyDescriptor({
            name: "hoverState",
            displayName: "On hover",
            category: KnownCategory.BEHAVIOR,
            type: BasicType.STRUCT
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
};

// Represents a text component descriptor.
rs.mimic.TextDescriptor = class extends rs.mimic.RegularComponentDescriptor {
    constructor() {
        super();
        const KnownCategory = rs.mimic.KnownCategory;
        const BasicType = rs.mimic.BasicType;
        const Subtype = rs.mimic.Subtype;
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
            type: BasicType.ENUM,
            subtype: Subtype.CONTENT_ALIGNMENT
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
            type: BasicType.STRUCT
        }));
    }
};

// Represents a picture component descriptor.
rs.mimic.PictureDescriptor = class extends rs.mimic.RegularComponentDescriptor {
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
            editor: PropertyEditor.IMAGE_PICKER
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
            type: BasicType.ENUM,
            subtype: Subtype.IMAGE_SIZE_MODE
        }));

        // layout
        this.add(new PropertyDescriptor({
            name: "padding",
            displayName: "Padding",
            category: KnownCategory.LAYOUT,
            type: BasicType.STRUCT
        }));
    }
};

// Represents a panel component descriptor.
rs.mimic.PanelDescriptor = class extends rs.mimic.RegularComponentDescriptor {
};

// Represents a faceplate descriptor.
rs.mimic.FaceplateDescriptor = class extends rs.mimic.ComponentDescriptor {
};

// Represents a structure descriptor.
rs.mimic.StructureDescriptor = class extends rs.mimic.ObjectDescriptor {
    constructor() {
        super();
        this.sorted = false;
    }
};

// Represents a descriptor for the Action structure.
rs.mimic.ActionDescriptor = class extends rs.mimic.StructureDescriptor {
    constructor() {
        super();
        const BasicType = rs.mimic.BasicType;
        const Subtype = rs.mimic.Subtype;
        const PropertyEditor = rs.mimic.PropertyEditor;
        const PropertyDescriptor = rs.mimic.PropertyDescriptor;

        this.add(new PropertyDescriptor({
            name: "actionType",
            displayName: "Action type",
            type: BasicType.ENUM,
            subtype: Subtype.ACTION_TYPE
        }));

        this.add(new PropertyDescriptor({
            name: "chartArgs",
            displayName: "Chart arguments",
            type: BasicType.STRING
        }));

        this.add(new PropertyDescriptor({
            name: "commandArgs",
            displayName: "Command arguments",
            type: BasicType.STRUCT,
            subtype: Subtype.COMMAND_ARGS
        }));

        this.add(new PropertyDescriptor({
            name: "linkArgs",
            displayName: "Link arguments",
            type: BasicType.STRUCT,
            subtype: Subtype.LINK_ARGS
        }));

        this.add(new PropertyDescriptor({
            name: "script",
            displayName: "Script",
            type: BasicType.STRING,
            editor: PropertyEditor.TEXT_EDITOR
        }));
    }
};

// Contains descriptors for a mimic and its components.
rs.mimic.DescriptorSet = class {
    static mimicDescriptor = new rs.mimic.MimicDescriptor();
    static faceplateDescriptor = new rs.mimic.FaceplateDescriptor();
    static componentDescriptors = new Map([
        ["Text", new rs.mimic.TextDescriptor()],
        ["Picture", new rs.mimic.PictureDescriptor()],
        ["Panel", new rs.mimic.PanelDescriptor()]
    ]);
    static structureDescriptors = new Map([
        ["Action", new rs.mimic.ActionDescriptor()],
    ]);
};
