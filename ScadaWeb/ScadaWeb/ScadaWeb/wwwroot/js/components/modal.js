// Contains classes: ModalButton, ModalSizes, ModalOptions, ModalManager
// Depends on jquery, bootstrap

// Specifies the buttons available for a modal dialog.
class ModalButton {
    static OK = "ok";
    static YES = "yes";
    static NO = "no";
    static EXEC = "execute";
    static CANCEL = "cancel";
    static CLOSE = "close";
};

// Specifies the sizes of a modal dialog.
class ModalSizes {
    static NORMAL = 0;
    static SMALL = 1;
    static LARGE = 2;
};

// Represents modal dialog options.
class ModalOptions {
    constructor(opt_buttons, opt_size, opt_height) {
        this.buttons = opt_buttons || [ModalButton.OK, ModalButton.CANCEL];
        this.size = opt_size || ModalSizes.NORMAL;
        this.height = opt_height || 0;
    }
}

// Manages modal dialogs.
class ModalManager {
    constructor() {

    }

    // Finds an existing manager instance.
    static _findInstance() {
        let wnd = window;

        while (wnd) {
            if (wnd.modalManager) {
                return wnd.viewHub;
            }

            wnd = wnd === window.top ? null : wnd.parent;
        }

        return null;
    }

    // Opens the modal dialog containing the specified page.
    // opt_callback is a function (result, args)
    showModal(url, opt_options, opt_callback) {

    }

    // Closes the modal dialog with the specified result.
    closeModal(modalWnd, result, args) {

    }

    // Updates the modal dialog height according to its frame height.
    updateModalHeight(modalWnd) {

    }

    // Sets the result of the modal dialog, keeping it open.
    setResult(modalWnd, result, args) {

    }

    // Sets the title of the modal dialog.
    setTitle(modalWnd, title) {

    }

    // Shows or hides the button of the modal dialog.
    setButtonVisible(modalWnd, buttonName, value) {

    }

    // Enables or disables the button of the modal dialog.
    setButtonEnabled(modalWnd, buttonName, value) {

    }

    // Finds an existing or create a new manager instance.
    static getInstance() {
        return ModalManager._findInstance() || new ModalManager();
    }
}
