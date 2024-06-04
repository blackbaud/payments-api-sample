using System.Text.Json.Serialization;

namespace Blackbaud.PaymentsAPITutorial.Models.Payments;

public class CheckoutTransactionRequest
{
    [JsonPropertyName("amount")]
    public int Amount { get; set; }

    [JsonPropertyName("authorization_token")]
    public string TransactionToken { get; set; }

    [JsonPropertyName("application_fee")]
    public int ApplicationFee { get; set; }
}
