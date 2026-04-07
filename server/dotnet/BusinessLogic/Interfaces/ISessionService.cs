using Blackbaud.PaymentsAPI.Sample.Backend.Models;

namespace Blackbaud.PaymentsAPI.Sample.Backend.BusinessLogic.Interfaces;

public interface ISessionService
{
    void SetTokens(RefreshTokenResponseModel response);
    Task ClearTokens();
    string GetAccessToken();
    string GetRefreshToken();
    DateTimeOffset? GetRefreshExpires();
    DateTimeOffset? GetAccessExpires();
    string GetStateVerifier(string state);
    void SetStateVerifier(string state, string verifier);
    void ClearStateVerifier(string state);
}
