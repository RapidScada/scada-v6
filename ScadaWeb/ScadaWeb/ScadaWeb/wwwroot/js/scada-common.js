// Contains common JavaScript classes.
// No dependencies.

// Provides helper methods.
class ScadaUtils {
    // The window width that is considered a small.
    static SMALL_WND_WIDTH = 800;

    // Checks if a browser window is small sized.
    static get isSmallScreen() {
        return top.innerWidth <= this.SMALL_WND_WIDTH;
    }

    // Checks if browser is in full screen mode switched on programmatically.
    // See https://developer.mozilla.org/en-US/docs/Web/API/Fullscreen_API
    static get isFullscreen() {
        return document.fullscreenElement ||
            document.mozFullScreenElement ||
            document.webkitFullscreenElement ||
            document.msFullscreenElement;
    }

    // Checks if browser is actually in full screen mode.
    static get isActualFullscreen() {
        return screen.height - window.innerHeight <= 1;
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

    // Switches browser to full screen mode and back to normal view.
    static toggleFullscreen() {
        if (this.isFullscreen) {
            this.exitFullscreen();
        } else {
            this.requestFullscreen();
        }
    }
}

// Specifies event types.
class ScadaEventTypes {
    // Notifies controls that the layout should be updated.
    static UPDATE_LAYOUT = "scada:updateLayout";
}
