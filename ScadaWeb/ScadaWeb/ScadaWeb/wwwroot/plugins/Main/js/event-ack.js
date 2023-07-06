// The variables below are in from EventAck.cshtml
var ackAllowed = false;
var closeModal = false;

const CLOSE_TIMEOUT = 1000;

$(document).ready(function () {
    // set event color
    let eventColor = $("#tblEvent").data("color");

    if (eventColor) {
        $("#tblEvent td").css("color", eventColor);
    }

    // hide OK button
    let modalManager = ModalManager.getInstance();

    if (!ackAllowed) {
        modalManager.setButtonVisible(window, ModalButton.OK, false);
    }

    // close modal
    if (closeModal) {
        setTimeout(function () {
            modalManager.closeModal(window, true);
        }, CLOSE_TIMEOUT)
    }
});
