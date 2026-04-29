// Contains classes: TableFormBase, TableFormAsync, TableFormSync
// Depends on modal.js, razor-pager.js

// Specifies the phrases for an async table form.
// Can be changed by a page script.
var tableFormPhrases = {};

// A base class that implements the behavior of a table form.
class TableFormBase {
    _pager;
    _elemId;

    constructor() {
        this._pager = null;
        this._elemId = this._getDefaultElemIds();
    }

    _getDefaultElemIds() {
        return {
            tableToolbar: "#divTableToolbar",
            searchInput: "#txtSearch",
            searchButton: "#btnSearch",
            newItemButton: "#btnNewItem",
            tableWrapper: "#divTableWrapper",
            pager: "pgrMain" // without #
        };
    }

    _bindEvents() {
    }

    init() {
        this._pager = typeof RazorPager === "function" ? new RazorPager(this._elemId.pager) : null;
        this._bindEvents();
    }
}

// A base class for a table form that 
//   reloads data asynchoniously, 
//   shows modals for creating and editing records.
class TableFormAsync extends TableFormBase {
    _phrases = {};

    _bindEvents() {
        $(this._elemId.tableToolbar).on("change", "select", async () => {
            await this._loadData();
        });

        $(this._elemId.searchInput).on("keydown", async event => {
            if (event.code === "Enter" || event.code === "NumpadEnter") {
                await this._loadData();
                event.preventDefault();
            }
        });

        $(this._elemId.searchButton).on("click", async () => {
            await this._loadData();
        });

        $(this._elemId.newItemButton).on("click", async () => {
            this._newItem();
        });

        $(this._elemId.tableWrapper).on("click", ".rs-view-item", event => {
            this._viewItem(this._getItemId(event.target));
        });

        $(this._elemId.tableWrapper).on("click", ".rs-edit-item", event => {
            this._editItem(this._getItemId(event.target));
        });

        $(this._elemId.tableWrapper).on("click", ".rs-delete-item", async event => {
            if (confirm(this._phrases.deleteItemConfirm)) {
                await this._deleteItem(this._getItemId(event.target));
            }
        });

        $(this._elemId.tableWrapper).on("click", ".rs-approve-item", async event => {
            await this._approveItem(this._getItemId(event.target));
        });

        $(this._elemId.tableWrapper).on("click", ".rs-unapprove-item", async event => {
            await this._unapproveItem(this._getItemId(event.target));
        });

        if (this._pager) {
            $(this._elemId.tableWrapper).on(RazorPager.PAGE_CLICK_EVENT, RazorPager.PAGER_CLASS, async () => {
                await this._loadData();
            });
        }
    }

    async _loadData(opt_handler, opt_params) {
        let url = this._getDataUrl(opt_handler);

        if (url) {
            let result = await this._fetchData(url, opt_params);

            if (result.ok) {
                console.log("Table data loaded successfully.");
                $(this._elemId.tableWrapper).html(result.data);
                this._pager?.bindEvents();
            } else {
                ModalBox.showMessage(catalogPhrases.loadDataError, { alert: ModalBoxAlerts.DANGER });
            }
        } else {
            console.warn("Unable to load table data because URL is undefined.")
        }
    }

    async _fetchData(url, opt_params) {
        let formData = new FormData($("form:first")[0]);

        if (opt_params) {
            for (let [key, value] of Object.entries(opt_params)) {
                formData.set(key, value);
            }
        }

        let response = await fetch(url, {
            method: "POST",
            headers: { RequestVerificationToken: this._getVerificationToken() },
            body: formData
        });

        return response.ok
            ? Dto.success(await response.text())
            : Dto.fail(await response.text() || response.statusText);
    }

    _newItem() {
        this._showModal(
            this._getNewUrl(),
            this._getNewOptions(),
            async result => {
                if (result) {
                    await this._loadData();
                }
            });
    }

    _viewItem(itemId) {
        this._showModal(
            this._getViewUrl(itemId),
            this._getViewOptions(),
            async result => {
                if (result) {
                    await this._loadData();
                }
            });
    }

    _editItem(itemId) {
        this._showModal(
            this._getEditUrl(itemId),
            this._getEditOptions(),
            async result => {
                if (result) {
                    await this._loadData();
                }
            });
    }

    async _deleteItem(itemId) {
        await this._loadData("Delete", { itemId: itemId });
    }

    async _approveItem(itemId) {
        await this._loadData("Approve", { itemId: itemId });
    }

    async _unapproveItem(itemId) {
        await this._loadData("Unapprove", { itemId: itemId });
    }

    _getDataUrl(opt_handler) {
        return null;
    }

    _getNewUrl() {
        return null;
    }

    _getViewUrl(itemId) {
        return null;
    }

    _getEditUrl(itemId) {
        return null;
    }

    _getNewOptions() {
        return { buttons: [ModalButton.OK, ModalButton.CANCEL] };
    }

    _getViewOptions() {
        return { buttons: [ModalButton.CLOSE] };
    }

    _getEditOptions() {
        return { buttons: [ModalButton.OK, ModalButton.CANCEL] };
    }

    _getVerificationToken() {
        return $("input[name='__RequestVerificationToken']:first").val();
    }

    _getItemId(rowElem) {
        return $(rowElem).closest("tr").data("item-id");
    }

    _showModal(url, opt_options, opt_callback) {
        if (!url) {
            console.warn("Modal URL is not specified.")
        } else if (!mainObj) {
            console.warn("Unable to show modal because mainObj is undefined.")
        } else {
            mainObj.modalManager.showModal(url, opt_options, opt_callback);
        }
    }

    init() {
        super.init();
        Object.assign(this._phrases, tableFormPhrases);
    }
}

// A base class for a table form that
//   reloads the entire page,
//   redirected by the server side for creating and editing records.
class TableFormSync extends TableFormBase {
    _bindEvents() {
        $(this._elemId.tableToolbar).on("change", "select", () => {
            this._loadData();
        });

        $(this._elemId.searchInput).on("keydown", event => {
            if (event.code === "Enter" || event.code === "NumpadEnter") {
                this._loadData();
                event.preventDefault();
            }
        });

        $(this._elemId.searchButton).on("click", () => {
            this._loadData();
        });

        if (this._pager) {
            $(this._elemId.tableWrapper).on(RazorPager.PAGE_CLICK_EVENT, RazorPager.PAGER_CLASS, async () => {
                this._loadData();
            });
        }
    }

    _loadData() {
        // override in child class
    }
}
