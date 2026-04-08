document.addEventListener("DOMContentLoaded", async () => {
  const _baseUrl = "https://localhost:5001/payments";

  // Fetch checkout config from api
  let configResponse = await fetch(`${_baseUrl}/checkoutconfiguration`);
  let config = await configResponse.json();
  let cardToken;

  document.getElementById("payment-config-id").innerText = config.payment_configuration_id;

  const checkoutConfig = {
    workflowMode: 'modal',
    paymentConfigurationId: 'bcdefafe-8869-42f2-8bae-3f14f35879a6',
    applicationName: 'Payments API',
    paymentMethodOptions: {
      card: {
        enabled: true,
      },
      directDebit: {
        enabled: true,
      },
      wallets: {
        applePayEnabled: true,
        googlePayEnabled: true,
        amazonPayEnabled: true,
      },
      payPal: {
        enabled: true,
      },
      dafPay: {
        enabled: true,
      },
    },
    primaryColor: '#1870B8',
  };

  const checkout = BlackbaudCheckout(checkoutConfig);

  checkout.checkoutComplete.subscribe(
    (evt) => {
      captureTransaction(
        evt.transaction.id,
        evt.transaction.amountDetails.total,
      );
    },
  );

  let amountInput = document.getElementById("amount");

  document.getElementById("donate").addEventListener("click", async () => {
    const amount = Math.round(parseFloat(amountInput.value) * 100);
    const modalOptions = {
      baseAmount: amount,
    };
    checkout.checkoutModal.openPaymentForm(modalOptions);
  });

  function captureTransaction(transactionId, authorizedAmount) {

    // Display results
    document.getElementById("transaction-id").innerText = transactionId;
    document.getElementById("authorized-amount").innerText = authorizedAmount;

    // Capture the payment
    fetch(`${_baseUrl}/transactions/${transactionId}/capture`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        payment_configuration_id: config.payment_configuration_id,
        amount: authorizedAmount,
      }),
    }).then(() => {
      alert('Payment captured');
    });
  }
});
