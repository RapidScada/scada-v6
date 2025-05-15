// Contains classes: StructTree, StructTreeEventType
// Depends on jquery, bootstrap

// Represents a component for displaying mimic structure.
class StructTree {
    _eventSource = document.createElement("structtree");

    structElem;
    mimic;
    phrases;

    constructor(elemID, mimic, phrases) {
        this.structElem = $("#" + elemID);
        this.mimic = mimic;
        this.phrases = phrases;
    }

    _prepareDependencies(listElem) {
        let dependenciesNode = $("<span class='node'></span>");
        $("<span class='node-text'></span>").text(this.phrases.dependenciesNode).appendTo(dependenciesNode);
        $("<span class='node-btn add-btn'><i class='fa-solid fa-plus'></i></span>").appendTo(dependenciesNode);

        let dependenciesItem = $("<li class='dependencies-item'></li>").append(dependenciesNode).appendTo(listElem);
        let dependenciesList = $("<ul></ul>").appendTo(dependenciesItem);

        for (let dependency of this.mimic.dependencies) {
            if (!dependency.isTransitive) {
                let dependencyNode = $("<span class='node'></span>");
                $("<span class='node-text'></span>").text(dependency.typeName)
                    .appendTo(dependencyNode);
                $("<span class='node-btn edit-btn'><i class='fa-solid fa-pen-to-square'></i></span>")
                    .appendTo(dependencyNode);
                $("<span class='node-btn remove-btn'><i class='fa-regular fa-trash-can'></i></span>")
                    .appendTo(dependencyNode);
                $("<li class='dependency-item'></li>").attr("data-name", dependency.typeName)
                    .append(dependencyNode).appendTo(dependenciesList);
            }
        }
    }

    _prepareImages(listElem) {
        let imagesNode = $("<span class='node'></span>");
        $("<span class='node-text'></span>").text(this.phrases.imagesNode).appendTo(imagesNode);
        $("<span class='node-btn add-btn'><i class='fa-solid fa-plus'></i></span>").appendTo(imagesNode);

        let imagesItem = $("<li class='images-item'></li>").append(imagesNode).appendTo(listElem);
        let imagesList = $("<ul></ul>").appendTo(imagesItem);

        for (let image of this.mimic.images) {
            let imageNode = $("<span class='node'></span>");
            $("<span class='node-text'></span>").text(image.name)
                .appendTo(imageNode);
            let viewBtn = $("<span class='node-btn view-btn'><i class='fa-regular fa-eye'></i></span>")
                .appendTo(imageNode);
            $("<span class='node-btn edit-btn'><i class='fa-solid fa-pen-to-square'></i></span>")
                .appendTo(imageNode);
            $("<span class='node-btn remove-btn'><i class='fa-regular fa-trash-can'></i></span>")
                .appendTo(imageNode);
            $("<li class='image-item'></li>").attr("data-name", image.name)
                .append(imageNode).appendTo(imagesList);
            this._initImagePopover(viewBtn, image.name);
        }
    }

    _initImagePopover(buttonElem, imageName) {
        const thisObj = this;

        bootstrap.Popover.getOrCreateInstance(buttonElem[0], {
            html: true,
            placement: "bottom",
            content: function () {
                // called twice by Bootstrap on each show
                let popoverContent = buttonElem.data("popoverContent");

                if (!popoverContent) {
                    let dataUrl = thisObj.mimic.imageMap.get(imageName)?.dataUrl;
                    popoverContent = dataUrl
                        ? `<img class="image-preview" src="${dataUrl}" />`
                        : thisObj.phrases.noImagePreview;
                    buttonElem.data("popoverContent", popoverContent);
                }

                return popoverContent;
            }
        });
    }

    _prepareComponents(listElem) {
        let mimicNode = $("<span class='node mimic-node'></span>").text(this.phrases.mimicNode);
        let mimicItem = $("<li class='mimic-item'></li>").append(mimicNode).appendTo(listElem);
        let componentList = $("<ul></ul>").appendTo(mimicItem);

        for (let component of this.mimic.children) {
            this._appendComponent(componentList, component);
        }
    }

    _appendComponent(listElem, component) {
        let componentNode = $("<span class='node comp-node'></span>").text(component.displayName);
        let componentItem = $("<li class='comp-item'></li>")
            .attr("id", "struct-comp-item" + component.id)
            .attr("data-id", component.id)
            .append(componentNode).appendTo(listElem);

        if (component.isContainer) {
            let childList = $("<ul></ul>").appendTo(componentItem);

            for (let childComponent of component.children) {
                this._appendComponent(childList, childComponent);
            }
        }
    }

    _bindEvents() {
        const thisObj = this;

        // dependencies
        this.structElem.find(".dependencies-item")
            .on("click", ".add-btn", function () {
                thisObj._eventSource.dispatchEvent(new CustomEvent(StructTreeEventType.ADD_DEPENDENCY_CLICK));
            })
            .on("click", ".edit-btn", function () {
                thisObj._eventSource.dispatchEvent(new CustomEvent(StructTreeEventType.EDIT_DEPENDENCY_CLICK, {
                    detail: { name: $(this).closest(".dependency-item").data("name") }
                }));
            })
            .on("click", ".remove-btn", function () {
                thisObj._eventSource.dispatchEvent(new CustomEvent(StructTreeEventType.REMOVE_DEPENDENCY_CLICK, {
                    detail: { name: $(this).closest(".dependency-item").data("name") }
                }));
            });

        // images
        this.structElem.find(".images-item")
            .on("click", ".add-btn", function () {
                thisObj._eventSource.dispatchEvent(new CustomEvent(StructTreeEventType.ADD_IMAGE_CLICK));
            })
            .on("click", ".edit-btn", function () {
                thisObj._eventSource.dispatchEvent(new CustomEvent(StructTreeEventType.EDIT_IMAGE_CLICK, {
                    detail: { name: $(this).closest(".image-item").data("name") }
                }));
            })
            .on("click", ".remove-btn", function () {
                thisObj._eventSource.dispatchEvent(new CustomEvent(StructTreeEventType.REMOVE_IMAGE_CLICK, {
                    detail: { name: $(this).closest(".image-item").data("name") }
                }));
            });

        // mimic and components
        this.structElem.find(".mimic-item")
            .on("click", ".mimic-node", function () {
                thisObj._eventSource.dispatchEvent(new CustomEvent(StructTreeEventType.MIMIC_CLICK));
            })
            .on("click", ".comp-node", function (event) {
                thisObj._eventSource.dispatchEvent(new CustomEvent(StructTreeEventType.COMPONENT_CLICK, {
                    detail: {
                        componentID: $(this).parent().data("id"),
                        isSelected: $(this).hasClass("selected"),
                        ctrlKey: event.ctrlKey
                    }
                }));
            });
    }

    _findMimicItem() {
        return this.structElem.find(".mimic-item");
    }

    _findComponentItem(component) {
        return this.structElem.find("#struct-comp-item" + component.id);
    }

    _findComponentNode(component) {
        return this._findComponentItem(component).children(".node");
    }

    prepare() {
        let listElem = $("<ul class='top-level-list'></ul>");
        this._prepareDependencies(listElem);
        this._prepareImages(listElem);
        this._prepareComponents(listElem);
        this.structElem.append(listElem);
        this._bindEvents();
    }

    refreshDependencies() {

    }

    refreshImages() {
        //let imagesList = this.structElem.find(".images-list:first");
    }

    addComponent(component) {
        let listElem = component.parentID > 0
            ? this.structElem.find(`#struct-comp-item${component.parentID}>ul`) 
            : this.structElem.find(".mimic-item>ul");

        if (listElem.length > 0) {
            this._appendComponent(listElem, component);
        }
    }

    updateComponent(component) {
        this._findComponentNode(component).text(component.displayName);
    }

    removeComponent(componentID) {
        this.structElem.find("#struct-comp-item" + componentID).remove();
    }

    selectMimic() {
        let mimicItem = this._findMimicItem();
        mimicItem.children(".node").addClass("selected");
        mimicItem.children("ul").find(".node").removeClass("selected");
    }

    selectNone() {
        this._findMimicItem().find(".node").removeClass("selected");
    }

    selectComponents(components) {
        this.selectNone();

        if (Array.isArray(components)) {
            for (let component of components) {
                this._findComponentNode(component).addClass("selected");
            }
        }
    }

    addToSelection(component) {
        this._findMimicItem().children(".node").removeClass("selected");
        this._findComponentNode(component).addClass("selected");
    }

    removeFromSelection(component) {
        this._findComponentNode(component).removeClass("selected");
    }

    addEventListener(type, listener) {
        this._eventSource.addEventListener(type, listener);
    }
}

// Specifies the event types for a mimic structure component.
class StructTreeEventType {
    static ADD_DEPENDENCY_CLICK = "addDependencyClick";
    static EDIT_DEPENDENCY_CLICK = "editDependencyClick";
    static REMOVE_DEPENDENCY_CLICK = "removeDependencyClick";

    static ADD_IMAGE_CLICK = "addImageClick";
    static EDIT_IMAGE_CLICK = "editImageClick";
    static REMOVE_IMAGE_CLICK = "removeImageClick";

    static MIMIC_CLICK = "mimicClick";
    static COMPONENT_CLICK = "componentClick";
}
