using System.Text.Json.Serialization;

namespace Blackbaud.PaymentsAPITutorial.Models.Payments;

public class PublicKeyRead
{
    [JsonPropertyName("public_key")]
    public string PublicKey { get; set; }
}
