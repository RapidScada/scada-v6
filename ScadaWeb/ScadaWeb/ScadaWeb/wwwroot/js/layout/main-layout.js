var mainLayout = {
    // Updates the layout to fit the window.
    updateLayout: function () {
        let divHeader = $("#Main_divHeader");
        let headerHeight = divHeader.length > 0 && $("body").hasClass("header-visible")
            ? divHeader.outerHeight()
            : 0;

        let contentHeight = $(window).height() - headerHeight;
        $("#Main_divLeftPanel").outerHeight(contentHeight);
        $("#Main_divTabPanel").outerWidth(contentHeight);
        $("#Main_divContent").outerHeight(contentHeight);
        $("#Main_divNotifPanel").outerHeight(contentHeight);
        //$(window).trigger(scada.EventTypes.UPDATE_LAYOUT);
    },

    // Activates the selected tab and shows corresponding tool window.
    activateTab: function (selectedTab) {
        // highlight clicked tab
        let tabs = selectedTab.siblings(".tab");
        tabs.removeClass("selected");
        selectedTab.addClass("selected");

        // deactivate all tool windows
        var toolWindows = $("#Main_divLeftPanel .tool-window");
        toolWindows.addClass("hidden");

        // activate corresponding tool window
        var toolWindowId = selectedTab.data("tool-window");
        $("#" + toolWindowId).removeClass("hidden");
    },

    bindEvents: function () {
        let thisObj = this;

        // update layout on window resize
        $(window).resize(function () {
            mainLayout.updateLayout();
        });

        // activate a clicked tab
        $("#Main_divTabPanel .tab")
            .off()
            .click(function () {
                thisObj.activateTab($(this));
            });
    }
};

$(document).ready(function () {
    if ($("#Main_divHeader").length > 0) {
        $("body").addClass("header-visible");
    }

    $("body").addClass("left-panel-visible");
    mainLayout.updateLayout();

    mainLayout.activateTab($("#Main_divMainMenuTab"));
    mainLayout.bindEvents();
});
