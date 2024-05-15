using Play.Inventory.Services.Dtos;
using Play.Inventory.Services.Entities;

namespace Play.Inventory.Services;

public static class Extensions
{
    public static InventoryItemDto AsDto(this InventoryItems item)
    {
        return new InventoryItemDto(item.CatalogItemId,item.Quentity,item.AcquiredDate);
    }
}

