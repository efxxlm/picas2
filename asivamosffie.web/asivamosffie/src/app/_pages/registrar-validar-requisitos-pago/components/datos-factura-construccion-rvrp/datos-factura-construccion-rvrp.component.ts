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
    @Input() tieneObservacionOrdenGiro: boolean;
    @Input() esPreconstruccion = true;
    @Output() semaforoObservacion = new EventEmitter<boolean>();
    addressForm = this.fb.group({
        numeroFactura: [null, Validators.required],
        fechaFactura: [null, Validators.required],
        numeroDescuentos: [ '' ],
        valorAPagarDespues: [ { value: null, disabled: true } ],
        aplicaDescuento: [ null ],
        descuentos: this.fb.array( [] )
    });
    valorFacturado = 0;
    tiposDescuentoArray: Dominio[] = [];
    listaTipoDescuento: Dominio[] = [];
    solicitudPagoFaseFacturaDescuento: any[] = [];
    solicitudPagoFaseFacturaId = 0;
    solicitudPagoFaseFactura: any;
    solicitudPagoFase: any;
    esAutorizar: boolean;
    observacion: any;
    solicitudPagoObservacionId = 0;
    estaEditando = false;
    minDate = new Date();

    get descuentos() {
        return this.addressForm.get('descuentos') as FormArray;
    }

    constructor(
        private fb: FormBuilder,
        private dialog: MatDialog,
        private commonSvc: CommonService,
        private routes: Router,
        private obsMultipleSvc: ObservacionesMultiplesCuService,
        private registrarPagosSvc: RegistrarRequisitosPagoService )
    {
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

    async getDatosFactura() {
        this.tiposDescuentoArray = await this.commonSvc.tiposDescuento().toPromise();
        this.listaTipoDescuento = [ ...this.tiposDescuentoArray ];
        const solicitudPagoRegistrarSolicitudPago = this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0];

        if ( this.esPreconstruccion === true ) {
            if ( solicitudPagoRegistrarSolicitudPago.solicitudPagoFase.length > 0 ) {
                for ( const solicitudPagoFase of solicitudPagoRegistrarSolicitudPago.solicitudPagoFase ) {
                    if ( solicitudPagoFase.esPreconstruccion === true ) {
                        this.solicitudPagoFase = solicitudPagoFase;
                    }
                }
            }
        }

        if ( this.esPreconstruccion === false ) {
            if ( solicitudPagoRegistrarSolicitudPago.solicitudPagoFase.length > 0 ) {
                for ( const solicitudPagoFase of solicitudPagoRegistrarSolicitudPago.solicitudPagoFase ) {
                    if ( solicitudPagoFase.esPreconstruccion === false ) {
                        this.solicitudPagoFase = solicitudPagoFase;
                    }
                }
            }
        }

        this.solicitudPagoFaseFactura = this.solicitudPagoFase.solicitudPagoFaseFactura[0];
        if ( this.solicitudPagoFaseFactura !== undefined ) {
            this.estaEditando = true;
            this.addressForm.markAllAsTouched();
            this.solicitudPagoFaseFacturaId = this.solicitudPagoFaseFactura.solicitudPagoFaseFacturaId;
            this.solicitudPagoFaseFacturaDescuento = this.solicitudPagoFaseFactura.solicitudPagoFaseFacturaDescuento;
            this.addressForm.get( 'numeroFactura' ).setValue( this.solicitudPagoFaseFactura.numero !== undefined ? this.solicitudPagoFaseFactura.numero : null );
            this.addressForm.get( 'fechaFactura' ).setValue( this.solicitudPagoFaseFactura.fecha !== undefined ? new Date( this.solicitudPagoFaseFactura.fecha ) : null );
            this.addressForm.get('aplicaDescuento').setValue(this.solicitudPagoFaseFactura.tieneDescuento !== undefined ? this.solicitudPagoFaseFactura.tieneDescuento : null);
            this.addressForm.get('numeroDescuentos').setValue(this.solicitudPagoFaseFacturaDescuento.length > 0 ? `${this.solicitudPagoFaseFacturaDescuento.length}` : '');
            this.addressForm.get('valorAPagarDespues').setValue(this.solicitudPagoFaseFactura.valorFacturadoConDescuento !== undefined ? this.solicitudPagoFaseFactura.valorFacturadoConDescuento : null);

            for (const descuento of this.solicitudPagoFaseFacturaDescuento) {
                this.descuentos.markAllAsTouched();
                this.descuentos.push(
                    this.fb.group(
                        {
                            solicitudPagoFaseFacturaDescuentoId: [descuento.solicitudPagoFaseFacturaDescuentoId],
                            solicitudPagoFaseFacturaId: [descuento.solicitudPagoFaseFacturaId],
                            tipoDescuentoCodigo: [descuento.tipoDescuentoCodigo],
                            valorDescuento: [descuento.valorDescuento]
                        }
                    )
                );
            }

            if ( this.solicitudPagoFaseFactura.registroCompleto === true && this.tieneObservacionOrdenGiro === undefined ) {
                this.addressForm.disable();
                this.descuentos.disable();
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
        this.addressForm.get('numeroDescuentos').valueChanges
            .subscribe(
                value => {
                    value = Number(value);
                    if ( value <= this.listaTipoDescuento.length ) {
                        if (this.solicitudPagoFaseFactura !== undefined && this.solicitudPagoFaseFacturaDescuento.length > 0) {
                            if (value > 0) {
                                this.descuentos.clear();
                                for (const descuento of this.solicitudPagoFaseFacturaDescuento) {
                                    this.estaEditando = true;
                                    this.descuentos.markAllAsTouched();
                                    this.descuentos.push(
                                        this.fb.group(
                                            {
                                                solicitudPagoFaseFacturaDescuentoId: [descuento.solicitudPagoFaseFacturaDescuentoId],
                                                solicitudPagoFaseFacturaId: [descuento.solicitudPagoFaseFacturaId],
                                                tipoDescuentoCodigo: [descuento.tipoDescuentoCodigo],
                                                valorDescuento: [descuento.valorDescuento]
                                            }
                                        )
                                    );
                                }
    
                                this.addressForm.get('numeroDescuentos').setValidators(Validators.min(this.descuentos.length));
                                const nuevosDescuentos = value - this.descuentos.length;
                                if (value < this.descuentos.length) {
                                    this.openDialog(
                                        '', '<b>Debe eliminar uno de los registros diligenciados para disminuir el total de los registros requeridos.</b>'
                                    );
                                    this.addressForm.get('numeroDescuentos').setValue(String(this.descuentos.length));
                                    return;
                                }
                                for (let i = 0; i < nuevosDescuentos; i++) {
                                    this.estaEditando = true;
                                    this.descuentos.markAllAsTouched();
                                    this.descuentos.push(
                                        this.fb.group(
                                            {
                                                solicitudPagoFaseFacturaDescuentoId: [0],
                                                solicitudPagoFaseFacturaId: [this.solicitudPagoFaseFacturaId],
                                                tipoDescuentoCodigo: [null],
                                                valorDescuento: [null]
                                            }
                                        )
                                    );
                                }
                            }
                        } else {
                            if (value > 0) {
                                if (this.descuentos.dirty === true) {
                                    this.addressForm.get('numeroDescuentos').setValidators(Validators.min(this.descuentos.length));
                                    const nuevosDescuentos = value - this.descuentos.length;
                                    if (value < this.descuentos.length) {
                                        this.openDialog(
                                            '', '<b>Debe eliminar uno de los registros diligenciados para disminuir el total de los registros requeridos.</b>'
                                        );
                                        this.addressForm.get('numeroDescuentos').setValue(String(this.descuentos.length));
                                        return;
                                    }
                                    for (let i = 0; i < nuevosDescuentos; i++) {
                                        this.estaEditando = true;
                                        this.descuentos.markAllAsTouched();
                                        this.descuentos.push(
                                            this.fb.group(
                                                {
                                                    solicitudPagoFaseFacturaDescuentoId: [0],
                                                    solicitudPagoFaseFacturaId: [this.solicitudPagoFaseFacturaId],
                                                    tipoDescuentoCodigo: [null],
                                                    valorDescuento: [null]
                                                }
                                            )
                                        );
                                    }
                                }
                                if (this.descuentos.dirty === false) {
                                    this.descuentos.clear();
                                    for (let i = 0; i < value; i++) {
                                        this.estaEditando = true;
                                        this.descuentos.markAllAsTouched();
                                        this.descuentos.push(
                                            this.fb.group(
                                                {
                                                    solicitudPagoFaseFacturaDescuentoId: [0],
                                                    solicitudPagoFaseFacturaId: [this.solicitudPagoFaseFacturaId],
                                                    tipoDescuentoCodigo: [null],
                                                    valorDescuento: [null]
                                                }
                                            )
                                        );
                                    }
                                }
                            }
                        }
                    } else {
                        this.addressForm.get('numeroDescuentos').setValue( String( this.listaTipoDescuento.length ) );
                        this.openDialog( '', `<b>Tiene parametrizados ${ this.listaTipoDescuento.length } descuentos para aplicar al pago.</b>` )
                    }
                }
            );
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

    getTipoDescuentoSeleccionado( codigo: string ): Dominio[] {
        if ( this.listaTipoDescuento.length > 0 ) {
            const descuento = this.listaTipoDescuento.find( descuento => descuento.codigo === codigo );

            if ( descuento !== undefined ) {
                return [ descuento ];
            }
        }
    }

    getCodigoTipoDescuento( codigo: string ) {
        const descuentoIndex = this.tiposDescuentoArray.findIndex( descuento => descuento.codigo === codigo );

        if ( descuentoIndex !== -1 ) {
            this.tiposDescuentoArray.splice( descuentoIndex, 1 );
        }
    }

    totalPagoDescuentos() {
        let totalDescuentos = 0;
        let valorDespuesDescuentos = 0;
        this.descuentos.controls.forEach(control => {
            if (control.value.valorDescuento !== null) {
                totalDescuentos += control.value.valorDescuento;
            }
        });
        if (totalDescuentos > 0) {
            if (totalDescuentos > this.valorFacturado) {
                this.openDialog('', '<b>El valor total de los descuentos es mayor al valor facturado.</b>');
                this.addressForm.get('valorAPagarDespues').setValue(null);
                return;
            } else {
                valorDespuesDescuentos = this.valorFacturado - totalDescuentos;
                this.addressForm.get('valorAPagarDespues').setValue(valorDespuesDescuentos);
                return;
            }
        } else {
            this.addressForm.get('valorAPagarDespues').setValue(null);
            return;
        }
    }

    addDescuento() {
        if ( this.tiposDescuentoArray.length > 0 ) {
            this.descuentos.push(
                this.fb.group(
                    {
                        solicitudPagoFaseFacturaDescuentoId: [0],
                        solicitudPagoFaseFacturaId: [this.solicitudPagoFaseFacturaId],
                        tipoDescuentoCodigo: [null],
                        valorDescuento: [null]
                    }
                )
            );
            this.addressForm.get( 'numeroDescuentos' ).setValidators( Validators.min( this.descuentos.length ) );
            this.addressForm.get( 'numeroDescuentos' ).setValue( String( this.descuentos.length ) );
        } else {
            this.openDialog( '', '<b>No tiene parametrizados más descuentos para aplicar al pago.</b>' )
        }
    }

    deleteDescuento(index: number, descuentoId: number) {
        this.openDialogTrueFalse('', '<b>¿Está seguro de eliminar esta información?</b>')
            .subscribe(
                response => {
                    if (response === true) {
                        if (descuentoId === 0) {
                            const codigo: string = this.descuentos.controls[ index ].get( 'tipoDescuentoCodigo' ).value;
                            const descuento = this.listaTipoDescuento.find( descuento => descuento.codigo === codigo );

                            if ( descuento !== undefined ) {
                                this.tiposDescuentoArray.push( descuento );
                            }

                            this.descuentos.removeAt(index);
                            this.addressForm.get( 'numeroDescuentos' ).setValue( `${ this.descuentos.length }` );
                            this.openDialog('', '<b>La información se ha eliminado correctamente.</b>');
                        } else {
                            this.registrarPagosSvc.deleteSolicitudPagoFaseFacturaDescuento(descuentoId)
                                .subscribe(
                                    () => {
                                        this.openDialog('', '<b>La información se ha eliminado correctamente.</b>');
                                        this.routes.navigateByUrl('/', { skipLocationChange: true }).then(
                                            () => this.routes.navigate(
                                                [
                                                    '/registrarValidarRequisitosPago/verDetalleEditar', this.solicitudPago.contratoId, this.solicitudPago.solicitudPagoId
                                                ]
                                            )
                                        );
                                    },
                                    err => this.openDialog('', `<b>${err.message}</b>`)
                                );
                        }
                    }
                }
            );
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
        this.descuentos.markAllAsTouched();
        const getSolicitudPagoFaseFacturaDescuento = () => {
            if (this.descuentos.length > 0) {
                if (this.addressForm.get('aplicaDescuento').value === true) {
                    return this.descuentos.value;
                } else {
                    return [];
                }
            } else {
                return [];
            }
        }

        const solicitudPagoFaseFactura = [
            {
                solicitudPagoFaseFacturaId: this.solicitudPagoFaseFacturaId,
                solicitudPagoFaseId: this.solicitudPagoFase.solicitudPagoFaseId,
                fecha: this.addressForm.get( 'fechaFactura' ).value !== null ? new Date( this.addressForm.get( 'fechaFactura' ).value ).toISOString() : this.addressForm.get( 'fechaFactura' ).value,
                valorFacturado: this.valorFacturado,
                numero: this.addressForm.get( 'numeroFactura' ).value,
                tieneDescuento: this.addressForm.get('aplicaDescuento').value,
                valorFacturadoConDescuento: this.addressForm.get('valorAPagarDespues').value,
                solicitudPagoFaseFacturaDescuento: getSolicitudPagoFaseFacturaDescuento()
            }
        ]
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
