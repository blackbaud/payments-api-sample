using System.Text.Json.Serialization;

namespace Blackbaud.PaymentsAPITutorial.Models.Payments;

public class SecurityTokenRead
{
    [JsonPropertyName("security_token")]
    public string SecurityToken { get; set; }

    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }
}
