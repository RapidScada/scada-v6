// Contains classes:
//     ActionType, CompareOperator, ImageSizeMode, LogicalOperator, LinkTarget, ModalWidth, ContentAlignment,
//     Action, Border, CommandArgs, Condition, CornerRadius, Font, ImageCondition, LinkArgs, Padding, Point,
//     PropertyBinding, PropertyExport, Size, VisualState,
//     PropertyParser
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
rs.mimic.ComparisonOperator = class {
    static NONE = "None";
    static EQUAL = "Equal";
    static NOT_EQUAL = "NotEqual";
    static LESS_THAN = "LessThan";
    static LESS_THAN_EQUAL = "LessThanEqual";
    static GREATER_THAN = "GreaterThan";
    static GREATER_THAN_EQUAL = "GreaterThanEqual";
};

// Specifies how an image is positioned within a component.
rs.mimic.ImageSizeMode = class {
    static NORMAL = "Normal";
    static STRETCH = "Stretch";
    static CENTER = "Center";
    static ZOOM = "Zoom";
}

// Specifies the logical operators.
rs.mimic.LogicalOperator = class {
    static NONE = "None";
    static AND = "And";
    static OR = "Or";
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

// --- Structures ---

// Represents an action.
rs.mimic.Action = class Action {
    typeName = "Action";
    actionType = rs.mimic.ActionType.NONE;
    chartArgs = "";
    commandArgs = new rs.mimic.CommandArgs();
    linkArgs = new rs.mimic.LinkArgs();
    script = "";

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

// Represents a condition.
rs.mimic.Condition = class Condition {
    comparisonOper1 = rs.mimic.ComparisonOperator.NONE;
    comparisonArg1 = 0.0;
    logicalOper = rs.mimic.LogicalOperator.NONE;
    comparisonOper2 = rs.mimic.ComparisonOperator.NONE;
    comparisonArg2 = 0.0;

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
            List.prototype.createItem = function () {
                return createItemFn.call(this);
            };
        }
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
        let imageConditions = [];

        if (Array.isArray(source)) {
            for (let sourceItem of source) {
                imageConditions.push(ImageCondition.parse(sourceItem));
            }
        }

        return imageConditions;
    }

    static parsePropertyBindings(source) {
        const PropertyBinding = rs.mimic.PropertyBinding;
        let propertyBindings = new rs.mimic.List(() => {
            return new PropertyBinding();
        });

        if (Array.isArray(source)) {
            for (let sourceItem of source) {
                propertyBindings.push(PropertyBinding.parse(sourceItem));
            }
        }

        return propertyBindings;
    }

    static parsePropertyExports(source) {
        const PropertyExport = rs.mimic.PropertyExport;
        let propertyExports = [];

        if (Array.isArray(source)) {
            for (let sourceItem of source) {
                propertyExports.push(PropertyExport.parse(sourceItem));
            }
        }

        return propertyExports;
    }
}
