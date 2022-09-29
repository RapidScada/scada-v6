$(document).ready(function () {
    $("#btnGenerateReport").click(function () {
        if ($("#frmReportArgs")[0].reportValidity()) {
            return false;
        }
    });
});
