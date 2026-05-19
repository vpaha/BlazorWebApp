window.googleMap = (() =>
{
    let map = null;
    let readyPromise = null;
    let bounds = null;
    let markers = [];
    let infoWindow = null;
    let dotNetHelper = null;
    let dotNetCallBack = null;
    let searchCenter = null;

    const ensureReady = () =>
    {
        readyPromise ??= Promise.all([
            google.maps.importLibrary("maps"),
            google.maps.importLibrary("marker"),
            google.maps.importLibrary("geocoding"),
            google.maps.importLibrary("places"),
        ]).then(([, markerLib, geocodingLib, placesLib]) => ({
            AdvancedMarkerElementCtor: markerLib.AdvancedMarkerElement,
            GeocoderCtor: geocodingLib.Geocoder,
            PlaceCtor: placesLib.Place,
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
        await ensureReady();

        const mapEl = document.querySelector(gmpMapSelector);
        if (!mapEl) throw new Error(`<gmp-map> not found: ${gmpMapSelector}`);

        if (!mapEl.innerMap)
        {
            await new Promise(resolve =>
                mapEl.addEventListener("gmp-ready", resolve, { once: true }));
        }

        map = mapEl.innerMap;

        const hasValidCoordinates =
            Number.isFinite(lat) &&
            Number.isFinite(lng);

        searchCenter = hasValidCoordinates
            ? { lat, lng }
            : { lat: 33.7490, lng: -84.3880 };

        map.setOptions({
            center: searchCenter,
            zoom,
            mapId: "5a696fac23189433db35035a",
            mapTypeControl: false,
            streetViewControl: false,
            fullscreenControl: false,
            ...options,
        });

        map.addListener("click", () =>
        {
            infoWindow?.close();
        });

        bounds = new google.maps.LatLngBounds();

        infoWindow = new google.maps.InfoWindow({
            headerDisabled: true,
        });

        return true;
    }

    async function fetchPlaces(category, maxCount = 2, helper = null, callback = null)
    {
        dotNetHelper = helper;
        dotNetCallBack = callback;

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
            fields: [
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
            ],
            locationBias: {
                center: searchCenter,
                radius: 15000
            },
            maxResultCount: maxCount
        });

        if (!places?.length)
            return 0;

        for (const place of places)
        {
            addPlaceMarker(place, AdvancedMarkerElementCtor, mapInstance);
        }

        if (!bounds.isEmpty())
        {
            mapInstance.fitBounds(bounds);
        }

        const placeDtos = places.map(toPlaceDto);
        if (dotNetHelper && dotNetCallBack)
        {
            await dotNetHelper.invokeMethodAsync(dotNetCallBack, placeDtos);
        }

        return placeDtos.length;
    }

    function addPlaceMarker(place, AdvancedMarkerElementCtor, mapInstance)
    {
        if (!place?.location)
            return null;

        const marker = new AdvancedMarkerElementCtor({
            map: mapInstance,
            position: place.location,
            title: place.displayName?.text ?? place.displayName,
            gmpClickable: true
        });

        marker.addListener("gmp-click", async () =>
        {
            infoWindow?.close();

            if (dotNetHelper)
            {
                await dotNetHelper.invokeMethodAsync("OpenVendor", toPlaceDto(place));
            }
            else
            {
                infoWindow.setContent(createPlaceInfoWindow(place));
                infoWindow.open({ anchor: marker, map: mapInstance });
                mapInstance.panTo(place.location);
            }
        });

        markers.push(marker);

        bounds?.extend(place.location);

        return marker;
    }

    async function searchPlaceById(placeId)
    {
        const {
            PlaceCtor,
            AdvancedMarkerElementCtor
        } = await ensureReady();

        if (!placeId)
            return null;

        const mapInstance = getMapOrThrow();

        clearMarkers();

        bounds = new google.maps.LatLngBounds();

        const place = new PlaceCtor({
            id: placeId
        });

        await place.fetchFields({
            fields: [
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
            ]
        });

        if (!place.location) return null;

        const marker = addPlaceMarker(
            place,
            AdvancedMarkerElementCtor,
            mapInstance
        );

        mapInstance.setCenter(place.location);

        mapInstance.setZoom(15);

        infoWindow?.close();

        infoWindow.setContent(createPlaceInfoWindow(place));

        infoWindow.open({
            anchor: marker,
            map: mapInstance
        });

        return marker;
    }

    function createPlaceInfoWindow(place)
    {
        return `
            <div style="min-width:240px">
                ${`<strong>${place.displayName}</strong>`}
                ${`<div>${place.formattedAddress}</div>`}
                ${`<div><i>${place.primaryTypeDisplayName}</i></div>`}
                ${`<div><a href="${place.googleMapsURI}" target="_blank" rel="noopener noreferrer">Open in Google Maps</a></div>`}
            </div>`;
    }

    function toPlaceDto(place)
    {
        const components = place.addressComponents ?? [];
        const getComponent = (type) => components.find(c => c.types?.includes(type));

        return {
            placeId: place.id,
            name: place.displayName,
            formattedAddress: place.formattedAddress,
            addressLine1: `${getComponent("street_number")?.longText ?? ""} ${getComponent("route")?.longText ?? ""}`.trim(),
            city: getComponent("locality")?.longText ?? getComponent("postal_town")?.longText,
            state: getComponent("administrative_area_level_1")?.shortText,
            postalCode: getComponent("postal_code")?.longText,
            country: getComponent("country")?.shortText,
            latitude: place.location?.lat(),
            longitude: place.location?.lng(),
            googleMapsUrl: place.googleMapsURI,
            status: place.businessStatus,
            primaryType: place.primaryType,
            primaryTypeDisplayName: place.primaryTypeDisplayName?.text ?? place.primaryTypeDisplayName,
            types: place.types,
        };
    }

    function clearMarkers()
    {
        for (const marker of markers)
        {
            marker.map = null;
        }

        markers = [];
    }

    return {
        ensureReady,
        initMap,
        fetchPlaces,
        clearMarkers,
        searchPlaceById
    };
})();