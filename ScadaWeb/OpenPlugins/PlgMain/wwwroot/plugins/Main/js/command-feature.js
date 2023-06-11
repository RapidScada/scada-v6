class CommandFeature extends CommandFeatureBase {
    constructor(appEnv) {
        super(appEnv);
    }

    show(cnlNum, args, opt_callback) {
        ModalManager.getInstance().showModal(
            appEnv.rootPath + "Main/Command?cnlNum=" + cnlNum,
            new ModalOptions({ buttons: [ModalButton.EXEC, ModalButton.CLOSE], size: ModalSize.LARGE }),
            opt_callback);
    }
}
