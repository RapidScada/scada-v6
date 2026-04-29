// Contains classes: ScadaUtils, ScadaEventType, PluginFeatures,
//     ChartFeatureBase, CommandFeatureBase, EventAckFeatureBase, Dto, Severity
// Contains objects: appEnvStub
// No dependencies.

// Provides helper methods.
class ScadaUtils {
    // The window width that is considered a small.
    static SMALL_WND_WIDTH = 800;
    // The z-index that moves an element to the front.
    static FRONT_ZINDEX = 10000;
    // The number of milliseconds in a day.
    static MS_PER_DAY = 24 * 60 * 60 * 1000;
    // Specifies how long an error badge is displayed, ms.
    static ERROR_DISPLAY_DURATION = 7000;
    // Specifies how long a button is locked after click, ms.
    static BUTTON_LOCK_DURATION = 3000;

    // Checks if a browser window is small sized, such as a mobile device.
    static get isSmallScreen() {
        return top.innerWidth <= ScadaUtils.SMALL_WND_WIDTH;
    }

    // Checks if browser is in full screen mode switched on programmatically.
    // See https://developer.mozilla.org/en-US/docs/Web/API/Fullscreen_API
    static get isFullscreen() {
        return document.fullscreenElement ||
            document.mozFullScreenElement ||
            document.webkitFullscreenElement ||
            document.msFullscreenElement
            ? true
            : false;
    }

    // Checks if browser is actually in full screen mode.
    static get isActualFullscreen() {
        return screen.height - window.innerHeight <= 1;
    }

    // Detects if iOS is used.
    static get iOS() {
        return /iPad|iPhone|iPod/.test(navigator.platform);
    }

    // Gets the current time string for logging.
    static getCurrentTime() {
        return new Date().toLocaleTimeString("en-GB");
    }

    // Converts the specified date object to a string in YYYY-MM-DD format.
    static dateToString(date) {
        return date.toISOString().slice(0, 10);
    }

    // Gets the number of days in the specified month and year. Month is between 0 and 11.
    static daysInMonth(year, month) {
        return new Date(year, month + 1, 0).getDate();
    }

    // Adds one day to the specified date. Returns a new date object.
    static addDay(value) {
        let date = new Date(value);
        date.setDate(date.getDate() + 1);
        return date;
    }

    // Adds one month to the specified date. Returns a new date object.
    static addMonth(value) {
        let date = new Date(value);
        date.setMonth(date.getMonth() + 1);
        return date;
    }

    // Switches browser to full screen mode.
    static requestFullscreen() {
        if (document.documentElement.requestFullscreen) {
            document.documentElement.requestFullscreen();
        } else if (document.documentElement.msRequestFullscreen) {
            document.documentElement.msRequestFullscreen();
        } else if (document.documentElement.mozRequestFullScreen) {
            document.documentElement.mozRequestFullScreen();
        } else if (document.documentElement.webkitRequestFullscreen) {
            document.documentElement.webkitRequestFullscreen(Element.ALLOW_KEYBOARD_INPUT);
        }
    }

    // Exits browser full screen mode.
    static exitFullscreen() {
        if (document.exitFullscreen) {
            document.exitFullscreen();
        } else if (document.msExitFullscreen) {
            document.msExitFullscreen();
        } else if (document.mozCancelFullScreen) {
            document.mozCancelFullScreen();
        } else if (document.webkitExitFullscreen) {
            document.webkitExitFullscreen();
        }
    }

    // Selects the specified option of the select HTML element if it exists.
    static selectOptionIfExists(jqSelect, value) {
        if (jqSelect.find("option[value='" + value + "']").length > 0) {
            jqSelect.val(value);
        }
    }

    // Plays a sound of the audio jQuery object.
    static playSound(jqAudio) {
        if (jqAudio.length > 0) {
            jqAudio[0]
                .play()
                .catch(error => console.error("Error playing sound", jqAudio.attr("src"), error));
        }
    }

    // Gets a value with the specified key in the given Storage object.
    static getStorageItem(storage, keyName, opt_defaultValue) {
        try {
            let val = storage.getItem(keyName);
            return val ? val : opt_defaultValue;
        } catch (ex) {
            console.error(ex);
            return opt_defaultValue;
        }
    }

    // Adds or updates the specified key and value in the given Storage object.
    static setStorageItem(storage, keyName, keyValue) {
        try {
            storage.setItem(keyName, keyValue);
        } catch (ex) {
            console.error(ex);
        }
    }

    // Gets a cookie by name.
    static getCookie(name) {
        let cookie = " " + document.cookie;
        let search = " " + name + "=";
        let offset = cookie.indexOf(search);

        if (offset >= 0) {
            offset += search.length;
            let end = cookie.indexOf(";", offset);

            if (end < 0)
                end = cookie.length;

            return decodeURIComponent(cookie.substring(offset, end));
        } else {
            return null;
        }
    }

    // Sets the cookie.
    static setCookie(name, value, opt_expireDays) {
        let expires = "";

        if (opt_expireDays) {
            let expireDate = new Date();
            expireDate.setDate(expireDate.getDate() + opt_expireDays);
            expires = "; expires=" + expiresDate.toUTCString();
        }

        document.cookie = name + "=" + encodeURIComponent(value) + expires + "; samesite=lax";
    }

    // Replaces the existing frame by a new one to prevent writing frame history. Returns the created frame.
    static replaceFrame(jqFrame, opt_url) {
        let frameParent = jqFrame.parent();
        let frameClone = jqFrame.clone();
        jqFrame.remove();

        if (opt_url) {
            frameClone.attr("src", opt_url);
        }

        frameClone.appendTo(frameParent);
        return frameClone;
    }

    // Checks that the frame is accessible due to the browser security.
    static checkAccessToFrame(frameWnd, opt_requireJq) {
        try {
            return frameWnd.document !== null && (!opt_requireJq || frameWnd.$);
        } catch (ex) {
            return false;
        }
    }

    // Creates a full copy of the specified object.
    static deepClone(obj, opt_withPrototype) {
        // alternatively, use structuredClone()
        let objClone = JSON.parse(JSON.stringify(obj));

        if (obj instanceof Object && opt_withPrototype) {
            let proto = Object.getPrototypeOf(obj);

            if (proto) {
                let newObj = Object.create(proto);
                Object.assign(newObj, objClone);
                return newObj;
            }
        }

        return objClone;
    }

    // Checks if the specified locale is Russian. If no argument is specified, the browser locale is checked.
    static isRussian(opt_locale) {
        let lang = opt_locale || navigator.language.toLowerCase();
        return lang === "ru" || lang.startsWith("ru");
    }

    // Checks if the specified string represents valid JSON.
    static isValidJSON(s) {
        try {
            JSON.parse(s);
            return true;
        } catch {
            return false;
        }
    }

    // Converts the string to an array of integers.
    static parseIntArray(s) {
        return s ? s.split(",").map(x => parseInt(x)) : [];
    }

    // Converts the string to a set of integers.
    static parseIntSet(s) {
        return new Set(ScadaUtils.parseIntArray(s));
    }

    // Converts the string representation of an integer range to an array that contains unique and sorted values.
    static parseRange(s) {
        let intSet = new Set();

        for (let part of s.split(",")) {
            if (part) {
                let dashIdx = part.indexOf('-');

                if (dashIdx >= 0) {
                    // two numbers separated by a dash
                    let val1 = parseInt(part.substring(0, dashIdx));
                    let val2 = parseInt(part.substring(dashIdx + 1));

                    if (isNaN(val1) || isNaN(val2)) {
                        return null;
                    } else {
                        for (let val = val1; val <= val2; val++) {
                            intSet.add(val);
                        }
                    }
                } else {
                    // single number
                    let val = parseInt(part);

                    if (isNaN(val)) {
                        return null;
                    } else {
                        intSet.add(val);
                    }
                }
            }
        }

        return [...intSet].sort();
    }

    // Replaces the format items with the arguments.
    static formatString(format, ...args) {
        let s = format;

        for (let i = 0; i < args.length; i++) {
            s = s.replace("{" + i + "}", args[i]);
        }

        return s;
    }
}

// Specifies the event types.
class ScadaEventType {
    // Notifies a page that the layout should be updated.
    // No parameters.
    static UPDATE_LAYOUT = "updatelayout.rs.common";

    // Notifies a page that the title should be updated.
    // No parameters.
    static UPDATE_TITLE = "updatetitle.rs.common";

    // Notifies that a modal dialog button has been clicked.
    // Event parameter: button value.
    static MODAL_BTN_CLICK = "modalbtnclick.rs.common";
}

// Provides access to plugin features implemented by various plugins.
class PluginFeatures {
    constructor(appEnv) {
        this.appEnv = appEnv;

        this._chart = null;
        this._command = null;
        this._eventAck = null;
    }

    // Gets the charting feature.
    get chart() {
        if (this._chart === null) {
            this._chart = typeof ChartFeature === "function" &&
                ChartFeature.prototype instanceof ChartFeatureBase
                ? new ChartFeature(this.appEnv)
                : new ChartFeatureBase(this.appEnv);
        }

        return this._chart;
    }

    // Gets the command feature.
    get command() {
        if (this._command === null) {
            this._command = typeof CommandFeature === "function" &&
                CommandFeature.prototype instanceof CommandFeatureBase
                ? new CommandFeature(this.appEnv)
                : new CommandFeatureBase(this.appEnv);
        }

        return this._command;
    }

    // Gets the event acknowledgement feature.
    get eventAck() {
        if (this._eventAck === null) {
            this._eventAck = typeof EventAckFeature === "function" &&
                EventAckFeature.prototype instanceof EventAckFeatureBase
                ? new EventAckFeature(this.appEnv)
                : new EventAckFeatureBase(this.appEnv);
        }

        return this._eventAck;
    }
}

// Represents a default charting feature.
class ChartFeatureBase {
    constructor(appEnv) {
        this.appEnv = appEnv;
        this._NO_PLUGIN = ScadaUtils.isRussian(appEnv.locale) ?
            "Ни один плагин не реализует функцию графиков." :
            "No plugin implements the charting feature.";
    }

    // Gets a chart URL.
    // cnlNums is a string containing a range of integers,
    // startDate is a string in the YYYY-MM-DD format,
    // args is a string or an object containing arbitrary arguments.
    getUrl(cnlNums, startDate, args) {
        console.warn(this._NO_PLUGIN);
        return "";
    }

    // Shows a chart.
    show(cnlNums, startDate, args) {
        let url = this.getUrl(cnlNums, startDate, args);

        if (url) {
            window.open(url);
        } else {
            alert(this.NO_PLUGIN);
        }
    }
}

// Represents a default command feature.
class CommandFeatureBase {
    constructor(appEnv) {
        this.appEnv = appEnv;
    }

    // Shows a command dialog.
    // cnlNum is the channel number that defines a command,
    // args is a string or an object containing arbitrary arguments,
    // opt_callback is a function (result), where result can be true or false.
    show(cnlNum, args, opt_callback) {
        alert(ScadaUtils.isRussian(appEnv.locale) ?
            "Ни один плагин не реализует функцию команд." :
            "No plugin implements the command feature.");
        opt_callback?.(false);
    }
}

// Represents a default event acknowledgement feature.
class EventAckFeatureBase {
    constructor(appEnv) {
        this.appEnv = appEnv;
    }

    // Shows an event acknowledgement dialog.
    // eventID specifies the event to display and acknowledge,
    // args is a string or an object containing arbitrary arguments,
    // opt_callback is a function (result), where result can be true or false.
    show(eventID, args, opt_callback) {
        alert(ScadaUtils.isRussian(appEnv.locale) ?
            "Ни один плагин не реализует функцию квитирования." :
            "No plugin implements the acknowledgement feature.");
        opt_callback?.(false);
    }
}

// Represents a data transfer object that carries data from the server side to a client.
class Dto {
    constructor() {
        this.ok = false;
        this.msg = "";
        this.data = null;
    }

    // Creates a data transfer object containing a successfull result.
    static success(data) {
        let dto = new Dto();
        dto.ok = true;
        dto.data = data;
        return dto;
    }

    // Creates a data transfer object containing a failed result.
    static fail(msg) {
        let dto = new Dto();
        dto.msg = msg;
        return dto;
    }
}

// Specifies the severity levels.
class Severity {
    static MIN = 1;
    static MAX = 999;
    static UNDEFINED = 0;
    static CRITICAL = 1;
    static MAJOR = 250;
    static MINOR = 500;
    static INFO = 750;

    // Gets the closest known severity.
    static closest(value) {
        if (Severity.CRITICAL <= value && value < Severity.MAJOR) {
            return Severity.CRITICAL;
        } else if (Severity.MAJOR <= value && value < Severity.MINOR) {
            return Severity.MAJOR;
        } else if (Severity.MINOR <= value && value < Severity.INFO) {
            return Severity.MINOR;
        } else if (Severity.INFO <= value && value < Severity.MAX) {
            return Severity.INFO;
        } else {
            return Severity.UNDEFINED;
        }
    }
}

// The stub of an application environment object.
const appEnvStub = {
    isStub: true,
    basePath: "",
    rootPath: "/",
    locale: "en-GB",
    productName: "Rapid SCADA"
};
