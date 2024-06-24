using System.Text.Json;

namespace xDesign.Nutrition.Api.Util;

public class UpperCaseJsonNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name)
    {
        if (name == null) throw new ArgumentNullException(nameof(name));
        return name.ToUpper();
    }
}