namespace Blackbaud.PaymentsAPI.Sample.Backend.DataAccess.Models;

public class AuthenticationData
{
    public string? RefreshToken { get; set; }
    public DateTimeOffset? AccessExpires { get; set; }
    public DateTimeOffset? RefreshExpires { get; set; }
}
