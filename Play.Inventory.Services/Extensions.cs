using Play.Inventory.Services.Dtos;
using Play.Inventory.Services.Entities;

namespace Play.Inventory.Services;

public static class Extensions
{
    public static InventoryItemDto AsDto(this InventoryItems item, string name, string description)
    {
        return new InventoryItemDto(item.CatalogItemId,name,description,item.Quentity,item.AcquiredDate);
    }
}

