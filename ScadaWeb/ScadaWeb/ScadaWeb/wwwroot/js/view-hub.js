// Provides data exchange between view frame and data window frame.
// Depends on scada-common.js
class ViewHub {
    constructor(appEnv) {
        // The application environment.
        this.appEnv = appEnv;
        // The current view ID.
        this.viewID = 0;
        // Provides access to plugin features.
        this.features = new PluginFeatures(appEnv);
    }

    // Finds an existing view hub instance.
    static _findInstance() {
        let wnd = window;

        while (wnd) {
            if (wnd.viewHub) {
                return wnd.viewHub;
            }

            wnd = wnd === window.top ? null : wnd.parent;
        }

        return null;
    }

    // Gets the view page URL.
    getViewUrl(viewID) {
        return appEnv.rootPath + "View/" + viewID;
    }

    // Finds an existing or create a new view hub instance.
    static getInstance() {
        return ViewHub._findInstance() || new ViewHub(appEnvStub);
    }
}
