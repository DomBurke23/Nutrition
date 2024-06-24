using xDesign.Nutrition.Api.Model;

namespace xDesign.Nutrition.Api.Services
{
    public interface ICsvLoaderService
    {
        IEnumerable<Food> LoadFoodsFromCsvFile(string csvFileName);
    }
}
