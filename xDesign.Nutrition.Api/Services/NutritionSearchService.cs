using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using xDesign.Nutrition.Api.Dtos;
using xDesign.Nutrition.Api.Model;
using xDesign.Nutrition.Api.Util;

namespace xDesign.Nutrition.Api.Services;

public class NutritionSearchService : INutritionSearchService
{
    private readonly string _csvFileName;

    public NutritionSearchService(string csvFileName)
    {
        _csvFileName = csvFileName;
    }

    public IEnumerable<Food> SearchNutrition(NutritionSearchRequest request)
    {
        var unsorted =  LoadFoodsFromCsvFile()
        .Where(food => request.FatRating == null || food.FatRating == request.FatRating)
        .Where(food => request.MinCalories == null || food.Calories >= request.MinCalories.Value)
        .Where(food => request.MaxCalories == null || food.Calories <= request.MaxCalories.Value)
        .ToList();

        var sorted = SortFoods(unsorted, request.SortCriteria).Take(request.Limit).ToList();
        return sorted;
    }

    private static IEnumerable<Food> SortFoods(IEnumerable<Food> unsorted, IList<Sort> requestSortCriteria)
    {
        if (!requestSortCriteria.Any())
        {
            return unsorted;
        }

        var firstSortCriteria = requestSortCriteria.First();
        var sorted = firstSortCriteria.SortOrder == SortOrder.Asc
            ? unsorted.OrderBy(KeySelector(firstSortCriteria.SortField))
            : unsorted.OrderByDescending(KeySelector(firstSortCriteria.SortField));

        foreach (var sortCriteria in requestSortCriteria.Skip(1))
        {
            sorted = sortCriteria.SortOrder == SortOrder.Asc
                ? sorted.ThenBy(KeySelector(sortCriteria.SortField))
                : sorted.ThenByDescending(KeySelector(sortCriteria.SortField));
        }

        return sorted.ToList();
    }

    private static Func<Food, object> KeySelector(SortField field)
    {
        return field switch
        {
            SortField.Name => f => f.Name,
            SortField.Calories => f => f.Calories,
            _ => throw new ArgumentOutOfRangeException(nameof(field), field, null)
        };
    }
    
    private IEnumerable<Food> LoadFoodsFromCsvFile()
    {
        using var readStream = File.OpenRead(_csvFileName);
        using var reader = new StreamReader(readStream);
        using var csvReader = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture));

        var foods = new List<Food>();

        csvReader.Read();
        csvReader.ReadHeader();
        while (csvReader.Read())
        {
            var name = csvReader.GetField(CsvColumnHeadings.NameField);
            var calories = csvReader.GetField(CsvColumnHeadings.CaloriesField);
            var totalFat = csvReader.GetField(CsvColumnHeadings.TotalFatField);
            var caffeine = csvReader.GetField(CsvColumnHeadings.CaffeineField);
            var servingSize = csvReader.GetField(CsvColumnHeadings.ServingSizeField);

            if (IsValidFood(name, calories, totalFat, caffeine, servingSize))
            {
                foods.Add(new Food
                {
                    Name = name!,
                    Calories = int.Parse(calories!),
                    TotalFat = double.Parse(totalFat![..totalFat.IndexOf('g')]),
                    Caffeine = caffeine!
                });
            }
        }

        return foods;
    }

    private static bool IsValidFood(string? name, string? calories, string? totalFat, string? caffeine, string? servingSize)
    {
        var requiredFields = new[] { name, calories, totalFat, caffeine, servingSize };
        if (requiredFields.Any(string.IsNullOrWhiteSpace))
        {
            return false;
        }

        return servingSize == "100 g";
    }
}