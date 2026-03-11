import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit } from '@angular/core';
import { AuthenticatedResponse } from '../shared/models/authenticated.response';
import { PaymentConfigurationListRead } from '../shared/models/payment-configuration-list-read';
import { PaymentConfigurationRead } from '../shared/models/payment-configuration-read';

@Component({
  selector: 'app-home',
  imports: [],
  templateUrl: './home.html',
  styleUrl: './home.scss',
})
export class Home implements OnInit {
  public authenticated: boolean = false;
  public paymentConfigs: PaymentConfigurationRead[] = [];
  public ngOnInit(): void {
    this.httpClient
      .get<AuthenticatedResponse>('https://localhost:5001/auth/authenticated')
      .subscribe((res: AuthenticatedResponse) => {
        this.authenticated = res.authenticated;
        if (this.authenticated) {
          this.fetchPaymentConfigs();
        }
      });
  }
  private httpClient = inject(HttpClient);

  public selectPaymentConfiguration(paymentConfigurationId: string): void {
    this.httpClient
      .post('https://localhost:5001/payments/paymentconfigurations/select', {
        payment_configuration_id: paymentConfigurationId,
      })
      .subscribe(() => {});
  }

  private fetchPaymentConfigs(): void {
    this.httpClient
      .get<PaymentConfigurationListRead>(
        'https://localhost:5001/payments/paymentconfigurations',
      )
      .subscribe((res: PaymentConfigurationListRead) => {
        this.paymentConfigs = res.value;
      });
  }
}
