using System.Text.Json.Serialization;

namespace Blackbaud.PaymentsAPITutorial.Models.Gifts;

public class CreateGiftRequest
{
    [JsonPropertyName("amount")]
    public Currency Amount { get; set; }

    [JsonPropertyName("constituent_id")]
    public string ConstituentId { get; set; }

    [JsonPropertyName("gift_splits")]
    public GiftSplitAdd[] GiftSplits { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("payments")]
    public PaymentAdd[] Payments { get; set; }

    [JsonPropertyName("is_manual")]
    public bool IsManual { get; set; }
}

public class PaymentAdd
{
    [JsonPropertyName("account_token")]
    public string AccountToken { get; set; }

    [JsonPropertyName("bbps_configuration_id")]
    public string BbpsConfigurationId { get; set; }

    [JsonPropertyName("bbps_transaction_id")]
    public string BbpsTransactionId { get; set; }

    [JsonPropertyName("checkout_transaction_id")]
    public string CheckoutTransactionId { get; set; }

    [JsonPropertyName("charge_transaction")]
    public bool ChargeTransaction { get; set; }

    [JsonPropertyName("payment_method")]
    public string PaymentMethod { get; set; }
}

public class GiftSplitAdd
{
    [JsonPropertyName("amount")]
    public Currency Amount { get; set; }

    [JsonPropertyName("fund_id")]
    public string FundId { get; set; }
}

public class Currency
{
    [JsonPropertyName("value")]
    public decimal Value { get; set; }
}
