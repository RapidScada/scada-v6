// Contains classes: LoadStep, LoadResult, LoadContext, ScaleType, Scale, ObjectHelper
// Depends on scada-common.js

// Namespaces
var rs = rs ?? {};
rs.mimic = rs.mimic ?? {};

// Specifies the steps for loading a mimic.
rs.mimic.LoadStep = class {
    static UNDEFINED = 0;
    static PROPERTIES = 1;
    static FACEPLATES = 2;
    static COMPONENTS = 3;
    static IMAGES = 4;
    static COMPLETE = 5;
};

// Represents a result of loading a mimic.
rs.mimic.LoadResult = class {
    ok = false;
    warn = false;
    msg = "";
};

// Represents a context of a loading operation.
rs.mimic.LoadContext = class {
    static COMPONENTS_TO_REQUEST = 1000;
    static IMAGES_TO_REQUEST = 100;
    static IMAGE_TOTAL_SIZE = 1048576;

    controllerUrl;
    mimicKey;
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

// Specifies the scale types. Corresponds to ScaleType.cs
rs.mimic.ScaleType = class {
    static NUMERIC = 0;
    static FIT_SCREEN = 1;
    static FIT_WIDTH = 2;
};

// Represents a scale.
rs.mimic.Scale = class Scale {
    static _TYPE_KEY = "Mimic.ScaleType";
    static _VALUE_KEY = "Mimic.ScaleValue";
    static _VALUES = [0.1, 0.25, 0.5, 0.75, 1, 1.25, 1.5, 2, 3, 4, 5, 10];
    static _MIN = 0.1;
    static _MAX = 10;

    type;
    value;

    constructor(opt_type, opt_value) {
        this.type = opt_type ?? rs.mimic.ScaleType.NUMERIC;
        this.value = Scale._normalize(opt_value);
    }

    static _normalize(value) {
        if (Number.isFinite(value)) {
            if (value < Scale._MIN) {
                return Scale._MIN;
            } if (value > Scale._MAX) {
                return Scale._MAX;
            } else {
                const factor = 1000000;
                return Math.trunc(value * factor) / factor;
            }
        } else {
            return 1;
        }
    }

    setValue(value) {
        this.value = Scale._normalize(value);
    }

    save(storage) {
        ScadaUtils.setStorageItem(storage, Scale._TYPE_KEY, this.type);
        ScadaUtils.setStorageItem(storage, Scale._VALUE_KEY, this.value);
    }

    load(storage) {
        this.type = parseInt(ScadaUtils.getStorageItem(storage, Scale._TYPE_KEY)) || this.type;
        this.value = parseFloat(ScadaUtils.getStorageItem(storage, Scale._VALUE_KEY)) || this.value;
    }

    getPrev() {
        for (let i = Scale._VALUES.length - 1; i >= 0; i--) {
            let prevVal = Scale._VALUES[i];

            if (scale.value > prevVal) {
                return new Scale(rs.mimic.ScaleType.NUMERIC, prevVal);
            }
        }

        return this;
    }

    getNext() {
        for (let i = 0, len = Scale._VALUES.length; i < len; i++) {
            let nextVal = Scale._VALUES[i];

            if (scale.value < nextVal) {
                return new Scale(rs.mimic.ScaleType.NUMERIC, nextVal);
            }
        }

        return this;
    }
}

// Provides access to the object properties.
rs.mimic.ObjectHelper = class ObjectHelper {
    // Gets the child object specified by the property chain.
    static _getObjectToUpdate(obj, propertyChain, chainIndex) {
        let objectToUpdate = obj;

        for (let i = chainIndex; i < propertyChain.length - 1; i++) {
            let propertyName = propertyChain[i];

            if (objectToUpdate instanceof Object && objectToUpdate.hasOwnProperty(propertyName)) {
                objectToUpdate = objectToUpdate[propertyName];
            } else {
                objectToUpdate = null;
                break;
            }
        }

        return objectToUpdate;
    }

    // Updates the value of the object property.
    static _updateValue(objectToUpdate, propertyName, newValue) {
        let curValue = objectToUpdate[propertyName];

        if (typeof curValue === "number") {
            objectToUpdate[propertyName] = Number(newValue) || 0;
        } else if (typeof curValue === "string") {
            objectToUpdate[propertyName] = String(newValue);
        } else if (typeof curValue === "boolean") {
            objectToUpdate[propertyName] = Boolean(newValue);
        } else if (curValue instanceof Object) {
            let newValueIsObject = newValue instanceof Object;

            for (let childPropertyName of Object.keys(curValue)) {
                ObjectHelper._updateValue(curValue, childPropertyName,
                    newValueIsObject ? newValue[childPropertyName] : null);
            }
        }
    }

    // Gets the value of the object property. Property chain is an array of property names.
    static getPropertyValue(obj, propertyChain, chainIndex) {
        let objectToUpdate = ObjectHelper._getObjectToUpdate(obj, propertyChain, chainIndex);

        if (objectToUpdate instanceof Object && propertyChain.length > chainIndex) {
            let propertyName = propertyChain.at(-1); // last
            return objectToUpdate[propertyName];
        }
    }

    // Sets the object property to the specified value keeping the data type unchanged.
    static setPropertyValue(obj, propertyChain, chainIndex, value) {
        let objectToUpdate = ObjectHelper._getObjectToUpdate(obj, propertyChain, chainIndex);

        if (objectToUpdate instanceof Object && propertyChain.length > chainIndex) {
            let propertyName = propertyChain.at(-1); // last
            ObjectHelper._updateValue(objectToUpdate, propertyName, value);
        }
    }

    // Creates a new value by merging the source value to the base value.
    static mergeValues(baseValue, sourceValue) {
        if (baseValue === null || baseValue === undefined ||
            sourceValue === null || sourceValue === undefined) {
            return baseValue;
        }

        if (typeof baseValue === "number") {
            return Number(sourceValue) || 0;
        } else if (typeof baseValue === "string") {
            return String(sourceValue);
        } else if (typeof baseValue === "boolean") {
            return Boolean(sourceValue);
        } else if (baseValue instanceof Object) {
            let mergedObject = ScadaUtils.deepClone(baseValue);
            let sourceIsObject = sourceValue instanceof Object;

            for (let [name, value] of Object.entries(baseValue)) {
                mergedObject[name] = ObjectHelper.mergeValues(value, sourceIsObject ? sourceValue[name] : null);
            }

            return mergedObject;
        } else {
            return baseValue;
        }
    }
}

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
            type: BasicType.LIST,
            subtype: Subtype.PROPERTY_EXPORT
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
            type: BasicType.LIST,
            subtype: Subtype.PROPERTY_BINDING
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
            name: "border",
            displayName: "Border",
            category: KnownCategory.APPEARANCE,
            type: BasicType.STRUCT,
            subtype: Subtype.BORDER
        }));

        this.add(new PropertyDescriptor({
            name: "cornerRadius",
            displayName: "Corner radius",
            category: KnownCategory.APPEARANCE,
            type: BasicType.STRUCT,
            subtype: Subtype.CORNER_RADIUS
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
            type: BasicType.STRUCT,
            subtype: Subtype.VISUAL_STATE
        }));

        this.add(new PropertyDescriptor({
            name: "clickAction",
            displayName: "On click",
            category: KnownCategory.BEHAVIOR,
            type: BasicType.STRUCT,
            subtype: Subtype.ACTION
        }));

        this.add(new PropertyDescriptor({
            name: "disabledState",
            displayName: "On disabled",
            category: KnownCategory.BEHAVIOR,
            type: BasicType.STRUCT,
            subtype: Subtype.VISUAL_STATE
        }));

        this.add(new PropertyDescriptor({
            name: "hoverState",
            displayName: "On hover",
            category: KnownCategory.BEHAVIOR,
            type: BasicType.STRUCT,
            subtype: Subtype.VISUAL_STATE
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
            type: BasicType.STRUCT,
            subtype: Subtype.PADDING
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
            type: BasicType.LIST,
            subtype: Subtype.IMAGE_CONDITION
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
            type: BasicType.STRUCT,
            subtype: Subtype.PADDING
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

// Contains classes: MimicFactory, ComponentFactory, RegularComponentFactory,
//     TextFactory, PictureFactory, PanelFactory, FaceplateFactory, FactorySet
// Depends on mimic-common.js, mimic-model.js, mimic-model-subtypes.js

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
            propertyExports: rs.mimic.PropertyExportList.parse(sourceProps.propertyExports),

            // layout
            size: rs.mimic.Size.parse(sourceProps.size)
        };
    }
}

// Represents an abstract component factory.
rs.mimic.ComponentFactory = class {
    // Copies the properties from the source object.
    _copyProperties(component, source) {
        component.id = source.id;
        component.typeName = source.typeName;
        component.properties = this.parseProperties(source.properties);
        component.properties.typeName = source.typeName;
        component.bindings = source.bindings;
        component.parentID = source.parentID;
    }

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
            propertyBindings: rs.mimic.PropertyBindingList.parse(sourceProps.propertyBindings),

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
        this._copyProperties(component, source);
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
        Object.assign(properties, {
            font: new rs.mimic.Font({ inherit: true }),
            text: "Text",
            textAlign: rs.mimic.ContentAlignment.TOP_LEFT,
            textDirection: rs.mimic.TextDirection.HORIZONTAL,
            wordWrap: false
        });

        // layout
        Object.assign(properties, {
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
            conditions: rs.mimic.ImageConditionList.parse(sourceProps.conditions),
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
    faceplate; // can be null

    constructor(faceplate) {
        super();
        this.faceplate = faceplate;
    }

    _updateSize(faceplateInstance) {
        faceplateInstance.properties.size = rs.mimic.Size.parse(this.faceplate.document.size);
    }

    _createComponents(faceplateInstance) {
        const FactorySet = rs.mimic.FactorySet;
        const MimicHelper = rs.mimic.MimicHelper;

        if (Array.isArray(this.faceplate.components)) {
            for (let sourceComponent of this.faceplate.components) {
                let factory = FactorySet.getComponentFactory(sourceComponent.typeName, this.faceplate.faceplateMap);

                if (factory) {
                    let componentCopy = factory.createComponentFromSource(sourceComponent);
                    faceplateInstance.components.push(componentCopy);

                    if (componentCopy.name) {
                        faceplateInstance.componentByName.set(componentCopy.name, componentCopy);
                    }
                }
            }

            MimicHelper.defineNesting(faceplateInstance, faceplateInstance.components);
        }
    }

    _createCustomProperties(faceplateInstance, sourceProps) {
        const ObjectHelper = rs.mimic.ObjectHelper;
        sourceProps ??= {};

        for (let propertyExport of this.faceplate.propertyExports) {
            let baseValue = faceplateInstance.getTargetPropertyValue(propertyExport);
            let sourceValue = sourceProps[propertyExport.name];

            if (sourceValue === undefined) {
                faceplateInstance.properties[propertyExport.name] = baseValue;
            } else {
                let mergedValue = ObjectHelper.mergeValues(baseValue, sourceValue);
                faceplateInstance.properties[propertyExport.name] = mergedValue;
                faceplateInstance.setTargetPropertyValue(propertyExport, mergedValue);
            }
        }
    }

    _applyModel(faceplateInstance, source) {
        faceplateInstance.typeName = faceplateInstance.properties.typeName = this.faceplate.typeName;
        faceplateInstance.model = this.faceplate;
        this._createComponents(faceplateInstance);
        this._createCustomProperties(faceplateInstance, source?.properties);
    }

    createComponent() {
        let faceplateInstance = new rs.mimic.FaceplateInstance();
        faceplateInstance.properties = this.createProperties();

        if (this.faceplate) {
            this._updateSize(faceplateInstance);
            this._applyModel(faceplateInstance, null);
        }

        return faceplateInstance;
    }

    createComponentFromSource(source) {
        let faceplateInstance = new rs.mimic.FaceplateInstance();
        this._copyProperties(faceplateInstance, source);

        if (this.faceplate) {
            this._applyModel(faceplateInstance, source);
        }

        return faceplateInstance;
    }
};

// Contains factories for mimic components.
rs.mimic.FactorySet = class FactorySet {
    static componentFactories = new Map([
        ["Text", new rs.mimic.TextFactory()],
        ["Picture", new rs.mimic.PictureFactory()],
        ["Panel", new rs.mimic.PanelFactory()]
    ]);
    static getFaceplateFactory(faceplate) {
        return new rs.mimic.FaceplateFactory(faceplate);
    }
    static getComponentFactory(typeName, faceplateMap) {
        if (faceplateMap.has(typeName)) {
            let faceplate = faceplateMap.get(typeName); // can be null
            return FactorySet.getFaceplateFactory(faceplate);
        } else {
            return FactorySet.componentFactories.get(typeName);
        }
    }
};

// Contains classes: MimicHelper, MimicBase, Mimic, Component, Image, FaceplateMeta, Faceplate, FaceplateInstance
// Depends on scada-common.js, mimic-common.js, mimic-model-subtypes.js, mimic-factory.js

// Provides helper methods for mimics and components.
rs.mimic.MimicHelper = class MimicHelper {
    // Indexes child components.
    static _indexComponents(parent, opt_start) {
        for (let index = opt_start ?? 0; index < parent.children.length; index++) {
            let component = parent.children[index];
            component.index = index;
        }
    }

    // Sets the component property according to the current data.
    static _setComponentProperty(component, binding, curData) {
        const DataProvider = rs.mimic.DataProvider;
        const ObjectHelper = rs.mimic.ObjectHelper;
        let fieldValue = DataProvider.getFieldValue(curData, binding.dataMember, binding.cnlProps.unit);

        if (binding.format) {
            fieldValue = binding.format.replace("{0}", String(fieldValue));
        }

        ObjectHelper.setPropertyValue(component.properties, binding.propertyChain, 0, fieldValue);

        if (component.isFaceplate) {
            component.handlePropertyChanged(binding.propertyName);
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

    // Updates the component properties according to the current data. Returns true if any property has changed.
    static updateData(component, dataProvider) {
        // component bindings are 
        // { inCnlNum, outCnlNum, objNum, deviceNum, checkRights, inCnlProps, outCnlProps, propertyBindings }
        // property binding is { propertyName, dataSource, dataMember, format, propertyChain, cnlNum, cnlProps }
        // channel properties are { joinLen, unit }
        const DataProvider = rs.mimic.DataProvider;
        let dataChanged = false;

        if (component.bindings && Array.isArray(component.bindings.propertyBindings) &&
            component.bindings.propertyBindings.length > 0) {
            for (let binding of component.bindings.propertyBindings) {
                if (binding.propertyName && binding.cnlNum > 0 && binding.cnlProps) {
                    let curData = dataProvider.getCurData(binding.cnlNum, binding.cnlProps.joinLen);
                    let prevData = dataProvider.getPrevData(binding.cnlNum, binding.cnlProps.joinLen);

                    if (!DataProvider.dataEqual(curData, prevData) || !dataProvider.prevCnlDataMap) {
                        MimicHelper._setComponentProperty(component, binding, curData);
                        dataChanged = true;
                    }
                }
            }
        }

        return dataChanged;
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
        return this.faceplateMap?.has(typeName);
    }

    // Gets the component factory for the specified type, or null if not found.
    getComponentFactory(typeName) {
        return rs.mimic.FactorySet.getComponentFactory(typeName, this.faceplateMap);
    }

    // Creates a component instance based on the source object. Returns null if the component factory is not found.
    createComponent(source) {
        let factory = this.getComponentFactory(source.typeName);
        return factory ? factory.createComponentFromSource(source) : null;
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

    // Gets the mimic width.
    get width() {
        return this.document ? this.document.size.width : 0;
    }

    // Gets the mimic height.
    get height() {
        return this.document ? this.document.size.height : 0;
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

            case LoadStep.FACEPLATES:
                if (this.dependencies.length > 0) {
                    let faceplateMeta = this.dependencies[loadContext.faceplateIndex];

                    if (faceplateMeta.hasError) {
                        continueLoading = true;
                    } else {
                        dto = await this._loadFaceplate(loadContext, faceplateMeta.typeName);
                    }

                    if (++loadContext.faceplateIndex >= this.dependencies.length) {
                        this._prepareFaceplates();
                        loadContext.step++;
                    }
                } else {
                    loadContext.step++;
                    continueLoading = true;
                }
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
            } else {
                this.faceplateMap.set(typeName, null);
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
                        loadContext.result.warn = true;
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

    // Prepares the faceplates for use.
    _prepareFaceplates() {
        for (let faceplate of this.faceplates) {
            for (let childFaceplateMeta of faceplate.dependencies) {
                let childFaceplate = this.faceplateMap.get(childFaceplateMeta.typeName);

                if (childFaceplate) {
                    faceplate.faceplates.push(childFaceplate);
                    faceplate.faceplateMap.set(childFaceplateMeta.typeName, childFaceplate);
                }
            }
        }
    }

    // Finds a parent and children for each component.
    _defineNesting() {
        rs.mimic.MimicHelper.defineNesting(this, this.components, this.componentMap);
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
    id = 0;             // component ID
    typeName = "";      // component type name
    properties = null;  // factory normalized properties
    bindings = null;    // server side prepared bindings, see ComponentBindings.cs
    parentID = 0;       // parent ID
    index = -1;         // sibling index

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
    propertyExports = [];
    propertyExportMap = new Map();

    constructor(source, typeName) {
        super();
        this.clear();
        this.document = source.document ?? {};
        this.typeName = typeName;
        this._fillDependencies(source);
        this._fillComponents(source);
        this._fillImages(source);
        this._fillPropertyExports();
    }

    _fillDependencies(source) {
        if (Array.isArray(source.dependencies)) {
            for (let sourceDependency of source.dependencies) {
                let faceplateMeta = new rs.mimic.FaceplateMeta(sourceDependency);
                this.dependencies.push(faceplateMeta);
                this.dependencyMap.set(faceplateMeta.typeName, faceplateMeta);
            }
        }
    }

    _fillComponents(source) {
        if (Array.isArray(source.components)) {
            for (let sourceComponent of source.components) {
                this.components.push(sourceComponent);
                this.componentMap.set(sourceComponent.id, sourceComponent);
            }
        }
    }

    _fillImages(source) {
        if (Array.isArray(source.images)) {
            for (let sourceImage of source.images) {
                let image = new rs.mimic.Image(sourceImage);
                this.images.push(image);
                this.imageMap.set(image.name, image);
            }
        }
    }

    _fillPropertyExports() {
        if (Array.isArray(this.document.propertyExports)) {
            for (let sourcePropertyExport of this.document.propertyExports) {
                if (sourcePropertyExport.name) {
                    let propertyExport = new rs.mimic.PropertyExport(sourcePropertyExport);
                    this.propertyExports.push(propertyExport);
                    this.propertyExportMap.set(propertyExport.name, propertyExport);
                }
            }
        }
    }
};

// Represents a faceplate instance.
rs.mimic.FaceplateInstance = class extends rs.mimic.Component {
    model = null;                // model of the Faceplate type
    components = [];             // components created according to the model
    componentByName = new Map(); // components accessible by name

    get isContainer() {
        // child components are essential part of the faceplate, it does not accept additional components
        return false;
    }

    get isFaceplate() {
        return true;
    }

    // Gets the value of the target property specified by the export path.
    getTargetPropertyValue(propertyExport) {
        const ObjectHelper = rs.mimic.ObjectHelper;
        let propertyChain = propertyExport.propertyChain;

        if (propertyChain.length >= 2) {
            let componentName = propertyChain[0];
            let component = this.componentByName.get(componentName);

            if (component) {
                if (component.isFaceplate) {
                    let topPropertyName = propertyChain[1];
                    let childPropertyExport = component.model?.propertyExportMap.get(topPropertyName);
                    return childPropertyExport
                        ? component.getTargetPropertyValue(childPropertyExport)
                        : ObjectHelper.getPropertyValue(component.properties, propertyChain, 1);
                } else {
                    return ObjectHelper.getPropertyValue(component.properties, propertyChain, 1);
                }
            }
        }

        return undefined;
    }

    // Sets the value of the target property specified by the export path.
    setTargetPropertyValue(propertyExport, value) {
        const ObjectHelper = rs.mimic.ObjectHelper;
        let propertyChain = propertyExport.propertyChain;

        if (propertyChain.length >= 2) {
            let componentName = propertyChain[0];
            let component = this.componentByName.get(componentName);

            if (component) {
                if (component.isFaceplate) {
                    let topPropertyName = propertyChain[1];
                    let childPropertyExport = component.model?.propertyExportMap.get(topPropertyName);

                    if (childPropertyExport) {
                        component.setTargetPropertyValue(childPropertyExport, value);
                    } else {
                        ObjectHelper.setPropertyValue(component.properties, propertyChain, 1, value);
                    }
                } else {
                    ObjectHelper.setPropertyValue(component.properties, propertyChain, 1, value);
                }
            }
        }
    }

    // Updates the target property corresponding to the changed property.
    handlePropertyChanged(propertyName) {
        let propertyExport = this.model?.propertyExportMap.get(propertyName);

        if (propertyExport) {
            let value = this.properties[propertyName];
            this.setTargetPropertyValue(propertyExport, value);
        }
    }
};

// Enumerations: ActionType, ComparisonOperator, DataMember, ImageSizeMode, LogicalOperator, LinkTarget, ModalWidth,
//     ContentAlignment, TextDirection
// Structures: Action, Border, CommandArgs, Condition, CornerRadius, Font, ImageCondition, LinkArgs, Padding, Point,
//     PropertyBinding, PropertyExport, Size, VisualState
// Misc: List, ImageConditionList, PropertyBindingList, PropertyExportList, PropertyParser, DataProvider
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

// Specifies the data members of a property binding.
rs.mimic.DataMember = class {
    static VALUE = "Value";
    static STATUS = "Status";
    static DISPLAY_VALUE = "DisplayValue";
    static DISPLAY_VALUE_WITH_UNIT = "DisplayValueWithUnit";
    static COLOR0 = "Color0";
    static COLOR1 = "Color1";
    static COLOR2 = "Color2";
}

// Specifies how an image is positioned within a component.
rs.mimic.ImageSizeMode = class {
    static NORMAL = "Normal";
    static CENTER = "Center";
    static STRETCH = "Stretch";
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
    inherit = false;
    name = "";
    size = 16;
    bold = false;
    italic = false;
    underline = false;

    constructor(source) {
        Object.assign(this, source);
    }

    get typeName() {
        return "Font";
    }

    static parse(source) {
        const PropertyParser = rs.mimic.PropertyParser;
        let font = new Font();

        if (source) {
            font.inherit = PropertyParser.parseBool(source.inherit);
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
            propertyBinding.dataMember = PropertyParser.parseString(source.dataMember, rs.mimic.DataMember.VALUE);
            propertyBinding.format = PropertyParser.parseString(source.format);
        }

        return propertyBinding;
    }
};

// Represents an exported property.
rs.mimic.PropertyExport = class PropertyExport {
    name = "";
    path = "";

    constructor(source) {
        Object.assign(this, source);
    }

    get typeName() {
        return "PropertyExport";
    }

    get displayName() {
        return this.name;
    }

    get propertyChain() {
        if (this.propertyChainCache !== undefined) {
            return this.propertyChainCache;
        }

        this.propertyChainCache = this.path ? this.path.split('.') : [];
        return this.propertyChainCache;
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
    underline = false;

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
            visualState.underline = PropertyParser.parseBool(source.underline);
        }

        return visualState;
    }
};

// --- Misc ---

// Represents a list that can create new items.
rs.mimic.List = class extends Array {
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
rs.mimic.ImageConditionList = class ImageConditionList extends rs.mimic.List {
    constructor() {
        super(() => {
            return new rs.mimic.ImageCondition();
        });
    }

    static parse(source) {
        const ImageCondition = rs.mimic.ImageCondition;
        let imageConditions = new ImageConditionList();

        if (Array.isArray(source)) {
            for (let sourceItem of source) {
                imageConditions.push(ImageCondition.parse(sourceItem));
            }
        }

        return imageConditions;
    }
}

// Represents a list of PropertyBinding items.
rs.mimic.PropertyBindingList = class PropertyBindingList extends rs.mimic.List {
    constructor() {
        super(() => {
            return new rs.mimic.PropertyBinding();
        });
    }

    static parse(source) {
        const PropertyBinding = rs.mimic.PropertyBinding;
        let propertyBindings = new PropertyBindingList();

        if (Array.isArray(source)) {
            for (let sourceItem of source) {
                propertyBindings.push(PropertyBinding.parse(sourceItem));
            }
        }

        return propertyBindings;
    }
}

// Represents a list of PropertyExport items.
rs.mimic.PropertyExportList = class PropertyExportList extends rs.mimic.List {
    constructor() {
        super(() => {
            return new rs.mimic.PropertyExport();
        });
    }

    static parse(source) {
        const PropertyExport = rs.mimic.PropertyExport;
        let propertyExports = new PropertyExportList();

        if (Array.isArray(source)) {
            for (let sourceItem of source) {
                propertyExports.push(PropertyExport.parse(sourceItem));
            }
        }

        return propertyExports;
    }
}

// Parses property values ​​from strings.
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
}

// Represents an abstract provider of channel data and channel properties.
rs.mimic.DataProvider = class DataProvider {
    static EMPTY_DATA = {
        d: { cnlNum: 0, val: 0.0, stat: 0 },
        df: { dispVal: "", colors: [] }
    };

    curDataMap = null;
    prevDataMap = null;

    getCurData(cnlNum, opt_joinLen) {
        return DataProvider.EMPTY_DATA;
    }

    getPrevData(cnlNum, opt_joinLen) {
        return DataProvider.EMPTY_DATA;
    }

    static dataEqual(data1, data2) {
        return data1.d.val === data2.d.val && data1.d.stat === data2.d.stat;
    }

    static getFieldValue(data, dataMember, opt_unit) {
        const DataMember = rs.mimic.DataMember;

        switch (dataMember) {
            case DataMember.VALUE:
                return data.d.val;

            case DataMember.STATUS:
                return data.d.stat;

            case DataMember.DISPLAY_VALUE:
                return data.df.dispVal;

            case DataMember.DISPLAY_VALUE_WITH_UNIT:
                return opt_unit && data.d.stat > 0
                    ? data.df.dispVal + " " + opt_unit
                    : data.df.dispVal;

            case DataMember.COLOR0:
                return data.df.colors.length > 0 ? data.df.colors[0] : "";

            case DataMember.COLOR1:
                return data.df.colors.length > 1 ? data.df.colors[1] : "";

            case DataMember.COLOR2:
                return data.df.colors.length > 2 ? data.df.colors[2] : "";

            default:
                return null;
        }
    }
};

// Contains classes: Renderer, MimicRenderer, ComponentRenderer, RegularComponentRenderer,
//     TextRenderer, PictureRenderer, PanelRenderer, RenderContext, RendererSet, UnitedRenderer
// Depends on jquery, scada-common.js, mimic-common.js

// Represents a renderer of a mimic or component.
rs.mimic.Renderer = class {
    // Sets the background image of the specified jQuery object.
    _setBackgroundImage(jqObj, image) {
        jqObj.css("background-image", this._imageToDataUrlCss(image));
    }

    // Sets the border of the specified jQuery object.
    _setBorder(jqObj, border) {
        if (border.width > 0) {
            jqObj.css({
                "border-width": border.width,
                "border-style": "solid",
                "border-color": border.color
            });
        } else {
            jqObj.css("border", "none");
        }
    }

    // Sets the corner radius of the specified jQuery object.
    _setCornerRadius(jqObj, cornerRadius) {
        jqObj.css({
            "border-top-left-radius": cornerRadius.topLeft,
            "border-top-right-radius": cornerRadius.topRight,
            "border-bottom-right-radius": cornerRadius.bottomRight,
            "border-bottom-left-radius": cornerRadius.bottomLeft
        });
    }

    // Sets the font of the specified jQuery object.
    _setFont(jqObj, font, fontMap) {
        if (font.inherit) {
            jqObj.css({
                "font-family": "",
                "font-size": "",
                "font-weight": "",
                "font-style": "",
                "text-decoration": "" // not inherited
            });
        } else {
            jqObj.css({
                "font-family": fontMap?.get(font.name)?.family,
                "font-size": font.size,
                "font-weight": font.bold ? "bold" : "normal",
                "font-style": font.italic ? "italic" : "normal",
                "text-decoration": font.underline ? "underline" : "none"
            });
        }
    }

    // Sets the left and top of the specified jQuery object.
    _setLocation(jqObj, location) {
        jqObj.css({
            "left": location.x + "px",
            "top": location.y + "px"
        });
    }

    // Sets the padding of the specified jQuery object.
    _setPadding(jqObj, padding) {
        jqObj.css({
            "padding-top": padding.top,
            "padding-right": padding.right,
            "padding-bottom": padding.bottom,
            "padding-left": padding.left
        });
    }

    // Sets the width and height of the specified jQuery object.
    _setSize(jqObj, size) {
        jqObj.css({
            "width": size.width + "px",
            "height": size.height + "px"
        });
    }

    // Returns a css property value for the image data URI.
    _imageToDataUrlCss(image) {
        return image ? "url('" + image.dataUrl + "')" : "";
    }

    // Creates a component DOM according to the component model. Returns a jQuery object or null.
    createDom(component, renderContext) {
        return null;
    }

    // Updates the existing component DOM according to the component model. Returns a jQuery object or null.
    updateDom(component, renderContext) {
        return null;
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
    appendChild(parent, child) {
        if (parent.dom && child.dom) {
            parent.dom.append(child.dom);
        }
    }

    // Removes the component DOM from the mimic keeping data associated with the removed elements.
    detach(component) {
        component.dom?.detach();
    }

    // Removes the component DOM from the mimic.
    remove(component) {
        component.dom?.remove();
    }

    // Arranges the child component DOMs according to their order.
    static arrangeChildren(parent) {
        if (parent && parent.children) {
            for (let component of parent.children) {
                component.renderer?.detach(component);
            }

            for (let component of parent.children) {
                parent.renderer?.appendChild(parent, component);
            }
        }
    }
};

// Represents a mimic renderer.
rs.mimic.MimicRenderer = class MimicRenderer extends rs.mimic.Renderer {
    static _GRID_COLOR = "#dee2e6"; // gray-300

    // Creates a grid canvas and draws grid cells.
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

    // Draws grid cells.
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

    // Calculates the scale value depending on the scale type and mimic size.
    static _calcScaleValue(mimic, scale) {
        const ScaleType = rs.mimic.ScaleType;

        if (scale.type === ScaleType.NUMERIC) {
            if (scale.value >= 0) {
                return scale.value;
            }
        } else {
            let parentDom = mimic.dom?.parent();

            if (parentDom && mimic.width > 0 && mimic.height > 0) {
                let areaWidth = parentDom.innerWidth();
                let horScale = areaWidth / mimic.width;

                if (scale.type === ScaleType.FIT_WIDTH) {
                    return horScale;
                } else if (scale.type === ScaleType.FIT_SCREEN) {
                    let areaHeight = parentDom.innerHeight();
                    let vertScale = areaHeight / mimic.height;
                    return Math.min(horScale, vertScale);
                }
            }
        }

        return 1.0;
    }

    // Sets the CSS properties of the mimic element.
    _setProps(mimicElem, mimic, renderContext) {
        let props = mimic.document;
        this._setBackgroundImage(mimicElem, renderContext.getImage(props.backgroundImage));
        this._setFont(mimicElem, props.font, renderContext.fontMap);
        this._setSize(mimicElem, props.size);
        this._setStyle(props.stylesheet);

        if (!renderContext.editMode) {
            $("body").css("background-color", props.backColor);
        }

        mimicElem
            .attr("title", props.tooltip)
            .css({
                "background-color": props.backColor,
                "background-repeat": "no-repeat",
                "background-size": props.size.width + "px " + props.size.height + "px",
                "color": props.foreColor
            });
    }

    // Adds a style element to the page head or replaces the existing style element.
    _setStyle(stylesheet) {
        if (stylesheet) {
            let newStyleElem = $("<style id='mimic-style'></style>").html(stylesheet);
            let oldStyleElem = $("head").find("#mimic-style");

            if (oldStyleElem.length > 0) {
                oldStyleElem.replaceWith(newStyleElem);
            } else {
                $("head").append(newStyleElem);
            }
        } else {
            $("head").find("#mimic-style").remove();
        }
    }

    // Creates a mimic DOM according to the mimic model.
    createDom(mimic, renderContext) {
        let mimicElem = $("<div class='mimic'></div>");

        if (renderContext.editMode && renderContext.editorOptions &&
            renderContext.editorOptions.showGrid && renderContext.editorOptions.gridStep > 1) {
            mimicElem.append(MimicRenderer._createGrid(renderContext.editorOptions.gridStep, mimic.document.size));
        }

        this._setProps(mimicElem, mimic, renderContext);
        mimic.dom = mimicElem;
        return mimicElem;
    }

    // Updates the existing mimic DOM according to the mimic model.
    updateDom(mimic, renderContext) {
        let mimicElem = mimic.dom;

        if (mimicElem) {
            this._setProps(mimicElem, mimic, renderContext);
        }

        return mimicElem;
    }

    // Sets the scale of the mimic DOM.
    setScale(mimic, scale) {
        if (mimic.dom) {
            let scaleValue = MimicRenderer._calcScaleValue(mimic, scale);

            if (scale.type !== rs.mimic.ScaleType.NUMERIC) {
                scale.setValue(scaleValue);
            }

            mimic.dom.css({
                "transform": `scale(${scale.value})`
            });
        }
    }
};

// Represents a component renderer.
rs.mimic.ComponentRenderer = class extends rs.mimic.Renderer {
    // Gets a value indicating whether the renderer can update the existing component DOM without recreating it.
    get canUpdateDom() {
        return true;
    }

    // Completes the creation of the component DOM.
    _completeDom(componentElem, component, renderContext) {
    }

    // Sets the CSS classes of the component element.
    _setClasses(componentElem, component, renderContext) {
        componentElem.removeClass(); // clear classes
        componentElem.addClass("comp");
        let props = component.properties;

        if (renderContext.editMode) {
            if (!renderContext.faceplateMode && component.isContainer) {
                componentElem.addClass("container")
            }

            if (component.isSelected) {
                componentElem.addClass("selected")
            }
        } else {
            if (!props.enabled) {
                componentElem.addClass("disabled")
            }

            if (!props.visible) {
                componentElem.addClass("hidden")
            }
        }
    }

    // Sets the CSS properties of the component element.
    _setProps(componentElem, component, renderContext) {
        let props = component.properties;
        this._setLocation(componentElem, props.location);
        this._setSize(componentElem, props.size);
    }

    // Creates a component DOM according to the component model.
    createDom(component, renderContext) {
        let componentElem = $("<div></div>")
            .attr("id", "comp" + renderContext.idPrefix + component.id)
            .attr("data-id", component.id);
        this._completeDom(componentElem, component, renderContext);
        this._setClasses(componentElem, component, renderContext);
        this._setProps(componentElem, component, renderContext);
        component.dom = componentElem;
        return componentElem;
    }

    // Updates the existing component DOM according to the component model.
    updateDom(component, renderContext) {
        let componentElem = component.dom;

        if (componentElem) {
            this._setClasses(componentElem, component, renderContext);
            this._setProps(componentElem, component, renderContext);
        }

        return componentElem;
    }

    // Sets the location of the component DOM without changing the component model.
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

    // Updates the location of the component DOM according to the component model.
    updateLocation(component) {
        if (component.dom) {
            this._setLocation(component.dom, component.properties.location);
        }
    }

    // Visually selects or deselects the component.
    updateSelected(component) {
        if (component.dom) {
            component.dom.toggleClass("selected", component.isSelected);
        }
    }

    // Gets a value indicating whether the component can be resized by a user.
    allowResizing(component) {
        return true;
    }
};

// Represents a renderer for regular non-faceplate components.
rs.mimic.RegularComponentRenderer = class extends rs.mimic.ComponentRenderer {
    _setClasses(componentElem, component, renderContext) {
        super._setClasses(componentElem, component, renderContext);
        let props = component.properties;

        if (props.cssClass) {
            componentElem.addClass(props.cssClass);
        }
    }

    _setProps(componentElem, component, renderContext) {
        super._setProps(componentElem, component, renderContext);
        let props = component.properties;
        this._setBorder(componentElem, props.border);
        this._setCornerRadius(componentElem, props.cornerRadius);

        componentElem
            .attr("title", props.tooltip)
            .css({
                "background-color": props.backColor,
                "color": props.foreColor
            });
    }
};

// Represents a text component renderer.
rs.mimic.TextRenderer = class extends rs.mimic.RegularComponentRenderer {
    _setClasses(componentElem, component, renderContext) {
        super._setClasses(componentElem, component, renderContext);
        componentElem.addClass("text");
    }

    _setProps(componentElem, component, renderContext) {
        super._setProps(componentElem, component, renderContext);
        let props = component.properties;
        this._setFont(componentElem, props.font, renderContext.fontMap);
        this._setPadding(componentElem, props.padding);
        this._setTextDirection(componentElem, props.textDirection);
        componentElem.text(props.text);

        if (props.autoSize) {
            componentElem.css({
                "display": "inline",
                "overflow": "visible",
                "white-space": "nowrap",
                "width": "",
                "height": ""
            });
        } else {
            componentElem
                .css({
                    "display": "flex",
                    "overflow": "hidden",
                    "white-space": props.wordWrap ? "normal" : "nowrap"
                });
            this._setTextAlign(componentElem, props.textAlign);
        }
    }

    _setTextAlign(jqObj, contentAlignment) {
        const ContentAlignment = rs.mimic.ContentAlignment;

        switch (contentAlignment) {
            case ContentAlignment.TOP_LEFT:
                jqObj.css({
                    "align-items": "flex-start",
                    "justify-content": "flex-start",
                    "text-align": "left"
                });
                break;

            case ContentAlignment.TOP_CENTER:
                jqObj.css({
                    "align-items": "flex-start",
                    "justify-content": "center",
                    "text-align": "center"
                });
                break;

            case ContentAlignment.TOP_RIGHT:
                jqObj.css({
                    "align-items": "flex-start",
                    "justify-content": "flex-end",
                    "text-align": "right"
                });
                break;

            case ContentAlignment.MIDDLE_LEFT:
                jqObj.css({
                    "align-items": "center",
                    "justify-content": "flex-start",
                    "text-align": "left"
                });
                break;

            case ContentAlignment.MIDDLE_CENTER:
                jqObj.css({
                    "align-items": "center",
                    "justify-content": "center",
                    "text-align": "center"
                });
                break;

            case ContentAlignment.MIDDLE_RIGHT:
                jqObj.css({
                    "align-items": "center",
                    "justify-content": "flex-end",
                    "text-align": "right"
                });
                break;

            case ContentAlignment.BOTTOM_LEFT:
                jqObj.css({
                    "align-items": "flex-end",
                    "justify-content": "flex-start",
                    "text-align": "left"
                });
                break;

            case ContentAlignment.BOTTOM_CENTER:
                jqObj.css({
                    "align-items": "flex-end",
                    "justify-content": "center",
                    "text-align": "center"
                });
                break;

            case ContentAlignment.BOTTOM_RIGHT:
                jqObj.css({
                    "align-items": "flex-end",
                    "justify-content": "flex-end",
                    "text-align": "right"
                });
                break;

            default:
                jqObj.css({
                    "align-items": "",
                    "justify-content": "",
                    "text-align": ""
                });
                break;
        }
    }

    _setTextDirection(jqObj, textDirection) {
        const TextDirection = rs.mimic.TextDirection;

        switch (textDirection) {
            case TextDirection.VERTICAL90:
                jqObj.css("writing-mode", "vertical-rl");
                break;

            case TextDirection.VERTICAL270:
                jqObj.css("writing-mode", "sideways-lr");
                break;

            default:
                jqObj.css("writing-mode", "");
                break;
        }
    }

    allowResizing(component) {
        return !component.properties.autoSize;
    }
};

// Represents a picture component renderer.
rs.mimic.PictureRenderer = class extends rs.mimic.RegularComponentRenderer {
    _completeDom(componentElem, component, renderContext) {
        componentElem.append("<div class='picture-content'></div>");
    }

    _setClasses(componentElem, component, renderContext) {
        super._setClasses(componentElem, component, renderContext);
        componentElem.addClass("picture");
    }

    _setProps(componentElem, component, renderContext) {
        super._setProps(componentElem, component, renderContext);
        const ImageSizeMode = rs.mimic.ImageSizeMode;
        let contentElem = componentElem.find(".picture-content:first");
        let props = component.properties;
        this._setPadding(componentElem, props.padding);
        this._setBackgroundImage(contentElem, renderContext.getImage(props.imageName));

        switch (props.sizeMode) {
            case ImageSizeMode.NORMAL:
                contentElem.css({
                    "background-position": "top left",
                    "background-size": ""
                });
                break;

            case ImageSizeMode.CENTER:
                contentElem.css({
                    "background-position": "center center",
                    "background-size": ""
                });
                break;

            case ImageSizeMode.STRETCH:
                let w = props.size.width - props.border.width * 2 - props.padding.left - props.padding.right;
                let h = props.size.height - props.border.width * 2 - props.padding.top - props.padding.bottom;
                contentElem.css({
                    "background-position": "center center",
                    "background-size": `${w}px ${h}px`
                });
                break;

            case ImageSizeMode.ZOOM:
                contentElem.css({
                    "background-position": "center center",
                    "background-size": "contain"
                });
                break;
        }
    }
};

// Represents a panel component renderer.
rs.mimic.PanelRenderer = class extends rs.mimic.RegularComponentRenderer {
    _setClasses(componentElem, component, renderContext) {
        super._setClasses(componentElem, component, renderContext);
        componentElem.addClass("panel");
    }
};

// Represents a faceplate renderer.
rs.mimic.FaceplateRenderer = class extends rs.mimic.ComponentRenderer {
    _setClasses(componentElem, component, renderContext) {
        super._setClasses(componentElem, component, renderContext);
        componentElem.addClass("faceplate");
    }
};

// Encapsulates information about a rendering operation.
rs.mimic.RenderContext = class {
    editMode = false;
    fontMap = null;
    editorOptions = null;
    controlRight = false;
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
    fontMap = null;
    editorOptions = null;
    controlRight = false;

    constructor(mimic, editMode) {
        this.mimic = mimic;
        this.editMode = editMode;
    }

    // Creates a render context for regular components.
    _createRenderContext(opt_withUnknownTypes) {
        return new rs.mimic.RenderContext({
            editMode: this.editMode,
            fontMap: this.fontMap,
            editorOptions: this.editorOptions,
            controlRight: this.controlRight,
            imageMap: this.mimic.imageMap,
            unknownTypes: opt_withUnknownTypes ? new Set() : null
        });
    }

    // Creates a render context for the faceplate.
    _createFaceplateContext(faceplateInstance, renderContext) {
        return new rs.mimic.RenderContext({
            editMode: this.editMode,
            fontMap: this.fontMap,
            editorOptions: this.editorOptions,
            controlRight: this.controlRight,
            faceplateMode: true,
            imageMap: faceplateInstance.model?.imageMap,
            idPrefix: renderContext.idPrefix,
            unknownTypes: renderContext.unknownTypes
        });
    }

    // Appends the component DOM to its parent.
    _appendToParent(component) {
        if (component.parent?.renderer) {
            component.parent.renderer.appendChild(component.parent, component);
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

        let faceplateContext = this._createFaceplateContext(faceplateInstance, renderContext);
        let renderer = rs.mimic.RendererSet.faceplateRenderer;
        faceplateInstance.renderer = renderer;
        renderer.createDom(faceplateInstance, faceplateContext);
        this._appendToParent(faceplateInstance);
        faceplateContext.idPrefix += faceplateInstance.id + "-";

        for (let component of faceplateInstance.components) {
            this._createComponentDom(component, faceplateContext);
        }
    }

    // Updates the component DOM.
    _updateComponentDom(component, renderContext) {
        if (component.dom && component.renderer) {
            if (component.isFaceplate) {
                this._updateFaceplateDom(component, renderContext);
            } else {
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

    // Updates the faceplate DOM.
    _updateFaceplateDom(faceplateInstance, renderContext) {
        if (faceplateInstance.model && faceplateInstance.dom && faceplateInstance.renderer) {
            let faceplateContext = this._createFaceplateContext(faceplateInstance, renderContext);
            faceplateInstance.renderer.updateDom(faceplateInstance, faceplateContext);
            faceplateContext.idPrefix += faceplateInstance.id + "-";

            for (let component of faceplateInstance.components) {
                this._updateComponentDom(component, faceplateContext);
            }
        }
    }

    // Configures the renderer.
    configure({
        fonts = null,
        editorOptions = null,
        controlRight = false
    }) {
        this.fontMap = Array.isArray(fonts) ? new Map(fonts.map(font => [font.name, font])) : null;
        this.editorOptions = editorOptions;
        this.controlRight = controlRight;
    }

    // Creates a mimic DOM according to the mimic model. Returns a jQuery object.
    createMimicDom() {
        let startTime = Date.now();
        let renderContext = this._createRenderContext(true);
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
            console.warn("Unable to create mimic DOM");
            return $();
        }
    }

    // Updates a mimic DOM according to the mimic model.
    updateMimicDom() {
        rs.mimic.RendererSet.mimicRenderer.updateDom(this.mimic, this._createRenderContext());
    }

    // Creates a component DOM according to the component model. Returns a jQuery object.
    createComponentDom(component) {
        this._createComponentDom(component, this._createRenderContext());
        return component.dom ?? $();
    }

    // Updates the component DOM according to the component model.
    updateComponentDom(component) {
        this._updateComponentDom(component, this._createRenderContext());
    }

    // Updates the components according to the current data.
    updateData(dataProvider) {
        let renderContext = this._createRenderContext();

        for (let component of this.mimic.components) {
            try {
                if (rs.mimic.MimicHelper.updateData(component, dataProvider)) {
                    this._updateComponentDom(component, renderContext);
                }
            } catch (ex) {
                console.error("Error updating data of the component with ID " + component.id +
                    " of type " + component.typeName);
            }
        }
    }
};
