class EventLogTableForm extends TableFormAsync {
    _bindEvents() {
        super._bindEvents();

    }

    _getDataUrl(opt_handler) {
        return opt_handler
            ? "EventLog?handler=" + opt_handler
            : "EventLog";
    }
}

$(() => {
    let form = new EventLogTableForm();
    form.init();
});
