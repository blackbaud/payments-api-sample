using Blackbaud.PaymentsAPI.Sample.Backend.Models.Gifts;
using Blackbaud.PaymentsAPI.Sample.Backend.Models.Payments;

namespace Blackbaud.PaymentsAPI.Sample.Backend.DataAccess.Models;

public class CheckoutTransactionData
{
    public string TransactionToken { get; set; }
    public TransactionRead Transaction { get; set; }
    public GiftRead Gift { get; set; }
}
