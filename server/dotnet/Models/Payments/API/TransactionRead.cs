using System.Text.Json.Serialization;

namespace Blackbaud.PaymentsAPI.Sample.Backend.Models.Payments;

public class TransactionRead
{
    [JsonPropertyName("amount")]
    public required int Amount { get; set; }

    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("token")]
    public string? Token { get; set; }
}
