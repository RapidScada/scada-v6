// Contains factories for basic components.

rs.mimic.BasicButtonFactory = class extends rs.mimic.RegularComponentFactory {
    createProperties() {
        let props = super.createProperties();

        // change inherited properties
        props.size.width = 100;
        props.size.height = 30;

        // appearance
        Object.assign(props, {
            imageName: "",
            imageSize: new rs.mimic.Size({ width: 16, height: 16 }),
            text: "Button",
        });

        return props;
    }

    parseProperties(sourceProps) {
        const PropertyParser = rs.mimic.PropertyParser;
        let props = super.parseProperties(sourceProps);
        sourceProps ??= {};

        // appearance
        Object.assign(props, {
            imageName: PropertyParser.parseString(sourceProps.imageName),
            imageSize: rs.mimic.Size.parse(sourceProps.imageSize),
            text: PropertyParser.parseString(sourceProps.text),
        });

        return props;
    }

    createComponent() {
        return super.createComponent("BasicButton");
    }
};

rs.mimic.BasicLedScript = class extends rs.mimic.ComponentScript {
    dataUpdated(args) {
        // select background color according to conditions
        let cnlNum = args.component.bindings?.inCnlNum;
        let props = args.component.properties;
        let conditions = props.conditions;

        if (cnlNum > 0 && conditions.length > 0) {
            let curData = args.dataProvider.getCurData(cnlNum);
            let prevData = args.dataProvider.getPrevData(cnlNum);

            if (dataProvider.dataChanged(curData, prevData)) {
                let color = props.defaultColor;

                if (curData.d.stat > 0) {
                    for (let condition of conditions) {
                        if (condition.satisfied(curData.d.val)) {
                            color = condition.color;
                            break;
                        }
                    }
                }

                if (props.backColor !== color) {
                    props.backColor = color;
                    args.propertyChanged = true;
                }
            }
        }
    }
};

rs.mimic.BasicLedFactory = class extends rs.mimic.RegularComponentFactory {
    _createExtraScript() {
        return new rs.mimic.BasicLedScript();
    }

    _addDefaultConditions(conditions) {
        const BasicColorCondition = rs.mimic.BasicColorCondition;
        const ComparisonOperator = rs.mimic.ComparisonOperator;

        conditions.push(new BasicColorCondition({
            comparisonOper1: ComparisonOperator.LESS_THAN_EQUAL,
            color: "Red"
        }));

        conditions.push(new BasicColorCondition({
            comparisonOper1: ComparisonOperator.GREATER_THAN,
            color: "Green"
        }));
    }

    createProperties() {
        let props = super.createProperties();

        // change inherited properties
        props.backColor = "Silver";
        props.border.color = "Black";
        props.border.width = 3;
        props.size.width = 30;
        props.size.height = 30;

        // appearance
        props.borderOpacity = 30;
        props.isSquare = false;

        // behavior
        props.conditions = new rs.mimic.BasicColorConditionList();
        props.defaultColor = "Silver";

        this._addDefaultConditions(props.conditions);
        return props;
    }

    parseProperties(sourceProps) {
        const PropertyParser = rs.mimic.PropertyParser;
        let props = super.parseProperties(sourceProps);
        sourceProps ??= {};

        // appearance
        props.borderOpacity = PropertyParser.parseInt(sourceProps.borderOpacity);
        props.isSquare = PropertyParser.parseBool(sourceProps.isSquare);

        // behavior
        props.conditions = rs.mimic.BasicColorConditionList.parse(sourceProps.conditions);
        props.defaultColor = PropertyParser.parseString(sourceProps.defaultColor);
        return props;
    }

    createComponent() {
        return super.createComponent("BasicLed");
    }
};

rs.mimic.BasicToggleScript = class extends rs.mimic.ComponentScript {
    dataUpdated(args) {
        // set toggle position
        const BasicTogglePosition = rs.mimic.BasicTogglePosition;
        let cnlNum = args.component.bindings?.inCnlNum;

        if (cnlNum > 0) {
            let curData = args.dataProvider.getCurData(cnlNum);
            let prevData = args.dataProvider.getPrevData(cnlNum);

            if (dataProvider.dataChanged(curData, prevData)) {
                let props = args.component.properties;
                let position = props.position;

                if (curData.d.stat > 0) {
                    position = curData.d.val > 0
                        ? BasicTogglePosition.ON
                        : BasicTogglePosition.OFF;
                } else {
                    position = BasicTogglePosition.NOT_SET;
                }

                if (props.position !== position) {
                    props.position = position;
                    args.propertyChanged = true;
                }
            }
        }
    }

    getCommandValue(args) {
        return args.component.properties.position === rs.mimic.BasicTogglePosition.ON ? 0 : 1;
    }
};

rs.mimic.BasicToggleFactory = class extends rs.mimic.RegularComponentFactory {
    _createDefaultBindings(component) {
        const DataMember = rs.mimic.DataMember;
        let cnlNum = component.bindings.inCnlNum;
        let cnlProps = component.bindings.inCnlProps;
        let bindings = [];

        if (!component.properties.backColor) {
            bindings.push({
                propertyName: "backColor",
                dataSource: String(cnlNum),
                dataMember: DataMember.COLOR0,
                format: "",
                propertyChain: ["backColor"],
                cnlNum: cnlNum,
                cnlProps: cnlProps
            });
        }

        if (!component.properties.border.color) {
            bindings.push({
                propertyName: "border.color",
                dataSource: String(cnlNum),
                dataMember: DataMember.COLOR0,
                format: "",
                propertyChain: ["border", "color"],
                cnlNum: cnlNum,
                cnlProps: cnlProps
            });
        }

        return bindings;
    }

    _createExtraScript() {
        return new rs.mimic.BasicToggleScript();
    }

    createProperties() {
        let props = super.createProperties();

        // change inherited properties
        props.border.width = 2;
        props.size.width = 60;
        props.size.height = 30;

        // appearance
        props.position = rs.mimic.BasicTogglePosition.ON;

        // layout
        props.padding = new rs.mimic.Padding();

        return props;
    }

    parseProperties(sourceProps) {
        const PropertyParser = rs.mimic.PropertyParser;
        let props = super.parseProperties(sourceProps);
        sourceProps ??= {};
        props.position = PropertyParser.parseString(sourceProps.position, rs.mimic.BasicTogglePosition.NOT_SET);
        props.padding = rs.mimic.Padding.parse(sourceProps.padding);
        return props;
    }

    createComponent() {
        return super.createComponent("BasicToggle");
    }
};

// Registers the factories. The function name must be unique.
function registerBasicFactories() {
    let componentFactories = rs.mimic.FactorySet.componentFactories;
    componentFactories.set("BasicButton", new rs.mimic.BasicButtonFactory());
    componentFactories.set("BasicLed", new rs.mimic.BasicLedFactory());
    componentFactories.set("BasicToggle", new rs.mimic.BasicToggleFactory());
}

registerBasicFactories();
