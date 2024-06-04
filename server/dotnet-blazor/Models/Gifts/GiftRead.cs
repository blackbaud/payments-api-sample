using System.Text.Json.Serialization;

namespace Blackbaud.PaymentsAPITutorial.Models.Gifts;

public class GiftRead
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
}
