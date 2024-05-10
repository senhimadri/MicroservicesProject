using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Play.Catalog.Services.Entities;
using Play.Catalog.Services.Remositories;
using Play.Catalog.Services.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options=>
{
    options.SuppressAsyncSuffixInActionNames = false;
});

var servicesSettings = builder.Configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();

builder.Services.AddSingleton(serviceProvider =>
{
    var mongoDbSettings = builder.Configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
    var MongoClient = new MongoClient( mongoDbSettings!.ConnectionString ?? string.Empty );
    return MongoClient.GetDatabase(servicesSettings!.ServiceName ?? string.Empty );
});

builder.Services.AddSingleton<IRepository<Item>>(serviceProvider=>
{
    var database = serviceProvider.GetService<IMongoDatabase>();
    return new MongoRepository<Item>(database!, "items");
});

BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

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
