using xDesign.Nutrition.Api.Dtos;
using xDesign.Nutrition.Api.Model;

namespace xDesign.Nutrition.Api.Services
{
    public interface INutritionSortService
    {
        IEnumerable<Food> SortFoods(IEnumerable<Food> unsorted, IList<Sort> requestSortCriteria);
    }
}
