// Contains classes: ModalContext, ModalBase, FaceplateModal, FontModal, ImageModal, TextEditor
// Depends on jquery, bootstrap, mimic-model.js

// Represents a context of a modal dialog.
class ModalContext {
    oldValue = null;
    newValue = null;
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

    _invokeCallback() {
        if (this._context.result && this._context.callback instanceof Function) {
            this._context.callback.call(this, this._context);
        }
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
                this._readFields(this._context.newValue);
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
                this._invokeCallback();
            });
    }

    _showFields(faceplateMeta) {
        $("#frmFaceplateModal").removeClass("was-validated")
        $("#faceplateModal_txtTypeName").val(faceplateMeta.typeName);
        $("#faceplateModal_txtPath").val(faceplateMeta.path);
    }

    _readFields(faceplateMeta) {
        faceplateMeta.typeName = $("#faceplateModal_txtTypeName").val();
        faceplateMeta.path = $("#faceplateModal_txtPath").val();
    }

    show(faceplateMeta, callback) {
        let newFaceplateMeta = new rs.mimic.FaceplateMeta();
        Object.assign(newFaceplateMeta, faceplateMeta);

        this._context = new ModalContext({
            oldValue: faceplateMeta,
            newValue: newFaceplateMeta,
            callback: callback
        });

        this._showFields(faceplateMeta);
        this._modal.show();
    }
}

// Represents a modal dialog for editing a font.
class FontModal extends ModalBase {
    constructor(elemID) {
        super(elemID);
        this._bindEvents();
    }

    _bindEvents() {
        $("#frmFontModal").on("submit", () => {
            $("#fontModal_btnOK").trigger("click");
            return false;
        });

        $("#fontModal_btnOK").on("click", () => {
            this._readFields(this._context.newValue);
            this._context.result = true;
            this._modal.hide();
        });

        $("#fontModal_chkInherit").on("change", (event) => {
            let inherit = $(event.target).prop("checked");
            $("#fontModal_fsProps").prop("disabled", inherit);
        });

        this._elem
            .on("shown.bs.modal", () => {
                $("#fontModal_chkInherit").focus();
            })
            .on("hidden.bs.modal", () => {
                this._invokeCallback();
            });
    }

    _showFields(font) {
        $("#fontModal_chkInherit").prop("checked", font.inherit);
        $("#fontModal_fsProps").prop("disabled", font.inherit);
        $("#fontModal_txtName").val(font.name);
        $("#fontModal_txtSize").val(font.size);
        $("#fontModal_chkBold").prop("checked", font.bold);
        $("#fontModal_chkItalic").prop("checked", font.italic);
        $("#fontModal_chkUnderline").prop("checked", font.underline);
    }

    _readFields(font) {
        font.inherit = $("#fontModal_chkInherit").prop("checked");
        font.name = $("#fontModal_txtName").val();
        font.size = Number.parseInt($("#fontModal_txtSize").val());
        font.bold = $("#fontModal_chkBold").prop("checked");
        font.italic = $("#fontModal_chkItalic").prop("checked");
        font.underline = $("#fontModal_chkUnderline").prop("checked");
    }

    show(font, options, callback) {
        let newFont = new rs.mimic.Font(font);
        this._context = new ModalContext({
            oldValue: font,
            newValue: newFont,
            callback: callback
        });

        this._showFields(font);
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
                this._readFields(this._context.newValue);
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
                this._invokeCallback();
            });
    }

    _showFields(image) {
        $("#frmImageModal").removeClass("was-validated")
        $("#imageModal_txtName").val(image.name);
        $("#imageModal_file").val("");
    }

    _readFields(image) {
        image.name = $("#imageModal_txtName").val();
        image.dataUrl = $("#imageModal_imgPreview").attr("src");
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
        let newImage = new rs.mimic.Image();
        Object.assign(newImage, image);

        this._context = new ModalContext({
            oldValue: image,
            newValue: newImage,
            callback: callback
        });

        this._showFields(image);
        this._showFileSize(this._getFileSize(image.data));
        this._showImage(image.dataUrl);
        this._modal.show();
    }
}

// Represents a modal dialog for editing text.
class TextEditor extends ModalBase {
    static DEFAULT_OPTIONS = {
        language: "none"
    }

    _flask;

    constructor(elemID) {
        super(elemID);
        let editorElem = $("#textEditor_divEditor");
        this._flask = new CodeFlask(editorElem[0], TextEditor.DEFAULT_OPTIONS);
        this._bindEvents();
    }

    _bindEvents() {
        $("#textEditor_btnOK").on("click", () => {
            this._context.newValue = this._flask.getCode();
            this._context.result = true;
            this._modal.hide();
        });

        this._elem
            .on("shown.bs.modal", () => {
                $("#textEditor_divEditor textarea").focus();
            })
            .on("hidden.bs.modal", () => {
                this._invokeCallback();
            });
    }

    _showLanguage(language) {
        let lblLanguage = $("#textEditor_lblLanguage");

        switch (language) {
            case "css":
                lblLanguage.text("CSS").removeClass("d-none");
                break;

            case "js":
                lblLanguage.text("JavaScript").removeClass("d-none");
                break;

            case "markup":
                lblLanguage.text("HTML/XML").removeClass("d-none");
                break;

            default:
                lblLanguage.text("Text").addClass("d-none");
                break;
        }
    }

    show(text, options, callback) {
        this._context = new ModalContext({
            oldValue: text,
            callback: callback
        });

        options ??= TextEditor.DEFAULT_OPTIONS;
        this._showLanguage(options.language);
        this._flask.updateLanguage(options.language);
        this._flask.updateCode(text);
        this._modal.show();
    }
}
