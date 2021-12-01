/*
 * Scheme data model
 *
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2021
 *
 * Requires:
 * - jquery
 * - utils.js
 * - ajaxqueue.js
 * - schemerender.js
 *
 * At runtime:
 * - clientapi.js
 *
 * Requires external object:
 * - ajaxQueue
 *
 * Inheritance hierarchy:
 * BaseComponent
 *   Scheme
 *   Component
 */

// Rapid SCADA namespace
var scada = scada || {};
// Scheme namespace
scada.scheme = scada.scheme || {};

/********** Base Component **********/

// Abstract parent type for scheme and its components
scada.scheme.BaseComponent = function (type) {
    // Name of the component type
    this.type = type;
    // Component properties received from a server. They are different depending on component type
    this.props = null;
    // jQuery objects representing DOM content
    this.dom = null;
    // Renderer of the component
    this.renderer = null;
};

/********** Scheme Loading States **********/

// Scheme loading states enumeration
scada.scheme.LoadStates = {
    UNDEFINED: 0,
    DOC_LOADING: 1,
    COMPONENTS_LOADING: 2,
    IMAGES_LOADING: 3,
    ERRORS_LOADING: 4,
    COMPLETE: 5
};

/********** Scheme **********/

// Scheme type
scada.scheme.Scheme = function () {
    scada.scheme.BaseComponent.call(this);
    this.renderer = new scada.scheme.SchemeRenderer();

    // Count of components received by a one request
    this.LOAD_COMP_CNT = 100;
    // Total data size of images received by a one request, 1 MB
    this.LOAD_IMG_SIZE = 1048576;
    // Timeout of command frame closing, ms
    this.CLOSE_CMD_TIMEOUT = 1000;

    // Input channel filter for request current data
    this._cnlFilter = null;

    // Indicates whether the scheme is used by an editor
    this.editMode = false;
    // Scheme environment
    this.schemeEnv = null;
    // Ajax queue used for request sequencing. Must be not null
    this.ajaxQueue = window.$;// scada.ajaxQueueLocator.getAjaxQueue();
    // WCF-service URL
    this.serviceUrl = "/Api/Scheme/"; //this.ajaxQueue.rootPath + "plugins/Scheme/SchemeSvc.svc/";
    // Scheme loading state
    this.loadState = scada.scheme.LoadStates.UNDEFINED;
    // Scheme view ID
    this.viewID = 0;
    // Scheme editor ID
    this.editorID = "";
    // Stamp of the view, unique within application scope
    this.viewStamp = 0;
    // Scheme components indexed by ID
    this.componentMap = new Map();
    // Scheme images indexed by name
    this.imageMap = new Map();
    // Index of the loaded component.
    this.componentIndex = 0;
    // Index of the loaded image.
    this.imageIndex = 0;
    // Scheme loading errors when it is loaded on server side
    this.loadErrors = [];
    // Render context
    this.renderContext = new scada.scheme.RenderContext();
    // Parent jQuery object
    this.parentDomElem = null;
    // Current scale
    this.scaleType = scada.scheme.ScaleTypes.NUMERIC;
    this.scaleValue = 1.0;
};

scada.scheme.Scheme.prototype = Object.create(scada.scheme.BaseComponent.prototype);
scada.scheme.Scheme.constructor = scada.scheme.Scheme;

// Continue scheme loading process
// callback is a function (success)
scada.scheme.Scheme.prototype._continueLoading = function (viewOrEditorID, callback) {
    var thisScheme = this;

    this._loadPart(viewOrEditorID, function (success, complete) {
        if (success) {
            if (complete) {
                callback(true);
            } else {
                setTimeout(thisScheme._continueLoading.bind(thisScheme), 0, viewOrEditorID, callback);
            }
        } else {
            callback(false);
        }
    });
};

// Load a part of the scheme depending on the loading state
// callback is a function (success, complete)
scada.scheme.Scheme.prototype._loadPart = function (viewOrEditorID, callback) {
    var LoadStates = scada.scheme.LoadStates;

    // check matching of view or editor
    if (this.editMode) {
        if (!this.editorID) {
            this.editorID = viewOrEditorID;
        } else if (this.editorID !== viewOrEditorID) {
            console.warn(ScadaUtils.getCurrentTime() + " Scheme editor ID mismatch. The existing ID=" +
                this.editorID + ". The requsested ID=" + viewOrEditorID);
            callback(false, false);
            return;
        }
    } else {
        if (this.viewID === 0) {
            this.viewID = viewOrEditorID;
            //this._cnlFilter = new scada.CnlFilter();
            //this._cnlFilter.viewID = viewOrEditorID;
            this._cnlFilter = { viewID: viewOrEditorID, cnlListID: 0 };
        } else if (this.viewID !== viewOrEditorID) {
            console.warn(ScadaUtils.getCurrentTime() + " Scheme view ID mismatch. The existing ID=" +
                this.viewID + ". The requsested ID=" + viewOrEditorID);
            callback(false, false);
            return;
        }
    }

    // loading data depending on the state
    if (this.loadState === LoadStates.UNDEFINED) {
        this.loadState = LoadStates.DOC_LOADING;
    }

    switch (this.loadState) {
        case LoadStates.DOC_LOADING:
            this._loadSchemeDoc(viewOrEditorID, callback);
            break;
        case LoadStates.COMPONENTS_LOADING:
            this._loadComponents(viewOrEditorID, callback);
            break;
        case LoadStates.IMAGES_LOADING:
            this._loadImages(viewOrEditorID, callback);
            break;
        case LoadStates.ERRORS_LOADING:
            this._loadErrors(viewOrEditorID, callback);
            break;
        case LoadStates.COMPLETE:
            console.warn("Scheme loading is already complete");
            callback(true, true);
            break;
        default:
            console.warn("Unknown scheme loading state " + this.loadState);
            callback(false, false);
            break;
    }
};

// Get part of query string to request scheme by view ID or editor ID
scada.scheme.Scheme.prototype._getAccessParamStr = function (viewOrEditorID) {
    return (this.editMode ? "?editorID=" : "?viewID=") + viewOrEditorID;
};

// Check whether the newly received data are matched with the existing
scada.scheme.Scheme.prototype._viewStampsMatched = function (browserViewStamp, serverViewStamp) {
    if (Number.isInteger(browserViewStamp) && Number.isInteger(serverViewStamp) &&
        serverViewStamp > 0 && (browserViewStamp === 0 || browserViewStamp === serverViewStamp)) {
        return true;
    } else {
        console.warn(ScadaUtils.getCurrentTime() + " Scheme view stamp mismatch");
        return false;
    }
};

// Load scheme document properties
// callback is a function (success, complete)
scada.scheme.Scheme.prototype._loadSchemeDoc = function (viewOrEditorID, callback) {
    var operation = this.serviceUrl + "GetSchemeDoc";
    var thisScheme = this;

    this.ajaxQueue.ajax({
        url: operation +
            this._getAccessParamStr(viewOrEditorID) +
            "&viewStamp=" + this.viewStamp,
        method: "GET",
        dataType: "json",
        cache: false
    })
    .done(function (data, textStatus, jqXHR) {
        try {
            var parsedData = data; //$.parseJSON(data.d);
            if (parsedData.ok) {
                thisScheme._logSuccessfulRequest(operation);
                if (thisScheme._obtainSchemeDoc(parsedData)) {
                    thisScheme.loadState = scada.scheme.LoadStates.COMPONENTS_LOADING;
                    callback(true, false);
                } else {
                    callback(false, false);
                }
            } else {
                thisScheme._logServiceError(operation, parsedData.msg);
                callback(false, false);
            }
        }
        catch (ex) {
            thisScheme._logProcessingError(operation, ex.message);
            callback(false, false);
        }
    })
    .fail(function (jqXHR, textStatus, errorThrown) {
        thisScheme._logFailedRequest(operation, jqXHR);
        callback(false, false);
    });
};

// Obtain received scheme document properties
scada.scheme.Scheme.prototype._obtainSchemeDoc = function (parsedData) {
    try {
        /*if (typeof parsedData.ViewStamp === "undefined") {
            throw { message: "ViewStamp property is missing." };
        }*/

        if (typeof parsedData.data === "undefined") {
            throw { message: "SchemeDoc property is missing." };
        }

        this.type = "";
        this.props = parsedData.data;
        return true;

        /*if (this._viewStampsMatched(this.viewStamp, parsedData.ViewStamp)) {
            this.type = "";
            this.props = parsedData.SchemeDoc;
            this.viewStamp = parsedData.ViewStamp;
            return true;
        } else {
            return false;
        }*/
    }
    catch (ex) {
        console.error(ScadaUtils.getCurrentTime() + " Error obtaining scheme properties:", ex.message);
        return false;
    }
};

// Load scheme components
// callback is a function (success, complete)
scada.scheme.Scheme.prototype._loadComponents = function (viewOrEditorID, callback) {
    var operation = this.serviceUrl + "GetComponents";
    var thisScheme = this;

    this.ajaxQueue.ajax({
        url: operation +
            this._getAccessParamStr(viewOrEditorID) +
            "&viewStamp=" + this.viewStamp +
            "&startIndex=" + this.componentIndex +
            "&count=" + this.LOAD_COMP_CNT,
        method: "GET",
        dataType: "json",
        cache: false
    })
    .done(function (data, textStatus, jqXHR) {
        try {
            var parsedData = data;//$.parseJSON(data.d);
            if (parsedData.ok) {
                thisScheme._logSuccessfulRequest(operation);
                if (thisScheme._obtainComponents(parsedData)) {
                    if (parsedData.data.endOfComponents) {
                        thisScheme.loadState = scada.scheme.LoadStates.IMAGES_LOADING;
                    }
                    callback(true, false);
                } else {
                    callback(false, false);
                }
            } else {
                thisScheme._logServiceError(operation, parsedData.msg);
                callback(false, false);
            }
        }
        catch (ex) {
            thisScheme._logProcessingError(operation, ex.message);
            callback(false, false);
        }
    })
    .fail(function (jqXHR, textStatus, errorThrown) {
        thisScheme._logFailedRequest(operation, jqXHR);
        callback(false, false);
    });
};

// Obtain received scheme components
scada.scheme.Scheme.prototype._obtainComponents = function (parsedData) {
    try {
        /*if (typeof parsedData.ViewStamp === "undefined") {
            throw { message: "ViewStamp property is missing." };
        }*/

        if (typeof parsedData.data.endOfComponents === "undefined") {
            throw { message: "EndOfComponents property is missing." };
        }

        if (typeof parsedData.data.components === "undefined") {
            throw { message: "Components property is missing." };
        }

        this._appendComponents(parsedData.data.components);
        return true;

        /*if (this._viewStampsMatched(this.viewStamp, parsedData.ViewStamp)) {
            this.viewStamp = parsedData.ViewStamp;
            this._appendComponents(parsedData.Components);
            return true;
        } else {
            return false;
        }*/
    }
    catch (ex) {
        console.error(ScadaUtils.getCurrentTime() + " Error obtaining scheme components:", ex.message);
        return false;
    }
};

// Append received components to the scheme
scada.scheme.Scheme.prototype._appendComponents = function (parsedComponents) {
    for (var parsedComponent of parsedComponents) {
        this.componentIndex++;

        if (!this._validateComponent(parsedComponent)) {
            console.warn("The component is skipped because the required properties are missed");
            continue;
        }

        var schemeComponent = new scada.scheme.Component(parsedComponent);
        schemeComponent.renderer = scada.scheme.rendererMap.get(schemeComponent.type);

        if (schemeComponent.renderer) {
            // add the component to the scheme
            this.componentMap.set(schemeComponent.id, schemeComponent);
        } else {
            console.warn("The component of type '" + schemeComponent.type +
                "' with ID=" + schemeComponent.id + " is not supported");
        }
    }
};

// Validate main component properties
scada.scheme.Scheme.prototype._validateComponent = function (parsedComponent) {
    return typeof parsedComponent !== "undefined" &&
        typeof parsedComponent.typeName !== "undefined" &&
        typeof parsedComponent.id !== "undefined";
};

// Load scheme images
// callback is a function (success, complete)
scada.scheme.Scheme.prototype._loadImages = function (viewOrEditorID, callback) {
    var operation = this.serviceUrl + "GetImages";
    var thisScheme = this;

    this.ajaxQueue.ajax({
        url: operation +
            this._getAccessParamStr(viewOrEditorID) +
            "&viewStamp=" + this.viewStamp +
            "&startIndex=" + this.imageIndex +
            "&totalDataSize=" + this.LOAD_IMG_SIZE,
        method: "GET",
        dataType: "json",
        cache: false
    })
    .done(function (data, textStatus, jqXHR) {
        try {
            var parsedData = data;//$.parseJSON(data.d);
            if (parsedData.ok) {
                thisScheme._logSuccessfulRequest(operation);
                if (thisScheme._obtainImages(parsedData)) {
                    if (parsedData.data.endOfImages) {
                        thisScheme.loadState = scada.scheme.LoadStates.ERRORS_LOADING;
                    }
                    callback(true, false);
                } else {
                    callback(false, false);
                }
            } else {
                thisScheme._logServiceError(operation, parsedData.msg);
                callback(false, false);
            }
        }
        catch (ex) {
            thisScheme._logProcessingError(operation, ex.message);
            callback(false, false);
        }
    })
    .fail(function (jqXHR, textStatus, errorThrown) {
        thisScheme._logFailedRequest(operation, jqXHR);
        callback(false, false);
    });
};

// Obtain received scheme images
scada.scheme.Scheme.prototype._obtainImages = function (parsedData) {
    try {
        /*if (typeof parsedData.ViewStamp === "undefined") {
            throw { message: "ViewStamp property is missing." };
        }*/

        if (typeof parsedData.data.endOfImages === "undefined") {
            throw { message: "EndOfImages property is missing." };
        }

        if (typeof parsedData.data.images === "undefined") {
            throw { message: "Images property is missing." };
        }

        this._appendImages(parsedData.data.images);
        return true;

        /*if (this._viewStampsMatched(this.viewStamp, parsedData.ViewStamp)) {
            this.viewStamp = parsedData.ViewStamp;
            this._appendImages(parsedData.Images);
            return true;
        } else {
            return false;
        }*/
    }
    catch (ex) {
        console.error(ScadaUtils.getCurrentTime() + " Error obtaining scheme images:", ex.message);
        return false;
    }
};

// Append received images to the scheme
scada.scheme.Scheme.prototype._appendImages = function (parsedImages) {
    for (var parsedImage of parsedImages) {
        this.imageIndex++;

        if (!this._validateImage(parsedImage)) {
            console.warn("The image is skipped because the required properties are missed");
            continue;
        }

        // add the image to the scheme
        this.imageMap.set(parsedImage.name, parsedImage);
    }
};

// Validate main image properties
scada.scheme.Scheme.prototype._validateImage = function (parsedImage) {
    return typeof parsedImage !== "undefined" &&
        typeof parsedImage.name !== "undefined" &&
        typeof parsedImage.data !== "undefined";
};

// Load scheme images
// callback is a function (success, complete)
scada.scheme.Scheme.prototype._loadErrors = function (viewOrEditorID, callback) {
    var operation = this.serviceUrl + "GetLoadErrors";
    var thisScheme = this;

    this.ajaxQueue.ajax({
        url: operation +
            this._getAccessParamStr(viewOrEditorID) +
            "&viewStamp=" + this.viewStamp,
        method: "GET",
        dataType: "json",
        cache: false
    })
    .done(function (data, textStatus, jqXHR) {
        try {
            var parsedData = data;//$.parseJSON(data.d);
            if (parsedData.ok) {
                thisScheme._logSuccessfulRequest(operation);
                thisScheme.loadErrors = parsedData.data;
                thisScheme.loadState = scada.scheme.LoadStates.COMPLETE;
                callback(true, true);

                /*if (typeof parsedData.ViewStamp !== "undefined" &&
                    thisScheme._viewStampsMatched(thisScheme.viewStamp, parsedData.ViewStamp)) {
                    thisScheme.loadErrors = parsedData.Data;
                    thisScheme.loadState = scada.scheme.LoadStates.COMPLETE;
                    callback(true, true);
                } else {
                    callback(false, false);
                }*/
            } else {
                thisScheme._logServiceError(operation, parsedData.msg);
                callback(false, false);
            }
        }
        catch (ex) {
            thisScheme._logProcessingError(operation, ex.message);
            callback(false, false);
        }
    })
    .fail(function (jqXHR, textStatus, errorThrown) {
        thisScheme._logFailedRequest(operation, jqXHR);
        callback(false, false);
    });
};

// Update the component using the current input channel data
scada.scheme.Scheme.prototype._updateComponentData = function (component) {
    try {
        if (component.dom) {
            component.renderer.updateData(component, this.renderContext);
        }
    }
    catch (ex) {
        console.error("Error updating data of the component of type '" +
            component.type + "' with ID=" + component.id + ":", ex.message);
        component.dom = null;
    }
};

// Open frame of sending command
scada.scheme.Scheme.prototype._openCommandFrame = function () {
    if (this.dom) {
        var frameID = "cmd" + Date.now();
        this.dom.append("<div id='" + frameID + "' class='cmd-frame'></div>");
        return frameID;
    } else {
        return null;
    }
};

// Close frame of sending command
scada.scheme.Scheme.prototype._closeCommandFrame = function (opt_frameID) {
    if (this.dom) {
        if (opt_frameID) {
            // remove specified frame
            this.dom.children("#" + opt_frameID).remove();
        } else {
            // remove all command frames
            this.dom.children(".cmd-frame").remove();
        }
    }
};

// Write information about the successful request to console
scada.scheme.Scheme.prototype._logSuccessfulRequest = function (operation, opt_data) {
    console.log(ScadaUtils.getCurrentTime() + " Request '" + operation + "' successful");
    if (opt_data) {
        console.log(opt_data.d);
    }
};

// Write information about the failed request to console
scada.scheme.Scheme.prototype._logFailedRequest = function (operation, jqXHR) {
    console.error(ScadaUtils.getCurrentTime() + " Request '" + operation + "' failed: " +
        jqXHR.status + " (" + jqXHR.statusText + ")");
};

// Write information about the internal service error to console
scada.scheme.Scheme.prototype._logServiceError = function (operation, opt_message) {
    console.error(ScadaUtils.getCurrentTime() + " Request '" + operation + "' reports internal service error" +
        (opt_message ? ": " + opt_message : ""));
};

// Write information about the request processing error to console
scada.scheme.Scheme.prototype._logProcessingError = function (operation, opt_message) {
    console.error(ScadaUtils.getCurrentTime() + " Error processing request '" + operation + "'" +
        (opt_message ? ": " + opt_message : ""));
};

// Clear the scheme
scada.scheme.Scheme.prototype.clear = function () {
    this.props = null;

    if (this.dom) {
        this.dom.remove();
        this.dom = null;
    }

    this._cnlFilter = null;
    this.loadState = scada.scheme.LoadStates.UNDEFINED;
    this.viewID = 0;
    this.editorID = "";
    this.viewStamp = 0;
    this.componentMap = new Map();
    this.imageMap = new Map();
    this.componentIndex = 0;
    this.imageIndex = 0;
    this.renderContext = new scada.scheme.RenderContext();
};

// Load the scheme
// callback is a function (success)
scada.scheme.Scheme.prototype.load = function (viewOrEditorID, callback) {
    var thisScheme = this;
    var startTime = Date.now();

    console.info(ScadaUtils.getCurrentTime() + " Start loading scheme");
    this.parentDomElem.addClass("loading");
    this.clear();

    this._continueLoading(viewOrEditorID, function (success) {
        if (success) {
            console.info(ScadaUtils.getCurrentTime() + " Scheme loading completed successfully in " +
                (Date.now() - startTime) + " ms");
        } else {
            console.error(ScadaUtils.getCurrentTime() + " Scheme loading failed");
        }

        thisScheme.parentDomElem.removeClass("loading");
        callback(success);
    });
};

// Create DOM content of the scheme
scada.scheme.Scheme.prototype.createDom = function (opt_controlRight) {
    this.renderContext.editMode = this.editMode;
    this.renderContext.schemeEnv = this.schemeEnv;
    this.renderContext.viewID = this.viewID;
    this.renderContext.imageMap = this.imageMap;
    this.renderContext.controlRight = typeof opt_controlRight === "undefined" ?
        true : opt_controlRight;
    var startTime = Date.now();

    try
    {
        this.renderer.createDom(this, this.renderContext);
        this.parentDomElem.append(this.dom);
    }
    catch (ex) {
        console.error("Error creating DOM content of the scheme:", ex.message);
        this.dom = null;
        return;
    }

    for (var component of this.componentMap.values()) {
        try {
            component.renderer.createDom(component, this.renderContext);
            if (this.dom) {
                var elem = this.renderContext.editMode ? component.renderer.wrap(component) : component.dom;
                this.dom.append(elem);
            }
        }
        catch (ex) {
            console.error("Error creating DOM content of the component of type '" +
                component.type + "' with ID=" + component.id + ":", ex.message);
            component.dom = null;
        }
    }

    console.info(ScadaUtils.getCurrentTime() + " Scheme document object model created in " +
        (Date.now() - startTime) + " ms");
};

// Update the scheme components according to the current input channel data
// callback is a function (success)
scada.scheme.Scheme.prototype.updateData = function (mainApi, callback) {
    var thisScheme = this;

    mainApi.getCurDataByView(this._cnlFilter.viewID, this._cnlFilter.cnlListID, function (dto) {
        if (dto.ok) {
            thisScheme._cnlFilter.cnlListID = dto.data.cnlListID;
            thisScheme.renderContext.curCnlDataMap = mainApi.mapCurData(dto.data);

            for (var component of thisScheme.componentMap.values()) {
                thisScheme._updateComponentData(component);
            }

            callback(true);
        } else {
            callback(false);
        }
    });
};

// Set the scheme scale
scada.scheme.Scheme.prototype.setScale = function (scaleType, scaleValue) {
    try {
        var newScaleValue = this.renderer.setScale(this, scaleType, scaleValue);
        this.scaleType = scaleType;
        this.scaleValue = newScaleValue;
    }
    catch (ex) {
        console.error("Error scaling the scheme:", ex.message);
    }
};

// Send telecommand
// callback is a function (success)
scada.scheme.Scheme.prototype.sendCommand = function (ctrlCnlNum, cmdVal, viewID, componentID, callback) {
    var operation = this.serviceUrl + "SendCommand";
    var thisScheme = this;

    var frameID = this._openCommandFrame();
    var closeTimeoutID = setTimeout(this._closeCommandFrame.bind(this), this.CLOSE_CMD_TIMEOUT, frameID);

    var failCallback = function () {
        clearTimeout(closeTimeoutID);
        thisScheme._closeCommandFrame(frameID);
        callback(false);
    };

    this.ajaxQueue.ajax({
        url: operation +
            "?ctrlCnlNum=" + ctrlCnlNum +
            "&cmdVal=" + cmdVal +
            "&viewID=" + viewID +
            "&componentID=" + componentID,
        method: "GET",
        dataType: "json",
        cache: false
    })
    .done(function (data, textStatus, jqXHR) {
        var parsedData = $.parseJSON(data.d);
        if (parsedData.Success) {
            thisScheme._logSuccessfulRequest(operation);
            if (parsedData.Data) {
                callback(true);
            } else {
                console.warn("Unable to send command");
                failCallback();
            }
        } else {
            thisScheme._logServiceError(operation, parsedData.ErrorMessage);
            failCallback();
        }
    })
    .fail(function (jqXHR, textStatus, errorThrown) {
        thisScheme._logFailedRequest(operation, jqXHR);
        failCallback();
    });
};

/********** Component **********/

// Component type
scada.scheme.Component = function (componentProps) {
    scada.scheme.BaseComponent.call(this, componentProps.typeName);
    this.props = componentProps;

    // Component ID
    this.id = componentProps.id;
    // Telecommand value in case of direct sending
    this.cmdVal = 0;
};

scada.scheme.Component.prototype = Object.create(scada.scheme.BaseComponent.prototype);
scada.scheme.Component.constructor = scada.scheme.Component;
