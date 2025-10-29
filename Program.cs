using MongoDB.Driver;
using realtorAPI.Services;
using realtorAPI.DTOs;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

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
    builder.Services.AddScoped<IPropertyService, PropertyService>();
    builder.Services.AddScoped<IOwnerService, OwnerService>();
}

var app = builder.Build();

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

app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (ArgumentException ex)
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        await context.Response.WriteAsJsonAsync(ApiResponse<object>.Error(ex.Message));
    }
    catch (KeyNotFoundException ex)
    {
        context.Response.StatusCode = (int)HttpStatusCode.NotFound;
        await context.Response.WriteAsJsonAsync(ApiResponse<object>.Error(ex.Message));
    }
    catch (Exception ex)
    {
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        await context.Response.WriteAsJsonAsync(ApiResponse<object>.Error("Unexpected error", ex.Message));
    }
});
app.UseAuthorization();
app.MapControllers();

app.Run();
