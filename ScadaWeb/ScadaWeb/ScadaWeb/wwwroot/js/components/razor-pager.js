// Contains classes: RazorPagerOptions, RazorPager
// Depends on jquery

/** Represents page size selector options. */
class RazorPagerOptions {
    /** Submits the form on item click. Otherwise triggers RazorPager.PAGE_CLICK_EVENT. */
    submitOnClick = false;
}

/**
 * Represents a component for managing pagination, working in conjunction with PagerTagHelper.
 * The markup is updated by server code only.
 */
class RazorPager {
    /** Triggered on page item click when submitOnClick is false. Handler: (event, pageIndex). */
    static PAGE_CLICK_EVENT = "pageclick.rs.pager";
    /** The CSS selector of the component. */
    static COMPONENT_SELECTOR = ".rs-pager";

    /** The ID of the HTML element representing the component. */
    componentElemId;
    /** The component options. */
    options;

    constructor(componentElemId, opt_options) {
        this.componentElemId = componentElemId;
        this.options = Object.assign(new RazorPagerOptions(), opt_options);
        this.bindEvents();
    }

    /** Handles a user click on a page item. */
    _handlePageClick(linkElem) {
        let pageIndex = linkElem.data("page");
        let pagerElem = linkElem.closest(".rs-pager");
        pagerElem.find("input[type=hidden]").val(pageIndex);

        if (this.options.submitOnClick) {
            pagerElem.closest("form").trigger("submit");
        } else {
            pagerElem.trigger(RazorPager.PAGE_CLICK_EVENT, pageIndex);
        }
    }

    /** Returns a jQuery object representing the component. */
    _getComponentElem() {
        return $("#" + this.componentElemId);
    }

    /** Binds events to the DOM elements of the component. */
    bindEvents() {
        this._getComponentElem().on("click", "a.page-link", (event) => {
            this._handlePageClick($(event.target));
            event.preventDefault();
        });
    }

    /** Sets the current page index to zero. */
    reset(opt_submit) {
        const componentElem = this._getComponentElem();
        componentElem.find("input[type=hidden]").val(0);

        if (opt_submit) {
            componentElem.closest("form").trigger("submit");
        }
    }
}
