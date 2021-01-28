import { Component } from '@angular/core';
import { FormBuilder, Validators, FormArray } from '@angular/forms';

@Component({
  selector: 'app-form-perfil',
  templateUrl: './form-perfil.component.html',
  styleUrls: ['./form-perfil.component.scss']
})
export class FormPerfilComponent {
  addressForm = this.fb.group({
    perfil: [null, Validators.required],
    hvRequeridas: [null, Validators.compose([
      Validators.required, Validators.minLength(1), Validators.maxLength(3)])
    ],
    hvRecibidas: [null, Validators.compose([
      Validators.required, Validators.minLength(1), Validators.maxLength(3)])
    ],
    hvAprovadas: [null, Validators.compose([
      Validators.required, Validators.minLength(1), Validators.maxLength(3)])
    ],
    fecha: [null, Validators.compose([
      Validators.required, Validators.minLength(5), Validators.maxLength(1000)])
    ],
    observaciones: [null, Validators.required],
    numeroRadicado: this.fb.array([
      [null, Validators.compose([
        Validators.required, Validators.minLength(1), Validators.maxLength(20)])
      ]
    ]),
    url: [null, Validators.required],
  });

  minDate: Date;

  perfilArray = [
    { name: 'Ingeniero de obra', value: '0' },
    { name: 'Ingeniero de obra', value: '1' },
    { name: 'Ingeniero de obra', value: '2' }
  ];

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

  get numeroRadicado() {
    return this.addressForm.get('numeroRadicado') as FormArray;
  }

  constructor(private fb: FormBuilder) {
    this.minDate = new Date();
  }

  // evalua tecla a tecla
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

  agregaRadicado() {
    this.numeroRadicado.push(
      this.fb.control(null, Validators.compose([
        Validators.required, Validators.minLength(1), Validators.maxLength(20)])
      )
    );
  }

  eliminarRadicado(i: number) {
    this.numeroRadicado.removeAt(i);
  }

  onSubmit() {
    console.log(this.addressForm.value);
  }
}
