class EventAckFeature extends BaseEventAckFeature {
    constructor(appEnv) {
        super(appEnv);
    }

    show(archiveBit, eventID, opt_callback) {
        ModalManager.getInstance().showModal(
            appEnv.rootPath + `Main/EventAck?archiveBit=${archiveBit}&eventID=${eventID}`,
            new ModalOptions([ModalButton.OK, ModalButton.CANCEL]),
            opt_callback);
    }
}
