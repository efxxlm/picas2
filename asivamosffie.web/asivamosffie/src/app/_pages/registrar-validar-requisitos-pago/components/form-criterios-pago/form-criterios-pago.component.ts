import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-form-criterios-pago',
  templateUrl: './form-criterios-pago.component.html',
  styleUrls: ['./form-criterios-pago.component.scss']
})
export class FormCriteriosPagoComponent implements OnInit {
  addressForm = this.fb.group({
    criterioPago: [null, Validators.required],
    tipoPago: [null, Validators.required],
    conceptoPago: [null, Validators.required],
    valorFacturado: [null, Validators.required]
  });
  criteriosArray = [
    { name: 'Estudios y diseños interventoria hasta 90%', value: '1' },
    { name: 'Criterio 2', value: '2' },
    { name: 'Criterio 3', value: '3' },
    { name: 'Criterio 4', value: '4' },
    { name: 'Criterio 5', value: '5' },
  ];
  tipoPagoArray = [
    { name: 'Costo variable', value: '1' },
    { name: 'Tipo de pago 2', value: '2' },
    { name: 'Tipo de pago 3', value: '3' },
    { name: 'Tipo de pago 4', value: '4' },
    { name: 'Tipo de pago 5', value: '5' },
  ];
  conceptoPagoArray = [
    { name: '718100002002- DEMOLICIONES', value: '1' }
  ];
  obj1: boolean;
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
  getvalues(values: any) {
    console.log(values);
    const criterio1 = values.find(value => value.value == "1");
    criterio1 ? this.obj1 = true : this.obj1 = false;
  }
  onSubmit() {

  }
}
