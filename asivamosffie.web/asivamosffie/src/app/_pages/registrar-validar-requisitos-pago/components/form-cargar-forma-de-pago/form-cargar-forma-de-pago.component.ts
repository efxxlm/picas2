import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-form-cargar-forma-de-pago',
  templateUrl: './form-cargar-forma-de-pago.component.html',
  styleUrls: ['./form-cargar-forma-de-pago.component.scss']
})
export class FormCargarFormaDePagoComponent implements OnInit {
  addressForm = this.fb.group({
    formaPagoPreconstruccion: [null, Validators.required],
    formaPagoConstruccion: [null, Validators.required]
  });
  formaPagoArray = [
    { name: 'Forma 1 (50/50)', value: '1' },
    { name: 'Forma 2 (60/40)', value: '2' },
    { name: 'Forma 3 (90/10)', value: '3' }
  ];
  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
  }
  onSubmit() {

  }
}
