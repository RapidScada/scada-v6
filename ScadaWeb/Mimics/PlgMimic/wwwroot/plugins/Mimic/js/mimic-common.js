// Contains classes: LoadStep, LoadResult, LoadContext, ObjectHelper
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
