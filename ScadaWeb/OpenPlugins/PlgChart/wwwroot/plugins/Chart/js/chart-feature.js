class ChartFeature extends BaseChartFeature {
    constructor(appEnv) {
        super(appEnv);
    }

    show(cnlNums, startDate, args) {
        window.open(appEnv.rootPath + `Chart/Chart?cnlNums=${cnlNums}&startDate=${startDate}` +
            (args ? "&" + args : ""));
    }
}
