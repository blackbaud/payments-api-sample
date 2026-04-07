import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit } from '@angular/core';
import { AuthenticatedResponse } from '../shared/models/authenticated.response';
import { PaymentConfigurationListRead } from '../shared/models/payment-configuration-list-read';
import { PaymentConfigurationRead } from '../shared/models/payment-configuration-read';
import { ProcessingConfiguration } from '../shared/models/processing-configuration';

@Component({
  selector: 'app-home',
  imports: [],
  templateUrl: './home.html',
  styleUrl: './home.scss',
})
export class Home implements OnInit {
  public authenticated: boolean = false;
  public loading: boolean = false;
  public paymentConfigs: PaymentConfigurationRead[] = [];
  public checkoutConfig?: ProcessingConfiguration;

  #client = inject(HttpClient);

  public ngOnInit(): void {
    this.#client
      .get<AuthenticatedResponse>('https://localhost:5001/auth/authenticated')
      .subscribe((res: AuthenticatedResponse) => {
        this.authenticated = res.authenticated;
        if (this.authenticated) {
          this.fetchPaymentConfigs();
          this.fetchCheckoutConfiguration();
        }
      });
  }

  public selectPaymentConfiguration(paymentConfigurationId: string): void {
    this.#client
      .post('https://localhost:5001/payments/paymentconfigurations/select', {
        payment_configuration_id: paymentConfigurationId,
      })
      .subscribe(() => {
        this.fetchCheckoutConfiguration();
      });
  }

  private fetchPaymentConfigs(): void {
    this.loading = true;
    this.#client
      .get<PaymentConfigurationListRead>(
        'https://localhost:5001/payments/paymentconfigurations',
      )
      .subscribe((res: PaymentConfigurationListRead) => {
        this.loading = false;
        this.paymentConfigs = res.value;
      });
  }

  private fetchCheckoutConfiguration(): void {
    this.#client
      .get<ProcessingConfiguration>(
        `https://localhost:5001/payments/checkoutconfiguration`,
      )
      .subscribe((configResponse: ProcessingConfiguration) => {
        this.checkoutConfig = configResponse;
      });
  }
}
