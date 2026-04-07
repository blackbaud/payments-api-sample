using Blackbaud.PaymentsAPI.Sample.Backend.Models;

namespace Blackbaud.PaymentsAPI.Sample.Backend.BusinessLogic.Interfaces;

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
    Task LogOut();
    Task<RefreshTokenResponseModel> RefreshAccessToken(CancellationToken cancellationToken);
}
