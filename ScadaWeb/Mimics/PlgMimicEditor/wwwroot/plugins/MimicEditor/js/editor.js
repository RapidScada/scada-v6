// Contains classes: UpdateDTO, ChangeType, Change.
// No dependencies

// Represents a data transfer object containing mimic changes.
class UpdateDTO {
    mimicKey;
    changes = [];

    constructor(mimicKey, ...changes) {
        this.mimicKey = mimicKey;

        if (changes) {
            this.changes.push(...changes);
        }
    }
}

// Specifies the change types.
class ChangeType {
    static NONE = 0;

    static UPDATE_DOCUMENT = 1;
    static ADD_DEPENDENCY = 2;
    static DELETE_DEPENDENCY = 3;

    static ADD_COMPONENT = 4;
    static UPDATE_COMPONENT = 5;
    static UPDATE_COMPONENT_PARENT = 6;
    static UPDATE_COMPONENT_BINDINGS = 7;
    static UPDATE_COMPONENT_ACCESS = 8;
    static DELETE_COMPONENT = 9;

    static ADD_IMAGE = 10;
    static RENAME_IMAGE = 11;
    static DELETE_IMAGE = 12;
}

// Represents a change in a mimic.
class Change {
    changeType;

    constructor(changeType) {
        this.changeType = changeType ?? ChangeType.NONE;
    }

    static updateComponent(componentID, properties) {
        let change = new Change(ChangeType.UPDATE_COMPONENT);
        change.componentID = componentID;
        change.properties = properties;
        return change;
    }
}
