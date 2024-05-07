using MongoDB.Driver;
using Play.Catalog.Services.Entities;

namespace Play.Catalog.Services.Remositories;

public class ItemRepository
{
    private const string collectionName = "items";
    private readonly IMongoCollection<Item> dbCollection;

    private readonly FilterDefinitionBuilder<Item> filterBuilder = Builders<Item>.Filter;
}

