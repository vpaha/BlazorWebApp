window.googleMap = (() =>
{
    let map = null;
    let readyPromise = null;
    let bounds = null;
    let infoWindow = null;
    let dotNetHelper = null;
    let searchCenter = null;

    const markersByPlaceId = new Map();
    const placesById = new Map();

    const placeFields = [
        "id",
        "displayName",
        "formattedAddress",
        "addressComponents",
        "location",
        "googleMapsURI",
        "businessStatus",
        "primaryType",
        "primaryTypeDisplayName",
        "types"
    ];

    const defaultCenter = { lat: 33.7490, lng: -84.3880 };

    const ensureReady = () =>
    {
        readyPromise ??= Promise.all([
            google.maps.importLibrary("maps"),
            google.maps.importLibrary("marker"),
            google.maps.importLibrary("places")
        ]).then(([, markerLib, placesLib]) => ({
            AdvancedMarkerElementCtor: markerLib.AdvancedMarkerElement,
            PlaceCtor: placesLib.Place
        }));

        return readyPromise;
    };

    const getMapOrThrow = () =>
    {
        if (!map)
            throw new Error("Map not initialized. Call initMap first.");

        return map;
    };

    async function initMap(gmpMapSelector, lat, lng, zoom = 13, options = {})
    {
        const { } = await ensureReady();
        const mapEl = document.querySelector(gmpMapSelector);

        if (!mapEl)
            throw new Error(`<gmp-map> not found: ${gmpMapSelector}`);

        if (!mapEl.innerMap)
        {
            await new Promise(resolve =>
                mapEl.addEventListener("gmp-ready", resolve, { once: true }));
        }

        map = mapEl.innerMap;

        searchCenter = hasValidCoordinates(lat, lng)
            ? { lat, lng }
            : defaultCenter;

        map.setOptions({
            center: searchCenter,
            zoom,
            mapId: "5a696fac23189433db35035a",
            mapTypeControl: false,
            streetViewControl: false,
            fullscreenControl: false,
            ...options
        });

        map.addListener("click", () => infoWindow?.close());

        bounds = new google.maps.LatLngBounds();

        infoWindow = new google.maps.InfoWindow({
            headerDisabled: true
        });

        return true;
    }

    async function loadPlaces(category, maxCount = 10, helper, callBack)
    {
        var places = await fetchPlaces(category, maxCount)

        if (callBack != null)
        {
            const placeDtos = places.map(toPlaceDto);
            await helper.invokeMethodAsync(callBack, placeDtos);
        }
    }

    async function fetchPlaces(category, maxCount = 10, helper = null)
    {
        if (helper)
            dotNetHelper = helper;
        else
            dotNetHelper = null;

        const {
            PlaceCtor,
            AdvancedMarkerElementCtor
        } = await ensureReady();

        const mapInstance = getMapOrThrow();

        clearMarkers();

        bounds = new google.maps.LatLngBounds();

        mapInstance.setCenter(searchCenter);

        const { places } = await PlaceCtor.searchByText({
            textQuery: category,
            fields: placeFields,
            locationBias: {
                center: searchCenter,
                radius: 15000
            },
            maxResultCount: maxCount
        });

        if (!places?.length)
            return [];

        for (const place of places)
        {
            addPlaceMarker(place, AdvancedMarkerElementCtor, mapInstance);
        }

        fitMapToBounds(mapInstance);
        return places;
    }

    function addPlaceMarker(place, AdvancedMarkerElementCtor, mapInstance)
    {
        if (!place?.id || !place?.location)
            return null;

        placesById.set(place.id, place);

        const marker = new AdvancedMarkerElementCtor({
            map: mapInstance,
            position: place.location,
            title: getDisplayText(place.displayName),
            gmpClickable: true
        });

        marker.addListener("gmp-click", () =>
        {
            openInfoWindow(place, marker, mapInstance);
        });

        markersByPlaceId.set(place.id, marker);
        bounds?.extend(place.location);

        return marker;
    }

    async function searchPlaceById(placeId, keepExistingMarkers = false)
    {
        if (!placeId)
            return null;

        dotNetHelper = null;

        const {
            PlaceCtor,
            AdvancedMarkerElementCtor
        } = await ensureReady();

        const mapInstance = getMapOrThrow();

        if (keepExistingMarkers && markersByPlaceId.has(placeId))
        {
            focusPlace(placeId);
            return markersByPlaceId.get(placeId);
        }

        if (!keepExistingMarkers)
            clearMarkers();

        bounds = new google.maps.LatLngBounds();

        const place = new PlaceCtor({ id: placeId });

        await place.fetchFields({
            fields: placeFields
        });

        if (!place.location)
            return null;

        const marker = addPlaceMarker(
            place,
            AdvancedMarkerElementCtor,
            mapInstance);

        focusPlace(placeId);

        return marker;
    }

    function focusPlace(placeId, zoom = 15)
    {
        const place = placesById.get(placeId);
        const marker = markersByPlaceId.get(placeId);

        if (!place || !marker || !map)
            return false;

        map.panTo(place.location);
        map.setZoom(zoom);

        openInfoWindow(place, marker, map);

        return true;
    }

    async function openVendorByPlaceId(placeId)
    {
        const place = placesById.get(placeId);

        if (!place) return false;

        await dotNetHelper.invokeMethodAsync(
            "OpenVendor",
            toPlaceDto(place));

        return true;
    }

    function openInfoWindow(place, marker, mapInstance)
    {
        infoWindow?.close();

        infoWindow.setContent(createPlaceInfoWindow(place));

        infoWindow.open({
            anchor: marker,
            map: mapInstance
        });

        mapInstance.panTo(place.location);
    }

    function createPlaceInfoWindow(place)
    {
        const placeId = escapeHtml(place.id);
        const name = escapeHtml(getDisplayText(place.displayName));
        const address = escapeHtml(place.formattedAddress);
        const type = escapeHtml(getDisplayText(place.primaryTypeDisplayName));
        const googleMapsUrl = escapeAttribute(place.googleMapsURI);

        const vendorButton = dotNetHelper
            ? `
            <button type="button"
                    onclick="window.googleMap.openVendorByPlaceId('${placeId}')">
                Vendor Information
            </button>`
            : "";

        return `
        <div style="min-width:240px">
            <strong>${name}</strong>

            <div>${address}</div>

            <div>
                <i>${type}</i>
            </div>

            <div style="margin-top:10px">
                ${vendorButton}

                <a href="${googleMapsUrl}"
                   target="_blank"
                   rel="noopener noreferrer"
                   style="margin-left:${dotNetHelper ? "8px" : "0"}">
                    Open in Google Maps
                </a>
            </div>
        </div>`;
    }

    function toPlaceDto(place)
    {
        const components = place.addressComponents ?? [];

        const getComponent = type =>
            components.find(c => c.types?.includes(type));

        return {
            placeId: place.id,
            name: getDisplayText(place.displayName),
            formattedAddress: place.formattedAddress,
            addressLine1:
                `${getComponent("street_number")?.longText ?? ""} ${getComponent("route")?.longText ?? ""}`.trim(),
            city:
                getComponent("locality")?.longText ??
                getComponent("postal_town")?.longText,
            state:
                getComponent("administrative_area_level_1")?.shortText,
            postalCode:
                getComponent("postal_code")?.longText,
            country:
                getComponent("country")?.shortText,
            latitude:
                place.location?.lat(),
            longitude:
                place.location?.lng(),
            googleMapsUrl:
                place.googleMapsURI,
            status:
                place.businessStatus,
            primaryType:
                place.primaryType,
            primaryTypeDisplayName:
                getDisplayText(place.primaryTypeDisplayName),
            types:
                place.types
        };
    }

    function clearMarkers()
    {
        infoWindow?.close();

        for (const marker of markersByPlaceId.values())
        {
            marker.map = null;
        }

        markersByPlaceId.clear();
        placesById.clear();
    }

    function fitMapToBounds(mapInstance)
    {
        if (!bounds || bounds.isEmpty())
            return;

        mapInstance.fitBounds(bounds);
    }

    function hasValidCoordinates(lat, lng)
    {
        return Number.isFinite(lat) && Number.isFinite(lng);
    }

    function getDisplayText(value)
    {
        return value?.text ?? value ?? "";
    }

    function escapeHtml(value)
    {
        return String(value ?? "")
            .replaceAll("&", "&amp;")
            .replaceAll("<", "&lt;")
            .replaceAll(">", "&gt;")
            .replaceAll('"', "&quot;")
            .replaceAll("'", "&#039;");
    }

    function escapeAttribute(value)
    {
        return escapeHtml(value);
    }

    return {
        ensureReady,
        initMap,
        fetchPlaces,
        loadPlaces,
        searchPlaceById,
        focusPlace,
        clearMarkers,
        openVendorByPlaceId
    };
})();