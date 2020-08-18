import { Component } from '@angular/core';
import { FormBuilder, Validators, FormArray } from '@angular/forms';


@Component({
  selector: 'app-votacion-solicitud',
  templateUrl: './votacion-solicitud.component.html',
  styleUrls: ['./votacion-solicitud.component.scss']
})
export class VotacionSolicitudComponent {
  miembros: any[] =  ['Juan Lizcano Garcia', 'Fernando José Aldemar Rojas', 'Gonzalo Díaz Mesa'];

  addressForm = this.fb.group({
    aprobacion: this.fb.array([]),
    observaciones: this.fb.array([]),
  });

  get aprobacion() {
    return this.addressForm.get('aprobacion') as FormArray;
  }

  get observaciones() {
    return this.addressForm.get('observaciones') as FormArray;
  }

  editorStyle = {
    height: '100px'
  };

  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  cargarDatos() {
    const aprobacion: any[] = ['', '', ''];

    aprobacion.forEach(valor => this.aprobacion.push(this.fb.control(valor)));
    aprobacion.forEach(valor => this.observaciones.push(this.fb.control(valor)));
  }

  constructor(private fb: FormBuilder) { }

  agregarAprovacion() {
    this.aprobacion.push(this.fb.control(null, Validators.required));
  }

  onSubmit() {
    alert('Thanks!');
  }
}
