using Play.Common;

namespace Play.Inventory.Services.Entities;


public class InventoryItems : IEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid CatalogItemId { get; set; }
    public int Quentity { get; set; }
    public DateTimeOffset AcquiredDate { get; set; }
}

