using xDesign.Nutrition.Api.Dtos;
using xDesign.Nutrition.Api.Model;

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
        // TODO replace with DI instead of creating instance
        var csvLoaderService = new CsvLoaderService();
        var unsorted = csvLoaderService.LoadFoodsFromCsvFile(_csvFileName)
        .Where(food => request.FatRating == null || food.FatRating == request.FatRating)
        .Where(food => request.MinCalories == null || food.Calories >= request.MinCalories.Value)
        .Where(food => request.MaxCalories == null || food.Calories <= request.MaxCalories.Value)
        .ToList();

        var sortService = new NutritionSortService(); // TODO replace with DI 
        var sorted = sortService.SortFoods(unsorted, request.SortCriteria).Take(request.Limit).ToList();
        return sorted;
    }
}