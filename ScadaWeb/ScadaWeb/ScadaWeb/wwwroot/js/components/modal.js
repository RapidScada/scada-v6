// Contains classes: ModalButton, ModalSizes, ModalOptions, ModalManager
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
        this.buttons = opt_buttons || [ModalButton.OK, ModalButton.CANCEL];
        this.size = opt_size || ModalSizes.NORMAL;
        this.height = opt_height || 0;
    }
}

// Specifies the modal phrases.
// Can be changed by a page script.
var modalPhrases = {};

// Manages modal dialogs.
class ModalManager {
    static MAX_TITLE_LEN = 50;

    // Builds an HTML markup of a modal dialog footer buttons.
    static _buildButtonsHtml(buttons) {
        let html = "";

        for (let btn of buttons) {
            let subclass = btn === ModalButton.OK || btn === ModalButton.YES
                ? "btn-primary"
                : (btn === ModalButton.EXEC ? "btn-danger" : "btn-default");
            let dismiss = btn === ModalButton.CANCEL || btn === ModalButton.CLOSE ? "data-bs-dismiss='modal'" : "";
            let caption = modalPhrases[btn] || btn;

            html += `<button type='button' class='btn ${subclass}' data-rs-value='${btn}' ${dismiss}>${caption}</button>`;
        }

        return html;
    }

    // Finds a modal button by its value.
    static _findButton(modalWnd, buttonValue) {
        return $(modalWnd.frameElement).closest(".modal")
            .find(".modal-footer button[data-rs-value='" + buttonValue + "']");
    };

    // Gets a bootstrap modal instance associated with the jQuery object.
    static _getModalObject(modalElem) {
        return bootstrap.Modal.getInstance(modalElem[0]);
    }

    // Truncates the title if it is too long.
    static _truncateTitle(s) {
        return s.length <= ModalManager.MAX_TITLE_LEN ? s : s.substr(0, ModalManager.MAX_TITLE_LEN) + "…";
    };

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

    // Opens the modal dialog containing the specified page.
    // opt_callback is a function (result, args)
    showModal(url, opt_options, opt_callback) {
        // create temporary overlay to prevent user activity
        let tempOverlay = $("<div class='rs-modal-overlay'></div>");
        $("body").append(tempOverlay);

        // create the modal
        let footerHtml = opt_options && Array.isArray(opt_options.buttons) ?
            "<div class='modal-footer'>" + ModalManager._buildButtonsHtml(opt_options.buttons) + "</div>" : "";
        let size = opt_options ? opt_options.size : ModalSize.NORMAL;
        let sizeClass = "";

        if (size === ModalSize.SMALL) {
            sizeClass = " modal-sm";
        } else if (size === ModalSize.LARGE) {
            sizeClass = " modal-lg";
        } else if (size === ModalSize.EXTRA_LARGE) {
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
        let modalObj = ModalManager._getModalObject(modalElem);

        if (opt_callback) {
            modalElem
                .data("rs-callback", opt_callback)
                .data("rs-result", false);
        }

        // create the frame
        let modalFrame = $("<iframe class='rs-modal-frame'></iframe>").css({
            "position": "fixed",
            "opacity": 0.0 // hide the frame while it's loading
        });

        let modalBody = modalElem.find(".modal-body");
        modalBody.append(modalFrame);
        $("body").append(modalElem);

        // create a function that hides the modal on press Escape key
        let hideModalOnEscapeFunc = function (event) {
            if (event.which === 27 /*Escape*/) {
                modalObj.hide();
            }
        };

        // load the frame
        modalFrame
            .on("load", function () {
                // remove the modal on press Escape key in the frame
                let frameWnd = modalFrame[0].contentWindow;
                if (ScadaUtils.checkAccessToFrame(frameWnd) && frameWnd.$) {
                    let jqFrameDoc = frameWnd.$(frameWnd.document);
                    jqFrameDoc.ready(function () {
                        jqFrameDoc
                            .off("keydown.rs.modal", hideModalOnEscapeFunc)
                            .on("keydown.rs.modal", hideModalOnEscapeFunc);
                    });
                }
            })
            .one("load", function () {
                // get the frame size
                let frameBody = modalFrame.contents().find("body");
                let frameWidth = frameBody.outerWidth(true);
                let frameHeight = frameBody.outerHeight(true);
                let specifiedHeight = opt_options ? opt_options.height : 0;

                // tune the modal
                let frameWnd = modalFrame[0].contentWindow;
                let frameAccessible = ScadaUtils.checkAccessToFrame(frameWnd);
                let modalPaddings = parseInt(modalBody.css("padding-left")) + parseInt(modalBody.css("padding-right"));
                modalElem.find(".modal-content").css("min-width", frameWidth + modalPaddings);
                modalElem.find(".modal-title").text(
                    ModalManager._truncateTitle(frameAccessible ? frameWnd.document.title : url));

                // set the frame style
                modalFrame.css({
                    "width": "100%",
                    "height": specifiedHeight || frameHeight,
                    "position": "",
                    "opacity": 1.0
                });

                // raise event on modal button click
                if (frameAccessible && frameWnd.$) {
                    modalElem.find(".modal-footer button").click(function () {
                        let buttonValue = $(this).data("rs-value");
                        if (buttonValue) {
                            frameWnd.$(frameWnd).trigger(ScadaEventType.MODAL_BTN_CLICK, buttonValue);
                        }
                    });
                }

                // display the modal
                modalElem
                    .on('shown.bs.modal', function () {
                        if (!specifiedHeight) {
                            modalFrame.css("height", frameBody.outerHeight(true)); // final update of the height
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

                modalObj.show();
            })
            .attr("src", url);
    }

    // Closes the modal dialog with the specified result.
    closeModal(modalWnd, result, args) {
        let modalElem = this.setResult(modalWnd, result, args);
        ModalManager._getModalObject(modalElem).hide();
    }

    // Updates the modal dialog height according to its frame height.
    updateModalHeight(modalWnd) {
        let frame = $(modalWnd.frameElement);
        let frameBody = frame.contents().find("body");
        let modalElem = frame.closest(".modal");

        //let iosScrollFix = ScadaUtils.iOS;
        //if (iosScrollFix) {
        //    modalElem.css("overflow-y", "hidden");
        //}

        frame.css("height", frameBody.outerHeight(true));

        //if (iosScrollFix) {
        //    modalElem.css("overflow-y", "");
        //}

        ModalManager._getModalObject(modalElem).handleUpdate();
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

    // Finds an existing or create a new manager instance.
    static getInstance() {
        return ModalManager._findInstance() || new ModalManager();
    }
}
