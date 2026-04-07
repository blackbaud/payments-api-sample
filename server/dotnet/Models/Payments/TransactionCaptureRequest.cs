using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class TransactionCaptureRequest
{
    [JsonPropertyName("amount")]
    public int Amount { get; set; }

    [JsonPropertyName("card_token")]
    public string? CardToken { get; set; }

    [JsonPropertyName("payment_configuration_id")]
    [Required]
    public string? PaymentConfigurationId { get; set; }
}
