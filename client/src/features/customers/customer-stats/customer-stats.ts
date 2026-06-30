import { Component, computed, input } from '@angular/core';
import { CustomerDto, CustomerStatsDto } from '../../../types/customers';

@Component({
  selector: 'app-customer-stats',
  imports: [],
  templateUrl: './customer-stats.html',
  styleUrl: './customer-stats.css',
})
export class CustomerStats {
  // Server-side στατιστικά (από όλους τους πελάτες, όχι μόνο την τρέχουσα σελίδα)
  stats = input<CustomerStatsDto | null>(null);
}
