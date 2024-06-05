using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Blackbaud.PaymentsAPITutorial.BusinessLogic.Interfaces;
using Blackbaud.PaymentsAPITutorial.DataAccess;
using Blackbaud.PaymentsAPITutorial.DataAccess.Models;
using Blackbaud.PaymentsAPITutorial.Models.Payments;

namespace Blackbaud.PaymentsAPITutorial.BusinessLogic;

/// <summary>
/// Interacts directly with SKY API Payments endpoints.
/// </summary>
public class PaymentsService
{
    private readonly ISessionService _sessionService;
    private readonly IAuthenticationService _authService;
    private readonly IHttpClientFactory _httpClientFactory = null!;
    private readonly LocalFileDataAdapter _localFileDataAdapter;

    /// <summary>
    /// Constructor
    /// </summary>
    public PaymentsService(
        ISessionService sessionService,
        IAuthenticationService authService,
        IHttpClientFactory httpClientFactory,
        LocalFileDataAdapter localFileDataAdapter
    )
    {
        _sessionService = sessionService;
        _authService = authService;
        _httpClientFactory = httpClientFactory;
        _localFileDataAdapter = localFileDataAdapter;
    }

    /// <summary>
    /// Returns a response containing a list of payment configurations.
    /// </summary>
    public async Task<PaymentConfigurationListRead> ListPaymentConfigurations(
        CancellationToken cancellationToken
    )
    {
        var httpClient = await GetClient(cancellationToken);
        var response = await httpClient.GetAsync($"paymentconfigurations", cancellationToken);
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
            .ReadFromJsonAsync<PaymentConfigurationListRead>(cancellationToken: cancellationToken);

        return model;
    }

    /// <inheritdoc/>
    public async Task<CheckoutConfiguration> GetCheckoutConfiguration(
        CancellationToken cancellationToken
    )
    {
        var publicKey = await GetPublicKey(cancellationToken);
        var savedPaymentData = await GetSavedPaymentData();
        var config = new CheckoutConfiguration
        {
            Key = publicKey,
            PaymentConfigurationId = savedPaymentData.PaymentConfigurationId
        };

        return config;
    }

    public async Task<TransactionRead> CaptureCheckoutTransaction(
        TransactionCaptureRequest request,
        CancellationToken cancellationToken
    )
    {
        var httpClient = await GetClient(cancellationToken, true);

        if (!string.IsNullOrEmpty(request.CardToken))
        {
            var savedPaymentData = await GetSavedPaymentData();
            savedPaymentData.CardToken = request.CardToken;

            await _localFileDataAdapter.WriteDataAsync(savedPaymentData);
        }

        var checkoutTransactionRequest = new CheckoutTransactionRequest
        {
            TransactionToken = request.TransactionToken,
            Amount = request.Amount
        };

        var requestBody = new StringContent(
            JsonSerializer.Serialize(checkoutTransactionRequest),
            Encoding.UTF8,
            "application/json"
        );

        var response = await httpClient.PostAsync(
            $"checkout/transaction",
            requestBody,
            cancellationToken
        );

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedAccessException();
        }

        response.EnsureSuccessStatusCode();

        var transactionRead = await response
            .Content
            .ReadFromJsonAsync<TransactionRead>(cancellationToken: cancellationToken);

        return transactionRead;
    }

    public async Task<TransactionRead> CreateBackofficeTransaction(
        CancellationToken cancellationToken
    )
    {
        var httpClient = await GetClient(cancellationToken, true);

        var savedPaymentData = await GetSavedPaymentData();
        var cardToken = savedPaymentData.CardToken;
        var paymentConfigurationId = savedPaymentData.PaymentConfigurationId;

        var createTransactionRequest = new CreateTransactionRequest
        {
            Amount = 1212,
            CardToken = cardToken,
            PaymentConfigurationId = paymentConfigurationId,
            TransactionType = "CardNotPresent",
            IsBackoffice = true
        };

        var requestBody = new StringContent(
            JsonSerializer.Serialize(createTransactionRequest),
            Encoding.UTF8,
            "application/json"
        );

        var response = await httpClient.PostAsync($"transactions", requestBody, cancellationToken);

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedAccessException();
        }

        response.EnsureSuccessStatusCode();

        var transactionRead = await response
            .Content
            .ReadFromJsonAsync<TransactionRead>(cancellationToken: cancellationToken);

        return transactionRead;
    }

    public async Task<string> GetPublicKey(CancellationToken cancellationToken)
    {
        var httpClient = await GetClient(cancellationToken, true);
        var response = await httpClient.GetAsync($"checkout/publickey", cancellationToken);
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
            .ReadFromJsonAsync<PublicKeyRead>(cancellationToken: cancellationToken);

        return model.PublicKey;
    }

    public async Task<string> GetSecurityToken(CancellationToken cancellationToken)
    {
        var savedPaymentData = await GetSavedPaymentData();
        var httpClient = await GetClient(cancellationToken, false);
        var response = await httpClient.GetAsync($"checkout/securitytoken/{savedPaymentData.PaymentConfigurationId}", cancellationToken);
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
            .ReadFromJsonAsync<SecurityTokenRead>(cancellationToken: cancellationToken);

        return model.SecurityToken;
    }

    public async Task<SavedPaymentData> GetSavedPaymentData()
    {
        var savedPaymentData = await _localFileDataAdapter.ReadDataAsync<SavedPaymentData>();
        return savedPaymentData;
    }

    public async Task SelectPaymentConfigurationId(string paymentConfigurationId)
    {
        var savedPaymentData = await _localFileDataAdapter.ReadDataAsync<SavedPaymentData>();
        savedPaymentData.PaymentConfigurationId = paymentConfigurationId;
        await _localFileDataAdapter.WriteDataAsync(savedPaymentData);
    }

    private async Task<HttpClient> GetClient(
        CancellationToken cancellationToken,
        bool anonymous = false
    )
    {
        var httpClient = _httpClientFactory.CreateClient("PaymentsService");

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
