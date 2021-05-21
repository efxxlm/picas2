import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-form-tercero-giro-gog',
  templateUrl: './form-tercero-giro-gog.component.html',
  styleUrls: ['./form-tercero-giro-gog.component.scss']
})
export class FormTerceroGiroGogComponent implements OnInit {

  @Input() solicitudPago: any;
  @Input() esVerDetalle: boolean;
  addressForm: FormGroup;

  constructor(
    private fb: FormBuilder
  ) { }

  ngOnInit(): void {
    this.addressForm = this.crearFormulario();
  }

  crearFormulario() {
    return this.fb.group({
        medioPagoGiroContrato: [null, Validators.required],
        transferenciaElectronica: this.fb.group( {
            ordenGiroTerceroId: [ 0 ],
            ordenGiroTerceroTransferenciaElectronicaId: [ 0 ],
            titularCuenta: [ '' ],
            titularNumeroIdentificacion: [ '' ],
            numeroCuenta: [ '' ],
            bancoCodigo: [ null ],
            esCuentaAhorros: [ null ]
        } ),
        chequeGerencia: this.fb.group( {
            ordenGiroTerceroId: [ 0 ],
            ordenGiroTerceroChequeGerenciaId: [ 0 ],
            nombreBeneficiario: [ '' ],
            numeroIdentificacionBeneficiario: [ '' ]
        } )
    })
}

}
