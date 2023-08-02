// The variables below must be defined in Chart.cshtml
// timeRange
// chartData
// chartTitle
// chartStatus
// locale
// gapBetweenPoints

// Sets the chart width and height.
function updateLayout() {
    $("#divChart")
        .outerWidth($(window).width())
        .outerHeight($(window).height());
}

$(document).ready(function () {
    // chart parameters must be defined in Chart.cshtml
    let chart = new scada.chart.Chart("divChart");
    chart.controlOptions.chartTitle = chartTitle;
    chart.controlOptions.chartStatus = chartStatus;
    chart.controlOptions.locale = locale;
    chart.controlOptions.gapBetweenPoints = gapBetweenPoints;
    chart.timeRange = timeRange;
    chart.chartData = chartData;
    chart.buildDom();

    updateLayout();
    chart.draw();
    chart.bindHintEvents();

    $(window).on("resize", function () {
        updateLayout();
        chart.draw();
    });
});
