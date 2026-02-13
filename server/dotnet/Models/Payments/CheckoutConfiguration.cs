using System.Text.Json.Serialization;

namespace Blackbaud.PaymentsAPI.Sample.Backend.Models.Payments;

public class CheckoutConfiguration
{
    [JsonPropertyName("key")]
    public string Key { get; set; }

    [JsonPropertyName("payment_configuration_id")]
    public string PaymentConfigurationId { get; set; }

    [JsonPropertyName("environment_id")]
    public string? EnvironmentId { get; set; }
}
