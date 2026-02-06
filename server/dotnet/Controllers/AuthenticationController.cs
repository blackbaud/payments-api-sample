using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blackbaud.PaymentsAPI.Sample.Backend.Controllers;

[Route("auth")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    /// <summary>
    /// Eecho
    /// </summary>
    [HttpGet("echo")]
    public ActionResult Echo()
    {
        return new OkObjectResult("Echo");
    }
}
