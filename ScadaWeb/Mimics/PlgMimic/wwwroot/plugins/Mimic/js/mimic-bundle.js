// Contains classes: LoadStep, LoadResult, LoadContext
// No dependencies

// Namespaces
var rs = rs ?? {};
rs.mimic = rs.mimic ?? {};

// Specifies the steps for loading a mimic.
rs.mimic.LoadStep = class {
    static UNDEFINED = 0;
    static PROPERTIES = 1;
    static COMPONENTS = 2;
    static IMAGES = 3;
    static FACEPLATES = 4;
    static COMPLETE = 5;
};

// Represents a result of loading a mimic.
rs.mimic.LoadResult = class {
    ok = false;
    msg = "";
};

// Represents a context of a loading operation.
rs.mimic.LoadContext = class {
    static COMPONENTS_TO_REQUEST = 1000;
    static IMAGES_TO_REQUEST = 100;
    static IMAGE_TOTAL_SIZE = 1048576;

    controllerUrl = "";
    mimicKey = "";
    result = new rs.mimic.LoadResult();
    step = rs.mimic.LoadStep.UNDEFINED;
    componentIndex = 0;
    imageIndex = 0;
    faceplateIndex = 0;
    unknownTypes = new Set();

    constructor(controllerUrl, mimicKey) {
        this.controllerUrl = controllerUrl;
        this.mimicKey = mimicKey.toString();
    }
};

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
            type: BasicType.STRING
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

// Contains classes: MimicFactory, ComponentFactory, RegularComponentFactory,
//     TextFactory, PictureFactory, PanelFactory, FaceplateFactory, FactorySet
// Depends on mimic-model.js, mimic-model-subtypes.js

// Create mimic properties.
rs.mimic.MimicFactory = class {
    // Parses the document properties from the specified source object.
    static parseProperties(sourceProps) {
        const PropertyParser = rs.mimic.PropertyParser;
        sourceProps ??= {};
        return {
            // appearance
            backColor: PropertyParser.parseString(sourceProps.backColor),
            backgroundImage: PropertyParser.parseString(sourceProps.backgroundImage),
            font: rs.mimic.Font.parse(sourceProps.font),
            foreColor: PropertyParser.parseString(sourceProps.foreColor),
            stylesheet: PropertyParser.parseString(sourceProps.stylesheet),

            // behavior
            script: PropertyParser.parseString(sourceProps.script),
            tooltip: PropertyParser.parseString(sourceProps.tooltip),

            // data
            propertyExports: PropertyParser.parsePropertyExports(sourceProps.propertyExports),

            // layout
            size: rs.mimic.Size.parse(sourceProps.size)
        };
    }
}

// Represents an abstract component factory.
rs.mimic.ComponentFactory = class {
    // Creates new component properties.
    createProperties() {
        return {
            // behavior
            blinking: false,
            enabled: true,
            visible: true,

            // data
            checkRights: false,
            deviceNum: 0,
            inCnlNum: 0,
            objNum: 0,
            outCnlNum: 0,
            propertyBindings: new rs.mimic.PropertyBindingList(),

            // design
            id: 0,
            name: "",
            typeName: "",

            // layout
            location: new rs.mimic.Point(),
            size: new rs.mimic.Size()
        };
    }

    // Parses the component properties from the specified source object.
    parseProperties(sourceProps) {
        const PropertyParser = rs.mimic.PropertyParser;
        sourceProps ??= {};
        return {
            // behavior
            blinking: PropertyParser.parseBool(sourceProps.blinking),
            enabled: PropertyParser.parseBool(sourceProps.enabled),
            visible: PropertyParser.parseBool(sourceProps.visible),

            // data
            checkRights: PropertyParser.parseBool(sourceProps.checkRights),
            deviceNum: PropertyParser.parseInt(sourceProps.deviceNum),
            inCnlNum: PropertyParser.parseInt(sourceProps.inCnlNum),
            objNum: PropertyParser.parseInt(sourceProps.objNum),
            outCnlNum: PropertyParser.parseInt(sourceProps.outCnlNum),
            propertyBindings: PropertyParser.parsePropertyBindings(sourceProps.propertyBindings),

            // design
            id: PropertyParser.parseInt(sourceProps.id),
            name: PropertyParser.parseString(sourceProps.name),
            typeName: PropertyParser.parseString(sourceProps.typeName),

            // layout
            location: rs.mimic.Point.parse(sourceProps.location),
            size: rs.mimic.Size.parse(sourceProps.size)
        };
    }

    // Creates a new component with the given type name.
    createComponent(typeName) {
        let component = new rs.mimic.Component();
        component.typeName = typeName;
        component.properties = this.createProperties(typeName);
        component.properties.typeName = typeName;
        return component;
    }

    // Creates a new component with the specified properties, making deep copies of the source properties.
    createComponentFromSource(source) {
        let component = new rs.mimic.Component();
        component.id = source.id;
        component.typeName = source.typeName;
        component.properties = this.parseProperties(source.properties);
        component.properties.typeName = source.typeName;
        component.parentID = source.parentID;
        return component;
    }
};

// Represents an abstract factory for regular non-faceplate components.
rs.mimic.RegularComponentFactory = class extends rs.mimic.ComponentFactory {
    createProperties() {
        let properties = super.createProperties();

        // appearance
        Object.assign(properties, {
            backColor: "",
            border: new rs.mimic.Border(),
            cornerRadius: new rs.mimic.CornerRadius(),
            cssClass: "",
            foreColor: ""
        });

        // behavior
        Object.assign(properties, {
            blinkingState: new rs.mimic.VisualState(),
            clickAction: new rs.mimic.Action(),
            disabledState: new rs.mimic.VisualState(),
            hoverState: new rs.mimic.VisualState(),
            script: "",
            tooltip: ""
        });

        return properties;
    }

    parseProperties(sourceProps) {
        const PropertyParser = rs.mimic.PropertyParser;
        let properties = super.parseProperties(sourceProps);
        sourceProps ??= {};

        // appearance
        Object.assign(properties, {
            backColor: PropertyParser.parseString(sourceProps.backColor),
            border: rs.mimic.Border.parse(sourceProps.border),
            cornerRadius: rs.mimic.CornerRadius.parse(sourceProps.cornerRadius),
            cssClass: PropertyParser.parseString(sourceProps.cssClass),
            foreColor: PropertyParser.parseString(sourceProps.foreColor)
        });

        // behavior
        Object.assign(properties, {
            blinkingState: rs.mimic.VisualState.parse(sourceProps.blinkingState),
            clickAction: rs.mimic.Action.parse(sourceProps.clickAction),
            disabledState: rs.mimic.VisualState.parse(sourceProps.disabledState),
            hoverState: rs.mimic.VisualState.parse(sourceProps.hoverState),
            script: PropertyParser.parseString(sourceProps.script),
            tooltip: PropertyParser.parseString(sourceProps.tooltip)
        });

        return properties;
    }
};

// Creates components of the Text type.
rs.mimic.TextFactory = class extends rs.mimic.RegularComponentFactory {
    createProperties() {
        let properties = super.createProperties();

        // appearance
        Object.assign(component.properties, {
            font: new rs.mimic.Font(),
            text: "Text",
            textAlign: rs.mimic.ContentAlignment.TOP_LEFT,
            textDirection: rs.mimic.TextDirection.HORIZONTAL,
            wordWrap: false
        });

        // layout
        Object.assign(component.properties, {
            autoSize: false,
            padding: new rs.mimic.Padding()
        });

        return properties;
    }

    parseProperties(sourceProps) {
        const PropertyParser = rs.mimic.PropertyParser;
        let properties = super.parseProperties(sourceProps);
        sourceProps ??= {};

        // appearance
        Object.assign(properties, {
            font: rs.mimic.Font.parse(sourceProps.font),
            text: PropertyParser.parseString(sourceProps.text, "Text"),
            textAlign: PropertyParser.parseString(sourceProps.textAlign, rs.mimic.ContentAlignment.TOP_LEFT),
            textDirection: PropertyParser.parseString(sourceProps.textDirection, rs.mimic.TextDirection.HORIZONTAL),
            wordWrap: PropertyParser.parseBool(sourceProps.wordWrap)
        });

        // layout
        Object.assign(properties, {
            autoSize: PropertyParser.parseBool(sourceProps.autoSize),
            padding: rs.mimic.Padding.parse(sourceProps.padding)
        });

        return properties;
    }

    createComponent() {
        return super.createComponent("Text");
    }
};

// Creates components of the Picture type.
rs.mimic.PictureFactory = class extends rs.mimic.RegularComponentFactory {
    createProperties() {
        let properties = super.createProperties();

        // appearance
        Object.assign(properties, {
            imageName: "",
            rotation: 0
        });

        // behavior
        Object.assign(properties, {
            conditions: new rs.mimic.ImageConditionList(),
            sizeMode: rs.mimic.ImageSizeMode.NORMAL
        });

        // layout
        Object.assign(properties, {
            padding: new rs.mimic.Padding()
        });

        return properties;
    }

    parseProperties(sourceProps) {
        const PropertyParser = rs.mimic.PropertyParser;
        let properties = super.parseProperties(sourceProps);
        sourceProps ??= {};

        // appearance
        Object.assign(properties, {
            imageName: PropertyParser.parseString(sourceProps.imageName),
            rotation: PropertyParser.parseFloat(sourceProps.rotation)
        });

        // behavior
        Object.assign(properties, {
            conditions: PropertyParser.parseImageConditions(sourceProps.conditions),
            sizeMode: PropertyParser.parseString(sourceProps.sizeMode, rs.mimic.ImageSizeMode.NORMAL)
        });

        // layout
        Object.assign(properties, {
            padding: rs.mimic.Padding.parse(sourceProps.padding)
        });

        return properties;
    }

    createComponent() {
        return super.createComponent("Picture");
    }
};

// Creates components of the Panel type.
rs.mimic.PanelFactory = class extends rs.mimic.RegularComponentFactory {
    createComponent() {
        let component = super.createComponent("Panel");
        component.children = []; // accept child components
        return component;
    }

    createComponentFromSource(source) {
        let component = super.createComponentFromSource(source);
        component.children = [];
        return component;
    }
};

// Creates faceplate instances.
rs.mimic.FaceplateFactory = class extends rs.mimic.ComponentFactory {
    createComponent(faceplate) {
        let component = new rs.mimic.FaceplateInstance();
        component.properties = this.createProperties();

        if (faceplate) {
            component.typeName = component.properties.typeName = faceplate.typeName;
            component.applyModel(faceplate);
        }

        return component;
    }

    createComponentFromSource(source, faceplate) {
        let component = new rs.mimic.FaceplateInstance();
        component.id = source.id;
        component.typeName = source.typeName;
        component.properties = this.parseProperties(source.properties);
        component.properties.typeName = source.typeName;
        component.parentID = source.parentID;

        if (faceplate) {
            component.typeName = component.properties.typeName = faceplate.typeName;
            component.applyModel(faceplate);
        }

        return component;
    }
};

// Contains factories for mimic components.
rs.mimic.FactorySet = class FactorySet {
    static faceplateFactory = new rs.mimic.FaceplateFactory();
    static componentFactories = new Map([
        ["Text", new rs.mimic.TextFactory()],
        ["Picture", new rs.mimic.PictureFactory()],
        ["Panel", new rs.mimic.PanelFactory()]
    ]);
    static getFaceplateFactory(faceplate) {
        return {
            createProperties: () => FactorySet.faceplateFactory.createProperties(),
            parseProperties: (sourceProps) => FactorySet.faceplateFactory.parseProperties(sourceProps),
            createComponent: () => FactorySet.faceplateFactory.createComponent(faceplate),
            createComponentFromSource: (source) =>
                FactorySet.faceplateFactory.createComponentFromSource(source, faceplate)
        };
    }
};

// Contains classes: MimicHelper, MimicBase, Mimic, Component, Panel, Image, 
//     FaceplateMeta, Faceplate, FaceplateInstance
// Depends on scada-common.js, mimic-common.js, mimic-factory.js

// Provides helper methods for mimics and components.
rs.mimic.MimicHelper = class MimicHelper {
    // Indexes child components.
    static _indexComponents(parent, opt_start) {
        for (let index = opt_start ?? 0; index < parent.children.length; index++) {
            let component = parent.children[index];
            component.index = index;
        }
    }

    // Finds a parent and children for each component.
    static defineNesting(root, components, opt_componentMap) {
        let componentMap = opt_componentMap ?? new Map(components.map(c => [c.id, c]));

        for (let component of components) {
            if (component.parentID > 0) {
                let parent = componentMap.get(component.parentID);

                if (parent) {
                    parent.children ??= [];
                    component.index = parent.children.length;
                    component.parent = parent;
                    parent.children.push(component);
                }
            } else {
                root.children ??= [];
                component.index = root.children.length;
                component.parent = root;
                root.children.push(component);
            }
        }
    }

    // Adds the component to the parent.
    static addToParent(parent, component, opt_index) {
        if (parent.children) {
            component.parentID = parent.id ?? 0;
            let index = Number.isInteger(opt_index) && 0 <= opt_index && opt_index < parent.children.length
                ? opt_index
                : parent.children.length;
            component.parent = parent;
            parent.children.splice(index, 0, component); // insert
            MimicHelper._indexComponents(parent, index);
        }
    }

    // Removes the component from its parent.
    static removeFromParent(component) {
        let parent = component.parent;

        if (parent.children && component.index >= 0) {
            parent.children.splice(component.index, 1); // delete
            MimicHelper._indexComponents(parent, component.index);
            component.index = -1;
        }
    }

    // Checks whether the specified objects are a parent and a child.
    static areRelatives(parent, child) {
        let current = child.parent;

        while (current) {
            if (current === parent) {
                return true;
            }

            current = current.parent;
        }

        return false;
    }

    // Checks whether the specified components belong to the same parent.
    static areSiblings(components) {
        let parentIDs = new Set(components.map(c => c.parentID));
        return parentIDs.size === 1;
    }

    // Moves the components to the beginning of the parent's children.
    static sendToBack(parent, components) {
        if (parent.children) {
            let componentIDs = new Set(components.map(c => c.id));
            parent.children = parent.children.filter(c => !componentIDs.has(c.id));
            parent.children.unshift(...components);
            MimicHelper._indexComponents(parent);
        }
    }

    // Moves the components one position towards the beginning of the parent's children.
    static sendBackward(parent, components) {
        if (parent.children) {
            let indexes = components.map(c => c.index).sort();
            let prevIndex = -1;

            for (let index of indexes) {
                if (index >= 0 && prevIndex < index - 1) {
                    prevIndex = index - 1;
                    let component = parent.children[index];
                    let prevComponent = parent.children[prevIndex];
                    parent.children[index] = prevComponent;
                    parent.children[prevIndex] = component;
                    component.index--;
                    prevComponent.index++;
                } else {
                    prevIndex = index;
                }
            }
        }
    }

    // Moves the components one position towards the end of the parent's children.
    static bringForward(parent, components) {
        if (parent.children) {
            let indexes = components.map(c => c.index).sort();
            let nextIndex = parent.children.length;

            for (let i = indexes.length - 1; i >= 0; i--) {
                let index = indexes[i];

                if (index >= 0 && nextIndex > index + 1) {
                    nextIndex = index + 1;
                    let component = parent.children[index];
                    let nextComponent = parent.children[nextIndex];
                    parent.children[index] = nextComponent;
                    parent.children[nextIndex] = component;
                    component.index++;
                    nextComponent.index--;
                } else {
                    nextIndex = index;
                }
            }
        }
    }

    // Moves the components to the end of the parent's children.
    static bringToFront(parent, components) {
        if (parent.children) {
            let componentIDs = new Set(components.map(c => c.id));
            parent.children = parent.children.filter(c => !componentIDs.has(c.id));
            parent.children.push(...components);
            MimicHelper._indexComponents(parent);
        }
    }

    // Moves the components before their sibling.
    static placeBefore(parent, components, sibling) {
        if (parent.children) {
            let componentIDs = new Set(components.map(c => c.id));
            let filtered = parent.children.filter(c => !componentIDs.has(c.id));
            let index = filtered.indexOf(sibling);

            if (index >= 0) {
                filtered.splice(index, 0, ...components);
                parent.children = filtered;
                MimicHelper._indexComponents(parent);
            }
        }
    }

    // Moves the components after their sibling.
    static placeAfter(parent, components, sibling) {
        if (parent.children) {
            let componentIDs = new Set(components.map(c => c.id));
            let filtered = parent.children.filter(c => !componentIDs.has(c.id));
            let index = filtered.indexOf(sibling);

            if (index >= 0) {
                filtered.splice(index + 1, 0, ...components);
                parent.children = filtered;
                MimicHelper._indexComponents(parent);
            }
        }
    }

    // Arranges the components according to the indexes.
    static arrange(parent, components, indexes) {
        if (parent.children && components.length === indexes.length) {
            // map components
            let componentByIndex = new Map();
            let componentIDs = new Set();

            for (let i = 0; i < components.length; i++) {
                let component = components[i];
                componentByIndex.set(indexes[i], component);
                componentIDs.add(component.id);
            }

            // copy children to new array
            let arranged = [];
            let arrangedIndex = 0;
            let sourceIndex = 0;
            let length = parent.children.length;

            while (arrangedIndex < length && sourceIndex < length) {
                let component = componentByIndex.get(arrangedIndex);

                if (component) {
                    component.index = arrangedIndex++;
                    arranged.push(component);
                } else {
                    component = parent.children[sourceIndex++];

                    if (!componentIDs.has(component.id)) {
                        component.index = arrangedIndex++;
                        arranged.push(component);
                    }
                }
            }

            parent.children = arranged;
        }
    }

    // Gets the minimum coordinates of the components.
    static getMinLocation(components) {
        let minX = NaN;
        let minY = NaN;

        for (let component of components) {
            let x = component.x;
            let y = component.y;

            if (isNaN(minX) || minX > x) {
                minX = x;
            }

            if (isNaN(minY) || minY > y) {
                minY = y;
            }
        }

        return {
            x: minX,
            y: minY
        };
    }
};

// A base class for mimic diagrams and faceplates.
rs.mimic.MimicBase = class {
    dependencies;  // meta information about faceplates
    document;      // mimic properties
    components;    // all components
    images;        // image collection
    faceplates;    // faceplate collection

    dependencyMap; // dependencies accessible by type name
    componentMap;  // components accessible by ID
    imageMap;      // images accessible by name
    faceplateMap;  // faceplates accessible by type name
    children;      // top-level components

    // Clears the mimic.
    clear() {
        this.dependencies = [];
        this.document = {};
        this.components = [];
        this.images = [];
        this.faceplates = [];

        this.dependencyMap = new Map();
        this.componentMap = new Map();
        this.imageMap = new Map();
        this.faceplateMap = new Map();
        this.children = [];
    }

    // Checks whether the specified type name represents a faceplate.
    isFaceplate(typeName) {
        return this.dependencyMap?.has(typeName);
    }

    // Gets the component factory for the specified type, or null if not found.
    getComponentFactory(typeName) {
        const FactorySet = rs.mimic.FactorySet;

        if (this.isFaceplate(typeName)) {
            let faceplate = this.faceplateMap.get(typeName); // can be null
            return FactorySet.getFaceplateFactory(faceplate);
        } else {
            return FactorySet.componentFactories.get(typeName);
        }
    }

    // Creates a component instance based on the source object. Returns null if the component factory is not found.
    createComponent(source) {
        let factory = this.getComponentFactory(source.typeName);
        return factory ? factory.createComponentFromSource(source) : null;
    }

    // Creates a copy of the component containing only the main properties.
    copyComponent(source) {
        return source instanceof rs.mimic.Component
            ? this.createComponent(source.toPlainObject())
            : this.createComponent(source);
    }
};

// Represents a mimic diagram.
rs.mimic.Mimic = class extends rs.mimic.MimicBase {
    dom;      // mimic DOM as a jQuery object
    renderer; // renders the mimic

    // Imitates a component ID to use as a parent ID.
    get id() {
        return 0;
    }

    // Indicates that a mimic can contain child components.
    get isContainer() {
        return true;
    }

    // Loads a part of the mimic.
    async _loadPart(loadContext) {
        const LoadStep = rs.mimic.LoadStep;
        let continueLoading = false;
        let dto = null;

        if (loadContext.step === LoadStep.UNDEFINED) {
            loadContext.step = LoadStep.PROPERTIES;
            loadContext.result.msg = "Not completed.";
        }

        switch (loadContext.step) {
            case LoadStep.PROPERTIES:
                dto = await this._loadProperties(loadContext);
                loadContext.step++;
                break;

            case LoadStep.COMPONENTS:
                dto = await this._loadComponents(loadContext);
                if (dto.ok && dto.data.endOfComponents) {
                    loadContext.step++;
                }
                break;

            case LoadStep.IMAGES:
                dto = await this._loadImages(loadContext);
                if (dto.ok && dto.data.endOfImages) {
                    loadContext.step++;
                }
                break;

            case LoadStep.FACEPLATES:
                if (this.dependencies.length > 0) {
                    let faceplateMeta = this.dependencies[loadContext.faceplateIndex];

                    if (faceplateMeta.hasError) {
                        continueLoading = true;
                    } else {
                        dto = await this._loadFaceplate(loadContext, faceplateMeta.typeName);
                    }

                    if (++loadContext.faceplateIndex >= this.dependencies.length) {
                        loadContext.step++;
                    }
                } else {
                    loadContext.step++;
                    continueLoading = true;
                }
                break;

            case LoadStep.COMPLETE:
                loadContext.result.ok = true;
                loadContext.result.msg = "";
                break;

            default:
                dto = Dto.fail("Unknown loading step.");
                break;
        }

        if (dto !== null) {
            if (dto.ok) {
                if (dto.data.mimicKey === loadContext.mimicKey) {
                    continueLoading = true;
                } else {
                    loadContext.result.msg = "Stamp mismatch.";
                }
            } else {
                loadContext.result.msg = dto.msg;
            }
        }

        return continueLoading;
    }

    // Loads the mimic properties.
    async _loadProperties(loadContext) {
        console.log(ScadaUtils.getCurrentTime() + " Load mimic properties");
        let response = await fetch(loadContext.controllerUrl + "GetMimicProperties?key=" + loadContext.mimicKey);

        if (response.ok) {
            let dto = await response.json();

            if (dto.ok) {
                if (Array.isArray(dto.data.dependencies)) {
                    for (let sourceDependency of dto.data.dependencies) {
                        let faceplateMeta = new rs.mimic.FaceplateMeta(sourceDependency);
                        this.dependencies.push(faceplateMeta);
                        this.dependencyMap.set(faceplateMeta.typeName, faceplateMeta);
                    }
                }

                this.document = rs.mimic.MimicFactory.parseProperties(dto.data.document);
            }

            return dto;
        } else {
            return Dto.fail(response.statusText);
        }
    }

    // Loads a range of components.
    async _loadComponents(loadContext) {
        console.log(ScadaUtils.getCurrentTime() + " Load components starting with " + loadContext.componentIndex);
        let response = await fetch(loadContext.controllerUrl +
            "GetComponents?key=" + loadContext.mimicKey +
            "&index=" + loadContext.componentIndex +
            "&count=" + rs.mimic.LoadContext.COMPONENTS_TO_REQUEST);

        if (response.ok) {
            let dto = await response.json();

            if (dto.ok && Array.isArray(dto.data.components)) {
                loadContext.componentIndex += dto.data.components.length;

                for (let sourceComponent of dto.data.components) {
                    let component = this.createComponent(sourceComponent);

                    if (component) {
                        this.components.push(component);
                        this.componentMap.set(component.id, component);
                    } else if (sourceComponent.typeName) {
                        loadContext.unknownTypes.add(sourceComponent.typeName);
                    }
                }
            }

            return dto;
        } else {
            return Dto.fail(response.statusText);
        }
    }

    // Loads a range of images.
    async _loadImages(loadContext) {
        console.log(ScadaUtils.getCurrentTime() + " Load images starting with " + loadContext.imageIndex);
        let response = await fetch(loadContext.controllerUrl +
            "GetImages?key=" + loadContext.mimicKey +
            "&index=" + loadContext.imageIndex +
            "&count=" + rs.mimic.LoadContext.COMPONENTS_TO_REQUEST +
            "&size=" + rs.mimic.LoadContext.IMAGE_TOTAL_SIZE);

        if (response.ok) {
            let dto = await response.json();

            if (dto.ok && Array.isArray(dto.data.images)) {
                loadContext.imageIndex += dto.data.images.length;

                for (let sourceImage of dto.data.images) {
                    let image = new rs.mimic.Image(sourceImage);
                    this.images.push(image);
                    this.imageMap.set(image.name, image);
                }
            }

            return dto;
        } else {
            return Dto.fail(response.statusText);
        }
    }

    // Loads a faceplate.
    async _loadFaceplate(loadContext, typeName) {
        console.log(ScadaUtils.getCurrentTime() + ` Load '${typeName}' faceplate`);
        let response = await fetch(loadContext.controllerUrl +
            "GetFaceplate?key=" + loadContext.mimicKey +
            "&typeName=" + typeName);

        if (response.ok) {
            let dto = await response.json();

            if (dto.ok) {
                let faceplate = new rs.mimic.Faceplate(dto.data, typeName);
                this.faceplates.push(faceplate);
                this.faceplateMap.set(typeName, faceplate);
            }

            return dto;
        } else {
            return Dto.fail(response.statusText);
        }
    }

    // Finds a parent and children for each component.
    _defineNesting() {
        rs.mimic.MimicHelper.defineNesting(this, this.components, this.componentMap);
    }

    // Prepares the faceplates for use.
    _prepareFaceplates() {
        for (let faceplate of this.faceplates) {
            for (let faceplateMeta of faceplate.dependencies) {
                let childFaceplate = this.faceplateMap.get(faceplateMeta.typeName);

                if (childFaceplate) {
                    faceplate.faceplates.push(childFaceplate);
                    faceplate.faceplateMap.set(faceplateMeta.typeName, childFaceplate);
                }
            }
        }
    }

    // Prepares the faceplates instances for use.
    _prepareFaceplateInstances() {
        for (let component of this.components) {
            if (component.isFaceplate) {
                let faceplate = mimic.faceplateMap.get(component.typeName);

                if (faceplate) {
                    component.applyModel(faceplate);
                }
            }
        }
    }

    // Clears the mimic.
    clear() {
        super.clear();
        this.dom = null;
        this.renderer = null;
    }

    // Loads the mimic. Returns a LoadResult.
    async load(controllerUrl, mimicKey) {
        let startTime = Date.now();
        console.log(ScadaUtils.getCurrentTime() + " Load mimic with key " + mimicKey)
        this.clear();
        let loadContext = new rs.mimic.LoadContext(controllerUrl, mimicKey);

        while (await this._loadPart(loadContext)) {
            // do nothing
        }

        if (loadContext.result.ok) {
            this._defineNesting();
            this._prepareFaceplates();
            this._prepareFaceplateInstances();

            let endTime = Date.now();
            let endTimeStr = ScadaUtils.getCurrentTime();

            if (loadContext.unknownTypes.size > 0) {
                console.warn(endTimeStr + " Unable to create components of types: " +
                    Array.from(loadContext.unknownTypes).sort().join(", "));
            }

            console.info(endTimeStr + " Mimic loading completed successfully in " + (endTime - startTime) + " ms");
        } else {
            console.error(ScadaUtils.getCurrentTime() + " Mimic loading failed: " + loadContext.result.msg);
        }

        return loadContext.result;
    }

    // Adds the dependency to the mimic or replaces the existing one.
    addDependency(faceplateMeta) {
        let existingDependency = this.dependencyMap.get(faceplateMeta.typeName);

        if (existingDependency) {
            let index = this.dependencies.indexOf(existingDependency);
            this.dependencies[index] = faceplateMeta;
        } else {
            this.dependencies.push(faceplateMeta);
        }

        this.dependencies.sort(); // sort by type name
        this.dependencyMap.set(faceplateMeta.typeName, faceplateMeta);
    }

    // Removes the dependency from the mimic.
    removeDependency(typeName) {
        let dependency = this.dependencyMap.get(typeName);

        if (dependency) {
            let index = this.dependencies.indexOf(dependency);
            this.dependencies.splice(index, 1); // delete
            this.dependencyMap.delete(typeName);
        }
    }

    // Adds the image to the mimic or replaces the existing one.
    addImage(image) {
        let existingImage = this.imageMap.get(image.name);

        if (existingImage) {
            let index = this.images.indexOf(existingImage);
            this.images[index] = image;
        } else {
            this.images.push(image);
        }

        this.images.sort(); // sort by name
        this.imageMap.set(image.name, image);
    }

    // Removes the image from the mimic.
    removeImage(imageName) {
        let image = this.imageMap.get(imageName);

        if (image) {
            let index = this.images.indexOf(image);
            this.images.splice(index, 1); // delete
            this.imageMap.delete(imageName);
        }
    }

    // Adds the component to the mimic. Returns true if the component was added.
    addComponent(component, parent, opt_index, opt_x, opt_y) {
        if (!component || !parent || !parent.isContainer ||
            component.id <= 0 || this.componentMap.has(component.id)) {
            return false;
        }

        if (opt_x && opt_y) {
            component.setLocation(opt_x, opt_y);
        }

        rs.mimic.MimicHelper.addToParent(parent, component, opt_index);
        this.components.push(component);
        this.componentMap.set(component.id, component);
        return true;
    }

    // Updates the parent of the component. Returns true if the parent was updated.
    updateParent(component, parent, opt_index, opt_x, opt_y) {
        if (!component || !parent || !parent.isContainer || component === parent ||
            rs.mimic.MimicHelper.areRelatives(component, parent) /*component contains parent*/) {
            return false;
        }

        if (opt_x && opt_y) {
            component.setLocation(opt_x, opt_y);
        }

        rs.mimic.MimicHelper.removeFromParent(component);
        rs.mimic.MimicHelper.addToParent(parent, component, opt_index);
        return true;
    }

    // Removes the component from the mimic. Returns the removed component.
    removeComponent(componentID) {
        let component = this.componentMap.get(componentID);

        if (component) {
            // get IDs to remove
            let idsToRemove = new Set();
            idsToRemove.add(componentID);

            if (component.isContainer) {
                for (let childComponent of component.getAllChildren()) {
                    idsToRemove.add(childComponent.id);
                }
            }

            // remove components
            this.components = this.components.filter(c => !idsToRemove.has(c.id));
            rs.mimic.MimicHelper.removeFromParent(component);

            for (let id of idsToRemove) {
                this.componentMap.delete(id);
            }
        }

        return component;
    }

    // Gets the parent of a compoment by ID.
    getComponentParent(parentID) {
        return parentID > 0 ? this.componentMap.get(parentID) : this;
    }

    // Returns a string that represents the current object.
    toString() {
        return "Mimic";
    }
};

// Represents a component of a mimic diagram.
rs.mimic.Component = class {
    id = 0;
    typeName = "";
    properties = null;
    bindings = null;
    access = null;
    parentID = 0;
    index = -1;

    parent = null;      // mimic or panel
    children = null;    // top-level child components
    dom = null;         // jQuery objects representing DOM content
    renderer = null;    // renders the component
    isSelected = false; // selected in the editor

    constructor(source) {
        Object.assign(this, source);
    }

    get isContainer() {
        return Array.isArray(this.children);
    }

    get isFaceplate() {
        return false;
    }

    get name() {
        return this.properties?.name ?? "";
    }

    get displayName() {
        return this.name
            ? `[${this.id}] ${this.name} - ${this.typeName}`
            : `[${this.id}] ${this.typeName}`;
    }

    get x() {
        return this.properties ? this.properties.location.x : 0;
    }

    set x(value) {
        if (this.properties) {
            this.properties.location.x = parseInt(value) || 0;
        }
    }

    get y() {
        return this.properties ? this.properties.location.y : 0;
    }

    set y(value) {
        if (this.properties) {
            this.properties.location.y = parseInt(value) || 0;
        }
    }

    get width() {
        return this.properties ? this.properties.size.width : 0;
    }

    set width(value) {
        if (this.properties) {
            this.properties.size.width = parseInt(value) || 0;
        }
    }

    get height() {
        return this.properties ? this.properties.size.height : 0;
    }

    set height(value) {
        if (this.properties) {
            this.properties.size.height = parseInt(value) || 0;
        }
    }

    setLocation(x, y) {
        if (this.properties) {
            this.properties.location.x = parseInt(x) || 0;
            this.properties.location.y = parseInt(y) || 0;
        }
    }

    setSize(width, height) {
        if (this.properties) {
            this.properties.size.width = parseInt(width) || 0;
            this.properties.size.height = parseInt(height) || 0;
        }
    }

    getAllChildren() {
        let allChildren = [];

        function appendChildren(component) {
            if (component.isContainer) {
                for (let child of component.children) {
                    allChildren.push(child);
                    appendChildren(child);
                }
            }
        }

        appendChildren(this);
        return allChildren;
    }

    toPlainObject() {
        return {
            id: this.id,
            name: this.name,
            typeName: this.typeName,
            properties: this.properties,
            bindings: this.bindings,
            access: this.access,
            parentID: this.parentID,
            index: this.index,
            children: this.children ? [] : null
        };
    }

    toString() {
        return this.displayName;
    }
};

// Represents an image of a mimic diagram.
rs.mimic.Image = class {
    name = "";
    mediaType = "";
    data = null;

    constructor(source) {
        Object.assign(this, source);
    }

    get dataUrl() {
        return this.data ? `data:${this.mediaType};base64,${this.data}` : "";
    }

    set dataUrl(value) {
        if (value && value.startsWith("data:")) {
            let index = value.indexOf(";base64,");

            if (index >= 0) {
                this.mediaType = value.substring(5, index);
                this.data = value.substring(index + 8);
                return;
            }
        }

        this.data = null;
    }

    toString() {
        // required for sorting
        return this.name;
    }
};

// Represents information about a faceplate.
rs.mimic.FaceplateMeta = class {
    typeName = "";
    path = "";
    isTransitive = false;
    hasError = false;

    constructor(source) {
        Object.assign(this, source);
    }

    toString() {
        // required for sorting
        return this.typeName;
    }
};

// Represents a faceplate, i.e. a user component.
rs.mimic.Faceplate = class extends rs.mimic.MimicBase {
    typeName = "";

    constructor(source, typeName) {
        super();
        this.clear();
        this.document = source.document ?? {};
        this.typeName = typeName;

        if (Array.isArray(source.dependencies)) {
            for (let sourceDependency of source.dependencies) {
                let faceplateMeta = new rs.mimic.FaceplateMeta(sourceDependency);
                this.dependencies.push(faceplateMeta);
                this.dependencyMap.set(faceplateMeta.typeName, faceplateMeta);
            }
        }

        if (Array.isArray(source.components)) {
            for (let sourceComponent of source.components) {
                this.components.push(sourceComponent);
                this.componentMap.set(sourceComponent.id, sourceComponent);
            }
        }

        if (Array.isArray(source.images)) {
            for (let sourceImage of source.images) {
                let image = new rs.mimic.Image(sourceImage);
                this.images.push(image);
                this.imageMap.set(image.name, image);
            }
        }
    }
};

// Represents a faceplate instance.
rs.mimic.FaceplateInstance = class extends rs.mimic.Component {
    model = null;      // model of the Faceplate type
    components = null; // copy of the model components

    get isContainer() {
        // child components are essential part of the faceplate, it does not accept additional components
        return false;
    }

    get isFaceplate() {
        return true;
    }

    applyModel(faceplate) {
        if (faceplate instanceof rs.mimic.Faceplate) {
            this.properties ??= {};
            this.properties.size ??= ScadaUtils.deepClone(faceplate.document.size);

            this.model = faceplate;
            this.components = [];

            for (let sourceComponent of faceplate.components) {
                let componentCopy = faceplate.copyComponent(sourceComponent);
                componentCopy.parent = this;
                this.components.push(componentCopy);

                if (componentCopy.isFaceplate) {
                    let childFaceplate = faceplate.faceplateMap.get(componentCopy.typeName);
                    componentCopy.applyModel(childFaceplate);
                }
            }

            rs.mimic.MimicHelper.defineNesting(this, this.components);
        }
    }
};

// Enumerations: ActionType, ComparisonOperator, ImageSizeMode, LogicalOperator, LinkTarget, ModalWidth,
//     ContentAlignment, TextDirection
// Structures: Action, Border, CommandArgs, Condition, CornerRadius, Font, ImageCondition, LinkArgs, Padding, Point,
//     PropertyBinding, PropertyExport, Size, VisualState
// Misc: List, ImageConditionList, PropertyBindingList, PropertyExportList, PropertyParser
// No dependencies

// --- Enumerations ---

// Specifies the action types.
rs.mimic.ActionType = class {
    static NONE = "None";
    static DRAW_CHART = "DrawChart";
    static SEND_COMMAND = "SendCommand";
    static OPEN_LINK = "OpenLink";
    static EXECUTE_SCRIPT = "ExecuteScript";
};

// Specifies the comparison operators.
rs.mimic.ComparisonOperator = class ComparisonOperator {
    static NONE = "None";
    static EQUAL = "Equal";
    static NOT_EQUAL = "NotEqual";
    static LESS_THAN = "LessThan";
    static LESS_THAN_EQUAL = "LessThanEqual";
    static GREATER_THAN = "GreaterThan";
    static GREATER_THAN_EQUAL = "GreaterThanEqual";

    static getDisplayName(value) {
        switch (value) {
            case ComparisonOperator.EQUAL:
                return "=";
            case ComparisonOperator.NOT_EQUAL:
                return "<>";
            case ComparisonOperator.LESS_THAN:
                return "<";
            case ComparisonOperator.LESS_THAN_EQUAL:
                return "<=";
            case ComparisonOperator.GREATER_THAN:
                return ">";
            case ComparisonOperator.GREATER_THAN_EQUAL:
                return ">=";
            default:
                return "";
        }
    }
};

// Specifies how an image is positioned within a component.
rs.mimic.ImageSizeMode = class {
    static NORMAL = "Normal";
    static STRETCH = "Stretch";
    static CENTER = "Center";
    static ZOOM = "Zoom";
}

// Specifies the logical operators.
rs.mimic.LogicalOperator = class LogicalOperator {
    static NONE = "None";
    static AND = "And";
    static OR = "Or";

    static getDisplayName(value) {
        switch (value) {
            case LogicalOperator.AND:
                return "&&";
            case LogicalOperator.OR:
                return "||";
            default:
                return "";
        }
    }
};

// Specifies the link targets.
rs.mimic.LinkTarget = class {
    static SELF = "Self";
    static NEW_TAB = "NewTab";
    static NEW_MODAL = "NewModal";
};

// Specifies the widths of a modal dialog.
rs.mimic.ModalWidth = class {
    static NORMAL = "Normal";
    static SMALL = "Small";
    static LARGE = "Large";
    static EXTRA_LARGE = "ExtraLarge";
};

// Specifies the alignments of component content.
rs.mimic.ContentAlignment = class {
    static TOP_LEFT = "TopLeft";
    static TOP_CENTER = "TopCenter";
    static TOP_RIGHT = "TopRight";
    static MIDDLE_LEFT = "MiddleLeft";
    static MIDDLE_CENTER = "MiddleCenter";
    static MIDDLE_RIGHT = "MiddleRight";
    static BOTTOM_LEFT = "BottomLeft";
    static BOTTOM_CENTER = "BottomCenter";
    static BOTTOM_RIGHT = "BottomRight";
};

// Specifies the text directions.
rs.mimic.TextDirection = class {
    static HORIZONTAL = "Horizontal";
    static VERTICAL90 = "Vertical90";
    static VERTICAL270 = "Vertical270";
}

// --- Structures ---

// Represents an action.
rs.mimic.Action = class Action {
    actionType = rs.mimic.ActionType.NONE;
    chartArgs = "";
    commandArgs = new rs.mimic.CommandArgs();
    linkArgs = new rs.mimic.LinkArgs();
    script = "";

    get typeName() {
        return "Action";
    }

    static parse(source) {
        const PropertyParser = rs.mimic.PropertyParser;
        let action = new Action();

        if (source) {
            action.actionType = PropertyParser.parseString(source.actionType, action.actionType);
            action.chartArgs = PropertyParser.parseString(source.chartArgs);
            action.commandArgs = rs.mimic.CommandArgs.parse(source.commandArgs);
            action.linkArgs = rs.mimic.LinkArgs.parse(source.linkArgs);
            action.script = PropertyParser.parseString(source.script);
        }

        return action;
    }
};

// Represents a border.
rs.mimic.Border = class Border {
    width = 0;
    color = ""

    get typeName() {
        return "Border";
    }

    static parse(source) {
        const PropertyParser = rs.mimic.PropertyParser;
        let border = new Border();

        if (source) {
            border.width = PropertyParser.parseInt(source.width);
            border.color = PropertyParser.parseString(source.color);
        }

        return border;
    }
};

// Represents arguments of the SEND_COMMAND action.
rs.mimic.CommandArgs = class CommandArgs {
    showDialog = true;
    cmdVal = 0.0;

    get typeName() {
        return "CommandArgs";
    }

    static parse(source) {
        const PropertyParser = rs.mimic.PropertyParser;
        let commandArgs = new CommandArgs();

        if (source) {
            commandArgs.showDialog = PropertyParser.parseBool(source.showDialog, true);
            commandArgs.cmdVal = PropertyParser.parseFloat(source.cmdVal);
        }

        return commandArgs;
    }
};

// Represents an abstract condition.
rs.mimic.Condition = class Condition {
    comparisonOper1 = rs.mimic.ComparisonOperator.NONE;
    comparisonArg1 = 0.0;
    logicalOper = rs.mimic.LogicalOperator.NONE;
    comparisonOper2 = rs.mimic.ComparisonOperator.NONE;
    comparisonArg2 = 0.0;

    get typeName() {
        return "Condition";
    }

    get displayName() {
        const ComparisonOperator = rs.mimic.ComparisonOperator;
        const LogicalOperator = rs.mimic.LogicalOperator;
        let co1 = ComparisonOperator.getDisplayName(this.comparisonOper1);
        let co2 = ComparisonOperator.getDisplayName(this.comparisonOper2);
        let lo = LogicalOperator.getDisplayName(this.logicalOper);
        let displayName = "";

        if (co1) {
            displayName += `X ${co1} ${this.comparisonArg1}`;

            if (co2 && lo) {
                displayName += ` ${lo} X ${co2} ${this.comparisonArg2}`;
            }
        }

        return displayName;
    }

    _copyFrom(source) {
        const PropertyParser = rs.mimic.PropertyParser;
        this.comparisonOper1 = PropertyParser.parseString(source.comparisonOper1, this.comparisonOper1);
        this.comparisonArg1 = PropertyParser.parseFloat(source.comparisonArg1);
        this.logicalOper = PropertyParser.parseString(source.logicalOper, this.logicalOper);
        this.comparisonOper2 = PropertyParser.parseString(source.comparisonOper2, this.comparisonOper2);
        this.comparisonArg2 = PropertyParser.parseFloat(source.comparisonArg2);
    }

    static parse(source) {
        let condition = new Condition();

        if (source) {
            condition._copyFrom(source);
        }

        return condition;
    }
};

// Represents a corner radius.
rs.mimic.CornerRadius = class CornerRadius {
    topLeft = 0;
    topRight = 0;
    bottomRight = 0;
    bottomLeft = 0;

    get typeName() {
        return "CornerRadius";
    }

    static parse(source) {
        const PropertyParser = rs.mimic.PropertyParser;
        let cornerRadius = new CornerRadius();

        if (source) {
            cornerRadius.topLeft = PropertyParser.parseInt(source.topLeft);
            cornerRadius.topRight = PropertyParser.parseInt(source.topRight);
            cornerRadius.bottomRight = PropertyParser.parseInt(source.bottomRight);
            cornerRadius.bottomLeft = PropertyParser.parseInt(source.bottomLeft);
        }

        return cornerRadius;
    }
};

// Represents a font.
rs.mimic.Font = class Font {
    name = "";
    size = 16;
    bold = false;
    italic = false;
    underline = false;

    get typeName() {
        return "Font";
    }

    static parse(source) {
        const PropertyParser = rs.mimic.PropertyParser;
        let font = new Font();

        if (source) {
            font.name = PropertyParser.parseString(source.name);
            font.size = PropertyParser.parseInt(source.size, font.size);
            font.bold = PropertyParser.parseBool(source.bold);
            font.italic = PropertyParser.parseBool(source.italic);
            font.underline = PropertyParser.parseBool(source.underline);
        }

        return font;
    }
};

// Represents an image condition.
rs.mimic.ImageCondition = class ImageCondition extends rs.mimic.Condition {
    imageName = "";

    get typeName() {
        return "ImageCondition";
    }

    static parse(source) {
        const PropertyParser = rs.mimic.PropertyParser;
        let imageCondition = new ImageCondition();

        if (source) {
            imageCondition.imageName = PropertyParser.parseString(source.imageName);
            imageCondition._copyFrom(source);
        }

        return imageCondition;
    }
};

// Represents arguments of the OPEN_LINK action.
rs.mimic.LinkArgs = class LinkArgs {
    url = "";
    target = rs.mimic.LinkTarget.SELF;
    viewID = 0;
    modalWidth = rs.mimic.ModalWidth.NORMAL;
    modalHeight = 0;

    get typeName() {
        return "LinkArgs";
    }

    static parse(source) {
        const PropertyParser = rs.mimic.PropertyParser;
        let linkArgs = new LinkArgs();

        if (source) {
            linkArgs.url = PropertyParser.parseString(source.url);
            linkArgs.target = PropertyParser.parseString(source.target, linkArgs.target);
            linkArgs.viewID = PropertyParser.parseInt(source.viewID);
            linkArgs.modalWidth = PropertyParser.parseString(source.modalWidth, linkArgs.modalWidth);
            linkArgs.modalHeight = PropertyParser.parseInt(source.modalHeight);
        }

        return linkArgs;
    }
};

// Represents paddings.
rs.mimic.Padding = class Padding {
    top = 0;
    right = 0;
    bottom = 0;
    left = 0;

    get typeName() {
        return "Padding";
    }

    static parse(source) {
        const PropertyParser = rs.mimic.PropertyParser;
        let padding = new Padding();

        if (source) {
            padding.top = PropertyParser.parseInt(source.top);
            padding.right = PropertyParser.parseInt(source.right);
            padding.bottom = PropertyParser.parseInt(source.bottom);
            padding.left = PropertyParser.parseInt(source.left);
        }

        return padding;
    }
};

// Represents a point.
rs.mimic.Point = class Point {
    x = 0;
    y = 0;

    get typeName() {
        return "Point";
    }

    static parse(source) {
        const PropertyParser = rs.mimic.PropertyParser;
        let point = new Point();

        if (source) {
            point.x = PropertyParser.parseInt(source.x);
            point.y = PropertyParser.parseInt(source.y);
        }

        return point;
    }
}

// Represents a property binding.
rs.mimic.PropertyBinding = class PropertyBinding {
    propertyName = "";
    dataSource = "";
    dataMember = "";
    format = "";

    get typeName() {
        return "PropertyBinding";
    }

    get displayName() {
        return this.propertyName;
    }

    static parse(source) {
        const PropertyParser = rs.mimic.PropertyParser;
        let propertyBinding = new PropertyBinding();

        if (source) {
            propertyBinding.propertyName = PropertyParser.parseString(source.propertyName);
            propertyBinding.dataSource = PropertyParser.parseString(source.dataSource);
            propertyBinding.dataMember = PropertyParser.parseString(source.dataMember);
            propertyBinding.format = PropertyParser.parseString(source.format);
        }

        return propertyBinding;
    }
};

// Represents an exported property.
rs.mimic.PropertyExport = class PropertyExport {
    name = "";
    path = "";

    get typeName() {
        return "PropertyExport";
    }

    get displayName() {
        return this.name;
    }

    static parse(source) {
        const PropertyParser = rs.mimic.PropertyParser;
        let propertyExport = new PropertyExport();

        if (source) {
            propertyExport.name = PropertyParser.parseString(source.name);
            propertyExport.path = PropertyParser.parseString(source.path);
        }

        return propertyExport;
    }
};

// Represents a size.
rs.mimic.Size = class Size {
    width = 100;
    height = 100;

    get typeName() {
        return "Size";
    }

    static parse(source) {
        const PropertyParser = rs.mimic.PropertyParser;
        let size = new Size();

        if (source) {
            size.width = PropertyParser.parseInt(source.width);
            size.height = PropertyParser.parseInt(source.height);
        }

        return size;
    }
}

// Represents a visual state.
rs.mimic.VisualState = class VisualState {
    backColor = "";
    foreColor = "";
    borderColor = "";

    get typeName() {
        return "VisualState";
    }

    static parse(source) {
        const PropertyParser = rs.mimic.PropertyParser;
        let visualState = new VisualState();

        if (source) {
            visualState.backColor = PropertyParser.parseString(source.backColor);
            visualState.foreColor = PropertyParser.parseString(source.foreColor);
            visualState.borderColor = PropertyParser.parseString(source.borderColor);
        }

        return visualState;
    }
};

// --- Misc ---

// Represents a list that can create new items.
rs.mimic.List = class List extends Array {
    constructor(createItemFn) {
        super();

        if (createItemFn instanceof Function) {
            Object.getPrototypeOf(this).createItem = function () {
                return createItemFn.call(this);
            };
        }
    }
}

// Represents a list of ImageCondition items.
rs.mimic.ImageConditionList = class extends rs.mimic.List {
    constructor() {
        super(() => {
            return new rs.mimic.ImageCondition();
        });
    }
}

// Represents a list of PropertyBinding items.
rs.mimic.PropertyBindingList = class extends rs.mimic.List {
    constructor() {
        super(() => {
            return new rs.mimic.PropertyBinding();
        });
    }
}

// Represents a list of PropertyExport items.
rs.mimic.PropertyExportList = class extends rs.mimic.List {
    constructor() {
        super(() => {
            return new rs.mimic.PropertyExport();
        });
    }
}

// Parses property values ​​from strings and objects.
rs.mimic.PropertyParser = class {
    static parseBool(string, defaultValue = false) {
        return !string
            ? defaultValue
            : string === "true" || string === "True";
    }

    static parseFloat(string, defaultValue = 0.0) {
        let number = Number.parseFloat(string);
        return Number.isFinite(number) ? number : defaultValue;
    }

    static parseInt(string, defaultValue = 0) {
        let number = Number.parseInt(string);
        return Number.isFinite(number) ? number : defaultValue;
    }

    static parseString(source, defaultValue = "") {
        if (source instanceof String) {
            return source;
        } else if (source && source.toString instanceof Function) {
            return source.toString();
        } else {
            return defaultValue;
        }
    }

    static parseImageConditions(source) {
        const ImageCondition = rs.mimic.ImageCondition;
        let imageConditions = new rs.mimic.ImageConditionList();

        if (Array.isArray(source)) {
            for (let sourceItem of source) {
                imageConditions.push(ImageCondition.parse(sourceItem));
            }
        }

        return imageConditions;
    }

    static parsePropertyBindings(source) {
        const PropertyBinding = rs.mimic.PropertyBinding;
        let propertyBindings = new rs.mimic.PropertyBindingList();

        if (Array.isArray(source)) {
            for (let sourceItem of source) {
                propertyBindings.push(PropertyBinding.parse(sourceItem));
            }
        }

        return propertyBindings;
    }

    static parsePropertyExports(source) {
        const PropertyExport = rs.mimic.PropertyExport;
        let propertyExports = new rs.mimic.PropertyExportList();

        if (Array.isArray(source)) {
            for (let sourceItem of source) {
                propertyExports.push(PropertyExport.parse(sourceItem));
            }
        }

        return propertyExports;
    }
}

// Contains classes: Renderer, MimicRenderer, ComponentRenderer,
//     TextRenderer, PictureRenderer, PanelRenderer, RenderContext, RendererSet, UnitedRenderer
// Depends on jquery, scada-common.js, mimic-common.js

// Represents a renderer of a mimic or component.
rs.mimic.Renderer = class {
    // Gets a value indocating whether the renderer supports DOM update.
    get canUpdateDom() {
        return false;
    }

    // Sets the left and top of the specified jQuery object.
    _setLocation(jqObj, location) {
        jqObj.css({
            "left": location.x + "px",
            "top": location.y + "px"
        });
    }

    // Sets the width and height of the specified jQuery object.
    _setSize(jqObj, size) {
        jqObj.css({
            "width": size.width + "px",
            "height": size.height + "px"
        });
    }

    // Sets the background image of the specified jQuery object.
    _setBackgroundImage(jqObj, image) {
        jqObj.css("background-image", this._imageToDataUrlCss(image));
    }

    // Returns a css property value for the image data URI.
    _imageToDataUrlCss(image) {
        return image ? "url('" + image.dataUrl + "')" : "";
    }

    // Creates a component DOM according to the component model. Returns a jQuery object.
    createDom(component, renderContext) {
        return null;
    }

    // Updates the existing component DOM according to the component model.
    updateDom(component, renderContext) {
    }

    // Updates the component view according to the current channel data.
    update(component, renderContext) {
    }

    // Sets the location of the component DOM.
    setLocation(component, x, y) {
        if (component.dom) {
            this._setLocation(component.dom, {
                x: x,
                y: y
            });
        }
    }

    // Gets the component location from its DOM.
    getLocation(component) {
        if (component.dom) {
            let position = component.dom.position();
            return {
                x: parseInt(position.left),
                y: parseInt(position.top)
            };
        } else {
            return {
                x: 0,
                y: 0
            };
        }
    }

    // Sets the size of the component DOM.
    setSize(component, width, height) {
        if (component.dom) {
            this._setSize(component.dom, {
                width: width,
                height: height
            });
        }
    }

    // Gets the component size from its DOM.
    getSize(component) {
        if (component.dom) {
            return {
                width: parseInt(component.dom.outerWidth()),
                height: parseInt(component.dom.outerHeight())
            };
        } else {
            return {
                width: 0,
                height: 0
            };
        }
    }

    // Appends the child DOM into the parent DOM.
    static appendChild(parent, child) {
        if (parent.dom && child.dom) {
            parent.dom.append(child.dom);
        }
    }

    // Removes the component DOM from the mimic keeping data associated with the removed elements.
    static detach(component) {
        component.dom?.detach();
    }

    // Removes the component DOM from the mimic.
    static remove(component) {
        component.dom?.remove();
    }

    // Arranges the child components according to their order.
    static arrangeChildren(parent) {
        if (parent.children && parent.dom) {
            for (let component of parent.children) {
                component.dom?.detach();
            }

            for (let component of parent.children) {
                if (component.dom) {
                    parent.dom.append(component.dom);
                }
            }
        }
    }
};

// Represents a mimic renderer.
rs.mimic.MimicRenderer = class MimicRenderer extends rs.mimic.Renderer {
    static _GRID_COLOR = "#dee2e6"; // gray-300

    get canUpdateDom() {
        return true;
    }

    static _createGrid(gridSize, mimicSize) {
        // create canvas
        let canvasElem = $("<canvas class='grid'></canvas>");
        let canvas = canvasElem[0];
        let width = canvas.width = mimicSize.width;
        let height = canvas.height = mimicSize.height;

        // prepare drawing context
        let context = canvas.getContext("2d");
        context.lineWidth = 1;
        context.strokeStyle = MimicRenderer._GRID_COLOR;

        // draw grids with small and large cells
        MimicRenderer._drawGrid(context, width, height, gridSize, [1, 1]);
        MimicRenderer._drawGrid(context, width, height, gridSize * 10);
        return canvasElem;
    }

    static _drawGrid(context, width, height, gridStep, dashSegments = []) {
        const adj = 0.5; // adjustment for sharpness of lines

        // draw vertical lines
        for (let x = gridStep; x < width; x += gridStep) {
            context.beginPath();
            context.setLineDash(dashSegments);
            context.moveTo(x + adj, 0);
            context.lineTo(x + adj, height);
            context.stroke();
        }

        // draw horizontal lines
        for (let y = gridStep; y < height; y += gridStep) {
            context.beginPath();
            context.setLineDash(dashSegments);
            context.moveTo(0, y + adj);
            context.lineTo(width, y + adj);
            context.stroke();
        }
    }

    createDom(mimic, renderContext) {
        let mimicElem = $("<div class='mimic'></div>");
        mimic.dom = mimicElem;

        if (renderContext.editMode && renderContext.editorOptions &&
            renderContext.editorOptions.showGrid && renderContext.editorOptions.gridStep > 1) {
            mimicElem.append(MimicRenderer._createGrid(renderContext.editorOptions.gridStep, mimic.document.size));
        }

        this.updateDom(mimic, renderContext);
        return mimicElem;
    }

    updateDom(mimic, renderContext) {
        if (mimic.dom) {
            let mimicElem = mimic.dom.first();
            this._setSize(mimicElem, mimic.document.size);
        }
    }
};

// Represents a component renderer.
rs.mimic.ComponentRenderer = class extends rs.mimic.Renderer {
    createDom(component, renderContext) {
        let componentElem = $("<div class='comp'></div>")
            .attr("id", "comp" + renderContext.idPrefix + component.id)
            .attr("data-id", component.id);

        if (renderContext.editMode) {
            if (!renderContext.faceplateMode && component.isContainer) {
                componentElem.addClass("container")
            }

            if (component.isSelected) {
                componentElem.addClass("selected")
            }
        }

        component.dom = componentElem;
        return componentElem;
    }

    updateLocation(component) {
        if (component.dom) {
            this._setLocation(component.dom, component.properties.location);
        }
    }

    updateSelected(component) {
        if (component.dom) {
            component.dom.toggleClass("selected", component.isSelected);
        }
    }

    allowResizing(component) {
        return true;
    }
};

// Represents a text component renderer.
rs.mimic.TextRenderer = class extends rs.mimic.ComponentRenderer {
    createDom(component, renderContext) {
        let textElem = super.createDom(component, renderContext);
        let props = component.properties;
        textElem.addClass("text").text(props.text);
        this._setLocation(textElem, props.location);
        this._setSize(textElem, props.size);
        return textElem;
    }
};

// Represents a picture component renderer.
rs.mimic.PictureRenderer = class extends rs.mimic.ComponentRenderer {
    createDom(component, renderContext) {
        let pictureElem = super.createDom(component, renderContext);
        let props = component.properties;
        pictureElem.addClass("picture");
        this._setLocation(pictureElem, props.location);
        this._setSize(pictureElem, props.size);
        this._setBackgroundImage(pictureElem, renderContext.getImage(props.imageName));
        return pictureElem;
    }
};

// Represents a panel component renderer.
rs.mimic.PanelRenderer = class extends rs.mimic.ComponentRenderer {
    get canUpdateDom() {
        return true;
    }

    createDom(component, renderContext) {
        let panelElem = super.createDom(component, renderContext);
        panelElem.addClass("panel");
        this.updateDom(component, renderContext);
        return panelElem;
    }

    updateDom(component, renderContext) {
        if (component.dom) {
            let panelElem = component.dom.first();
            let props = component.properties;
            this._setLocation(panelElem, props.location);
            this._setSize(panelElem, props.size);
        }
    }
};

// Represents a faceplate renderer.
rs.mimic.FaceplateRenderer = class extends rs.mimic.ComponentRenderer {
    get canUpdateDom() {
        return true;
    }

    createDom(component, renderContext) {
        let faceplateElem = super.createDom(component, renderContext);
        faceplateElem.addClass("faceplate");
        this.updateDom(component, renderContext);
        return faceplateElem;
    }

    updateDom(component, renderContext) {
        if (component.dom) {
            let faceplateElem = component.dom.first();
            let props = component.properties;
            this._setLocation(faceplateElem, props.location);
            this._setSize(faceplateElem, props.size);
        }
    }
};

// Encapsulates information about a rendering operation.
rs.mimic.RenderContext = class {
    editMode = false;
    editorOptions = null;
    faceplateMode = false;
    imageMap = null;
    idPrefix = "";
    unknownTypes = null;

    constructor(source) {
        Object.assign(this, source);
    }

    getImage(imageName) {
        return this.imageMap instanceof Map ? this.imageMap.get(imageName) : null;
    }
};

// Contains renderers for a mimic and its components.
rs.mimic.RendererSet = class {
    static mimicRenderer = new rs.mimic.MimicRenderer();
    static faceplateRenderer = new rs.mimic.FaceplateRenderer();
    static componentRenderers = new Map([
        ["Text", new rs.mimic.TextRenderer()],
        ["Picture", new rs.mimic.PictureRenderer()],
        ["Panel", new rs.mimic.PanelRenderer()]
    ]);
};

// Renders a mimic using appropriate renderers.
rs.mimic.UnitedRenderer = class {
    mimic;
    editMode;
    editorOptions;

    constructor(mimic, editMode) {
        this.mimic = mimic;
        this.editMode = editMode;
        this.editorOptions = null;
    }

    // Appends the component DOM to its parent.
    _appendToParent(component) {
        if (component.parent) {
            rs.mimic.Renderer.appendChild(component.parent, component);
        }
    }

    // Creates a component DOM.
    _createComponentDom(component, renderContext) {
        if (component.isFaceplate) {
            this._createFaceplateDom(component, renderContext);
        } else {
            let renderer = rs.mimic.RendererSet.componentRenderers.get(component.typeName);

            if (renderer) {
                component.renderer = renderer;
                renderer.createDom(component, renderContext);
                this._appendToParent(component);
            } else {
                renderContext.unknownTypes?.add(component.typeName);
            }
        }
    }

    // Creates a faceplate DOM.
    _createFaceplateDom(faceplateInstance, renderContext) {
        if (!faceplateInstance.model) {
            renderContext.unknownTypes?.add(faceplateInstance.typeName);
            return;
        }

        let faceplateContext = new rs.mimic.RenderContext({
            editMode: this.editMode,
            editorOptions: this.editorOptions,
            faceplateMode: true,
            imageMap: faceplateInstance.model.imageMap,
            idPrefix: renderContext.idPrefix,
            unknownTypes: renderContext.unknownTypes
        });
        let renderer = rs.mimic.RendererSet.faceplateRenderer;
        faceplateInstance.renderer = renderer;
        renderer.createDom(faceplateInstance, faceplateContext);
        this._appendToParent(faceplateInstance);
        faceplateContext.idPrefix += faceplateInstance.id + "-";

        for (let component of faceplateInstance.components) {
            this._createComponentDom(component, faceplateContext);
        }
    }

    // Creates a mimic DOM according to the mimic model. Returns a jQuery object.
    createMimicDom() {
        let startTime = Date.now();
        let renderContext = new rs.mimic.RenderContext({
            editMode: this.editMode,
            editorOptions: this.editorOptions,
            imageMap: this.mimic.imageMap,
            unknownTypes: new Set()
        });
        let renderer = rs.mimic.RendererSet.mimicRenderer;
        this.mimic.renderer = renderer;
        renderer.createDom(this.mimic, renderContext);

        for (let component of this.mimic.components) {
            this._createComponentDom(component, renderContext);
        }

        if (renderContext.unknownTypes.size > 0) {
            console.warn("Unable to render components of types: " +
                Array.from(renderContext.unknownTypes).sort().join(", "));
        }

        if (this.mimic.dom) {
            console.info("Mimic DOM created in " + (Date.now() - startTime) + " ms");
            return this.mimic.dom;
        } else {
            return $();
        }
    }

    // Updates a mimic DOM according to the mimic model.
    updateMimicDom() {
        rs.mimic.RendererSet.mimicRenderer.updateDom(this.mimic);
    }

    // Creates a component DOM according to the component model. Returns a jQuery object.
    createComponentDom(component) {
        this._createComponentDom(component, new rs.mimic.RenderContext({
            editMode: this.editMode,
            editorOptions: this.editorOptions,
            imageMap: this.mimic.imageMap
        }));
        return component.dom ?? $();
    }

    // Updates the component DOM according to the component model.
    updateComponentDom(component) {
        if (component.dom && component.renderer) {
            let renderContext = new rs.mimic.RenderContext({
                editMode: this.editMode,
                editorOptions: this.editorOptions
            });

            if (component.isFaceplate) {
                renderContext.faceplateMode = true;
                renderContext.imageMap = component.model.imageMap;
                component.renderer.updateDom(component, renderContext);
            } else {
                renderContext.imageMap = mimic.imageMap;

                if (component.renderer.canUpdateDom) {
                    component.renderer.updateDom(component, renderContext);
                } else {
                    let oldDom = component.dom;
                    component.renderer.createDom(component, renderContext);
                    oldDom.replaceWith(component.dom);
                }
            }
        }
    }

    // Arranges the child component DOMs according to the parent's model.
    arrangeChildren(parent) {
        if (parent.children) {
            // detach components
            for (let component of parent.children) {
                if (component.dom) {
                    component.dom.detach();
                }
            }

            // append components
            for (let component of parent.children) {
                this._appendToParent(component);
            }
        }
    }
};
