using System.Net;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc.Testing;
using FluentAssertions;

namespace xDesign.Nutrition.Tests.ControllerTests;

public class NutritionControllerAdditionalTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public NutritionControllerAdditionalTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Should_ReturnFirst5HighFatItems_OrderedByCaloriesDesc()
    {
        var response = await _client.GetAsync("/nutrition?fatRating=HIGH&sort=calories_desc&limit=5");
        var responseDeserialised = await Helper.DeserialiseResponse(response);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);
        responseDeserialised.Should().BeEquivalentTo(Helper.ReadJsonFile("high-fat-sort-by-cals-desc.json"));
    }
    
    [Fact]
    public async Task Should_ReturnFirst5LowFatItems_OrderedByCaloriesDesc_WithIdenticalCalorieItemsOrderedAlphabetically()
    {
        var response = await _client.GetAsync("/nutrition?fatRating=LOW&sort=calories_desc&sort=name_asc&limit=5");
        var responseDeserialised = await Helper.DeserialiseResponse(response);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);
        responseDeserialised.Should().BeEquivalentTo(Helper.ReadJsonFile("first-five-low-fat-ordered-by-calories-desc-then-name-asc.json"));
    }
    
    [Fact]
    public async Task Should_ReturnLowFatItems_With400CaloriesOrMore_OrderedByCaloriesDesc()
    {
        var response = await _client.GetAsync("/nutrition?fatRating=LOW&minCalories=400&sort=calories_desc");
        var responseDeserialised = await Helper.DeserialiseResponse(response);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);
        responseDeserialised.Should().BeEquivalentTo(Helper.ReadJsonFile("low-fat-400-cals-or-more-sort-by-cals-desc.json"));
    }
    
    [Fact]
    public async Task Should_ReturnHighFatItems_With200CaloriesOrLess_OrderedByCaloriesAsc()
    {
        var response = await _client.GetAsync("/nutrition?fatRating=HIGH&maxCalories=200&sort=calories_asc");
        var responseDeserialised = await Helper.DeserialiseResponse(response);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);
        responseDeserialised.Should().BeEquivalentTo(Helper.ReadJsonFile("high-fat-max-cals-200-sort-by-cals-asc.json"));
    }
}