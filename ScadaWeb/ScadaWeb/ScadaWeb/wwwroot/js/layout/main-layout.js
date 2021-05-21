// Depends on tree-view.js
var mainLayout = {
    // Prepares the main menu and view explorer tree views.
    prepareTreeViews: function () {
        let mainMenu = new TreeView("Main_divMainMenu");
        let viewExplorer = new TreeView("Main_divViewExplorer");
        mainMenu.prepare();
        viewExplorer.prepare();
    },

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

    // Shows the left panel.
    showLeftPanel: function () {
        $("body").addClass("left-panel-visible");
    },

    // Hides the left panel.
    hideLeftPanel: function () {
        $("body").removeClass("left-panel-visible");
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

    // Enables the full screen mode.
    enableFullscreen: function () {
        $("body").removeClass("header-visible");
        this.updateLayout();
    },

    // Exits the full screen mode.
    exitFullscreen: function () {
        $("body").addClass("header-visible");
        this.updateLayout();
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

        // toggle the left panel
        $("#Main_spanMenuBtn, #Main_spanMenuBtn2")
            .off()
            .click(function () {
                if ($("body").hasClass("left-panel-visible")) {
                    thisObj.hideLeftPanel();
                } else {
                    thisObj.showLeftPanel();
                }
            });

        // enable full screen
        $("#Main_spanFullscreenBtn")
            .off()
            .click(function () {
                thisObj.enableFullscreen();
            });

        // exit full screen
        $("#Main_spanExitFullscreenBtn")
            .off()
            .click(function () {
                thisObj.exitFullscreen();
            });
    }
};

$(document).ready(function () {
    if ($("#Main_divHeader").length > 0) {
        $("body").addClass("header-visible");
    }

    mainLayout.updateLayout();
    mainLayout.prepareTreeViews();
    mainLayout.showLeftPanel();

    mainLayout.activateTab($("#Main_divMainMenuTab"));
    mainLayout.bindEvents();
});
