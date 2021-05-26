// Depends on scada-common.js, tree-view.js

var appEnv = {
    rootPath: "/",
    locale: "en-GB",
    productName: "Rapid SCADA"
};

var mainLayout = {
    // The storage key for the left panel visibility.
    _LEFT_PANEL_VISIBLE_KEY: "MainLayout.LeftPanelVisible",

    // The notification panel.
    notifPanel: null,

    // The jQuery objects that represent tabs.
    tabs: {
        default: $(),
        mainMenu: $(),
        explorerView: $()
    },

    // Prepares the main menu and view explorer tree views.
    _prepareTreeViews: function () {
        let mainMenu = new TreeView("Main_divMainMenu");
        let viewExplorer = new TreeView("Main_divViewExplorer");
        mainMenu.prepare();
        viewExplorer.prepare();
    },

    // Prepares the notification panel.
    _prepareNotifPanel: function () {
        this.notifPanel = new NotifPanel("Main_divNotifPanel", "Main_spanNotifBtn", "Main_spanNotifBtn2");
        this.notifPanel.prepare(appEnv.rootPath);
        this.notifPanel.addSamples();
    },

    // Hides unused buttons.
    _prepareButtons: function () {
        if ($("#Main_divLeftPanel").length == 0) {
            $("#Main_spanMenuBtn, #Main_spanMenuBtn2").remove();
        }

        if ($("#Main_divMainMenu").length == 0) {
            $("#Main_divMainMenuTab").remove();
        }

        if ($("#Main_divViewExplorer").length == 0) {
            $("#Main_divViewExplorerTab").remove();
        }

        if ($("#Main_divNotifPanel").length == 0) {
            $("#Main_spanNotifBtn, #Main_spanNotifBtn2").remove();
        }

        this.tabs.default = $("#Main_divTabPanel .tab:first");
        this.tabs.mainMenu = $("#Main_divMainMenuTab");
        this.tabs.explorerView = $("#Main_divViewExplorerTab");
    },

    // Binds events to the DOM elements.
    _bindEvents: function () {
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
                    thisObj._hideLeftPanel(true);
                } else {
                    thisObj._showLeftPanel(true);
                }
            });

        // enter full screen
        $("#Main_spanFullscreenBtn, #Main_spanFullscreenBtn2")
            .off()
            .click(function () {
                thisObj._enterFullscreen();
            });

        // exit full screen
        $("#Main_spanExitFullscreenBtn")
            .off()
            .click(function () {
                thisObj._exitFullscreen();
            });

        // in Chrome fires only if the mode is switched programmatically
        $(document).on("fullscreenchange webkitfullscreenchange mozfullscreenchange MSFullscreenChange", function () {
            console.info("Full screen is " + ScadaUtils.isFullscreen);
        });
    },

    // Shows the header.
    _showHeader: function () {
        if ($("#Main_divHeader").length > 0) {
            $("body").addClass("header-visible");
        }
    },

    // Hides the header.
    _hideHeader: function () {
        $("body").removeClass("header-visible");
    },

    // Shows the left panel.
    _showLeftPanel: function (saveState) {
        if ($("#Main_divLeftPanel").length > 0) {
            $("body").addClass("left-panel-visible");

            if (saveState) {
                ScadaUtils.setStorageItem(localStorage, this._LEFT_PANEL_VISIBLE_KEY, "true");
            }
        }
    },

    // Hides the left panel.
    _hideLeftPanel: function (saveState) {
        $("body").removeClass("left-panel-visible");

        if (saveState) {
            ScadaUtils.setStorageItem(localStorage, this._LEFT_PANEL_VISIBLE_KEY, "false");
        }
    },

    // Restores the saved state of the left panel.
    _restoreLeftPanel: function () {
        if (ScadaUtils.getStorageItem(localStorage, this._LEFT_PANEL_VISIBLE_KEY,
            ScadaUtils.isSmallScreen ? "false" : "true") === "true") {
            this._showLeftPanel(false);
        }
    },

    // Enters full screen mode.
    _enterFullscreen: function () {
        if (!ScadaUtils.isFullscreen) {
            ScadaUtils.requestFullscreen();
        }

        $("body").addClass("full-screen");
        this._hideHeader();
        this._hideLeftPanel(false);
        this.updateLayout();
    },

    // Exits full screen mode.
    _exitFullscreen: function () {
        if (ScadaUtils.isFullscreen) {
            ScadaUtils.exitFullscreen();
        }

        $("body").removeClass("full-screen");
        this._showHeader();
        this._restoreLeftPanel();
        this.updateLayout();
    },

    // Prepares the layout for work.
    prepare: function () {
        this._showHeader();
        this._restoreLeftPanel();
        this._prepareTreeViews();
        this._prepareNotifPanel();
        this._prepareButtons();
        this._bindEvents();
        this.activateTab(this.tabs.default);
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
        $(window).trigger(ScadaEventTypes.UPDATE_LAYOUT);
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
};

$(document).ready(function () {
    mainLayout.prepare();
    mainLayout.updateLayout();
});
