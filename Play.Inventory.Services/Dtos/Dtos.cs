namespace Play.Inventory.Services.Dtos;

public record GrantItemsDto(Guid UserId, Guid CatalogItemId, int Quentity);

public record InventoryItemDto(Guid CatalogItemId, string Name, string Description, int Quentity,DateTimeOffset AcquiredDate);


public record CatalogItemDto(Guid Id, string Name, string Description);

