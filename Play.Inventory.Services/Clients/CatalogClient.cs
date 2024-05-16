using Play.Inventory.Services.Dtos;

namespace Play.Inventory.Services.Clients;

public class CatalogClient
{
    private readonly HttpClient _httpclient;

    public CatalogClient(HttpClient httpclient) => _httpclient = httpclient;

    public async Task<IReadOnlyCollection<CatalogItemDto>> GetCatalogItemsAsync()
    {
        var items = await _httpclient.GetFromJsonAsync<IReadOnlyCollection<CatalogItemDto>>("/items");
        return (items ?? Array.Empty<CatalogItemDto>());
    }
}

