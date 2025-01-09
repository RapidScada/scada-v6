/*
 * Extension of scheme for edit
 *
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2018
 *
 * Requires:
 * - jquery
 * - utils.js
 * - schemecommon.js
 * - schememodel.js
 * - schemerender.js
 */

// Rapid SCADA namespace
var scada = scada || {};
// Scheme namespace
scada.scheme = scada.scheme || {};

/********** Scheme Changes Results **********/

// Scheme changes results enumeration
scada.scheme.GetChangesResults = {
    SUCCESS: 0,
    RELOAD_SCHEME: 1,
    EDITOR_UNKNOWN: 2,
    DATA_ERROR: 3,
    COMM_ERROR: 4
};

/********** Types of Scheme Changes **********/

// Types of scheme changes enumeration
scada.scheme.SchemeChangeTypes = {
    NONE: 0,
    SCHEME_DOC_CHANGED: 1,
    COMPONENT_ADDED: 2,
    COMPONENT_CHANGED: 3,
    COMPONENT_DELETED: 4,
    IMAGE_ADDED: 5,
    IMAGE_RENAMED: 6,
    IMAGE_DELETED: 7
};

/********** Select Component Actions **********/

scada.scheme.SelectActions = {
    SELECT: "select",
    APPEND: "append",
    DESELECT: "deselect",
    DESELECT_ALL: "deselectall"
};

/********** Main Form Actions **********/

scada.scheme.FormActions = {
    NEW: "new",
    OPEN: "open",
    SAVE: "save",
    CUT: "cut",
    COPY: "copy",
    PASTE: "paste",
    UNDO: "undo",
    REDO: "redo",
    POINTER: "pointer",
    DELETE: "delete"
};

/********** Drag Modes **********/

scada.scheme.DragModes = {
    NONE: 0,
    MOVE: 1,
    NW_RESIZE: 2,
    NE_RESIZE: 3,
    SW_RESIZE: 4,
    SE_RESIZE: 5,
    W_RESIZE: 6,
    E_RESIZE: 7,
    N_RESIZE: 8,
    S_RESIZE: 9,
    ROTATE: 10, //旋转
};

/********** Dragging **********/

// Dragging type
scada.scheme.Dragging = function () {
    // Width of the border allows to resize component
    this.BORDER_WIDTH = 5;
    // Minimum size required for enable resizing
    this.MIN_SIZE = 15;
    // Minimally moving
    this.MIN_MOVING = 5;
    // Minimally moving
    this.MIN_GUIDE_LEN = 5;

    // Dragging mode
    this.mode = scada.scheme.DragModes.NONE;
    // X coordinate of dragging start
    this.startX = 0;
    // Y coordinate of dragging start
    this.startY = 0;
    // Last value of horizontal moving
    this.lastDx = 0;
    // Last value of vertical moving
    this.lastDy = 0;
    // Last value of resized width
    this.lastW = 0;
    // Last value of resized height
    this.lastH = 0;
    // 角度
    this.lastRotate = 0;
    // Dragged elements
    this.draggedElem = $();
    // Element was moved during dragging
    this.moved = false;
    // Element was resized during dragging
    this.resized = false;
    //旋转
    this.rotated = false;
};

// Get drag mode depending on the pointer position over the element
scada.scheme.Dragging.prototype._getDragMode = function (compJqObj, pageX, pageY, singleSelection) {
    var DragModes = scada.scheme.DragModes;
    var component = compJqObj.data("component");

    if (singleSelection && component && component.renderer.allowResizing(component) && false) {
        var elemOffset = compJqObj.offset();
        var elemPtrX = pageX - elemOffset.left;
        var elemPtrY = pageY - elemOffset.top;
        var compW = compJqObj.outerWidth();
        var compH = compJqObj.outerHeight();

        if (compW >= this.MIN_SIZE && compH >= this.MIN_SIZE) {
            // check if the cursor is over the border
            var onTheLeft = elemPtrX <= this.BORDER_WIDTH;
            var onTheRight = elemPtrX >= compW - this.BORDER_WIDTH;
            var onTheTop = elemPtrY <= this.BORDER_WIDTH;
            var atTheBot = elemPtrY >= compH - this.BORDER_WIDTH;

            if (onTheTop && onTheLeft) {
                return DragModes.NW_RESIZE;
            } else if (onTheTop && onTheRight) {
                return DragModes.NE_RESIZE;
            } else if (atTheBot && onTheLeft) {
                return DragModes.SW_RESIZE;
            } else if (atTheBot && onTheRight) {
                return DragModes.SE_RESIZE;
            } else if (onTheLeft) {
                return DragModes.W_RESIZE;
            } else if (onTheRight) {
                return DragModes.E_RESIZE;
            } else if (onTheTop) {
                return DragModes.N_RESIZE;
            } else if (atTheBot) {
                return DragModes.S_RESIZE;
            }
        }
    }

    return DragModes.MOVE;
};
//计算旋转角度
scada.scheme.Dragging.prototype._getAngle = function (o, s, e) {
    const d = e.x - s.x > 0 ? 1 : -1;
    const AB = Math.sqrt(Math.pow(o.x - e.x, 2) + Math.pow(o.y - e.y, 2));
    const AC = Math.sqrt(Math.pow(o.x - s.x, 2) + Math.pow(o.y - s.y, 2));
    const BC = Math.sqrt(Math.pow(e.x - s.x, 2) + Math.pow(e.y - s.y, 2));
    const cosA = (Math.pow(AB, 2) + Math.pow(AC, 2) - Math.pow(BC, 2)) / (2 * AB * AC);
    return (d * Math.round(((Math.acos(cosA) * 180) / Math.PI) * 10)) / 10;
};
//获取旋转角度
scada.scheme.Dragging.prototype._getRotationAngle = function(obj) {
    var matrix = obj.css("transform");
    if(matrix !== 'none') {
        var values = matrix.split('(')[1].split(')')[0].split(',');
        var a = values[0];
        var b = values[1];
        var angle = Math.round(Math.atan2(b, a) * (180/Math.PI));
    } else { var angle = 0; }
    return (angle < 0) ? angle + 360 : angle;
};
scada.scheme.Dragging.prototype._getNewRect = function(point, scale, oTransformedRect, baseIndex) {
    const scaledRect = this._getScaledRect({
      x: point.x,
      y: point.y,
      width: point.width,
      height: point.height,
      scale: scale,
    });
    const transformedRotateRect = this._getTransformPosition(scaledRect, point.rotate || 0);

    // 计算到平移后的新坐标
    const translatedX = oTransformedRect.point[baseIndex].x - transformedRotateRect.point[baseIndex].x + transformedRotateRect.left;
    const translatedY = oTransformedRect.point[baseIndex].y - transformedRotateRect.point[baseIndex].y + transformedRotateRect.top;

    // 计算平移后元素左上角的坐标
    const newX = translatedX + transformedRotateRect.width / 2 - scaledRect.width / 2;
    const newY = translatedY + transformedRotateRect.height / 2 - scaledRect.height / 2;
    // 缩放后元素的高宽
    const newWidth = scaledRect.width;
    const newHeight = scaledRect.height;
    return {
      x: newX,
      y: newY,
      width: newWidth,
      height: newHeight,
    };
  },
  scada.scheme.Dragging.prototype._getScaledRect = function(params) {
    const { x, y, width, height, scale } = params;
    const deltaXScale = scale.x - 1;
    const deltaYScale = scale.y - 1;
    const deltaWidth = width * deltaXScale;
    const deltaHeight = height * deltaYScale;
    const newWidth = width + deltaWidth;
    const newHeight = height + deltaHeight;
    return {
      x: x,
      y: y,
      width: newWidth,
      height: newHeight,
    };
  };
  scada.scheme.Dragging.prototype._getTransformPosition = function(point, angle) {
    const x = point.x;
    const y = point.y;
    const width = point.width || 0;
    const height = point.height || 0;

    const r = Math.sqrt(Math.pow(width, 2) + Math.pow(height, 2)) / 2;
    const a = (Math.atan(height / width) * 180) / Math.PI;
    const tlbra = 180 - angle - a;
    const trbla = a - angle;
    const ta = 90 - angle;
    const ra = angle;

    const halfWidth = width / 2;
    const halfHeight = height / 2;

    const middleX = x + halfWidth;
    const middleY = y + halfHeight;

    const topLeft = {
      x: middleX + r * Math.cos((tlbra * Math.PI) / 180),
      y: middleY - r * Math.sin((tlbra * Math.PI) / 180),
    };
    const top = {
      x: middleX + halfHeight * Math.cos((ta * Math.PI) / 180),
      y: middleY - halfHeight * Math.sin((ta * Math.PI) / 180),
    };
    const topRight = {
      x: middleX + r * Math.cos((trbla * Math.PI) / 180),
      y: middleY - r * Math.sin((trbla * Math.PI) / 180),
    };
    const right = {
      x: middleX + halfWidth * Math.cos((ra * Math.PI) / 180),
      y: middleY + halfWidth * Math.sin((ra * Math.PI) / 180),
    };
    const bottomRight = {
      x: middleX - r * Math.cos((tlbra * Math.PI) / 180),
      y: middleY + r * Math.sin((tlbra * Math.PI) / 180),
    };
    const bottom = {
      x: middleX - halfHeight * Math.sin((ra * Math.PI) / 180),
      y: middleY + halfHeight * Math.cos((ra * Math.PI) / 180),
    };
    const bottomLeft = {
      x: middleX - r * Math.cos((trbla * Math.PI) / 180),
      y: middleY + r * Math.sin((trbla * Math.PI) / 180),
    };
    const left = {
      x: middleX - halfWidth * Math.cos((ra * Math.PI) / 180),
      y: middleY - halfWidth * Math.sin((ra * Math.PI) / 180),
    };
    const minX = Math.min(topLeft.x, topRight.x, bottomRight.x, bottomLeft.x);
    const maxX = Math.max(topLeft.x, topRight.x, bottomRight.x, bottomLeft.x);
    const minY = Math.min(topLeft.y, topRight.y, bottomRight.y, bottomLeft.y);
    const maxY = Math.max(topLeft.y, topRight.y, bottomRight.y, bottomLeft.y);
    return {
      point: [topLeft, top, topRight, right, bottomRight, bottom, bottomLeft, left],
      width: maxX - minX,
      height: maxY - minY,
      left: minX,
      right: maxX,
      top: minY,
      bottom: maxY,
    };
  }
// Move element horizontally during dragging
scada.scheme.Dragging.prototype._moveElemHor = function (compJqObj, dx) {
    this.lastDx = dx;
    this.moved = true;
    var component = compJqObj.data("component");
    var startLocation = compJqObj.data("start-location");
    var location = component.renderer.getLocation(component);
    component.renderer.setLocation(component, startLocation.x + dx, location.y);
};

// Move element vertically during dragging
scada.scheme.Dragging.prototype._moveElemVert = function (compJqObj, dy) {
    this.lastDy = dy;
    this.moved = true;
    var component = compJqObj.data("component");
    var startLocation = compJqObj.data("start-location");
    var location = component.renderer.getLocation(component);
    component.renderer.setLocation(component, location.x, startLocation.y + dy);
};

// Resize element during dragging
scada.scheme.Dragging.prototype._resizeElem = function (compJqObj, width, height) {
    this.lastW = width;
    this.lastH = height;
    this.resized = true;
    var component = compJqObj.data("component");
    this._defineResizePoint(compJqObj.parent(),width,height)
    

    component.renderer.setSize(component, width, height);
};

// Resize points during dragging
scada.scheme.Dragging.prototype._defineResizePoint = function (compJqObj, width, height) {
    //添加旋转
    if (compJqObj.find('.dzr-rotate').length == 0) {
        compJqObj.append($(`<div class="dzr-rotate"><svg viewBox="0 0 1024 1024" version="1.1" xmlns="http://www.w3.org/2000/svg"><path data-v-c2cb7cf8="" d="M929 849a30 30 0 0 1-30-30v-83.137a447.514 447.514 0 0 1-70.921 92.209C722.935 933.225 578.442 975.008 442 953.482a444.917 444.917 0 0 1-241.139-120.591 30 30 0 1 1 37.258-47.01l0.231-0.231A385.175 385.175 0 0 0 442 892.625v-0.006c120.855 22.123 250.206-13.519 343.656-106.975a386.646 386.646 0 0 0 70.6-96.653h-87.247a30 30 0 0 1 0-60H929a30 30 0 0 1 30 30V819a30 30 0 0 1-30 30zM512 392a120 120 0 1 1-120 120 120 120 0 0 1 120-120z m293.005-147.025a29.87 29.87 0 0 1-19.117-6.882l-0.232 0.231A386.5 386.5 0 0 0 689.478 168h-0.011c-145.646-75.182-329.021-51.747-451.117 70.35a386.615 386.615 0 0 0-70.6 96.65H255a30 30 0 0 1 0 60H95a30 30 0 0 1-30-30V205a30 30 0 0 1 60 0v83.129a447.534 447.534 0 0 1 70.923-92.206C317.981 73.866 493.048 37.2 647 85.836v-0.045a444.883 444.883 0 0 1 176.143 105.291 30 30 0 0 1-18.138 53.893z" fill="#0f0"></path></svg></div>`))
    }
    //添加缩放
    var curRotate = this._getRotationAngle(compJqObj);
    compJqObj.data("rotate",curRotate);
    if (compJqObj.find('.dzr-zoom').length == 0) {
        var zoomDiv = $(`<div class="dzr-zoom"></div>`);
        var width = compJqObj.width(), height = compJqObj.height();
        var tlpoint = $('<div id="resize_tl" class="dzr-resize-point touch-none" style="cursor: nw-resize;"></div>').css({ "left": "0px", "top": "0px" })
        var tcpoint = $('<div id="resize_tc" class="dzr-resize-point touch-none" style="cursor: n-resize;"></div>').css({ "left": width / 2 + "px", "top": "0px" })
        var trpoint = $('<div id="resize_tr" class="dzr-resize-point touch-none" style="cursor: ne-resize;"></div>').css({ "left": width + "px", "top": "0px" })
        var lpoint = $('<div id="resize_l" class="dzr-resize-point touch-none" style="cursor: w-resize;"></div>').css({ "left": "0px", "top": height / 2 + "px" })
        var rpoint = $('<div id="resize_r" class="dzr-resize-point touch-none" style="cursor: e-resize;"></div>').css({ "left": width + "px", "top": height / 2 + "px" })
        var blpoint = $('<div id="resize_bl" class="dzr-resize-point touch-none" style="cursor: sw-resize;"></div>').css({ "left": "0px", "top": height + "px" })
        var bcpoint = $('<div id="resize_bc" class="dzr-resize-point touch-none" style="cursor: s-resize;"></div>').css({ "left": width / 2 + "px", "top": height + "px" })
        var brpoint = $('<div id="resize_br" class="dzr-resize-point touch-none" style="cursor: se-resize;"></div>').css({ "left": width + "px", "top": height + "px" })
        zoomDiv.append(tlpoint).append(tcpoint).append(trpoint)
            .append(lpoint).append(rpoint)
            .append(blpoint).append(bcpoint).append(brpoint);
        compJqObj.append(zoomDiv)
    }
    else if (compJqObj.find('.dzr-zoom').length > 0) {
        compJqObj.find('.dzr-zoom #resize_tl').css({ "left": "0px", "top": "0px" })
        compJqObj.find('.dzr-zoom #resize_tc').css({ "left": width / 2 + "px", "top": "0px" })
        compJqObj.find('.dzr-zoom #resize_tr').css({ "left": width + "px", "top": "0px" })
        compJqObj.find('.dzr-zoom #resize_l').css({ "left": "0px", "top": height / 2 + "px" })
        compJqObj.find('.dzr-zoom #resize_r').css({ "left": width + "px", "top": height / 2 + "px" })
        compJqObj.find('.dzr-zoom #resize_bl').css({ "left": "0px", "top": height + "px" })
        compJqObj.find('.dzr-zoom #resize_bc').css({ "left": width / 2 + "px", "top": height + "px" })
        compJqObj.find('.dzr-zoom #resize_br').css({ "left": width + "px", "top": height + "px" })
    }
};

// Define the cursor depending on the pointer position
scada.scheme.Dragging.prototype.defineCursor = function (jqObj, pageX, pageY, singleSelection) {
    var DragModes = scada.scheme.DragModes;
    var compElem = jqObj.is(".comp-wrapper") ? jqObj.children(".comp") : jqObj.closest(".comp");

    if (compElem.length > 0) {
        var cursor = "";

        if (compElem.parent(".comp-wrapper").is(".selected")) {
            var dragMode = this._getDragMode(compElem, pageX, pageY, singleSelection);

            if (dragMode === DragModes.NW_RESIZE || dragMode === DragModes.SE_RESIZE) {
                cursor = "nwse-resize";
            } else if (dragMode === DragModes.NE_RESIZE || dragMode === DragModes.SW_RESIZE) {
                cursor = "nesw-resize";
            } else if (dragMode === DragModes.E_RESIZE || dragMode === DragModes.W_RESIZE) {
                cursor = "e,w-resize";
            } else if (dragMode === DragModes.N_RESIZE || dragMode === DragModes.S_RESIZE) {
                cursor = "ns-resize";
            } else {
                cursor = "move";
            }
        }

        jqObj.css("cursor", cursor);
    }
};

// Start dragging the specified elements
scada.scheme.Dragging.prototype.startDragging = function (captCompJqObj, selCompJqObj, pageX, pageY, mode) {
    var DragModes = scada.scheme.DragModes;

    this.mode = this._getDragMode(captCompJqObj, pageX, pageY, selCompJqObj.length <= 1);
    this.startX = pageX;
    this.startY = pageY;
    this.lastDx = 0;
    this.lastDy = 0;
    this.lastW = 0;
    this.lastH = 0;
    this.draggedElem = selCompJqObj;
    this.moved = false;
    this.resized = false;
    this.rotated = false;
    if (mode === DragModes.ROTATE) {
        this.rotated = true;
        this.mode = DragModes.ROTATE
    }
    if(mode >= 2 && mode <=9 ){
        this.mode = mode
    }

    // save starting offset and size of the dragged components
    var thisObj = this;
    this.draggedElem.each(function () {
        var elem = $(this);
        var component = elem.data("component");
        elem.data("start-location", component.renderer.getLocation(component));

        if (thisObj.mode > DragModes.MOVE) {
            elem.data("start-size", component.renderer.getSize(component));
        }
    });
};

// Continue dragging
scada.scheme.Dragging.prototype.continueDragging = function (pageX, pageY) {
    var DragModes = scada.scheme.DragModes;
    var thisObj = this;
    var dx = pageX - this.startX;
    var dy = pageY - this.startY;

    //辅助线
    if (this.draggedElem.length > 0 && this.mode === DragModes.MOVE && (this.moved || this.resized)) {
        var tsi, bsi, lsi, rsi, tso, bso, lso, rso, l, r, t, b, i, inner;
        this.draggedElem.each(function () {
            let that = this;
            let parentElem = $(that).parent();
            var startLocation = $(that).data("start-location");
            var rotate = parentElem.data("rotate");
            var elemRect = $(parentElem)[0].getBoundingClientRect();
            var x1 = elemRect.x, x2 = x1 + elemRect.width - 2;
            var y1 = elemRect.y, y2 = y1 + elemRect.height - 2;
            var guideDom = document.querySelector('#guide');
            $('.comp-wrapper:not(.selected)').each(function () {
                l = $(this).offset().left;
                r = l + $(this).outerWidth();
                t = $(this).offset().top;
                b = t + $(this).outerHeight();
                if (!(tsi || bsi || tso || bso)) {
                    tsi = Math.abs(t - y2) < thisObj.MIN_GUIDE_LEN;
                    bsi = Math.abs(b - y1) < thisObj.MIN_GUIDE_LEN;
                    if (tsi) guideDom.style.setProperty("--guide-top", y2 + "px");
                    if (bsi) guideDom.style.setProperty("--guide-top", y1 + "px");
                    tso = Math.abs(t - y1) < thisObj.MIN_GUIDE_LEN;
                    bso = Math.abs(b - y2) < thisObj.MIN_GUIDE_LEN;
                    if (tso) guideDom.style.setProperty("--guide-top", y1 + "px");
                    if (bso) guideDom.style.setProperty("--guide-top", y2 + "px");
                }
                if (!(lsi || rsi || lso || rso)) {
                    lsi = Math.abs(l - x2) < thisObj.MIN_GUIDE_LEN;
                    rsi = Math.abs(r - x1) < thisObj.MIN_GUIDE_LEN;
                    if (lsi) guideDom.style.setProperty("--guide-left", x2 + "px");
                    if (rsi) guideDom.style.setProperty("--guide-left", x1 + "px");
                    inner = (tsi || bsi || lsi || rsi);
                    //outer align
                    lso = Math.abs(l - x1) < thisObj.MIN_GUIDE_LEN;
                    rso = Math.abs(r - x2) < thisObj.MIN_GUIDE_LEN;
                    if (lso) guideDom.style.setProperty("--guide-left", x1 + "px");
                    if (rso) guideDom.style.setProperty("--guide-left", x2 + "px");
                }
            })
            if (tsi || bsi || tso || bso) guideDom.style.setProperty("--guide-x", "blcok");
            else guideDom.style.setProperty("--guide-x", "none");
            if (lsi || rsi || lso || rso) guideDom.style.setProperty("--guide-y", "blcok");
            else guideDom.style.setProperty("--guide-y", "none");
        });
    }
    if (this.draggedElem.length > 0 &&
        (this.moved || this.resized || this.rotated || Math.abs(dx) >= this.MIN_MOVING || Math.abs(dy) >= this.MIN_MOVING)) {
        if (this.mode === DragModes.MOVE) {
            // move elements
            this.lastDx = dx;
            this.lastDy = dy;
            this.moved = true;
            this.draggedElem.each(function () {
                var component = $(this).data("component");
                var startLocation = $(this).data("start-location");
                component.renderer.setLocation(component, startLocation.x + dx, startLocation.y + dy);
            });
        } else if (this.mode === DragModes.ROTATE) {
            // move elements
            this.lastDx = dx;
            this.lastDy = dy;
            this.rotated = true;
            this.draggedElem.each(function () {
                var selComp = $(this).parent();
                var rect = selComp[0].getBoundingClientRect();
                let originPoint = {
                    x: rect.x + rect.width / 2,
                    y: rect.y + rect.height / 2,
                }
                let startPoint = {
                    x: originPoint.x, y: originPoint.y - 200,
                }
                var angle = Math.ceil(thisObj._getAngle(originPoint, startPoint, { x: pageX, y: pageY }))
                $(this).parent().css("transform", "rotate(" + angle + "deg)")
                thisObj.lastRotate = angle
            });
        } else {
            var resizeLeft = this.mode === DragModes.NW_RESIZE ||
                this.mode === DragModes.SW_RESIZE || this.mode === DragModes.W_RESIZE;
            var resizeRight = this.mode === DragModes.NE_RESIZE ||
                this.mode === DragModes.SE_RESIZE || this.mode === DragModes.E_RESIZE;
            var resizeTop = this.mode === DragModes.NW_RESIZE ||
                this.mode === DragModes.NE_RESIZE || this.mode === DragModes.N_RESIZE;
            var resizeBot = this.mode === DragModes.SW_RESIZE ||
                this.mode === DragModes.SE_RESIZE || this.mode === DragModes.S_RESIZE;
            var elem = this.draggedElem.eq(0);
            var startSize = elem.data("start-size");
            var newWidth = startSize.width;
            var newHeight = startSize.height;

            if (resizeLeft) {
                // resize by pulling the left edge
                newWidth = Math.max(newWidth - dx, this.MIN_SIZE);
                this._moveElemHor(elem, Math.min(dx, startSize.width - this.MIN_SIZE));
                this._resizeElem(elem, newWidth, newHeight);
            } else if (resizeRight) {
                // resize by pulling the right edge
                newWidth = Math.max(newWidth + dx, this.MIN_SIZE);
                this._resizeElem(elem, newWidth, newHeight);
            }

            if (resizeTop) {
                // resize by pulling the top edge
                newHeight = Math.max(newHeight - dy, this.MIN_SIZE);
                this._moveElemVert(elem, Math.min(dy, startSize.height - this.MIN_SIZE));
                this._resizeElem(elem, newWidth, newHeight);
            } else if (resizeBot) {
                // resize by pulling the bottom edge
                newHeight = Math.max(newHeight + dy, this.MIN_SIZE);
                this._resizeElem(elem, newWidth, newHeight);
            }
        }
    }
};

// Stop dragging.
// callback is a function (dx, dy, w, h)
scada.scheme.Dragging.prototype.stopDragging = function (callback) {
    let that = this;

    var guideDom = document.querySelector('#guide');
    guideDom.style.setProperty("--guide-x", "none");
    guideDom.style.setProperty("--guide-y", "none");
    // clear starting offsets and sizes
    this.draggedElem.each(function () {
        $(this)
            .removeData("start-location")
            .removeData("start-size");
    });
    this.mode = scada.scheme.DragModes.NONE;

    // execute callback function
    if(this.rotated && typeof callback === "function"){
        callback(0, 0, 0, 0, this.lastRotate);
    }
    else if ((this.moved || this.resized || this.rotated) && typeof callback === "function") {
        callback(this.lastDx, this.lastDy, this.lastW, this.lastH,undefined);
    }
};

// Get status of dragging
scada.scheme.Dragging.prototype.getStatus = function () {
    var DragModes = scada.scheme.DragModes;

    if (this.mode === DragModes.NONE) {
        return "";
    } else {
        var component = this.draggedElem.data("component");
        var location = component.renderer.getLocation(component);
        var locationStr = "X: " + location.x + ", Y: " + location.y;

        if (this.mode === DragModes.MOVE) {
            return locationStr;
        } else {
            var size = component.renderer.getSize(component);
            return locationStr + ", W: " + size.width + ", H: " + size.height;
        }
    }
};

/********** Editable Scheme **********/

// Editable scheme type
scada.scheme.EditableScheme = function () {
    scada.scheme.Scheme.call(this);
    this.editMode = true;

    // Editor grid step
    this.GRID_STEP = 5;

    // Stamp of the last processed change
    this.lastChangeStamp = 0;
    // Adding new component mode
    this.newComponentMode = false;
    // IDs of the selected components
    this.selComponentIDs = [];
    // Provides dragging and resizing
    this.dragging = new scada.scheme.Dragging();
    // Useful information for a user
    this.status = "";
};

scada.scheme.EditableScheme.prototype = Object.create(scada.scheme.Scheme.prototype);
scada.scheme.EditableScheme.constructor = scada.scheme.EditableScheme;

// Apply the received scheme changes
scada.scheme.EditableScheme.prototype._processChanges = function (changes) {
    var SchemeChangeTypes = scada.scheme.SchemeChangeTypes;

    for (var change of changes) {
        var changedObject = change.ChangedObject;

        switch (change.ChangeType) {
            case SchemeChangeTypes.SCHEME_DOC_CHANGED:
                this._updateSchemeProps(changedObject);
                break;
            case SchemeChangeTypes.COMPONENT_ADDED:
            case SchemeChangeTypes.COMPONENT_CHANGED:
                if (this._validateComponent(changedObject)) {
                    this._updateComponentProps(changedObject);
                }
                break;
            case SchemeChangeTypes.COMPONENT_DELETED:
                var component = this.componentMap.get(change.ComponentID);
                if (component) {
                    this.componentMap.delete(component.id);
                    if (component.dom) {
                        component.dom.parent(".comp-wrapper").remove();
                    }
                }
                break;
            case SchemeChangeTypes.IMAGE_ADDED:
                if (this._validateImage(changedObject)) {
                    this.imageMap.set(changedObject.Name, changedObject);
                    this._refreshImages([changedObject.Name]);
                }
                break;
            case SchemeChangeTypes.IMAGE_RENAMED:
                var image = this.imageMap.get(change.OldImageName);
                if (image) {
                    this.imageMap.delete(change.OldImageName);
                    image.Name = change.ImageName;
                    this.imageMap.set(image.Name, image);
                    this._refreshImages([change.OldImageName, change.ImageName]);
                }
                break;
            case SchemeChangeTypes.IMAGE_DELETED:
                this.imageMap.delete(change.ImageName);
                this._refreshImages([change.ImageName]);
                break;
        }

        this.lastChangeStamp = change.Stamp;
    }
};

// Update the scheme properties
scada.scheme.EditableScheme.prototype._updateSchemeProps = function (parsedSchemeDoc) {
    try {
        this.props = parsedSchemeDoc;
        this.dom.detach();
        this.renderer.updateDom(this, this.renderContext);
        this.parentDomElem.append(this.dom);
    }
    catch (ex) {
        console.error("Error updating scheme properties:", ex.message);
    }
};

// Update the component properties or add the new component
scada.scheme.EditableScheme.prototype._updateComponentProps = function (parsedComponent) {
    try {
        var newComponent = new scada.scheme.Component(parsedComponent);
        var renderer = scada.scheme.rendererMap.get(newComponent.type);
        newComponent.renderer = renderer;

        if (renderer) {
            renderer.createDom(newComponent, this.renderContext);

            if (newComponent.dom) {
                newComponent.dom.first().data("component", newComponent);
                var componentID = parsedComponent.ID;
                var oldComponent = this.componentMap.get(componentID);
                this.componentMap.set(componentID, newComponent);

                if (oldComponent && oldComponent.dom) {
                    // replace component in the DOM
                    oldComponent.dom.replaceWith(newComponent.dom);
                    renderer.setWrapperProps(newComponent);
                } else {
                    // add component into the DOM
                    this.dom.append(renderer.wrap(newComponent));
                }
            }
        }
    }
    catch (ex) {
        console.error("Error updating properties of the component of type '" +
            parsedComponent.TypeName + "' with ID=" + parsedComponent.ID + ":", ex.message);
    }
};

// Refresh scheme components that contain the specified images
scada.scheme.EditableScheme.prototype._refreshImages = function (imageNames) {
    try {
        this.renderer.refreshImages(this, this.renderContext, imageNames);

        for (var component of this.componentMap.values()) {
            if (component.dom) {
                component.renderer.refreshImages(component, this.renderContext, imageNames);
            }
        }
    }
    catch (ex) {
        console.error("Error refreshing scheme images:", ex.message);
    }
};

// Highlight the selected components
scada.scheme.EditableScheme.prototype._processSelection = function (selCompIDs) {
    // add currently selected components to the set
    var idSet = new Set(this.selComponentIDs);

    // process changes of the selection
    var divScheme = this._getSchemeDiv();

    for (var selCompID of selCompIDs) {
        if (idSet.has(selCompID)) {
            idSet.delete(selCompID);
        } else {
            var selComp = divScheme.find("#comp" + selCompID).parent(".comp-wrapper");
            selComp.addClass("selected");
            this.dragging._defineResizePoint(selComp);//定义旋转/缩放
            
        }
    }

    for (var deselCompID of idSet) {
        var selComp = divScheme.find("#comp" + deselCompID).parent(".comp-wrapper")
        selComp.removeClass("selected");
        selComp.find('.dzr-rotate').remove();//移除旋转抓手
        selComp.find('.dzr-zoom').remove();//移除缩放点
    }

    this.selComponentIDs = Array.isArray(selCompIDs) ? selCompIDs : [];
};

// Proccess mode of the editor
scada.scheme.EditableScheme.prototype._processMode = function (mode) {
    mode = !!mode;

    if (this.newComponentMode !== mode) {
        if (mode) {
            this._getSchemeDiv().addClass("new-component-mode");
        } else {
            this._getSchemeDiv().removeClass("new-component-mode");
        }

        this.newComponentMode = mode;
    }
};

// Proccess editor title
scada.scheme.EditableScheme.prototype._processTitle = function (editorTitle) {
    if (editorTitle && document.title !== editorTitle) {
        document.title = editorTitle;
    }
};

// Proccess editor form state
scada.scheme.EditableScheme.prototype._processFormState = function (opt_formState) {
    var divSchWrapper = this._getSchemeDiv().closest(".scheme-wrapper");
    var prevFormState = divSchWrapper.data("form-state");
    var stickToLeft = prevFormState ? prevFormState.StickToLeft : false;
    var stickToRight = prevFormState ? prevFormState.StickToRight : false;
    var width = prevFormState ? prevFormState.Width : 0;
    var changed = false;

    if (opt_formState && opt_formState.StickToLeft && opt_formState.Width > 0) {
        if (!(stickToLeft && width === opt_formState.Width)) {
            // add space to the left
            changed = true;
            divSchWrapper.css({
                "border-left-width": opt_formState.Width,
                "border-right-width": 0
            });
        }
    } else if (opt_formState && opt_formState.StickToRight && opt_formState.Width > 0) {
        if (!(stickToRight && width === opt_formState.Width)) {
            // add space to the right
            changed = true;
            divSchWrapper.css({
                "border-left-width": 0,
                "border-right-width": opt_formState.Width
            });
        }
    } else if (stickToLeft || stickToRight) {
        // remove space
        changed = true;
        divSchWrapper.css({
            "border-left-width": 0,
            "border-right-width": 0
        });
    }

    if (changed) {
        if (opt_formState) {
            divSchWrapper.data("form-state", opt_formState);
        } else {
            divSchWrapper.removeData("form-state");
        }

        divSchWrapper.outerWidth($(window).width());
    }
};

// Get the main div element of the scheme
scada.scheme.EditableScheme.prototype._getSchemeDiv = function () {
    return this.dom ? this.dom.first() : $();
};

// Send a request to add a new component to the scheme
scada.scheme.EditableScheme.prototype._addComponent = function (x, y) {
    var operation = this.serviceUrl + "AddComponent";

    $.ajax({
        url: operation +
            "?editorID=" + this.editorID +
            "&viewStamp=" + this.viewStamp +
            "&x=" + x +
            "&y=" + y,
        method: "GET",
        dataType: "json",
        cache: false
    })
        .done(function () {
            scada.utils.logSuccessfulRequest(operation);
        })
        .fail(function (jqXHR) {
            scada.utils.logFailedRequest(operation, jqXHR);
        });
};

// Send a request to change scheme component selection
scada.scheme.EditableScheme.prototype._changeSelection = function (action, opt_componentID) {
    var operation = this.serviceUrl + "ChangeSelection";

    $.ajax({
        url: operation +
            "?editorID=" + this.editorID +
            "&viewStamp=" + this.viewStamp +
            "&action=" + action +
            "&componentID=" + (opt_componentID ? opt_componentID : "-1"),
        method: "GET",
        dataType: "json",
        cache: false
    })
        .done(function () {
            scada.utils.logSuccessfulRequest(operation);
        })
        .fail(function (jqXHR) {
            scada.utils.logFailedRequest(operation, jqXHR);
        });
};

// Send a request to move and resize selected scheme components
scada.scheme.EditableScheme.prototype._moveResize = function (dx, dy, w, h) {
    var operation = this.serviceUrl + "MoveResize";

    $.ajax({
        url: operation +
            "?editorID=" + this.editorID +
            "&viewStamp=" + this.viewStamp +
            "&dx=" + dx +
            "&dy=" + dy +
            "&w=" + w +
            "&h=" + h,
        method: "GET",
        dataType: "json",
        cache: false
    })
        .done(function () {
            scada.utils.logSuccessfulRequest(operation);
        })
        .fail(function (jqXHR) {
            scada.utils.logFailedRequest(operation, jqXHR);
        });
};
// Send a request to rotate selected scheme components
scada.scheme.EditableScheme.prototype._rotateComp = function (r) {
    var operation = this.serviceUrl + "RotateComp";

    $.ajax({
        url: operation +
            "?editorID=" + this.editorID +
            "&viewStamp=" + this.viewStamp +
            "&r=" + r,
        method: "GET",
        dataType: "json",
        cache: false
    })
        .done(function () {
            scada.utils.logSuccessfulRequest(operation);
        })
        .fail(function (jqXHR) {
            scada.utils.logFailedRequest(operation, jqXHR);
        });
};

// Send a request to perform action of the main form
scada.scheme.EditableScheme.prototype._formAction = function (action) {
    var operation = this.serviceUrl + "FormAction";

    $.ajax({
        url: operation +
            "?editorID=" + this.editorID +
            "&viewStamp=" + this.viewStamp +
            "&action=" + action,
        method: "GET",
        dataType: "json",
        cache: false
    })
        .done(function () {
            scada.utils.logSuccessfulRequest(operation);
        })
        .fail(function (jqXHR) {
            scada.utils.logFailedRequest(operation, jqXHR);
        });
};

// Send a request to save aux data for component to the scheme
scada.scheme.EditableScheme.prototype._saveComponentAuxData = function (compId, auxData) {
    var operation = this.serviceUrl + "SetAuxData";
    let that = this;
    $.ajax({
        url: operation,
        method: 'POST',
        data: JSON.stringify({
            "editorID": that.editorID,
            "viewStamp": that.viewStamp,
            "auxData": auxData
        }),
        contentType: "application/json;charset=utf-8",
        dataType: 'json',
        processData: true,
        success: function (response) {
            console.log(response);
        },
        error: function (xhr, textStatus, errorThrown) {
            console.error(errorThrown);
        }
    });
};

// Create DOM content of the scheme
scada.scheme.EditableScheme.prototype.createDom = function (opt_controlRight) {
    scada.scheme.Scheme.prototype.createDom.call(this, opt_controlRight);
    var SelectActions = scada.scheme.SelectActions;
    var DragModes = scada.scheme.DragModes;
    var thisScheme = this;

    // store references to the components in the DOM
    for (var component of this.componentMap.values()) {
        if (component.dom) {
            component.dom.first().data("component", component);
        }
    }

    // bind events for dragging
    var divScheme = this._getSchemeDiv();
    divScheme
        .on("mousedown", function (event) {
            if (thisScheme.newComponentMode) {
                // add new component
                var offset = divScheme.offset();
                thisScheme._addComponent(event.pageX - parseInt(offset.left), event.pageY - parseInt(offset.top));
            } else {
                // deselect all components
                console.log(scada.utils.getCurTime() + " Scheme background is clicked.");
                thisScheme._changeSelection(SelectActions.DESELECT_ALL);
            }
        })
        .on("mousedown", ".dzr-rotate", function (event) {
            event.mode = DragModes.ROTATE;//强制旋转模式
        })
        .on("mousedown",".dzr-zoom .dzr-resize-point",function(event){
            var curPointId = $(this).attr('id');
            switch(curPointId){
                case "resize_tl": event.mode = DragModes.NW_RESIZE;break;
                case "resize_tc": event.mode = DragModes.N_RESIZE;break;
                case "resize_tr": event.mode = DragModes.NE_RESIZE;break;
                case "resize_l":  event.mode = DragModes.W_RESIZE;break;
                case "resize_r":  event.mode = DragModes.E_RESIZE;break;
                case "resize_bl": event.mode = DragModes.SW_RESIZE;break;
                case "resize_bc": event.mode = DragModes.S_RESIZE;break;
                case "resize_br": event.mode = DragModes.SE_RESIZE;break;
            }
        })
        .on("mousedown", ".comp-wrapper", function (event) {
            if (!thisScheme.newComponentMode) {
                // select or deselect component and start dragging
                var compElem = $(this).children(".comp");
                var componentID = compElem.data("id");
                var selected = $(this).hasClass("selected");
                console.log(scada.utils.getCurTime() + " Component with ID=" + componentID + " is clicked.");
                if (event.ctrlKey) {
                    thisScheme._changeSelection(
                        selected ? SelectActions.DESELECT : SelectActions.APPEND,
                        componentID);
                } else {
                    if (!selected) {
                        divScheme.find(".comp-wrapper.selected").removeClass("selected");
                        $(this).addClass("selected");
                        thisScheme._changeSelection(SelectActions.SELECT, componentID);
                    }

                    thisScheme.dragging.startDragging(
                        compElem, divScheme.find(".comp-wrapper.selected .comp"), event.pageX, event.pageY, event.mode);
                }

                event.stopPropagation();
            }
        })
        .on("mousemove", "*:not(.dzr-rotate)", function (event) {
            if (thisScheme.dragging.mode === DragModes.NONE) {
                thisScheme.dragging.defineCursor($(event.target), event.pageX, event.pageY,
                    thisScheme.selComponentIDs.length <= 1);

                if (thisScheme.newComponentMode) {
                    var offset = divScheme.offset();
                    thisScheme.status = "X: " + (event.pageX - parseInt(offset.left)) +
                        ", Y: " + (event.pageY - parseInt(offset.top));
                } else {
                    thisScheme.status = "";
                }
            } else {
                thisScheme.dragging.continueDragging(event.pageX, event.pageY);
                thisScheme.status = thisScheme.dragging.getStatus();
            }
        })
        .on("mousemove", ".dzr-rotate", function (event) {
            if (thisScheme.dragging.mode === DragModes.ROTATE) {
                thisScheme.dragging.continueDragging(event.pageX, event.pageY);
                //thisScheme.status = thisScheme.dragging.getStatus();
            }
        })
        .on("mouseup mouseleave", function (event) {
            if (thisScheme.dragging.mode !== DragModes.NONE) {
                thisScheme.dragging.stopDragging(function (dx, dy, w, h, r) {
                    // send changes to server under the assumption that the selection was not changed during dragging
                    if(r != undefined) thisScheme._rotateComp(r);
                    else thisScheme._moveResize(dx, dy, w, h);
                });
                thisScheme.status = "";
            }
        })
        .on("saveAuxData", function (event, obj) {
            thisScheme._saveComponentAuxData(obj.compId, obj.svgData)
        })
        .on("selectstart", ".comp-wrapper", false)
        .on("dragstart", false);
};

// Iteration of getting scheme changes
// callback is a function (result)
scada.scheme.EditableScheme.prototype.getChanges = function (callback) {
    var GetChangesResults = scada.scheme.GetChangesResults;
    var operation = this.serviceUrl + "GetChanges";
    var thisScheme = this;

    $.ajax({
        url: operation +
            "?editorID=" + this.editorID +
            "&viewStamp=" + this.viewStamp +
            "&changeStamp=" + this.lastChangeStamp +
            "&status=" + encodeURIComponent(this.status ? this.status : " "), // space needed by Mono
        method: "GET",
        dataType: "json",
        cache: false
    })
        .done(function (data, textStatus, jqXHR) {
            try {
                var parsedData = $.parseJSON(data.d);
                if (parsedData.Success) {
                    scada.utils.logSuccessfulRequest(operation);
                    thisScheme._processFormState(parsedData.FormState);

                    if (parsedData.EditorUnknown) {
                        console.error(scada.utils.getCurTime() + " Editor is unknown. Normal operation is impossible.");
                        callback(GetChangesResults.EDITOR_UNKNOWN);
                    } else if (thisScheme.viewStamp && parsedData.ViewStamp) {
                        if (thisScheme.viewStamp === parsedData.ViewStamp) {
                            thisScheme._processChanges(parsedData.Changes);
                            thisScheme._processSelection(parsedData.SelCompIDs);
                            thisScheme._processMode(parsedData.NewCompMode);
                            thisScheme._processTitle(parsedData.EditorTitle);
                            callback(GetChangesResults.SUCCESS);
                        } else {
                            console.log(scada.utils.getCurTime() + " View stamps are different. Need to reload scheme.");
                            callback(GetChangesResults.RELOAD_SCHEME);
                        }
                    } else {
                        console.error(scada.utils.getCurTime() + " View stamp is undefined on client or server side.");
                        callback(GetChangesResults.DATA_ERROR);
                    }
                } else {
                    scada.utils.logServiceError(operation, parsedData.ErrorMessage);
                    callback(GetChangesResults.DATA_ERROR);
                }
            }
            catch (ex) {
                scada.utils.logProcessingError(operation, ex.message);
                callback(GetChangesResults.DATA_ERROR);
            }
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            scada.utils.logFailedRequest(operation, jqXHR);
            thisScheme._processFormState();
            callback(GetChangesResults.COMM_ERROR);
        });
};

// Perform an action depending on the pressed key. Returns false if the key is handled
scada.scheme.EditableScheme.prototype.processKey = function (keyChar, keyCode, ctrlKey) {
    var DragModes = scada.scheme.DragModes;
    var FormActions = scada.scheme.FormActions;

    // use keyCode instead of keyChar to provide case insensitiveness and culture independence
    if (37 <= keyCode && keyCode <= 40 /*arrow keys*/ &&
        this.dragging.mode === DragModes.NONE) {
        // move selected components
        var move = ctrlKey ? 1 : this.GRID_STEP;
        var dx = 0;
        var dy = 0;

        if (keyCode === 37 /*left arrow*/) {
            dx = -move;
        } else if (keyCode === 39 /*right arrow*/) {
            dx = move;
        } else if (keyCode === 38 /*up arrow*/) {
            dy = -move;
        } else if (keyCode === 40 /*down arrow*/) {
            dy = move;
        }

        this._getSchemeDiv().find(".comp-wrapper.selected").each(function () {
            var offset = $(this).offset();
            $(this).offset({ left: offset.left + dx, top: offset.top + dy });
        });

        // send changes to server
        this._moveResize(dx, dy, 0, 0);
    } else if (ctrlKey) {
        if (keyCode === 78 /*N*/) {
            this._formAction(FormActions.NEW); // doesn't work
        } else if (keyCode === 79 /*O*/) {
            this._formAction(FormActions.OPEN);
        } else if (keyCode === 83 /*S*/) {
            this._formAction(FormActions.SAVE);
        } else if (keyCode === 88 /*X*/) {
            this._formAction(FormActions.CUT);
        } else if (keyCode === 67 /*C*/) {
            this._formAction(FormActions.COPY);
        } else if (keyCode === 86 /*V*/) {
            this._formAction(FormActions.PASTE);
        } else if (keyCode === 90 /*Z*/) {
            this._formAction(FormActions.UNDO);
        } else if (keyCode === 89 /*Y*/) {
            this._formAction(FormActions.REDO);
        } else {
            return true;
        }
    } else if (keyCode === 27 /*Escape*/) {
        this._formAction(FormActions.POINTER);
    } else if (keyCode === 46 /*Delete*/) {
        this._formAction(FormActions.DELETE);
    } else {
        return true;
    }

    return false;
};
/* //添加辅助线
$(function () {
    $("#guide").remove();
    $('body').append($(`
    <div id="guide"
         style="--guide-scale: scale(1); --guide-padding: 0px 0px; --guide-width: 1920px; --guide-height: 1080px; --guide-ml: 0px; --guide-mt: 0px; --guide-x: none; --guide-left: 0px; --guide-y: none; --guide-top: 0px;">
        <div id="guide-x"></div>
        <div id="guide-y"></div>
    </div>`))
})
*/