using System.Text.Json.Serialization;

public class UpdateObjectApiResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("data")]
    public ApiData Data { get; set; }

    public class ApiData
    {
        [JsonPropertyName("year")]
        public int Year { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("CPU_model")]
        public string CPU_model { get; set; }

        [JsonPropertyName("Hard_disk_size")]
        public string Hard_disk_size { get; set; }

        [JsonPropertyName("color")]
        public string Color { get; set; }
    }
}