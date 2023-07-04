class EventAckFeature extends EventAckFeatureBase {
    constructor(appEnv) {
        super(appEnv);
    }

    show(eventID, args, opt_callback) {
        ModalManager.getInstance().showModal(
            appEnv.rootPath + "Main/EventAck?eventID=" + eventID +
            (args ? "&" + new URLSearchParams(args).toString() : ""),
            new ModalOptions({ buttons: [ModalButton.OK, ModalButton.CLOSE], size: ModalSize.LARGE }),
            opt_callback);
    }
}
