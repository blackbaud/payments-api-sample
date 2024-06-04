using Blackbaud.PaymentsAPITutorial;
using Blackbaud.PaymentsAPITutorial.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.Services.AddControllers();
builder.Services.AddSession();

// Add CORS services and configure to allow all origins
builder
    .Services
    .AddCors(options =>
    {
        options.AddPolicy(
            "AllowAllOrigins",
            builder =>
            {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            }
        );
    });

builder.Services.ConfigureApplicationServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseCors("AllowAllOrigins");

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.MapControllers();
app.UseSession();

app.Run("https://localhost:5001");
