// Depends on jquery, scada-common.js

var viewPage = {
    _bindEvents: function () {
        let thisObj = this;

        $(window).on(ScadaEventTypes.UPDATE_LAYOUT, function () {
            thisObj.updateLayout();
        });
    },

    _getOuterHeight: function (elemID) {
        let jqObj = $("#" + elemID);
        return jqObj.hasClass("hidden") || jqObj.css("display") === "none"
            ? 0
            : jqObj.outerHeight();
    },

    prepare: function () {
        let splitter = new Splitter("divViewSplitter");

        splitter.exitResizeModeCallbacks.add(function () {
            console.log("exitResizeModeCallback");
        });

        this._bindEvents();
    },

    updateLayout: function () {
        let divView = $("#divView");
        let totalHeight = divView.parent().innerHeight();
        divView.outerHeight(totalHeight - this._getOuterHeight("divViewSplitter") -
            this._getOuterHeight("divDataWindow") - this._getOuterHeight("divBottomPanel"));
    },

    loadView(viewID, viewFrameUrl) {
        console.log("loadView viewID = " + viewID)
    }
};

$(document).ready(function () {
    viewPage.prepare();
    viewPage.updateLayout();
});
