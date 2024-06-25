using System.Text.Json.Serialization;
using xDesign.Nutrition.Api.Services;
using xDesign.Nutrition.Api.Util;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(new UpperCaseJsonNamingPolicy()));
    });

var fileName = builder.Configuration.GetSection("NutritionSearch").GetValue<string>("FileName");

// Register Services
builder.Services.AddScoped<NutritionSortService>();
builder.Services.AddScoped<DataLoaderService>();
builder.Services.AddScoped(serviceProvider =>
{
    var sortService = serviceProvider.GetRequiredService<NutritionSortService>();
    var dataLoaderService = serviceProvider.GetRequiredService<DataLoaderService>();
    return new NutritionSearchService(
        fileName ?? throw new ArgumentNullException(nameof(fileName)),
        sortService,
        dataLoaderService
    );
});

var app = builder.Build();

app.MapControllers();

app.Run();

public partial class Program { };