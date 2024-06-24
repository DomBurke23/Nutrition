using System.Text.Json.Serialization;
using xDesign.Nutrition.Api.Services;
using xDesign.Nutrition.Api.Util;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(new UpperCaseJsonNamingPolicy()));
    });

var csvFileName = builder.Configuration.GetSection("NutritionSearch").GetValue<string>("FileName");
builder.Services.AddScoped(_ => new NutritionSearchService(csvFileName ?? throw new ArgumentNullException("csvFileName")));

var app = builder.Build();

app.MapControllers();

app.Run();

public partial class Program { };