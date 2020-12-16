import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-form-solicitud-otros-costosservicios',
  templateUrl: './form-solicitud-otros-costosservicios.component.html',
  styleUrls: ['./form-solicitud-otros-costosservicios.component.scss']
})
export class FormSolicitudOtrosCostosserviciosComponent implements OnInit {
  addressForm = this.fb.group({
    numeroContrato: [null, Validators.required],
    numeroRadicadoSAC: [null, Validators.required],
    numeroFactura: [null, Validators.required],
    valorFacturado: [null, Validators.required],
    tipoPago: [null, Validators.required],
  });
  contratoId: any;
  contratosArray = [
    { name: 'N801801', value: '1' }
  ];
  tipoPagoArray = [
    { name: 'Costo variable', value: '1' }
  ];
  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
  }
  seleccionAutocomplete(id:any){
    this.addressForm.value.numeroContrato = id;
    this.contratoId = id;
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
