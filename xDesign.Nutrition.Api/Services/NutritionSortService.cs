using xDesign.Nutrition.Api.Dtos;
using xDesign.Nutrition.Api.Model;

namespace xDesign.Nutrition.Api.Services
{
    public class NutritionSortService : INutritionSortService
    {
        public NutritionSortService()
        {
        }

        public IEnumerable<Food> SortFoods(IEnumerable<Food> unsorted, IList<Sort> requestSortCriteria)
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
    }
}
