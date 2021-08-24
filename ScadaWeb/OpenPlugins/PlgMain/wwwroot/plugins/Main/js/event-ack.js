// The variables below are set from EventAck.cshtml
var ackAllowed = false;
var closeModal = false;

const CLOSE_TIMEOUT = 1000;

$(document).ready(function () {
    let modalManager = ModalManager.getInstance();

    // hide OK button
    if (!ackAllowed) {
        modalManager.setButtonVisible(window, ModalButton.OK, false);
    }

    // close modal
    if (closeModal) {
        setTimeout(function () {
            modalManager.closeModal(window, true);
        }, CLOSE_TIMEOUT)
    }

    // set event color
    let eventColor = $("#tblEvent").data("color");

    if (eventColor) {
        $("#tblEvent td").css("color", eventColor);
    }

    // submit the form on OK button click
    $(window).on(ScadaEventType.MODAL_BTN_CLICK, function (event, buttonValue) {
        if (buttonValue === ModalButton.OK) {
            $("#btnSubmit").click();
        }
    });
});
