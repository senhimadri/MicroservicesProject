using Play.Common.MongoDb;
using Play.Inventory.Services.Clients;
using Play.Inventory.Services.Entities;
using Polly;
using Polly.Timeout;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddMongo()
                .AddMongoRepository<InventoryItems>("inventoryitems");

Random jitterer = new Random();

builder.Services.AddHttpClient<CatalogClient>(client=>
{
    client.BaseAddress = new Uri("https://localhost:7138");
})
    .AddTransientHttpErrorPolicy(builders => builders.Or<TimeoutRejectedException>().WaitAndRetryAsync(
        retryCount: 5,
        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,retryAttempt))
                                                + TimeSpan.FromMilliseconds(jitterer.Next(0,1000)),
        onRetry:(outcome, timeSpan, retryAttempt)=>
        {
            var serviceProvider = builder.Services.BuildServiceProvider();
            serviceProvider.GetService<ILogger<CatalogClient>>()?
                .LogWarning($"Retry attempt {retryAttempt} due to {outcome.Exception}.");
        }))
    .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(1)));

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
