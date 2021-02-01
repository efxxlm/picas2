import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Contratacion } from 'src/app/_interfaces/project-contracting';

@Component({
  selector: 'app-consideraciones-especiales',
  templateUrl: './consideraciones-especiales.component.html',
  styleUrls: ['./consideraciones-especiales.component.scss']
})
export class ConsideracionesEspecialesComponent implements OnInit {

  @Input() contratacion: Contratacion;
  @Output() guardar: EventEmitter<any> = new EventEmitter()

  addressForm = this.fb.group({
    reasignacion: ['', Validators.required],
    descripcion: [ null ]
  });
  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };

  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
  }

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n - 1, e.editor.getLength());
    }
  }

  textoLimpio( evento: any, n: number ) {
    if ( evento !== undefined ) {
        return evento.getLength() > n ? n : evento.getLength();
    } else {
        return 0;
    }
  }

  onSubmit() {

    this.contratacion.esObligacionEspecial = this.addressForm.get('reasignacion').value;
    this.contratacion.consideracionDescripcion = this.addressForm.get('descripcion').value;

    this.guardar.emit(null);
    console.log( this.contratacion );

  }

  cargarRegistros(){

    this.addressForm.get('reasignacion').setValue( this.contratacion.esObligacionEspecial !== undefined ? this.contratacion.esObligacionEspecial : '' );
    this.addressForm.get('descripcion').setValue( this.contratacion.consideracionDescripcion ? this.contratacion.consideracionDescripcion : null );

  }

}
