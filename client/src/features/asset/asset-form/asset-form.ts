import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AssetType, FieldDataType } from '../../../types/Rent';

@Component({
  selector: 'app-asset-form',
  imports: [ReactiveFormsModule ],
  templateUrl: './asset-form.html',
  styleUrl: './asset-form.css',
})
export class AssetForm implements OnInit {
 assetType!: AssetType; // Έρχεται από το γονικό component (αφού διαλέξει κατηγορία ο χρήστης)
  
  assetForm!: FormGroup;
  FieldDataType = FieldDataType; // Για να το βλέπει το HTML

  constructor(private fb: FormBuilder) {}

  ngOnInit() {
    this.buildForm();
  }

  buildForm() {
    // 1. Σταθερά πεδία που έχουν ΟΛΑ τα Assets (από την BaseEntity & το βασικό Asset μοντέλο)
    const formControls: any = {
      title: ['', Validators.required],
      serialNumber: [''],
      status: [0, Validators.required]
    };

    // 2. Δυναμικά πεδία βάσει του EAV
    // Φτιάχνουμε ένα υπο-FormGroup αποκλειστικά για τα δυναμικά χαρακτηριστικά
    const propertiesGroup: any = {};

    this.assetType.fields.forEach(field => {
      const validators = field.isRequired ? [Validators.required] : [];
      
      // Default τιμές ανάλογα με τον τύπο
      let defaultValue: any = '';
      if (field.dataType === FieldDataType.Boolean) defaultValue = false;
      if (field.dataType === FieldDataType.Number) defaultValue = null;

      propertiesGroup[field.name] = [defaultValue, validators];
    });

    // Ενώνουμε τα σταθερά με τα δυναμικά (ως nested object: propertiesJson)
    formControls.propertiesJson = this.fb.group(propertiesGroup);
    
    this.assetForm = this.fb.group(formControls);
  }

  onSubmit() {
    if (this.assetForm.valid) {
      const payload = this.assetForm.value;
      // Το payload.propertiesJson είναι ήδη ένα τέλειο JSON object! 
      // Το στέλνεις κατευθείαν στο .NET API σου.
      console.log('Saving Asset:', payload);
    }
  }
}