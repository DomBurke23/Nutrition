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

// Register NutritionSortService
builder.Services.AddScoped<NutritionSortService>();

// Register NutritionSearchService
builder.Services.AddScoped(serviceProvider =>
{
    var sortService = serviceProvider.GetRequiredService<NutritionSortService>();
    return new NutritionSearchService(
        fileName ?? throw new ArgumentNullException(nameof(fileName)),
        sortService
    );
});

var app = builder.Build();

app.MapControllers();

app.Run();

public partial class Program { };