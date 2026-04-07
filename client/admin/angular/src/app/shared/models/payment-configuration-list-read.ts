import { PaymentConfigurationRead } from './payment-configuration-read';

export interface PaymentConfigurationListRead {
  count: number;
  value: PaymentConfigurationRead[];
}
