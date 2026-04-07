import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit } from '@angular/core';
import {
  BlackbaudCheckoutConstructor,
  BlackbaudCheckoutService,
  CheckoutCompleteEvent,
  CheckoutConfiguration,
  CheckoutModalPaymentOptions,
  CheckoutWorkflowMode,
} from '@blackbaud/checkout';

import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ProcessingConfiguration } from '../../shared/models/processing-configuration';

declare let BlackbaudCheckout: typeof BlackbaudCheckoutConstructor;

@Component({
  selector: 'app-checkout',
  imports: [ReactiveFormsModule],
  templateUrl: './checkout.html',
  styleUrl: './checkout.scss',
})
export class Checkout implements OnInit {
  #checkout?: BlackbaudCheckoutService;

  #baseUrl = 'https://localhost:5001/payments';

  #client = inject(HttpClient);

  public checkoutConfig?: ProcessingConfiguration;
  public checkoutForm: FormGroup;
  public checkoutComplete?: CheckoutCompleteEvent;
  public capturing: boolean = false;

  get amount(): number {
    const currencyAmount = this.checkoutForm.get('amount')!.value as number;
    return Math.round(currencyAmount * 100);
  }

  constructor() {
    this.checkoutForm = new FormGroup({
      amount: new FormControl(27.5, {
        nonNullable: true,
        validators: [Validators.required, Validators.min(0)],
      }),
    });
  }

  ngOnInit(): void {
    this.#client
      .get<ProcessingConfiguration>(`${this.#baseUrl}/checkoutconfiguration`)
      .subscribe((configResponse: ProcessingConfiguration) => {
        this.checkoutConfig = configResponse;

        const checkoutConfig: CheckoutConfiguration = {
          workflowMode: CheckoutWorkflowMode.Modal,
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

        this.#checkout = BlackbaudCheckout(checkoutConfig);

        this.#checkout.checkoutComplete.subscribe(
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
    let modalOptions: CheckoutModalPaymentOptions = {
      baseAmount: this.amount,
    };
    this.#checkout!.checkoutModal.openPaymentForm(modalOptions);
  }

  private async captureTransaction(transactionId: string, amount: number) {
    // Capture the payment
    this.capturing = true;
    fetch(`${this.#baseUrl}/transactions/${transactionId}/capture`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        amount: amount,
        payment_configuration_id: this.checkoutConfig?.payment_configuration_id,
      }),
    }).then(() => {
      this.capturing = false;
      alert('Payment captured');
    });
  }
}
