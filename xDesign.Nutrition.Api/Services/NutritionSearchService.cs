using xDesign.Nutrition.Api.Dtos;
using xDesign.Nutrition.Api.Model;

namespace xDesign.Nutrition.Api.Services;

public class NutritionSearchService : INutritionSearchService
{
    private readonly string _fileName;
    private readonly IDataLoaderService _dataLoaderService;
    private readonly INutritionSortService _sortService;

    public NutritionSearchService(string fileName,
        INutritionSortService sortService,
        IDataLoaderService dataLoaderService)
    {
        _fileName = fileName;
        _sortService = sortService;
        _dataLoaderService = dataLoaderService;
    }

    private IEnumerable<Food> LoadFoods(string extension)
    {
        Func<string, IEnumerable<Food>> loadFunction = extension switch
        {
            ".csv" => _dataLoaderService.LoadFoodsFromCsvFile,
            ".xml" => _dataLoaderService.LoadFoodsFromXmlFile,
            ".json" => _dataLoaderService.LoadFoodsFromJsonFile,
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