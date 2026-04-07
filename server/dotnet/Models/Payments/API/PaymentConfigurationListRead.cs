using System.Text.Json.Serialization;

namespace Blackbaud.PaymentsAPI.Sample.Backend.Models.Payments;

public class PaymentConfigurationListRead
{
    [JsonPropertyName("count")]
    public required int Count { get; set; }

    [JsonPropertyName("value")]
    public required ICollection<PaymentConfigurationRead> Value { get; set; }
}
