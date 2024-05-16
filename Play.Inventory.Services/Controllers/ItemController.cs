using Microsoft.AspNetCore.Mvc;
using Play.Common;
using Play.Inventory.Services.Clients;
using Play.Inventory.Services.Dtos;
using Play.Inventory.Services.Entities;

namespace Play.Inventory.Services.Controllers;

[Route("items")]
[ApiController]
public class ItemController : ControllerBase
{
    private readonly IRepository<InventoryItems> _itemRepository;
    private readonly CatalogClient _catalogClient; 

	public ItemController(IRepository<InventoryItems> itemRepository, CatalogClient catalogClient) 
                        => (_itemRepository, _catalogClient)  = (itemRepository, catalogClient);

    [HttpGet]
    public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetAsync(Guid userId)
    {
        if (userId == Guid.Empty)
            return BadRequest();



        var catalogItems = await _catalogClient.GetCatalogItemsAsync();

        var InventoryItemEntities = await _itemRepository.GetAllAsync(item=>item.UserId == userId);

        var inventoryItemDto = InventoryItemEntities.Select(inventoryItem =>
        {
            var catalogItem = catalogItems.Single(catalogItem => catalogItem.Id == inventoryItem.CatalogItemId);

            return inventoryItem.AsDto(catalogItem.Name, catalogItem.Description);
        });

        return Ok(inventoryItemDto);
    }


    [HttpPost]
    public async Task<ActionResult> PostAsync(GrantItemsDto grantItemsDto)
    {
        var InventoryItem = await _itemRepository
                                .GetAsync(item => item.UserId == grantItemsDto.UserId
                                                    && item.CatalogItemId == grantItemsDto.CatalogItemId);

        if (InventoryItem is null)
        {
            InventoryItem = new InventoryItems
            {
                CatalogItemId = grantItemsDto.CatalogItemId,
                UserId = grantItemsDto.UserId,
                Quentity =grantItemsDto.Quentity,
                AcquiredDate = DateTimeOffset.UtcNow
            };

            await _itemRepository.CreateAsync(InventoryItem);
        }
        else
        {
            InventoryItem.Quentity += grantItemsDto.Quentity;
            await _itemRepository.UpdateAsync(InventoryItem);
        }

        return Ok();
    }
}

