using Play.Common.MongoDb;
using Play.Inventory.Services.Clients;
using Play.Inventory.Services.Entities;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddMongo()
                .AddMongoRepository<InventoryItems>("inventoryitems");

builder.Services.AddHttpClient<CatalogClient>(client=>
{
    client.BaseAddress = new Uri("https://localhost:7138");
});

builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
