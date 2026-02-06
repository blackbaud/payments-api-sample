using System.Text.Json.Serialization;

namespace Blackbaud.PaymentsAPITutorial.Models.Payments;

public class CaptureTransactionRequest
{
    [JsonPropertyName("amount")]
    public int Amount { get; set; }
}
