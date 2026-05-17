using System.Text.Json.Serialization;

public sealed class PlaceDto : LocationDto
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("googleMapsUrl")]
    public string? GoogleMaps { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("primaryType")]
    public string? PrimaryType { get; set; }

    [JsonPropertyName("primaryTypeDisplayName")]
    public string? PrimaryTypeDisplayName { get; set; }

    [JsonPropertyName("pureServiceAreaBusiness")]
    public bool TravelToYou { get; set; }

    [JsonPropertyName("types")]
    public string[]? Types { get; set; }    
}

public class LocationDto
{
    [JsonPropertyName("PlaceId")]
    public string? PlaceId { get; set; }

    [JsonPropertyName("formattedAddress")]
    public string? Address { get; set; }

    [JsonPropertyName("addressLine1")]
    public string? AddressLine1 { get; set; }

    [JsonPropertyName("city")]
    public string? City { get; set; }

    [JsonPropertyName("state")]
    public string? State { get; set; }

    [JsonPropertyName("postalCode")]
    public string? PostalCode { get; set; }

    [JsonPropertyName("country")]
    public string? Country { get; set; }

    public string? Region { get; set; }

    public string? Title { get; set; }

    [JsonPropertyName("latitude")]
    public double? Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public double? Longitude { get; set; }
}