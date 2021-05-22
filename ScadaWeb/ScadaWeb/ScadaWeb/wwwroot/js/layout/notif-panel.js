// Represents a panel that contains notifications.
// Depends on jquery.
class NotifPanel {
    constructor(panelID, bellID) {
        this.panelElem = $("#" + panelID);
        this.bellElem = $("#" + bellID);
    }

    get isVisible() {
        return !this.panelElem.hasClass("hidden");
    }

    show(opt_animate) {
        if (!this.isVisible) {
            let cancel = false;

            /*if (opt_ifNewer) {
                let hiddenNotifKey = sessionStorage.getItem(this._HIDDEN_NOTIF_KEY);
                let lastNotifKey = this._getLastNotifKey();
                cancel = hiddenNotifKey !== null && lastNotifKey !== null && hiddenNotifKey === lastNotifKey.toString();
            }*/

            if (!cancel) {
                //sessionStorage.removeItem(this._HIDDEN_NOTIF_KEY);
                this.panelElem.removeClass("hidden");

                if (opt_animate) {
                    this.panelElem.css("right", -this.panelElem.outerWidth());
                    this.panelElem.animate({ right: 0 }, "fast");
                }
            }
        }
    }

    hide() {
        this.panelElem.addClass("hidden");
        //sessionStorage.setItem(this._HIDDEN_NOTIF_KEY, this._getLastNotifKey());
    }
}
