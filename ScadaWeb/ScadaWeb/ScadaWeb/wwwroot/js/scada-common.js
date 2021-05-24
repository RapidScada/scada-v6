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
}

// Specifies event types.
class ScadaEventTypes {
    // Notifies controls that the layout should be updated.
    static UPDATE_LAYOUT = "scada:updateLayout";
}
