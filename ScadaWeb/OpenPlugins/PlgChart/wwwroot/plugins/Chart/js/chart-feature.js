class ChartFeature extends ChartFeatureBase {
    constructor(appEnv) {
        super(appEnv);
    }

    getUrl(cnlNums, startDate, args) {
        return appEnv.rootPath + `Chart/Chart?cnlNums=${cnlNums}&startDate=${startDate}` +
            (args ? "&" + new URLSearchParams(args).toString() : "");
    }
}
