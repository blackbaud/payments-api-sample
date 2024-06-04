using Blackbaud.PaymentsAPITutorial.BusinessLogic;
using Microsoft.AspNetCore.Mvc;

namespace Blackbaud.PaymentsAPITutorial.Controllers
{
    /// <summary>
    /// Contains endpoints that interact with SKY API (constituents).
    /// </summary>
    [Route("api/[controller]")]
    public class ConstituentsController : Controller
    {
        private readonly ConstituentsService _constituentsService;

        public ConstituentsController(ConstituentsService constituentsService)
        {
            _constituentsService = constituentsService;
        }

        /// <summary>
        /// Returns a paginated list of constituents.
        /// </summary>
        [HttpGet()]
        public async Task<IActionResult> GetList(CancellationToken cancellationToken)
        {
            try
            {
                var model = await _constituentsService.GetConstituentsAsync(cancellationToken);
                return Ok(model);
            }
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("LogIn", "Authentication");
            }
        }

        /// <summary>
        /// Returns a constituent record from a provided ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id, CancellationToken cancellationToken)
        {
            try
            {
                var model = await _constituentsService.GetConstituentAsync(id, cancellationToken);
                return Ok(model);
            }
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("LogIn", "Authentication");
            }
        }
    }
}
