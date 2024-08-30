using Blackbaud.PaymentsAPITutorial.Models;

namespace Blackbaud.PaymentsAPITutorial.BusinessLogic.Interfaces;

public interface IAuthenticationService
{
    Task<RefreshTokenResponseModel> ExchangeCodeForAccessToken(
        string code,
        string state,
        CancellationToken cancellationToken
    );
    Uri GetAuthorizationUri();
    Task<bool> IsAuthenticated();
    bool IsAccessTokenValid();
    Task<bool> HasValidRefreshToken();
    void LogOut();
    Task<RefreshTokenResponseModel> RefreshAccessToken(CancellationToken cancellationToken);
}
