var mainLayout = {
    // Updates the layout to fit the window.
    updateLayout: function () {
        let divHeader = $("#Main_divHeader");
        let headerHeight = divHeader.length > 0 && $("body").hasClass("header-visible")
            ? divHeader.outerHeight()
            : 0;

        let contentHeight = $(window).height() - headerHeight;
        $("#Main_divLeftPanel").outerHeight(contentHeight);
        $("#Main_divContent").outerHeight(contentHeight);
        $("#Main_divNotifPanel").outerHeight(contentHeight);
        //$(window).trigger(scada.EventTypes.UPDATE_LAYOUT);
    },
};

$(document).ready(function () {
    if ($("#Main_divHeader").length > 0) {
        $("body").addClass("header-visible");
    }

    $("body").addClass("left-panel-visible");
    mainLayout.updateLayout();

    // update layout on window resize
    $(window).resize(function () {
        mainLayout.updateLayout();
    });
});
