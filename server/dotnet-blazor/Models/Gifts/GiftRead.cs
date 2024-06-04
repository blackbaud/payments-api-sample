using System.Text.Json.Serialization;

namespace Blackbaud.PaymentsAPITutorial.Models.Gifts;

public class GiftRead
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("amount")]
    public Currency Amount { get; set; } = null!;

    [JsonPropertyName("payments")]
    public PaymentRead[] Payments { get; set; }
}

public class PaymentRead
{
    [JsonPropertyName("account_token")]
    public string AccountToken { get; set; }

    [JsonPropertyName("bbps_configuration_id")]
    public string BbpsConfigurationId { get; set; }

    [JsonPropertyName("bbps_transaction_id")]
    public string BbpsTransactionId { get; set; }

    [JsonPropertyName("checkout_transaction_id")]
    public string CheckoutTransactionId { get; set; }

    [JsonPropertyName("payment_method")]
    public string PaymentMethod { get; set; }
}
