import { Component, OnInit, inject, signal, computed } from '@angular/core';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { PaymentService } from '../../../core/services/payment-service';
import { PaymentListItemDto, PaymentMethod, TransactionType } from '../../../types/payment';
import { PaginatedResult } from '../../../types/pagination';

type Tab = 'income' | 'expense';

@Component({
  selector: 'app-transaction-list',
  imports: [CurrencyPipe, DatePipe, RouterLink],
  templateUrl: './transaction-list.html',
})
export class TransactionList implements OnInit {
  private svc = inject(PaymentService);

  readonly TransactionType = TransactionType;
  readonly PaymentMethod   = PaymentMethod;

  activeTab = signal<Tab>('income');

  // Income
  income        = signal<PaginatedResult<PaymentListItemDto> | null>(null);
  incomeLoading = signal(false);
  incomePage    = signal(1);

  // Expenses
  expenses        = signal<PaginatedResult<PaymentListItemDto> | null>(null);
  expensesLoading = signal(false);
  expensesPage    = signal(1);

  // Delete
  deleting = signal<string | null>(null);
  errorMsg = signal('');

  // Computed
  incomeItems   = computed(() => this.income()?.items   ?? []);
  expenseItems  = computed(() => this.expenses()?.items ?? []);

  incomeTotalOnPage  = computed(() => this.incomeItems().reduce((s, p) => s + p.amount, 0));
  expenseTotalOnPage = computed(() => this.expenseItems().reduce((s, p) => s + p.amount, 0));

  ngOnInit() {
    this.loadIncome();
    this.loadExpenses();
  }

  switchTab(t: Tab) {
    this.activeTab.set(t);
    this.errorMsg.set('');
  }

  loadIncome(page = this.incomePage()) {
    this.incomeLoading.set(true);
    this.svc.getIncome(page, 15).subscribe({
      next: r => { this.income.set(r); this.incomePage.set(r.metadata.currentPage); this.incomeLoading.set(false); },
      error: () => this.incomeLoading.set(false)
    });
  }

  loadExpenses(page = this.expensesPage()) {
    this.expensesLoading.set(true);
    this.svc.getExpenses(page, 15).subscribe({
      next: r => { this.expenses.set(r); this.expensesPage.set(r.metadata.currentPage); this.expensesLoading.set(false); },
      error: () => this.expensesLoading.set(false)
    });
  }

  delete(id: string) {
    if (!confirm('Διαγραφή συναλλαγής;')) return;
    this.deleting.set(id);
    this.errorMsg.set('');
    this.svc.deletePayment(id).subscribe({
      next: () => {
        this.deleting.set(null);
        if (this.activeTab() === 'income') this.loadIncome(this.incomePage());
        else this.loadExpenses(this.expensesPage());
      },
      error: err => {
        this.errorMsg.set(err.error?.message ?? 'Σφάλμα διαγραφής.');
        this.deleting.set(null);
      }
    });
  }

  incomePages()   { return this.pages(this.income()?.metadata.totalPages   ?? 1); }
  expensePages()  { return this.pages(this.expenses()?.metadata.totalPages ?? 1); }
  private pages(n: number) { return Array.from({ length: n }, (_, i) => i + 1); }

  methodLabel(m: PaymentMethod): string {
    return ['Μετρητά', 'Κάρτα', 'Τράπεζα'][m] ?? '—';
  }

  methodBadge(m: PaymentMethod): string {
    const map: Record<number, string> = {
      [PaymentMethod.Cash]: 'badge-ghost',
      [PaymentMethod.Card]: 'badge-info',
      [PaymentMethod.BankTransfer]: 'badge-primary',
    };
    return `badge badge-sm badge-outline ${map[m] ?? ''}`;
  }
}