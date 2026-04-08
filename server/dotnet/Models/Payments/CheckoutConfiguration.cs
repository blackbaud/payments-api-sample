using System.Text.Json.Serialization;

namespace Blackbaud.PaymentsAPI.Sample.Backend.Models.Payments;

public class CheckoutConfiguration
{
    [JsonPropertyName("payment_configuration_id")]
    public string? PaymentConfigurationId { get; set; }
}
