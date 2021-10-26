import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router, ActivatedRoute } from '@angular/router';
import { ObservacionesMultiplesCuService } from 'src/app/core/_services/observacionesMultiplesCu/observaciones-multiples-cu.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-form-amortizacion',
  templateUrl: './form-amortizacion.component.html',
  styleUrls: ['./form-amortizacion.component.scss']
})
export class FormAmortizacionComponent implements OnInit {

    @Input() solicitudPago: any;
    @Input() esVerDetalle = false;
    @Input() contrato: any;
    @Input() tieneObservacion: boolean;
    @Input() listaMenusId: any;
    @Input() amortizacionAnticipoCodigo: string;
    @Input() tieneObservacionOrdenGiro: boolean;
    @Input() contratacionProyectoId: number;
    @Output() semaforoObservacion = new EventEmitter<boolean>();
    esPreconstruccion = false;
    solicitudPagoFase: any;
    solicitudPagoFaseAmortizacionId = 0;
    valorTotalDelContrato = 0;
    esAutorizar: boolean;
    observacion: any;
    solicitudPagoObservacionId = 0;
    addressForm = this.fb.group({
      //porcentajeAmortizacion: [null, Validators.required],
      valorAmortizacion: [{ value: null, disabled: true }, Validators.required]
    });
    estaEditando = false;
    valorPorAmortizar: FormControl;

    constructor(
        private fb: FormBuilder )
    {}

    ngOnInit(): void {
        this.getDataAmortizacion()
        this.valorPorAmortizar = new FormControl({value: this.getProyectoId(this.contratacionProyectoId), disabled: true}, [Validators.required, Validators.max(100)]);
    }

    getProyectoId(codigo: any) {
        if (this.contrato.vAmortizacionXproyecto.length > 0) {
          const proyectoId = this.contrato.vAmortizacionXproyecto.find(proyectoId => proyectoId.contratacionProyectoId === codigo);
          if (proyectoId !== undefined) {
            return proyectoId.valorPorAmortizar;
          }
        }
    }

    getDataAmortizacion() {
        if (this.contrato.contratacion.disponibilidadPresupuestal.length > 0) {
            this.contrato.contratacion.disponibilidadPresupuestal.forEach( ddp => this.valorTotalDelContrato += ddp.valorSolicitud );
        }

        if ( this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0] !== undefined ) {
            if ( this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase !== undefined && this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase[ 0 ] !== undefined ) {
                if (this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase.length > 0) {
                    for (const solicitudPagoFase of this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase) {
                        if (solicitudPagoFase.esPreconstruccion === false && solicitudPagoFase.contratacionProyectoId === this.contratacionProyectoId) {
                            this.solicitudPagoFase = solicitudPagoFase;
                        }
                    }
                }

                if ( this.solicitudPagoFase !== undefined ) {
                    if ( this.solicitudPagoFase.solicitudPagoFaseAmortizacion.length > 0 ) {
                        const solicitudPagoFaseAmortizacion = this.solicitudPagoFase.solicitudPagoFaseAmortizacion[0];
                        this.solicitudPagoFaseAmortizacionId = solicitudPagoFaseAmortizacion.solicitudPagoFaseAmortizacionId;
                        this.estaEditando = true;

                        this.addressForm.markAllAsTouched();
                        this.addressForm.setValue(
                            {
                                //porcentajeAmortizacion: solicitudPagoFaseAmortizacion.porcentajeAmortizacion !== undefined ? solicitudPagoFaseAmortizacion.porcentajeAmortizacion : null,
                                valorAmortizacion: solicitudPagoFaseAmortizacion.valorAmortizacion !== undefined ? solicitudPagoFaseAmortizacion.valorAmortizacion : null
                            }
                        );
                    }
                }
            }
        }
    }

    /*
    crearFormulario() {
        return this.fb.group({
            fechaCreacion: [ null ],
            tieneObservaciones: [null, Validators.required],
            observaciones:[null, Validators.required],
        })
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

    openDialog(modalTitle: string, modalText: string) {
        const dialogRef = this.dialog.open(ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText }
        });
    }

    onSubmit() {
        this.estaEditando = true;
        this.addressForm.markAllAsTouched();
        if ( this.addressForm.get( 'tieneObservaciones' ).value !== null && this.addressForm.get( 'tieneObservaciones' ).value === false ) {
            this.addressForm.get( 'observaciones' ).setValue( '' );
        }

        const pSolicitudPagoObservacion = {
            solicitudPagoObservacionId: this.solicitudPagoObservacionId,
            solicitudPagoId: this.solicitudPago.solicitudPagoId,
            observacion: this.addressForm.get( 'observaciones' ).value !== null ? this.addressForm.get( 'observaciones' ).value : this.addressForm.get( 'observaciones' ).value,
            tipoObservacionCodigo: this.amortizacionAnticipoCodigo,
            menuId: this.aprobarSolicitudPagoId,
            idPadre: this.solicitudPagoFaseAmortizacionId,
            tieneObservacion: this.addressForm.get( 'tieneObservaciones' ).value !== null ? this.addressForm.get( 'tieneObservaciones' ).value : this.addressForm.get( 'tieneObservaciones' ).value
        };

        console.log( pSolicitudPagoObservacion );
        this.obsMultipleSvc.createUpdateSolicitudPagoObservacion( pSolicitudPagoObservacion )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                        () => this.routes.navigate(
                            [
                                '/verificarSolicitudPago/aprobacionSolicitud',  this.activatedRoute.snapshot.params.idContrato, this.activatedRoute.snapshot.params.idSolicitudPago
                            ]
                        )
                    );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            )
    }
    */

}
