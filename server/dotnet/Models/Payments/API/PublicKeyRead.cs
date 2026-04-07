using System.Text.Json.Serialization;

namespace Blackbaud.PaymentsAPI.Sample.Backend.Models.Payments;

public class PublicKeyRead
{
    [JsonPropertyName("public_key")]
    public required string PublicKey { get; set; }
}
