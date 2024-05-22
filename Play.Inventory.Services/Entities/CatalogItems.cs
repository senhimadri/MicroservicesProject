using Play.Common;

namespace Play.Inventory.Services.Entities;


public class CatalogItems : IEntity
{
    public Guid Id { get; set; }

    public string? Name { get; set; }
    public string? Description { get; set; }
}

