// The variables below are set in Command.cshtml
var hideExecBtn = false;
var updateHeight = false;
var closeModal = false;

const CLOSE_TIMEOUT = 1000;

$(document).ready(function () {
    // hide Execute button
    let modalManager = ModalManager.getInstance();

    if (hideExecBtn) {
        modalManager.setButtonVisible(window, ModalButton.EXEC, false);
    }

    // update modal height
    if (updateHeight) {
        modalManager.updateModalHeight(window, true);
    }

    // close modal
    if (closeModal) {
        setTimeout(function () {
            modalManager.closeModal(window, true);
        }, CLOSE_TIMEOUT)
    }

    // send enumeration command
    $("#divEnum button").on("click", function () {
        $("#hidCmdEnum").val($(this).data("cmdval"));
        $("form:first").submit();
    });

    // reset invalid state
    $("input.is-invalid, textarea.is-invalid").on("input", function () {
        $(this).removeClass("is-invalid");
    });
});
