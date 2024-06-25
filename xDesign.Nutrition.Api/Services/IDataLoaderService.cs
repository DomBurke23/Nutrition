using xDesign.Nutrition.Api.Model;

namespace xDesign.Nutrition.Api.Services
{
    public interface IDataLoaderService
    {
        IEnumerable<Food> LoadFoodsFromCsvFile(string csvFileName);
        IEnumerable<Food> LoadFoodsFromJsonFile(string csvFileName);
        IEnumerable<Food> LoadFoodsFromXmlFile(string csvFileName);
    }
}
