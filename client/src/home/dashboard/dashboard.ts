import { Component, inject, OnInit, signal, computed } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CurrencyPipe, DatePipe, NgClass } from '@angular/common';
import { DashboardService } from '../../core/services/dashboard-service';
import {
  DashboardDto, MonthlyChartDto, OverdueContractDto, RecentTransactionDto
} from '../../types/dashboard';


@Component({
  selector: 'app-dashboard',
  imports: [RouterLink, CurrencyPipe, DatePipe, NgClass],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
})
export class Dashboard implements OnInit {
  private svc = inject(DashboardService);

  data    = signal<DashboardDto | null>(null);
  loading = signal(true);
  error   = signal<string | null>(null);

  kpi          = computed(() => this.data()?.kpi);
  overdue      = computed(() => this.data()?.overdueContracts ?? []);
  transactions = computed(() => this.data()?.recentTransactions ?? []);
  chart        = computed(() => this.data()?.monthlyChart ?? []);

  // bar chart helpers
  chartMax = computed(() => {
    const bars = this.chart();
    const max = Math.max(...bars.map(b => Math.max(b.income, b.expenses)), 1);
    return max;
  });

  barHeight(value: number): number {
    const max = this.chartMax();
    return max > 0 ? Math.round((value / max) * 100) : 0;
  }

  ngOnInit() {
    this.svc.get().subscribe({
      next: d => { this.data.set(d); this.loading.set(false); },
      error: () => { this.error.set('Αδυναμία φόρτωσης δεδομένων.'); this.loading.set(false); }
    });
  }

  daysSince(dateStr: string): number {
    return Math.floor((Date.now() - new Date(dateStr).getTime()) / 86_400_000);
  }

  methodLabel(m: number): string {
    return ['Μετρητά', 'Κάρτα', 'Τραπεζική'][m] ?? '';
  }
}
