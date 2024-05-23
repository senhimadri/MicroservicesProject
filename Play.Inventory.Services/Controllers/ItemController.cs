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
    private readonly IRepository<InventoryItems> _inventoryItemRepository;
    //private readonly CatalogClient _catalogClient;
    private readonly IRepository<CatalogItems> _catalogItemRepository;



	public ItemController(IRepository<InventoryItems> inventoryItemRepository, IRepository<CatalogItems> catalogItemRepository) 
                        => (_inventoryItemRepository, _catalogItemRepository)  = (inventoryItemRepository, catalogItemRepository);

    [HttpGet]
    public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetAsync(Guid userId)
    {
        if (userId == Guid.Empty)
            return BadRequest();

        var inventoryItemEntities = await _inventoryItemRepository.GetAllAsync(item=>item.UserId == userId);

        var itemId = inventoryItemEntities.Select(item => item.CatalogItemId);

        var catalogItemEntitys = await _catalogItemRepository.GetAllAsync(item => itemId.Contains(item.Id));

        var inventoryItemDto = inventoryItemEntities.Select(inventoryItem =>
        {
            var catalogItem = catalogItemEntitys.Single(catalogItem => catalogItem.Id == inventoryItem.CatalogItemId);

            return inventoryItem.AsDto(catalogItem.Name?? string.Empty, catalogItem.Description?? string.Empty);
        });

        return Ok(inventoryItemDto);
    }


    [HttpPost]
    public async Task<ActionResult> PostAsync(GrantItemsDto grantItemsDto)
    {
        var InventoryItem = await _inventoryItemRepository
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

            await _inventoryItemRepository.CreateAsync(InventoryItem);
        }
        else
        {
            InventoryItem.Quentity += grantItemsDto.Quentity;
            await _inventoryItemRepository.UpdateAsync(InventoryItem);
        }

        return Ok();
    }
}

