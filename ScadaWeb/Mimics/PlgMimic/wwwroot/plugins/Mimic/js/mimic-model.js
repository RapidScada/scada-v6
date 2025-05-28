// Contains classes: MimicHelper, MimicBase, Mimic, Component, Panel, Image, 
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

    // Moves the components to the beginning of the parent's children.
    static sendToBack(parent, components) {
        if (parent.children) {
            let componentIDs = new Set(components.map(c => c.id));
            parent.children = parent.children.filter(c => !componentIDs.has(c.id));
            parent.children.unshift(...components);
        }
    }

    // Moves the components one position towards the beginning of the parent's children.
    static sendBackward(parent, components) {
        if (parent.children) {
            let indexes = components.map(c => parent.children.indexOf(c)).sort();
            let prevIndex = -1;

            for (let index of indexes) {
                if (index >= 0 && prevIndex < index - 1) {
                    prevIndex = index - 1;
                    [parent.children[index], parent.children[prevIndex]] =
                        [parent.children[prevIndex], parent.children[index]];
                } else {
                    prevIndex = index;
                }
            }
        }
    }

    // Moves the components one position towards the end of the parent's children.
    static bringForward(parent, components) {
        if (parent.children) {
            let indexes = components.map(c => parent.children.indexOf(c)).sort();
            let nextIndex = parent.children.length;

            for (let i = indexes.length - 1; i >= 0; i--) {
                let index = indexes[i];

                if (index >= 0 && nextIndex > index + 1) {
                    nextIndex = index + 1;
                    [parent.children[index], parent.children[nextIndex]] =
                        [parent.children[nextIndex], parent.children[index]];
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
        }
    }

    // Moves the components before their sibling.
    static placeBefore(parent, sibling, components) {
        if (parent.children) {
            let componentIDs = new Set(components.map(c => c.id));
            let filtered = parent.children.filter(c => !componentIDs.has(c.id));
            let index = filtered.indexOf(sibling);

            if (index >= 0) {
                filtered.splice(index, 0, ...components);
                parent.children = filtered;
            }
        }
    }

    // Moves the components after their sibling.
    static placeAfter(parent, sibling, components) {
        if (parent.children) {
            let componentIDs = new Set(components.map(c => c.id));
            let filtered = parent.children.filter(c => !componentIDs.has(c.id));
            let index = filtered.indexOf(sibling);

            if (index >= 0) {
                filtered.splice(index + 1, 0, ...components);
                parent.children = filtered;
            }
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

        component.addToParent(parent, opt_index);
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

        component.removeFromParent();
        component.addToParent(parent, opt_index);
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
            component.removeFromParent();

            for (let id of idsToRemove) {
                this.componentMap.delete(id);
            }
        }

        return component;
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

    addToParent(parent, opt_index) {
        if (parent?.children) {
            this.parentID = parent.id ?? 0;
            this.index = Number.isInteger(opt_index) && 0 <= opt_index && opt_index < parent.children.length
                ? opt_index
                : parent.children.length;
            this.parent = parent;
            parent.children.splice(this.index, 0, this); // insert
        }
    }

    removeFromParent() {
        if (this.index >= 0 && this.parent?.children) {
            this.parent.children.splice(this.index, 1); // delete
            this.index = -1;
        }
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
