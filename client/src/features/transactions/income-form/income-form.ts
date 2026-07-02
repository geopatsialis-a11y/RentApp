import { Component, OnInit, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { CurrencyPipe, DatePipe, DecimalPipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { PaymentService } from '../../../core/services/payment-service';
import { ContractPaymentDto, PaymentMethod } from '../../../types/payment';
import { RentalStatus } from '../../../types/asset';

@Component({
  selector: 'app-income-form',
  imports: [ReactiveFormsModule, CurrencyPipe, DatePipe, RouterLink,DecimalPipe],
  templateUrl: './income-form.html',
})
export class IncomeForm implements OnInit {
  private svc = inject(PaymentService);
  private fb  = inject(FormBuilder);

  readonly PaymentMethod = PaymentMethod;
  readonly RentalStatus  = RentalStatus;

  contracts    = signal<ContractPaymentDto[]>([]);
  totalPages   = signal(1);
  currentPage  = signal(1);
  totalCount   = signal(0);
  loading      = signal(false);
  search       = signal('');
  statusFilter = signal<number | null>(null);

  selected = signal<ContractPaymentDto | null>(null);
  saving   = signal(false);
  success  = signal(false);
  errorMsg = signal('');

  form = this.fb.group({
    amount:        [null as number | null, [Validators.required, Validators.min(0.01)]],
    paymentDate:   [new Date().toISOString().slice(0, 10), Validators.required],
    paymentMethod: [PaymentMethod.Cash as number],
    notes:         [''],
  });

  ngOnInit() { this.load(); }

  load(page = 1) {
    this.loading.set(true);
    this.svc.getContracts(this.search(), this.statusFilter() ?? undefined, page, 10).subscribe({
      next: r => {
        this.contracts.set(r.items);
        this.totalPages.set(r.metadata.totalPages);
        this.currentPage.set(r.metadata.currentPage);
        this.totalCount.set(r.metadata.totalCount);
        this.loading.set(false);
        // Refresh selected contract balance if it appears in the new page
        const cur = this.selected();
        if (cur) {
          const refreshed = r.items.find(c => c.id === cur.id);
          if (refreshed) this.selected.set(refreshed);
        }
      },
      error: () => this.loading.set(false)
    });
  }

  onSearch(e: Event) {
    this.search.set((e.target as HTMLInputElement).value);
    this.load(1);
  }

  onStatus(e: Event) {
    const v = (e.target as HTMLSelectElement).value;
    this.statusFilter.set(v === '' ? null : Number(v));
    this.load(1);
  }

  select(c: ContractPaymentDto) {
    this.selected.set(c);
    this.errorMsg.set('');
    this.success.set(false);
    this.form.patchValue({ amount: c.outstandingBalance > 0 ? c.outstandingBalance : null });
  }

  clearSelected() { this.selected.set(null); }

  submit() {
    if (this.form.invalid || !this.selected()) { this.form.markAllAsTouched(); return; }
    this.saving.set(true);
    this.errorMsg.set('');
    this.success.set(false);
    const v = this.form.value;
    this.svc.recordIncome({
      contractId:    this.selected()!.id,
      amount:        v.amount!,
      paymentDate:   v.paymentDate!,
      paymentMethod: Number(v.paymentMethod) as PaymentMethod,
      notes:         v.notes || undefined,
    }).subscribe({
      next: () => {
        this.success.set(true);
        this.saving.set(false);
        this.form.patchValue({ amount: null, notes: '' });
        this.load(this.currentPage());
      },
      error: err => {
        this.errorMsg.set(err.error?.message ?? 'Σφάλμα αποθήκευσης.');
        this.saving.set(false);
      }
    });
  }

  statusLabel(s: RentalStatus): string {
    const map: Record<number, string> = {
      [RentalStatus.Pending]: 'Εκκρεμής', [RentalStatus.Active]: 'Ενεργό',
      [RentalStatus.Completed]: 'Ολοκλ.', [RentalStatus.Cancelled]: 'Ακυρωμένο',
    };
    return map[s] ?? '—';
  }

  statusBadge(s: RentalStatus): string {
    const map: Record<number, string> = {
      [RentalStatus.Pending]: 'badge-warning', [RentalStatus.Active]: 'badge-success',
      [RentalStatus.Completed]: 'badge-ghost', [RentalStatus.Cancelled]: 'badge-error',
    };
    return `badge badge-sm ${map[s] ?? ''}`;
  }

  pages() { return Array.from({ length: this.totalPages() }, (_, i) => i + 1); }
}