# Nutrition Data

-- Doms Notes -- 
all contract tests pass 
![contractTestsPass](https://github.com/DomBurke23/Nutrition/assets/22835921/5d7f98b0-41ed-4c8c-a809-ac942498bde5)
and you can still run the code locally and are able to update the url https://localhost:7248/nutrition?fatRating=LOW depending on the nutrition criteria (see tests for examples). 


A simple web service which supports searching and sorting nutrition data.  Query results are returned as a list of foods and drinks, formatted as a JSON document.

Data comes from https://www.kaggle.com/datasets/trolukovich/nutritional-values-for-common-foods-and-products?resource=download
and is available under the [CCO: Public Domain](https://creativecommons.org/publicdomain/zero/1.0/) license.


## Querying the data
A number of query parameters are available to customize the search criteria. All query parameters are optional and
may be applied in any order. The full list of query parameters is:

* `fatRating (Low | Medium | High)` - filter by fat rating.  When this parameter is omitted, all entries are returned.  Categorization based on https://www.heartuk.org.uk/low-cholesterol-foods/saturated-fat
* `minCalories` - Items will match if they are greater than or equal to the minimum calories.
* `maxCalories` - Items will match if they are less than or equal to the maximum calories.
* `limit` - Limit the number of items returned.  Must be a positive integer.
* `sort` - Items may be sorted by `name` or `calories` or both. The sort parameter is specified as the name of the field,
  followed by an underscore (`_`) and then the sort order (`asc` or `desc`), e.g. `calories_desc`. To sort by both name
  and calories, specify two sort parameters in the query string, one for each field. The order of the fields
  determines the final order of the items, e.g. if sorting by calories descending and name ascending, the second sort
  parameter is only used when the calories are equal.

Test Change
## Notes
* You will need .NET 7 to compile the project.
* Tests are split into two files:
   -   xDesign.Nutrition.Tests/ControllerTests/
       - NutritionControllerFilterTests.cs should pass on completion of step 1
       - NutritionControllerAdditionalTests.cs are the target of task 4
* The test code and test fixtures (JSON files) contain no errors.
* There are no errors in the CSV file of nutrition data.
* Please contact [talent.team@xdesign.com](mailto:talent.team@xdesign.com) if you have any questions, and we'll respond within normal business hours (Mon-Fri, 09:00 - 17:00).

