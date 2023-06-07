class CommandFeature extends CommandFeatureBase {
    constructor(appEnv) {
        super(appEnv);
    }

    show(cnlNum, opt_callback) {
        ModalManager.getInstance().showModal(
            appEnv.rootPath + "Main/Command?cnlNum=" + cnlNum,
            new ModalOptions([ModalButton.EXEC, ModalButton.CLOSE], ModalSize.LARGE),
            opt_callback);
    }
}
