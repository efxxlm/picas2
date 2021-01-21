import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-form-datos-factura',
  templateUrl: './form-datos-factura.component.html',
  styleUrls: ['./form-datos-factura.component.scss']
})
export class FormDatosFacturaComponent implements OnInit {
  addressForm = this.fb.group({
    numeroFactura: [null, Validators.required],
    fechaFactura: [null, Validators.required],
    aplicaDescuento: [null, Validators.required],
    tipoDescuento: [null, Validators.required],
    valorDescuento: [null, Validators.required],
    valorAPagarDespues: [null, Validators.required]
  });
  tiposDescuentoArray = [
    { name: '2x1000', value: '1' },
    { name: '4x1000', value: '2' },
  ];
  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
  }
  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  textoLimpio(texto: string) {
    let saltosDeLinea = 0;
    saltosDeLinea += this.contarSaltosDeLinea(texto, '<p>');
    saltosDeLinea += this.contarSaltosDeLinea(texto, '<li>');

    if ( texto ){
      const textolimpio = texto.replace(/<(?:.|\n)*?>/gm, '');
      return textolimpio.length + saltosDeLinea;
    }
  }

  private contarSaltosDeLinea(cadena: string, subcadena: string) {
    let contadorConcurrencias = 0;
    let posicion = 0;
    while ((posicion = cadena.indexOf(subcadena, posicion)) !== -1) {
      ++contadorConcurrencias;
      posicion += subcadena.length;
    }
    return contadorConcurrencias;
  }
  onSubmit() {

  }
}
