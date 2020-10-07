import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Contratacion } from 'src/app/_interfaces/project-contracting';

@Component({
  selector: 'app-consideraciones-especiales',
  templateUrl: './consideraciones-especiales.component.html',
  styleUrls: ['./consideraciones-especiales.component.scss']
})
export class ConsideracionesEspecialesComponent implements OnInit {

  @Input() contratacion: Contratacion
  @Output() guardar: EventEmitter<any> = new EventEmitter()

  addressForm = this.fb.group({
    reasignacion: ['', Validators.required],
    descripcion: null
  });

  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
  }

  onSubmit() {

    this.contratacion.esObligacionEspecial = this.addressForm.get('reasignacion').value;
    this.contratacion.consideracionDescripcion = this.addressForm.get('descripcion').value;

    this.guardar.emit(null);
    
  }

  cargarRegistros(){

    this.addressForm.get('reasignacion').setValue( this.contratacion.esObligacionEspecial ? this.contratacion.esObligacionEspecial.toString() : false );
    this.addressForm.get('descripcion').setValue( this.contratacion.consideracionDescripcion );

  }

}
