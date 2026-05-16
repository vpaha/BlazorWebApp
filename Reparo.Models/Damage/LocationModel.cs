using System.Text.Json.Serialization;

public sealed class PlaceDto : LocationDto
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("phone")]
    public string? Phone { get; set; }

    [JsonPropertyName("websiteUrl")]
    public string? Website { get; set; }

    [JsonPropertyName("googleMapsUrl")]
    public string? GoogleMaps { get; set; }

    [JsonPropertyName("types")]
    public string[]? Types { get; set; }

    [JsonPropertyName("rating")]
    public double? Rating { get; set; }

    [JsonPropertyName("reviewCount")]
    public int? ReviewCount { get; set; }

    [JsonPropertyName("isOperational")]
    public bool IsOperational { get; set; }
}

public class LocationDto
{
    [JsonPropertyName("PlaceId")]
    public string? PlaceId { get; set; }

    [JsonPropertyName("address")]
    public string? Address { get; set; }

    [JsonPropertyName("addressLine1")]
    public string? AddressLine1 { get; set; }

    [JsonPropertyName("addressLine2")]
    public string? AddressLine2 { get; set; }

    [JsonPropertyName("city")]
    public string? City { get; set; }

    [JsonPropertyName("state")]
    public string? State { get; set; }

    [JsonPropertyName("postalCode")]
    public string? PostalCode { get; set; }

    [JsonPropertyName("country")]
    public string? Country { get; set; } = "US";

    [JsonPropertyName("region")]
    public string? Region { get; set; }

    [JsonPropertyName("placename")]
    public string? Title { get; set; }

    [JsonPropertyName("latitude")]
    public double? Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public double? Longitude { get; set; }
}