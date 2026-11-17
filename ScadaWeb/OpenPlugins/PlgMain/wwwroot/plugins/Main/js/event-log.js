class EventLogTableForm extends TableFormAsync {
    _bindEvents() {
        super._bindEvents();

        $("#btnAckSelected").on("click", () => {

        });

        $("#btnRefreshEvents").on("click", async () => {
            await this._loadData("Refresh");
        });

        $("#btnExportEvents").on("click", () => {

        });
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
