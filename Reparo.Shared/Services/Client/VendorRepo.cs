using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;

public interface IVendorRepo
{
    Task<VendorModel?> GetVendorByPlaceIdAsync(string placeId, CancellationToken cancellationToken = default);
    Task<VendorModel?> GetVendorByVendorIdAsync(int vendorId, CancellationToken cancellationToken = default);
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

    public async Task<VendorModel?> GetVendorByVendorIdAsync(int vendorId, CancellationToken ct = default)
    {
        return await _http.GetFromJsonAsync<VendorModel>($"damage/vendor-get?vendorid={vendorId}", ct);
    }
}