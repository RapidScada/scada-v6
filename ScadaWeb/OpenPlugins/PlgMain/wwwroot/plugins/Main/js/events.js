// Depends on jquery, scada-common.js, view-hub.js, main-api.js, table-common.js

// The variables below are set from Events.cshtml
var phrases = {};

const ALL_EVENTS_KEY = "Events.AllEvents";
const POSTPONE_SCROLL_PERIOD = 10000; // ms

var filterID = 0;
var arcWriteTime = 0;
var allEvents = false;
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

    // load all events
    $("#spanAllEventsBtn").click(function () {
        $(this).addClass("selected");
        $("#spanEventsByViewBtn").removeClass("selected");

        allEvents = true;
        ScadaUtils.setStorageItem(localStorage, ALL_EVENTS_KEY, "true");
        resetEvents();
    });

    // load events by view
    $("#spanEventsByViewBtn").click(function () {
        $(this).addClass("selected");
        $("#spanAllEventsBtn").removeClass("selected");

        allEvents = false;
        ScadaUtils.setStorageItem(localStorage, ALL_EVENTS_KEY, "false");
        resetEvents();
    });

    $("#spanPrintBtn").click(function () {
        developmentAlert();
    });

    // postpone scroll
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
                            arcWriteTime = newArcWriteTime;
                            showEvents(dto.data, filterID > 0);
                            filterID = dto.data.filterID;
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

function showEvents(data, animateScroll) {
    if (data.records.length > 0) {
        let tbodyElem = $("<tbody></tbody>");

        for (let record of data.records) {
            let e = record.e;   // event data
            let ef = record.ef; // formatted event

            let row = $("<tr>" +
                "<td class='time'>" + ef.time + "</td>" +
                "<td class='obj'>" + ef.obj + "</td>" +
                "<td class='dev'>" + ef.dev + "</td>" +
                "<td class='cnl'>" + ef.cnl + "</td>" +
                "<td class='descr'>" + ef.descr + "</td>" +
                "<td class='sev'>" + e.severity + "</td>" +
                "<td class='ack'>" + ef.ack + "</td>" +
                "</tr>");

            if (ef.Color) {
                row.css("color", ef.Color);
            }

            tbodyElem.append(row);
        }

        $("#divMessage").addClass("hidden");
        $("#divTableWrapper tbody").remove();
        $("#divTableWrapper table").append(tbodyElem);
        $("#divTableWrapper").removeClass("hidden");

        // scroll down
        if (scrollTimeoutID <= 0) {
            scrollDownEvents(animateScroll);
        }
    } else {
        $("#divTableWrapper tbody").remove();
        showMessage(phrases.NoEvents);
    }
}

function resetEvents() {
    showMessage(phrases.Loading);
    $("#divTableWrapper tbody").remove();

    filterID = 0;
    arcWriteTime = 0;
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
    restoreFilter();
    initTooltips();
    bindEvents();
    updateLayout();
    showMessage(phrases.Loading);
    startUpdatingEvents();
});
