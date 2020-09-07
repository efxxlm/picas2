import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-form-contratacion',
  templateUrl: './form-contratacion.component.html',
  styleUrls: ['./form-contratacion.component.scss']
})
export class FormContratacionComponent implements OnInit {

  form: FormGroup;

  constructor ( private fb: FormBuilder ) {
    this.crearFormulario();
  };

  ngOnInit(): void {
  };

  crearFormulario () {
    this.form = this.fb.group({
      numeroContrato                : [ '', Validators.required ],
      fechaEnvioParaFirmaContratista: [ null ],
      fechaFirmaPorParteContratista : [ null ],
      fechaEnvioParaFirmaFiduciaria : [ null ],
      fechaFirmaPorParteFiduciaria  : [ null ],
      observaciones                 : [ null ],
      documento                     : [ null ],
      documentoFile                 : [ null ]
    });
  };

};
