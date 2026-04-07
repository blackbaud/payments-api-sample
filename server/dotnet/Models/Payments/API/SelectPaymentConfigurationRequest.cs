using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Blackbaud.PaymentsAPI.Sample.Backend.Models.Payments;

public class SelectPaymentConfigurationRequest
{
    [JsonPropertyName("payment_configuration_id")]
    [Required]
    public required string PaymentConfigurationId { get; set; }
}
