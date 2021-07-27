// The plugin's web API.
// No dependencies.

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

// Represents a time range.
class TimeRange {
    constructor(startTime, endTime, endInclusive) {
        this.startTime = startTime;
        this.endTime = endTime;
        this.endInclusive = endInclusive;
    }

    param() {
        return `startTime=${this.startTime}&endTime=${this.endTime}&endInclusive=${this.endInclusive}`;
    }
}

// Represents a JavaScript wrapper for the plugin's web API.
// Callbacks are function (dto)
class MainApi {
    constructor() {
        this.rootPath = "/";
    }

    // Formats the channel numbers for use in a query string.
    _cnlNumsToParam(cnlNums) {
        return "cnlNums=" + Array.isArray(cnlNums)
            ? cnlNums.join(",")
            : cnlNums;
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
        fetch(this.rootPath + "Api/Main/GetCurData?" + this._cnlNumsToParam(cnlNums))
            .then(response => response.ok ? response.json() : Dto.fail(response.statusText))
            .then(data => this._doCallback(callback, data, "getCurData"))
            .catch(error => this._doCallback(callback, Dto.fail(error.message), "getCurData"));
    }

    // Gets the current data of the specified channels.
    // Set useCache to true only with a subsequent call to the getCurDataStep2 method.
    // URL example: http://localhost/Api/Main/GetCurDataStep1?cnlNums=101-105,110&useCache=true
    getCurDataStep1(cnlNums, useCache, callback) {
        fetch(this.rootPath + "Api/Main/GetCurDataStep1?" + this._cnlNumsToParam(cnlNums) + "&useCache=" + useCache)
            .then(response => response.ok ? response.json() : Dto.fail(response.statusText))
            .then(data => this._doCallback(callback, data, "getCurDataStep1"))
            .catch(error => this._doCallback(callback, Dto.fail(error.message), "getCurDataStep1"));
    }

    // Gets the current data by the channel list ID returned in step 1.
    // URL example: http://localhost/Api/Main/GetCurDataStep2?cnlListID=1
    getCurDataStep2(cnlListID, callback) {
        fetch(this.rootPath + "Api/Main/GetCurDataStep2?cnlListID=" + cnlListID)
            .then(response => response.ok ? response.json() : Dto.fail(response.statusText))
            .then(data => this._doCallback(callback, data, "getCurDataStep2"))
            .catch(error => this._doCallback(callback, Dto.fail(error.message), "getCurDataStep2"));
    }

    // Gets the current data by view.
    // URL example: http://localhost/Api/Main/GetCurDataByView?viewID=1&cnlListID=0
    getCurDataByView(viewID, cnlListID, callback) {
        fetch(this.rootPath + "Api/Main/GetCurDataByView?viewID=" + viewID + "&cnlListID=" + cnlListID)
            .then(response => response.ok ? response.json() : Dto.fail(response.statusText))
            .then(data => this._doCallback(callback, data, "getCurDataByView"))
            .catch(error => this._doCallback(callback, Dto.fail(error.message), "getCurDataByView"));
    }

    // Gets the historical data.
    // URL example: http://localhost/Api/Main/GetHistData?archiveBit=1&startTime=2021-12-31T00:00:00.000Z&endTime=2021-12-31T23:59:59Z&endInclusive=true&cnlNums=101-105,110
    getHistData(archiveBit, timeRange, cnlNums, callback) {
        fetch(this.rootPath + "Api/Main/GetHistData?archiveBit=" + archiveBit +
            "&" + timeRange.param() + "&" + this._cnlNumsToParam(cnlNums))
            .then(response => response.ok ? response.json() : Dto.fail(response.statusText))
            .then(data => this._doCallback(callback, data, "getHistData"))
            .catch(error => this._doCallback(callback, Dto.fail(error.message), "getHistData"));
    }

    // Gets the historical data.
    // URL example: http://localhost/Api/Main/GetHistData?archiveBit=1&startTime=2021-12-31T00:00:00.000Z&endTime=2021-12-31T23:59:59Z&endInclusive=true&viewID=1
    getHistDataByView(archiveBit, timeRange, viewID, callback) {
        fetch(this.rootPath + "Api/Main/GetHistDataByView?archiveBit=" + archiveBit +
            "&" + timeRange.param() + "&viewID=" + viewID)
            .then(response => response.ok ? response.json() : Dto.fail(response.statusText))
            .then(data => this._doCallback(callback, data, "getHistDataByView"))
            .catch(error => this._doCallback(callback, Dto.fail(error.message), "getHistDataByView"));
    }

    // Gets all events for the period.
    // URL example: http://localhost/Api/Main/GetEvents?archiveBit=1&startTime=2021-12-31T00:00:00.000Z&endTime=2021-12-31T23:59:59Z&endInclusive=true
    getEvents(archiveBit, timeRange, callback) {
        fetch(this.rootPath + "Api/Main/GetEvents?archiveBit=" + archiveBit + "&" + timeRange.param())
            .then(response => response.ok ? response.json() : Dto.fail(response.statusText))
            .then(data => this._doCallback(callback, data, "getEvents"))
            .catch(error => this._doCallback(callback, Dto.fail(error.message), "getEvents"));
    }

    // Gets the last events.
    // URL example: http://localhost/Api/Main/GetLastEvents?archiveBit=1&period=2&limit=100
    getLastEvents(archiveBit, period, limit, callback) {
        fetch(this.rootPath + "Api/Main/GetEvents?archiveBit=" + archiveBit + "&period=" + period + "&limit=" + limit)
            .then(response => response.ok ? response.json() : Dto.fail(response.statusText))
            .then(data => this._doCallback(callback, data, "getLastEvents"))
            .catch(error => this._doCallback(callback, Dto.fail(error.message), "getLastEvents"));
    }

    // Gets the last events by view.
    // URL example: http://localhost/Api/Main/GetLastEventsByView?archiveBit=1&period=2&limit=100&viewID=1&filterID=1
    getLastEventsByView(archiveBit, period, limit, viewID, filterID, callback) {
        fetch(this.rootPath + "Api/Main/GetLastEventsByView?archiveBit=" + archiveBit +
            "&period=" + period + "&limit=" + limit + "&viewID=" + viewID + "&filterID=" + filterID)
            .then(response => response.ok ? response.json() : Dto.fail(response.statusText))
            .then(data => this._doCallback(callback, data, "getLastEventsByView"))
            .catch(error => this._doCallback(callback, Dto.fail(error.message), "getLastEventsByView"));
    }

    // Gets the Unix time in milliseconds when the archive was last written to.
    // URL example: http://localhost/Api/Main/GetArcWriteTime?archiveBit=1
    getArcWriteTime(archiveBit, callback) {
        fetch(this.rootPath + "Api/Main/GetArcWriteTime?archiveBit=" + archiveBit)
            .then(response => response.ok ? response.json() : Dto.fail(response.statusText))
            .then(data => this._doCallback(callback, data, "getArcWriteTime"))
            .catch(error => this._doCallback(callback, Dto.fail(error.message), "getArcWriteTime"));
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

    // Creates a map of channel indexes accessed by channel number.
    mapCnlNums(cnlNums) {
        let map = new Map();

        if (cnlNums) {
            let idx = 0;

            for (let cnlNum of cnlNums) {
                map.set(cnlNum, idx);
                idx++;
            }
        }

        return map;
    }
}
