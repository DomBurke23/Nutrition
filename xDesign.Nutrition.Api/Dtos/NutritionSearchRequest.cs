using System.ComponentModel.DataAnnotations;
using xDesign.Nutrition.Api.Model;

namespace xDesign.Nutrition.Api.Dtos;

public record NutritionSearchRequest
{
    public int? MinCalories { get; private set; }
    public int? MaxCalories { get; private set; }
    public FatRating? FatRating { get; private set; }
    public IList<Sort> SortCriteria { get; private set; }
    public int Limit { get; private set; }
    
    public NutritionSearchRequest(
        int? minCalories,
        int? maxCalories,
        FatRating? fatRating,
        IList<string> sortCriteria,
        int limit)
    {
        if (limit <= 0)
        {
            throw new ValidationException($"Limit must be greater than zero; found  {limit}");
        }
        
        ValidateCaloriesBracket(minCalories, maxCalories);
        
        MinCalories = minCalories;
        MaxCalories = maxCalories;
        FatRating = fatRating;
        SortCriteria = ParseSortCriteria(sortCriteria);
        Limit = limit;
    }

    private static void ValidateCaloriesBracket(int? minCalories, int? maxCalories)
    {
        if (minCalories is < 0) {
            throw new ValidationException($"MinCalories must be greater than zero; found {minCalories}");
        }

        if (maxCalories is < 0)
        {
            throw new ValidationException($"MaxCalories must be greater than zero; found {maxCalories}");
        }

        if (minCalories == null || maxCalories == null)
        {
            return;
        }
        
        if (minCalories > maxCalories)
        {
            throw new ValidationException("MinCalories must be less than or equal to MaxCalories");
        }
    }
    
    private static IList<Sort> ParseSortCriteria(IList<string> sortCriteria)
    {
        var sorts = sortCriteria
            .Select(ParseSort)
            .ToList();

        var duplicateSortFields = FindDuplicateSortFields(sorts).ToList();
        if (duplicateSortFields.Any())
        {
            throw new ValidationException($"Duplicate sort criteria found for fields named {string.Join(", ", duplicateSortFields)}");
        }

        return sorts;
    }

    private static Sort ParseSort(string criterion) 
    {
        if (!criterion.Contains('_'))
        {
            throw new ValidationException($"Sort parameter format is 'fieldName_order'. Found {criterion}");
        }

        var parts = criterion.Split("_");
        if (!Enum.TryParse<SortField>(parts[0], true, out var sortField))
        {
            throw new ValidationException($"{parts[0]} is not a sortable field");
        }

        if (!Enum.TryParse<SortOrder>(parts[1], true, out var sortOrder))
        {
            throw new ValidationException($"Unknown sort order {parts[1]}", null, null);
        }

        return new Sort(sortField, sortOrder);
    }

    private static IList<SortField> FindDuplicateSortFields(IList<Sort> sorts)
    {
        return sorts.GroupBy(s => s.SortField)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();
    }
}