import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-manejo-anticipo',
  templateUrl: './manejo-anticipo.component.html',
  styleUrls: ['./manejo-anticipo.component.scss']
})
export class ManejoAnticipoComponent implements OnInit {

  formAnticipo: FormGroup;
  @Input() contratoConstruccion: any;
  @Output() manejoAnticipo = new EventEmitter();

  constructor ( private fb: FormBuilder ) {
    this.crearFormulario();
  }

  ngOnInit(): void {
    if ( this.contratoConstruccion ) {
      this.formAnticipo.setValue(
        {
          requiereAnticipo              : this.contratoConstruccion.manejoAnticipoRequiere !== undefined ? this.contratoConstruccion.manejoAnticipoRequiere : null,
          planInversionAnticipo         : this.contratoConstruccion.manejoAnticipoPlanInversion !== undefined ? this.contratoConstruccion.manejoAnticipoPlanInversion : null,
          cronogramaAmortizacionAprobado: this.contratoConstruccion.manejoAnticipoCronogramaAmortizacion !== undefined ? this.contratoConstruccion.manejoAnticipoCronogramaAmortizacion : null,
          urlSoporte                    : this.contratoConstruccion.manejoAnticipoRutaSoporte !== undefined ? this.contratoConstruccion.manejoAnticipoRutaSoporte : null,
        }
      )
    }
  }

  crearFormulario () {
    this.formAnticipo = this.fb.group({
      requiereAnticipo: [null, Validators.required],
      planInversionAnticipo: [null, Validators.required],
      cronogramaAmortizacionAprobado: [null, Validators.required],
      urlSoporte: [null, Validators.required]
    });
  };

  guardar () {
    this.manejoAnticipo.emit( this.formAnticipo.value );
  }

};