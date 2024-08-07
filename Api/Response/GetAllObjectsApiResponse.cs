using System.Text.Json.Serialization;

public class GetAllObjectsApiResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("data")]
    public ApiData Data { get; set; }

    public class ApiData
    {
        [JsonPropertyName("color")]
        public string Color { get; set; }

        [JsonPropertyName("capacity")]
        public string Capacity { get; set; }
    }
}
