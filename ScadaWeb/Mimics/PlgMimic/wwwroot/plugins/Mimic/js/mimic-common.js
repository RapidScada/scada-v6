// Contains classes: ...

// Namespaces
const rs = {};
rs.mimic = rs.mimic || {};

// Specifies the steps for loading a mimic.
rs.mimic.LoadStep = class {
    static UNDEFINED = 0;
    static PROPERTIES = 1;
    static COMPONENTS = 2;
    static IMAGES = 3;
    static FACEPLATES = 4;
    static COMPLETE = 5;
}

// Represents a result of loading a mimic.
rs.mimic.LoadResult = class {
    ok = false;
    msg = "";
}

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

    constructor(controllerUrl, mimicKey) {
        this.controllerUrl = controllerUrl;
        this.mimicKey = mimicKey;
    }
}
