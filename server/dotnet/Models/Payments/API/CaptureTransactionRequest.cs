using System.Text.Json.Serialization;

namespace Blackbaud.PaymentsAPI.Sample.Backend.Models.Payments;

public class CaptureTransactionRequest
{
    [JsonPropertyName("amount")]
    public int Amount { get; set; }
}
