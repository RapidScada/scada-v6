$(document).ready(function () {
    // set event color
    let eventColor = $("#tblEvent").data("color");

    if (eventColor) {
        $("#tblEvent td").css("color", eventColor);
    }
});
