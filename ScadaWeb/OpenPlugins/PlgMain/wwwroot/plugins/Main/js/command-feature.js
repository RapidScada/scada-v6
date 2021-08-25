class CommandFeature extends BaseCommandFeature {
    constructor(appEnv) {
        super(appEnv);
    }

    show(outCnlNum, opt_callback) {
        ModalManager.getInstance().showModal(
            appEnv.rootPath + "Main/Command?outCnlNum=" + outCnlNum,
            new ModalOptions([ModalButton.OK, ModalButton.CLOSE], ModalSize.LARGE),
            opt_callback);
    }
}
