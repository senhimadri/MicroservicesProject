using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Services.Entities;
using Play.Catalog.Services.ItemsDtos;
using Play.Common;
namespace Play.Catalog.Services.Controllers;


[Route("items")]
[ApiController]
public class ItemController : ControllerBase
{
    private readonly IRepository<Item> _itemRepos;
    private static int _requestCounter = 0;

    public ItemController(IRepository<Item> itemRepos) => _itemRepos = itemRepos;



    [HttpGet]
    public async Task<ActionResult<IEnumerable<ItemDto>>> GetAsync()
    {
        _requestCounter++;
        Console.WriteLine($"Request :{_requestCounter} starting. ");

        if (_requestCounter<=2 )
        {
            Console.WriteLine($"Request :{_requestCounter} delaying. ");
            await Task.Delay(TimeSpan.FromSeconds(10));
        }

        if (_requestCounter <= 2)
        {
            Console.WriteLine($"Request{_requestCounter} : 500 (Internal Server Error.)");
            return StatusCode(500);
        }

        var items = (await _itemRepos.GetAllAsync())
                        .Select(item => item.AsDto());

        Console.WriteLine($"Request{_requestCounter} : 200 (Ok)");

        return Ok(items);
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

    [HttpPut("{id}")]
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

    [HttpDelete]
    public async Task<IActionResult> DeleteTestAsync(Guid id)
    {
        var existingItem = await _itemRepos.GetAsync(id);

        if (existingItem is null)
            return NotFound();

        await _itemRepos.RemoveAsync(existingItem.Id);

        return NoContent();
    }
}

