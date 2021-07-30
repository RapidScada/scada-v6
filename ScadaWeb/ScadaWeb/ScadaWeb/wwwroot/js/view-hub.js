// Contains classes: ViewHub, PluginFeatures, BaseChartFeature, BaseCommandFeature, BaseEventAckFeature
// Depends on scada-common.js

// Provides data exchange between view frame and data window frame.
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

// Provides access to plugin features implemented by various plugins.
class PluginFeatures {
    constructor(appEnv) {
        this._chart = null;
        this._command = null;
        this._eventAck = null;
    }

    // Gets the charting feature.
    get chart() {
        if (this._chart === null) {
            this._chart = typeof ChartFeature === "function" &&
                ChartFeature.prototype instanceof BaseChartFeature
                ? new ChartFeature(appEnv)
                : new BaseChartFeature(appEnv);
        }

        return this._chart;
    }

    // Gets the command feature.
    get command() {
        if (this._command === null) {
            this._command = typeof CommandFeature === "function" &&
                CommandFeature.prototype instanceof BaseCommandFeature
                ? new CommandFeature(appEnv)
                : new BaseCommandFeature(appEnv);
        }

        return this._command;
    }

    // Gets the event acknowledgement feature.
    get eventAck() {
        if (this._eventAck === null) {
            this._eventAck = typeof EventAckFeature === "function" &&
                EventAckFeature.prototype instanceof BaseEventAckFeature
                ? new EventAckFeature(appEnv)
                : new BaseEventAckFeature(appEnv);
        }

        return this._eventAck;
    }
}

// Represents a default charting feature.
class BaseChartFeature {
    constructor(appEnv) {
        this.appEnv = appEnv;
    }

    show(cnlNums, startDate) {
        alert(ScadaUtils.isRussian(appEnv.locale) ?
            "Ни один плагин не реализует функцию графиков." :
            "No plugin implements the charting feature.");
    }
}

// Represents a default command feature.
class BaseCommandFeature {
    constructor(appEnv) {
        this.appEnv = appEnv;
    }

    show(outCnlNum) {
        alert(ScadaUtils.isRussian(appEnv.locale) ?
            "Ни один плагин не реализует функцию команд." :
            "No plugin implements the command feature.");
    }
}

// Represents a default event acknowledgement feature.
class BaseEventAckFeature {
    constructor(appEnv) {
        this.appEnv = appEnv;
    }

    show(archiveBit, eventID) {
        alert(ScadaUtils.isRussian(appEnv.locale) ?
            "Ни один плагин не реализует функцию квитирования." :
            "No plugin implements the acknowledgement feature.");
    }
}
