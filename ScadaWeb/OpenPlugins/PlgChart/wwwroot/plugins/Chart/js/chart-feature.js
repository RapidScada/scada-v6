class ChartFeature extends ChartFeatureBase {
    constructor(appEnv) {
        super(appEnv);
    }

    getUrl(cnlNums, startDate, opt_args) {
        return appEnv.rootPath + `Chart/Chart?cnlNums=${cnlNums}&startDate=${startDate}` +
            (opt_args ? "&" + new URLSearchParams(opt_args).toString() : "");
    }
}
