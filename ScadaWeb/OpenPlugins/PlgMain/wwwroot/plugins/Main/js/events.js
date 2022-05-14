// Depends on jquery, scada-common.js, view-hub.js, main-api.js, table-common.js

// The variables below are set in Events.cshtml
var phrases = {};

// Specifies the severity levels.
class Severity {
    static MIN = 1;
    static MAX = 999;
    static UNDEFINED = 0;
    static CRITICAL = 1;
    static MAJOR = 250;
    static MINOR = 500;
    static INFO = 750;

    static closest(value) {
        if (Severity.CRITICAL <= value && value < Severity.MAJOR) {
            return Severity.CRITICAL;
        } else if (Severity.MAJOR <= value && value < Severity.MINOR) {
            return Severity.MAJOR;
        } else if (Severity.MINOR <= value && value < Severity.INFO) {
            return Severity.MINOR;
        } else if (Severity.INFO <= value && value < Severity.MAX) {
            return Severity.INFO;
        } else {
            return Severity.UNDEFINED;
        }
    }
}

const ALL_EVENTS_KEY = "Events.AllEvents";
const POSTPONE_SCROLL_PERIOD = 10000; // ms

var allEvents = false;
var filterID = 0;
var arcWriteTime = 0;
var eventBeepTime = "";
var scrollTimeoutID = 0;

function restoreFilter() {
    allEvents = ScadaUtils.getStorageItem(localStorage, ALL_EVENTS_KEY, "false") === "true";

    if (allEvents) {
        $("#spanAllEventsBtn").addClass("selected");
    } else {
        $("#spanEventsByViewBtn").addClass("selected");
    }
}

function initTooltips() {
    new bootstrap.Tooltip(document.getElementById("spanInfoBtn"));
};

function bindEvents() {
    $(window).resize(function () {
        updateLayout();
    });

    $("#spanAllEventsBtn").click(function () {
        // load all events
        $(this).addClass("selected");
        $("#spanEventsByViewBtn").removeClass("selected");

        allEvents = true;
        ScadaUtils.setStorageItem(localStorage, ALL_EVENTS_KEY, "true");
        resetEvents();
    });

    $("#spanEventsByViewBtn").click(function () {
        // load events by view
        $(this).addClass("selected");
        $("#spanAllEventsBtn").removeClass("selected");

        allEvents = false;
        ScadaUtils.setStorageItem(localStorage, ALL_EVENTS_KEY, "false");
        resetEvents();
    });

    $("#spanPrintBtn").click(function () {
        developmentAlert();
    });

    $("#tblEvents").click(function (event) {
        let target = $(event.target);
        if (target.is("td.ack i")) {
            // show event acknowledgement dialog
            let eventID = target.closest(".row-event").attr("data-id");
            viewHub.features.eventAck.show(archiveBit, eventID);
        }
    });

    $("#divTableWrapper").on("mousemove wheel touchstart", function () {
        postponeScroll();
    });
}

function updateLayout() {
    let h = $(window).height() - $("#divToolbar").outerHeight();
    $("#divTableWrapper").outerHeight(h);
    $("#divMessage").outerHeight(h);
};

function startUpdatingEvents() {
    updateEvents(function () {
        setTimeout(startUpdatingEvents, pluginOptions.refreshRate);
    });
}

function updateEvents(callback) {
    if (archiveBit >= 0) {
        mainApi.getArcWriteTime(archiveBit, function (dto) {
            if (dto.ok) {
                let newArcWriteTime = dto.data;

                if (arcWriteTime !== newArcWriteTime) {
                    // request events
                    let retrieveEvents = function (dto) {
                        if (dto.ok) {
                            let oldFilterID = filterID;
                            filterID = dto.data.filterID;

                            if (filterID > 0) {
                                arcWriteTime = newArcWriteTime;
                                showEvents(dto.data, oldFilterID > 0);
                            } else {
                                showErrorBadge();
                            }
                        } else {
                            showErrorBadge();
                        }

                        callback();
                    };

                    if (allEvents) {
                        mainApi.getLastAvailableEvents(archiveBit, pluginOptions.eventDepth,
                            pluginOptions.eventCount, filterID,
                            function (dto) { retrieveEvents(dto); });
                    } else {
                        mainApi.getLastEventsByView(archiveBit, pluginOptions.eventDepth,
                            pluginOptions.eventCount, viewHub.viewID, filterID,
                            function (dto) { retrieveEvents(dto); });
                    }
                } else {
                    // data has not changed
                    callback();
                }
            } else {
                showErrorBadge();
                callback();
            }
        });
    } else {
        console.warn("Unable to request events");
        showErrorBadge();
        callback();
    }
}

function showEvents(data, enableEffects) {
    if (data.records.length > 0) {
        let tbodyElem = $("<tbody></tbody>");
        let newEventBeepTime = eventBeepTime;

        for (let record of data.records) {
            let e = record.e;   // event data
            let ef = record.ef; // formatted event

            let row = $("<tr class='row-event' data-id='" + record.id + "'>" +
                "<td class='time'>" + ef.time + "</td>" +
                "<td class='obj'>" + ef.obj + "</td>" +
                "<td class='dev'>" + ef.dev + "</td>" +
                "<td class='cnl'>" + ef.cnl + "</td>" +
                "<td class='descr'>" + ef.descr + "</td>" +
                "<td class='sev'>" + getSeverityHtml(e.severity, ef.sev) + "</td>" +
                "<td class='ack'>" + getAckHtml(e, ef) + "</td>" +
                "</tr>");

            if (ef.color) {
                row.css("color", ef.color);
            }

            if (ef.beep) {
                newEventBeepTime = e.timestamp;
            }

            tbodyElem.append(row);
        }

        $("#divMessage").addClass("hidden");
        $("#divTableWrapper tbody").remove();
        $("#divTableWrapper table").append(tbodyElem);
        $("#divTableWrapper").removeClass("hidden");

        // scroll down
        if (scrollTimeoutID <= 0) {
            scrollDownEvents(enableEffects);
        }

        // beep
        if (eventBeepTime < newEventBeepTime) {
            eventBeepTime = newEventBeepTime;

            if (enableEffects) {
                ScadaUtils.playSound($("#audEvent"));
            }
        }
    } else {
        $("#divTableWrapper tbody").remove();
        showMessage(phrases.NoEvents);
    }
}

function getSeverityHtml(severityValue, severityText) {
    switch (Severity.closest(severityValue)) {
        case Severity.CRITICAL:
            return `<i class='fas fa-exclamation-circle critical' title='${severityText}'></i>`;

        case Severity.MAJOR:
            return `<i class='fas fa-exclamation-triangle major' title='${severityText}'></i>`;

        case Severity.MINOR:
            return `<i class='fas fa-exclamation-triangle minor' title='${severityText}'></i>`;

        case Severity.INFO:
            return `<i class='fas fa-info info' title='${severityText}'></i>`;

        default:
            return "";
    }
}

function getAckHtml(e, ef) {
    if (e.ack) {
        return `<i class='far fa-check-square ack-yes' title='${ef.ack}'></i>`;
    } else if (e.ackRequired) {
        return `<i class='far fa-square ack-no' title='${phrases.Ack}'></i>`;
    } else {
        return "";
    }
}

function resetEvents() {
    showMessage(phrases.Loading);
    $("#divTableWrapper tbody").remove();

    filterID = 0;
    arcWriteTime = 0;
    eventBeepTime = "";
    clearTimeout(scrollTimeoutID);
    scrollTimeoutID = 0;
}

function showMessage(text) {
    $("#divTableWrapper").addClass("hidden");
    $("#divMessage").text(text).removeClass("hidden");
}

function postponeScroll() {
    clearTimeout(scrollTimeoutID);

    scrollTimeoutID = setTimeout(function () {
        scrollDownEvents(true);
        scrollTimeoutID = 0;
    }, POSTPONE_SCROLL_PERIOD);
}

function scrollDownEvents(animate) {
    let divTableWrapper = $("#divTableWrapper");
    let scrollHeight = divTableWrapper[0].scrollHeight;
    let newScrollTop = scrollHeight - divTableWrapper.innerHeight();

    if (divTableWrapper.scrollTop() < newScrollTop) {
        if (animate) {
            divTableWrapper.animate({ scrollTop: newScrollTop }, "slow");
        } else {
            divTableWrapper.scrollTop(newScrollTop);
        }
    }
}

$(document).ready(function () {
    mainApi.rootPath = viewHub.appEnv.rootPath;
    restoreFilter();
    initTooltips();
    bindEvents();
    updateLayout();
    showMessage(phrases.Loading);
    startUpdatingEvents();
});
