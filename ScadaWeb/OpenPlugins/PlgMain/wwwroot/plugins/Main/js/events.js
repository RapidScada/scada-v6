function updateLayout() {
    let h = $(window).height() - $("#divToolbar").outerHeight();
    $("#divTableWrapper").outerHeight(h);
    $("#divLoading").outerHeight(h);
    $("#divNoEvents").outerHeight(h);
};

$(document).ready(function () {
    updateLayout();
});
