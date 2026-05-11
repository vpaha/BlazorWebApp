using System.Text.Json.Serialization;

public sealed class GeocodeResponse
{
    [JsonPropertyName("results")]
    public List<GeocodeResult> Results { get; set; } = [];
}

public sealed class GeocodeResult
{
    [JsonPropertyName("types")]
    public List<string>? Types { get; set; }

    [JsonPropertyName("formatted_address")]
    public string? FormattedAddress { get; set; }

    [JsonPropertyName("address_components")]
    public List<AddressComponent>? AddressComponents { get; set; }

    [JsonPropertyName("partial_match")]
    public bool? PartialMatch { get; set; }

    [JsonPropertyName("place_id")]
    public string? PlaceId { get; set; }

    [JsonPropertyName("postcode_localities")]
    public List<string>? PostcodeLocalities { get; set; }

    [JsonPropertyName("geometry")]
    public GeocodeGeometry? Geometry { get; set; }
}

public sealed class AddressComponent
{
    [JsonPropertyName("short_name")]
    public string? ShortName { get; set; }

    [JsonPropertyName("long_name")]
    public string? LongName { get; set; }

    [JsonPropertyName("postcode_localities")]
    public List<string>? PostcodeLocalities { get; set; }

    [JsonPropertyName("types")]
    public List<string>? Types { get; set; }
}

public sealed class GeocodeGeometry
{
    [JsonPropertyName("location")]
    public LatLng? Location { get; set; }

    [JsonPropertyName("location_type")]
    public string? LocationType { get; set; } // GeocoderLocationType as string

    [JsonPropertyName("viewport")]
    public LatLngBounds? Viewport { get; set; }

    [JsonPropertyName("bounds")]
    public LatLngBounds? Bounds { get; set; }
}

public sealed class LatLng
{
    [JsonPropertyName("lat")]
    public double Latitude { get; set; }

    [JsonPropertyName("lng")]
    public double Longitude { get; set; }
}

public sealed class LatLngBounds
{
    [JsonPropertyName("northeast")]
    public LatLng? Northeast { get; set; }

    [JsonPropertyName("southwest")]
    public LatLng? Southwest { get; set; }
}