using Blackbaud.PaymentsAPITutorial.Models;

namespace Blackbaud.PaymentsAPITutorial.BusinessLogic.Interfaces;

public interface ISessionService
{
    void SetTokens(RefreshTokenResponseModel response);
    void ClearTokens();
    string GetAccessToken();
    string GetRefreshToken();
    DateTimeOffset? GetRefreshExpires();
    DateTimeOffset? GetAccessExpires();
    string GetStateVerifier(string state);
    void SetStateVerifier(string state, string verifier);
    void ClearStateVerifier(string state);
}
