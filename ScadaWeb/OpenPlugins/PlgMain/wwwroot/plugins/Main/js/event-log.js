class EventLogTableForm extends TableFormAsync {
    constructor(phrases) {
        super("frmEventLog", phrases);
    }

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

// Set in *.cshtml
var tableFormPhrases = {};

$(() => {
    let form = new EventLogTableForm(tableFormPhrases);
    form.init();
});
