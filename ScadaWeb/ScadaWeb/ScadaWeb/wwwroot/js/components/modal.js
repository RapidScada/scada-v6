// Contains classes: ModalButton, ModalSize, ModalOptions, ModalManager
// Depends on jquery, bootstrap, scada-common.js

// Specifies the buttons available for a modal dialog.
class ModalButton {
    static OK = "Ok";
    static YES = "Yes";
    static NO = "No";
    static EXEC = "Execute";
    static CANCEL = "Cancel";
    static CLOSE = "Close";
};

// Specifies the sizes of a modal dialog.
class ModalSize {
    static NORMAL = 0;
    static SMALL = 1;
    static LARGE = 2;
    static EXTRA_LARGE = 3;
};

// Represents modal dialog options.
class ModalOptions {
    constructor(opt_buttons, opt_size, opt_height) {
        this.buttons = opt_buttons || [ModalButton.CLOSE];
        this.size = opt_size || ModalSize.NORMAL;
        this.height = opt_height || 0;
    }
}

// Describes modal dialog behavior after postback.
class ModalPostbackArgs {
    closeModal = false;
    closeDelay = 0;
    modalResult = false;
    resultArgs = null;
    updateHeight = false;
    growOnly = false;
}

// Specifies the modal phrases.
// Can be changed by a page script.
var modalPhrases = {};

// Manages modal dialogs.
class ModalManager {
    static MAX_TITLE_LEN = 50;

    // Sets up the modal document.
    _setupModalDoc(modalWnd, modalElem) {
        const thisObj = this;
        let modalDoc = modalWnd.$(modalWnd.document);

        modalDoc.ready(function () {
            // prevent scrollbars from appearing because of margins
            modalDoc.find("body").css("overflow", "hidden");

            // remove the modal on press the Escape key
            modalDoc
                .off("keydown.rs.modal")
                .on("keydown.rs.modal", function (event) {
                    if (event.which === 27 /*Escape*/) {
                        ModalManager._getModalObject(modalElem).hide();
                    }
                });

            // handles the modal postback
            if (modalWnd.modalPostbackArgs) {
                let postbackArgs = modalWnd.modalPostbackArgs;

                if (postbackArgs.closeModal) {
                    setTimeout(function () {
                        thisObj.closeModal(modalWnd, postbackArgs.modalResult, postbackArgs.resultArgs);
                    }, postbackArgs.closeDelay);
                } else if (postbackArgs.updateHeight) {
                    thisObj.updateModalHeight(modalWnd, postbackArgs.growOnly);
                }
            }
        });
    }

    // Builds an HTML markup of a modal dialog footer buttons.
    static _buildButtonsHtml(buttons) {
        let html = "";

        for (let btn of buttons) {
            let subclass = btn === ModalButton.OK || btn === ModalButton.YES
                ? "btn-primary"
                : (btn === ModalButton.EXEC ? "btn-danger" : "btn-secondary");
            let dismiss = "";

            if (btn === ModalButton.CANCEL || btn === ModalButton.CLOSE) {
                dismiss = "data-bs-dismiss='modal'";
            } else {
                subclass += " rs-btn-submit";
            }

            let caption = modalPhrases[btn] || btn;
            html += `<button type='button' class='btn ${subclass}' data-rs-value='${btn}' ${dismiss}>${caption}</button>`;
        }

        return html;
    }

    // Finds a modal button by its value.
    static _findButton(modalWnd, buttonValue) {
        return $(modalWnd.frameElement).closest(".modal")
            .find(".modal-footer button[data-rs-value='" + buttonValue + "']");
    }

    // Gets a bootstrap modal instance associated with the jQuery object.
    static _getModalObject(modalElem) {
        return bootstrap.Modal.getOrCreateInstance(modalElem[0]);
    }

    // Truncates the title if it is too long.
    static _truncateTitle(s) {
        return s.length <= ModalManager.MAX_TITLE_LEN ? s : s.substr(0, ModalManager.MAX_TITLE_LEN) + "…";
    }

    // Finds an existing manager instance.
    static _findInstance() {
        let wnd = window;

        while (wnd) {
            if (wnd.modalManager) {
                return wnd.modalManager;
            }

            wnd = wnd === window.top ? null : wnd.parent;
        }

        return null;
    }

    // Determines if the specified window is a modal dialog.
    isModal(wnd) {
        return $(wnd.frameElement).closest(".modal").length > 0;
    }

    // Opens the modal dialog containing the specified page.
    // opt_callback is a function (result, args)
    showModal(url, opt_options, opt_callback) {
        const thisObj = this;

        // create temporary overlay to prevent user activity
        let tempOverlay = $("<div class='rs-modal-overlay'></div>");
        $("body").append(tempOverlay);

        // create a modal
        let options = opt_options || new ModalOptions();
        let footerHtml = Array.isArray(options.buttons) ?
            "<div class='modal-footer'>" + ModalManager._buildButtonsHtml(options.buttons) + "</div>" : "";
        let sizeClass = "";

        if (options.size === ModalSize.SMALL) {
            sizeClass = " modal-sm";
        } else if (options.size === ModalSize.LARGE) {
            sizeClass = " modal-lg";
        } else if (options.size === ModalSize.EXTRA_LARGE) {
            sizeClass = " modal-xl";
        }

        let modalElem = $(
            "<div class='modal fade' tabindex='-1'>" +
            "<div class='modal-dialog" + sizeClass + "'>" +
            "<div class='modal-content'>" +
            "<div class='modal-header'>" +
            "<h5 class='modal-title'></h5>" +
            "<button type='button' class='btn-close' data-bs-dismiss='modal'></button></div>" +
            "<div class='modal-body'></div>" +
            footerHtml +
            "</div></div></div>");

        if (opt_callback) {
            modalElem
                .data("rs-callback", opt_callback)
                .data("rs-result", false);
        }

        // create a frame
        let modalFrame = $("<iframe class='rs-modal-frame'></iframe>").css({
            "position": "fixed",
            "opacity": 0.0 // hide the frame while it's loading
        });

        let modalBody = modalElem.find(".modal-body");
        modalBody.append(modalFrame);
        $("body").append(modalElem);

        // load the frame
        modalFrame
            .on("load", function () {
                // setup the modal document
                let frameWnd = modalFrame[0].contentWindow;

                if (ScadaUtils.checkAccessToFrame(frameWnd) && frameWnd.$) {
                    thisObj._setupModalDoc(frameWnd, modalElem);
                }
            })
            .one("load", function () {
                // update the modal style
                modalFrame.css({
                    "width": "100%",
                    "position": "",
                    "opacity": 1.0
                });

                let frameWnd = modalFrame[0].contentWindow;
                let frameAccessible = ScadaUtils.checkAccessToFrame(frameWnd);
                let frameBody = $();

                if (frameAccessible && frameWnd.$) {
                    // get the actual frame size
                    frameBody = modalFrame.contents().find("body");
                    let frameWidth = frameBody.outerWidth(true);
                    let frameHeight = frameBody.outerHeight(true);

                    // set the modal size
                    let modalPaddings = parseInt(modalBody.css("padding-left")) + parseInt(modalBody.css("padding-right"));
                    modalElem.find(".modal-content").css("min-width", frameWidth + modalPaddings);
                    modalFrame.css("height", options.height || frameHeight);

                    // set the modal title
                    modalElem.find(".modal-title").text(ModalManager._truncateTitle(frameWnd.document.title));

                    // handle button click
                    modalElem.find(".modal-footer button").click(function () {
                        // raise event
                        let buttonValue = $(this).data("rs-value");
                        frameWnd.$(frameWnd).trigger(ScadaEventType.MODAL_BTN_CLICK, buttonValue);

                        // submit the modal
                        if ($(this).hasClass("rs-btn-submit")) {
                            frameBody.find("form .rs-modal-value").val(buttonValue);
                            frameBody.find("form .rs-modal-submit").click();
                        }
                    });
                } else {
                    // set the modal title
                    modalElem.find(".modal-title").text(url);

                    // set the modal height
                    if (options.height) {
                        modalFrame.css("height", options.height);
                    }
                }

                // display the modal
                modalElem
                    .on('shown.bs.modal', function () {
                        // update the modal height
                        if (frameAccessible && !options.height) {
                            modalFrame.css("height", frameBody.outerHeight(true));
                        }

                        tempOverlay.remove();
                        modalFrame.focus();
                    })
                    .on('hidden.bs.modal', function () {
                        let callback = $(this).data("rs-callback");
                        if (typeof callback === "function") {
                            callback($(this).data("rs-result"), $(this).data("rs-args"));
                        }

                        $(this).remove();
                    });

                ModalManager._getModalObject(modalElem).show();
            })
            .attr("src", url);
    }

    // Closes the modal dialog with the specified result.
    closeModal(modalWnd, result, args) {
        let modalElem = this.setResult(modalWnd, result, args);
        ModalManager._getModalObject(modalElem).hide();
    }

    // Sets the result of the modal dialog, keeping it open. Returns a jQuery object represents the modal dialog.
    setResult(modalWnd, result, args) {
        return $(modalWnd.frameElement)
            .closest(".modal")
            .data("rs-result", result)
            .data("rs-args", args);
    }

    // Sets the title of the modal dialog.
    setTitle(modalWnd, title) {
        $(modalWnd.frameElement).closest(".modal").find(".modal-title")
            .text(ModalManager._truncateTitle(title));
    }

    // Shows or hides the button of the modal dialog.
    setButtonVisible(modalWnd, buttonValue, visible) {
        ModalManager._findButton(modalWnd, buttonValue).css("display", visible ? "" : "none");
    }

    // Enables or disables the button of the modal dialog.
    setButtonEnabled(modalWnd, buttonValue, enabled) {
        let btnElem = ModalManager._findButton(modalWnd, buttonValue);

        if (enabled) {
            btnElem.removeAttr("disabled");
        } else {
            btnElem.attr("disabled", "disabled");
        }
    }

    // Updates the modal dialog height according to its frame height.
    updateModalHeight(modalWnd, opt_growOnly) {
        let frame = $(modalWnd.frameElement);
        let frameBody = frame.contents().find("body");
        let newHeight = frameBody.outerHeight(true);

        if (!opt_growOnly || newHeight > frame.height()) {
            frame.css("height", newHeight);
            let modalElem = frame.closest(".modal");
            ModalManager._getModalObject(modalElem).handleUpdate();
        }
    }

    // Finds an existing or create a new manager instance.
    static getInstance() {
        return ModalManager._findInstance() || new ModalManager();
    }
}
