using MassTransit;
using Play.Catalog.Services.Entities;
using Play.Catalog.Services.Settings;
using Play.Common.MongoDb;
using Play.Common.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options=>
{
    options.SuppressAsyncSuffixInActionNames = false;
});

var servicesSettings = builder.Configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();


builder.Services.AddMongo().AddMongoRepository<Item>("items");

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context,configurator)=>
    {
        var rabbitMQSettings = builder.Configuration.GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>();
        configurator.Host(rabbitMQSettings!.Host);
        configurator.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter(prefix: servicesSettings!.ServiceName, includeNamespace: false));
    });
});


//builder.Services.AddMassTransitHostedService();

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
