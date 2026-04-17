window.googleMap = (() =>
{
    let map = null;
    let readyPromise = null;
    let bounds = null;
    let markers = [];
    let infoWindow = null;
    let dotNetHelper = null;

    const ensureReady = () =>
    {
        readyPromise ??= Promise.all([
            google.maps.importLibrary("maps"),
            google.maps.importLibrary("marker"),
            google.maps.importLibrary("geocoding"),
            google.maps.importLibrary("places"),
        ]).then(([mapsLib, markerLib, geocodingLib, placesLib]) => ({
            AdvancedMarkerElementCtor: markerLib.AdvancedMarkerElement,
            GeocoderCtor: geocodingLib.Geocoder,
            PlaceCtor: placesLib.Place,
        }));

        return readyPromise;
    };

    const getMapOrThrow = () =>
    {
        if (!map) throw new Error("Map not initialized. Call initMap first.");
        return map;
    };

    async function initMap(gmpMapId, lat, lng, zoom = 13, helper = null, options = {})
    {
        await ensureReady();

        dotNetHelper = helper;

        const mapEl = document.getElementById(gmpMapId);
        if (!mapEl) throw new Error(`<gmp-map> not found: ${gmpMapId}`);

        if (!mapEl.innerMap)
        {
            await new Promise(resolve =>
                mapEl.addEventListener("gmp-ready", resolve, { once: true })
            );
        }

        map = mapEl.innerMap;
        map.setOptions({
            center: { lat, lng },
            zoom,
            mapId: "5a696fac23189433db35035a",
            mapTypeControl: false,
            streetViewControl: false,
            fullscreenControl: false,
            ...options,
        });
        map.addListener("click", () =>
        {
            if (infoWindow) infoWindow.close();
        });

        bounds = new google.maps.LatLngBounds();
        infoWindow = new google.maps.InfoWindow({
            headerDisabled: true,
        });

        return true;
    }

    async function searchPlaces(lat, lng)
    {
        const { PlaceCtor } = await ensureReady();

        clearMarkers();
        bounds = new google.maps.LatLngBounds();

        getMapOrThrow().setCenter({ lat, lng });

        const { places } = await PlaceCtor.searchByText({
            textQuery: "general contractor",
            fields: ["id"],
            locationBias: getMapOrThrow().getCenter(),
            maxResultCount: 12,
        });

        if (!places?.length) return [];

        for (const p of places)
        {
            if (!p?.id) continue;
            await addMarkerByPlaceId(p.id);
        }

        if (markers.length > 1)
            getMapOrThrow().fitBounds(bounds);
        else if (markers.length === 1)
            getMapOrThrow().setCenter(bounds.getCenter());

        return places;
    }

    async function addMarkerByPlaceId(placeId)
    {
        const mapInstance = getMapOrThrow();
        const { AdvancedMarkerElementCtor, PlaceCtor } = await ensureReady();

        const place = new PlaceCtor({ id: placeId });
        await place.fetchFields({
            fields: ["displayName", "location", "formattedAddress", "googleMapsURI", "nationalPhoneNumber", "rating", "userRatingCount"]
        });

        if (!place.location) return null;

        const displayNameText = place.displayName;

        const marker = new AdvancedMarkerElementCtor({
            map: mapInstance,
            position: place.location,
            title: displayNameText,
            gmpClickable: true,
            collisionBehavior: google.maps.CollisionBehavior?.REQUIRED_AND_HIDES_OPTIONAL,
        });

        marker.addListener("gmp-click", () =>
        {
            const content = document.createElement("div");
            content.style = "cursor:pointer;"

            content.addEventListener("click", () =>
            {
                infoWindow.close();
                dotNetHelper.invokeMethodAsync("OpenVendorDialog", placeId, displayNameText);
            });

            const nameDiv = document.createElement("div");
            nameDiv.textContent = displayNameText;
            nameDiv.style.fontWeight = "bold";
            nameDiv.style.marginBottom = "10px";

            const addressDiv = document.createElement("div");
            addressDiv.textContent = place.formattedAddress ?? "";
            content.append(nameDiv, addressDiv);

            if (place.nationalPhoneNumber)
            {
                const phoneDiv = document.createElement("div");
                phoneDiv.textContent = place.nationalPhoneNumber;
                content.appendChild(phoneDiv);
            }
            if (place.googleMapsURI)
            {
                const link = document.createElement("a");
                link.href = place.googleMapsURI;
                link.target = "_blank";
                link.rel = "noopener noreferrer";
                link.textContent = "map";
                content.appendChild(link);
            }
            if (place.rating)
            {
                const ratingDiv = document.createElement("div");
                ratingDiv.textContent = `${place.rating.toFixed(1)} ⭐ (${place.userRatingCount} reviews)`;
                content.appendChild(ratingDiv);
            }

            infoWindow.setContent(content);
            infoWindow.open({
                anchor: marker,
                map: mapInstance,
            });

            mapInstance.panTo(place.location);
        });

        markers.push(marker);
        bounds.extend(place.location);
        return marker;
    }

    function clearMarkers()
    {
        for (const m of markers) m.map = null;
        markers = [];
    }

    return { ensureReady, initMap, searchPlaces, clearMarkers };
})();