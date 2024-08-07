using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using System.Text;
using System.Net;
using Xunit.Sdk;

public class ApiTests
{
    private readonly HttpClient _client;
    private static string sharedValue;

    public ApiTests()
    {
        _client = new HttpClient();
    }

    [Fact]
    public async Task Test1_ShouldBeAbleToGetAllObjects()
    {
        var requestUrl = "https://api.restful-api.dev/objects";
        var response = await _client.GetAsync(requestUrl);
        var responseContent = await response.Content.ReadAsStringAsync();

        List<GetAllObjectsApiResponse> apiResponses = null;
        try
        {
            apiResponses = JsonSerializer.Deserialize<List<GetAllObjectsApiResponse>>(responseContent);
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"JSON Deserialization Error: {ex.Message}");
        }

        // Assertions
        // Check the status of the response 
        response.IsSuccessStatusCode.Should().BeTrue("because the request should succeed");

        // Check number of objects 
        apiResponses.Count.Should().Be(13, $"because the API should return 13 objects");

        // Check data of the first object
        var firstResponse = apiResponses[0];
        firstResponse.Id.Should().Be("1");
        firstResponse.Name.Should().Be("Google Pixel 6 Pro");
        firstResponse.Data.Should().NotBeNull();
        firstResponse.Data.Color.Should().Be("Cloudy White");
        firstResponse.Data.Capacity.Should().Be("128 GB");

        // Check data of the second object
        var secondResponse = apiResponses[1];
        secondResponse.Id.Should().Be("2");
        secondResponse.Name.Should().Be("Apple iPhone 12 Mini, 256GB, Blue");
        secondResponse.Data.Should().BeNull();
    }

    [Fact]
    public async Task Test2_ShouldBeAbleToAddAnObject()
    {
        var requestUrl = "https://api.restful-api.dev/objects";
        var requestData = new
        {
            name = "Apple MacBook Pro 16",
            data = new
            {
                year = 2019,
                price = 1849.99,
                CPU_model = "Intel Core i9",
                Hard_disk_size = "1 TB"
            }
        };

        var json = JsonSerializer.Serialize(requestData);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _client.PostAsync(requestUrl, content);

        var responseContent = await response.Content.ReadAsStringAsync();

        AddObjectApiResponse responseObject = null;
        try
        {
            responseObject = JsonSerializer.Deserialize<AddObjectApiResponse>(responseContent);
            
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"JSON Deserialization Error: {ex.Message}");
            Assert.Fail();
        }

        sharedValue = responseObject.Id;

        // Assertions
        response.StatusCode.Should().Be(HttpStatusCode.OK, "because the POST request should succeed");

        // Assert the deserialized response object
        responseObject.Name.Should().Be("Apple MacBook Pro 16");
        responseObject.Data.Should().NotBeNull();
        responseObject.Data.Year.Should().Be(2019);
        responseObject.Data.Price.Should().Be(1849.99M);
        responseObject.Data.CPU_model.Should().Be("Intel Core i9");
        responseObject.Data.Hard_disk_size.Should().Be("1 TB");
        responseObject.CreatedAt.Should().NotBe(default(DateTime));
    }

    [Fact]
    public async Task Test3_ShouldBeAbleToGetAnObject()
    {
        var objectId = sharedValue; // ID of the object you want to fetch
        
        Console.WriteLine(sharedValue);
        var requestUrl = $"https://api.restful-api.dev/objects/{objectId}";

        var response = await _client.GetAsync(requestUrl);

        var responseContent = await response.Content.ReadAsStringAsync();

        GetSingleObjectApiResponse responseObject = null;
        try
        {
            responseObject = JsonSerializer.Deserialize<GetSingleObjectApiResponse>(responseContent);
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"JSON Deserialization Error: {ex.Message}");
            Assert.Fail();
        }



        // Assertions
        response.StatusCode.Should().Be(HttpStatusCode.OK, "because the GET request should succeed");

        // Assert the deserialized response object
        responseObject.Should().NotBeNull();
        responseObject.Id.Should().Be(sharedValue);
        responseObject.Name.Should().Be("Apple MacBook Pro 16");
        responseObject.Data.Should().NotBeNull();
        responseObject.Data.Year.Should().Be(2019);
        responseObject.Data.Price.Should().Be(1849.99M);
        responseObject.Data.CPU_model.Should().Be("Intel Core i9");
        responseObject.Data.Hard_disk_size.Should().Be("1 TB");
    }

    [Fact]
    public async Task Test4_ShouldBeAbleToUpdateAnObject()
    {
        var objectId = "7";
        var requestUrl = $"https://api.restful-api.dev/objects/{objectId}";
        var requestData = new
        {
            name = "Apple MacBook Pro 16",
            data = new
            {
                year = 2019,
                price = 2049.99,
                CPU_model = "Intel Core i9",
                Hard_disk_size = "1 TB",
                color = "silver"
            }
        };

        var json = JsonSerializer.Serialize(requestData);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _client.PutAsync(requestUrl, content);

        var responseContent = await response.Content.ReadAsStringAsync();

        UpdateObjectApiResponse responseObject = null;
        try
        {
            responseObject = JsonSerializer.Deserialize<UpdateObjectApiResponse>(responseContent);
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"JSON Deserialization Error: {ex.Message}");
            Assert.Fail();
        }

        // Assertions
        response.StatusCode.Should().Be(HttpStatusCode.OK, "because the PUT request should succeed");

        // Assert the deserialized response object
        responseObject.Should().NotBeNull();
        responseObject.Id.Should().Be(objectId);
        responseObject.Name.Should().Be("Apple MacBook Pro 16");
        responseObject.Data.Should().NotBeNull();
        responseObject.Data.Year.Should().Be(2019);
        responseObject.Data.Price.Should().Be(2049.99M);
        responseObject.Data.CPU_model.Should().Be("Intel Core i9");
        responseObject.Data.Hard_disk_size.Should().Be("1 TB");
        responseObject.Data.Color.Should().Be("silver");
    }

    [Fact]
    public async Task Test5_ShouldBeAbleToDeleteAnObject()
    {
        var objectId = "6"; // ID of the object you want to delete
        var requestUrl = $"https://api.restful-api.dev/objects/{objectId}";

        var response = await _client.DeleteAsync(requestUrl);

        var responseContent = await response.Content.ReadAsStringAsync();

        DeleteResponse deleteResponse = null;
        try
        {
            deleteResponse = JsonSerializer.Deserialize<DeleteResponse>(responseContent);
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"JSON Deserialization Error: {ex.Message}");
        }



        // Assertions
        response.StatusCode.Should().Be(HttpStatusCode.OK, "because the DELETE request should succeed");

        // Assert the deserialized response object
        deleteResponse.Should().NotBeNull();
        deleteResponse.Message.Should().Be($"Object with id = {objectId}, has been deleted.");
    }

    // Note : TC #4 & TC #5 are failing because the status code is 405
}
