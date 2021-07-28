// Depends on jquery, scada-common.js, view-hub.js, main-api.js, table-common.js

// The variables below are set from Events.cshtml
var phrases = {};

var filterID = 0;
var arcWriteTime = 0;

function bindEvents() {
    $(window).resize(function () {
        updateLayout();
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
                    mainApi.getLastEventsByView(archiveBit, pluginOptions.eventDepth,
                        pluginOptions.eventCount, viewHub.viewID, filterID, function (dto) {

                            if (dto.ok) {
                                filterID = dto.data.filterID;
                                arcWriteTime = newArcWriteTime;
                                showEvents(dto.data);
                            } else {
                                showErrorBadge();
                            }

                            callback();
                        });
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

function showEvents(data) {
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
    } else {
        $("#divTableWrapper tbody").remove();
        showMessage(phrases.NoEvents);
    }
}

function showMessage(text) {
    $("#divTableWrapper").addClass("hidden");
    $("#divMessage").text(text).removeClass("hidden");
}

$(document).ready(function () {
    bindEvents();
    updateLayout();
    showMessage(phrases.Loading);
    startUpdatingEvents();
});
