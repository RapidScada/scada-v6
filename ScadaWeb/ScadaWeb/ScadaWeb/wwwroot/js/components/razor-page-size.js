// Contains classes: RazorPageSizeOptions, RazorPageSize
// Depends on jquery

/** Represents page size selector options. */
class RazorPageSizeOptions {
    /** Submits the form on item click. Otherwise triggers RazorPageSize.ITEM_CLICK_EVENT. */
    submitOnClick = false;
}

/** 
 * Represents a page size selector, working in conjunction with PageSizeTagHelper. 
 * The markup is updated by server code only.
 */
class RazorPageSize {
    /** Triggered on item click when submitOnClick is false. Handler: (event, pageSize). */
    static ITEM_CLICK_EVENT = "itemclick.rs.page-size";
    /** The CSS selector of the component. */
    static COMPONENT_SELECTOR = ".rs-page-size";

    /** The ID of the HTML element representing the component. */
    componentElemId;
    /** The component options. */
    options;

    constructor(componentElemId, opt_options) {
        this.componentElemId = componentElemId;
        this.options = Object.assign(new RazorPageSizeOptions(), opt_options);
        this.bindEvents();
    }

    /** Handles a user click on a menu item. */
    _handleItemClick(linkElem) {
        const pageSize = linkElem.data("page-size");
        const componentElem = linkElem.closest(".rs-page-size");
        componentElem.find("input[type=hidden]").val(pageSize);

        if (this.options.submitOnClick) {
            componentElem.closest("form").trigger("submit");
        } else {
            componentElem.trigger(RazorPageSize.ITEM_CLICK_EVENT, pageSize);
        }
    }

    /** Returns a jQuery object representing the component. */
    _getComponentElem() {
        return $("#" + this.componentElemId);
    }

    /** Binds events to the DOM elements of the component. */
    bindEvents() {
        this._getComponentElem().on("click", "button.dropdown-item", (event) => {
            event.preventDefault();
            this._handleItemClick($(event.currentTarget));
        });
    }
}
