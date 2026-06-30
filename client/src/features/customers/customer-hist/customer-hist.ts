import { Component, inject, OnInit, signal } from '@angular/core';
import { CustomerDto } from '../../../types/customers';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { CustomerService } from '../../../core/services/customer-service';

@Component({
  selector: 'app-customer-hist',
  imports: [RouterLink],
  templateUrl: './customer-hist.html',
  styleUrl: './customer-hist.css',
})
export class CustomerHist implements OnInit {
  private route = inject(ActivatedRoute);
  private service = inject(CustomerService);

  customer = signal<CustomerDto | null>(null);
  loading = signal(true);

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id')!;
    this.service.getById(id).subscribe({
      next: (c) => { this.customer.set(c); this.loading.set(false); },
      error: () => this.loading.set(false),
    });
  }
}