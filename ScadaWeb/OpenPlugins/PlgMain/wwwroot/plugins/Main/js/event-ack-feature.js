class EventAckFeature extends EventAckFeatureBase {
    constructor(appEnv) {
        super(appEnv);
    }

    show(archiveBit, eventID, opt_callback) {
        ModalManager.getInstance().showModal(
            appEnv.rootPath + `Main/EventAck?archiveBit=${archiveBit}&eventID=${eventID}`,
            new ModalOptions([ModalButton.OK, ModalButton.CLOSE], ModalSize.LARGE),
            opt_callback);
    }
}
