using Blackbaud.PaymentsAPI.Sample.Backend.BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blackbaud.PaymentsAPI.Sample.Backend.Controllers;

[Route("auth")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authService;

    public AuthenticationController(IAuthenticationService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Returns a JSON response determining session's authenticated status.
    /// </summary>
    [HttpGet("authenticated")]
    public ActionResult Authenticated()
    {
        return new OkObjectResult(new { authenticated = _authService.IsAuthenticated() });
    }

    /// <summary>
    /// Fetches access token (using auth code from request body) and redirects to Home Page.
    /// </summary>
    [HttpGet("callback")]
    public async Task<IActionResult> Callback(
        [FromQuery] string code,
        [FromQuery] string state,
        CancellationToken cancellationToken
    )
    {
        await _authService.ExchangeCodeForAccessToken(code, state, cancellationToken);
        return Redirect("/");
    }

    /// <summary>
    /// Redirects user to authorization endpoint.
    /// </summary>
    [HttpGet("login")]
    public ActionResult LogIn()
    {
        Uri address = _authService.GetAuthorizationUri();
        return Redirect(address.ToString());
    }

    /// <summary>
    /// Destroys the authenticated session and redirects to Home Page.
    /// </summary>
    [HttpGet("logout")]
    public ActionResult LogOut()
    {
        _authService.LogOut();
        return Redirect("/");
    }

    /// <summary>
    /// Deliberately makes a call to the auth provider to refresh access token.
    /// </summary>
    [HttpGet("refresh-token")]
    public async Task<IActionResult> RefreshToken(CancellationToken cancellationToken)
    {
        var model = await _authService.RefreshAccessToken(cancellationToken);
        return Ok(model);
    }
}
