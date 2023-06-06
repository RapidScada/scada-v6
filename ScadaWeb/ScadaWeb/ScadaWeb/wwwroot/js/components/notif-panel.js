// Contains notification classes.
// Depends on jquery, scada-common.js

// Represents a panel that contains notifications.
class NotifPanel {
    constructor(panelID, ...bellIDs) {
        // The storage key for muting.
        this._MUTE_KEY = "NotifPanel.Mute";

        // The jQuery object that represents the mute button.
        this._muteBtn = $();
        // The jQuery object that represents the acknowledge all button.
        this._ackAllBtn = $();
        // The jQuery object that represents an empty notification.
        this._emptyNotifElem = $();
        // The jQuery objects that represent elements to play sounds.
        this._audio = {
            info: null,
            warning: null,
            error: null
        };
        // The notification ID counter.
        this._lastNotifID = 0;
        // The highest notification type of the existing notifications.
        this._notifType = null;
        // The notification counters accessed by notification type.
        this._notifCounters = [];

        // The jQuery object that represents the notification panel.
        this.panelElem = $("#" + panelID);
        // The jQuery object that represents the notification bells.
        this.bellElem = bellIDs ? $("#" + bellIDs.join(", #")) : $();
    }

    // Determines whether the notification panel is visible.
    get _isVisible() {
        return !this.panelElem.hasClass("hidden");
    }

    // Determines whether the notification panel contains any notifications.
    get _isEmpty() {
        return this.panelElem.children(".notif:not(.empty):first").length === 0;
    }

    // Determines whether sound is muted.
    get _isMuted() {
        return ScadaUtils.getStorageItem(sessionStorage, this._MUTE_KEY, "false") === "true";
    }

    // Gets the jQuery object that represents the acknowledge all button.
    get ackAllBtn() {
        return this._ackAllBtn;
    }

    // Binds events to the DOM elements.
    _bindEvents() {
        let thisObj = this;

        this._muteBtn
            .off()
            .on("click", function () {
                if (thisObj._isMuted) {
                    thisObj._unmute();
                } else {
                    thisObj._mute();
                }
            });

        this.bellElem
            .off()
            .on("click", function () {
                thisObj._toggle();
            });
    }

    // Shows the notification panel.
    _show(opt_animate) {
        if (!this._isVisible) {
            this.panelElem.removeClass("hidden");

            if (opt_animate) {
                this.panelElem.css("right", -this.panelElem.outerWidth());
                this.panelElem.animate({ right: 0 }, "fast");
            }
        }
    }

    // Hides the notification panel.
    _hide() {
        this.panelElem.addClass("hidden");
    }

    // Shows or hides the notification panel.
    _toggle() {
        if (this._isVisible) {
            this._hide();
        } else {
            this._show();
        }
    }

    // Sets the bell style, plays or stops a sound, and shows or hides the notification panel.
    _alarmOnOff() {
        this.bellElem.removeClass("info warning error");
        let bellIcon = this.bellElem.find("i:first");
        bellIcon.removeClass("far fas");
        let showPanel = true;

        switch (this._notifType) {
            case NotifType.INFO:
                this.bellElem.addClass("info");
                bellIcon.addClass("fas");
                this._playInfoSound();
                break;

            case NotifType.WARNING:
                this.bellElem.addClass("warning");
                bellIcon.addClass("fas");
                this._playWarningSound();
                break;

            case NotifType.ERROR:
                this.bellElem.addClass("error");
                bellIcon.addClass("fas");
                this._playErrorSound();
                break;

            default:
                bellIcon.addClass("far");
                this._stopSounds();
                showPanel = false;
                break;
        }

        if (showPanel) {
            this._show(true);
        } else {
            this._hide();
        }
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
                return "<i class='fas fa-info info'></i>";
            case NotifType.WARNING:
                return "<i class='fas fa-exclamation-triangle warning'></i>";
            case NotifType.ERROR:
                return "<i class='fas fa-exclamation-circle error'></i>";
            default:
                return "";
        }
    }

    // Mutes notification sound.
    _mute() {
        ScadaUtils.setStorageItem(sessionStorage, this._MUTE_KEY, "true");
        this._stopSounds();
        this._displayMuteState(true);
    }

    // Unmutes notification sound.
    _unmute() {
        ScadaUtils.setStorageItem(sessionStorage, this._MUTE_KEY, "false");
        this._continueSounds();
        this._displayMuteState(false);
    }

    // Stops all sounds.
    _stopSounds() {
        this._audio.warning[0].pause();
        this._audio.error[0].pause();
    }

    // Continues to play sounds if needed.
    _continueSounds() {
        if (this._notifType === NotifType.WARNING) {
            this._playWarningSound();
        } else if (this._notifType === NotifType.ERROR) {
            this._playErrorSound();
        }
    }

    // Plays the information sound.
    _playInfoSound() {
        this._audio.warning[0].pause();
        this._audio.error[0].pause();

        if (!this._isMuted) {
            ScadaUtils.playSound(this._audio.info);
        }
    }

    // Plays the warning sound.
    _playWarningSound() {
        this._audio.error[0].pause();

        if (!this._isMuted) {
            ScadaUtils.playSound(this._audio.warning);
        }
    }

    // Plays the error sound.
    _playErrorSound() {
        this._audio.warning[0].pause();

        if (!this._isMuted) {
            ScadaUtils.playSound(this._audio.error);
        }
    }

    // Updates the mute button according to the mute state.
    _displayMuteState(isMuted) {
        if (isMuted) {
            this._muteBtn.children("i").removeClass("fa-toggle-on").addClass("fa-toggle-off");
            this._muteBtn.children("span").text(notifPhrases.Unmute);
        } else {
            this._muteBtn.children("i").removeClass("fa-toggle-off").addClass("fa-toggle-on");
            this._muteBtn.children("span").text(notifPhrases.Mute);
        }
    }

    // Updates the elements depending on whether notifications exist or not.
    _displayEmptyState(isEmpty) {
        if (isEmpty) {
            this._ackAllBtn.addClass("disabled");
            this._emptyNotif.prependTo(this.panelElem);
        } else {
            this._ackAllBtn.removeClass("disabled");
            this._emptyNotifElem.detach();
        }
    }

    // Increases a notification counter corresponding to the specified type.
    _incNotifCounter(notifType) {
        this._notifCounters[notifType]++;

        if (this._notifType === null || this._notifType < notifType) {
            this._notifType = notifType;
        }
    }

    // Decreases a notification counter corresponding to the specified type.
    _decNotifCounter(notifType) {
        if (this._notifCounters[notifType] > 0) {
            this._notifCounters[notifType]--;
        }

        if (this._notifCounters[NotifType.ERROR] > 0) {
            this._notifType = NotifType.ERROR;
        } else if (this._notifCounters[NotifType.WARNING] > 0) {
            this._notifType = NotifType.WARNING;
        } else if (this._notifCounters[NotifType.INFO] > 0) {
            this._notifType = NotifType.INFO;
        } else {
            this._notifType = null;
        }
    }

    // Resets the notification counters.
    _resetNotifCounters() {
        this._notifCounters[NotifType.INFO] = 0;
        this._notifCounters[NotifType.WARNING] = 0;
        this._notifCounters[NotifType.ERROR] = 0;
        this._notifType = null;
    }

    // Gets the key of the last notification.
    _getLastNotifKey() {
        let lastNotifElem = this.panelElem.children(".notif:first");
        return lastNotifElem.length > 0 ? lastNotifElem.data("notif").key : null;
    }

    // Prepares the notification panel for work.
    prepare(rootPath) {
        let toolbarElem = $("<div class='notif-toolbar'></div>").appendTo(this.panelElem);

        this._muteBtn = $("<div class='notif-tool-btn'><i class='fas fa-toggle-on'></i><span></span></div>");
        this._displayMuteState(this._isMuted);
        toolbarElem.append(this._muteBtn);

        this._ackAllBtn = $("<div class='notif-tool-btn disabled'><i class='fas fa-check-double'></i><span></span></div>");
        this._ackAllBtn.children("span:first").text(notifPhrases.AckAll);
        toolbarElem.append(this._ackAllBtn);

        this._emptyNotifElem = $("<div class='notif empty'></div>")
            .text(notifPhrases.NoNotif)
            .appendTo(this.panelElem);

        if (ScadaUtils.isSmallScreen) {
            this.panelElem.addClass("mobile");
        }

        let soundPath = rootPath + "sounds/";
        this._audio.info = $(`<audio preload src="${soundPath}notif-info.mp3" />`).appendTo(this.panelElem);
        this._audio.warning = $(`<audio preload loop src="${soundPath}notif-warning.mp3" />`).appendTo(this.panelElem);
        this._audio.error = $(`<audio preload loop src="${soundPath}notif-error.mp3" />`).appendTo(this.panelElem);

        this._bindEvents();
    }

    // Adds the notification to the notification panel. Returns the notification ID.
    addNotification(notif) {
        notif.id = ++this._lastNotifID;
        this._displayEmptyState(false);
        this.panelElem.prepend(this._createNotifElem(notif));
        this._incNotifCounter(notif.notifType);
        this._alarmOnOff();
        return notif.id;
    }

    // Adds the collection of notifications to the notification panel.
    addNotifications(notifs) {
        if (notifs.length > 0) {
            this._displayEmptyState(false);
            let lastNotifKey = this._getLastNotifKey();

            for (let notif of notifs) {
                if (lastNotifKey === null || notif.key > lastNotifKey) {
                    notif.id = ++this._lastNotifID;
                    this.panelElem.prepend(this._createNotifElem(notif));
                    this._incNotifCounter(notif.notifType);
                }
            }

            this._alarmOnOff();
        }
    }

    // Adds sample notifications.
    addSamples() {
        const messageHtml =
            "<div>Notification text</div>" +
            "<div><a href='#' class='notif-btn'>Info</a><a href='#' class='notif-btn'>Ack</a></div>";

        this.addNotifications([
            new Notif("a", NotifType.INFO, new Date(), messageHtml),
            new Notif("b", NotifType.WARNING, new Date(), messageHtml),
            new Notif("c", NotifType.ERROR, new Date(), messageHtml)
        ]);
    }

    // Removes the notification with the specified ID from the notification panel.
    removeNotification(notifID) {
        let notifElem = this.panelElem.children("#notif_" + notifID);
        let notif = notifElem.data("notif");
        notifElem.remove();
        this._displayEmptyState(this._isEmpty);

        if (notif) {
            this._decNotifCounter(notif.notifType);
            this._alarmOnOff();
        }
    }

    // Removes all notifications from the notification panel.
    clearNotifications() {
        this.panelElem.children(".notif:not(.empty)").remove();
        this._displayEmptyState(true);
        this._resetNotifCounters();
        this._alarmOnOff();
    }
}

// Specifies the notification phrases.
// Can be changed by a page script.
// C# naming style. 
var notifPhrases = {
    NoNotif: "No notifications",
    Mute: "Mute",
    Unmute: "Unmute",
    AckAll: "Ack All"
};

// Specifies the notification types.
class NotifType {
    static INFO = 0;
    static WARNING = 1;
    static ERROR = 2
}

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
