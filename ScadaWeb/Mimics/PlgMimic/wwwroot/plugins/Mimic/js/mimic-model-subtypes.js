// Enumerations: ActionType, ComparisonOperator, DataMember, ImageSizeMode, LogicalOperator, LinkTarget, ModalWidth,
//     ContentAlignment, TextDirection
// Structures: Action, Border, CommandArgs, Condition, CornerRadius, Font, ImageCondition, LinkArgs, Padding, Point,
//     PropertyBinding, PropertyExport, Size, UrlParams, VisualState
// Lists: List, ImageConditionList, PropertyBindingList, PropertyExportList
// Scripts: ComponentScript, DomUpdateArgs, DataUpdateArgs, CommandSendArgs, ActionScriptArgs
// Misc: PropertyParser, DataProvider
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

    static getDisplayName(oper) {
        switch (oper) {
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

    static compare(oper, val1, val2) {
        switch (oper) {
            case ComparisonOperator.EQUAL:
                return val1 === val2;
            case ComparisonOperator.NOT_EQUAL:
                return val1 !== val2;
            case ComparisonOperator.LESS_THAN:
                return val1 < val2;
            case ComparisonOperator.LESS_THAN_EQUAL:
                return val1 <= val2;
            case ComparisonOperator.GREATER_THAN:
                return val1 > val2;
            case ComparisonOperator.GREATER_THAN_EQUAL:
                return val1 >= val2;
            default:
                return false;
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

    static getDisplayName(oper) {
        switch (oper) {
            case LogicalOperator.AND:
                return "&&";
            case LogicalOperator.OR:
                return "||";
            default:
                return "";
        }
    }

    isTrue(oper, val1, val2) {
        switch (oper) {
            case LogicalOperator.AND:
                return val1 && val2;
            case LogicalOperator.OR:
                return val1 || val2;
            default:
                return false;
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
rs.mimic.ModalWidth = class ModalWidth {
    static NORMAL = "Normal";
    static SMALL = "Small";
    static LARGE = "Large";
    static EXTRA_LARGE = "ExtraLarge";

    static toModalSize(modalWidth) {
        switch (modalWidth) {
            case ModalWidth.NORMAL:
                return 0;
            case ModalWidth.SMALL:
                return 1;
            case ModalWidth.LARGE:
                return 2;
            case ModalWidth.EXTRA_LARGE:
                return 3;
            default:
                return 0;
        };
    }
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

    satisfied(value) {
        const ComparisonOperator = rs.mimic.ComparisonOperator;
        const LogicalOperator = rs.mimic.LogicalOperator;
        let comp1 = ComparisonOperator.compare(this.comparisonOper1, value, this.comparisonArg1);

        if (this.logicalOper === LogicalOperator.NONE) {
            return comp1;
        } else {
            let comp2 = ComparisonOperator.compare(this.comparisonOper2, value, this.comparisonArg2);
            return LogicalOperator.isTrue(this.logicalOper, comp1, comp2);
        }
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

    get isSet() {
        return this.topLeft > 0 || this.topRight > 0 || this.bottomRight > 0 || this.bottomLeft > 0;
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
    urlParams = new rs.mimic.UrlParams();
    viewID = 0;
    target = rs.mimic.LinkTarget.SELF;
    modalWidth = rs.mimic.ModalWidth.NORMAL;
    modalHeight = 0;

    get typeName() {
        return "LinkArgs";
    }

    getModalSize() {
        return rs.mimic.ModalWidth.toModalSize(this.modalWidth);
    }

    static parse(source) {
        const PropertyParser = rs.mimic.PropertyParser;
        let linkArgs = new LinkArgs();

        if (source) {
            linkArgs.url = PropertyParser.parseString(source.url);
            linkArgs.urlParams = rs.mimic.UrlParams.parse(source.urlParams);
            linkArgs.viewID = PropertyParser.parseInt(source.viewID);
            linkArgs.target = PropertyParser.parseString(source.target, linkArgs.target);
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

    constructor(source) {
        Object.assign(this, source);
    }

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

// Represents URL parameters.
rs.mimic.UrlParams = class UrlParams {
    enabled = false;
    param0 = "";
    param1 = "";
    param2 = "";
    param3 = "";
    param4 = "";
    param5 = "";
    param6 = "";
    param7 = "";
    param8 = "";
    param9 = "";

    get typeName() {
        return "UrlParams";
    }

    toArray() {
        return [
            this.param0,
            this.param1,
            this.param2,
            this.param3,
            this.param4,
            this.param5,
            this.param6,
            this.param7,
            this.param8,
            this.param9
        ];
    }

    static parse(source) {
        const PropertyParser = rs.mimic.PropertyParser;
        let urlParams = new UrlParams();

        if (source) {
            urlParams.enabled = PropertyParser.parseBool(source.enabled);
            urlParams.param0 = PropertyParser.parseString(source.param0);
            urlParams.param1 = PropertyParser.parseString(source.param1);
            urlParams.param2 = PropertyParser.parseString(source.param2);
            urlParams.param3 = PropertyParser.parseString(source.param3);
            urlParams.param4 = PropertyParser.parseString(source.param4);
            urlParams.param5 = PropertyParser.parseString(source.param5);
            urlParams.param6 = PropertyParser.parseString(source.param6);
            urlParams.param7 = PropertyParser.parseString(source.param7);
            urlParams.param8 = PropertyParser.parseString(source.param8);
            urlParams.param9 = PropertyParser.parseString(source.param9);
        }

        return urlParams;
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

    get isSet() {
        return this.backColor || this.foreColor || this.borderColor || this.underline;
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

// --- Lists ---

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

// --- Scripts ---

// A base class for mimic or component logic.
rs.mimic.ComponentScript = class ComponentScript {
    domCreated(args) {
    }

    domUpdated(args) {
    }

    dataUpdated(args) {
    }

    getCommandValue(args) {
        return Number.NaN;
    }

    static createFromSource(sourceCode) {
        const CustomClass = new Function("ComponentScript", "return " + sourceCode)(ComponentScript);
        return new CustomClass();
    }
};

// Provides arguments when creating and updating a mimic or component DOM.
rs.mimic.DomUpdateArgs = class {
    constructor({ mimic, component, renderContext }) {
        this.mimic = mimic;
        this.component = component;
        this.renderContext = renderContext;
    }
};

// Provides arguments when updating mimic or component data.
rs.mimic.DataUpdateArgs = class {
    constructor({ mimic, component, dataProvider }) {
        this.mimic = mimic;
        this.component = component;
        this.dataProvider = dataProvider;
        this.propertyChanged = false;
    }
};

// Provides arguments when sending a command.
rs.mimic.CommandSendArgs = class {
    constructor(component) {
        this.component = component;
    }
}

// Provides arguments when an action script is executed.
rs.mimic.ActionScriptArgs = class {
    constructor({ component, renderContext }) {
        this.component = component;
        this.renderContext = renderContext;
    }
};

// --- Misc ---

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

    dataChanged(curData, prevData) {
        return !DataProvider.dataEqual(curData, prevData) || !this.prevDataMap;
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
