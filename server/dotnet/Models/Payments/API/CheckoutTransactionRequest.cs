using System.Text.Json.Serialization;

namespace Blackbaud.PaymentsAPI.Sample.Backend.Models.Payments;

public class CheckoutTransactionRequest
{
    [JsonPropertyName("amount")]
    public int Amount { get; set; }

    [JsonPropertyName("authorization_token")]
    public required string TransactionToken { get; set; }

    [JsonPropertyName("application_fee")]
    public int ApplicationFee { get; set; }
}
