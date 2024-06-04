using System.Text.Json.Serialization;

namespace Blackbaud.PaymentsAPITutorial.Models.Gifts;

public class PostResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
}
