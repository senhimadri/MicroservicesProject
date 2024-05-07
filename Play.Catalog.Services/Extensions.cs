using Play.Catalog.Services.Entities;
using Play.Catalog.Services.ItemsDtos;
using System.Runtime.CompilerServices;

namespace Play.Catalog.Services;

public static class Extensions
{
    public static ItemDto AsDto(this Item item) => 
        new(item.Id, item.Name, item.Description, item.Price, item.CreatedDate);

}

