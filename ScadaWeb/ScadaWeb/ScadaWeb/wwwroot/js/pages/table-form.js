// Contains classes: TableFormBase, TableFormAsync, TableFormSync
// Depends on modal.js, razor-pager.js

/** @deprecated */
var tableFormPhrases = null;

/** Contains phrases used by table forms. */
class TableFormPhrases {
    loadDataError;
    deleteItemConfirm;
    itemExists;
    fillOutFields;
    validJsonRequired;

    constructor() {
        for (const key of Object.keys(this)) {
            this[key] = key;
        }
    }
}

/** A base class that implements the behavior of a table form. */
class TableFormBase {
    _formElem;
    _controlId;
    _pager;
    _phrases;

    constructor(opt_formId, opt_phrases) {
        if (opt_formId) {
            // access controls by class names inside the parent form
            this._formElem = $("#" + opt_formId);
            this._controlId = {};
        } else {
            // access controls directly by IDs
            this._formElem = null;
            this._controlId = this._getDefaultControlIds();
        }

        this._pager = null;
        this._phrases = opt_phrases ?? tableFormPhrases ?? new TableFormPhrases();
    }

    _getDefaultControlIds() {
        return {
            tableToolbar: "#divTableToolbar",
            searchButton: "#btnSearch",
            newItemButton: "#btnNewItem",
            tableWrapper: "#divTableWrapper",
            pager: "pgrMain" // without #
        };
    }

    _initPager() {
        if (typeof RazorPager === "function") {
            if (this._formElem) {
                const pagerElem = this._formElem.find(RazorPager.PAGER_CLASS);

                if (pagerElem.length > 0) {
                    this._pager = new RazorPager(pagerElem.prop("id"));
                }
            } else {
                this._pager = new RazorPager(this._controlId.pager);
            }
        }
    }

    _bindEvents() {
        if (this._formElem) {
            this._bindEventsByClass();
        } else {
            this._bindEventsById();
        }
    }

    _bindEventsById() {
        // override in derived class
    }

    _bindEventsByClass() {
        // override in derived class
    }

    _getTableToolbar() {
        return this._formElem
            ? this._formElem.find(".rs-table-toolbar:first")
            : $(this._controlId.tableToolbar);
    }

    _getTableWrapper() {
        return this._formElem
            ? this._formElem.find(".rs-table-wrapper:first")
            : $(this._controlId.tableWrapper);
    }

    init() {
        this._initPager();
        this._bindEvents();
    }
}

/** 
 * A base class for a table form that:
 * - reloads table asynchronously,
 * - shows modals for creating and editing records. 
 */
class TableFormAsync extends TableFormBase {
    _bindEventsById() {
        const tableToolbarElem = $(this._controlId.tableToolbar);
        const tableWrapperElem = $(this._controlId.tableWrapper);
        this._bindToolbarEvents(tableToolbarElem);
        this._bindTableEvents(tableWrapperElem);

        $(this._controlId.searchButton).on("click", async () => {
            await this._loadData();
        });

        $(this._controlId.newItemButton).on("click", async () => {
            this._newItem();
        });
    }

    _bindEventsByClass() {
        const tableToolbarElem = this._formElem.find(".rs-table-toolbar:first");
        const tableWrapperElem = this._formElem.find(".rs-table-wrapper:first");
        this._bindToolbarEvents(tableToolbarElem);
        this._bindTableEvents(tableWrapperElem);

        tableToolbarElem.find(".rs-search-button").on("click", async () => {
            await this._loadData();
        });

        tableToolbarElem.find(".rs-new-item-button").on("click", async () => {
            this._newItem();
        });
    }

    _bindToolbarEvents(tableToolbarElem) {
        tableToolbarElem
            .on("change", "select", async () => {
                await this._loadData();
            })
            .on("keydown", "input", async (event) => {
                if (event.code === "Enter" || event.code === "NumpadEnter") {
                    event.preventDefault(); // before await
                    await this._loadData();
                }
            })
            .on("input", "input[type=datetime-local]", async (event) => {
                const elem = $(event.currentTarget);
                elem.data("changed", true);
            })
            .on("blur", "input[type=datetime-local]", async (event) => {
                const elem = $(event.currentTarget);

                if (elem.data("changed")) {
                    await this._loadData();
                }
            });
    }

    _bindTableEvents(tableWrapperElem) {
        tableWrapperElem
            .on("click", ".rs-view-item", (event) => {
                this._viewItem(this._getItemId(event.target));
            })
            .on("click", ".rs-edit-item", (event) => {
                this._editItem(this._getItemId(event.target));
            })
            .on("click", ".rs-delete-item", async (event) => {
                if (confirm(this._phrases.deleteItemConfirm)) {
                    await this._deleteItem(this._getItemId(event.target));
                }
            })
            .on("click", ".rs-approve-item", async (event) => {
                await this._approveItem(this._getItemId(event.target));
            })
            .on("click", ".rs-unapprove-item", async (event) => {
                await this._unapproveItem(this._getItemId(event.target));
            });

        if (this._pager) {
            tableWrapperElem.on(RazorPager.PAGE_CLICK_EVENT, RazorPager.PAGER_CLASS, async () => {
                await this._loadData();
            });
        }
    }

    async _loadData(opt_handler, opt_params) {
        this._clearChangedAttrs();
        const url = this._getDataUrl(opt_handler);

        if (url) {
            const result = await this._fetchData(url, opt_params);

            if (result.ok) {
                console.log("Table data loaded successfully.");
                this._getTableWrapper().html(result.data);
                this._pager?.bindEvents();
            } else {
                ModalBox.showMessage(result.msg, { alert: ModalBoxAlerts.DANGER });
            }
        } else {
            console.warn("Unable to load table data because URL is undefined.")
        }
    }

    async _fetchData(url, opt_params) {
        const formData = new FormData($("form:first")[0]);

        if (opt_params) {
            for (const [key, value] of Object.entries(opt_params)) {
                formData.set(key, value);
            }
        }

        try {
            const response = await fetch(url, {
                method: "POST",
                headers: {
                    RequestVerificationToken: this._getVerificationToken(),
                    "X-Requested-With": "XMLHttpRequest" // indicates AJAX request
                },
                body: formData
            });

            const text = await response.text();
            return response.ok
                ? Dto.success(text)
                : Dto.fail(text || this._phrases.loadDataError);
        } catch (ex) {
            return Dto.fail(ex.message);
        }
    }

    _clearChangedAttrs() {
        this._getTableToolbar().find("input[type=datetime-local]").data("changed", false);
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
        // override in derived class
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
}

/**
 * A base class for a table form that:
 * - reloads the entire page,
 * - redirected by the server side for creating and editing records.
 * Not implemented.
 */
class TableFormSync extends TableFormBase {
}
