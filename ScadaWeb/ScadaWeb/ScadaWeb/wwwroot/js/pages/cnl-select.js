$(function () {
    let pager = new RazorPager("pgrCnl", { submitOnClick: true });

    $("#selObj").on("change", function () {
        pager.reset();
        $("#chkOnlySelected").prop("checked", false);
        $("form:first").trigger("submit");
    });

    $("#chkOnlySelected").on("change", function () {
        pager.reset();
        $("#selObj").val("0");
        $("form:first").trigger("submit");
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
