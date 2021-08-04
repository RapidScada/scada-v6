// Contains common JavaScript classes and objects.
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
    static checkAccessToFrame(frameWnd) {
        try {
            return frameWnd.document !== null;
        } catch (ex) {
            return false;
        }
    }

    // Checks if the specified locale is Russian. If no argument is specified, the browser locale is checked.
    static isRussian(opt_locale) {
        let lang = opt_locale || navigator.language.toLowerCase();
        return lang === "ru" || lang.startsWith("ru");
    }
}

// Specifies event types.
// Do not use dots in event type names because dots are used by event listeners to separate event name and namespace.
class ScadaEventType {
    // Notifies controls that the layout should be updated.
    static UPDATE_LAYOUT = "rs:updateLayout";

    // Notifies that a modal dialog button has been clicked.
    // Event parameter: button value.
    static MODAL_BTN_CLICK = "rs:modalBtnClick";
}

// The stub of an application environment object.
const appEnvStub = {
    isStub: true,
    rootPath: "/",
    locale: "en-GB",
    productName: "Rapid SCADA"
};
