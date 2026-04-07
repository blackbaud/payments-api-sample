using System.Text.Json.Serialization;

namespace Blackbaud.PaymentsAPI.Sample.Backend.Models.Payments;

public class SecurityTokenRead
{
    [JsonPropertyName("security_token")]
    public required string SecurityToken { get; set; }

    [JsonPropertyName("expires_in")]
    public required int ExpiresIn { get; set; }
}
