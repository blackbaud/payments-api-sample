using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Blackbaud.PaymentsAPITutorial.BusinessLogic.Interfaces;
using Blackbaud.PaymentsAPITutorial.DataAccess;
using Blackbaud.PaymentsAPITutorial.Models.Gifts;
using Blackbaud.PaymentsAPITutorial.Models.Payments;

namespace Blackbaud.PaymentsAPITutorial.BusinessLogic;

/// <summary>
/// Interacts directly with SKY API Payments endpoints.
/// </summary>
public class GiftsService
{
    private readonly ISessionService _sessionService;
    private readonly IAuthenticationService _authService;
    private readonly IHttpClientFactory _httpClientFactory = null!;

    /// <summary>
    /// Constructor
    /// </summary>
    public GiftsService(
        ISessionService sessionService,
        IAuthenticationService authService,
        IHttpClientFactory httpClientFactory
    )
    {
        _sessionService = sessionService;
        _authService = authService;
        _httpClientFactory = httpClientFactory;
    }

    /// <summary>
    /// Create a gift in RE NXT
    /// </summary>
    public async Task<GiftRead> CreateGift(
        TransactionRead transaction,
        string paymentConfigId,
        string cardToken,
        bool anonymous,
        CancellationToken cancellationToken,
        string checkoutTransactionId = null
    )
    {
        var httpClient = await GetClient(cancellationToken, anonymous);

        // Convert the amount from cents to dollars
        var amount = (decimal)transaction.Amount / 100;

        // Build payment add
        var paymentAdd = new PaymentAdd
        {
            // One of either AccountToken or CheckoutTransactionId are required
            // for payment method details to display on the gift record
            AccountToken = cardToken,
            CheckoutTransactionId = checkoutTransactionId,
            BbpsTransactionId = transaction.Id,
            BbpsConfigurationId = paymentConfigId,
            PaymentMethod = "CreditCard"
        };

        var createGiftRequest = new CreateGiftRequest
        {
            Amount = new Currency { Value = amount },
            ConstituentId = "280",
            GiftSplits = new GiftSplitAdd[]
            {
                new GiftSplitAdd
                {
                    Amount = new Currency { Value = amount },
                    FundId = "18"
                }
            },
            Type = "Donation",
            IsManual = true,
            Payments = new PaymentAdd[] { paymentAdd }
        };

        var requestBody = new StringContent(
            JsonSerializer.Serialize(createGiftRequest),
            Encoding.UTF8,
            "application/json"
        );

        var response = await httpClient.PostAsync($"gifts", requestBody, cancellationToken);

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedAccessException();
        }

        response.EnsureSuccessStatusCode();

        var postResponse = await response
            .Content
            .ReadFromJsonAsync<PostResponse>(cancellationToken: cancellationToken);

        var giftRead = await GetGift(postResponse.Id, cancellationToken);

        return giftRead;
    }

    /// <summary>
    /// Get a gift in RE NXT
    /// </summary>
    public async Task<GiftRead> GetGift(
        string giftId,
        CancellationToken cancellationToken
    )
    {
        var httpClient = await GetClient(cancellationToken, true);

        var response = await httpClient.GetAsync($"gifts/{giftId}", cancellationToken);

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedAccessException();
        }

        response.EnsureSuccessStatusCode();

        var giftRead = await response
            .Content
            .ReadFromJsonAsync<GiftRead>(cancellationToken: cancellationToken);

        return giftRead;
    }

    private async Task<HttpClient> GetClient(
        CancellationToken cancellationToken,
        bool anonymous = false
    )
    {
        var httpClient = _httpClientFactory.CreateClient("GiftsService");

        string token;

        // check for and invalid access token
        if (anonymous)
        {
            var refresh = await _authService.RefreshAccessTokenFromData(cancellationToken);
            token = refresh.AccessToken;
        }
        else
        {
            if (!_authService.IsAccessTokenValid() && _authService.HasValidRefreshToken())
            {
                await _authService.RefreshAccessToken(cancellationToken);
            }
            token = _sessionService.GetAccessToken();
        }

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
}
