using System.Text.Json.Serialization;

namespace Blackbaud.PaymentsAPI.Sample.Backend.Models.Gifts;

public class PostResponse
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }
}
