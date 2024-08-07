using System.Text.Json.Serialization;

public class DeleteResponse
{
    [JsonPropertyName("message")]
    public string Message { get; set; }
}