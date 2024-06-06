using System.Text.Json.Serialization;

namespace Blackbaud.PaymentsAPITutorial.Models.Payments;

public class CreateTransactionRequest
{
    [JsonPropertyName("amount")]
    public int Amount { get; set; }

    [JsonPropertyName("card_token")]
    public string CardToken { get; set; }

    [JsonPropertyName("payment_configuration_id")]
    public string PaymentConfigurationId { get; set; }

    [JsonPropertyName("application_fee")]
    public int ApplicationFee { get; set; }

    [JsonPropertyName("transaction_type")]
    public string TransactionType { get; set; }

    [JsonPropertyName("is_backoffice")]
    public bool IsBackoffice { get; set; }
}
