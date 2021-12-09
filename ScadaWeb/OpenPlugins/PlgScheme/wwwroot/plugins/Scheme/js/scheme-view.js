// Displays a scheme.
// Depends on jquery, bootstrap, scada-common.js, main-api.js, view-hub.js

// Namespaces
var scada = scada || {};
scada.scheme = scada.scheme || {};

// Used for testing
const DEBUG_MODE = false;
// How long to show the error badge
const ERROR_DISPLAY_DURATION = 10000; // ms

// Scheme object
var scheme = null;
// Possible scale values
var scaleVals = [0.1, 0.25, 0.5, 0.75, 1, 1.25, 1.5, 2, 2.5, 3, 4, 5];
// View hub object
var viewHub = ViewHub.getInstance();
// Provides access to current data
var mainApi = new MainApi();
// Identifies the error badge display timeout
var errorTimeoutID = 0;

// The variables below are set from SchemeView.cshtml
// View ID
var viewID = 0;
// Scheme refresh rate
var refrRate = 1000;
// View control right
var controlRight = false;
// Scheme options
var schemeOptions = scada.scheme.defaultOptions;

// Scheme environment object accessible by the scheme and its components
scada.scheme.env = {
    // View hub object
    viewHub: viewHub,

    // Send telecommand
    sendCommand: function (ctrlCnlNum, cmdVal, viewID, componentID) {
        scheme.sendCommand(ctrlCnlNum, cmdVal, viewID, componentID, function (success) {
            if (!success) {
                console.error("Unable to send command");
                showErrorBadge();
            }
        });
    }
};

// Load the scheme
function loadScheme(viewID) {
    scheme.load(viewID, function (success) {
        if (success) {
            if (!DEBUG_MODE) {
                // show errors
                if (Array.isArray(scheme.loadErrors) && scheme.loadErrors.length > 0) {
                    for (let err of scheme.loadErrors) {
                        console.error(err);
                        addNotification(err, true);
                    }

                    showErrorBadge();
                }

                // show scheme
                scheme.createDom(controlRight);
                loadScale();
                displayScale();
                startUpdatingScheme();
            }
        } else {
            console.error("Scheme loading failed");
            showErrorBadge(true);
        }
    });
}

// Reload the scheme
function reloadScheme() {
    location.reload(true);
}

// Start cyclic scheme updating process
function startUpdatingScheme() {
    scheme.updateData(mainApi, function (success) {
        if (!success) {
            console.error("Error updating scheme data");
            showErrorBadge();
        }

        setTimeout(startUpdatingScheme, refrRate);
    });
}

// Bind handlers of the toolbar buttons
function initToolbar() {
    var ScaleTypes = scada.scheme.ScaleTypes;

    $("#spanFitScreenBtn").click(function () {
        scheme.setScale(ScaleTypes.FIT_SCREEN, 0);
        displayScale();
        saveScale();
    });

    $("#spanFitWidthBtn").click(function () {
        scheme.setScale(ScaleTypes.FIT_WIDTH, 0);
        displayScale();
        saveScale();
    });

    $("#spanZoomInBtn").click(function (event) {
        scheme.setScale(ScaleTypes.NUMERIC, getNextScale());
        displayScale();
        saveScale();
    });

    $("#spanZoomOutBtn").click(function (event) {
        scheme.setScale(ScaleTypes.NUMERIC, getPrevScale());
        displayScale();
        saveScale();
    });
}

// Get the previous scale value from the possible values array
function getPrevScale() {
    var curScale = scheme.scaleValue;
    for (var i = scaleVals.length - 1; i >= 0; i--) {
        var prevScale = scaleVals[i];
        if (curScale > prevScale) {
            return prevScale;
        }
    }
    return curScale;
}

// Get the next scale value from the possible values array
function getNextScale() {
    var curScale = scheme.scaleValue;
    for (var i = 0, len = scaleVals.length; i < len; i++) {
        var nextScale = scaleVals[i];
        if (curScale < nextScale) {
            return nextScale;
        }
    }
    return curScale;
}

// Display the scheme scale
function displayScale() {
    $("#spanCurScale").text(Math.round(scheme.scaleValue * 100) + "%");
}

// Load the scheme scale from the local storage
function loadScale() {
    var scaleType = NaN;
    var scaleValue = NaN;

    if (schemeOptions.rememberScale) {
        scaleType = parseInt(localStorage.getItem("Scheme.ScaleType"));
        scaleValue = parseFloat(localStorage.getItem("Scheme.ScaleValue"));
    }

    if (isNaN(scaleType) || isNaN(scaleValue)) {
        scheme.setScale(schemeOptions.scaleType, schemeOptions.scaleValue);
    } else {
        scheme.setScale(scaleType, scaleValue);
    }
}

// Save the scheme scale in the local storage
function saveScale() {
    localStorage.setItem("Scheme.ScaleType", scheme.scaleType);
    localStorage.setItem("Scheme.ScaleValue", scheme.scaleValue);
}

// Update the scheme scale if the scheme should fit size
function updateScale() {
    if (scheme.scaleType !== scada.scheme.ScaleTypes.NUMERIC) {
        scheme.setScale(scheme.scaleType, scheme.scaleValue);
        displayScale();
    }
}

// Update layout of the top level div elements
function updateLayout() {
    let divNotif = $("#divNotif");
    let divSchWrapper = $("#divSchWrapper");
    let divToolbar = $("#divToolbar");
    let notifHeight = divNotif.css("display") === "block" ? divNotif.outerHeight() : 0;
    let windowWidth = $(window).width();

    $("body").css("padding-top", notifHeight);
    divNotif.outerWidth(windowWidth);
    divSchWrapper
        .outerWidth(windowWidth)
        .outerHeight($(window).height() - notifHeight);
    divToolbar.css("top", notifHeight);
}

// Show error badge for a period of time or permanently
function showErrorBadge(opt_permanent) {
    clearTimeout(errorTimeoutID);
    let spanErrorBadge = $("#spanErrorBadge");
    spanErrorBadge.removeClass("hidden");

    if (opt_permanent) {
        spanErrorBadge.addClass("permanent");
    } else if (!spanErrorBadge.hasClass("permanent")) {
        errorTimeoutID = setTimeout(function () {
            $("#spanErrorBadge").addClass("hidden");
            errorTimeoutID = 0;
        }, ERROR_DISPLAY_DURATION);
    }
}

// Add a notification message
function addNotification(messageHtml, isError) {
    let divMessage = $("<div class='message'></div>").html(messageHtml);

    if (isError) {
        divMessage.addClass("error");
    }

    let divNotif = $("#divNotif");
    divNotif
        .removeClass("hidden")
        .append(divMessage)
        .scrollTop(divNotif.prop("scrollHeight"));

    $(window).trigger(ScadaEventType.UPDATE_LAYOUT);
}

// Initialize debug tools
function initDebugTools() {
    $("#divDebugTools").css("display", "inline-block");

    $("#spanLoadSchemeBtn").click(function () {
        loadScheme(viewID);
    });

    $("#spanCreateDomBtn").click(function () {
        scheme.createDom();
    });

    $("#spanStartUpdBtn").click(function () {
        startUpdatingScheme();
        $(this).prop("disabled", true);
    });

    $("#spanAddNotifBtn").click(function () {
        addNotification("Test notification", true);
    });
}

$(document).ready(function () {
    // setup client API
    mainApi.rootPath = viewHub.appEnv.rootPath;

    // create scheme
    var divSchWrapper = $("#divSchWrapper");
    scheme = new scada.scheme.Scheme();
    scheme.schemeEnv = scada.scheme.env;
    scheme.parentDomElem = divSchWrapper;

    // setup user interface
    initToolbar();
    //scada.utils.styleIOS(divSchWrapper);
    updateLayout();

    $(window).on("resize " + ScadaEventType.UPDATE_LAYOUT, function () {
        updateLayout();
        updateScale();
    });

    if (DEBUG_MODE) {
        initDebugTools();
    } else {
        loadScheme(viewID);
    }
});
