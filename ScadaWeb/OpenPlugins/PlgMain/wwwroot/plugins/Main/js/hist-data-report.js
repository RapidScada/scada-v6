// The variables below are set in HistDataReport.cshtml
var phrases = {};
var maxReportPeriod = 0;

function hideErrorMessage() {
    $("#divErrorMessage").addClass("hidden");
}

function reportValidityExtra() {
    let errors = [];

    // time range
    let startTimeMs = Date.parse($("#txtStartTime").val());
    let endTimeMs = Date.parse($("#txtEndTime").val());

    if (startTimeMs > endTimeMs) {
        errors.push(phrases.InvalidPeriod);
    } else if (endTimeMs - startTimeMs > maxReportPeriod * ScadaUtils.MS_PER_DAY) {
        errors.push(ScadaUtils.formatString(phrases.PeriodTooLong, maxReportPeriod));
    }

    // channel numbers
    let cnlNums = ScadaUtils.parseRange($("#txtCnlNums").val());

    if (!(cnlNums && cnlNums.length > 0)) {
        errors.push(phrases.InvalidChannels);
    }

    if (errors.length > 0) {
        $("#divErrorMessage")
            .html(errors.join("<br />"))
            .removeClass("hidden");
        return false;
    } else {
        return true;
    }
}

function lockGenerateButton() {
    $("#btnGenerateReport").prop("disabled", true);
    $("#divWaitHint").removeClass("hidden");

    setTimeout(function () {
        $("#btnGenerateReport").prop("disabled", false);
        $("#divWaitHint").addClass("hidden");
    }, ScadaUtils.BUTTON_LOCK_DURATION);
}

function getReportUrl() {
    return "Print/PrintHistDataReport" +
        "?startTime=" + $("#txtStartTime").val() +
        "&endTime=" + $("#txtEndTime").val() +
        "&archive=" + $("#selArchive option:selected").val() +
        "&cnlNums=" + $("#txtCnlNums").val();
}

$(document).ready(function () {
    $("#btnGenerateReport").on("click", function () {
        hideErrorMessage();

        if ($("#frmReportArgs")[0].reportValidity() && reportValidityExtra()) {
            lockGenerateButton();
            location = getReportUrl();
        }

        return false;
    });

    $("#btnSelectCnls").on("click", function () {
        let dialogs = new Dialogs("../../");
        let txtCnlNums = $("#txtCnlNums");

        dialogs.selectChannels(txtCnlNums.val(), function (result) {
            if (result) {
                txtCnlNums.val(result.cnlNums);
            }
        });

        return false;
    });
});
