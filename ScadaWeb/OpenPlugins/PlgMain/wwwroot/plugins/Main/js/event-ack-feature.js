class EventAckFeature extends EventAckFeatureBase {
    constructor(appEnv) {
        super(appEnv);
    }

    show(eventID, opt_args, opt_callback) {
        ModalManager.getInstance().showModal(
            appEnv.rootPath + "Main/EventAck?eventID=" + eventID +
            (opt_args ? "&" + new URLSearchParams(opt_args).toString() : ""),
            new ModalOptions([ModalButton.OK, ModalButton.CLOSE], ModalSize.LARGE),
            opt_callback);
    }
}
