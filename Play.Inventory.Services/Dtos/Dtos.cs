namespace Play.Inventory.Services.Dtos;

public record GrantItemsDto(Guid UserId, Guid CatalogItemId, int Quentity);

public record InventoryItemDto(Guid CatalogItemId, int Quentity,DateTimeOffset AcquiredDate);
