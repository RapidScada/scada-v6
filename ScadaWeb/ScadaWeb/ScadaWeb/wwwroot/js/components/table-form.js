// Contains classes: TableFormBase, TableFormAsync, TableFormSync
// Depends on scada-common.js, modal.js, razor-pager.js

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
    _pager;
    _pageSize;
    _phrases;

    constructor(formId, opt_phrases) {
        this._formElem = $("#" + formId);
        this._pager = null;
        this._pageSize = null;
        this._phrases = opt_phrases ?? new TableFormPhrases();
    }

    _initPager() {
        if (typeof RazorPager === "function") {
            const pagerElemId = this._formElem.find(RazorPager.COMPONENT_SELECTOR).prop("id");

            if (pagerElemId) {
                this._pager = new RazorPager(pagerElemId);
            }
        }
    }

    _initPageSize() {
        if (typeof RazorPageSize === "function") {
            const pageSizeElemId = this._formElem.find(RazorPageSize.COMPONENT_SELECTOR).prop("id");

            if (pageSizeElemId) {
                this._pageSize = new RazorPageSize(pageSizeElemId);
            }
        }
    }

    _bindEvents() {
        // override in derived class
    }

    _getTableToolbar() {
        return this._formElem.find(".rs-table-toolbar").first();
    }

    _getTableWrapper() {
        return this._formElem.find(".rs-table-wrapper").first();
    }

    init() {
        this._initPager();
        this._initPageSize();
        this._bindEvents();
    }
}

/** 
 * A base class for a table form that:
 * - reloads table asynchronously,
 * - shows modals for creating and editing records. 
 */
class TableFormAsync extends TableFormBase {
    static _DEBOUNCE_MS = 1000; // wait for input when editing date and time
    static _LOADING_MS = 500; // delay before displaying the loading state
    _debounceTimeoutId = null;
    _loadingTimeoutId = null;
    _abortController = null;

    _bindEvents() {
        this._bindToolbarEvents();
        this._bindTableEvents();
    }

    _bindToolbarEvents() {
        this._getTableToolbar()
            .on("change", "select", async () => {
                await this._loadTable({ resetPager: true });
            })
            .on("keydown", "input", async (event) => {
                if (event.key === "Enter") {
                    event.preventDefault(); // before await
                    await this._loadTable({ resetPager: true });
                }
            })
            .on("input", "input[type=datetime-local]", () => {
                clearTimeout(this._debounceTimeoutId);
            })
            .on("change", "input[type=datetime-local]", async (event) => {
                const input = event.currentTarget;

                if (!input.value || !input.validity.valid) {
                    return; // input incomplete
                }

                if (ScadaUtils.getPlatform().isMobile) {
                    await this._loadTable({ resetPager: true });
                } else {
                    this._debounceTimeoutId = setTimeout(
                        () => this._loadTable({ resetPager: true }),
                        TableFormAsync._DEBOUNCE_MS);
                }
            })
            .on("click", ".rs-search-button", async () => {
                await this._loadTable({ resetPager: true });
            })
            .on("click", ".rs-new-item-button", () => {
                this._newItem();
            });
    }

    _bindTableEvents() {
        const tableWrapperElem = this._getTableWrapper()
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
            });

        if (this._pager) {
            tableWrapperElem.on(RazorPager.PAGE_CLICK_EVENT, RazorPager.COMPONENT_SELECTOR, async () => {
                await this._loadTable();
            });
        }
    }

    async _loadTable({ handler, fields, resetPager = false } = {}) {
        clearTimeout(this._debounceTimeoutId);
        const url = this._getDataUrl(handler);

        if (!url) {
            console.warn("Unable to load table because URL is undefined.")
            return;
        }

        if (resetPager) {
            this._pager?.reset();
        }

        this._hideError();
        const result = await this._fetchData(url, fields);

        if (result === null) {
            console.log("Table loading was aborted.");
        } else if (result.ok) {
            console.log("Table loaded successfully.");
            this._getTableWrapper().html(result.data);
            this._pager?.bindEvents();
        } else {
            this._getTableWrapper().empty();
            this._showError(result.msg);
        }
    }

    async _fetchData(url, opt_fields) {
        const formData = new FormData($("form")[0]);

        if (opt_fields) {
            for (const [key, value] of Object.entries(opt_fields)) {
                formData.set(key, value);
            }
        }

        // abort previous request, latest wins
        this._abortController?.abort();
        const controller = new AbortController();
        this._abortController = controller;

        this._showLoading();

        try {
            const response = await fetch(url, {
                method: "POST",
                headers: {
                    RequestVerificationToken: this._getVerificationToken(),
                    "X-Requested-With": "XMLHttpRequest" // indicates AJAX request
                },
                body: formData,
                signal: controller.signal
            });

            const text = await response.text();
            return response.ok
                ? Dto.success(text)
                : Dto.fail(text || this._phrases.loadDataError);
        } catch (ex) {
            if (ex.name === "AbortError") {
                return null; // request aborted
            }

            return Dto.fail(ex.message);
        } finally {
            if (this._abortController === controller) {
                this._abortController = null;
                this._hideLoading();
            }
        }
    }

    _showError(message) {
        let errorElem = $();

        if (this._formElem) {
            errorElem = this._formElem.find(".rs-table-error");
        }

        if (errorElem.length > 0) {
            errorElem.text(message).prop("hidden", false);
        } else if (typeof ModalBox === "function") {
            ModalBox.showMessage(message, { alert: ModalBoxAlerts.DANGER });
        } else {
            alert(message);
        }
    }

    _hideError() {
        if (this._formElem) {
            const errorElem = this._formElem.find(".rs-table-error");
            errorElem.text("").prop("hidden", true);
        }
    }

    _showLoading() {
        clearTimeout(this._loadingTimeoutId);
        this._loadingTimeoutId = setTimeout(
            () => {
                this._getTableWrapper()
                    .addClass("rs-table-loading")
                    .append("<div class='rs-table-spinner box-center fs-2 text-secondary'>" +
                        "<i class='fa-solid fa-spinner fa-spin-pulse fa-3x'></i></div>");
            },
            TableFormAsync._LOADING_MS
        );
    }

    _hideLoading() {
        clearTimeout(this._loadingTimeoutId);
        this._getTableWrapper().removeClass("rs-table-loading");
    }

    _newItem() {
        this._showModal(
            this._getNewUrl(),
            this._getNewOptions(),
            async result => {
                if (result) {
                    await this._loadTable();
                }
            });
    }

    _viewItem(itemId) {
        this._showModal(
            this._getViewUrl(itemId),
            this._getViewOptions(),
            async result => {
                if (result) {
                    await this._loadTable();
                }
            });
    }

    _editItem(itemId) {
        this._showModal(
            this._getEditUrl(itemId),
            this._getEditOptions(),
            async result => {
                if (result) {
                    await this._loadTable();
                }
            });
    }

    async _deleteItem(itemId) {
        await this._loadTable({
            handler: "Delete",
            fields: { itemId: itemId }
        });
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
        return $("input[name='__RequestVerificationToken']").val();
    }

    _getItemId(rowElem) {
        return $(rowElem).closest("tr").data("item-id");
    }

    _showModal(url, opt_options, opt_callback) {
        if (!url) {
            console.warn("Modal URL is not specified.")
        } else if (typeof mainObj === "undefined") {
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
