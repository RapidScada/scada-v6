// Contains classes: ModalButton, ModalSize, ModalOptions, ModalBase, ModalManager,
//     ModalBoxOptions, ModalBoxAlerts, ModalBox
// Depends on jquery, bootstrap, scada-common.js

// Specifies the modal dialog buttons.
class ModalButton {
    static OK = "ok";
    static YES = "yes";
    static NO = "no";
    static EXEC = "execute";
    static CANCEL = "cancel";
    static CLOSE = "close";

    static OK_CANCEL = [ModalButton.OK, ModalButton.CANCEL];
    static YES_NO_CANCEL = [ModalButton.YES, ModalButton.NO, ModalButton.CANCEL];
    static EXEC_CLOSE = [ModalButton.EXEC, ModalButton.CLOSE];
}

// Specifies the sizes of a modal dialog.
class ModalSize {
    static NORMAL = 0;
    static SMALL = 1;
    static LARGE = 2;
    static EXTRA_LARGE = 3;
}

// Represents modal dialog options.
class ModalOptions {
    title = null;
    buttons = [ModalButton.CLOSE];
    size = ModalSize.NORMAL;
    height = 0;

    constructor(fields) {
        Object.assign(this, fields);
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

// Contains common functions for creating modals.
class ModalBase {
    // Creates a jQuery element for a modal.
    static createModalElem() {
        // footer not included
        return $(
            "<div class='modal fade' tabindex='-1'>" +
            "<div class='modal-dialog'>" +
            "<div class='modal-content'>" +
            "<div class='modal-header'><h5 class='modal-title text-truncate'></h5>" +
            "<button type='button' class='btn-close' data-bs-dismiss='modal'></button></div>" +
            "<div class='modal-body'></div>" +
            "</div></div></div>");
    }

    // Sets the modal width.
    static setModalSize(modalElem, size) {
        let sizeClass = "";

        switch (size) {
            case ModalSize.SMALL:
                sizeClass = "modal-sm";
                break;

            case ModalSize.LARGE:
                sizeClass = "modal-lg";
                break;

            case ModalSize.EXTRA_LARGE:
                sizeClass = "modal-xl";
                break;

            default:
                sizeClass = "";
                break;
        }

        if (sizeClass) {
            modalElem.children(".modal-dialog:first").addClass(sizeClass);
        }
    }

    // Adds a footer with the buttons to the modal.
    static addModalFooter(modalElem, buttons) {
        let footerElem = modalElem.find(".modal-footer:first");

        if (footerElem.length === 0) {
            footerElem = $("<div class='modal-footer'></div>")
            modalElem.find(".modal-content:first").append(footerElem);
        }

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
            footerElem.append(`<button type='button' class='btn ${subclass}' data-rs-value='${btn}' ${dismiss}>${caption}</button>`);
        }
    }

    // Sets the modal title.
    static setModalTitle(modalElem, title) {
        modalElem.find(".modal-title:first").text(title)
    }

    // Sets the contents of the modal body. The contents is a jQuery object or a string.
    static setModalBody(modalElem, contents) {
        let modalBodyElem = modalElem.find(".modal-body:first");
        modalBodyElem.empty();

        if (contents instanceof jQuery) {
            modalBodyElem.append(contents);
        } else {
            modalBodyElem.text(contents);
        }
    }
}

// Manages modal dialogs.
class ModalManager extends ModalBase {
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
            submitElem.trigger("click");
        } else {
            let formElem = modalWnd.$("form:first");

            if (formElem.length > 0 && formElem[0].reportValidity()) {
                formElem.trigger("submit");
            }
        }
    }

    // Finds a modal button by its value.
    static _findButton(modalWnd, buttonValue) {
        return ModalManager._getModalElem(modalWnd)
            .find(".modal-footer button[data-rs-value='" + buttonValue + "']");
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
        let options = Object.assign(new ModalOptions(), opt_options);
        let modalElem = ModalBase.createModalElem();
        ModalBase.setModalSize(modalElem, options.size);
        ModalBase.addModalFooter(modalElem, options.buttons);

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

        ModalBase.setModalBody(modalElem, modalFrame);
        $("body").append(modalElem);

        // bind events to the modal
        modalElem
            .on('shown.bs.modal', function () {
                let frameWnd = ModalManager._getModalWnd(modalElem);
                let frameAccessible = ScadaUtils.checkAccessToFrame(frameWnd, true);

                // update the modal height
                if (frameAccessible && !options.height) {
                    // html height can be greater than body height because of element margins
                    modalFrame.css("height", frameWnd.$("html").height());
                }

                // set input focus
                let autofocusElem = frameAccessible
                    ? frameWnd.$(frameWnd.document).find("[autofocus]").add(modalFrame).first()
                    : modalFrame;
                autofocusElem.trigger("focus");

                // remove overlay to allow user activity
                tempOverlay.remove();
            })
            .on('hide.bs.modal', function () {
                // remove focus to prevent browser warning
                if (document.activeElement instanceof HTMLElement) {
                    document.activeElement.blur();
                }
            })
            .on('hidden.bs.modal', function () {
                let callback = $(this).data("rs-callback");

                if (typeof callback === "function") {
                    callback($(this).data("rs-result"));
                }

                $(this).remove();
            });

        modalElem.find(".modal-footer button").on("click", function () {
            let frameWnd = ModalManager._getModalWnd(modalElem);

            if (ScadaUtils.checkAccessToFrame(frameWnd, true)) {
                // raise event
                let buttonValue = $(this).data("rs-value");
                frameWnd.$(frameWnd).trigger(ScadaEventType.MODAL_BTN_CLICK, buttonValue);

                // submit the modal
                if ($(this).hasClass("rs-btn-submit")) {
                    ModalManager._submitModal(frameWnd, buttonValue);
                }
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
                    ModalBase.setModalTitle(modalElem, options.title || url);
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
        let modalElem = ModalManager._getModalElem(modalWnd);
        ModalBase.setModalTitle(modalElem, title);
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
        let frameHtml = frame.contents().find("html");
        let newHeight = frameHtml.height();

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

// Represents modal box options. 
class ModalBoxOptions {
    title = null;
    alert = null;
    isHtml = false;

    constructor(fields) {
        Object.assign(this, fields);
    }
}

// Specifies the alerts for modal messages.
class ModalBoxAlerts {
    static DANGER = "danger";
    static WARNING = "warning";
}

// Represents a modal box.
// Modal box is a modal that does not contain another web page.
class ModalBox extends ModalBase {
    // Shows a modal containing the specified message.
    static showMessage(message, opt_options) {
        let options = opt_options ?? new ModalBoxOptions();
        let modalElem = this.createModalElem();
        this.addModalFooter(modalElem, [ModalButton.CLOSE]);
        this.setModalTitle(modalElem, options.title ?? document.title);

        if (options.alert) {
            let alertElem = $(`<div class='alert alert-${options.alert}'></div>`);

            if (options.isHtml) {
                alertElem.html(message);
            } else {
                alertElem.text(message);
            }

            this.setModalBody(modalElem, alertElem);
        } else {
            this.setModalBody(modalElem, message);
        }

        modalElem.on('hidden.bs.modal', function () {
            $(this).remove();
        });

        let modal = new bootstrap.Modal(modalElem[0], {});
        modal.show();
    }
}
