import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
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
    @Input() aprobarSolicitudPagoId: any;
    @Input() amortizacionAnticipoCodigo: string;
    @Output() estadoSemaforo = new EventEmitter<string>();
    solicitudPagoObservacionId = 0;
    solicitudPagoFase: any;
    solicitudPagoFaseAmortizacionId = 0;
    valorTotalDelContrato = 0;
    addressForm: FormGroup;
    detalleForm = this.fb.group({
        porcentajeAmortizacion: [null, Validators.required],
        valorAmortizacion: [ { value: null, disabled: true } , Validators.required]
    });
    editorStyle = {
      height: '45px',
      overflow: 'auto'
    };
    config = {
      toolbar: [
        ['bold', 'italic', 'underline'],
        [{ list: 'ordered' }, { list: 'bullet' }],
        [{ indent: '-1' }, { indent: '+1' }],
        [{ align: [] }],
      ]
    };
    estaEditando = false;
    constructor(
        private fb: FormBuilder,
        private routes: Router,
        private activatedRoute: ActivatedRoute,
        private dialog: MatDialog,
        private obsMultipleSvc: ObservacionesMultiplesCuService )
    {
        this.addressForm = this.crearFormulario();
    }

    ngOnInit(): void {
        if ( this.contrato.contratacion.disponibilidadPresupuestal.length > 0 ) {
            this.contrato.contratacion.disponibilidadPresupuestal.forEach( ddp => this.valorTotalDelContrato += ddp.valorSolicitud );
        }

        if ( this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase.length > 0 ) {
            for ( const solicitudPagoFase of this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase ) {
                if ( solicitudPagoFase.esPreconstruccion === false ) {
                    this.solicitudPagoFase = solicitudPagoFase;
                }
            }
        }

        if ( this.solicitudPagoFase.solicitudPagoFaseAmortizacion.length > 0 ) {
            const solicitudPagoFaseAmortizacion = this.solicitudPagoFase.solicitudPagoFaseAmortizacion[0]
            this.solicitudPagoFaseAmortizacionId = solicitudPagoFaseAmortizacion.solicitudPagoFaseAmortizacionId;
            // Get observaciones
            this.obsMultipleSvc.getObservacionSolicitudPagoByMenuIdAndSolicitudPagoId(
                this.aprobarSolicitudPagoId,
                this.solicitudPago.solicitudPagoId,
                this.solicitudPagoFaseAmortizacionId,
                this.amortizacionAnticipoCodigo )
                .subscribe(
                    response => {
                        const obsSupervisor = response.filter( obs => obs.archivada === false )[0];

                        if ( obsSupervisor !== undefined ) {
                            if ( obsSupervisor.registroCompleto === false ) {
                                this.estadoSemaforo.emit( 'en-proceso' );
                            }
                            if ( obsSupervisor.registroCompleto === true ) {
                                this.estadoSemaforo.emit( 'completo' );
                            }
                            this.solicitudPagoObservacionId = obsSupervisor.solicitudPagoObservacionId;
                            this.estaEditando = true;
                            this.addressForm.markAllAsTouched();
                            this.addressForm.setValue(
                                {
                                    fechaCreacion: obsSupervisor.fechaCreacion,
                                    tieneObservaciones: obsSupervisor.tieneObservacion !== undefined ? obsSupervisor.tieneObservacion : null,
                                    observaciones: obsSupervisor.observacion !== undefined ? ( obsSupervisor.observacion.length > 0 ? obsSupervisor.observacion : null ) : null
                                }
                            );
                        }
                    }
                );
            // Get detalle amortización
            this.detalleForm.setValue(
                {
                    porcentajeAmortizacion: solicitudPagoFaseAmortizacion.porcentajeAmortizacion !== undefined ? solicitudPagoFaseAmortizacion.porcentajeAmortizacion : null,
                    valorAmortizacion: solicitudPagoFaseAmortizacion.valorAmortizacion !== undefined ? solicitudPagoFaseAmortizacion.valorAmortizacion : null
                }
            );
        }
    }

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

}
