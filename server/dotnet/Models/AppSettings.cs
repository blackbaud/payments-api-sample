namespace Blackbaud.PaymentsAPI.Sample.Backend.Models;

/// <summary>
/// Stores app-wide configuration properties, mapped to appsettings.json.
/// </summary>
public class AppSettings
{
    public required string AuthBaseUri { get; set; }
    public required string AuthClientId { get; set; }
    public required string AuthClientSecret { get; set; }
    public required string AuthRedirectUri { get; set; }
    public string? GeneralSubscriptionKey { get; set; }
    public string? PaymentsSubscriptionKey { get; set; }
    public string? SkyApiBaseUri { get; set; }
}
