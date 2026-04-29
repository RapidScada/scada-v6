// Depends on jquery, bootstrap, scada-common.js, modal.js, notif-panel.js, tree-view.js

// Contains environment variables.
var appEnv = appEnv ?? appEnvStub;
// Manages modal dialogs.
var modalManager = new ModalManager();
// Contains references to widely used objects.
var mainObj = {
    appEnv: appEnv,
    modalManager: modalManager,
    features: new PluginFeatures(appEnv)
};

// Manages the layout page.
var mainLayout = {
    // The storage key for the left panel visibility.
    _LEFT_PANEL_VISIBLE_KEY: "MainLayout.LeftPanelVisible",

    // The components of the main layout.
    mainMenu: null,
    viewExplorer: null,
    notifPanel: null,

    // The jQuery objects that represent tabs.
    tabs: {
        default: $(),
        mainMenu: $(),
        viewExplorer: $()
    },

    // Prepares the main menu and view explorer tree views.
    _prepareTreeViews: function () {
        // main menu
        this.mainMenu = new TreeView("Main_divMainMenu");

        if (this.mainMenu.treeViewElem.length > 0) {
            this.mainMenu.prepare();
        }

        // view explorer
        this.viewExplorer = new TreeView("Main_divViewExplorer");
        const thisObj = this;

        if (this.viewExplorer.treeViewElem.length > 0) {
            this.viewExplorer.nodeClickCallbacks.add(function (node, result) {
                if (typeof viewPage !== "undefined") {
                    let viewID = parseInt(node.data("viewid"));
                    let viewFrameUrl = node.data("viewframeurl");
                    let viewPageUrl = node.attr("href");

                    if (viewID > 0 && viewFrameUrl && viewPageUrl) {
                        // hide the left panel on mobile devices
                        if (ScadaUtils.isSmallScreen) {
                            thisObj._hideLeftPanel(false);
                        }

                        // load view frame
                        viewPage.loadView(viewID, viewFrameUrl, viewPageUrl);
                        result.handled = true;
                    }
                }
            });

            this.viewExplorer.prepare();
        }
    },

    // Prepares the notification panel.
    _prepareNotifPanel: function () {
        if ($("#Main_divNotifPanel").length > 0) {
            this.notifPanel = new NotifPanel("Main_divNotifPanel", "Main_spanNotifBtn", "Main_spanNotifBtn2");
            this.notifPanel.prepare(appEnv.rootPath);
            //this.notifPanel.addSamples();
        }
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
        this.tabs.viewExplorer = $("#Main_divViewExplorerTab");
    },

    // Selects the active tab depending on the page.
    _selectActiveTab: function () {
        let selectedTab = typeof viewPage === "undefined"
            ? this.tabs.mainMenu
            : this.tabs.viewExplorer;

        this._activateTab(selectedTab.length > 0 ? selectedTab : this.tabs.default);
    },

    // Binds events to the DOM elements.
    _bindEvents: function () {
        const thisObj = this;

        // update layout on window resize
        $(window).on("resize", function () {
            thisObj.updateLayout();
        });

        // activate a clicked tab
        $("#Main_divTabPanel .tab")
            .off()
            .on("click", function () {
                thisObj._activateTab($(this));
            });

        // toggle the left panel
        $("#Main_spanMenuBtn, #Main_spanMenuBtn2")
            .off()
            .on("click", function () {
                if ($("body").hasClass("left-panel-visible")) {
                    thisObj._hideLeftPanel(true);
                } else {
                    thisObj._showLeftPanel(true);
                }
            });

        // enter full screen
        $("#Main_spanFullscreenBtn, #Main_spanFullscreenBtn2")
            .off()
            .on("click", function () {
                thisObj._enterFullscreen();
            });

        // exit full screen
        $("#Main_spanExitFullscreenBtn")
            .off()
            .on("click", function () {
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

    // Activates the selected tab and shows corresponding tool window.
    _activateTab: function (selectedTab) {
        if (selectedTab.length > 0) {
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
        }
    },

    // Restores the saved state of the left panel.
    _restoreLeftPanel: function () {
        // keep the left panel hidden on mobile devices
        if (!ScadaUtils.isSmallScreen &&
            ScadaUtils.getStorageItem(localStorage, this._LEFT_PANEL_VISIBLE_KEY, "true") === "true") {
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
        this._selectActiveTab();
        this._bindEvents();
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
        $(window).trigger(ScadaEventType.UPDATE_LAYOUT);
    },

    // Selects the specified view in the view explorer.
    selectView: function (viewID) {
        this._activateTab(this.tabs.viewExplorer);
        this.viewExplorer.selectNode(this.viewExplorer.treeViewElem.find(".node[data-viewid=" + viewID + "]"));
    }
};

$(() => {
    mainLayout.prepare();
    mainLayout.updateLayout();
});
