using Blackbaud.PaymentsAPI.Sample.Backend.BusinessLogic;
using Blackbaud.PaymentsAPI.Sample.Backend.BusinessLogic.Interfaces;
using Blackbaud.PaymentsAPI.Sample.Backend.DataAccess;
using Blackbaud.PaymentsAPI.Sample.Backend.Models;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddControllers();
services.AddOpenApi();

var configuration = builder.Configuration;
var appSettings = configuration.GetSection("AppSettings").Get<AppSettings>();
services.Configure<AppSettings>(configuration.GetSection("AppSettings"));

services.AddHttpClient<PaymentsService>(
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

services.AddHttpClient<GiftsService>(
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

// Configure session.
services.AddMemoryCache();
services.AddDistributedMemoryCache();
services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.Name = ".AuthCodeFlowTutorial.Session";
});

var app = builder.Build();

app.MapControllers();
app.MapOpenApi();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "v1");
});

app.UseSession();

app.Run();
