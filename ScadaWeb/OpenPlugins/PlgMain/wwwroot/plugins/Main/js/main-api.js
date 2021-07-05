// Represents a data transfer object that carries data from the server side to a client.
class Dto {
    constructor() {
        this.ok = false;
        this.msg = "";
        this.data = null;
    }

    static fail(msg) {
        let dto = new Dto();
        dto.msg = msg;
        return dto;
    }
}

// Represents a JavaScript wrapper for the plugin's web API.
// Callbacks are function (dto)
class MainApi {
    constructor() {
        this.rootPath = "/";
    }

    // Formats the channel numbers for use in a request path.
    _formatCnlNums(obj) {
        return Array.isArray(obj)
            ? obj.join(",")
            : obj;
    }

    // Parses the response and calls the callback method.
    _processResponse(response, methodName, callback) {
        (response.ok
            ? response.json()
            : new Promise(resolve => resolve(Dto.fail(response.statusText))))
            .then(data => callback(data))
            .catch(error => console.error(`Error in ${methodName}.`, error));
    }

    // Calls the callback function without any exception.
    _doCallback(callback, dto, methodName) {
        try {
            callback(dto);

            if (!dto.ok) {
                console.error(`Error in ${methodName}.`, dto.msg)
            }
        }
        catch (ex) {
            console.error(`Error in ${methodName}.`, ex)
        }
    }

    // Gets the current data without formatting.
    // URL example: http://localhost/Api/Main/GetCurData?cnlNums=101-105,110
    getCurData(cnlNums, callback) {
        fetch(this.rootPath + "Api/Main/GetCurData?cnlNums=" + this._formatCnlNums(cnlNums))
            .then(response => this._processResponse(response, "getCurData", callback));
    }

    // Gets the current data of the specified channels.
    // Set useCache to true only with a subsequent call to the getCurDataStep2 method.
    // URL example: http://localhost/Api/Main/GetCurDataStep1?cnlNums=101-105,110&useCache=true
    getCurDataStep1(cnlNums, useCache, callback) {
        fetch(this.rootPath + "Api/Main/GetCurDataStep1?cnlNums=" + this._formatCnlNums(cnlNums) + "&useCache=" + useCache)
            .then(response => this._processResponse(response, "getCurDataStep1", callback));
    }

    // Gets the current data by the channel list ID returned in step 1.
    // URL example: http://localhost/Api/Main/GetCurDataStep2?cnlListID=1
    getCurDataStep2(cnlListID, callback) {
        fetch(this.rootPath + "Api/Main/GetCurDataStep2?cnlListID=" + cnlListID)
            .then(response => this._processResponse(response, "getCurDataStep2", callback));
    }

    // Gets the current data by view.
    // URL example: http://localhost/Api/Main/GetCurDataByView?viewID=1&cnlListID=0
    getCurDataByView(viewID, cnlListID, callback) {
        fetch(this.rootPath + "Api/Main/GetCurDataByView?viewID=" + viewID + "&cnlListID=" + cnlListID)
            //.then(response => this._processResponse(response, "getCurDataByView", callback));
            .then(response => response.ok ? response.json() : Dto.fail(response.statusText))
            .then(data => this._doCallback(callback, data, "getCurDataByView"))
            .catch(error => this._doCallback(callback, Dto.fail(error.message), "getCurDataByView"));
    }

    // Creates a map of current data records accessed by channel number.
    mapCurData(curData) {
        let map = new Map();

        if (curData) {
            for (let record of curData.records) {
                map.set(record.d.cnlNum, record);
            }
        }

        return map;
    }
}
