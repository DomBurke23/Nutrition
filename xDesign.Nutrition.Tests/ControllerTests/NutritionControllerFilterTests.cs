using System.Net;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc.Testing;
using FluentAssertions;

namespace xDesign.Nutrition.Tests.ControllerTests;

public class NutritionControllerFilterTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public NutritionControllerFilterTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Should_ReturnAllItems()
    {
        var response = await _client.GetAsync("/nutrition");
        var responseDeserialised = await Helper.DeserialiseResponse(response);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);
        responseDeserialised.Should().BeEquivalentTo(Helper.ReadJsonFile("all-foods.json"));
    }
    
    [Fact]
    public async Task Should_ReturnOnlyLowFatItems()
    {
        var response = await _client.GetAsync("/nutrition?fatRating=LOW");
        var responseDeserialised = await Helper.DeserialiseResponse(response);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);
        responseDeserialised.Should().BeEquivalentTo(Helper.ReadJsonFile("all-low-fat-foods.json"));
    }
    
    [Fact]
    public async Task Should_ReturnOnlyLowFatItems_With300CaloriesOrLess()
    {
        var response = await _client.GetAsync("/nutrition?fatRating=LOW&maxCalories=300");
        var responseDeserialised = await Helper.DeserialiseResponse(response);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);
        responseDeserialised.Should().BeEquivalentTo(Helper.ReadJsonFile("low-fat-under-300-cals.json"));
    }

    [Fact]
    public async Task Should_ReturnOnlyHighFatItems_With300CaloriesOrMore()
    {
        var response = await _client.GetAsync("/nutrition?fatRating=HIGH&minCalories=300");
        var responseDeserialised = await Helper.DeserialiseResponse(response);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);
        responseDeserialised.Should().BeEquivalentTo(Helper.ReadJsonFile("high-fat-over-300-cals.json"));
    }

    [Fact]
    public async Task Should_ReturnItemsWithExactly300Calories()
    {
        var response = await _client.GetAsync("/nutrition?minCalories=300&maxCalories=300");
        var responseDeserialised = await Helper.DeserialiseResponse(response);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);
        responseDeserialised.Should().BeEquivalentTo(Helper.ReadJsonFile("all-foods-exactly-300-cals.json"));
    }
}