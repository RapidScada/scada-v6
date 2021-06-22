// Represents a splitter component.
// Depends on jquery, scada-common.js
class Splitter {
    constructor(splitterElemID) {
        // The minimum size of the resized element if not specified.
        this.DEFAULT_MIN_SIZE = 50;

        // Indicates that the splitter is in resize mode.
        this._isResizeMode = false;
        // Indicates that a touch pad is being used.
        this._touchEnabled = false;

        // The starting state of resizing.
        this._startX = 0;
        this._startY = 0;
        this._prevElemStartSize = 0;
        this._nextElemStartSize = 0;

        // The jQuery object that represents the splitter.
        this.splitterElem = $("#" + splitterElemID);
        // The jQuery object that represents the resized element before the splitter.
        this.prevElem = this.splitterElem.prev("div");
        // The jQuery object that represents the resized element after the splitter.
        this.nextElem = this.splitterElem.next("div");
        // Indicates that the splitter is horizontal.
        this.isHorizontal = this.splitterElem.hasClass("hor");
        // The callbacks executed when resizing of the splitter is stopped.
        this.exitResizeModeCallbacks = $.Callbacks();

        if (this.splitterElem.length > 0 &&
            this.prevElem.length > 0 &&
            this.nextElem.length > 0) {

            this._bindEvents();
        }
    }

    // Binds events to the DOM elements.
    _bindEvents() {
        let thisObj = this;

        $(document)
            .on("mouseup mouseleave touchend touchcancel", function () {
                thisObj._exitResizeMode();
            })
            .on("touchmove", function (event) {
                if (thisObj._isResizeMode) {
                    thisObj._touchEnabled = true;
                    event = event.originalEvent.touches[0];
                    thisObj._doResize(event.pageX, event.pageY);
                    return false;
                }
            })
            .on("mousemove", function (event) {
                if (thisObj._isResizeMode && !thisObj._touchEnabled) {
                    thisObj._doResize(event.pageX, event.pageY);
                    return false;
                }
            });

        $(window)
            .on("resize", function () {
                thisObj._exitResizeMode();
            });

        this.splitterElem
            .off()
            .on("touchstart", function (event) {
                thisObj._touchEnabled = true;
                event = event.originalEvent.touches[0];
                thisObj._enterResizeMode(event.pageX, event.pageY);
                return false;
            })
            .on("mousedown", function (event) {
                if (!thisObj._touchEnabled) {
                    thisObj._enterResizeMode(event.pageX, event.pageY);
                }
                return false;
            });
    }

    // Starts resizing.
    _enterResizeMode(x, y) {
        this._isResizeMode = true;
        this._startX = x;
        this._startY = y;

        if (this.isHorizontal) {
            this._prevElemStartSize = this.prevElem.height();
            this._nextElemStartSize = this.nextElem.height();
        } else {
            this._prevElemStartSize = this.prevElem.width();
            this._nextElemStartSize = this.nextElem.width();
        }

        this._addOverlay(); // allow receiving events
        this.splitterElem.addClass("splitter-active");
    }

    // Stops resizing.
    _exitResizeMode() {
        if (this._isResizeMode) {
            this._isResizeMode = false;
            this._removeOverlay();
            this.splitterElem.removeClass("splitter-active");
            this.exitResizeModeCallbacks.fire();
        }
    }

    // Resizes the elements according to the cursor coordinates.
    _doResize(x, y) {
        if (this._isResizeMode) {
            if (this.isHorizontal) {
                let deltaY = y - this._startY;

                if (deltaY >= 0) {
                    var nextElemSize = Math.max(this._nextElemStartSize - deltaY, this._getMinHeight(this.nextElem));
                    var prevElemSize = this._prevElemStartSize + this._nextElemStartSize - nextElemSize;
                } else {
                    var prevElemSize = Math.max(this._prevElemStartSize + deltaY, this._getMinHeight(this.prevElem));
                    var nextElemSize = this._prevElemStartSize + this._nextElemStartSize - prevElemSize;
                }

                this.prevElem.height(prevElemSize);
                this.nextElem.height(nextElemSize);
            } else {
                let deltaX = x - this._startX;

                if (deltaX >= 0) {
                    var nextElemSize = Math.max(this._nextElemStartSize - deltaX, this._getMinWidth(this.nextElem));
                    var prevElemSize = this._prevElemStartSize + this._nextElemStartSize - nextElemSize;
                } else {
                    var prevElemSize = Math.max(this._prevElemStartSize + deltaX, this._getMinWidth(this.prevElem));
                    var nextElemSize = this._prevElemStartSize + this._nextElemStartSize - prevElemSize;
                }

                this.prevElem.width(prevElemSize);
                this.nextElem.width(nextElemSize);
            }
        }
    }

    // Adds an overlay div into the resized div to allow receiving events over iframe.
    _addOverlay() {
        $("<div class='splitter-overlay'><div/>").css({
            "cursor": this.isHorizontal ? "row-resize" : "col-resize",
            "z-index": ScadaUtils.FRONT_ZINDEX
        }).appendTo("body");
    }

    // Removes an overlay div.
    _removeOverlay() {
        $("div.splitter-overlay").remove();
    }

    // Gets the minimum width of the resized element.
    _getMinWidth(jqObj) {
        let minWidth = parseInt(jqObj.css("min-width"), 10);
        return minWidth ? minWidth : this.DEFAULT_MIN_SIZE;
    }

    // Gets the minimum height of the resized element.
    _getMinHeight(jqObj) {
        let minHeight = parseInt(jqObj.css("min-height"), 10);
        return minHeight ? minHeight : this.DEFAULT_MIN_SIZE;
    }
}
