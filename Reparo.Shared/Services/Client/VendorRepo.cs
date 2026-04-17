using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;

public interface IVendorRepo
{
    Task<VendorModel?> GetVendorByPlaceIdAsync(string placeId, CancellationToken cancellationToken = default);
}

public sealed class VendorRepo : IVendorRepo
{
    private readonly HttpClient _http;

    public VendorRepo(HttpClient http, AuthenticationStateProvider authProvider)
    {
        _http = http;
    }

    public async Task<VendorModel?> GetVendorByPlaceIdAsync(string placeId, CancellationToken ct = default)
    {
        return await _http.GetFromJsonAsync<VendorModel>($"damage/vendor-get?placeid={placeId}", ct);
    }
}