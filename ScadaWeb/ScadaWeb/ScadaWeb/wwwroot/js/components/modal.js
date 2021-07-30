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

    // Finds an existing or create a new manager instance.
    static getInstance() {
        return ModalManager._findInstance() || new ModalManager();
    }
}
