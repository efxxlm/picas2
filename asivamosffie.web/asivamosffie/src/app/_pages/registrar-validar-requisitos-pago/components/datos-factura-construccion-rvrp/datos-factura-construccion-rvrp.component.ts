import { RegistrarRequisitosPagoService } from './../../../../core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';
import { Router } from '@angular/router';
import { Component, Input, OnInit, OnChanges, SimpleChanges, EventEmitter, Output } from '@angular/core';
import { Validators, FormBuilder, FormArray } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Dominio, CommonService } from 'src/app/core/_services/common/common.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { ObservacionesMultiplesCuService } from 'src/app/core/_services/observacionesMultiplesCu/observaciones-multiples-cu.service';

@Component({
  selector: 'app-datos-factura-construccion-rvrp',
  templateUrl: './datos-factura-construccion-rvrp.component.html',
  styleUrls: ['./datos-factura-construccion-rvrp.component.scss']
})
export class DatosFacturaConstruccionRvrpComponent implements OnInit, OnChanges {

    @Input() solicitudPago: any;
    @Input() esVerDetalle = false;
    @Input() tieneObservacion: boolean;
    @Input() datosFacturaCodigo: string;
    @Input() listaMenusId: any;
    @Output() semaforoObservacion = new EventEmitter<boolean>();
    addressForm = this.fb.group({
        numeroFactura: [null, Validators.required],
        fechaFactura: [null, Validators.required]
    });
    valorFacturado = 0;
    tiposDescuentoArray: Dominio[] = [];
    solicitudPagoFaseFacturaDescuento: any[] = [];
    solicitudPagoFaseFacturaId = 0;
    solicitudPagoFaseFactura: any;
    solicitudPagoFase: any;
    esAutorizar: boolean;
    observacion: any;
    solicitudPagoObservacionId = 0;
    estaEditando = false;
    constructor(
        private fb: FormBuilder,
        private dialog: MatDialog,
        private commonSvc: CommonService,
        private routes: Router,
        private obsMultipleSvc: ObservacionesMultiplesCuService,
        private registrarPagosSvc: RegistrarRequisitosPagoService )
    {
        this.commonSvc.tiposDescuento()
            .subscribe( response => this.tiposDescuentoArray = response );
    }
    ngOnChanges(changes: SimpleChanges): void {
        if ( this.esVerDetalle === false ) {
            if ( changes.tieneObservacion.currentValue === true ) {
                this.addressForm.enable();
            }
        }
    }

    ngOnInit(): void {
        this.getDatosFactura();
    }

    getDatosFactura() {
        this.solicitudPagoFase = this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase[0];
        this.solicitudPagoFaseFactura = this.solicitudPagoFase.solicitudPagoFaseFactura[0];
        if ( this.solicitudPagoFaseFactura !== undefined ) {
            this.estaEditando = true;
            this.addressForm.markAllAsTouched();
            this.solicitudPagoFaseFacturaId = this.solicitudPagoFaseFactura.solicitudPagoFaseFacturaId;
            this.solicitudPagoFaseFacturaDescuento = this.solicitudPagoFaseFactura.solicitudPagoFaseFacturaDescuento;
            this.addressForm.get( 'numeroFactura' ).setValue( this.solicitudPagoFaseFactura.numero !== undefined ? this.solicitudPagoFaseFactura.numero : null );
            this.addressForm.get( 'fechaFactura' ).setValue( this.solicitudPagoFaseFactura.fecha !== undefined ? new Date( this.solicitudPagoFaseFactura.fecha ) : null );

            if ( this.addressForm.get( 'numeroFactura' ).value !== undefined && this.addressForm.get( 'fechaFactura' ).value !== undefined ) {
                this.addressForm.disable();
            }

            if ( this.esVerDetalle === false ) {
                // Get observacion CU autorizar solicitud de pago 4.1.9
                this.obsMultipleSvc.getObservacionSolicitudPagoByMenuIdAndSolicitudPagoId(
                    this.listaMenusId.autorizarSolicitudPagoId,
                    this.solicitudPago.solicitudPagoId,
                    this.solicitudPagoFaseFactura.solicitudPagoFaseFacturaId,
                    this.datosFacturaCodigo )
                    .subscribe(
                        response => {
                            const observacion = response.find( obs => obs.archivada === false );
                            if ( observacion !== undefined ) {
                                this.esAutorizar = true;
                                this.observacion = observacion;

                                if ( this.observacion.tieneObservacion === true ) {
                                    this.addressForm.enable();
                                    this.semaforoObservacion.emit( true );
                                    this.solicitudPagoObservacionId = observacion.solicitudPagoObservacionId;
                                }
                            }
                        }
                    );

                // Get observacion CU verificar solicitud de pago 4.1.8
                this.obsMultipleSvc.getObservacionSolicitudPagoByMenuIdAndSolicitudPagoId(
                    this.listaMenusId.aprobarSolicitudPagoId,
                    this.solicitudPago.solicitudPagoId,
                    this.solicitudPagoFaseFactura.solicitudPagoFaseFacturaId,
                    this.datosFacturaCodigo )
                    .subscribe(
                        response => {
                            const observacion = response.find( obs => obs.archivada === false );
                            if ( observacion !== undefined ) {
                                this.esAutorizar = false;
                                this.observacion = observacion;

                                if ( this.observacion.tieneObservacion === true ) {
                                    this.addressForm.enable();
                                    this.semaforoObservacion.emit( true );
                                    this.solicitudPagoObservacionId = observacion.solicitudPagoObservacionId;
                                }
                            }
                        }
                    );
            }
        }
        for ( const criterio of this.solicitudPagoFase.solicitudPagoFaseCriterio ) {
            this.valorFacturado += criterio.valorFacturado;
        }
    }

    validateNumberKeypress(event: KeyboardEvent) {
      const alphanumeric = /[0-9]/;
      const inputChar = String.fromCharCode(event.charCode);
      return alphanumeric.test(inputChar) ? true : false;
    }

    validateNumber( value: any ) {
        if ( isNaN( Number( value ) ) === true ) {
            this.addressForm.get( 'numeroDescuentos' ).setValue( '' );
        }
    }

    getTipoDescuento( tipoDescuentoCodigo: string ) {
        if ( this.tiposDescuentoArray.length > 0 ) {
            const descuento = this.tiposDescuentoArray.filter( descuento => descuento.codigo === tipoDescuentoCodigo );
            return descuento[0].nombre;
        }
    }

    openDialog(modalTitle: string, modalText: string) {
        const dialogRef = this.dialog.open(ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText }
        });
    }

    openDialogTrueFalse(modalTitle: string, modalText: string) {

        const dialogRef = this.dialog.open(ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText, siNoBoton: true }
        });

        return dialogRef.afterClosed();
    }

    onSubmit() {
        this.estaEditando = true;
        this.addressForm.markAllAsTouched();
        /*
        Se comenta metodo para obtener descuentos por novedad diseño

        const getSolicitudPagoFaseFacturaDescuento = () => {
            if ( this.descuentos.length > 0 ) {
                if ( this.addressForm.get( 'aplicaDescuento' ).value === true ) {
                    return this.descuentos.value;
                } else {
                    return [];
                }
            } else {
                return [];
            }
        }
        */

        const solicitudPagoFaseFactura = [
            {
                solicitudPagoFaseFacturaId: this.solicitudPagoFaseFacturaId,
                solicitudPagoFaseId: this.solicitudPagoFase.solicitudPagoFaseId,
                fecha: this.addressForm.get( 'fechaFactura' ).value !== null ? new Date( this.addressForm.get( 'fechaFactura' ).value ).toISOString() : this.addressForm.get( 'fechaFactura' ).value,
                valorFacturado: this.valorFacturado,
                numero: this.addressForm.get( 'numeroFactura' ).value,
                tieneDescuento: null,
                valorFacturadoConDescuento: null,
                solicitudPagoFaseFacturaDescuento: null
            }
        ]
        console.log( solicitudPagoFaseFactura );
        this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase[0].solicitudPagoFaseFactura = solicitudPagoFaseFactura;
        this.registrarPagosSvc.createEditNewPayment( this.solicitudPago )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    if ( this.observacion !== undefined ) {
                        this.observacion.archivada = !this.observacion.archivada;
                        this.obsMultipleSvc.createUpdateSolicitudPagoObservacion( this.observacion ).subscribe();
                    }
                    this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                        () => this.routes.navigate(
                            [
                                '/registrarValidarRequisitosPago/verDetalleEditar', this.solicitudPago.contratoId, this.solicitudPago.solicitudPagoId
                            ]
                        )
                    );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

}
