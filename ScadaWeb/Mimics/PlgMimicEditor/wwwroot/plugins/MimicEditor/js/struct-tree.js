// Contains classes: StructTree
// Depends on jquery

// Represents a component for displaying mimic structure.
class StructTree {
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
        $("<span class='node-btn view-btn'><i class='fa-solid fa-plus'></i></span>").appendTo(dependenciesNode);

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
                $("<li></li>").append(dependencyNode).appendTo(dependenciesList);
            }
        }
    }

    _prepareImages(listElem) {
        let imagesNode = $("<span class='node'></span>");
        $("<span class='node-text'></span>").text(this.phrases.imagesNode).appendTo(imagesNode);
        $("<span class='node-btn view-btn'><i class='fa-solid fa-plus'></i></span>").appendTo(imagesNode);

        let imagesItem = $("<li class='images-item'></li>").append(imagesNode).appendTo(listElem);
        let imagesList = $("<ul></ul>").appendTo(imagesItem);

        for (let image of this.mimic.images) {
            let imageNode = $("<span class='node'></span>");
            $("<span class='node-text'></span>").text(image.name)
                .appendTo(imageNode);
            $("<span class='node-btn view-btn'><i class='fa-regular fa-eye'></i></span>")
                .appendTo(imageNode);
            $("<span class='node-btn edit-btn'><i class='fa-solid fa-pen-to-square'></i></span>")
                .appendTo(imageNode);
            $("<span class='node-btn remove-btn'><i class='fa-regular fa-trash-can'></i></span>")
                .appendTo(imageNode);
            $("<li></li>").append(imageNode).appendTo(imagesList);
        }
    }

    _prepareComponents(listElem) {
        let mimicNode = $("<span class='node'></span>").text(this.phrases.mimicNode);
        let mimicItem = $("<li class='mimic-item'></li>").append(mimicNode).appendTo(listElem);
        let componentList = $("<ul></ul>").appendTo(mimicItem);

        for (let component of this.mimic.children) {
            this._appendComponent(componentList, component);
        }
    }

    _appendComponent(listElem, component) {
        let componentNode = $("<span class='node'></span>").text(component.displayName);
        let componentItem = $("<li></li>")
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

    _findMimicItem() {
        return this.structElem.find(".mimic-item");
    }

    _findComponentItem(component) {
        return this.structElem.find("#struct-comp-item" + component.id);
    }

    prepare() {
        let listElem = $("<ul class='top-level-list'></ul>");
        this._prepareDependencies(listElem);
        this._prepareImages(listElem);
        this._prepareComponents(listElem);
        this.structElem.append(listElem);
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
        this._findComponentItem(component).children(".node").text(component.displayName);
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

    addToSelection(component) {
        this._findMimicItem().children(".node").removeClass("selected");
        this._findComponentItem(component).children(".node").addClass("selected");
    }

    removeFromSelection(component) {
        this._findComponentItem(component).children(".node").removeClass("selected");
    }
}
