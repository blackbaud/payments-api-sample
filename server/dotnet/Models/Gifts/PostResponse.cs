using System.Text.Json.Serialization;

namespace Blackbaud.PaymentsAPI.Sample.Backend.Models.Gifts;

public class PostResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
}
