// Represents a component for managing pagination.
// Depends on jquery
class Pager {
    // The jQuery object that represents the pager.
    pagerElem;

    constructor(pagerElemID) {        
        this.pagerElem = $("#" + pagerElemID);

        this.pagerElem.find("a.page-link").click(function () {
            Pager._handlePageClick($(this));
            return false;
        });
    }

    // Handles a user click on a page item.
    static _handlePageClick(elem) {
        elem.closest(".rs-pager").find("input:hidden:first").val(elem.data("page"));
        elem.closest("form").submit();
    }

    // Sets the current page index to zero.
    reset(opt_submit) {
        this.pagerElem.find("input:hidden:first").val(0);

        if (opt_submit) {
            elem.closest("form").submit();
        }
    }

    // Binds events to all pagers of the document.
    static bindEvents() {
        $(".rs-pager a.page-link").click(function () {
            Pager._handlePageClick($(this));
            return false;
        });
    }
}
