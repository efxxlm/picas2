import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-verdetalleedit-avance-actua-derivadas',
  templateUrl: './verdetalleedit-avance-actua-derivadas.component.html',
  styleUrls: ['./verdetalleedit-avance-actua-derivadas.component.scss']
})
export class VerdetalleeditAvanceActuaDerivadasComponent implements OnInit {

  addressForm = this.fb.group({
    fechaActuacionDerivada: [null, Validators.required],
    descripcionActuacionAdelantada: [null, Validators.required],
    estadoActuacionDerivada: [null, Validators.required],
    observaciones: [null, Validators.required],
    urlSoporte: [null, Validators.required]
  });
  editorStyle = {
    height: '25px'
  };

  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };
  estadoDerivadaArray = [
    { name: 'Cumplida', value: '1' },
    { name: 'Incumplida', value: '2' },
  ];
  constructor( private router: Router, private fb: FormBuilder) { }

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
    const textolimpio = texto.replace(/<[^>]*>/g, '');
    return textolimpio.length;
  }
  onSubmit() {

  }

}
