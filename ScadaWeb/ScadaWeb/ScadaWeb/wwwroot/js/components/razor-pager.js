// Represents a component for managing pagination, working in conjunction with PagerTagHelper.
// Depends on jquery
class RazorPager {
    // Notifies that a page item has been clicked.
    static PAGE_CLICK_EVENT = "pageclick.rs.pager";
    // The CSS class of the pager.
    static PAGER_CLASS = ".rs-pager";

    // The default pager options.
    static defaultOptions = {
        submitOnClick: false // submits the form when a page is clicked
    };

    pagerElemID; // ID of the HTML element representing the pager
    options;     // pager options

    constructor(pagerElemID, opt_options) {
        this.pagerElemID = pagerElemID;
        this.options = Object.assign({}, RazorPager.defaultOptions, opt_options);
        this.bindEvents();
    }

    // Handles a user click on a page item.
    _handlePageClick(linkElem) {
        let pageIndex = linkElem.data("page");
        let pagerElem = linkElem.closest(".rs-pager");
        pagerElem.find("input:hidden:first").val(pageIndex);

        if (this.options.submitOnClick) {
            pagerElem.closest("form").trigger("submit");
        } else {
            pagerElem.trigger(RazorPager.PAGE_CLICK_EVENT, pageIndex);
        }
    }

    // Returns a jQuery object representing the pager.
    _getPagerElem() {
        return $("#" + this.pagerElemID);
    }

    // Binds events to the DOM elements.
    bindEvents() {
        this._getPagerElem().on("click", "a.page-link", (event) => {
            this._handlePageClick($(event.target));
            event.preventDefault();
        });
    }

    // Sets the current page index to zero.
    reset(opt_submit) {
        let pagerElem = this._getPagerElem();
        pagerElem.find("input:hidden:first").val(0);

        if (opt_submit) {
            pagerElem.closest("form").trigger("submit");
        }
    }
}
