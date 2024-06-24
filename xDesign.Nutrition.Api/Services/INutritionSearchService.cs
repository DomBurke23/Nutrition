using xDesign.Nutrition.Api.Dtos;
using xDesign.Nutrition.Api.Model;

namespace xDesign.Nutrition.Api.Services
{
    public interface INutritionSearchService
    {
        IEnumerable<Food> SearchNutrition(NutritionSearchRequest request);
    }
}
