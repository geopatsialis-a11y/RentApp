import { Component, inject, OnInit, signal } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { CustomerService } from '../../../core/services/customer-service';

@Component({
  selector: 'app-customer-form',
  imports: [ReactiveFormsModule,RouterLink],
  templateUrl: './customer-form.html',
  styleUrl: './customer-form.css',
})
export class CustomerForm implements OnInit {
  private fb = inject(FormBuilder);
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  private service = inject(CustomerService);

  isEdit = signal(false);
  loading = signal(false);
  errorMessage = signal('');
  private customerId: string | null = null;

  form = this.fb.group({
    name:    ['', Validators.required],
    afm:     ['', [Validators.required, Validators.pattern(/^\d{9}$/)]],
    email:   ['', Validators.email],
    phones:  this.fb.array([this.fb.control('', Validators.required)]),
    address: ['']
  });

  get f() { return this.form.controls; }
  get phonesArray() { return this.form.get('phones') as FormArray; }

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEdit.set(true);
      this.customerId = id;
      this.service.getById(id).subscribe(c => {
        this.form.patchValue({ name: c.name, afm: c.afm, email: c.email, address: c.address });
        this.phonesArray.clear();
        c.phones.forEach(p => this.phonesArray.push(this.fb.control(p, Validators.required)));
      });
    }
  }

  addPhone() { this.phonesArray.push(this.fb.control('', Validators.required)); }
  removePhone(i: number) { if (this.phonesArray.length > 1) this.phonesArray.removeAt(i); }

  ctrl(c: AbstractControl) { return c as import('@angular/forms').FormControl; }

  onSubmit() {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }
    this.loading.set(true);
    this.errorMessage.set('');
    const dto = { ...this.form.value, phones: this.phonesArray.value } as any;
    const req = this.isEdit()
      ? this.service.update(this.customerId!, dto)
      : this.service.create(dto);
    req.subscribe({
      next: () => this.router.navigateByUrl('/customer'),
      error: (err: any) => { this.errorMessage.set(err.error || 'Σφάλμα αποθήκευσης.'); this.loading.set(false); }
    });
  }
}