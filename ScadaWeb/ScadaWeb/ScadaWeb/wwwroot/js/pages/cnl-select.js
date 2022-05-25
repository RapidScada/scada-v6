$(document).ready(function () {
    $("#selObj").change(function () {
        $("#chkOnlySelected").prop("checked", false);
        $("form:first").submit();
    });

    $("#chkOnlySelected").change(function () {
        $("#selObj").val("0");
        $("form:first").submit();
    });

    $("#tblCnl .chk-sel").change(function () {
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
