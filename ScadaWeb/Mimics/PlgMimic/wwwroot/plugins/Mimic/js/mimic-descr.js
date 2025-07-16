// Contains classes:
//     KnownCategory, BasicType, Subtype, PropertyEditor,
//     PropertyDescriptor, ObjectDescriptor, MimicDescriptor, ComponentDescriptor, RegularComponentDescriptor,
//     TextDescriptor, PictureDescriptor, PanelDescriptor, FaceplateDescriptor,
//     StructureDescriptor, ActionDescriptor, BorderDescriptor, CommandArgsDescriptor, ConditionDescriptor,
//     CornerRadiusDescriptor, ImageConditionDescriptor, LinkArgsDescriptor, PaddingDescriptor,
//     PropertyBindingDescriptor, PropertyExportDescriptor, VisualStateDescriptor,
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
    static DATA_MEMBER = "DataMember";
    static IMAGE_SIZE_MODE = "ImageSizeMode";
    static LOGICAL_OPERATOR = "LogicalOperator";
    static LINK_TARGET = "LinkTarget";
    static MODAL_WIDTH = "ModalWidth";
    static CONTENT_ALIGNMENT = "ContentAlignment";
    static TEXT_DIRECTION = "TextDirection";

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
};

// Specifies the property editors.
rs.mimic.PropertyEditor = class {
    static COLOR_DIALOG = "ColorDialog";
    static FONT_DIALOG = "FontDialog";
    static IMAGE_DIALOG = "ImageDialog";
    static PROPERTY_DIALOG = "PropertyDialog";
    static TEXT_EDITOR = "TextEditor";
};

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
    editorOptions = null;

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
            editor: PropertyEditor.COLOR_DIALOG
        }));

        this.add(new PropertyDescriptor({
            name: "backgroundImage",
            displayName: "Background image",
            category: KnownCategory.APPEARANCE,
            type: BasicType.STRING,
            editor: PropertyEditor.IMAGE_DIALOG
        }));

        this.add(new PropertyDescriptor({
            name: "font",
            displayName: "Font",
            category: KnownCategory.APPEARANCE,
            type: BasicType.STRUCT,
            subtype: Subtype.FONT,
            editor: PropertyEditor.FONT_DIALOG
        }));

        this.add(new PropertyDescriptor({
            name: "foreColor",
            displayName: "Foreground color",
            category: KnownCategory.APPEARANCE,
            type: BasicType.STRING,
            editor: PropertyEditor.COLOR_DIALOG
        }));

        this.add(new PropertyDescriptor({
            name: "stylesheet",
            displayName: "Stylesheet",
            category: KnownCategory.APPEARANCE,
            type: BasicType.STRING,
            editor: PropertyEditor.TEXT_EDITOR,
            editorOptions: { language: "css" }
        }));

        // behavior
        this.add(new PropertyDescriptor({
            name: "script",
            displayName: "Script",
            category: KnownCategory.BEHAVIOR,
            type: BasicType.STRING,
            editor: PropertyEditor.TEXT_EDITOR,
            editorOptions: { language: "js" }
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
            name: "checkRights",
            displayName: "Check rights",
            category: KnownCategory.DATA,
            type: BasicType.BOOL
        }));

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
            editor: PropertyEditor.COLOR_DIALOG
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
            editor: PropertyEditor.COLOR_DIALOG
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
            type: BasicType.STRING,
            editor: PropertyEditor.TEXT_EDITOR,
            editorOptions: { language: "js" }
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
        const PropertyEditor = rs.mimic.PropertyEditor;
        const PropertyDescriptor = rs.mimic.PropertyDescriptor;

        // appearance
        this.add(new PropertyDescriptor({
            name: "font",
            displayName: "Font",
            category: KnownCategory.APPEARANCE,
            type: BasicType.STRUCT,
            subtype: Subtype.FONT,
            editor: PropertyEditor.FONT_DIALOG
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
            name: "textDirection",
            displayName: "Text direction",
            category: KnownCategory.APPEARANCE,
            type: BasicType.ENUM,
            subtype: Subtype.TEXT_DIRECTION
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
            editor: PropertyEditor.IMAGE_DIALOG
        }));

        this.add(new PropertyDescriptor({
            name: "rotation",
            displayName: "Rotation",
            category: KnownCategory.APPEARANCE,
            type: BasicType.FLOAT
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
            editor: PropertyEditor.TEXT_EDITOR,
            editorOptions: { language: "js" }
        }));
    }
};

// Represents a descriptor for the Border structure.
rs.mimic.BorderDescriptor = class extends rs.mimic.StructureDescriptor {
    constructor() {
        super();
        const BasicType = rs.mimic.BasicType;
        const PropertyEditor = rs.mimic.PropertyEditor;
        const PropertyDescriptor = rs.mimic.PropertyDescriptor;

        this.add(new PropertyDescriptor({
            name: "width",
            displayName: "Width",
            type: BasicType.INT
        }));

        this.add(new PropertyDescriptor({
            name: "color",
            displayName: "Color",
            type: BasicType.STRING,
            editor: PropertyEditor.COLOR_DIALOG
        }));
    }
};

// Represents a descriptor for the CommandArgs structure.
rs.mimic.CommandArgsDescriptor = class extends rs.mimic.StructureDescriptor {
    constructor() {
        super();
        const BasicType = rs.mimic.BasicType;
        const PropertyDescriptor = rs.mimic.PropertyDescriptor;

        this.add(new PropertyDescriptor({
            name: "showDialog",
            displayName: "Show dialog",
            type: BasicType.BOOL
        }));

        this.add(new PropertyDescriptor({
            name: "cmdVal",
            displayName: "Command value",
            type: BasicType.FLOAT
        }));
    }
};

// Represents an abstract descriptor for the Condition structure.
rs.mimic.ConditionDescriptor = class extends rs.mimic.StructureDescriptor {
    constructor() {
        super();
        const BasicType = rs.mimic.BasicType;
        const Subtype = rs.mimic.Subtype;
        const PropertyDescriptor = rs.mimic.PropertyDescriptor;

        this.add(new PropertyDescriptor({
            name: "comparisonOper1",
            displayName: "Comparison 1",
            type: BasicType.ENUM,
            subtype: Subtype.COMPARISON_OPERATOR
        }));

        this.add(new PropertyDescriptor({
            name: "comparisonArg1",
            displayName: "Argument 1",
            type: BasicType.FLOAT
        }));

        this.add(new PropertyDescriptor({
            name: "logicalOper",
            displayName: "Logical operator",
            type: BasicType.ENUM,
            subtype: Subtype.LOGICAL_OPERATOR
        }));

        this.add(new PropertyDescriptor({
            name: "comparisonOper2",
            displayName: "Comparison 2",
            type: BasicType.ENUM,
            subtype: Subtype.COMPARISON_OPERATOR
        }));

        this.add(new PropertyDescriptor({
            name: "comparisonArg2",
            displayName: "Argument 2",
            type: BasicType.FLOAT
        }));
    }
};

// Represents a descriptor for the CornerRadius structure.
rs.mimic.CornerRadiusDescriptor = class extends rs.mimic.StructureDescriptor {
    constructor() {
        super();
        const BasicType = rs.mimic.BasicType;
        const PropertyDescriptor = rs.mimic.PropertyDescriptor;

        this.add(new PropertyDescriptor({
            name: "topLeft",
            displayName: "Top-left",
            type: BasicType.INT
        }));

        this.add(new PropertyDescriptor({
            name: "topRight",
            displayName: "Top-right",
            type: BasicType.INT
        }));

        this.add(new PropertyDescriptor({
            name: "bottomRight",
            displayName: "Bottom-right",
            type: BasicType.INT
        }));

        this.add(new PropertyDescriptor({
            name: "bottomLeft",
            displayName: "Bottom-left",
            type: BasicType.INT
        }));
    }
}

// Represents a descriptor for the ImageCondition structure.
rs.mimic.ImageConditionDescriptor = class extends rs.mimic.ConditionDescriptor {
    constructor() {
        super();
        const BasicType = rs.mimic.BasicType;
        const PropertyEditor = rs.mimic.PropertyEditor;
        const PropertyDescriptor = rs.mimic.PropertyDescriptor;

        this.add(new PropertyDescriptor({
            name: "imageName",
            displayName: "Image",
            type: BasicType.STRING,
            editor: PropertyEditor.IMAGE_DIALOG
        }));
    }
}

// Represents a descriptor for the LinkArgs structure.
rs.mimic.LinkArgsDescriptor = class extends rs.mimic.StructureDescriptor {
    constructor() {
        super();
        const BasicType = rs.mimic.BasicType;
        const Subtype = rs.mimic.Subtype;
        const PropertyDescriptor = rs.mimic.PropertyDescriptor;

        this.add(new PropertyDescriptor({
            name: "url",
            displayName: "URL",
            type: BasicType.STRING
        }));

        this.add(new PropertyDescriptor({
            name: "target",
            displayName: "Target",
            type: BasicType.ENUM,
            subtype: Subtype.LINK_TARGET
        }));

        this.add(new PropertyDescriptor({
            name: "viewID",
            displayName: "View ID",
            type: BasicType.INT
        }));

        this.add(new PropertyDescriptor({
            name: "modalWidth",
            displayName: "Modal width",
            type: BasicType.ENUM,
            subtype: Subtype.MODAL_WIDTH
        }));

        this.add(new PropertyDescriptor({
            name: "modalHeight",
            displayName: "Modal height",
            type: BasicType.INT
        }));
    }
}

// Represents a descriptor for the Padding structure.
rs.mimic.PaddingDescriptor = class extends rs.mimic.StructureDescriptor {
    constructor() {
        super();
        const BasicType = rs.mimic.BasicType;
        const PropertyDescriptor = rs.mimic.PropertyDescriptor;

        this.add(new PropertyDescriptor({
            name: "top",
            displayName: "Top",
            type: BasicType.INT
        }));

        this.add(new PropertyDescriptor({
            name: "right",
            displayName: "Right",
            type: BasicType.INT
        }));

        this.add(new PropertyDescriptor({
            name: "bottom",
            displayName: "Bottom",
            type: BasicType.INT
        }));

        this.add(new PropertyDescriptor({
            name: "left",
            displayName: "Left",
            type: BasicType.INT
        }));
    }
};

// Represents a descriptor for the PropertyBinding structure.
rs.mimic.PropertyBindingDescriptor = class extends rs.mimic.StructureDescriptor {
    constructor() {
        super();
        const BasicType = rs.mimic.BasicType;
        const Subtype = rs.mimic.Subtype;
        const PropertyEditor = rs.mimic.PropertyEditor;
        const PropertyDescriptor = rs.mimic.PropertyDescriptor;

        this.add(new PropertyDescriptor({
            name: "propertyName",
            displayName: "Property name",
            type: BasicType.STRING,
            editor: PropertyEditor.PROPERTY_DIALOG
        }));

        this.add(new PropertyDescriptor({
            name: "dataSource",
            displayName: "Data source",
            type: BasicType.STRING
        }));

        this.add(new PropertyDescriptor({
            name: "dataMember",
            displayName: "Data member",
            type: BasicType.ENUM,
            subtype: Subtype.DATA_MEMBER
        }));

        this.add(new PropertyDescriptor({
            name: "format",
            displayName: "Format",
            type: BasicType.STRING
        }));
    }
};

// Represents a descriptor for the PropertyExport structure.
rs.mimic.PropertyExportDescriptor = class extends rs.mimic.StructureDescriptor {
    constructor() {
        super();
        const BasicType = rs.mimic.BasicType;
        const PropertyDescriptor = rs.mimic.PropertyDescriptor;

        this.add(new PropertyDescriptor({
            name: "name",
            displayName: "Property name",
            type: BasicType.STRING
        }));

        this.add(new PropertyDescriptor({
            name: "path",
            displayName: "Path",
            type: BasicType.STRING
        }));
    }
};

// Represents a descriptor for the VisualState structure.
rs.mimic.VisualStateDescriptor = class extends rs.mimic.StructureDescriptor {
    constructor() {
        super();
        const BasicType = rs.mimic.BasicType;
        const PropertyEditor = rs.mimic.PropertyEditor;
        const PropertyDescriptor = rs.mimic.PropertyDescriptor;

        this.add(new PropertyDescriptor({
            name: "backColor",
            displayName: "Back color",
            type: BasicType.STRING,
            editor: PropertyEditor.COLOR_DIALOG
        }));

        this.add(new PropertyDescriptor({
            name: "foreColor",
            displayName: "Fore color",
            type: BasicType.STRING,
            editor: PropertyEditor.COLOR_DIALOG
        }));

        this.add(new PropertyDescriptor({
            name: "borderColor",
            displayName: "Border color",
            type: BasicType.STRING,
            editor: PropertyEditor.COLOR_DIALOG
        }));

        this.add(new PropertyDescriptor({
            name: "underline",
            displayName: "Underline",
            type: BasicType.BOOL
        }));
    }
};

// Contains descriptors for a mimic and its components.
rs.mimic.DescriptorSet = class {
    static mimicDescriptor = new rs.mimic.MimicDescriptor();
    static componentDescriptors = new Map([
        ["Text", new rs.mimic.TextDescriptor()],
        ["Picture", new rs.mimic.PictureDescriptor()],
        ["Panel", new rs.mimic.PanelDescriptor()]
    ]);
    static getFaceplateDescriptor(faceplate) {
        const KnownCategory = rs.mimic.KnownCategory;
        const PropertyDescriptor = rs.mimic.PropertyDescriptor;
        let descriptor = new rs.mimic.FaceplateDescriptor();

        if (faceplate && Array.isArray(faceplate.document.propertyExports)) {
            for (let propertyExport of faceplate.document.propertyExports) {
                if (propertyExport.name) {
                    descriptor.add(new PropertyDescriptor({
                        name: propertyExport.name,
                        displayName: propertyExport.name,
                        category: KnownCategory.MISC
                    }));
                }
            }
        }

        return descriptor;
    }
    static structureDescriptors = new Map([
        ["Action", new rs.mimic.ActionDescriptor()],
        ["Border", new rs.mimic.BorderDescriptor()],
        ["CommandArgs", new rs.mimic.CommandArgsDescriptor()],
        ["CornerRadius", new rs.mimic.CornerRadiusDescriptor()],
        ["ImageCondition", new rs.mimic.ImageConditionDescriptor()],
        ["LinkArgs", new rs.mimic.LinkArgsDescriptor()],
        ["Padding", new rs.mimic.PaddingDescriptor()],
        ["PropertyBinding", new rs.mimic.PropertyBindingDescriptor()],
        ["PropertyExport", new rs.mimic.PropertyExportDescriptor()],
        ["VisualState", new rs.mimic.VisualStateDescriptor()]
    ]);
};
