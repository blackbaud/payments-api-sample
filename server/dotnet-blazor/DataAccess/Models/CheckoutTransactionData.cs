using Blackbaud.PaymentsAPITutorial.Models.Gifts;
using Blackbaud.PaymentsAPITutorial.Models.Payments;

namespace Blackbaud.PaymentsAPITutorial.DataAccess.Models;

public class CheckoutTransactionData
{
    public string TransactionToken { get; set; }
    public TransactionRead Transaction { get; set; }
    public GiftRead Gift { get; set; }
}
