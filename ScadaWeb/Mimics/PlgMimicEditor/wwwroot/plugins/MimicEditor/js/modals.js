// Contains classes: ModalContext, ModalBase, ColorModal, FaceplateModal, FontModal,
//     ImageEditModal, ImageSelectModal, PropertyModal, TextEditor
// Depends on jquery, bootstrap, mimic-model.js, prop-grid.js

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
                this._handleShown();
            })
            .on("hidden.bs.modal", () => {
                this._invokeCallback();
            });
    }

    _setFocus() {
        // do nothing
    }

    _handleShown() {
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
    static _RECENT_COLOR_COUNT = 5;

    _recentColors = [];
    _colorsFilled = false;

    _bindEvents() {
        super._bindEvents();

        $("#colorModal_btnOK").on("click", () => {
            let color = $("#colorModal_txtColor").val();
            this._addRecentColor(color);
            this._context.newValue = color;
            this._context.result = true;
            this._modal.hide();
        });

        $("#colorModal_divKnownColors table").on("click", "tr", (event) => {
            let rowElem = $(event.currentTarget);
            let color = rowElem.data("color");

            if (color?.name) {
                this._selectRow(rowElem);
                $("#colorModal_txtColor").val(color.name);
            }
        });
    }

    _setFocus() {
        $("#colorModal_txtColor").focus();
    }

    _handleShown() {
        let color = $("#colorModal_txtColor").val();
        this._selectColor(color);
    }

    _fillRecentColors() {
        this._fillColorTable("colorModal_tblRecentColors",
            this._recentColors.map(color => {
                return { name: color, hex: "" };
            }));
    }

    _fillNamedColors() {
        if (this._colorsFilled) {
            return;
        }

        // colors taken from https://htmlcolorcodes.com/color-names/

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
        tableElem.prop("hidden", colors.length === 0);
        let tbodyElem = $("<tbody></tbody>");

        for (let color of colors) {
            let rowElem = $("<tr></tr>").data("color", color).appendTo(tbodyElem);
            let circleElem = $("<div class='rounded-circle'></div>").css("background-color", color.name);
            $("<td></td>").append(circleElem).appendTo(rowElem);
            $("<td></td>").text(color.name).appendTo(rowElem);
            $("<td></td>").text(color.hex).appendTo(rowElem);
        }

        tableElem.children("tbody").remove(); // remove table body if exists
        tableElem.append(tbodyElem);
    }

    _selectColor(color) {
        let rowToSelect = null;

        if (color) {
            $("#colorModal_divKnownColors table tr").each((index, element) => {
                let rowElem = $(element);
                let colorObj = rowElem.data("color");

                if (colorObj && (colorObj.name === color || colorObj.hex === color)) {
                    rowToSelect = rowElem;
                    return false; // break loop
                }
            });
        }

        this._selectRow(rowToSelect);

        if (rowToSelect) {
            rowToSelect[0].scrollIntoView(false);
        } else {
            $("#colorModal_divKnownColors").scrollTop(0);
        }
    }

    _selectRow(rowElem) {
        $("#colorModal_divKnownColors table tr").removeClass("rs-selected");
        rowElem?.addClass("rs-selected");
    }

    _addRecentColor(color) {
        if (color) {
            let newRecentColors = [color];
            let colorIndex = 0;

            while (colorIndex < this._recentColors.length && newRecentColors.length < ColorModal._RECENT_COLOR_COUNT) {
                let recentColor = this._recentColors[colorIndex++];

                if (recentColor !== color) {
                    newRecentColors.push(recentColor);
                }
            }

            this._recentColors = newRecentColors;
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
class ImageEditModal extends ModalBase {
    _bindEvents() {
        super._bindEvents();

        $("#imageEditModal_btnOK").on("click", () => {
            let formElem = $("#frmImageEditModal");

            if (formElem[0].checkValidity()) {
                this._readFields(this._context.newValue);
                this._context.result = true;
                this._modal.hide();
            }

            formElem.addClass("was-validated");
        });

        $("#imageEditModal_btnUpload").on("click", () => {
            $("#imageEditModal_file").trigger("click");
        });

        $("#imageEditModal_btnDownload").on("click", (event) => {
            let linkElem = $(event.target);
            this._downloadImage(linkElem);
        });

        $("#imageEditModal_file").on("change", (event) => {
            let file = event.target.files[0];

            if (file) {
                this._uploadImage(file);
            }
        });
    }

    _setFocus() {
        $("#imageEditModal_txtName").focus();
    }

    _showFields(image) {
        $("#frmImageEditModal").removeClass("was-validated")
        $("#imageEditModal_txtName").val(image.name);
        $("#imageEditModal_file").val("");
    }

    _readFields(image) {
        image.name = $("#imageEditModal_txtName").val();
        image.dataUrl = $("#imageEditModal_imgPreview").attr("src");
    }

    _showFileSize(size) {
        $("#imageEditModal_spnFileSize").text(size ? "(" + Math.round(size / 1024) + " KB)" : "");
    }

    _getFileSize(imageData) {
        return imageData ? atob(imageData).length : 0;
    }

    _showImage(dataUrl) {
        if (dataUrl) {
            $("#imageEditModal_imgPreview").attr("src", dataUrl).removeClass("d-none");
            $("#imageEditModal_divNoImage").addClass("d-none");
            $("#imageEditModal_btnDownload").prop("disabled", false);
        } else {
            $("#imageEditModal_imgPreview").attr("src", "").addClass("d-none");
            $("#imageEditModal_divNoImage").removeClass("d-none");
            $("#imageEditModal_btnDownload").prop("disabled", true);
        }
    }

    _uploadImage(file) {
        let reader = new FileReader();

        reader.onload = () => {
            let txtName = $("#imageEditModal_txtName");

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
        let name = $("#imageEditModal_txtName").val();
        let dataUrl = $("#imageEditModal_imgPreview").attr("src");
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

// Represents a modal dialog for choosing an image.
class ImageSelectModal extends ModalBase {
    static _RECENT_IMAGE_COUNT = 3;

    _mimic;
    _recentImages = [];
    _imagesFilled = false;
    _lazyObserver = null;

    constructor(elemID, mimic) {
        super(elemID);

        if (mimic === null || mimic === undefined) {
            throw new Error("Mimic must not be null.");
        }

        this._mimic = mimic;
        this._initLazyObserver();
    }

    _bindEvents() {
        super._bindEvents();

        $("#imageSelectModal_btnOK").on("click", () => {
            let imageName = $("#imageSelectModal_txtName").val();
            this._addRecentImage(imageName);
            this._context.newValue = imageName;
            this._context.result = true;
            this._modal.hide();
        });

        $("#imageSelectModal_divAvailableImages").on("click", ".rs-image-item", (event) => {
            let itemElem = $(event.currentTarget);
            let image = itemElem.data("image");

            if (image?.name) {
                this._selectItem(itemElem);
                $("#imageSelectModal_txtName").val(image.name);
            }
        });
    }

    _setFocus() {
        $("#imageSelectModal_txtName").focus();
    }

    _handleShown() {
        let imageName = $("#imageSelectModal_txtName").val();
        this._selectImage(imageName);
    }

    _initLazyObserver() {
        this._lazyObserver = new IntersectionObserver(this._handleIntersect, {
            root: $("#imageSelectModal_divAvailableImages")[0],
            rootMargin: "100px 0px", // double item height
            threshold: 0.1
        });
    }

    _handleIntersect(entries, observer) {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                let itemElem = $(entry.target);
                let imgElem = itemElem.find("img:first");
                let image = itemElem.data("image");

                if (imgElem.not("[src]") && image) {
                    imgElem.attr("src", image.dataUrl);
                }

                observer.unobserve(entry.target);
            }
        });
    }

    _fillRecentImages() {
        this._fillImageList("imageSelectModal_divRecentImages", this._recentImages);
    }

    _fillAllImages() {
        if (!this._imagesFilled) {
            this._fillImageList("imageSelectModal_divAllImages", this._mimic.images);
            this._imagesFilled = true;
        }
    }

    _fillImageList(listID, images) {
        let listElem = $("#" + listID);
        listElem.prop("hidden", images.length === 0);
        let contentElem = $("<div class='rs-image-list-content'></div>");

        for (let image of images) {
            let itemElem = $("<div class='rs-image-item'></div>").data("image", image).appendTo(contentElem);
            this._lazyObserver.observe(itemElem[0]);
            $("<div class='rs-image-thumb'><img decoding='async' /></div>").appendTo(itemElem);
            $("<div class='rs-image-name'></div>").text(image.name).appendTo(itemElem);
        }

        listElem.children(".rs-image-list-content").remove(); // remove list content if exists
        listElem.append(contentElem);
    }

    _selectImage(imageName) {
        let itemToSelect = null;

        if (imageName) {
            $("#imageSelectModal_divAvailableImages div.rs-image-item").each((index, element) => {
                let itemElem = $(element);
                let image = itemElem.data("image");

                if (image && image.name === imageName) {
                    itemToSelect = itemElem;
                    return false; // break loop
                }
            });
        }

        this._selectItem(itemToSelect);

        if (itemToSelect) {
            itemToSelect[0].scrollIntoView(false);
        } else {
            $("#imageSelectModal_divAvailableImages").scrollTop(0);
        }
    }

    _selectItem(itemElem) {
        $("#imageSelectModal_divAvailableImages div.rs-image-item").removeClass("rs-selected");
        itemElem?.addClass("rs-selected");
    }

    _addRecentImage(imageName) {
        let image = this._mimic.imageMap.get(imageName);

        if (image) {
            let newRecentImages = [image];
            let imageIndex = 0;

            while (imageIndex < this._recentImages.length &&
                newRecentImages.length < ImageSelectModal._RECENT_IMAGE_COUNT) {
                let recentImage = this._recentImages[imageIndex++];

                if (recentImage !== image) {
                    newRecentImages.push(recentImage);
                }
            }

            this._recentImages = newRecentImages;
        }
    }

    show(imageName, callback) {
        this._context = new ModalContext({
            oldValue: imageName,
            callback: callback
        });

        $("#imageSelectModal_txtName").val(imageName);
        this._fillRecentImages();
        this._fillAllImages();
        this._modal.show();
    }

    invalidate() {
        this._imagesFilled = false;
    }
}

// Represents a modal dialog for choosing an object property.
class PropertyModal extends ModalBase {
    static DEFAULT_OPTIONS = {
        canSelectObject: false
    };

    _mimic;
    _options = null;

    constructor(elemID, mimic) {
        super(elemID);

        if (mimic === null || mimic === undefined) {
            throw new Error("Mimic must not be null.");
        }

        this._mimic = mimic;
    }

    _bindEvents() {
        super._bindEvents();

        $("#propertyModal_btnOK").on("click", () => {
            this._context.newValue = $("#propertyModal_txtPropertyName").val();
            this._context.result = true;
            this._modal.hide();
        });

        // show properties of the selected object
        $("#propertyModal_selObject").on("change", (event) => {
            let selectedVal = $(event.target).val();
            let componentID = Number.parseInt(selectedVal);
            this._showObjectProperties(this._mimic.componentMap.get(componentID));
            this._selectProperty(null);
        });

        // select clicked property
        $("#propertyModal_divObjectProperties").on("click", "li", (event) => {
            event.stopPropagation();
            let itemElem = $(event.currentTarget);

            if (!itemElem.hasClass("rs-disabled")) {
                let propertyName = itemElem.attr("data-path");
                $("#propertyModal_txtPropertyName").val(propertyName);
                this._selectItem(itemElem);
            }
        });
    }

    _setFocus() {
        $("#propertyModal_txtPropertyName").focus();
    }

    _handleShown() {
        let propertyName = $("#propertyModal_txtPropertyName").val();
        this._selectProperty(propertyName);
    }

    _selectProperty(propertyName) {
        let itemToSelect = null;

        if (propertyName) {
            $("#propertyModal_divObjectProperties li.rs-item").each((index, element) => {
                let itemElem = $(element);
                let path = itemElem.attr("data-path");

                if (path === propertyName) {
                    itemToSelect = itemElem;
                    return false; // break loop
                }
            });
        }

        this._selectItem(itemToSelect);

        if (itemToSelect) {
            itemToSelect[0].scrollIntoView(false);
        } else {
            $("#propertyModal_divObjectProperties").scrollTop(0);
        }
    }

    _selectItem(itemElem) {
        $("#propertyModal_divObjectProperties li.rs-item").removeClass("rs-selected");
        itemElem?.addClass("rs-selected");
    }

    _showSingleObject(obj) {
        $("#propertyModal_txtObjectDisplayName").val(obj?.toString()).prop("hidden", false);
        $("#propertyModal_selObject").prop("hidden", true);
    }

    _showObjectProperties(obj) {
        let appendItemsFunc = (parentElem, obj, parentPath) => {
            let targetObject = PropGridHelper.getTargetObject(obj);
            let objectDescriptor = PropGridHelper.getObjectDescriptor(obj);

            if (targetObject) {
                for (let [name, value] of Object.entries(targetObject).sort(([a], [b]) => a.localeCompare(b))) {
                    let path = parentPath + name;
                    let propertyDescriptor = objectDescriptor?.get(name);
                    let isBindable = propertyDescriptor?.isBindable ?? true;
                    let itemText = propertyDescriptor ? `${name} (${propertyDescriptor.displayName})` : name;
                    let itemElem = $("<li class='rs-item'></li>").attr("data-path", path).appendTo(parentElem);
                    $("<span class='rs-item-text'></span>").text(itemText).appendTo(itemElem);

                    if (isBindable) {
                        if (value instanceof Object) {
                            let childListElem = $("<ul></ul>").appendTo(itemElem);
                            appendItemsFunc(childListElem, value, path + ".");
                        }
                    } else {
                        itemElem.addClass("rs-disabled");
                    }
                }
            }
        };

        let listElem = $("<ul></ul>");
        let rootPath = this._options.canSelectObject ? obj?.name + "." : "";
        appendItemsFunc(listElem, obj, rootPath);
        $("#propertyModal_divObjectProperties").empty().append(listElem);
    }

    _findObject(propertyName) {
        if (propertyName) {
            let objectName = propertyName.split('.')[0];
            return objectName ? this._mimic.components.find(c => c.name === objectName) : null;
        } else {
            return null;
        }
    }

    _fillObjectList(selectedObject) {
        // clear list
        let selectElem = $("#propertyModal_selObject");
        let firstOptionElem = selectElem.children("option:first");
        firstOptionElem.detach();
        selectElem.empty();

        // create list options
        let optionArr = [firstOptionElem];
        let objectArr = [...this._mimic.components].sort((a, b) => a.id - b.id);

        for (let obj of objectArr) {
            optionArr.push($("<option></option>")
                .val(obj.id)
                .text(obj.toString())
                .prop("selected", selectedObject === obj)
            );
        }

        firstOptionElem.prop("selected", !selectedObject);
        selectElem.append(optionArr).prop("hidden", false);
        $("#propertyModal_txtObjectDisplayName").prop("hidden", true);
    }

    show(selectedObject, propertyName, options, callback) {
        this._context = new ModalContext({
            oldValue: propertyName,
            callback: callback
        });

        $("#propertyModal_txtPropertyName").val(propertyName);
        this._options = options ?? PropertyModal.DEFAULT_OPTIONS;

        if (this._options.canSelectObject) {
            let obj = this._findObject(propertyName);
            this._fillObjectList(obj);
            this._showObjectProperties(obj);
        } else {
            this._showSingleObject(selectedObject);
            this._showObjectProperties(selectedObject);
        }

        this._modal.show();
    }
}

// Represents a modal dialog for editing text.
class TextEditor extends ModalBase {
    static DEFAULT_OPTIONS = {
        language: "none"
    };

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
