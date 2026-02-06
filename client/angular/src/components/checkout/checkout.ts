import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit } from '@angular/core';
import {
  BlackbaudCheckoutConstructor,
  BlackbaudCheckoutService,
  CheckoutConfiguration,
  CheckoutModalPaymentOptions,
} from '@blackbaud/checkout';

import { SkyCheckboxModule, SkyInputBoxModule } from '@skyux/forms';
import { SkyPageModule } from '@skyux/pages';
import { ProcessingConfiguration } from '../../shared/models/processing-configuration';

declare let BlackbaudCheckout: typeof BlackbaudCheckoutConstructor;

@Component({
  selector: 'app-checkout',
  imports: [SkyPageModule, SkyCheckboxModule, SkyInputBoxModule],
  templateUrl: './checkout.html',
  styleUrl: './checkout.scss',
})
export class Checkout implements OnInit {
  public checkout?: BlackbaudCheckoutService;

  #baseUrl = 'https://localhost:5001/api/payments';

  #client = inject(HttpClient);

  public checkoutConfig?: ProcessingConfiguration;

  ngOnInit(): void {
    this.#client
      .get<ProcessingConfiguration>(`${this.#baseUrl}/checkoutconfiguration`)
      .subscribe((configResponse: ProcessingConfiguration) => {
        this.checkoutConfig = configResponse;

        const checkoutConfig: CheckoutConfiguration = {
          environmentId: configResponse.environment_id,
          paymentConfigurationId: configResponse.payment_configuration_id,
          applicationName: 'Payments Engineering Demo',
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
      });
  }

  public openCheckout() {
    let modal = this.checkout?.createCheckoutModalComponent();
    let modalOptions: CheckoutModalPaymentOptions = {
      baseAmount: 2750,
    };
    modal?.openPaymentForm(modalOptions);
  }
}
