using Blackbaud.PaymentsAPI.Sample.Backend;
using Blackbaud.PaymentsAPI.Sample.Backend.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

var services = builder.Services;

services.AddControllers();
services.AddOpenApi();

var configuration = builder.Configuration;
var appSettings = configuration.GetSection("AppSettings").Get<AppSettings>();
services.Configure<AppSettings>(configuration.GetSection("AppSettings"));

// Configure session.
services.AddMemoryCache();
services.AddDistributedMemoryCache();
services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.Name = ".AuthCodeFlowTutorial.Session";
});

services.ConfigureApplicationServices(configuration);

var app = builder.Build();

app.Urls.Add("https://localhost:5001");

app.MapControllers();
app.MapOpenApi();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "v1");
});

app.UseSession();
app.UseHttpsRedirection();
app.UseCors();

app.Run();
