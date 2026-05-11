const documentView = { handle: null };
const claimSearchView = { handle: null };

document.addEventListener("DOMContentLoaded", () =>
{
    console.log("DOMContentLoaded fired");
});

document.addEventListener("blazor:enhancedload", async () =>
{
    console.log("blazor:enhancedload fired");

    const settings = window.getTheme();
    await window.setCulture(settings.culture);
    window.setTheme(settings);
});

async function initAttachment(name, parameters)
{
    console.log("Registering the Attachment handler");
    console.log({ name, parameters });
}

async function initClaimSearch(name, parameters)
{
    console.log("Registering the Claim Search handler");
    console.log({ name, parameters });
}

async function renderComponent(handleRef, elementId, componentName, params = {})
{
    if (!window.Blazor?.rootComponents) return;

    const element = document.getElementById(elementId);
    if (!element) return;

    handleRef.handle = await Blazor.rootComponents.add(element, componentName, params);
}

async function removeAttachments()
{
    await removeComponent(documentView);
}

async function removeClaims()
{
    await removeComponent(claimSearchView);
}

async function removeComponent(handleRef)
{
    if (!handleRef?.handle)
        return;

    await handleRef.handle.dispose().catch(() => { });
    handleRef.handle = null;
}

function renderAttachment(referenceType, referenceId)
{
    removeAttachments();

    return renderComponent(documentView, "blazor-document", "document", {
        ReferenceType: referenceType,
        ReferenceId: referenceId
    });
}

function renderClaimSearchView()
{
    removeClaims();
    return renderComponent(claimSearchView, "blazor-search", "claimSearch");
}
window.getBrowserLocation = () =>
{
    return new Promise((resolve, reject) =>
    {
        navigator.geolocation.getCurrentPosition(
            pos => resolve({
                lat: pos.coords.latitude,
                lng: pos.coords.longitude
            }),
            err => reject(err.message)
        );
    });
};

//window.setClipboard = function (text)
//{
//    if (!text) return;
//    navigator.clipboard.writeText(text);
//};
