using xDesign.Nutrition.Api.Dtos;
using xDesign.Nutrition.Api.Model;

namespace xDesign.Nutrition.Api.Services;

public class NutritionSearchService : INutritionSearchService
{
    private readonly string _fileName;
    private readonly INutritionSortService _sortService;

    public NutritionSearchService(string fileName,
        INutritionSortService sortService)
    {
        _fileName = fileName;
        _sortService = sortService;
    }

    private IEnumerable<Food> LoadFoods(string extension)
    {
        var dataLoaderService = new DataLoaderService();
        Func<string, IEnumerable<Food>> loadFunction = extension switch
        {
            ".csv" => dataLoaderService.LoadFoodsFromCsvFile,
            ".xml" => dataLoaderService.LoadFoodsFromXmlFile,
            ".json" => dataLoaderService.LoadFoodsFromJsonFile,
            _ => throw new NotSupportedException($"Unsupported file type: {extension}")
        };

        return loadFunction(_fileName);
    }

    public IEnumerable<Food> SearchNutrition(NutritionSearchRequest request)
    {
        string extension = Path.GetExtension(_fileName).ToLower();
        var unsorted = LoadFoods(extension)
            .Where(food => request.FatRating == null || food.FatRating == request.FatRating)
            .Where(food => request.MinCalories == null || food.Calories >= request.MinCalories.Value)
            .Where(food => request.MaxCalories == null || food.Calories <= request.MaxCalories.Value)
            .ToList();
        var sorted = _sortService.SortFoods(unsorted, request.SortCriteria).Take(request.Limit).ToList();
        return sorted;
    }
}