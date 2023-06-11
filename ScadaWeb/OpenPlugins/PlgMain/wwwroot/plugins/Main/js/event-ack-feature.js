class EventAckFeature extends EventAckFeatureBase {
    constructor(appEnv) {
        super(appEnv);
    }

    show(eventID, opt_args, opt_callback) {
        ModalManager.getInstance().showModal(
            appEnv.rootPath + "Main/EventAck?eventID=" + eventID +
            (opt_args ? "&" + new URLSearchParams(opt_args).toString() : ""),
            new ModalOptions({ buttons: [ModalButton.OK, ModalButton.CLOSE], size: ModalSize.LARGE }),
            opt_callback);
    }
}
