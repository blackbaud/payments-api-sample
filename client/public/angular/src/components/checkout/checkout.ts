import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit } from '@angular/core';
import {
  BlackbaudCheckoutConstructor,
  BlackbaudCheckoutService,
  CheckoutCompleteEvent,
  CheckoutConfiguration,
  CheckoutModalComponent,
  CheckoutModalPaymentOptions,
} from '@blackbaud/checkout';

import { ProcessingConfiguration } from '../../shared/models/processing-configuration';

declare let BlackbaudCheckout: typeof BlackbaudCheckoutConstructor;

@Component({
  selector: 'app-checkout',
  imports: [],
  templateUrl: './checkout.html',
  styleUrl: './checkout.scss',
})
export class Checkout implements OnInit {
  public checkout?: BlackbaudCheckoutService;

  #baseUrl = 'https://localhost:5001/payments';

  #client = inject(HttpClient);

  public checkoutConfig?: ProcessingConfiguration;

  public checkoutComplete?: CheckoutCompleteEvent;

  ngOnInit(): void {
    this.#client
      .get<ProcessingConfiguration>(`${this.#baseUrl}/checkoutconfiguration`)
      .subscribe((configResponse: ProcessingConfiguration) => {
        this.checkoutConfig = configResponse;

        const checkoutConfig: CheckoutConfiguration = {
          environmentId: configResponse.environment_id,
          paymentConfigurationId: configResponse.payment_configuration_id,
          applicationName: 'Payments API',
          completeCoverOptions: {},
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

        this.checkout = BlackbaudCheckout(checkoutConfig);

        this.checkout.checkoutComplete.subscribe(
          (evt: CheckoutCompleteEvent) => {
            this.checkoutComplete = evt;
            this.captureTransaction(
              evt.transaction?.id!,
              evt.transaction?.amountDetails?.total!,
            );
          },
        );
      });
  }

  public openCheckout() {
    let modal: CheckoutModalComponent =
      this.checkout!.createCheckoutModalComponent();
    let modalOptions: CheckoutModalPaymentOptions = {
      baseAmount: 2750,
    };
    modal.openPaymentForm(modalOptions);
  }

  private async captureTransaction(transactionId: string, amount: number) {
    // Capture the payment
    fetch(`${this.#baseUrl}/transactions/${transactionId}/capture`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        amount: amount,
      }),
    }).then((response) => {
      alert('Payment captured');
    });
  }
}
