﻿// Contains classes: MimicHelper, MimicBase, Mimic, Component, Panel, Image, 
//     FaceplateMeta, Faceplate, FaceplateInstance
// Depends on scada-common.js, mimic-common.js

// Provides helper methods for mimics and components.
rs.mimic.MimicHelper = class {
    // Finds a parent and children for each component.
    static defineNesting(root, components, opt_componentMap) {
        let componentMap = opt_componentMap ?? new Map(components.map(c => [c.id, c]));

        for (let component of components) {
            if (component.parentID > 0) {
                let parent = componentMap.get(component.parentID);

                if (parent) {
                    component.parent = parent;
                    parent.children ??= [];
                    parent.children.push(component);
                }
            } else {
                component.parent = root;
                root.children ??= [];
                root.children.push(component);
            }
        }
    }
}

// A base class for mimic diagrams and faceplates.
rs.mimic.MimicBase = class {
    document;   // mimic properties
    components; // all components
    images;     // image collection
    children;   // top-level components

    // Creates a component instance based on the received object.
    _createComponent(source) {
        return new rs.mimic.Component(source);
    }

    // Finds a parent and children for each component.
    _defineNesting(componentMap) {
        rs.mimic.MimicHelper.defineNesting(this, this.components, componentMap);
    }
}

// Represents a mimic diagram.
rs.mimic.Mimic = class extends rs.mimic.MimicBase {
    dependencies; // meta information about faceplates
    faceplates;   // TODO: remove

    componentMap;
    imageMap;
    dependencyMap;
    faceplateMap;
    dom;

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
                if (dto.data.mimicKey === loadContext.mimicKey) {
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
            return Dto.fail(response.statusText);
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

                for (let sourceComponent of dto.data.components) {
                    let component = this._createComponent(sourceComponent);
                    this.components.push(component);
                    this.componentMap.set(component.id, component);
                }
            }

            return dto;
        } else {
            return Dto.fail(response.statusText);
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

                for (let sourceImage of dto.data.images) {
                    let image = new rs.mimic.Image(sourceImage);
                    this.images.push(image);
                    this.imageMap.set(image.name, image);
                }
            }

            return dto;
        } else {
            return Dto.fail(response.statusText);
        }
    }

    // Loads a faceplate.
    async _loadFaceplate(loadContext) {
        let faceplateMeta = this.dependencies[loadContext.faceplateIndex];
        let typeName = faceplateMeta.typeName;
        console.log(ScadaUtils.getCurrentTime() + ` Load '${typeName}' faceplate`);
        let response = await fetch(loadContext.controllerUrl +
            "GetFaceplate?key=" + loadContext.mimicKey +
            "&typeName=" + typeName);

        if (response.ok) {
            let dto = await response.json();

            if (dto.ok) {
                loadContext.faceplateIndex++;
                let faceplate = new rs.mimic.Faceplate(dto.data, typeName);
                this.faceplates.push(faceplate);
                this.faceplateMap.set(typeName, faceplate);
            }

            return dto;
        } else {
            return Dto.fail(response.statusText);
        }
    }

    // Creates a component instance based on the received object.
    _createComponent(source) {
        return this.dependencyMap.has(source.typeName)
            ? new rs.mimic.FaceplateInstance(source)
            : super._createComponent(source);
    }

    // Prepares the faceplates instances for use.
    _prepareFaceplates() {
        for (let component of this.components) {
            if (component instanceof rs.mimic.FaceplateInstance) {
                let faceplate = mimic.faceplateMap.get(component.typeName);

                if (faceplate) {
                    component.applyModel(faceplate);
                }
            }
        }
    }

    // Clears the mimic.
    clear() {
        this.document = {};
        this.components = [];
        this.images = [];
        this.children = [];

        this.dependencies = [];
        this.faceplates = [];

        this.componentMap = new Map();
        this.imageMap = new Map();
        this.dependencyMap = new Map();
        this.faceplateMap = new Map();
        this.dom = null;
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
            this._defineNesting(this.componentMap);
            this._prepareFaceplates();
            console.info(ScadaUtils.getCurrentTime() + " Mimic loading completed successfully in " +
                (Date.now() - startTime) + " ms");
        } else {
            console.error(ScadaUtils.getCurrentTime() + " Mimic loading failed: " + loadContext.result.msg);
        }

        return loadContext.result;
    }

    // Adds the component to the mimic.
    addComponent(component, parent, x, y) {
        if (parent.isContainer) {
            component.parentID = parent.id;
            component.parent = parent;
            parent.children.push(component);
        } else {
            component.parentID = 0;
            component.parent = this;
            this.children.push(component);
        }

        component.properties.location = {
            x: x.toString(),
            y: y.toString()
        };

        this.components.push(component);
        this.componentMap.set(component.id, component);
    }

    // Removes the component from the mimic. Returns the removed component.
    removeComponent(componentID) {
        let component = this.componentMap.get(componentID);

        if (component) {
            let idsToRemove = new Set();
            idsToRemove.add(componentID);

            if (component.parent) {
                let index = component.parent.children.indexOf(component);
                component.parent.children.splice(index, 1); // delete
            }

            if (component.isContainer) {
                for (let childComponent of component.getAllChildren()) {
                    idsToRemove.add(childComponent.id);
                }
            }

            this.components = this.components.filter(c => !idsToRemove.has(c.id));

            for (let id of idsToRemove) {
                this.componentMap.delete(id);
            }
        }

        return component;
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
    children = null; // top-level child components
    dom = null;      // jQuery objects representing DOM content
    renderer = null; // renders the component

    constructor(source) {
        Object.assign(this, source);
    }

    get isContainer() {
        return Array.isArray(this.children);
    }

    get isFaceplate() {
        return false;
    }

    get displayName() {
        return this.name
            ? `[${this.id}] ${this.name} - ${this.typeName}`
            : `[${this.id}] ${this.typeName}`;
    }

    getAllChildren() {
        let allChildren = [];

        function appendChildren(component) {
            if (component.isContainer) {
                for (let child of component.children) {
                    allChildren.push(child);
                    appendChildren(child);
                }
            }
        }

        appendChildren(this);
        return allChildren;
    }
}

// Represents an image of a mimic diagram.
rs.mimic.Image = class {
    name = "";
    data = null;

    constructor(source) {
        Object.assign(this, source);
    }
}

// Represents information about a faceplate.
rs.mimic.FaceplateMeta = class {
    typeName = "";
    path = "";

    constructor(source) {
        Object.assign(this, source);
    }
}

// Represents a faceplate, i.e. a user component.
rs.mimic.Faceplate = class extends rs.mimic.MimicBase {
    typeName = "";

    constructor(source, typeName) {
        super();
        this.document = source.document ?? {};
        this.components = [];
        this.images = [];
        this.children = [];
        this.typeName = typeName;

        if (Array.isArray(source.components)) {
            for (let sourceComponent of source.components) {
                this.components.push(this._createComponent(sourceComponent));
            }

            this._defineNesting(null);
        }

        if (Array.isArray(source.images)) {
            for (let sourceImage of source.images) {
                this.images.push(new rs.mimic.Image(sourceImage));
            }
        }
    }

    copyComponents() {
        let components = [];

        for (let sourceComponent of this.components) {
            components.push(this._createComponent(sourceComponent));
        }

        return components;
    }
}

// Represents a faceplate instance.
rs.mimic.FaceplateInstance = class extends rs.mimic.Component {
    model = null;      // model of the Faceplate type
    components = null; // copy of the model components
    children = null;   // top-level child components

    get isContainer() {
        // child components are essential part of the faceplate, it does not accept additional components
        return false;
    }

    get isFaceplate() {
        return true;
    }

    applyModel(faceplate) {
        this.properties ??= {};
        this.properties.size ??= Object.assign({}, faceplate.document.size);

        this.model = faceplate;
        this.components = faceplate.copyComponents();
        this.children = [];
        rs.mimic.MimicHelper.defineNesting(this, this.components);
    }
}
