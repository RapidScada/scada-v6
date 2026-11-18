class EventLogTableForm extends TableFormAsync {
    constructor() {
        super("frmEventLog");
    }

    _bindEvents() {
        super._bindEvents();

        $("#txtStartTime, #txtEndTime").on("change", async () => {
            await this._loadData();
        });

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
var maxReportPeriod = 0;
var reportPhrases = {};

$(() => {
    let form = new EventLogTableForm();
    form.init();
});
