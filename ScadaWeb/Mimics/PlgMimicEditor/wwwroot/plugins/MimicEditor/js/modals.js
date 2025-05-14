// Contains classes: ModalContext, ImageModal
// Depends on jquery, bootstrap, mimic-model.js

// Represents a context of a modal dialog.
class ModalContext {
    callback = null;
    objectToEdit = null;
    result = false;

    constructor(source) {
        Object.assign(this, source);
    }
}

// Represents a modal dialog for editing an image.
class ImageModal {
    _elem;
    _modal;
    _context;

    constructor(elemID) {
        this._elem = $("#" + elemID);
        this._modal = new bootstrap.Modal(this._elem[0]);
        this._context = new ModalContext();
        this._bindEvents();
    }

    _bindEvents() {
        $("#frmImageModal").on("submit", () => {
            $("#imageModal_btnOK").trigger("click");
            return false;
        });

        $("#imageModal_btnOK").on("click", () => {
            let formElem = $("#frmImageModal");

            if (formElem[0].checkValidity()) {
                this._readFields();
                this._context.result = true;
                this._modal.hide();
            }

            formElem.addClass("was-validated");
        });

        $("#imageModal_btnUpload").on("click", () => {

        });

        $("#imageModal_btnDownload").on("click", () => {

        });

        this._elem
            .on("shown.bs.modal", () => {
                $("#imageModal_txtName").focus();
            })
            .on("hidden.bs.modal", () => {
                if (this._context.result && this._context.callback instanceof Function) {
                    this._context.callback.call(this);
                }
            });
    }

    _readFields() {
        if (this._context.objectToEdit) {
            this._context.objectToEdit.name = $("#imageModal_txtName").val();
        }
    }

    show(image, callback) {
        image = image instanceof rs.mimic.Image
            ? image
            : new rs.mimic.Image();

        this._context = new ModalContext({
            callback: callback,
            objectToEdit: image
        });

        $("#frmImageModal").removeClass("was-validated")
        $("#imageModal_txtName").val(image.name);

        if (image.data) {
            $("#imageModal_imgPreview").attr("src", "data:;base64," + image.data).removeClass("d-none");
            $("#imageModal_divNoImage").addClass("d-none");
            $("#imageModal_btnDownload").prop("disabled", false);
        } else {
            $("#imageModal_imgPreview").attr("src", "").addClass("d-none");
            $("#imageModal_divNoImage").removeClass("d-none");
            $("#imageModal_btnDownload").prop("disabled", true);
        }

        this._modal.show();
    }
}
