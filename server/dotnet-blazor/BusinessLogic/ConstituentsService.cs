using System.Net.Http.Headers;
using Blackbaud.PaymentsAPITutorial.BusinessLogic.Interfaces;
using Blackbaud.PaymentsAPITutorial.Models;

namespace Blackbaud.PaymentsAPITutorial.BusinessLogic;

/// <summary>
/// Interacts directly with SKY API Constituent endpoints.
/// </summary>
public class ConstituentsService
{
    private readonly ISessionService _sessionService;
    private readonly IAuthenticationService _authService;
    private readonly IHttpClientFactory _httpClientFactory = null!;

    /// <summary>
    /// Constructor
    /// </summary>
    public ConstituentsService(
        ISessionService sessionService,
        IAuthenticationService authService,
        IHttpClientFactory httpClientFactory
    )
    {
        _sessionService = sessionService;
        _authService = authService;
        _httpClientFactory = httpClientFactory;
    }

    private async Task<HttpClient> GetClient(CancellationToken cancellationToken)
    {
        var httpClient = _httpClientFactory.CreateClient("ConstituentService");

        // check for and invalid access token
        var validRefresh = await _authService.HasValidRefreshToken();
        if (!_authService.IsAccessTokenValid() && validRefresh)
        {
            await _authService.RefreshAccessToken(cancellationToken);
        }
        var token = _sessionService.GetAccessToken();

        if (string.IsNullOrEmpty(token))
        {
            throw new UnauthorizedAccessException();
        }

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer",
            token
        );

        return httpClient;
    }

    /// <summary>
    /// Returns a response containing a constituent record (from an ID).
    /// </summary>
    public async Task<ConstituentModel> GetConstituentAsync(
        string id,
        CancellationToken cancellationToken
    )
    {
        var httpClient = await GetClient(cancellationToken);
        var response = await httpClient.GetAsync($"constituents/{id}", cancellationToken);
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null!;
        }

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedAccessException();
        }

        response.EnsureSuccessStatusCode();

        var model = await response
            .Content
            .ReadFromJsonAsync<ConstituentModel>(cancellationToken: cancellationToken);

        return model;
    }

    /// <summary>
    /// Returns a response containing a paginated list of constituents.
    /// </summary>
    public async Task<IEnumerable<ConstituentModel>> GetConstituentsAsync(
        CancellationToken cancellationToken
    )
    {
        var httpClient = await GetClient(cancellationToken);

        var response = await httpClient.GetAsync($"constituents", cancellationToken);

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedAccessException();
        }

        response.EnsureSuccessStatusCode();

        var models = await response
            .Content
            .ReadFromJsonAsync<IEnumerable<ConstituentModel>>(cancellationToken: cancellationToken);

        return models;
    }
}
