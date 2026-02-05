// Contains classes: ModalContext, ModalBase, ColorModal, FaceplateModal, FontModal, ImageModal, TextEditor
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

        if (this._elem.length === 0) {
            throw new Error(`Modal #${elemID} not found.`);
        }

        this._modal = new bootstrap.Modal(this._elem[0]);
        this._context = new ModalContext();
        this._bindEvents();
    }

    _bindEvents() {
        this._elem.find("form:first").on("submit", (event) => {
            this._elem.find(".modal-footer .btn-primary:first").trigger("click");
            event.preventDefault();
        });

        this._elem
            .on("shown.bs.modal", () => {
                this._setFocus();
            })
            .on("hidden.bs.modal", () => {
                this._invokeCallback();
            });
    }

    _setFocus() {
        // do nothing
    }

    _invokeCallback() {
        if (this._context.result && this._context.callback instanceof Function) {
            this._context.callback.call(this, this._context);
        }
    }
}

// Represents a modal dialog for choosing a color.
class ColorModal extends ModalBase {
    _colorsFilled = false;

    _bindEvents() {
        super._bindEvents();

        $("#colorModal_btnOK").on("click", () => {
            this._context.newValue = $("#colorModal_txtColor").val();
            this._context.result = true;
            this._modal.hide();
        });
    }

    _setFocus() {
        $("#colorModal_txtColor").focus();
    }

    _fillRecentColors() {

    }

    _fillNamedColors() {
        if (this._colorsFilled) {
            return;
        }

        const redColors = [
            { name: "IndianRed", hex: "#CD5C5C" },
            { name: "LightCoral", hex: "#F08080" },
            { name: "Salmon", hex: "#FA8072" },
            { name: "DarkSalmon", hex: "#E9967A" },
            { name: "LightSalmon", hex: "#FFA07A" },
            { name: "Crimson", hex: "#DC143C" },
            { name: "Red", hex: "#FF0000" },
            { name: "FireBrick", hex: "#B22222" },
            { name: "DarkRed", hex: "#8B0000" },
        ];

        const pinkColors = [
            { name: "Pink", hex: "#FFC0CB" },
            { name: "LightPink", hex: "#FFB6C1" },
            { name: "HotPink", hex: "#FF69B4" },
            { name: "DeepPink", hex: "#FF1493" },
            { name: "MediumVioletRed", hex: "#C71585" },
            { name: "PaleVioletRed", hex: "#DB7093" },
        ];

        const orangeColors = [
            { name: "LightSalmon", hex: "#FFA07A" },
            { name: "Coral", hex: "#FF7F50" },
            { name: "Tomato", hex: "#FF6347" },
            { name: "OrangeRed", hex: "#FF4500" },
            { name: "DarkOrange", hex: "#FF8C00" },
            { name: "Orange", hex: "#FFA500" },
        ];

        const yellowColors = [
            { name: "Gold", hex: "#FFD700" },
            { name: "Yellow", hex: "#FFFF00" },
            { name: "LightYellow", hex: "#FFFFE0" },
            { name: "LemonChiffon", hex: "#FFFACD" },
            { name: "LightGoldenrodYellow", hex: "#FAFAD2" },
            { name: "PapayaWhip", hex: "#FFEFD5" },
            { name: "Moccasin", hex: "#FFE4B5" },
            { name: "PeachPuff", hex: "#FFDAB9" },
            { name: "PaleGoldenrod", hex: "#EEE8AA" },
            { name: "Khaki", hex: "#F0E68C" },
            { name: "DarkKhaki", hex: "#BDB76B" },
        ];

        const purpleColors = [
            { name: "Lavender", hex: "#E6E6FA" },
            { name: "Thistle", hex: "#D8BFD8" },
            { name: "Plum", hex: "#DDA0DD" },
            { name: "Violet", hex: "#EE82EE" },
            { name: "Orchid", hex: "#DA70D6" },
            { name: "Fuchsia", hex: "#FF00FF" },
            { name: "Magenta", hex: "#FF00FF" },
            { name: "MediumOrchid", hex: "#BA55D3" },
            { name: "MediumPurple", hex: "#9370DB" },
            { name: "RebeccaPurple", hex: "#663399" },
            { name: "BlueViolet", hex: "#8A2BE2" },
            { name: "DarkViolet", hex: "#9400D3" },
            { name: "DarkOrchid", hex: "#9932CC" },
            { name: "DarkMagenta", hex: "#8B008B" },
            { name: "Purple", hex: "#800080" },
            { name: "Indigo", hex: "#4B0082" },
            { name: "SlateBlue", hex: "#6A5ACD" },
            { name: "DarkSlateBlue", hex: "#483D8B" },
            { name: "MediumSlateBlue", hex: "#7B68EE" },
        ];

        const greenColors = [
            { name: "GreenYellow", hex: "#ADFF2F" },
            { name: "Chartreuse", hex: "#7FFF00" },
            { name: "LawnGreen", hex: "#7CFC00" },
            { name: "Lime", hex: "#00FF00" },
            { name: "LimeGreen", hex: "#32CD32" },
            { name: "PaleGreen", hex: "#98FB98" },
            { name: "LightGreen", hex: "#90EE90" },
            { name: "MediumSpringGreen", hex: "#00FA9A" },
            { name: "SpringGreen", hex: "#00FF7F" },
            { name: "MediumSeaGreen", hex: "#3CB371" },
            { name: "SeaGreen", hex: "#2E8B57" },
            { name: "ForestGreen", hex: "#228B22" },
            { name: "Green", hex: "#008000" },
            { name: "DarkGreen", hex: "#006400" },
            { name: "YellowGreen", hex: "#9ACD32" },
            { name: "OliveDrab", hex: "#6B8E23" },
            { name: "Olive", hex: "#808000" },
            { name: "DarkOliveGreen", hex: "#556B2F" },
            { name: "MediumAquamarine", hex: "#66CDAA" },
            { name: "DarkSeaGreen", hex: "#8FBC8B" },
            { name: "LightSeaGreen", hex: "#20B2AA" },
            { name: "DarkCyan", hex: "#008B8B" },
            { name: "Teal", hex: "#008080" },
        ];

        const blueColors = [
            { name: "Aqua", hex: "#00FFFF" },
            { name: "Cyan", hex: "#00FFFF" },
            { name: "LightCyan", hex: "#E0FFFF" },
            { name: "PaleTurquoise", hex: "#AFEEEE" },
            { name: "Aquamarine", hex: "#7FFFD4" },
            { name: "Turquoise", hex: "#40E0D0" },
            { name: "MediumTurquoise", hex: "#48D1CC" },
            { name: "DarkTurquoise", hex: "#00CED1" },
            { name: "CadetBlue", hex: "#5F9EA0" },
            { name: "SteelBlue", hex: "#4682B4" },
            { name: "LightSteelBlue", hex: "#B0C4DE" },
            { name: "PowderBlue", hex: "#B0E0E6" },
            { name: "LightBlue", hex: "#ADD8E6" },
            { name: "SkyBlue", hex: "#87CEEB" },
            { name: "LightSkyBlue", hex: "#87CEFA" },
            { name: "DeepSkyBlue", hex: "#00BFFF" },
            { name: "DodgerBlue", hex: "#1E90FF" },
            { name: "CornflowerBlue", hex: "#6495ED" },
            { name: "MediumSlateBlue", hex: "#7B68EE" },
            { name: "RoyalBlue", hex: "#4169E1" },
            { name: "Blue", hex: "#0000FF" },
            { name: "MediumBlue", hex: "#0000CD" },
            { name: "DarkBlue", hex: "#00008B" },
            { name: "Navy", hex: "#000080" },
            { name: "MidnightBlue", hex: "#191970" },
        ];

        const brownColors = [
            { name: "Cornsilk", hex: "#FFF8DC" },
            { name: "BlanchedAlmond", hex: "#FFEBCD" },
            { name: "Bisque", hex: "#FFE4C4" },
            { name: "NavajoWhite", hex: "#FFDEAD" },
            { name: "Wheat", hex: "#F5DEB3" },
            { name: "BurlyWood", hex: "#DEB887" },
            { name: "Tan", hex: "#D2B48C" },
            { name: "RosyBrown", hex: "#BC8F8F" },
            { name: "SandyBrown", hex: "#F4A460" },
            { name: "Goldenrod", hex: "#DAA520" },
            { name: "DarkGoldenrod", hex: "#B8860B" },
            { name: "Peru", hex: "#CD853F" },
            { name: "Chocolate", hex: "#D2691E" },
            { name: "SaddleBrown", hex: "#8B4513" },
            { name: "Sienna", hex: "#A0522D" },
            { name: "Brown", hex: "#A52A2A" },
            { name: "Maroon", hex: "#800000" },
        ];

        const whiteColors = [
            { name: "White", hex: "#FFFFFF" },
            { name: "Snow", hex: "#FFFAFA" },
            { name: "HoneyDew", hex: "#F0FFF0" },
            { name: "MintCream", hex: "#F5FFFA" },
            { name: "Azure", hex: "#F0FFFF" },
            { name: "AliceBlue", hex: "#F0F8FF" },
            { name: "GhostWhite", hex: "#F8F8FF" },
            { name: "WhiteSmoke", hex: "#F5F5F5" },
            { name: "SeaShell", hex: "#FFF5EE" },
            { name: "Beige", hex: "#F5F5DC" },
            { name: "OldLace", hex: "#FDF5E6" },
            { name: "FloralWhite", hex: "#FFFAF0" },
            { name: "Ivory", hex: "#FFFFF0" },
            { name: "AntiqueWhite", hex: "#FAEBD7" },
            { name: "Linen", hex: "#FAF0E6" },
            { name: "LavenderBlush", hex: "#FFF0F5" },
            { name: "MistyRose", hex: "#FFE4E1" },
        ];

        const grayColors = [
            { name: "Gainsboro", hex: "#DCDCDC" },
            { name: "LightGray", hex: "#D3D3D3" },
            { name: "Silver", hex: "#C0C0C0" },
            { name: "DarkGray", hex: "#A9A9A9" },
            { name: "Gray", hex: "#808080" },
            { name: "DimGray", hex: "#696969" },
            { name: "LightSlateGray", hex: "#778899" },
            { name: "SlateGray", hex: "#708090" },
            { name: "DarkSlateGray", hex: "#2F4F4F" },
            { name: "Black", hex: "#000000" },
        ];

        this._fillColorTable("colorModal_tblRedColors", redColors);
        this._fillColorTable("colorModal_tblPinkColors", pinkColors);
        this._fillColorTable("colorModal_tblOrangeColors", orangeColors);
        this._fillColorTable("colorModal_tblYellowColors", yellowColors);
        this._fillColorTable("colorModal_tblPurpleColors", purpleColors);
        this._fillColorTable("colorModal_tblGreenColors", greenColors);
        this._fillColorTable("colorModal_tblBlueColors", blueColors);
        this._fillColorTable("colorModal_tblBrownColors", brownColors);
        this._fillColorTable("colorModal_tblWhiteColors", whiteColors);
        this._fillColorTable("colorModal_tblGrayColors", grayColors);
        this._colorsFilled = true;
    }

    _fillColorTable(tableID, colors) {
        let tableElem = $("#" + tableID);
        tableElem.remove("tbody"); // remove table body if exists
        let tbodyElem = $("<tbody></tbody>").appendTo(tableElem);

        for (let color of colors) {
            let rowElem = $("<tr></tr>").appendTo(tbodyElem);
            $(`<td><div class="rounded-circle" style="background-color:${color.hex}"></div></td>`).appendTo(rowElem);
            $("<td></td>").text(color.name).appendTo(rowElem);
            $("<td></td>").text(color.hex).appendTo(rowElem);
        }
    }

    show(color, callback) {
        this._context = new ModalContext({
            oldValue: color,
            callback: callback
        });

        $("#colorModal_txtColor").val(color);
        this._fillRecentColors();
        this._fillNamedColors();
        this._modal.show();
    }
}

// Represents a modal dialog for editing a faceplate meta.
class FaceplateModal extends ModalBase {
    _bindEvents() {
        super._bindEvents();

        $("#faceplateModal_btnOK").on("click", () => {
            let formElem = $("#frmFaceplateModal");

            if (formElem[0].checkValidity()) {
                this._readFields(this._context.newValue);
                this._context.result = true;
                this._modal.hide();
            }

            formElem.addClass("was-validated");
        });
    }

    _setFocus() {
        $("#faceplateModal_txtTypeName").focus();
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
        Object.assign(newFaceplateMeta, faceplateMeta); // faceplateMeta can be null

        this._context = new ModalContext({
            oldValue: faceplateMeta,
            newValue: newFaceplateMeta,
            callback: callback
        });

        this._showFields(newFaceplateMeta);
        this._modal.show();
    }
}

// Represents a modal dialog for editing a font.
class FontModal extends ModalBase {
    _bindEvents() {
        super._bindEvents();

        $("#fontModal_btnOK").on("click", () => {
            this._readFields(this._context.newValue);
            this._context.result = true;
            this._modal.hide();
        });

        $("#fontModal_chkInherit").on("change", (event) => {
            let inherit = $(event.target).prop("checked");
            $("#fontModal_fsProps").prop("disabled", inherit);
        });
    }

    _setFocus() {
        $("#fontModal_chkInherit").focus();
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

    show(font, callback) {
        let newFont = new rs.mimic.Font(font);
        this._context = new ModalContext({
            oldValue: font,
            newValue: newFont,
            callback: callback
        });

        this._showFields(newFont);
        this._modal.show();
    }
}

// Represents a modal dialog for editing an image.
class ImageModal extends ModalBase {
    _bindEvents() {
        super._bindEvents();

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
    }

    _setFocus() {
        $("#imageModal_txtName").focus();
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
        Object.assign(newImage, image); // image can be null

        this._context = new ModalContext({
            oldValue: image,
            newValue: newImage,
            callback: callback
        });

        this._showFields(newImage);
        this._showFileSize(this._getFileSize(newImage.data));
        this._showImage(newImage.dataUrl);
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
    }

    _bindEvents() {
        super._bindEvents();

        $("#textEditor_btnOK").on("click", () => {
            this._context.newValue = this._flask.getCode();
            this._context.result = true;
            this._modal.hide();
        });
    }

    _setFocus() {
        $("#textEditor_divEditor textarea").focus();
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
