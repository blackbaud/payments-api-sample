using System.Text.Json.Serialization;

public class TransactionCaptureRequest
{
    [JsonPropertyName("transaction_token")]
    public string TransactionToken { get; set; }
    [JsonPropertyName("amount")]
    public int Amount { get; set; }
    [JsonPropertyName("card_token")]
    public string CardToken { get; set; }
    [JsonPropertyName("payment_configuration_id")]
    public string PaymentConfigurationId { get; set; }
}
