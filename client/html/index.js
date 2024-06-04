document.addEventListener("DOMContentLoaded", async () => {
  const _baseUrl = "https://localhost:5001/api/payments";

  // Fetch checkout config from api
  let configResponse = await fetch(`${_baseUrl}/checkoutconfiguration`);
  let config = await configResponse.json();
  let cardToken;

  document.getElementById("payment-config-id").innerText = config.payment_configuration_id;
  document.getElementById("public-key").innerText = config.key;

  let recurringInput = document.getElementById("recurring");
  let amountInput = document.getElementById("amount");

  // Set up base transaction data properties
  let transactionData = {
    key: config.key,
    payment_configuration_id: config.payment_configuration_id
  };

  document.getElementById("donate").addEventListener("click", async () => {
    transactionData.amount = parseFloat(amountInput.value);
    if (recurringInput.checked) {
      cardToken = crypto.randomUUID();
      transactionData.card_token = cardToken;
    }
    Blackbaud_OpenPaymentForm(transactionData); // credit card transaction
  });

  document.addEventListener("checkoutComplete", async function (event) {
    // Capture the payment
    const transactionToken = event.detail.transactionToken;
    const amount = Math.round(event.detail.authorizedamount * 100);
    await fetch(`${_baseUrl}/checkouttransactions/capture`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        amount: amount,
        transaction_token: transactionToken,
        card_token: cardToken,
        payment_configuration_id: transactionData.payment_configuration_id,
      }),
    });
  });
});
