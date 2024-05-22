using MassTransit;
using Play.Catalog.Contracts;
using Play.Common;
using Play.Inventory.Services.Entities;

namespace Play.Inventory.Services.Consumers;

public class CatalogItemUpdatedConsumers : IConsumer<CatalogItemUpdated>
{

    private readonly IRepository<CatalogItems> _repository;

    public CatalogItemUpdatedConsumers(IRepository<CatalogItems> repository) => _repository= repository;


    public async Task Consume(ConsumeContext<CatalogItemUpdated> context)
    {
        var message = context.Message;
        var item = await _repository.GetAsync(message.ItemId);

        if (item is null)
        {
            item = new CatalogItems
            {
                Id = message.ItemId,
                Name = message.Name,
                Description = message.Description
            };
            await _repository.CreateAsync(item);
        }
        else
        {
            item.Name = message.Name;
            item.Description = message.Description;

            await _repository.UpdateAsync(item);
        }


    }
}

