// Represents a client-side component for managing pagination.
// Depends on jquery
class ClientPager {
    // The default page size.
    static DEFAULT_PAGE_SIZE = 10;
    // The number of pages displayed by a pager, not including the Previous and Next buttons.
    static DISPLAY_PAGE_COUNT = 9;
    // Notifies that a page link has been clicked.
    static PAGE_CLICK_EVENT = "rs:pageClick";

    // The jQuery object that represents the pager.
    pagerElem;
    // The number of items per page.
    pageSize;
    // The index of the current page.
    pageIndex = 0;

    constructor(pagerElemID, opt_pageSize) {
        this.pagerElem = $("#" + pagerElemID);
        this.pageSize = opt_pageSize ?? ClientPager.DEFAULT_PAGE_SIZE;
        this._bindEvents();
    }

    // Gets an array of page numbers to display.
    static _getPageNumbers(activePage, pageCount) {
        // [1][.][4][5][6][.][9]
        let leftPart = [];
        let rightPart = [];
        let leftPage = activePage - 1;
        let rightPage = activePage + 1;
        let maxPartsCount = ClientPager.DISPLAY_PAGE_COUNT - 1;

        while (leftPart.length + rightPart.length < maxPartsCount) {
            let pageAdded = false;

            if (leftPage >= 1) {
                leftPart.push(leftPage);
                leftPage--;
                pageAdded = true;
            }

            if (rightPage <= pageCount) {
                rightPart.push(rightPage);
                rightPage++;
                pageAdded = true;
            }

            if (!pageAdded)
                break;
        }

        let pageNumbers = [];
        let pageNumIdx = 0;

        for (let i = leftPart.length - 1; i >= 0; i--)
        {
            pageNumbers[pageNumIdx++] = leftPart[i];
        }

        pageNumbers[pageNumIdx++] = activePage;

        for (let i = 0; i < rightPart.length; i++)
        {
            pageNumbers[pageNumIdx++] = rightPart[i];
        }

        if (pageNumbers[0] != 1 && pageNumbers[0] != activePage) {
            pageNumbers[0] = 1;
            pageNumbers[1] = 0;
        }

        if (pageNumbers.at(-1) != pageCount && pageNumbers.at(-1) != activePage) {
            pageNumbers.at(-1) = pageCount;
            pageNumbers.at(-2) = 0;
        }

        return pageNumbers;
    }

    // Creates a DOM content of the pager.
    static _createDom(pageIndex, pageCount) {
        let listElem = $("<ul class='pagination'></ul>");
        let activePage = pageIndex + 1;
        let pageNumbers = ClientPager._getPageNumbers(activePage, pageCount);

        // previous page
        if (activePage > 1) {
            $(`<li class='page-item'><a class='page-link' href='#' data-page='${activePage - 2}'>&laquo;</a></li>`)
                .appendTo(listElem);
        } else {
            $("<li class='page-item disabled'><span class='page-link'>&laquo;</span></li>")
                .appendTo(listElem);
        }

        // page numbers
        for (let pageNumber of pageNumbers) {
            if (pageNumber <= 0) {
                $("<li class='page-item disabled'><span class='page-link'>...</span></li>")
                    .appendTo(listElem);
            } else if (pageNumber == activePage) {
                $(`<li class='page-item active'><span class='page-link'>${pageNumber}</span></li>`)
                    .appendTo(listElem);
            } else {
                $(`<li class='page-item'><a class='page-link' href='#' data-page='${pageNumber - 1}'>${pageNumber}</a></li>`)
                    .appendTo(listElem);
            }
        }

        // next page
        if (activePage < pageCount) {
            $(`<li class='page-item'><a class='page-link' href='#' data-page='${activePage}'>&raquo;</a></li>`)
                .appendTo(listElem);
        } else {
            $("<li class='page-item disabled'><span class='page-link'>&raquo;</span></li>")
                .appendTo(listElem);
        }

        return listElem;
    }

    // Binds events to the DOM elements.
    _bindEvents() {
        const thisObj = this;
        this.pagerElem
            .on("click", "a.page-link", function () {
                thisObj.pageIndex = parseInt($(this).data("page"));
                thisObj.pagerElem.trigger(ClientPager.PAGE_CLICK_EVENT, thisObj.pageIndex);
                return false;
            });
    }

    // Counts the page count depending on the total number of items.
    _calcPageCount(itemCount) {
        return Math.ceil(itemCount / this.pageSize);
    }

    // Selects items to display on the current page.
    sliceItems(items) {
        let start = this.pageIndex * this.pageSize;
        let end = start + this.pageSize;
        return Array.isArray(items)
            ? items.slice(start, end)
            : items;
    }

    // Updates the pager element.
    update(itemCount) {
        if (this.pageIndex === 0 && itemCount <= this.pageSize) {
            this.pagerElem
                .empty()
                .addClass("hidden");
        } else {
            let pageCount = this._calcPageCount(itemCount);
            this.pagerElem
                .empty()
                .append(ClientPager._createDom(this.pageIndex, pageCount))
                .removeClass("hidden");
        }
    }
}
