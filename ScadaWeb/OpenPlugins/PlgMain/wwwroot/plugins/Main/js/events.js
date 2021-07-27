var arcWriteTime = 0;

function updateLayout() {
    let h = $(window).height() - $("#divToolbar").outerHeight();
    $("#divTableWrapper").outerHeight(h);
    $("#divLoading").outerHeight(h);
    $("#divNoEvents").outerHeight(h);
};

function updateEvents(callback) {
    if (archiveBit >= 0) {
        mainApi.getArcWriteTime(archiveBit, function (dto) {
            if (dto.ok) {
                let newArcWriteTime = dto.data;

                if (arcWriteTime !== newArcWriteTime) {
                    // request events
                    mainApi.getLastEventsByView(viewHub.viewID, 100, 0, 2, archiveBit, function (dto) {
                        if (!dto.ok) {
                            showErrorBadge();
                        }

                        arcWriteTime = newArcWriteTime;
                        //showEvents(dto.data);
                        callback();
                    });
                } else {
                    // data has not changed
                    callback();
                }
            } else {
                showErrorBadge();
                callback();
            }
        });
    } else {
        console.warn("Unable to request events");
        showErrorBadge();
        callback();
    }
}

$(document).ready(function () {
    updateLayout();
    updateEvents(function () {
    });
});
