using System.Text.Json.Serialization;

public sealed class GoogleNearbyResponse
{
    [JsonPropertyName("places")]
    public List<GooglePlace>? Places { get; set; }
}

public sealed class GooglePlace
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }
    [JsonPropertyName("latitude")]
    public double Latitude { get; set; }
    [JsonPropertyName("longitude")]
    public double Longitude { get; set; }
    [JsonPropertyName("address")]
    public string? Address { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }
    [JsonPropertyName("rating")]
    public double? Rating { get; set; }
    [JsonPropertyName("userrating")]
    public int? UserRating { get; set; }
    [JsonPropertyName("businessstatus")]
    public string? BusinessStatus { get; set; }
    [JsonPropertyName("website")]
    public string? Website { get; set; }
    [JsonPropertyName("phonenumber")]
    public string? PhoneNumber { get; set; }
    [JsonPropertyName("googlemaps")]
    public string? GoogleMaps { get; set; }
}

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
    //public double? Rating { get; set; }
    //public int? UserRatingCount { get; set; }

    //public string? BusinessStatus { get; set; }

    //public string? Website { get; set; }
    //public string? Phone { get; set; }
    //public string? MapsUrl { get; set; }
}

public class LocationDto
{
    public string? PlaceId { get; set; }
    public string? Address { get; set; }

    public string? Region { get; set; }
    public string? Placename { get; set; }

    public double Latitude { get; set; }
    public double Longitude { get; set; }
}