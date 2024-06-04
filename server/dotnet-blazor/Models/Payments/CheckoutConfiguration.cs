using System.Text.Json.Serialization;

namespace Blackbaud.PaymentsAPITutorial.Models.Payments;

public class CheckoutConfiguration
{
    [JsonPropertyName("key")]
    public string Key { get; set; }

    [JsonPropertyName("payment_configuration_id")]
    public string PaymentConfigurationId { get; set; }
}
