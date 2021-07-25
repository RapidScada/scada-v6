class ChartFeature extends BaseChartFeature {
    constructor(appEnv) {
        super(appEnv);
    }

    show(cnlNums, startDate) {
        window.open(appEnv.rootPath + `Chart/Chart?cnlNums=${cnlNums}&startDate=${startDate}`);
    }
}
