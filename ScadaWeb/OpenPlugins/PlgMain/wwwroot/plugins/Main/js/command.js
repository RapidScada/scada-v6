// The variables below are set from Command.cshtml
var closeModal = false;

const CLOSE_TIMEOUT = 1000;

$(document).ready(function () {
    if (closeModal) {
        setTimeout(function () {
            ModalManager.getInstance().closeModal(window, true);
        }, CLOSE_TIMEOUT)
    }
});
