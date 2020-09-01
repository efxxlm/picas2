import { Component, Input } from '@angular/core';
import { FormBuilder, Validators, FormArray } from '@angular/forms';
import { ComiteTecnico, SesionComiteSolicitud } from 'src/app/_interfaces/technicalCommitteSession';

@Component({
  selector: 'app-form-solicitud',
  templateUrl: './form-solicitud.component.html',
  styleUrls: ['./form-solicitud.component.scss']
})
export class FormSolicitudComponent {

  @Input() sesionComiteSolicitud: SesionComiteSolicitud; 

  fechaSolicitud: Date;
  numeroSolicitud: string;
  justificacion: string;

  tieneVotacion: boolean = true;
  cantidadAprobado: number = 0;
  cantidadNoAprobado: number = 0;
  resultadoVotacion: string = '';


  addressForm = this.fb.group({
    estadoSolicitud: [null, Validators.required],
    observaciones: [null, Validators.required],
    url: null,
    tieneCompromisos: [null, Validators.required],
    cuantosCompromisos: [null, Validators.required],
    compromisos: this.fb.array([])
  });

  estadosArray = [
    { name: 'estado 1', value: '1' },
    { name: 'estado 2', value: '2' },
    { name: 'estado 3', value: '3' },
    { name: 'estado 4', value: '4' },
    { name: 'estado 5', value: '5' },
    { name: 'estado 6', value: '6' }
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

  get compromisos() {
    return this.addressForm.get('compromisos') as FormArray;
  }

  constructor(private fb: FormBuilder) { }

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  textoLimpio(texto: string) {
    const textolimpio = texto.replace(/<[^>]*>/g, '');
    return textolimpio.length;
  }

  borrarArray(borrarForm: any, i: number) {
    borrarForm.removeAt(i);
  }

  agregaCompromiso() {
    this.compromisos.push(this.crearCompromiso());
  }

  crearCompromiso() {
    return this.fb.group({
      tarea: [null, Validators.compose([
        Validators.required, Validators.minLength(5), Validators.maxLength(100)])
      ],
      responsable: [null, Validators.required],
      fecha: [null, Validators.required]
    });
  }

  CambioCantidadCompromisos() {
    const FormGrupos = this.addressForm.value;
    if (FormGrupos.cuantosCompromisos > this.compromisos.length && FormGrupos.cuantosCompromisos < 100) {
      while (this.compromisos.length < FormGrupos.cuantosCompromisos) {
        this.compromisos.push(this.crearCompromiso());
      }
    } else if (FormGrupos.cuantosCompromisos <= this.compromisos.length && FormGrupos.cuantosCompromisos >= 0) {
      while (this.compromisos.length > FormGrupos.cuantosCompromisos) {
        this.borrarArray(this.compromisos, this.compromisos.length - 1);
      }
    }
  }

  onSubmit() {
    alert('Thanks!');
  }

  cargarRegistro(){

    this.sesionComiteSolicitud[0].tipoSolicitud
  }


}
