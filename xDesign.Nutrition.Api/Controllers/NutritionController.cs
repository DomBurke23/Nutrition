using System.Net.Mime;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using xDesign.Nutrition.Api.Dtos;
using xDesign.Nutrition.Api.Model;
using xDesign.Nutrition.Api.Services;

namespace xDesign.Nutrition.Api.Controllers;

[ApiController]
[Route("nutrition")]
public class NutritionController : ControllerBase
{
    private readonly NutritionSearchService _service;

    public NutritionController(NutritionSearchService service)
    {
        _service = service;
    }

    [HttpGet]
    [Produces(MediaTypeNames.Application.Json)]
    public ActionResult<IEnumerable<Food>> SearchFoods(
        [FromQuery] int? minCalories,
        [FromQuery] int? maxCalories,
        [FromQuery] FatRating? fatRating,
        [FromQuery(Name = "sort")] IList<string> sortCriteria,
        [FromQuery] int limit = 1000)
    {
        try {
            var request = new NutritionSearchRequest(
                minCalories,
                maxCalories,
                fatRating,
                sortCriteria,
                limit);

            var results = _service.SearchNutrition(request);
        
            return Ok(results);
        }
        catch (ValidationException e)
        {
            return BadRequest(e.ValidationResult);
        }
    }
}