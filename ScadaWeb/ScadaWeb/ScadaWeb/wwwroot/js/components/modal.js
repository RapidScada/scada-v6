// Contains classes: ModalButton, ModalSize, ModalOptions, ModalManager
// Depends on jquery, bootstrap, scada-common.js

// Specifies the modal dialog buttons.
class ModalButton {
    static OK = "Ok";
    static YES = "Yes";
    static NO = "No";
    static EXEC = "Execute";
    static CANCEL = "Cancel";
    static CLOSE = "Close";

    static OK_CANCEL = [ModalButton.OK, ModalButton.CANCEL];
    static YES_NO_CANCEL = [ModalButton.YES, ModalButton.NO, ModalButton.CANCEL];
    static EXEC_CLOSE = [ModalButton.EXEC, ModalButton.CLOSE];
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
    modalResult = null;
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
    _setupModalDoc(modalWnd) {
        const thisObj = this;
        let modalDoc = modalWnd.$(modalWnd.document);

        modalDoc.ready(function () {
            // set the modal title
            thisObj.setTitle(modalWnd, modalWnd.document.title);

            // remove the modal on press the Escape key
            modalDoc
                .off("keydown.rs.modal")
                .on("keydown.rs.modal", function (event) {
                    if (event.which === 27 /*Escape*/) {
                        let modalElem = ModalManager._getModalElem(modalWnd);
                        ModalManager._getModalObject(modalElem).hide();
                    }
                });

            // handles the modal postback
            let postbackArgs = modalWnd.modalPostbackArgs;

            if (postbackArgs) {
                if (postbackArgs.closeModal) {
                    setTimeout(function () {
                        thisObj.closeModal(modalWnd, postbackArgs.modalResult);
                    }, postbackArgs.closeDelay);
                } else if (postbackArgs.updateHeight) {
                    thisObj.updateModalHeight(modalWnd, postbackArgs.growOnly);
                }
            }
        });
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

    // Gets the window object of the frame contained in the specified modal element.
    static _getModalWnd(modalElem) {
        return modalElem.find("iframe:first")[0].contentWindow;
    }

    // Gets a bootstrap modal instance associated with the jQuery object.
    static _getModalObject(modalElem) {
        return bootstrap.Modal.getOrCreateInstance(modalElem[0]);
    }

    // Gets the modal element that contains the specified window object.
    static _getModalElem(modalWnd) {
        return $(modalWnd.frameElement).closest(".modal");
    }

    // Submits the modal form.
    static _submitModal(modalWnd, modalValue) {
        modalWnd.$(".rs-modal-value:first").val(modalValue);
        let submitElem = modalWnd.$(".rs-modal-submit:first");

        if (submitElem.length > 0) {
            submitElem.click();
        } else {
            modalWnd.$("form:first").submit();
        }
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
        return ModalManager._getModalElem(modalWnd)
            .find(".modal-footer button[data-rs-value='" + buttonValue + "']");
    }

    // Truncates the title if it is too long.
    static _truncateTitle(s) {
        return s.length <= ModalManager.MAX_TITLE_LEN ? s : s.substr(0, ModalManager.MAX_TITLE_LEN) + "…";
    }

    // Determines if the specified window is a modal dialog.
    isModal(wnd) {
        return $(wnd.frameElement).closest(".modal").length > 0;
    }

    // Opens the modal dialog containing the specified page.
    // opt_callback is a function (result)
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
                .data("rs-result", null);
        }

        // create a frame
        let modalFrame = $("<iframe class='rs-modal-frame'></iframe>").css({
            "position": "fixed",
            "opacity": 0.0 // hide the frame while it's loading
        });

        let modalBody = modalElem.find(".modal-body");
        modalBody.append(modalFrame);
        $("body").append(modalElem);

        // bind events to the modal
        modalElem
            .on('shown.bs.modal', function () {
                // update the modal height
                let frameWnd = ModalManager._getModalWnd(modalElem);
                if (ScadaUtils.checkAccessToFrame(frameWnd, true) && !options.height) {
                    let frameBody = frameWnd.$("body");
                    modalFrame.css("height", frameBody.outerHeight(true));
                }

                tempOverlay.remove();
                modalFrame.focus();
            })
            .on('hidden.bs.modal', function () {
                let callback = $(this).data("rs-callback");

                if (typeof callback === "function") {
                    callback($(this).data("rs-result"));
                }

                $(this).remove();
            });

        modalElem.find(".modal-footer button").click(function () {
            // raise event
            let buttonValue = $(this).data("rs-value");
            let frameWnd = ModalManager._getModalWnd(modalElem);
            frameWnd.$(frameWnd).trigger(ScadaEventType.MODAL_BTN_CLICK, buttonValue);

            // submit the modal
            if ($(this).hasClass("rs-btn-submit")) {
                ModalManager._submitModal(frameWnd, buttonValue);
            }
        });

        // load the frame
        modalFrame
            .on("load", function () {
                let frameWnd = ModalManager._getModalWnd(modalElem);

                if (ScadaUtils.checkAccessToFrame(frameWnd, true)) {
                    // setup the modal document
                    thisObj._setupModalDoc(frameWnd);
                } else {
                    // set the modal title
                    modalElem.find(".modal-title").text(url);
                }
            })
            .one("load", function () {
                // update the modal style
                modalFrame.css({
                    "width": "100%",
                    "position": "",
                    "opacity": 1.0
                });

                // set the modal height if specified
                if (options.height) {
                    modalFrame.css("height", options.height);
                }

                // display the modal
                ModalManager._getModalObject(modalElem).show();
            })
            .attr("src", url);
    }

    // Closes the modal dialog with the specified result.
    closeModal(modalWnd, result) {
        let modalElem = this.setResult(modalWnd, result);
        ModalManager._getModalObject(modalElem).hide();
    }

    // Sets the result of the modal dialog, keeping it open. Returns a jQuery object represents the modal dialog.
    setResult(modalWnd, result) {
        return ModalManager._getModalElem(modalWnd)
            .data("rs-result", result);
    }

    // Sets the title of the modal dialog.
    setTitle(modalWnd, title) {
        ModalManager._getModalElem(modalWnd)
            .find(".modal-title")
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
            let modalElem = ModalManager._getModalElem(modalWnd);
            ModalManager._getModalObject(modalElem).handleUpdate();
        }
    }

    // Finds an existing or create a new manager instance.
    static getInstance() {
        return ModalManager._findInstance() || new ModalManager();
    }
}
