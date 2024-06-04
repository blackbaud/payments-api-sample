namespace Blackbaud.PaymentsAPITutorial.DataAccess.Models;

public class AuthenticationData
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTimeOffset? AccessExpires { get; set; }
    public DateTimeOffset? RefreshExpires { get; set; }
}
