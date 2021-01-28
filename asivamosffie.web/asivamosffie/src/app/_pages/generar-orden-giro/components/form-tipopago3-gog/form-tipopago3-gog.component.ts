import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-form-tipopago3-gog',
  templateUrl: './form-tipopago3-gog.component.html',
  styleUrls: ['./form-tipopago3-gog.component.scss']
})
export class FormTipopago3GogComponent implements OnInit {
  addressForm = this.fb.group({
    valorDescuento: [null, Validators.required]
  });
  constructor( private fb: FormBuilder) { }

  ngOnInit(): void {
  }
  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }
  onSubmit() {
    console.log(this.addressForm.value);
  }
}
