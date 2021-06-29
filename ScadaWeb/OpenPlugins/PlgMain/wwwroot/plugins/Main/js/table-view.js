function updateLayout() {
    let h = $(window).height() - $("#divToolbar").outerHeight();
    $("#divTableWrapper").outerHeight(h);
    $("#divError").outerHeight(h);
};

$(document).ready(function () {
    updateLayout();
});
