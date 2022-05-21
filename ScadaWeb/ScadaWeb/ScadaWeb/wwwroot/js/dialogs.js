// Contains classes: Dialogs
// Depends on jquery, scada-common.js, modal.js

// Contains methods for displaying standard modal dialogs.
class Dialogs {
    rootPath;
    modalManager;

    constructor(rootPath, opt_modalManager) {
        this.rootPath = rootPath;
        this.modalManager = opt_modalManager;
    }

    _getModalManager() {
        return this.modalManager
            ? this.modalManager
            : ModalManager.getInstance();
    }

    // Shows a dialog for selecting channels.
    // opt_callback is a function (result)
    // result can be null or an object { cnlNums }
    selectChannels(selectedCnlNums, callback) {
        this._getModalManager().showModal(
            this.rootPath + "CnlSelect?cnlNums=" + selectedCnlNums,
            new ModalOptions(ModalButton.OK_CANCEL, ModalSize.LARGE),
            callback);
    }
}
