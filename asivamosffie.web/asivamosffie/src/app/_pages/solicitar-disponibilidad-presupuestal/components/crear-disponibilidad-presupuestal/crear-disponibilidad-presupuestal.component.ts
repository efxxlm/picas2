import { Component, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-crear-disponibilidad-presupuestal',
  templateUrl: './crear-disponibilidad-presupuestal.component.html',
  styleUrls: ['./crear-disponibilidad-presupuestal.component.scss']
})
export class CrearSolicitudDeDisponibilidadPresupuestalComponent implements OnInit {

  objeto: FormControl;
  consecutivo: FormControl;

  editorStyle = {
    height: '45px'
  };

  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };

  constructor() {
    this.declararObjeto();
    this.declararConsecutivo();
  }

  ngOnInit(): void {
  }

  private declararObjeto() {
    this.objeto = new FormControl(null, [Validators.required]);
  }

  private declararConsecutivo() {
    this.consecutivo = new FormControl(null, [Validators.required]);
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

  enviarObjeto() {
    console.log(this.objeto.value);
  }

}
