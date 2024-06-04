using System.Text.Json.Serialization;

namespace Blackbaud.PaymentsAPITutorial.Models.Payments;

public class PaymentConfigurationRead
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}
