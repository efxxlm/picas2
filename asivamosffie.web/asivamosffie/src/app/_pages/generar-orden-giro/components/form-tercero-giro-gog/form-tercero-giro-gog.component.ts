import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-form-tercero-giro-gog',
  templateUrl: './form-tercero-giro-gog.component.html',
  styleUrls: ['./form-tercero-giro-gog.component.scss']
})
export class FormTerceroGiroGogComponent implements OnInit {
  medioPagoArray = [
    { name: 'Transferencia electrónica', value: '1' }
  ];
  bancosArray = [
    { name: 'Bancolombia', value: '1' }
  ];
  addressForm = this.fb.group({});
  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
    this.addressForm = this.crearFormulario();
  }
  crearFormulario() {
    return this.fb.group({
      medioPagoGiroContrato: [null, Validators.required],
      titularCuenta:[null, Validators.required],
      numIdentificacionTitular:[null, Validators.required],
      numeroCuenta:[null, Validators.required],
      banco:[null, Validators.required],
      tipoCuenta:[null, Validators.required],
    })
  }
  maxLength(e: any, n: number) {
    console.log(e.editor.getLength()+" "+n);
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n-1, e.editor.getLength());
    }
  }
  textoLimpio(texto,n) {
    if (texto!=undefined) {
      return texto.getLength() > n ? n : texto.getLength();
    }
  }
  onSubmit() {
    console.log(this.addressForm.value);
  }
}
