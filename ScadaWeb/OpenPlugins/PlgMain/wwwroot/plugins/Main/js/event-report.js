﻿const BUTTON_LOCK_DURATION = 3000; // ms
const MS_PER_DAY = 24 * 60 * 60 * 1000;

// The variables below are set in EventReport.cshtml
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
    } else if (endTimeMs - startTimeMs > maxReportPeriod * MS_PER_DAY) {
        errors.push(ScadaUtils.formatString(phrases.PeriodTooLong, maxReportPeriod));
    }

    // severity
    if (!isSeveritySelected()) {
        errors.push(phrases.NoSeverity);
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
    }, BUTTON_LOCK_DURATION);
}

function isSeveritySelected() {
    let isSelected = false;

    $("#ulSeverity input:checkbox").each(function () {
        if ($(this).prop("checked")) {
            isSelected = true;
            return false; // break loop
        }
    });

    return isSelected;
}

function getSeverityRange() {
    let anySeverity = true;
    let severityArr = [];

    $("#ulSeverity input:checkbox").each(function () {
        if ($(this).prop("checked")) {
            severityArr.push($(this).val());
        } else {
            anySeverity = false;
        }
    });

    return anySeverity ? "" : severityArr.join(",");
}

$(document).ready(function () {
    $("#btnGenerateReport").click(function () {
        hideErrorMessage();

        if ($("#frmReportArgs")[0].reportValidity() && reportValidityExtra()) {
            lockGenerateButton();
            let reportUrl = "Print/PrintEventReport" +
                "?startTime=" + $("#txtStartTime").val() +
                "&endTime=" + $("#txtEndTime").val() +
                "&archive=" + $("#selArchive option:selected").val() +
                "&objNum=" + $("#selObj option:selected").val() +
                "&severities=" + getSeverityRange();
            location = reportUrl;
        }

        return false;
    });
});
