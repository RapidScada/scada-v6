// Contains classes:
//    ActionType, CompareOperator, LogicalOperator, LinkTarget, ModalWidth, ContentAlignment,
//    Action, Border, CommandArgs, Condition, CornerRadius, Font, ImageCondition, LinkArgs,
//    Location, Padding, PropertyAlias, PropertyBinding, Size, VisualState
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
rs.mimic.CompareOperator = class {
    static NONE = "None";
    static EQUAL = "Equal";
    static NOT_EQUAL = "NotEqual";
    static LESS_THAN = "LessThan";
    static LESS_THAN_EQUAL = "LessThanEqual";
    static GREATER_THAN = "GreaterThan";
    static GREATER_THAN_EQUAL = "GreaterThanEqual";
};

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
rs.mimic.Action = class {
    actionType = rs.mimic.ActionType.NONE;
    chartArgs = "";
    commandArgs = new rs.mimic.CommandArgs();
    linkArgs = new rs.mimic.LinkArgs();
    script = "";
};

// Represents a border.
rs.mimic.Border = class {
    width = 0;
    color = ""
};

// Represents arguments of the SEND_COMMAND action.
rs.mimic.CommandArgs = class {
    showDialog = true;
    cmdVal = 0.0;
    cmdDataHex = "";
    cmdDataStr = "";
};

// Represents a condition.
rs.mimic.Condition = class {
    compareOperator1 = rs.mimic.CompareOperator.NONE;
    compareArgument1 = 0.0;
    logicalOperator = rs.mimic.LogicalOperator.NONE;
    compareOperator2 = rs.mimic.CompareOperator.NONE;
    compareArgument2 = 0.0;
};

// Represents a corner radius.
rs.mimic.CornerRadius = class {
    topLeft = 0;
    topRight = 0;
    bottomRight = 0;
    bottomLeft = 0;
};

// Represents a font.
rs.mimic.Font = class {
    name = "";
    size = 16;
    bold = false;
    italic = false;
    underline = false;
};

// Represents an image condition.
rs.mimic.ImageCondition = class extends rs.mimic.Condition {
    imageName = "";
};

// Represents arguments of the OPEN_LINK action.
rs.mimic.LinkArgs = class {
    url = "";
    target = rs.mimic.LinkTarget.SELF;
    viewID = 0;
    modalWidth = rs.mimic.ModalWidth.NORMAL;
    modalHeight = 0;
};

// Represents a location.
rs.mimic.Location = class {
    x = 0;
    y = 0;
}

// Represents paddings.
rs.mimic.Padding = class {
    top = 0;
    right = 0;
    bottom = 0;
    left = 0;
};

// Represents a property alias.
rs.mimic.PropertyAlias = class {
    name = "";
    path = "";
};

// Represents a property binding.
rs.mimic.PropertyBinding = class {
    propertyName = "";
    dataSource = "";
    dataMember = "";
    format = "";
};

// Represents a size.
rs.mimic.Size = class {
    width = 100;
    height = 100;
}

// Represents a visual state.
rs.mimic.VisualState = class {
    backColor = "";
    foreColor = "";
    borderColor = "";
};
