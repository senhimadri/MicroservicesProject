using MassTransit;
using Play.Catalog.Contracts;
using Play.Common;
using Play.Inventory.Services.Entities;

namespace Play.Inventory.Services.Consumers;

public class CatalogItemCreatedConsumers : IConsumer<CatalogItemCreated>
{

    private readonly IRepository<CatalogItems> _repository;

    public CatalogItemCreatedConsumers(IRepository<CatalogItems> repository) => _repository= repository;


    public async Task Consume(ConsumeContext<CatalogItemCreated> context)
    {
        var message = context.Message;
        var item = await _repository.GetAsync(message.ItemId);

        if (item is not null)
            return;

        item = new CatalogItems
        {
            Id = message.ItemId,
            Name = message.Name,
            Description = message.Description
        };

        await _repository.CreateAsync(item);
    }
}

