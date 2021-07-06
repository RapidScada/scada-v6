// Provides data exchange between view frame and data window frame.
// Depends on scada-common.js
class ViewHub {
    constructor(appEnv) {
        // The application environment.
        this.appEnv = appEnv;
        // The current view ID.
        this.viewID = 0;
    }

    // Gets the view page URL.
    getViewUrl(viewID) {
        return appEnv.rootPath + "View/" + viewID;
    }

    // Finds an existing view hub instance.
    static findInstance() {
        let wnd = window;

        while (wnd) {
            if (wnd.viewHub) {
                return wnd.viewHub;
            }

            wnd = wnd === window.top ? null : wnd.parent;
        }

        return null;
    }

    // Finds an existing or create a new view hub instance.
    static getInstance() {
        return ViewHub.findInstance() || new ViewHub(appEnvStub);
    }
}
