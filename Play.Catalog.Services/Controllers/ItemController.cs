using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Services.Entities;
using Play.Catalog.Services.ItemsDtos;
using Play.Catalog.Services.Remositories;

namespace Play.Catalog.Services.Controllers;

[Route("items")]
[ApiController]
public class ItemController : ControllerBase
{
    private readonly ItemRepository _itemRepos = new();

    [HttpGet]
    public async Task<IEnumerable<ItemDto>> GetAsync()
    {
        var items = (await _itemRepos.GetAllAsync())
                    .Select(item => item.AsDto())
                        ;
        return items;
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<ItemDto>> GetByIdAsync(Guid id)
    {
        var item = await _itemRepos.GetAsync(id);

        if (item is null)
            return NotFound();

        return item.AsDto();
    }

    [HttpPost]
    public async Task<ActionResult<ItemDto>> PostAsync (CreateItemDto createItemDto)
    {
        var item = new Item {
            Name = createItemDto.Name,
            Description= createItemDto.Description,
            Price = createItemDto.Price,
            CreatedDate= DateTimeOffset.UtcNow};

        await _itemRepos.CreateAsync(item);

        return CreatedAtAction(nameof(GetByIdAsync), new { id = item.Id }, item);
    }

    [HttpPut("id")]
    public async Task<IActionResult> PutAsync (Guid Id, UpdateItemDto updatedItemDto)
    {
        var existingItem = await _itemRepos.GetAsync(Id);

        if (existingItem is null)
            return NotFound();

        existingItem.Name = updatedItemDto.Name;
        existingItem.Description = updatedItemDto.Description;
        existingItem.Price = updatedItemDto.Price;

        await _itemRepos.UpdateAsync(existingItem);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var existingItem = await _itemRepos.GetAsync(id);

        if (existingItem is null)
            return NotFound();

        await _itemRepos.RemoveAsync(existingItem.Id);

        return NoContent();
    }

}

