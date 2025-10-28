using MongoDB.Driver;
using realtorAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Realtor API",
        Version = "v1",
        Description = "Real Estate Properties API"
    });
});

var mongoConnectionString = builder.Configuration["MongoDB:ConnectionString"];
var mongoDatabaseName = builder.Configuration["MongoDB:DatabaseName"];

if (!string.IsNullOrEmpty(mongoConnectionString) && !string.IsNullOrEmpty(mongoDatabaseName))
{
    var settings = MongoClientSettings.FromConnectionString(mongoConnectionString);
    settings.ServerApi = new ServerApi(ServerApiVersion.V1);
    
    var mongoClient = new MongoClient(settings);
    var mongoDatabase = mongoClient.GetDatabase(mongoDatabaseName);
    
    builder.Services.AddSingleton(mongoDatabase);
    builder.Services.AddScoped<PropertyService>();
    builder.Services.AddScoped<OwnerService>();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Realtor API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
