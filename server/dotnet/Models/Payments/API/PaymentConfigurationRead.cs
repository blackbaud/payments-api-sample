using System.Text.Json.Serialization;

namespace Blackbaud.PaymentsAPI.Sample.Backend.Models.Payments;

public class PaymentConfigurationRead
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}
