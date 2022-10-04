﻿// Contains classes: ScadaUtils, ScadaEventType, PluginFeatures, BaseChartFeature, BaseCommandFeature, BaseEventAckFeature
// Contains objects: appEnvStub
// No dependencies.

// Provides helper methods.
class ScadaUtils {
    // The window width that is considered a small.
    static SMALL_WND_WIDTH = 800;
    // The z-index that moves an element to the front.
    static FRONT_ZINDEX = 10000;

    // Checks if a browser window is small sized, such as a mobile device.
    static get isSmallScreen() {
        return top.innerWidth <= this.SMALL_WND_WIDTH;
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

    // Gets the number of days in the specified month and year. Month is between 0 and 11.
    static daysInMonth(year, month) {
        return new Date(year, month + 1, 0).getDate();
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

    // Checks if the specified locale is Russian. If no argument is specified, the browser locale is checked.
    static isRussian(opt_locale) {
        let lang = opt_locale || navigator.language.toLowerCase();
        return lang === "ru" || lang.startsWith("ru");
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

// Specifies event types.
// Do not use dots in event type names because dots are used by event listeners to separate event name and namespace.
class ScadaEventType {
    // Notifies a page that the layout should be updated.
    // No parameters.
    static UPDATE_LAYOUT = "rs:updateLayout";

    // Notifies a page that the title should be updated.
    // No parameters.
    static UPDATE_TITLE = "rs:updateTitle";

    // Notifies that a modal dialog button has been clicked.
    // Event parameter: button value.
    static MODAL_BTN_CLICK = "rs:modalBtnClick";
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
                ChartFeature.prototype instanceof BaseChartFeature
                ? new ChartFeature(this.appEnv)
                : new BaseChartFeature(this.appEnv);
        }

        return this._chart;
    }

    // Gets the command feature.
    get command() {
        if (this._command === null) {
            this._command = typeof CommandFeature === "function" &&
                CommandFeature.prototype instanceof BaseCommandFeature
                ? new CommandFeature(this.appEnv)
                : new BaseCommandFeature(this.appEnv);
        }

        return this._command;
    }

    // Gets the event acknowledgement feature.
    get eventAck() {
        if (this._eventAck === null) {
            this._eventAck = typeof EventAckFeature === "function" &&
                EventAckFeature.prototype instanceof BaseEventAckFeature
                ? new EventAckFeature(this.appEnv)
                : new BaseEventAckFeature(this.appEnv);
        }

        return this._eventAck;
    }
}

// Represents a default charting feature.
class BaseChartFeature {
    constructor(appEnv) {
        this.appEnv = appEnv;
    }

    // Shows a chart.
    // cnlNums is a string containing a range of integers,
    // startDate is a string in the YYYY-MM-DD format,
    // args is a string containing arbitrary arguments.
    show(cnlNums, startDate, args) {
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

    // Shows a command dialog.
    // opt_callback is a function (result), where result can be true or false.
    show(cnlNum, opt_callback) {
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

    // Shows an event acknowledgement dialog.
    // opt_callback is a function (result), where result can be true or false.
    show(archiveBit, eventID, opt_callback) {
        alert(ScadaUtils.isRussian(appEnv.locale) ?
            "Ни один плагин не реализует функцию квитирования." :
            "No plugin implements the acknowledgement feature.");
    }
}

// The stub of an application environment object.
const appEnvStub = {
    isStub: true,
    rootPath: "/",
    locale: "en-GB",
    productName: "Rapid SCADA"
};
