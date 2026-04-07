using System.Net.Http.Headers;
using System.Text;
using Blackbaud.PaymentsAPI.Sample.Backend.BusinessLogic;
using Blackbaud.PaymentsAPI.Sample.Backend.BusinessLogic.Interfaces;
using Blackbaud.PaymentsAPI.Sample.Backend.DataAccess;
using Blackbaud.PaymentsAPI.Sample.Backend.Models;

namespace Blackbaud.PaymentsAPI.Sample.Backend;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds services to the container.
    /// </summary>
    public static void ConfigureApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        // Configure app settings so we can inject it into other classes.
        services.AddOptions();
        var appSettings = configuration.GetRequiredSection("AppSettings").Get<AppSettings>()!;
        services.Configure<AppSettings>(configuration.GetSection("AppSettings"));

        // Add Http client for authorizing with SKY API
        services.AddHttpClient(
            "AuthenticationService",
            client =>
            {
                // Set the base address to the AuthBaseUrl
                client.BaseAddress = new Uri(appSettings.AuthBaseUri!);

                // encode the client id and secret
                var encoded = Convert.ToBase64String(
                    Encoding.UTF8.GetBytes(
                        $"{appSettings.AuthClientId}:{appSettings.AuthClientSecret}"
                    )
                );

                // Add the Authorization header using basic authentication of client id and secret base 64 encoded
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Basic",
                    encoded
                );
            }
        );

        services.AddHttpClient(
            "PaymentsService",
            client =>
            {
                // Set the base address to the SkyApiBaseUri and append constituent/v1/
                client.BaseAddress = new Uri($"{appSettings.SkyApiBaseUri}payments/v1/");

                // Set request headers for bb-api-subscription-key
                client.DefaultRequestHeaders.Add(
                    "bb-api-subscription-key",
                    appSettings.PaymentsSubscriptionKey
                );
            }
        );

        services.AddHttpClient(
            "GiftsService",
            client =>
            {
                // Set the base address to the SkyApiBaseUri and append gift/v1/
                client.BaseAddress = new Uri($"{appSettings.SkyApiBaseUri}gift/v1/");

                // Set request headers for bb-api-subscription-key
                client.DefaultRequestHeaders.Add(
                    "bb-api-subscription-key",
                    appSettings.GeneralSubscriptionKey
                );
            }
        );

        // Services to be injected.
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddSingleton<IAuthenticationService, AuthenticationService>();
        services.AddTransient<ISessionService, SessionService>();
        services.AddSingleton<PaymentsService>();
        services.AddSingleton<GiftsService>();
        services.AddSingleton<LocalFileDataAdapter>();

        services.AddControllers();

        // Configure session.
        services.AddMemoryCache();
        services.AddDistributedMemoryCache();
        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(10);
            options.Cookie.Name = ".AuthCodeFlowTutorial.Session";
        });
        services.AddLogging(logging =>
        {
            logging.AddConfiguration(configuration.GetSection("Logging"));
            logging.AddConsole();
            logging.AddDebug();
        });
    }
}
