// Contains classes: Mimic, Component
// Depends on scada-common.js, mimic-common.js

// Represents a mimic diagram.
rs.mimic.Mimic = class {
    // Loads a part of the mimic.
    async _loadPart(loadContext) {
        const LoadStep = rs.mimic.LoadStep;
        let continueLoading = false;
        let dto = null;

        if (loadContext.step === LoadStep.UNDEFINED) {
            loadContext.step = LoadStep.PROPERTIES;
            loadContext.result.msg = "Not completed";
        }

        switch (loadContext.step) {
            case LoadStep.PROPERTIES:
                dto = await this._loadProperties(loadContext);
                loadContext.step++;
                break;

            case LoadStep.COMPONENTS:
                dto = await this._loadComponents(loadContext);
                if (dto.ok && dto.data.endOfComponents) {
                    loadContext.step++;
                }
                break;

            case LoadStep.IMAGES:
                break;

            case LoadStep.FACEPLATES:
                break;

            case LoadStep.COMPLETE:
                loadContext.result.ok = true;
                loadContext.result.msg = "";
                break;

            default:
                dto = Dto.fail("Unknown loading step");
                break;
        }

        if (dto !== null) {
            if (dto.ok) {
                continueLoading = true;
            } else {
                loadContext.result.msg = dto.msg;
            }
        }

        return continueLoading;
    }

    // Loads the mimic properties.
    async _loadProperties(loadContext) {
        console.log(ScadaUtils.getCurrentTime() + " Load mimic properties");
        let response = await fetch(loadContext.controllerUrl + "GetMimicProperties?key=" + loadContext.mimicKey);
        return response.ok
            ? Dto.success(await response.json())
            : Dto.fail(await response.statusText);
    }

    // Loads a range of components.
    async _loadComponents(loadContext) {
        console.log(ScadaUtils.getCurrentTime() + " Load components starting with " + loadContext.componentIndex);
        let response = await fetch(loadContext.controllerUrl + "GetComponents?key=" + loadContext.mimicKey +
            "&index=" + loadContext.componentIndex + "&count=" + rs.mimic.LoadContext.COMPONENTS_TO_REQUEST);

        if (response.ok) {
            let dto = await response.json();

            if (dto.ok) {
                loadContext.componentIndex += dto.data.components.length;
            }

            return dto;
        } else {
            return Dto.fail(await response.statusText);
        }
    }

    // Loads a range of images.
    async _loadImages(loadContext) {

    }

    // Clears the mimic.
    clear() {

    }

    // Loads the mimic.
    async load(controllerUrl, key) {
        let startTime = Date.now();
        console.log(ScadaUtils.getCurrentTime() + " Load mimic with key " + key)
        this.clear();

        let loadContext = new rs.mimic.LoadContext(controllerUrl, key);

        while (await this._loadPart(loadContext)) {
            // do nothing
        }

        if (loadContext.result.ok) {
            console.info(ScadaUtils.getCurrentTime() + " Mimic loading completed successfully in " +
                (Date.now() - startTime) + " ms");
        } else {
            console.error(ScadaUtils.getCurrentTime() + " Mimic loading failed: " + loadContext.result.msg);
        }

        return loadContext.result;
    }
}

// Represents a component of a mimic diagram.
rs.mimic.Component = class {

}
