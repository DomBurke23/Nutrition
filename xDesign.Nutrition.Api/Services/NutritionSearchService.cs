using xDesign.Nutrition.Api.Dtos;
using xDesign.Nutrition.Api.Model;

namespace xDesign.Nutrition.Api.Services;

public class NutritionSearchService : INutritionSearchService
{
    private readonly string _fileName;

    public NutritionSearchService(string fileName)
    {
        _fileName = fileName;
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
        var sortService = new NutritionSortService(); // TODO replace with DI 
        var sorted = sortService.SortFoods(unsorted, request.SortCriteria).Take(request.Limit).ToList();
        return sorted;
    }
}