// Contains classes: ViewHub
// Depends on scada-common.js

// Provides data exchange between view frame and data window frame.
class ViewHub {
    constructor(mainWindow, mainObj) {
        // The reference to the main window object.
        this.mainWindow = mainWindow;
        // Contains environment variables.
        this.appEnv = mainObj.appEnv;
        // Manages modal dialogs.
        this.modalManager = mainObj.modalManager;
        // Provides access to plugin features.
        this.features = mainObj.features;
        // The current view ID.
        this.viewID = 0;
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
    getViewUrl(viewID, opt_openInFrame) {
        return opt_openInFrame
            ? appEnv.rootPath + "ViewFrame/" + viewID
            : appEnv.rootPath + "View/" + viewID;
    }

    // Pulls the trigger of the main window.
    notifyMainWindow(eventType, opt_extraParams) {
        this.mainWindow.dispatchEvent(new CustomEvent(eventType, opt_extraParams));
        //this.mainWindow.$(this.mainWindow).trigger(eventType, opt_extraParams);
    }

    // Finds an existing or create a new view hub instance.
    static getInstance() {
        return ViewHub._findInstance() ??
            new ViewHub(window, {
                appEnv: appEnvStub,
                modalManager: new ModalManager(),
                features: new PluginFeatures(appEnvStub)
            });
    }
}
