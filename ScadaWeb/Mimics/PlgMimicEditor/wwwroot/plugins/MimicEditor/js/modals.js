// Contains classes: ImageModal
// Depends on jquery, bootstrap

// Represents a modal dialog for editing an image.
class ImageModal {
    _elem;
    _modal;

    constructor(elemID) {
        this._elem = $("#" + elemID);
        this._modal = new bootstrap.Modal(this._elem[0]);
    }

    show(image) {
        this._modal.show();
    }
}
