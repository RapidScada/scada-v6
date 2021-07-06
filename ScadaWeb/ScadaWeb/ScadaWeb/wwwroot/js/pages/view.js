// Depends on jquery, scada-common.js, view-hub.js, main-layout.js

var viewHub = new ViewHub(appEnv);

var viewPage = {
    _DATA_WINDOW_VISIBLE_KEY: "View.DataWindowVisible",
    _DATA_WINDOW_HEIGHT_KEY: "View.DataWindowHeight",
    _DATA_WINDOW_URL_KEY: "View.DataWindowUrl",
    _DEFAULT_DATA_WINDOW_HEIGHT: 100,

    _initialPageTitle: document.title,
    initialViewID: 0,
    initialViewFrameUrl: "",

    _bindEvents: function () {
        let thisObj = this;

        $("#divBottomPanel .bottom-pnl-tab")
            .off()
            .click(function () {
                thisObj._showDataWindow($(this), true);
            });

        $("#divHideDataWindowBtn")
            .off()
            .click(function () {
                thisObj._hideDataWindow(true);
            });

        $(window)
            .on(ScadaEventTypes.UPDATE_LAYOUT, function () {
                thisObj.updateLayout();
            })
            .on("popstate", function (event) {
                // load view from history
                let historyState = event.originalEvent.state;

                if (historyState) {
                    thisObj.loadView(historyState.viewID, historyState.viewFrameUrl, true);
                    mainLayout.selectView(historyState.viewID);
                } else {
                    thisObj.loadView(thisObj.initialViewID, thisObj.initialViewFrameUrl, true);
                    mainLayout.selectView(thisObj.initialViewID);
                }
            });
    },

    _getOuterHeight: function (elemID) {
        let jqObj = $("#" + elemID);
        return jqObj.hasClass("hidden") || jqObj.css("display") === "none"
            ? 0
            : jqObj.outerHeight();
    },

    _hideDataWindow: function (saveState) {
        $("#divViewSplitter").addClass("hidden");
        $("#divDataWindow").addClass("hidden");
        $("#divHideDataWindowBtn").addClass("hidden");
        $("#frameDataWindow").attr("src", "");
        $("#divBottomPanel .bottom-pnl-tab").removeClass("selected");
        this.updateLayout();

        if (saveState) {
            ScadaUtils.setStorageItem(localStorage, this._DATA_WINDOW_VISIBLE_KEY, false);
        }
    },

    _showDataWindow: function (selectedTab, saveState) {
        $("#divViewSplitter").removeClass("hidden");
        $("#divDataWindow").removeClass("hidden");
        $("#divHideDataWindowBtn").removeClass("hidden");
        selectedTab.addClass("selected");
        this.updateLayout();

        let url = selectedTab.data("url");
        ScadaUtils.replaceFrame($("#frameDataWindow"), url);

        if (saveState) {
            ScadaUtils.setStorageItem(localStorage, this._DATA_WINDOW_VISIBLE_KEY, true);
            ScadaUtils.setStorageItem(localStorage, this._DATA_WINDOW_URL_KEY, url);
        }
    },

    _showDataWindowByUrl(url) {
        let thisObj = this;
        let found = false;

        if (url) {
            $("#divBottomPanel .bottom-pnl-tab").each(function () {
                let tabUrl = $(this).data("url");

                if (url === tabUrl) {
                    thisObj._showDataWindow($(this));
                    found = true;
                    return false;
                }
            });
        }

        if (!found) {
            this._showDataWindow($("#divBottomPanel .bottom-pnl-tab:first"));
        }
    },

    _reloadDataWindow() {
        if (!$("#divDataWindow").hasClass("hidden")) {
            ScadaUtils.replaceFrame($("#frameDataWindow"));
        }
    },

    prepare: function () {
        if ($("#divBottomPanel .bottom-pnl-tab").length > 0) {
            // create splitter
            let thisObj = this;
            let splitter = new Splitter("divViewSplitter");

            splitter.exitResizeModeCallbacks.add(function () {
                ScadaUtils.setStorageItem(localStorage, thisObj._DATA_WINDOW_HEIGHT_KEY,
                    $("#divDataWindow").outerHeight());
            });

            // show data window
            $("#divDataWindow").outerHeight(ScadaUtils.getStorageItem(localStorage,
                this._DATA_WINDOW_HEIGHT_KEY, this._DEFAULT_DATA_WINDOW_HEIGHT))

            if (ScadaUtils.getStorageItem(localStorage, this._DATA_WINDOW_VISIBLE_KEY,
                ScadaUtils.isSmallScreen ? "false" : "true") === "true") {
                viewHub.viewID = thisObj.initialViewID;
                this._showDataWindowByUrl(ScadaUtils.getStorageItem(localStorage, this._DATA_WINDOW_URL_KEY, ""));
            } else {
                this._hideDataWindow(false);
            }
        } else {
            // remove unnecessary components
            $("#divViewSplitter").remove();
            $("#divDataWindow").remove();
            $("#divBottomPanel").remove();
        }

        this._bindEvents();
    },

    updateLayout: function () {
        let divView = $("#divView");
        let totalHeight = divView.parent().innerHeight();
        divView.outerHeight(totalHeight - this._getOuterHeight("divViewSplitter") -
            this._getOuterHeight("divDataWindow") - this._getOuterHeight("divBottomPanel"));
    },

    loadView(viewID, viewFrameUrl, opt_notWriteHistory, opt_notReloadDataWindow) {
        console.log(`${ScadaUtils.getCurrentTime()} Load view ${viewID} from ${viewFrameUrl}`);
        viewHub.viewID = viewID;

        // write history
        let historyState = null;
        let historyUrl = "";

        if (!opt_notWriteHistory) {
            historyState = { viewID: viewID, viewFrameUrl: viewFrameUrl };
            historyUrl = viewHub.getViewUrl(viewID);
            history.pushState(historyState, "", historyUrl);
        }

        // load view
        document.title = this._initialPageTitle;
        let frameView = ScadaUtils.replaceFrame($("#frameView"), viewFrameUrl);

        frameView.on("load", function () {
            var frameWnd = frameView[0].contentWindow;

            if (ScadaUtils.checkAccessToFrame(frameWnd)) {
                // update page title
                document.title = frameWnd.document.title;

                // update history because view frame might be redirected
                if (historyState !== null) {
                    historyState.viewFrameUrl = frameWnd.location.href;
                    history.replaceState(historyState, "", historyUrl);
                }
            }
        });

        // reload data window
        if (!opt_notReloadDataWindow) {
            this._reloadDataWindow();
        }
    }
};

$(document).ready(function () {
    viewPage.prepare();
    viewPage.updateLayout();
    viewPage.loadView(viewPage.initialViewID, viewPage.initialViewFrameUrl, true, true);
});
