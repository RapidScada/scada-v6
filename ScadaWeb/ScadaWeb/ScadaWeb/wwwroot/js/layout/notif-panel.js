// Contains notification classes.
// Depends on jquery, scada-common.js

// Represents a panel that contains notifications.
class NotifPanel {
    constructor(panelID, ...bellIDs) {
        // The notification ID counter.
        this._lastNotifID = 0;
        // The jQuery object that represents the mute button.
        this._muteBtn = $();
        // The jQuery object that represents an empty notification.
        this._emptyNotifElem = $();

        // The jQuery object that represents the notification panel.
        this.panelElem = $("#" + panelID);
        // The jQuery object that represents the notification bells.
        this.bellElem = bellIDs ? $("#" + bellIDs.join(", #")) : $();
    }

    // Determines whether the notification panel is visible.
    get isVisible() {
        return !this.panelElem.hasClass("hidden");
    }

    // Creates a jQuery element for the notification.
    _createNotifElem(notif) {
        let time = notif.timestamp instanceof Date ? notif.timestamp.toLocaleString() : notif.timestamp;
        let notifElem = $("<div id='notif_" + notif.id + "' class='notif'>" +
            "<div class='notif-icon'>" + this._getNotifTypeIcon(notif.notifType) + "</div>" +
            "<div class='notif-time'>" + time + "</div>" +
            "<div class='notif-msg'>" + notif.messageHtml + "</div></div>");
        notifElem.data("notif", notif);
        return notifElem;
    }

    // Gets a Font Awesome HTML code for the icon corresponding to the notification type.
    _getNotifTypeIcon(notifType) {
        switch (notifType) {
            case NotifType.INFO:
                return "<i class='fas fa-info-circle info'></i>";
            case NotifType.WARNING:
                return "<i class='fas fa-exclamation-triangle warning'></i>";
            case NotifType.ERROR:
                return "<i class='fas fa-exclamation-circle error'></i>";
            default:
                return "";
        }
    };

    // Prepares the notification panel for work.
    prepare() {
        let toolbarElem = $("<div class='notif-toolbar'></div>").appendTo(this.panelElem);

        this._muteBtn = $("<div class='notif-tool-btn'><i class='fas fa-toggle-on'></i><span></span></div>");
        this._muteBtn.children("span:first").text(NotifPhrases.Mute);
        toolbarElem.append(this._muteBtn);

        let ackAllBtn = $("<div class='notif-tool-btn'><i class='fas fa-check-double'></i><span></span></div>");
        ackAllBtn.children("span:first").text(NotifPhrases.AckAll);
        toolbarElem.append(ackAllBtn);

        this._emptyNotifElem = $("<div class='notif empty'></div>")
            .text(NotifPhrases.NoNotif)
            .appendTo(this.panelElem);

        if (ScadaUtils.isSmallScreen) {
            this.panelElem.addClass("mobile");
        }
    }

    // Shows the notification panel.
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

    // Hides the notification panel.
    hide() {
        this.panelElem.addClass("hidden");
        //sessionStorage.setItem(this._HIDDEN_NOTIF_KEY, this._getLastNotifKey());
    }

    // Adds a notification to the panel. Returns the notification ID.
    addNotification(notif) {
        notif.id = ++this._lastNotifID;
        this._emptyNotifElem.detach();
        this.panelElem.prepend(this._createNotifElem(notif));
        return notif.id;
    }

    // Adds sample notifications.
    addSamples() {
        const messageHtml =
            "<div>Notification text</div>" + 
            "<div><a href='#' class='notif-btn'>Info</a><a href='#' class='notif-btn'>Ack</a></div>";

        this.addNotification(new Notif("a", NotifType.INFO, new Date(), messageHtml));
        this.addNotification(new Notif("b", NotifType.WARNING, new Date(), messageHtml));
        this.addNotification(new Notif("c", NotifType.ERROR, new Date(), messageHtml));
    }
}

// Specifies the notification phrases.
// Can be changed by a page script.
var NotifPhrases = {
    NoNotif: "No notifications",
    Mute: "Mute",
    Unmute: "Unmute",
    AckAll: "Ack All"
}

// Specifies the notification types.
class NotifType {
    static INFO = 0;
    static WARNING = 1;
    static ERROR = 2
};

// Represents a notification.
class Notif {
    constructor(key, notifType, timestamp, messageHtml) {
        this.id = 0;                // assigned by a panel
        this.key = key;             // assigned by a plugin
        this.notifType = notifType;
        this.timestamp = timestamp; // can be Date or String
        this.messageHtml = messageHtml;
    }
}
