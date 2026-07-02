import { Component, OnInit, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { PaymentService } from '../../../core/services/payment-service';
import { AssetService } from '../../../core/services/asset-service';
import { PaymentMethod } from '../../../types/payment';
import { AssetLookupDto } from '../../../types/asset';

@Component({
  selector: 'app-expense-form',
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './expense-form.html',
})
export class ExpenseForm {
  private svc    = inject(PaymentService);
  private assets = inject(AssetService);
  private fb     = inject(FormBuilder);

  readonly PaymentMethod = PaymentMethod;

  saving   = signal(false);
  success  = signal(false);
  errorMsg = signal('');

  // Asset picker
  assetSearch   = signal('');
  assetResults  = signal<AssetLookupDto[]>([]);
  assetLoading  = signal(false);
  selectedAssets = signal<AssetLookupDto[]>([]);

  // File
  file = signal<File | null>(null);

  form = this.fb.group({
    amount:        [null as number | null, [Validators.required, Validators.min(0.01)]],
    paymentDate:   [new Date().toISOString().slice(0, 10), Validators.required],
    paymentMethod: [PaymentMethod.Cash as number],
    description:   ['', [Validators.required, Validators.maxLength(500)]],
    notes:         [''],
  });

  onAssetInput(e: Event) {
    const term = (e.target as HTMLInputElement).value.trim();
    this.assetSearch.set(term);
    if (!term) { this.assetResults.set([]); return; }
    this.assetLoading.set(true);
    this.assets.getAssetLookup(term).subscribe({
      next: r => {
        const ids = new Set(this.selectedAssets().map(a => a.id));
        this.assetResults.set(r.filter(a => !ids.has(a.id)));
        this.assetLoading.set(false);
      },
      error: () => this.assetLoading.set(false)
    });
  }

  pickAsset(a: AssetLookupDto) {
    this.selectedAssets.update(list => [...list, a]);
    this.assetResults.set([]);
    this.assetSearch.set('');
  }

  removeAsset(id: string) {
    this.selectedAssets.update(list => list.filter(a => a.id !== id));
  }

  onFile(e: Event) {
    this.file.set((e.target as HTMLInputElement).files?.[0] ?? null);
  }

  clearFile() {
    this.file.set(null);
    const input = document.querySelector<HTMLInputElement>('#expense-file');
    if (input) input.value = '';
  }

  submit() {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }
    this.saving.set(true);
    this.errorMsg.set('');
    this.success.set(false);
    const v = this.form.value;
    this.svc.recordExpense(
      {
        amount:        v.amount!,
        paymentDate:   v.paymentDate!,
        paymentMethod: Number(v.paymentMethod) as PaymentMethod,
        description:   v.description!,
        notes:         v.notes || undefined,
        assetIds:      this.selectedAssets().map(a => a.id),
      },
      this.file() ?? undefined
    ).subscribe({
      next: () => {
        this.success.set(true);
        this.saving.set(false);
        this.form.reset({
          amount: null,
          paymentDate: new Date().toISOString().slice(0, 10),
          paymentMethod: PaymentMethod.Cash,
          description: '',
          notes: '',
        });
        this.selectedAssets.set([]);
        this.clearFile();
      },
      error: err => {
        this.errorMsg.set(err.error?.message ?? 'Σφάλμα αποθήκευσης.');
        this.saving.set(false);
      }
    });
  }
}