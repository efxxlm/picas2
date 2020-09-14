import { Component, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-crear-solicitud-de-disponibilidad-presupuestal',
  templateUrl: './crear-solicitud-de-disponibilidad-presupuestal.component.html',
  styleUrls: ['./crear-solicitud-de-disponibilidad-presupuestal.component.scss']
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
    const textolimpio = texto.replace(/<[^>]*>/g, '');
    return textolimpio.length;
  }

  enviarObjeto() {
    console.log(this.objeto.value);
  }

}
