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

    public IEnumerable<Food> SearchNutrition(NutritionSearchRequest request)
    {
        string extension = Path.GetExtension(_fileName).ToLower();
        var unsorted = new List<Food>();
        var dataLoaderService = new DataLoaderService();
        switch (extension)
        {
            case ".csv":
                // TODO replace with DI instead of creating instance
                unsorted = dataLoaderService.LoadFoodsFromCsvFile(_fileName)
                .Where(food => request.FatRating == null || food.FatRating == request.FatRating)
                .Where(food => request.MinCalories == null || food.Calories >= request.MinCalories.Value)
                .Where(food => request.MaxCalories == null || food.Calories <= request.MaxCalories.Value)
                .ToList();
                break;
            case ".xml":
                // TODO replace with DI instead of creating instance
                unsorted = dataLoaderService.LoadFoodsFromXmlFile(_fileName)
                .Where(food => request.FatRating == null || food.FatRating == request.FatRating)
                .Where(food => request.MinCalories == null || food.Calories >= request.MinCalories.Value)
                .Where(food => request.MaxCalories == null || food.Calories <= request.MaxCalories.Value)
                .ToList();
                break;
            case ".json":
                // TODO replace with DI instead of creating instance
                unsorted = dataLoaderService.LoadFoodsFromJsonFile(_fileName)
                .Where(food => request.FatRating == null || food.FatRating == request.FatRating)
                .Where(food => request.MinCalories == null || food.Calories >= request.MinCalories.Value)
                .Where(food => request.MaxCalories == null || food.Calories <= request.MaxCalories.Value)
                .ToList();
                break;
            default:
                Console.WriteLine("Unsupported file type.");
                throw new NotSupportedException($"Unsupported file type: {extension}");
        }

        var sortService = new NutritionSortService(); // TODO replace with DI 
        var sorted = sortService.SortFoods(unsorted, request.SortCriteria).Take(request.Limit).ToList();
        return sorted;
    }
}