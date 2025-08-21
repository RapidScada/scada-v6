// Handles the mimic view page.
// Depends on jquery, scada-common.js, view-hub.js, main-api.js, mimic-common.js, mimic-model.js, mimic-render.js

class MimicDataProvider extends rs.mimic.DataProvider {
    getCurData = (cnlNum, opt_joinLen) =>
        MainApi.getCurDataFromMap(this.curDataMap, cnlNum, opt_joinLen);
    getPrevData = (cnlNum, opt_joinLen) =>
        MainApi.getCurDataFromMap(this.prevDataMap, cnlNum, opt_joinLen);
}

const viewHub = ViewHub.getInstance();
const mainApi = new MainApi({ rootPath: viewHub.appEnv.rootPath });
const mimic = new rs.mimic.Mimic();
const unitedRenderer = new rs.mimic.UnitedRenderer(mimic, false);
const dataProvider = new MimicDataProvider();

// Set in MimicView.cshtml
var viewID = 0;
var controlRight = false;
var refreshRate = 1000;
var fonts = [];
var runtimeOptions = {};
var phrases = {};

let errorElem = $();
let spinnerElem = $();
let mimicWrapperElem = $();
let toolbarElem = $();

let cnlListID = 0;
let scale = new rs.mimic.Scale();
let errorTimeoutID = 0;

function initElements() {
    errorElem = $("#divError");
    spinnerElem = $("#divSpinner");
    mimicWrapperElem = $("#divMimicWrapper");
    toolbarElem = $("#divToolbar");
}

function bindEvents() {
    $(window).on("resize", function () {
        updateLayout();

        if (scale.type !== rs.mimic.ScaleType.NUMERIC) {
            updateScale();
        }
    });

    $(document).on("keydown", function (event) {
        let targetElem = $(event.target);

        if ((targetElem.is("body") || targetElem.closest("#divToolbar").length > 0) &&
            handleKeyDown(event.code, event.ctrlKey)) {
            event.preventDefault();
        }
    });

    $("#btnFitScreen").on("click", function () {
        scale = new rs.mimic.Scale(rs.mimic.ScaleType.FIT_SCREEN);
        updateScale();
    });

    $("#btnFitWidthBtn").on("click", function () {
        scale = new rs.mimic.Scale(rs.mimic.ScaleType.FIT_WIDTH);
        updateScale();
    });

    $("#btnZoomOutBtn").on("click", function () {
        scale = scale.getPrev();
        updateScale();
    });

    $("#btnZoomInBtn").on("click", function () {
        scale = scale.getNext();
        updateScale();
    });
}

function bindGestureEvents() {
    // when zoom in quickly, glitches may occur because the scrollable area does not expand fast enough
    let initialCenter = null;
    let initialDistance = 0;
    let initialScale = 0;
    let initialScroll = null;

    let getCenter = (event) => {
        let touch1 = event.touches[0];
        let touch2 = event.touches[1];

        return {
            x: (touch1.clientX + touch2.clientX) / 2,
            y: (touch1.clientY + touch2.clientY) / 2
        };
    };

    let getDistance = (event) => {
        let touch1 = event.touches[0];
        let touch2 = event.touches[1];

        return Math.hypot(
            touch2.clientX - touch1.clientX,
            touch2.clientY - touch1.clientY
        );
    };

    mimicWrapperElem
        .on("touchstart", function (event) {
            if (event.touches.length === 2) {
                initialCenter = getCenter(event);
                initialDistance = getDistance(event);
                initialScale = scale.value;
                initialScroll = {
                    x: mimicWrapperElem.scrollLeft(),
                    y: mimicWrapperElem.scrollTop()
                };
                event.preventDefault();
            }
        })
        .on("touchmove", function (event) {
            if (event.touches.length === 2 && initialCenter && initialDistance > 0 && initialScale > 0) {
                // measure gesture
                let currentCenter = getCenter(event);
                let currentDistance = getDistance(event);

                // update scale
                let newScale = initialScale * currentDistance / initialDistance;
                scale = new rs.mimic.Scale(rs.mimic.ScaleType.NUMERIC, newScale);
                updateScale();

                // scroll
                let scaleRatio = scale.value / initialScale;
                let scrollLeft = Math.round((initialScroll.x + initialCenter.x) * scaleRatio - currentCenter.x);
                let scrollTop = Math.round((initialScroll.y + initialCenter.y) * scaleRatio - currentCenter.y);
                mimicWrapperElem.scrollLeft(scrollLeft);
                mimicWrapperElem.scrollTop(scrollTop);
                event.preventDefault();
            }
        });
}

function updateLayout() {
    let h = $(window).height();
    errorElem.outerHeight(h);
    spinnerElem.outerHeight(h);
    mimicWrapperElem.outerHeight(h);
    alignHorizontally();
}

async function loadMimic() {
    spinnerElem.removeClass("d-none");
    let result = await mimic.load(getLoaderUrl(), viewID);
    spinnerElem.addClass("d-none");

    if (result.ok) {
        mimicWrapperElem.append(unitedRenderer.createMimicDom());
        toolbarElem.removeClass("d-none");
        initScale();
        startUpdatingData();
        startBlinking();
    } else {
        showErrorText(result.msg);
    }
}

function getLoaderUrl() {
    return viewHub.appEnv.rootPath + "Api/Mimic/";
}

function startUpdatingData() {
    updateData(function () {
        setTimeout(startUpdatingData, refreshRate);
    });
}

function updateData(callback) {
    mainApi.getCurDataByView(viewID, cnlListID, function (dto) {
        if (dto.ok) {
            cnlListID = dto.data.cnlListID;
        } else {
            showErrorIcon();
        }

        dataProvider.prevDataMap = dataProvider.curDataMap;
        dataProvider.curDataMap = MainApi.mapCurData(dto.data);
        unitedRenderer.updateData(dataProvider);
        callback();
    });
}

function startBlinking() {
    let blinkers = getBlinkers(); // components that can blink

    if (blinkers.length > 0 && runtimeOptions.blinkingRate > 0) {
        let blinkOn = false;
        let blinkIteration = () => {
            blinkOn = !blinkOn;
            blink(blinkers, blinkOn);
            setTimeout(blinkIteration, runtimeOptions.blinkingRate);
        };

        setTimeout(blinkIteration, runtimeOptions.blinkingRate);
    }
}

function getBlinkers() {
    let blinkers = [];

    for (let component of mimic.components) {
        if (!(component.properties?.blinkingState?.isEmpty ?? true)) {
            blinkers.push(component);
        }
    }

    return blinkers;
}

function blink(blinkers, blinkOn) {
    const EventType = rs.mimic.EventType;

    for (let blinker of blinkers) {
        if (blinker.properties.blinking && blinker.dom) {
            if (blinkOn) {
                blinker.dom.addClass("blink-on");
                blinker.dom.trigger(EventType.BLINK_ON);
            } else {
                blinker.dom.removeClass("blink-on");
                blinker.dom.trigger(EventType.BLINK_OFF);
            }
        }
    }
}

function initScale() {
    if (runtimeOptions.rememberScale) {
        scale.load(localStorage);
    } else {
        scale.type = runtimeOptions.scaleType;
        scale.value = runtimeOptions.scaleValue;
    }

    updateScale(false);
}

function updateScale(saveScale = true) {
    mimic.renderer?.setScale(mimic, scale);
    $("#spanScaleValue").text(Math.round(scale.value * 100) + "%");
    alignHorizontally();

    if (saveScale) {
        scale.save(localStorage);
    }
}

function alignHorizontally() {
    let wrapperPadding = 0;

    if (mimic.dom) {
        let mimicWidth = mimic.dom[0].getBoundingClientRect().width; // taking scaling into account
        let wrapperWidth = mimicWrapperElem.innerWidth();

        if (wrapperWidth > mimicWidth) {
            wrapperPadding = parseInt((wrapperWidth - mimicWidth) / 2);
        }
    }

    mimicWrapperElem.css("padding-left", wrapperPadding);
};

function showErrorText(text) {
    errorElem
        .text(text)
        .removeClass("d-none");
}

function showErrorIcon() {
    clearTimeout(errorTimeoutID);
    $("#spanErrorIcon").removeClass("d-none");

    errorTimeoutID = setTimeout(function () {
        $("#spanErrorIcon").addClass("d-none");
        errorTimeoutID = 0;
    }, ScadaUtils.ERROR_DISPLAY_DURATION);
}

function handleKeyDown(code, ctrlKey) {
    if (ctrlKey) {
        switch (code) {
            case "Minus":
                scale = scale.getPrev();
                updateScale();
                return true;

            case "Equal":
                scale = scale.getNext();
                updateScale();
                return true;

            case "Digit0":
                scale = new rs.mimic.Scale();
                updateScale();
                return true;
        }
    }

    return false;
}

$(async function () {
    unitedRenderer.configure({ viewHub, mainApi, fonts, controlRight });
    initElements();
    bindEvents();
    bindGestureEvents();
    updateLayout();
    await loadMimic();
});
