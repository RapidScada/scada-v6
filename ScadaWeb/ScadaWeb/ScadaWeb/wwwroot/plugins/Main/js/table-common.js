// The variables below are set in *.cshtml
var archiveBit = -1;
var pluginOptions = {
    refreshRate: 1000,
    eventCount: 100,
    eventDepth: 2
};

var viewHub = ViewHub.getInstance();
var mainApi = new MainApi();
var errorTimeoutID = 0;

function showErrorBadge() {
    clearTimeout(errorTimeoutID);
    $("#spanErrorBadge").removeClass("hidden");

    errorTimeoutID = setTimeout(function () {
        $("#spanErrorBadge").addClass("hidden");
        errorTimeoutID = 0;
    }, ScadaUtils.ERROR_DISPLAY_DURATION);
}
