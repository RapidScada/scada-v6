// Provides data exchange between view frame and data window frame.
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

    // Finds a view hub instance.
    static getInstance() {
        let wnd = window;
        while (wnd) {
            if (wnd.viewHub) {
                return wnd.viewHub;
            }
            wnd = wnd === window.top ? null : wnd.parent;
        }
        return null;
    }
}
