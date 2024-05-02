using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Services.ItemsDtos;

namespace Play.Catalog.Services.Controllers;

// https://localhost:5001/items
[Route("items")]
[ApiController]
public class ItemController : ControllerBase
{

    private static readonly List<ItemDto> items = new()
    {
        new ItemDto(Guid.NewGuid(),"Potion","Restores a Small Amount of Hp", 5,DateTimeOffset.UtcNow),
        new ItemDto(Guid.NewGuid(),"Antodote","Cures poison",  7 ,DateTimeOffset.UtcNow),
        new ItemDto(Guid.NewGuid(),"Bronze sword","Deals a small amount of Damage",  20 ,DateTimeOffset.UtcNow),
    };

    [HttpGet]
    public IEnumerable<ItemDto> Get()
    {
        return items;
    }


    [HttpGet("{id}")]
    public ActionResult<ItemDto> GetById(Guid id)
    {
        var item = items
                    .Where(item => item.Id == id)
                    .SingleOrDefault();

        if (item is null)
            return NotFound();

        return item;
    }

    [HttpPost]
    public ActionResult<ItemDto> Post(CreateItemDto createItemDto)
    {
        var item = new ItemDto(Guid.NewGuid(),
                               createItemDto.Name,
                               createItemDto.Description,
                               createItemDto.Price,
                               DateTimeOffset.UtcNow);

        items.Add(item);

        return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
    }

    [HttpPut("id")]
    public IActionResult Put(Guid Id, UpdateItemDto updatedItemDto)
    {
        var existingItem = items.Where(x => x.Id == Id).FirstOrDefault();

        if (existingItem is null)
            return NotFound();


        var updatedItem = existingItem with
        {
            Name = updatedItemDto.Name,
            Description = updatedItemDto.Description,
            Price = updatedItemDto.Price
        };
        var index = items.FindIndex(x=>x.Id==Id);

        items[index] = updatedItem;
        return NoContent();
    }
    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        var index = items.FindIndex(x=>x.Id==id);

        if (index< 0)
            return NotFound();

        items.RemoveAt(index);
        return NoContent();
    }

}

