// Contains subtypes for basic components.

rs.mimic.BasicColorCondition = class BasicColorCondition extends rs.mimic.Condition {
    color = "";

    get typeName() {
        return "BasicColorCondition";
    }

    static parse(source) {
        const PropertyParser = rs.mimic.PropertyParser;
        let colorCondition = new BasicColorCondition();

        if (source) {
            colorCondition.color = PropertyParser.parseString(source.color);
            colorCondition._copyFrom(source);
        }

        return colorCondition;
    }
};
