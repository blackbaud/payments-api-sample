using Blackbaud.PaymentsAPI.Sample.Backend.BusinessLogic;
using Blackbaud.PaymentsAPI.Sample.Backend.DataAccess;
using Blackbaud.PaymentsAPI.Sample.Backend.DataAccess.Models;
using Blackbaud.PaymentsAPI.Sample.Backend.Models.Payments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blackbaud.PaymentsAPI.Sample.Backend.Controllers;

[Route("payments")]
[ApiController]
public class PaymentsController : ControllerBase
{
    private readonly PaymentsService _paymentsService;
    private readonly GiftsService _giftsService;
    private readonly LocalFileDataAdapter _localFileDataAdapter;

    public PaymentsController(
        PaymentsService paymentsService,
        GiftsService giftService,
        LocalFileDataAdapter localFileDataAdapter
    )
    {
        _paymentsService = paymentsService;
        _giftsService = giftService;
        _localFileDataAdapter = localFileDataAdapter;
    }

    /// <summary>
    /// Returns a JSON response determining session's authenticated status.
    /// </summary>
    [HttpGet("paymentconfigurations")]
    public async Task<PaymentConfigurationListRead> ListPaymentConfigurations(
        CancellationToken cancellationToken
    )
    {
        return await _paymentsService.ListPaymentConfigurations(cancellationToken);
    }

    [HttpPost("paymentconfigurations/select")]
    public async Task<ActionResult> SelectPaymentConfiguration(
        [FromBody] SelectPaymentConfigurationRequest request
    )
    {
        await _paymentsService.SelectPaymentConfigurationId(request.PaymentConfigurationId);
        return Ok();
    }

    /// <summary>
    /// Returns selected payment configuration
    /// </summary>
    [HttpGet("checkoutconfiguration")]
    [AllowAnonymous]
    public async Task<IActionResult> GetCheckoutConfiguration(CancellationToken cancellationToken)
    {
        var model = await _paymentsService.GetCheckoutConfiguration(cancellationToken);
        return Ok(model);
    }

    /// <summary>
    /// Captures a transaction using old Checkout
    /// </summary>
    [HttpPost("checkouttransactions/capture")]
    [AllowAnonymous]
    public async Task<IActionResult> CaptureOldCheckoutTransaction(
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
            TransactionToken = request.TransactionToken,
            Transaction = transactionRead,
            Gift = giftRead,
        };

        await _localFileDataAdapter.WriteDataAsync(checkoutTransactionData);

        return Ok();
    }

    /// <summary>
    /// Captures a transaction
    /// </summary>
    [HttpPost("transactions/{transactionId}/capture")]
    [AllowAnonymous]
    public async Task<IActionResult> CaptureTransaction(
        [FromRoute] string transactionId,
        [FromBody] TransactionCaptureRequest request,
        CancellationToken cancellationToken
    )
    {
        // Capture the checkout transaction
        var transactionRead = await _paymentsService.CaptureTransaction(
            transactionId,
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
            TransactionToken = request.TransactionToken,
            Transaction = transactionRead,
            Gift = giftRead,
        };

        await _localFileDataAdapter.WriteDataAsync(checkoutTransactionData);

        return Ok();
    }
}
