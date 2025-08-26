// Contains classes: LoadStep, LoadResult, LoadContext, EventType, ScaleType, Scale, ObjectHelper
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

// Specifies the event types used by a mimic.
rs.mimic.EventType = class {
    static BLINK_ON = "blinkon.rs.mimic";
    static BLINK_OFF = "blinkoff.rs.mimic";
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
