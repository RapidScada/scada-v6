// Contains notification classes.
// Depends on jquery, scada-common.js

// Represents a panel that contains notifications.
class NotifPanel {
    // The storage key for muting.
    static _MUTE_KEY = "NotifPanel.Mute";
    // An event that occurs when the Ack All button is clicked.
    static ACK_ALL_EVENT = "ackall.rs.notif";

    // The jQuery object that represents the mute button.
    _muteBtn = $();
    // The jQuery object that represents the acknowledge all button.
    _ackAllBtn = $();
    // The jQuery object that displays the waiting state.
    _spinnerElem = $();
    // The jQuery object that represents an empty notification.
    _emptyNotifElem = $();
    // The jQuery object that contains notifications.
    _notifContainerElem = $();
    // The jQuery objects that represent elements to play sounds.
    _audio = { info: null, warning: null, error: null };
    // The highest severity of the existing notifications.
    _highestSeverity = Severity.UNDEFINED;
    // The notification counters accessed by known severity.
    _notifCounters = [];
    // The jQuery object that represents the notification panel.
    panelElem;
    // The jQuery object that represents the notification bells.
    bellElem;

    constructor(panelID, ...bellIDs) {
        this.panelElem = $("#" + panelID);
        this.bellElem = bellIDs ? $("#" + bellIDs.join(", #")) : $();
    }

    // Determines whether the notification panel is visible.
    get _isVisible() {
        return !this.panelElem.hasClass("hidden");
    }

    // Determines whether the notification panel contains any notifications.
    get _isEmpty() {
        return this._notifContainerElem.children(".notif:first").length === 0;
    }

    // Determines whether sound is muted.
    get _isMuted() {
        return ScadaUtils.getStorageItem(sessionStorage, NotifPanel._MUTE_KEY, "false") === "true";
    }

    // Binds events to the DOM elements.
    _bindEvents() {
        const thisObj = this;

        this._muteBtn
            .off()
            .on("click", function () {
                if (thisObj._isMuted) {
                    thisObj._unmute();
                } else {
                    thisObj._mute();
                }
            });

        this._ackAllBtn
            .off()
            .on("click", function () {
                if (!thisObj._ackAllBtn.hasClass("disabled")) {
                    thisObj.panelElem.trigger(NotifPanel.ACK_ALL_EVENT);
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
        this.bellElem.removeClass("critical major minor info");
        let bellIcon = this.bellElem.find("i:first");
        bellIcon.removeClass("fa-regular fa-solid");
        let showPanel = true;

        switch (this._highestSeverity) {
            case Severity.CRITICAL:
                this.bellElem.addClass("critical");
                bellIcon.addClass("fa-solid");
                this._playErrorSound();
                break;

            case Severity.MAJOR:
                this.bellElem.addClass("major");
                bellIcon.addClass("fa-solid");
                this._playWarningSound();
                break;

            case Severity.MINOR:
                this.bellElem.addClass("minor");
                bellIcon.addClass("fa-solid");
                this._playWarningSound();
                break;

            case Severity.INFO:
                this.bellElem.addClass("info");
                bellIcon.addClass("fa-solid");
                this._playInfoSound();
                break;

            default:
                bellIcon.addClass("fa-regular");
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
        let notifElem = $(`<div id='${this._getNotifElemID(notif.key)}' class='notif'></div>`).data("notif", notif);
        this._getNotifIconElem(notif.knownSeverity).appendTo(notifElem);

        if (notif.timestamp) {
            let time = notif.timestamp instanceof Date ? notif.timestamp.toLocaleString() : notif.timestamp;
            $(`<div class='notif-time'>${time}</div>`).appendTo(notifElem);
        }

        let messageElem = $("<div class='notif-msg'></div>").appendTo(notifElem);

        if (notif.message instanceof jQuery) {
            messageElem.append(notif.message);
        } else if (notif.isHtml) {
            messageElem.html(notif.message);
        } else {
            messageElem.text(notif.message);
        }

        notifElem.data("notif", notif);
        return notifElem;
    }

    // Gets the element ID corresponding to the notification key.
    _getNotifElemID(key) {
        return "notif_" + key;
    }

    // Gets a notification icon jQuery element corresponding to the severity.
    _getNotifIconElem(knownSeverity) {
        switch (knownSeverity) {
            case Severity.CRITICAL:
                return $("<div class='notif-icon critical'><i class='fa-solid fa-circle-exclamation'></i></div>");

            case Severity.MAJOR:
                return $("<div class='notif-icon major'><i class='fa-solid fa-triangle-exclamation'></i></div>");

            case Severity.MINOR:
                return $("<div class='notif-icon minor'><i class='fa-solid fa-triangle-exclamation'></i></div>");

            case Severity.INFO:
                return $("<div class='notif-icon info'><i class='fa-solid fa-info'></i></div>");

            default:
                return $("<div class='notif-icon undef'><i class='fa-regular fa-circle'></i></div>");
        }
    }

    // Mutes notification sound.
    _mute() {
        ScadaUtils.setStorageItem(sessionStorage, NotifPanel._MUTE_KEY, "true");
        this._stopSounds();
        this._displayMuteState(true);
    }

    // Unmutes notification sound.
    _unmute() {
        ScadaUtils.setStorageItem(sessionStorage, NotifPanel._MUTE_KEY, "false");
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
        if (this._highestSeverity === Severity.MINOR ||
            this._highestSeverity === Severity.MAJOR) {
            this._playWarningSound();
        } else if (this._highestSeverity === Severity.CRITICAL) {
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
            this._muteBtn.children("span").text(notifPhrases.unmute);
        } else {
            this._muteBtn.children("i").removeClass("fa-toggle-off").addClass("fa-toggle-on");
            this._muteBtn.children("span").text(notifPhrases.mute);
        }
    }

    // Updates the elements depending on whether notifications exist or not.
    _displayEmptyState(isEmpty) {
        if (isEmpty) {
            this._ackAllBtn.addClass("disabled");
            this._emptyNotifElem.prependTo(this.panelElem);
        } else {
            this._ackAllBtn.removeClass("disabled");
            this._emptyNotifElem.detach();
        }
    }

    // Increases a notification counter corresponding to the notification severity.
    _incNotifCounter(notif) {
        let knownSeverity = notif.knownSeverity;
        this._notifCounters[knownSeverity]++;

        if (knownSeverity !== Severity.UNDEFINED &&
            (this._highestSeverity === Severity.UNDEFINED || this._highestSeverity > knownSeverity)) {
            this._highestSeverity = knownSeverity;
        }
    }

    // Decreases a notification counter corresponding to the notification severity.
    _decNotifCounter(notif) {
        let knownSeverity = notif.knownSeverity;

        if (this._notifCounters[knownSeverity] > 0) {
            this._notifCounters[knownSeverity]--;
        }

        if (this._notifCounters[Severity.CRITICAL] > 0) {
            this._highestSeverity = Severity.CRITICAL;
        } else if (this._notifCounters[Severity.MAJOR] > 0) {
            this._highestSeverity = Severity.MAJOR;
        } else if (this._notifCounters[Severity.MINOR] > 0) {
            this._highestSeverity = Severity.MINOR;
        } else if (this._notifCounters[Severity.INFO] > 0) {
            this._highestSeverity = Severity.INFO;
        } else {
            this._highestSeverity = Severity.UNDEFINED;
        }
    }

    // Resets the notification counters.
    _resetNotifCounters() {
        this._notifCounters[Severity.CRITICAL] = 0;
        this._notifCounters[Severity.MAJOR] = 0;
        this._notifCounters[Severity.MINOR] = 0;
        this._notifCounters[Severity.INFO] = 0;
        this._highestSeverity = Severity.UNDEFINED;
    }

    // Calculates the notification counters.
    _calcNotifCounters(notifs) {
        this._resetNotifCounters();

        for (let notif of notifs) {
            this._incNotifCounter(notif);
        }
    }

    // Prepares the notification panel for work.
    prepare(rootPath) {
        let toolbarElem = $("<div class='notif-toolbar'></div>").appendTo(this.panelElem);

        this._muteBtn = $("<div class='notif-tool-btn'><i class='fa-solid fa-toggle-on'></i><span></span></div>");
        this._displayMuteState(this._isMuted);
        toolbarElem.append(this._muteBtn);

        this._ackAllBtn = $("<div class='notif-tool-btn disabled'>" +
            "<i class='fa-solid fa-check-double'></i><span></span></div>");
        this._ackAllBtn.children("span:first").text(notifPhrases.ackAll);
        toolbarElem.append(this._ackAllBtn);

        this._spinnerElem = $("<div class='notif-tool-item hidden'>" +
            "<i class='fa-solid fa-spinner fa-spin-pulse'></i></div>");
        toolbarElem.append(this._spinnerElem);

        this._emptyNotifElem = $("<div class='notif empty'></div>")
            .text(notifPhrases.noNotif)
            .appendTo(this.panelElem);

        this._notifContainerElem = $("<div class='notif-container'></div>")
            .appendTo(this.panelElem);

        if (ScadaUtils.isSmallScreen) {
            this.panelElem.addClass("mobile");
        }

        let soundPath = rootPath + "sounds/";
        this._audio.info = $(`<audio preload src="${soundPath}notif-info.mp3" />`).appendTo(this.panelElem);
        this._audio.warning = $(`<audio preload loop src="${soundPath}notif-warning.mp3" />`).appendTo(this.panelElem);
        this._audio.error = $(`<audio preload loop src="${soundPath}notif-error.mp3" />`).appendTo(this.panelElem);

        this._bindEvents();
        this._resetNotifCounters();
    }

    // Adds the notification to the notification panel.
    addNotification(notif) {
        this._displayEmptyState(false);
        this._notifContainerElem.prepend(this._createNotifElem(notif));
        this._incNotifCounter(notif);
        this._alarmOnOff();
    }

    // Deletes the existing notifications and adds the specified notifications.
    replaceNotifications(notifs) {
        if (!this._isEmpty || notifs.length > 0) {
            this._notifContainerElem.empty();
            this._notifContainerElem.append(Array.from(notifs, n => this._createNotifElem(n)).reverse());
            this._displayEmptyState(this._isEmpty);
            this._calcNotifCounters(notifs);
            this._alarmOnOff();
        }
    }

    // Adds sample notifications.
    addSamples() {
        const message =
            "<div>Notification text</div>" +
            "<div><span class='notif-btn'>Info</span><span class='notif-btn'>Ack</span></div>";

        this.replaceNotifications([
            new Notif("a", Severity.INFO, new Date(), message, true),
            new Notif("b", Severity.MINOR, new Date(), message, true),
            new Notif("c", Severity.MAJOR, new Date(), message, true),
            new Notif("d", Severity.CRITICAL, new Date(), message, true)
        ]);
    }

    // Removes the notification with the specified key from the notification panel.
    removeNotification(notifKey) {
        let notifElem = this._notifContainerElem.children("#" + this._getNotifElemID(notifKey));

        if (notifElem.length > 0) {
            let notif = notifElem.data("notif");
            notifElem.remove();
            this._displayEmptyState(this._isEmpty);

            if (notif) {
                this._decNotifCounter(notif);
                this._alarmOnOff();
            }
        }
    }

    // Removes all notifications from the notification panel.
    clearNotifications() {
        if (!this._isEmpty) {
            this._notifContainerElem.empty();
            this._displayEmptyState(true);
            this._resetNotifCounters();
            this._alarmOnOff();
        }
    }

    // Shows or hides the wait symbol.
    displayWaitingState(isWaiting) {
        if (isWaiting) {
            this._spinnerElem.removeClass("hidden");
        } else {
            this._spinnerElem.addClass("hidden");
        }
    }
}

// Specifies the notification phrases.
// Can be changed by a page script.
var notifPhrases = {
    noNotif: "No notifications",
    mute: "Mute",
    unmute: "Unmute",
    ackAll: "Ack All"
};

// Represents a notification.
class Notif {
    constructor(key, severity, timestamp, message, opt_isHtml) {
        this.key = key;                    // unique notification key assigned by a plugin
        this.severity = severity;          // severity as a number
        this.timestamp = timestamp;        // can be Date, String or empty value
        this.message = message;            // can be plain string, HTML string or jQuery object
        this.isHtml = opt_isHtml ?? false; // message is HTML
        this.knownSeverity = Severity.closest(severity);
    }
}
