using Play.Catalog.Services.Settings;
using Play.Catalog.Services.Remositories;
using Play.Catalog.Services.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options=>
{
    options.SuppressAsyncSuffixInActionNames = false;
});

var servicesSettings = builder.Configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();


builder.Services.AddMongo().AddMongoRepository<Item>("items");
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
