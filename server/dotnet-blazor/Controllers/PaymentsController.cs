using Blackbaud.PaymentsAPITutorial.BusinessLogic;
using Blackbaud.PaymentsAPITutorial.DataAccess;
using Blackbaud.PaymentsAPITutorial.DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blackbaud.PaymentsAPITutorial.Controllers
{
    /// <summary>
    /// Contains endpoints that interact with SKY API (constituents).
    /// </summary>
    [Route("api/[controller]")]
    public class PaymentsController : Controller
    {
        private readonly PaymentsService _paymentsService;
        private readonly GiftsService _giftsService;
        private readonly LocalFileDataAdapter _localFileDataAdapter;

        public PaymentsController(PaymentsService paymentsService, GiftsService giftService, LocalFileDataAdapter localFileDataAdapter)
        {
            _paymentsService = paymentsService;
            _giftsService = giftService;
            _localFileDataAdapter = localFileDataAdapter;
        }

        /// <summary>
        /// Returns selected payment configuration
        /// </summary>
        [HttpGet("checkoutconfiguration")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCheckoutConfiguration(
            CancellationToken cancellationToken
        )
        {
            var model = await _paymentsService.GetCheckoutConfiguration(cancellationToken);
            return Ok(model);
        }

        /// <summary>
        /// Returns selected payment configuration
        /// </summary>
        [HttpPost("checkouttransactions/capture")]
        [AllowAnonymous]
        public async Task<IActionResult> CaptureCheckoutTransaction(
            [FromBody] TransactionCaptureRequest request,
            CancellationToken cancellationToken
        )
        {
            // Capture the checkout transaction
            var transactionRead = await _paymentsService.CaptureCheckoutTransaction(
                request,
                cancellationToken
            );

            // Create a Gift record in RE NXT connected to the transaction
            var giftRead = await _giftsService.CreateGift(
                transactionRead,
                request.PaymentConfigurationId,
                request.CardToken,
                anonymous: false,
                cancellationToken,
                request.TransactionToken
            );

            // Save the transaction and gift records to a local file
            var checkoutTransactionData = new CheckoutTransactionData
            {
                Transaction = transactionRead,
                Gift = giftRead
            };

            await _localFileDataAdapter.WriteDataAsync(checkoutTransactionData);

            return Ok();
        }
    }
}
