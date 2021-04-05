import { Component, OnInit } from '@angular/core';
import { Validators, FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-vigencias-valor-rapg',
  templateUrl: './vigencias-valor-rapg.component.html',
  styleUrls: ['./vigencias-valor-rapg.component.scss']
})
export class VigenciasValorRapgComponent implements OnInit {

  addressForm = this.fb.group({
    vigenciaActualizadaPoliza: [null, Validators.required],
    vigenciaActualizadaAmparo: [null, Validators.required],
    valorActualizadoAmparo: [null, Validators.required]
  });
  estaEditando = false;
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
    saltosDeLinea += this.contarSaltosDeLinea(texto, '<p');
    saltosDeLinea += this.contarSaltosDeLinea(texto, '<li');

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
    this.estaEditando = true;
  }

}
