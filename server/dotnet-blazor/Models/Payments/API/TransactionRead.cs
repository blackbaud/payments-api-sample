using System.Text.Json.Serialization;

namespace Blackbaud.PaymentsAPITutorial.Models.Payments;

public class TransactionRead
{
    [JsonPropertyName("amount")]
    public int Amount { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("token")]
    public string Token { get; set; }
}
