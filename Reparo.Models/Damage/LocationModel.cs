using System.Text.Json.Serialization;

public sealed class GoogleDisplayName
{
    public string? Text { get; set; }
}

public sealed class GoogleLocation
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}

public sealed class NearbyPlacesDto
{
    public List<PlaceDto> Places { get; set; } = [];
}

public sealed class PlaceDto : LocationDto
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("rating")]
    public double? Rating { get; set; }

    [JsonPropertyName("reviewCount")]
    public int? ReviewCount { get; set; }

    [JsonPropertyName("phone")]
    public string? Phone { get; set; }

    [JsonPropertyName("website")]
    public string? Website { get; set; }

    [JsonPropertyName("googlemaps")]
    public string? googleMaps { get; set; }    

    [JsonPropertyName("types")]
    public string[]? Types { get; set; }
}

public class LocationDto
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("address")]
    public string? Address { get; set; }

    [JsonPropertyName("region")]
    public string? Region { get; set; }

    [JsonPropertyName("placename")]
    public string? Title { get; set; }

    [JsonPropertyName("latitude")]
    public double Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public double Longitude { get; set; }
}