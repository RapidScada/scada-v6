$(document).ready(function () {
    // scroll to the active item of the table of contents
    let tocWrapper = $("#divTocWrapper");
    let activeItem = tocWrapper.find(".item-text.active:first");

    if (activeItem.length > 0) {
        //tocWrapper.scrollTop(activeItem.offset().top);
        //activeItem[0].scrollIntoView();
    }
});
