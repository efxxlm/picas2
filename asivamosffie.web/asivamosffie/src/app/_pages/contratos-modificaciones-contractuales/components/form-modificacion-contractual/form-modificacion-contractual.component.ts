import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-form-modificacion-contractual',
  templateUrl: './form-modificacion-contractual.component.html',
  styleUrls: ['./form-modificacion-contractual.component.scss']
})
export class FormModificacionContractualComponent implements OnInit {

  formModificacionContractual: FormGroup;

  constructor ( private fb: FormBuilder ) {
    this.crearFormulario();
  }

  ngOnInit(): void {
  };

  crearFormulario () {
    this.formModificacionContractual = this.fb.group({
      numeroOtroSi                  : [ '' ],
      fechaEnvioParaFirmaContratista: [ null ],
      fechaFirmaPorParteContratista : [ null ],
      fechaEnvioParaFirmaFiduciaria : [ null ],
      fechaFirmaPorParteFiduciaria  : [ null ],
      observaciones                 : [ null ],
      documento                     : [ null ],
      documentoFile                 : [ null ]
    });
  };

}
