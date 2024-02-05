﻿$(document).ready(function () {
    let pager = new Pager("pgrCnl");

    $("#selObj").on("change", function () {
        pager.reset();
        $("#chkOnlySelected").prop("checked", false);
        $("form:first").submit();
    });

    $("#chkOnlySelected").on("change", function () {
        pager.reset();
        $("#selObj").val("0");
        $("form:first").submit();
    });

    $("#tblCnl .chk-sel").on("change", function () {
        // add or remove selected channel number
        let cnlNumsElem = $("#hidSelectedCnlNums");
        let cnlNums = ScadaUtils.parseIntSet(cnlNumsElem.val());
        let cnlNum = parseInt($(this).data("cnlnum"));

        if ($(this).prop("checked")) {
            cnlNums.add(cnlNum);
        } else {
            cnlNums.delete(cnlNum);
        }

        cnlNumsElem.val(Array.from(cnlNums).join());
    });
});
