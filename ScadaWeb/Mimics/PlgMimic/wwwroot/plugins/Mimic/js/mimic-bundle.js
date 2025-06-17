// Contains classes: LoadStep, LoadResult, LoadContext
// No dependencies

// Namespaces
var rs = rs ?? {};
rs.mimic = rs.mimic ?? {};

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
        this.mimicKey = mimicKey.toString();
    }
}

// Contains classes: ComponentFactory, TextFactory, PictureFactory, PanelFactory, FaceplateFactory, FactorySet
// Depends on mimic-model.js

// Represents an abstract component factory.
rs.mimic.ComponentFactory = class {
    createComponent(typeName) {
        let component = new rs.mimic.Component();
        component.typeName = typeName;
        component.properties = {
            enabled: true,
            visible: true,
            location: { x: "0", y: "0" },
            size: { width: "100", height: "100" }
        };
        return component;
    }
}

// Creates components of the Text type.
rs.mimic.TextFactory = class extends rs.mimic.ComponentFactory {
    createComponent() {
        let component = super.createComponent("Text");
        component.properties.text = "Text";
        return component;
    }
}

// Creates components of the Picture type.
rs.mimic.PictureFactory = class extends rs.mimic.ComponentFactory {
    createComponent() {
        return super.createComponent("Picture");
    }
}

// Creates components of the Panel type.
rs.mimic.PanelFactory = class extends rs.mimic.ComponentFactory {
    createComponent() {
        let component = super.createComponent("Panel");
        component.children = [];
        return component;
    }
}

// Creates faceplates.
rs.mimic.FaceplateFactory = class {
    static createComponent(faceplate) {
        let faceplateInstance = new rs.mimic.FaceplateInstance();
        faceplateInstance.typeName = faceplate.typeName;
        faceplateInstance.properties = {
            location: { x: "0", y: "0" }
        };
        faceplateInstance.applyModel(faceplate);
        return faceplateInstance;
    }
}

// Contains factories for mimic components.
rs.mimic.FactorySet = class {
    static componentFactories = new Map([
        ["Text", new rs.mimic.TextFactory()],
        ["Picture", new rs.mimic.PictureFactory()],
        ["Panel", new rs.mimic.PanelFactory()]
    ]);

    static getFaceplateFactory(faceplate) {
        return {
            createComponent: function () {
                return rs.mimic.FaceplateFactory.createComponent(faceplate);
            }
        };
    }
}

// Contains classes: MimicHelper, MimicBase, Mimic, Component, Panel, Image, 
//     FaceplateMeta, Faceplate, FaceplateInstance
// Depends on scada-common.js, mimic-common.js

// Provides helper methods for mimics and components.
rs.mimic.MimicHelper = class MimicHelper {
    // Indexes child components.
    static _indexComponents(parent, opt_start) {
        for (let index = opt_start ?? 0; index < parent.children.length; index++) {
            let component = parent.children[index];
            component.index = index;
        }
    }

    // Finds a parent and children for each component.
    static defineNesting(root, components, opt_componentMap) {
        let componentMap = opt_componentMap ?? new Map(components.map(c => [c.id, c]));

        for (let component of components) {
            if (component.parentID > 0) {
                let parent = componentMap.get(component.parentID);

                if (parent) {
                    parent.children ??= [];
                    component.index = parent.children.length;
                    component.parent = parent;
                    parent.children.push(component);
                }
            } else {
                root.children ??= [];
                component.index = root.children.length;
                component.parent = root;
                root.children.push(component);
            }
        }
    }

    // Adds the component to the parent.
    static addToParent(parent, component, opt_index) {
        if (parent.children) {
            component.parentID = parent.id ?? 0;
            let index = Number.isInteger(opt_index) && 0 <= opt_index && opt_index < parent.children.length
                ? opt_index
                : parent.children.length;
            component.parent = parent;
            parent.children.splice(index, 0, component); // insert
            MimicHelper._indexComponents(parent, index);
        }
    }

    // Removes the component from its parent.
    static removeFromParent(component) {
        let parent = component.parent;

        if (parent.children && component.index >= 0) {
            parent.children.splice(component.index, 1); // delete
            MimicHelper._indexComponents(parent, component.index);
            component.index = -1;
        }
    }

    // Checks whether the specified objects are a parent and a child.
    static areRelatives(parent, child) {
        let current = child.parent;

        while (current) {
            if (current === parent) {
                return true;
            }

            current = current.parent;
        }

        return false;
    }

    // Checks whether the specified components belong to the same parent.
    static areSiblings(components) {
        let parentIDs = new Set(components.map(c => c.parentID));
        return parentIDs.size === 1;
    }

    // Moves the components to the beginning of the parent's children.
    static sendToBack(parent, components) {
        if (parent.children) {
            let componentIDs = new Set(components.map(c => c.id));
            parent.children = parent.children.filter(c => !componentIDs.has(c.id));
            parent.children.unshift(...components);
            MimicHelper._indexComponents(parent);
        }
    }

    // Moves the components one position towards the beginning of the parent's children.
    static sendBackward(parent, components) {
        if (parent.children) {
            let indexes = components.map(c => c.index).sort();
            let prevIndex = -1;

            for (let index of indexes) {
                if (index >= 0 && prevIndex < index - 1) {
                    prevIndex = index - 1;
                    let component = parent.children[index];
                    let prevComponent = parent.children[prevIndex];
                    parent.children[index] = prevComponent;
                    parent.children[prevIndex] = component;
                    component.index--;
                    prevComponent.index++;
                } else {
                    prevIndex = index;
                }
            }
        }
    }

    // Moves the components one position towards the end of the parent's children.
    static bringForward(parent, components) {
        if (parent.children) {
            let indexes = components.map(c => c.index).sort();
            let nextIndex = parent.children.length;

            for (let i = indexes.length - 1; i >= 0; i--) {
                let index = indexes[i];

                if (index >= 0 && nextIndex > index + 1) {
                    nextIndex = index + 1;
                    let component = parent.children[index];
                    let nextComponent = parent.children[nextIndex];
                    parent.children[index] = nextComponent;
                    parent.children[nextIndex] = component;
                    component.index++;
                    nextComponent.index--;
                } else {
                    nextIndex = index;
                }
            }
        }
    }

    // Moves the components to the end of the parent's children.
    static bringToFront(parent, components) {
        if (parent.children) {
            let componentIDs = new Set(components.map(c => c.id));
            parent.children = parent.children.filter(c => !componentIDs.has(c.id));
            parent.children.push(...components);
            MimicHelper._indexComponents(parent);
        }
    }

    // Moves the components before their sibling.
    static placeBefore(parent, components, sibling) {
        if (parent.children) {
            let componentIDs = new Set(components.map(c => c.id));
            let filtered = parent.children.filter(c => !componentIDs.has(c.id));
            let index = filtered.indexOf(sibling);

            if (index >= 0) {
                filtered.splice(index, 0, ...components);
                parent.children = filtered;
                MimicHelper._indexComponents(parent);
            }
        }
    }

    // Moves the components after their sibling.
    static placeAfter(parent, components, sibling) {
        if (parent.children) {
            let componentIDs = new Set(components.map(c => c.id));
            let filtered = parent.children.filter(c => !componentIDs.has(c.id));
            let index = filtered.indexOf(sibling);

            if (index >= 0) {
                filtered.splice(index + 1, 0, ...components);
                parent.children = filtered;
                MimicHelper._indexComponents(parent);
            }
        }
    }

    // Arranges the components according to the indexes.
    static arrange(parent, components, indexes) {
        if (parent.children && components.length === indexes.length) {
            // map components
            let componentByIndex = new Map();
            let componentIDs = new Set();

            for (let i = 0; i < components.length; i++) {
                let component = components[i];
                componentByIndex.set(indexes[i], component);
                componentIDs.add(component.id);
            }

            // copy children to new array
            let arranged = [];
            let arrangedIndex = 0;
            let sourceIndex = 0;
            let length = parent.children.length;

            while (arrangedIndex < length && sourceIndex < length) {
                let component = componentByIndex.get(arrangedIndex);

                if (component) {
                    component.index = arrangedIndex++;
                    arranged.push(component);
                } else {
                    component = parent.children[sourceIndex++];

                    if (!componentIDs.has(component.id)) {
                        component.index = arrangedIndex++;
                        arranged.push(component);
                    }
                }
            }

            parent.children = arranged;
        }
    }

    // Gets the minimum coordinates of the components.
    static getMinLocation(components) {
        let minX = NaN;
        let minY = NaN;

        for (let component of components) {
            let x = component.x;
            let y = component.y;

            if (isNaN(minX) || minX > x) {
                minX = x;
            }

            if (isNaN(minY) || minY > y) {
                minY = y;
            }
        }

        return {
            x: minX,
            y: minY
        };
    }
}

// A base class for mimic diagrams and faceplates.
rs.mimic.MimicBase = class {
    dependencies;  // meta information about faceplates
    document;      // mimic properties
    components;    // all components
    images;        // image collection
    faceplates;    // faceplate collection

    dependencyMap; // dependencies accessible by type name
    componentMap;  // components accessible by ID
    imageMap;      // images accessible by name
    faceplateMap;  // faceplates accessible by type name
    children;      // top-level components

    // Clears the mimic.
    clear() {
        this.dependencies = [];
        this.document = {};
        this.components = [];
        this.images = [];
        this.faceplates = [];

        this.dependencyMap = new Map();
        this.componentMap = new Map();
        this.imageMap = new Map();
        this.faceplateMap = new Map();
        this.children = [];
    }

    // Creates a component instance based on the source object.
    createComponent(source) {
        return this.dependencyMap?.has(source.typeName)
            ? new rs.mimic.FaceplateInstance(source)
            : new rs.mimic.Component(source);
    }

    // Creates a copy of the component containing only the main properties.
    copyComponent(source) {
        if (source instanceof rs.mimic.Component) {
            source = source.toPlainObject();
        }

        return this.createComponent(ScadaUtils.deepClone(source));
    }
}

// Represents a mimic diagram.
rs.mimic.Mimic = class extends rs.mimic.MimicBase {
    dom;      // mimic DOM as a jQuery object
    renderer; // renders the mimic

    // Imitates a component ID to use as a parent ID.
    get id() {
        return 0;
    }

    // Indicates that a mimic can contain child components.
    get isContainer() {
        return true;
    }

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
                    let faceplateMeta = this.dependencies[loadContext.faceplateIndex];

                    if (faceplateMeta.hasError) {
                        continueLoading = true;
                    } else {
                        dto = await this._loadFaceplate(loadContext, faceplateMeta.typeName);
                    }

                    if (++loadContext.faceplateIndex >= this.dependencies.length) {
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
                    for (let sourceDependency of dto.data.dependencies) {
                        let faceplateMeta = new rs.mimic.FaceplateMeta(sourceDependency);
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
                    let component = this.createComponent(sourceComponent);
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
    async _loadFaceplate(loadContext, typeName) {
        console.log(ScadaUtils.getCurrentTime() + ` Load '${typeName}' faceplate`);
        let response = await fetch(loadContext.controllerUrl +
            "GetFaceplate?key=" + loadContext.mimicKey +
            "&typeName=" + typeName);

        if (response.ok) {
            let dto = await response.json();

            if (dto.ok) {
                let faceplate = new rs.mimic.Faceplate(dto.data, typeName);
                this.faceplates.push(faceplate);
                this.faceplateMap.set(typeName, faceplate);
            }

            return dto;
        } else {
            return Dto.fail(response.statusText);
        }
    }

    // Finds a parent and children for each component.
    _defineNesting() {
        rs.mimic.MimicHelper.defineNesting(this, this.components, this.componentMap);
    }

    // Prepares the faceplates for use.
    _prepareFaceplates() {
        for (let faceplate of this.faceplates) {
            for (let faceplateMeta of faceplate.dependencies) {
                let childFaceplate = this.faceplateMap.get(faceplateMeta.typeName);

                if (childFaceplate) {
                    faceplate.faceplates.push(childFaceplate);
                    faceplate.faceplateMap.set(faceplateMeta.typeName, childFaceplate);
                }
            }
        }
    }

    // Prepares the faceplates instances for use.
    _prepareFaceplateInstances() {
        for (let component of this.components) {
            if (component.isFaceplate) {
                let faceplate = mimic.faceplateMap.get(component.typeName);

                if (faceplate) {
                    component.applyModel(faceplate);
                }
            }
        }
    }

    // Clears the mimic.
    clear() {
        super.clear();
        this.dom = null;
        this.renderer = null;
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
            this._prepareFaceplates();
            this._prepareFaceplateInstances();
            console.info(ScadaUtils.getCurrentTime() + " Mimic loading completed successfully in " +
                (Date.now() - startTime) + " ms");
        } else {
            console.error(ScadaUtils.getCurrentTime() + " Mimic loading failed: " + loadContext.result.msg);
        }

        return loadContext.result;
    }

    // Adds the dependency to the mimic or replaces the existing one.
    addDependency(faceplateMeta) {
        let existingDependency = this.dependencyMap.get(faceplateMeta.typeName);

        if (existingDependency) {
            let index = this.dependencies.indexOf(existingDependency);
            this.dependencies[index] = faceplateMeta;
        } else {
            this.dependencies.push(faceplateMeta);
        }

        this.dependencies.sort(); // sort by type name
        this.dependencyMap.set(faceplateMeta.typeName, faceplateMeta);
    }

    // Removes the dependency from the mimic.
    removeDependency(typeName) {
        let dependency = this.dependencyMap.get(typeName);

        if (dependency) {
            let index = this.dependencies.indexOf(dependency);
            this.dependencies.splice(index, 1); // delete
            this.dependencyMap.delete(typeName);
        }
    }

    // Adds the image to the mimic or replaces the existing one.
    addImage(image) {
        let existingImage = this.imageMap.get(image.name);

        if (existingImage) {
            let index = this.images.indexOf(existingImage);
            this.images[index] = image;
        } else {
            this.images.push(image);
        }

        this.images.sort(); // sort by name
        this.imageMap.set(image.name, image);
    }

    // Removes the image from the mimic.
    removeImage(imageName) {
        let image = this.imageMap.get(imageName);

        if (image) {
            let index = this.images.indexOf(image);
            this.images.splice(index, 1); // delete
            this.imageMap.delete(imageName);
        }
    }

    // Adds the component to the mimic. Returns true if the component was added.
    addComponent(component, parent, opt_index, opt_x, opt_y) {
        if (!component || !parent || !parent.isContainer ||
            component.id <= 0 || this.componentMap.has(component.id)) {
            return false;
        }

        if (opt_x && opt_y) {
            component.setLocation(opt_x, opt_y);
        }

        rs.mimic.MimicHelper.addToParent(parent, component, opt_index);
        this.components.push(component);
        this.componentMap.set(component.id, component);
        return true;
    }

    // Updates the parent of the component. Returns true if the parent was updated.
    updateParent(component, parent, opt_index, opt_x, opt_y) {
        if (!component || !parent || !parent.isContainer || component === parent ||
            rs.mimic.MimicHelper.areRelatives(component, parent) /*component contains parent*/) {
            return false;
        }

        if (opt_x && opt_y) {
            component.setLocation(opt_x, opt_y);
        }

        rs.mimic.MimicHelper.removeFromParent(component);
        rs.mimic.MimicHelper.addToParent(parent, component, opt_index);
        return true;
    }

    // Removes the component from the mimic. Returns the removed component.
    removeComponent(componentID) {
        let component = this.componentMap.get(componentID);

        if (component) {
            // get IDs to remove
            let idsToRemove = new Set();
            idsToRemove.add(componentID);

            if (component.isContainer) {
                for (let childComponent of component.getAllChildren()) {
                    idsToRemove.add(childComponent.id);
                }
            }

            // remove components
            this.components = this.components.filter(c => !idsToRemove.has(c.id));
            rs.mimic.MimicHelper.removeFromParent(component);

            for (let id of idsToRemove) {
                this.componentMap.delete(id);
            }
        }

        return component;
    }

    // Gets the parent of a compoment by ID.
    getComponentParent(parentID) {
        return parentID > 0 ? this.componentMap.get(parentID) : this;
    }

    // Returns a string that represents the current object.
    toString() {
        return "Mimic";
    }
}

// Represents a component of a mimic diagram.
rs.mimic.Component = class {
    id = 0;
    name = "";
    typeName = "";
    properties = null;
    bindings = null;
    access = null;
    parentID = 0;
    index = -1;

    parent = null;      // mimic or panel
    children = null;    // top-level child components
    dom = null;         // jQuery objects representing DOM content
    renderer = null;    // renders the component
    isSelected = false; // selected in the editor

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

    get x() {
        return parseInt(this.properties?.location?.x) || 0;
    }

    set x(value) {
        if (this.properties?.location) {
            this.properties.location.x = value.toString();
        }
    }

    get y() {
        return parseInt(this.properties?.location?.y) || 0;
    }

    set y(value) {
        if (this.properties?.location) {
            this.properties.location.y = value.toString();
        }
    }

    get width() {
        return parseInt(this.properties?.size?.width) || 0;
    }

    set width(value) {
        if (this.properties?.size) {
            this.properties.size.width = value.toString();
        }
    }

    get height() {
        return parseInt(this.properties?.size?.height) || 0;
    }

    set height(value) {
        if (this.properties?.size) {
            this.properties.size.height = value.toString();
        }
    }

    setLocation(x, y) {
        if (this.properties) {
            this.properties.location = {
                x: x.toString(),
                y: y.toString()
            };
        }
    }

    setSize(width, height) {
        if (this.properties) {
            this.properties.size = {
                width: width.toString(),
                height: height.toString()
            };
        }
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

    toPlainObject() {
        return {
            id: this.id,
            name: this.name,
            typeName: this.typeName,
            properties: this.properties,
            bindings: this.bindings,
            access: this.access,
            parentID: this.parentID,
            index: this.index,
            children: this.children ? [] : null
        };
    }

    toString() {
        return this.displayName;
    }
}

// Represents an image of a mimic diagram.
rs.mimic.Image = class {
    name = "";
    mediaType = "";
    data = null;

    constructor(source) {
        Object.assign(this, source);
    }

    get dataUrl() {
        return this.data ? `data:${this.mediaType};base64,${this.data}` : "";
    }

    set dataUrl(value) {
        if (value && value.startsWith("data:")) {
            let index = value.indexOf(";base64,");

            if (index >= 0) {
                this.mediaType = value.substring(5, index);
                this.data = value.substring(index + 8);
                return;
            } 
        }

        this.data = null;
    }

    toString() {
        // required for sorting
        return this.name;
    }
}

// Represents information about a faceplate.
rs.mimic.FaceplateMeta = class {
    typeName = "";
    path = "";
    isTransitive = false;
    hasError = false;

    constructor(source) {
        Object.assign(this, source);
    }

    toString() {
        // required for sorting
        return this.typeName;
    }
}

// Represents a faceplate, i.e. a user component.
rs.mimic.Faceplate = class extends rs.mimic.MimicBase {
    typeName = "";

    constructor(source, typeName) {
        super();
        this.clear();
        this.document = source.document ?? {};
        this.typeName = typeName;

        if (Array.isArray(source.dependencies)) {
            for (let sourceDependency of source.dependencies) {
                let faceplateMeta = new rs.mimic.FaceplateMeta(sourceDependency);
                this.dependencies.push(faceplateMeta);
                this.dependencyMap.set(faceplateMeta.typeName, faceplateMeta);
            }
        }

        if (Array.isArray(source.components)) {
            for (let sourceComponent of source.components) {
                let component = this.createComponent(sourceComponent);
                this.components.push(component);
                this.componentMap.set(component.id, component);
            }
        }

        if (Array.isArray(source.images)) {
            for (let sourceImage of source.images) {
                let image = new rs.mimic.Image(sourceImage);
                this.images.push(image);
                this.imageMap.set(image.name, image);
            }
        }
    }
}

// Represents a faceplate instance.
rs.mimic.FaceplateInstance = class extends rs.mimic.Component {
    model = null;      // model of the Faceplate type
    components = null; // copy of the model components

    get isContainer() {
        // child components are essential part of the faceplate, it does not accept additional components
        return false;
    }

    get isFaceplate() {
        return true;
    }

    applyModel(faceplate) {
        if (faceplate instanceof rs.mimic.Faceplate) {
            this.properties ??= {};
            this.properties.size ??= ScadaUtils.deepClone(faceplate.document.size);

            this.model = faceplate;
            this.components = [];

            for (let sourceComponent of faceplate.components) {
                let componentCopy = faceplate.copyComponent(sourceComponent);
                componentCopy.parent = this;
                this.components.push(componentCopy);

                if (componentCopy.isFaceplate) {
                    let childFaceplate = faceplate.faceplateMap.get(componentCopy.typeName);
                    componentCopy.applyModel(childFaceplate);
                }
            }

            rs.mimic.MimicHelper.defineNesting(this, this.components);
        }
    }
}

// Contains classes: Renderer, MimicRenderer, ComponentRenderer,
//     TextRenderer, PictureRenderer, PanelRenderer, RenderContext, RendererSet, UnitedRenderer
// Depends on jquery, scada-common.js, mimic-common.js

// Represents a renderer of a mimic or component.
rs.mimic.Renderer = class {
    // Gets a value indocating whether the renderer supports DOM update.
    get canUpdateDom() {
        return false;
    }

    // Sets the left and top of the specified jQuery object.
    _setLocation(jqObj, location) {
        jqObj.css({
            "left": location.x + "px",
            "top": location.y + "px"
        });
    }

    // Sets the width and height of the specified jQuery object.
    _setSize(jqObj, size) {
        jqObj.css({
            "width": size.width + "px",
            "height": size.height + "px"
        });
    }

    // Sets the background image of the specified jQuery object.
    _setBackgroundImage(jqObj, image) {
        jqObj.css("background-image", this._imageToDataUrlCss(image));
    }

    // Returns a css property value for the image data URI.
    _imageToDataUrlCss(image) {
        return image ? "url('" + image.dataUrl + "')" : "";
    }

    // Creates a component DOM according to the component model. Returns a jQuery object.
    createDom(component, renderContext) {
        return null;
    }

    // Updates the existing component DOM according to the component model.
    updateDom(component, renderContext) {
    }

    // Updates the component view according to the current channel data.
    update(component, renderContext) {
    }

    // Sets the location of the component DOM.
    setLocation(component, x, y) {
        if (component.dom) {
            this._setLocation(component.dom, {
                x: x.toString(),
                y: y.toString()
            });
        }
    }

    // Gets the component location from its DOM.
    getLocation(component) {
        if (component.dom) {
            let position = component.dom.position();
            return {
                x: position.left.toString(),
                y: position.top.toString()
            };
        } else {
            return {
                x: "0",
                y: "0"
            };
        }
    }

    // Sets the size of the component DOM.
    setSize(component, width, height) {
        if (component.dom) {
            this._setSize(component.dom, {
                width: width.toString(),
                height: height.toString()
            });
        }
    }

    // Gets the component size from its DOM.
    getSize(component) {
        if (component.dom) {
            return {
                width: component.dom.outerWidth().toString(),
                height: component.dom.outerHeight().toString()
            };
        } else {
            return {
                width: "0",
                height: "0"
            };
        }
    }

    // Appends the child DOM into the parent DOM.
    static appendChild(parent, child) {
        if (parent.dom && child.dom) {
            parent.dom.append(child.dom);
        }
    }

    // Removes the component DOM from the mimic keeping data associated with the removed elements.
    static detach(component) {
        component.dom?.detach();
    }

    // Removes the component DOM from the mimic.
    static remove(component) {
        component.dom?.remove();
    }

    // Arranges the child components according to their order.
    static arrangeChildren(parent) {
        if (parent.children && parent.dom) {
            for (let component of parent.children) {
                component.dom?.detach();
            }

            for (let component of parent.children) {
                if (component.dom) {
                    parent.dom.append(component.dom);
                }
            }
        }
    }
}

// Represents a mimic renderer.
rs.mimic.MimicRenderer = class MimicRenderer extends rs.mimic.Renderer {
    static _GRID_COLOR = "#dee2e6"; // gray-300

    get canUpdateDom() {
        return true;
    }

    static _createGrid(gridSize, mimicSize) {
        // create canvas
        let canvasElem = $("<canvas class='grid'></canvas>");
        let canvas = canvasElem[0];
        let width = canvas.width = mimicSize.width;
        let height = canvas.height = mimicSize.height;

        // prepare drawing context
        let context = canvas.getContext("2d");
        context.lineWidth = 1;
        context.strokeStyle = MimicRenderer._GRID_COLOR;

        // draw grids with small and large cells
        MimicRenderer._drawGrid(context, width, height, gridSize, [1, 1]);
        MimicRenderer._drawGrid(context, width, height, gridSize * 10);
        return canvasElem;
    }

    static _drawGrid(context, width, height, gridStep, dashSegments = []) {
        const adj = 0.5; // adjustment for sharpness of lines

        // draw vertical lines
        for (let x = gridStep; x < width; x += gridStep) {
            context.beginPath();
            context.setLineDash(dashSegments);
            context.moveTo(x + adj, 0);
            context.lineTo(x + adj, height);
            context.stroke();
        }

        // draw horizontal lines
        for (let y = gridStep; y < height; y += gridStep) {
            context.beginPath();
            context.setLineDash(dashSegments);
            context.moveTo(0, y + adj);
            context.lineTo(width, y + adj);
            context.stroke();
        }
    }

    createDom(mimic, renderContext) {
        let mimicElem = $("<div class='mimic'></div>");
        mimic.dom = mimicElem;

        if (renderContext.editMode && renderContext.editorOptions &&
            renderContext.editorOptions.showGrid && renderContext.editorOptions.gridStep > 1) {
            mimicElem.append(MimicRenderer._createGrid(renderContext.editorOptions.gridStep, mimic.document.size));
        }

        this.updateDom(mimic, renderContext);
        return mimicElem;
    }

    updateDom(mimic, renderContext) {
        if (mimic.dom) {
            let mimicElem = mimic.dom.first();
            this._setSize(mimicElem, mimic.document.size);
        }
    }
}

// Represents a component renderer.
rs.mimic.ComponentRenderer = class extends rs.mimic.Renderer {
    createDom(component, renderContext) {
        let componentElem = $("<div class='comp'></div>")
            .attr("id", "comp" + renderContext.idPrefix + component.id)
            .attr("data-id", component.id);

        if (renderContext.editMode) {
            if (!renderContext.faceplateMode && component.isContainer) {
                componentElem.addClass("container")
            }

            if (component.isSelected) {
                componentElem.addClass("selected")
            }
        }

        component.dom = componentElem;
        return componentElem;
    }

    updateLocation(component) {
        if (component.dom) {
            this._setLocation(component.dom, component.properties.location);
        }
    }

    updateSelected(component) {
        if (component.dom) {
            component.dom.toggleClass("selected", component.isSelected);
        }
    }

    allowResizing(component) {
        return true;
    }
}

// Represents a text component renderer.
rs.mimic.TextRenderer = class extends rs.mimic.ComponentRenderer {
    createDom(component, renderContext) {
        let textElem = super.createDom(component, renderContext);
        let props = component.properties;
        textElem.addClass("text").text(props.text);
        this._setLocation(textElem, props.location);
        this._setSize(textElem, props.size);
        return textElem;
    }
}

// Represents a picture component renderer.
rs.mimic.PictureRenderer = class extends rs.mimic.ComponentRenderer {
    createDom(component, renderContext) {
        let pictureElem = super.createDom(component, renderContext);
        let props = component.properties;
        pictureElem.addClass("picture");
        this._setLocation(pictureElem, props.location);
        this._setSize(pictureElem, props.size);
        this._setBackgroundImage(pictureElem, renderContext.getImage(props.imageName));
        return pictureElem;
    }
}

// Represents a panel component renderer.
rs.mimic.PanelRenderer = class extends rs.mimic.ComponentRenderer {
    get canUpdateDom() {
        return true;
    }

    createDom(component, renderContext) {
        let panelElem = super.createDom(component, renderContext);
        panelElem.addClass("panel");
        this.updateDom(component, renderContext);
        return panelElem;
    }

    updateDom(component, renderContext) {
        if (component.dom) {
            let panelElem = component.dom.first();
            let props = component.properties;
            this._setLocation(panelElem, props.location);
            this._setSize(panelElem, props.size);
        }
    }
}

// Represents a faceplate renderer.
rs.mimic.FaceplateRenderer = class extends rs.mimic.ComponentRenderer {
    get canUpdateDom() {
        return true;
    }

    createDom(component, renderContext) {
        let faceplateElem = super.createDom(component, renderContext);
        faceplateElem.addClass("faceplate");
        this.updateDom(component, renderContext);
        return faceplateElem;
    }

    updateDom(component, renderContext) {
        if (component.dom) {
            let faceplateElem = component.dom.first();
            let props = component.properties;
            this._setLocation(faceplateElem, props.location);
            this._setSize(faceplateElem, props.size);
        }
    }
}

// Encapsulates information about a rendering operation.
rs.mimic.RenderContext = class {
    editMode = false;
    editorOptions = null;
    faceplateMode = false;
    imageMap = null;
    idPrefix = "";

    constructor(source) {
        Object.assign(this, source);
    }

    getImage(imageName) {
        return this.imageMap instanceof Map ? this.imageMap.get(imageName) : null;
    }
}

// Contains renderers for a mimic and its components.
rs.mimic.RendererSet = class {
    static mimicRenderer = new rs.mimic.MimicRenderer();
    static faceplateRenderer = new rs.mimic.FaceplateRenderer();
    static componentRenderers = new Map([
        ["Text", new rs.mimic.TextRenderer()],
        ["Picture", new rs.mimic.PictureRenderer()],
        ["Panel", new rs.mimic.PanelRenderer()]
    ]);
}

// Renders a mimic using appropriate renderers.
rs.mimic.UnitedRenderer = class {
    mimic;
    editMode;
    editorOptions;

    constructor(mimic, editMode) {
        this.mimic = mimic;
        this.editMode = editMode;
        this.editorOptions = null;
    }

    // Appends the component DOM to its parent.
    _appendToParent(component) {
        if (component.parent) {
            rs.mimic.Renderer.appendChild(component.parent, component);
        }
    }

    // Creates a component DOM.
    _createComponentDom(component, renderContext, opt_unknownTypes) {
        if (component.isFaceplate) {
            this._createFaceplateDom(component, renderContext, opt_unknownTypes);
        } else {
            let renderer = rs.mimic.RendererSet.componentRenderers.get(component.typeName);

            if (renderer) {
                component.renderer = renderer;
                renderer.createDom(component, renderContext);
                this._appendToParent(component);
            } else {
                opt_unknownTypes?.add(component.typeName);
            }
        }
    }

    // Creates a faceplate DOM.
    _createFaceplateDom(faceplateInstance, renderContext, opt_unknownTypes) {
        if (!faceplateInstance.model) {
            opt_unknownTypes?.add(faceplateInstance.typeName);
            return;
        }

        let faceplateContext = new rs.mimic.RenderContext({
            editMode: this.editMode,
            editorOptions: this.editorOptions,
            faceplateMode: true,
            imageMap: faceplateInstance.model.imageMap,
            idPrefix: renderContext.idPrefix
        });
        let renderer = rs.mimic.RendererSet.faceplateRenderer;
        faceplateInstance.renderer = renderer;
        renderer.createDom(faceplateInstance, faceplateContext);
        this._appendToParent(faceplateInstance);
        faceplateContext.idPrefix += faceplateInstance.id + "-";

        for (let component of faceplateInstance.components) {
            this._createComponentDom(component, faceplateContext, opt_unknownTypes);
        }
    }

    // Creates a mimic DOM according to the mimic model. Returns a jQuery object.
    createMimicDom() {
        let startTime = Date.now();
        let unknownTypes = new Set();
        let renderContext = new rs.mimic.RenderContext({
            editMode: this.editMode,
            editorOptions: this.editorOptions,
            imageMap: this.mimic.imageMap
        });
        let renderer = rs.mimic.RendererSet.mimicRenderer;
        this.mimic.renderer = renderer;
        renderer.createDom(this.mimic, renderContext);

        for (let component of this.mimic.components) {
            this._createComponentDom(component, renderContext, unknownTypes);
        }

        if (unknownTypes.size > 0) {
            console.warn("Unknown component types: " + Array.from(unknownTypes).sort().join(", "));
        }

        if (this.mimic.dom) {
            console.info("Mimic DOM created in " + (Date.now() - startTime) + " ms");
            return this.mimic.dom;
        } else {
            return $();
        }
    }

    // Updates a mimic DOM according to the mimic model.
    updateMimicDom() {
        rs.mimic.RendererSet.mimicRenderer.updateDom(this.mimic);
    }

    // Creates a component DOM according to the component model. Returns a jQuery object.
    createComponentDom(component) {
        this._createComponentDom(component, new rs.mimic.RenderContext({
            editMode: this.editMode,
            editorOptions: this.editorOptions,
            imageMap: this.mimic.imageMap
        }));
        return component.dom ?? $();
    }

    // Updates the component DOM according to the component model.
    updateComponentDom(component) {
        if (component.dom && component.renderer) {
            let renderContext = new rs.mimic.RenderContext({
                editMode: this.editMode,
                editorOptions: this.editorOptions
            });

            if (component.isFaceplate) {
                renderContext.faceplateMode = true;
                renderContext.imageMap = component.model.imageMap;
                component.renderer.updateDom(component, renderContext);
            } else {
                renderContext.imageMap = mimic.imageMap;

                if (component.renderer.canUpdateDom) {
                    component.renderer.updateDom(component, renderContext);
                } else {
                    let oldDom = component.dom;
                    component.renderer.createDom(component, renderContext);
                    oldDom.replaceWith(component.dom);
                }
            }
        }
    }

    // Arranges the child component DOMs according to the parent's model.
    arrangeChildren(parent) {
        if (parent.children) {
            // detach components
            for (let component of parent.children) {
                if (component.dom) {
                    component.dom.detach();
                }
            }

            // append components
            for (let component of parent.children) {
                this._appendToParent(component);
            }
        }
    }
}
