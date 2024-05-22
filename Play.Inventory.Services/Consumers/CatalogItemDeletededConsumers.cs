using MassTransit;
using Play.Catalog.Contracts;
using Play.Common;
using Play.Inventory.Services.Entities;

namespace Play.Inventory.Services.Consumers;

public class CatalogItemDeletededConsumers : IConsumer<CatalogItemDeleted>
{

    private readonly IRepository<CatalogItems> _repository;

    public CatalogItemDeletededConsumers(IRepository<CatalogItems> repository) => _repository= repository;

    public async Task Consume(ConsumeContext<CatalogItemDeleted> context)
    {
        var message = context.Message;
        var item = await _repository.GetAsync(message.ItemId);

        if (item is null)
            return;

        await _repository.RemoveAsync(item.Id);
    }
}

