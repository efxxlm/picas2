import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-amortizacion-pago',
  templateUrl: './amortizacion-pago.component.html',
  styleUrls: ['./amortizacion-pago.component.scss']
})
export class AmortizacionPagoComponent implements OnInit {

    @Input() solicitudPago: any;
    @Input() esVerDetalle = false;
    @Input() contrato: any;
    @Input() aprobarSolicitudPagoId: any;
    @Input() amortizacionAnticipoCodigo: string;
    solicitudPagoObservacionId = 0;
    solicitudPagoFase: any;
    solicitudPagoFaseAmortizacionId = 0;
    valorTotalDelContrato = 0;
    detalleForm: FormGroup = this.fb.group({
        //porcentajeAmortizacion: [null, Validators.required],
        valorAmortizacion: [ { value: null, disabled: true } , Validators.required]
    });

    constructor(
        private fb: FormBuilder )
    { }

    ngOnInit(): void {
        if ( this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase.length > 0 ) {
            for ( const solicitudPagoFase of this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase ) {
                if ( solicitudPagoFase.esPreconstruccion === false ) {
                    this.solicitudPagoFase = solicitudPagoFase;
                }
            }
        }

        if ( this.solicitudPagoFase.solicitudPagoFaseAmortizacion.length > 0 ) {
            const solicitudPagoFaseAmortizacion = this.solicitudPagoFase.solicitudPagoFaseAmortizacion[0];
            // Get detalle amortización
            this.detalleForm.setValue(
                {
                    //porcentajeAmortizacion: solicitudPagoFaseAmortizacion.porcentajeAmortizacion !== undefined ? solicitudPagoFaseAmortizacion.porcentajeAmortizacion : null,
                    valorAmortizacion: solicitudPagoFaseAmortizacion.valorAmortizacion !== undefined ? solicitudPagoFaseAmortizacion.valorAmortizacion : null
                }
            );
        }
    }

}
