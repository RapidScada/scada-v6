// Contains classes: Mimic, Component, Panel, Image, FaceplateMeta, Faceplate, FaceplateInstance
// Depends on scada-common.js, mimic-common.js

// Represents a mimic diagram.
rs.mimic.Mimic = class {
    dependencies;
    dependencyMap;
    document;
    components;
    componentMap;
    images;
    imageMap;
    faceplates;
    faceplateMap;

    // Loads a part of the mimic.
    async _loadPart(loadContext) {
        const LoadStep = rs.mimic.LoadStep;
        let continueLoading = false;
        let dto = null;

        if (loadContext.step === LoadStep.UNDEFINED) {
            loadContext.step = LoadStep.PROPERTIES;
            loadContext.result.msg = "Not completed.";
        }

        switch (loadContext.step) {
            case LoadStep.PROPERTIES:
                dto = await this._loadProperties(loadContext);
                loadContext.step++;
                break;

            case LoadStep.COMPONENTS:
                dto = await this._loadComponents(loadContext);
                if (dto.ok && dto.data.endOfComponents) {
                    loadContext.step++;
                }
                break;

            case LoadStep.IMAGES:
                dto = await this._loadImages(loadContext);
                if (dto.ok && dto.data.endOfImages) {
                    loadContext.step++;
                }
                break;

            case LoadStep.FACEPLATES:
                if (this.dependencies.length > 0) {
                    dto = await this._loadFaceplate(loadContext);
                    if (dto.ok && loadContext.faceplateIndex >= this.dependencies.length) {
                        loadContext.step++;
                    }
                } else {
                    loadContext.step++;
                    continueLoading = true;
                }
                break;

            case LoadStep.COMPLETE:
                loadContext.result.ok = true;
                loadContext.result.msg = "";
                break;

            default:
                dto = Dto.fail("Unknown loading step.");
                break;
        }

        if (dto !== null) {
            if (dto.ok) {
                if (dto.data.mimicStamp === loadContext.mimicKey) {
                    continueLoading = true;
                } else {
                    loadContext.result.msg = "Stamp mismatch.";
                }
            } else {
                loadContext.result.msg = dto.msg;
            }
        }

        return continueLoading;
    }

    // Loads the mimic properties.
    async _loadProperties(loadContext) {
        console.log(ScadaUtils.getCurrentTime() + " Load mimic properties");
        let response = await fetch(loadContext.controllerUrl + "GetMimicProperties?key=" + loadContext.mimicKey);

        if (response.ok) {
            let dto = await response.json();

            if (dto.ok) {
                if (Array.isArray(dto.data.dependencies)) {
                    for (let dependency of dto.data.dependencies) {
                        let faceplateMeta = new rs.mimic.FaceplateMeta(dependency);
                        this.dependencies.push(faceplateMeta);
                        this.dependencyMap.set(faceplateMeta.typeName, faceplateMeta);
                    }
                }

                this.document = dto.data.document || {};
            }

            return dto;
        } else {
            return Dto.fail(await response.statusText);
        }
    }

    // Loads a range of components.
    async _loadComponents(loadContext) {
        console.log(ScadaUtils.getCurrentTime() + " Load components starting with " + loadContext.componentIndex);
        let response = await fetch(loadContext.controllerUrl +
            "GetComponents?key=" + loadContext.mimicKey +
            "&index=" + loadContext.componentIndex +
            "&count=" + rs.mimic.LoadContext.COMPONENTS_TO_REQUEST);

        if (response.ok) {
            let dto = await response.json();

            if (dto.ok && Array.isArray(dto.data.components)) {
                loadContext.componentIndex += dto.data.components.length;

                for (let componentObj of dto.data.components) {
                    let component = this._createComponent(componentObj);
                    this.components.push(component);
                    this.componentMap.set(component.id, component);
                }
            }

            return dto;
        } else {
            return Dto.fail(await response.statusText);
        }
    }

    // Loads a range of images.
    async _loadImages(loadContext) {
        console.log(ScadaUtils.getCurrentTime() + " Load images starting with " + loadContext.imageIndex);
        let response = await fetch(loadContext.controllerUrl +
            "GetImages?key=" + loadContext.mimicKey +
            "&index=" + loadContext.imageIndex +
            "&count=" + rs.mimic.LoadContext.COMPONENTS_TO_REQUEST +
            "&size=" + rs.mimic.LoadContext.IMAGE_TOTAL_SIZE);

        if (response.ok) {
            let dto = await response.json();

            if (dto.ok && Array.isArray(dto.data.images)) {
                loadContext.imageIndex += dto.data.images.length;

                for (let imageObj of dto.data.images) {
                    let image = new rs.mimic.Image(imageObj);
                    this.images.push(image);
                    this.imageMap.set(image.name, image);
                }
            }

            return dto;
        } else {
            return Dto.fail(await response.statusText);
        }
    }

    // Loads a faceplate.
    async _loadFaceplate(loadContext) {
        let faceplateMeta = this.dependencies[loadContext.faceplateIndex];
        console.log(ScadaUtils.getCurrentTime() + ` Load '${faceplateMeta.typeName}' faceplate`);
        let response = await fetch(loadContext.controllerUrl +
            "GetFaceplate?key=" + loadContext.mimicKey +
            "&typeName=" + faceplateMeta.typeName);

        if (response.ok) {
            let dto = await response.json();

            if (dto.ok) {
                loadContext.faceplateIndex++;
                let faceplate = new rs.mimic.Faceplate(dto.data);
                this.faceplates.push(faceplate);
                this.faceplateMap.set(faceplateMeta.typeName, faceplate);
            }

            return dto;
        } else {
            return Dto.fail(await response.statusText);
        }
    }

    // Creates a component instance based on the received object.
    _createComponent(source) {
        if (source.typeName === "Panel") {
            return new rs.mimic.Panel(source);
        } else if (this.dependencyMap.has(source.typeName)) {
            return new rs.mimic.FaceplateInstance(source);
        } else {
            return new rs.mimic.Component(source);
        }
    }

    // Finds a parent and children for each component.
    _defineNesting() {
        for (let component of this.components) {
            if (component.parentID > 0) {
                let parent = this.componentMap.get(component.parentID);

                if (parent instanceof rs.mimic.Panel) {
                    component.parent = parent;
                    parent.components.push(component);
                }
            } else {
                component.parent = this;
            }
        }
    }

    // Clears the mimic.
    clear() {
        this.dependencies = [];
        this.dependencyMap = new Map();
        this.document = {};
        this.components = [];
        this.componentMap = new Map();
        this.images = [];
        this.imageMap = new Map();
        this.faceplates = [];
        this.faceplateMap = new Map();
    }

    // Loads the mimic. Returns a LoadResult.
    async load(controllerUrl, mimicKey) {
        let startTime = Date.now();
        console.log(ScadaUtils.getCurrentTime() + " Load mimic with key " + mimicKey)
        this.clear();

        let loadContext = new rs.mimic.LoadContext(controllerUrl, mimicKey);

        while (await this._loadPart(loadContext)) {
            // do nothing
        }

        if (loadContext.result.ok) {
            this._defineNesting();
            console.info(ScadaUtils.getCurrentTime() + " Mimic loading completed successfully in " +
                (Date.now() - startTime) + " ms");
        } else {
            console.error(ScadaUtils.getCurrentTime() + " Mimic loading failed: " + loadContext.result.msg);
        }

        return loadContext.result;
    }
}

// Represents a component of a mimic diagram.
rs.mimic.Component = class {
    id = 0;
    name = "";
    typeName = "";
    parentID = 0;
    properties = null;
    bindings = null;
    access = null;

    parent = null;   // mimic or panel
    dom = null;      // jQuery objects representing DOM content
    renderer = null; // renderer of the component

    constructor(fields) {
        Object.assign(this, fields);
    }

    get isFaceplate() {
        return false;
    }
}

// Represents a panel that can contain child components.
rs.mimic.Panel = class extends rs.mimic.Component {
    components = [];
}

// Represents an image of a mimic diagram.
rs.mimic.Image = class {
    name = "";
    data = null;

    constructor(fields) {
        Object.assign(this, fields);
    }
}

// Represents information about a faceplate.
rs.mimic.FaceplateMeta = class {
    typeName = "";
    path = "";

    constructor(fields) {
        Object.assign(this, fields);
    }
}

// Represents a faceplate, i.e. a user component.
rs.mimic.Faceplate = class {
    document = {};
    components = [];
    images = [];

    constructor(fields) {
        Object.assign(this, fields);
    }
}

// Represents a faceplate instance.
rs.mimic.FaceplateInstance = class extends rs.mimic.Component {
    model = null; // the faceplate model

    get isFaceplate() {
        return true;
    }
}
