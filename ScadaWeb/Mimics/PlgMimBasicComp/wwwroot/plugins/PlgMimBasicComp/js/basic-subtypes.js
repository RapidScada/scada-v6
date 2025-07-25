// Contains subtypes for basic components.

rs.mimic.BasicSubtype = class {
    static COLOR_CONDITION = "BasicColorCondition";
};

rs.mimic.BasicColorCondition = class extends rs.mimic.Condition {
    color = "";

    get typeName() {
        return "BasicColorCondition";
    }

    static parse(source) {
        const PropertyParser = rs.mimic.PropertyParser;
        let colorCondition = new rs.mimic.BasicColorCondition();

        if (source) {
            colorCondition.color = PropertyParser.parseString(source.color);
            colorCondition._copyFrom(source);
        }

        return colorCondition;
    }
};

rs.mimic.BasicColorConditionList = class extends rs.mimic.List {
    constructor() {
        super(() => {
            return new rs.mimic.BasicColorCondition();
        });
    }

    static parse(source) {
        const BasicColorCondition = rs.mimic.BasicColorCondition;
        let colorConditions = new rs.mimic.BasicColorConditionList();

        if (Array.isArray(source)) {
            for (let sourceItem of source) {
                colorConditions.push(BasicColorCondition.parse(sourceItem));
            }
        }

        return colorConditions;
    }
}
