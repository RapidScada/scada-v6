// Contains classes: ModalContext, ModalBase, FaceplateModal, ImageModal
// Depends on jquery, bootstrap, mimic-model.js

// Represents a context of a modal dialog.
class ModalContext {
    oldObject = null;
    newObject = null;
    result = false;
    callback = null;

    constructor(source) {
        Object.assign(this, source);
    }
}

// A base class for modal dialogs.
class ModalBase {
    _elem;
    _modal;
    _context;

    constructor(elemID) {
        this._elem = $("#" + elemID);
        this._modal = new bootstrap.Modal(this._elem[0]);
        this._context = new ModalContext();
    }
}

// Represents a modal dialog for editing a faceplate meta.
class FaceplateModal extends ModalBase {
    constructor(elemID) {
        super(elemID);
        this._bindEvents();
    }

    _bindEvents() {
        $("#frmFaceplateModal").on("submit", () => {
            $("#faceplateModal_btnOK").trigger("click");
            return false;
        });

        $("#faceplateModal_btnOK").on("click", () => {
            let formElem = $("#frmFaceplateModal");

            if (formElem[0].checkValidity()) {
                this._readFields();
                this._context.result = true;
                this._modal.hide();
            }

            formElem.addClass("was-validated");
        });

        this._elem
            .on("shown.bs.modal", () => {
                $("#faceplateModal_txtTypeName").focus();
            })
            .on("hidden.bs.modal", () => {
                if (this._context.result && this._context.callback instanceof Function) {
                    this._context.callback.call(this, this._context);
                }
            });
    }

    _readFields() {
        let obj = this._context.newObject;

        if (obj) {
            obj.typeName = $("#faceplateModal_txtTypeName").val();
            obj.path = $("#faceplateModal_txtPath").val();
        }
    }

    show(faceplateMeta, callback) {
        let obj = new rs.mimic.FaceplateMeta();
        Object.assign(obj, faceplateMeta);

        this._context = new ModalContext({
            oldObject: faceplateMeta,
            newObject: obj,
            callback: callback
        });

        $("#frmFaceplateModal").removeClass("was-validated")
        $("#faceplateModal_txtTypeName").val(obj.typeName);
        $("#faceplateModal_txtPath").val(obj.path);
        this._modal.show();
    }
}

// Represents a modal dialog for editing an image.
class ImageModal extends ModalBase {
    constructor(elemID) {
        super(elemID);
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
            $("#imageModal_file").trigger("click");
        });

        $("#imageModal_btnDownload").on("click", (event) => {
            let linkElem = $(event.target);
            this._downloadImage(linkElem);
        });

        $("#imageModal_file").on("change", (event) => {
            let file = event.target.files[0];

            if (file) {
                this._uploadImage(file);
            }
        });

        this._elem
            .on("shown.bs.modal", () => {
                $("#imageModal_txtName").focus();
            })
            .on("hidden.bs.modal", () => {
                if (this._context.result && this._context.callback instanceof Function) {
                    this._context.callback.call(this, this._context);
                }
            });
    }

    _readFields() {
        let obj = this._context.newObject;

        if (obj) {
            obj.name = $("#imageModal_txtName").val();
            obj.dataUrl = $("#imageModal_imgPreview").attr("src");
        }
    }

    _showFileSize(size) {
        $("#imageModal_spnFileSize").text(size ? "(" + Math.round(size / 1024) + " KB)" : "");
    }

    _getFileSize(imageData) {
        return imageData ? atob(imageData).length : 0;
    }

    _showImage(dataUrl) {
        if (dataUrl) {
            $("#imageModal_imgPreview").attr("src", dataUrl).removeClass("d-none");
            $("#imageModal_divNoImage").addClass("d-none");
            $("#imageModal_btnDownload").prop("disabled", false);
        } else {
            $("#imageModal_imgPreview").attr("src", "").addClass("d-none");
            $("#imageModal_divNoImage").removeClass("d-none");
            $("#imageModal_btnDownload").prop("disabled", true);
        }
    }

    _uploadImage(file) {
        let reader = new FileReader();

        reader.onload = () => {
            let txtName = $("#imageModal_txtName");

            if (!txtName.val()) {
                txtName.val(file.name);
            }

            this._showFileSize(file.size);
            this._showImage(reader.result);
        };

        reader.onerror = () => {
            console.error("Error reading file.");
        };

        reader.readAsDataURL(file);
    }

    _downloadImage(linkElem) {
        let name = $("#imageModal_txtName").val();
        let dataUrl = $("#imageModal_imgPreview").attr("src");
        linkElem
            .attr("download", name)
            .attr("href", dataUrl);
    }

    show(image, callback) {
        let obj = new rs.mimic.Image();
        Object.assign(obj, image);

        this._context = new ModalContext({
            oldObject: image,
            newObject: obj,
            callback: callback
        });

        $("#frmImageModal").removeClass("was-validated")
        $("#imageModal_txtName").val(obj.name);
        $("#imageModal_file").val("");
        this._showFileSize(this._getFileSize(obj.data));
        this._showImage(obj.dataUrl);
        this._modal.show();
    }
}
