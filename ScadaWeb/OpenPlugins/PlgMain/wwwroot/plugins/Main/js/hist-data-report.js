$(document).ready(function () {
    $("#btnGenerateReport").click(function () {
        if ($("#frmReportArgs")[0].reportValidity()) {
            let reportUrl = "Print/PrintHistDataReport" +
                "?startTime=" + $("#txtStartTime").val() +
                "&endTime=" + $("#txtEndTime").val() +
                "&archive=" + ($("#selArchive option:selected").val() ?? "") +
                "&cnlNums=" + $("#txtCnlNums").val();
            location = reportUrl;
        }

        return false;
    });

    $("#btnSelectCnls").click(function () {
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
