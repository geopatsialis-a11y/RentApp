import { Component, OnInit, Input, inject, signal } from '@angular/core';
import { DatePipe, CurrencyPipe } from '@angular/common';
import { AssetService } from '../../../core/services/asset-service';
import { AssetContractHistDto, RentalStatus } from '../../../types/asset';
import { PaginatedResult, PaginationMetadata,  } from '../../../types/pagination';

@Component({
  selector: 'app-asset-rental-history',
  imports: [DatePipe, CurrencyPipe],
  templateUrl: './asset-rental-history.html',
})
export class AssetRentalHistory implements OnInit {
  @Input({ required: true }) assetId!: string;

  private service = inject(AssetService);

  readonly RentalStatus = RentalStatus;

  isOpen  = signal(false);
  loading = signal(false);
  error   = signal('');

  records  = signal<AssetContractHistDto[]>([]);
  metadata = signal<PaginationMetadata | null>(null);
  page     = signal(1);
  pageSize = 5;

  ngOnInit() {  }

  toggle() {
    
    this.isOpen.update(v => !v);
    if (this.isOpen() && this.records().length === 0) {
      this.load();
    }
  }

  load(page = this.page()) {

    this.loading.set(true);
    this.error.set('');
    this.service.getContractHistory(this.assetId, page, this.pageSize).subscribe({
      next: (result: PaginatedResult<AssetContractHistDto>) => {
        this.records.set(result.items);
        this.metadata.set(result.metadata);
        this.page.set(result.metadata.currentPage);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('Σφάλμα φόρτωσης ιστορικού μισθώσεων.');
        this.loading.set(false);
      }
    });
  }

  goTo(p: number) {
    const meta = this.metadata();
    if (!meta || p < 1 || p > meta.totalPages) return;
    this.load(p);
  }

  get pages(): number[] {
    const total = this.metadata()?.totalPages ?? 0;
    return Array.from({ length: total }, (_, i) => i + 1);
  }

  statusLabel(s: RentalStatus): string {
    const map: Record<number, string> = {
      [RentalStatus.Pending]:   'Εκκρεμής',
      [RentalStatus.Active]:    'Ενεργό',
      [RentalStatus.Completed]: 'Ολοκληρωμένο',
      [RentalStatus.Cancelled]: 'Ακυρωμένο',
    };
    return map[s] ?? '—';
  }

  statusBadgeClass(s: RentalStatus): string {
    const map: Record<number, string> = {
      [RentalStatus.Pending]:   'badge-warning',
      [RentalStatus.Active]:    'badge-success',
      [RentalStatus.Completed]: 'badge-info',
      [RentalStatus.Cancelled]: 'badge-error',
    };
    return `badge badge-sm ${map[s] ?? ''}`;
  }
}
