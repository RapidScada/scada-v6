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

    _appendComponent(listElem, component) {
        let componentNode = $("<span></span>").text(component.displayName);
        let componentItem = $("<li></li>")
            .attr("id", "comp-item" + component.id)
            .attr("data-id", component.id)
            .append(componentNode).appendTo(listElem);

        if (!component.isFaceplate && component.isContainer && component.children.length > 0) {
            let childList = $("<ul></ul>").appendTo(componentItem);

            for (let childComponent of component.children) {
                this._appendComponent(childList, childComponent);
            }
        }
    }

    prepare() {
        let listElem = $("<ul class='top-level-list'></ul>");

        // dependencies
        let dependenciesItem = $("<li></li>").text(this.phrases.dependenciesNode).appendTo(listElem);
        let dependenciesList = $("<ul></ul>").appendTo(dependenciesItem);

        for (let dependency of this.mimic.dependencies) {
            let dependencyNode = $("<span></span>").text(dependency.typeName);
            $("<li></li>").append(dependencyNode).appendTo(dependenciesList);
        }

        // components
        let mimicNode = $("<span></span>").text(this.phrases.mimicNode);
        let mimicItem = $("<li class='mimic-item'></li>").append(mimicNode).appendTo(listElem);
        let componentList = $("<ul></ul>").appendTo(mimicItem);

        for (let component of this.mimic.children) {
            this._appendComponent(componentList, component);
        }

        // images
        let imagesItem = $("<li></li>").text(this.phrases.imagesNode).appendTo(listElem);
        let imagesList = $("<ul></ul>").appendTo(imagesItem);

        for (let image of this.mimic.images) {
            let imageNode = $("<span></span>").text(image.name);
            $("<li></li>").append(imageNode).appendTo(imagesList);
        }

        this.structElem.append(listElem);
    }

    addComponent(component) {
        let listElem = component.parentID > 0
            ? this.structElem.find(`#comp-item${component.parentID}>ul`) 
            : this.structElem.find(".mimic-item>ul");

        if (listElem.length > 0) {
            this._appendComponent(listElem, component);
        }
    }

    updateComponent(component) {
        this.structElem.find(`#comp-item${component.id} span:first`).text(component.displayName);
    }

    removeComponent(componentID) {

    }
}
