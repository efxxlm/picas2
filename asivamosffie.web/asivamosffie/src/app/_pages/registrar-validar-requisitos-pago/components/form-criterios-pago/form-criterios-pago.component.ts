import { Component, Input, OnInit } from '@angular/core';
import { async } from '@angular/core/testing';
import { FormBuilder, Validators, FormArray } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { RegistrarRequisitosPagoService } from 'src/app/core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-form-criterios-pago',
  templateUrl: './form-criterios-pago.component.html',
  styleUrls: ['./form-criterios-pago.component.scss']
})
export class FormCriteriosPagoComponent implements OnInit {

    @Input() solicitudPago: any;
    @Input() esPreconstruccion = true;
    @Input() contratacionProyectoId: number;
    solicitudPagoRegistrarSolicitudPago: any;
    solicitudPagoFase: any;
    addressForm = this.fb.group({
        criterioPago: [null, Validators.required],
        criterios: this.fb.array( [] )
    });
    criteriosArray: { codigo: string, nombre: string }[] = [];
    obj1: boolean;

    get criterios() {
        return this.addressForm.get( 'criterios' ) as FormArray;
    }

    constructor(
        private fb: FormBuilder,
        private dialog: MatDialog,
        private routes: Router,
        private registrarPagosSvc: RegistrarRequisitosPagoService )
    { }

    ngOnInit(): void {
        this.getCriterios();
    }

    getCriterios() {
        const solicitudPagoCargarFormaPago = this.solicitudPago.solicitudPagoCargarFormaPago[0];
        if ( this.esPreconstruccion === true ) {
            const fasePreConstruccionFormaPagoCodigo = solicitudPagoCargarFormaPago.fasePreConstruccionFormaPagoCodigo;
            this.registrarPagosSvc.getCriterioByFormaPagoCodigo( fasePreConstruccionFormaPagoCodigo )
                .subscribe(
                    response => {
                        const criteriosArray = [];
                        this.solicitudPagoRegistrarSolicitudPago = this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0];
                        this.solicitudPagoFase = this.solicitudPagoRegistrarSolicitudPago.solicitudPagoFase[0];

                        if ( this.solicitudPagoFase.solicitudPagoFaseCriterio.length > 0 ) {
                            this.solicitudPagoFase.solicitudPagoFaseCriterio.forEach( async criterio => {
                                // GET Criterio seleccionado
                                const criterioSeleccionado = response.filter( value => value.codigo === criterio.tipoCriterioCodigo );
                                criteriosArray.push( criterioSeleccionado[0] );
                                // GET tipos de pago
                                const tiposDePago = await this.registrarPagosSvc.getTipoPagoByCriterioCodigo( criterio.tipoCriterioCodigo );
                                const tipoDePago = tiposDePago.filter( value => value.codigo === criterio.tipoPagoCodigo );
                                // GET conceptos de pago
                                const conceptosDePago = await this.registrarPagosSvc.getConceptoPagoCriterioCodigoByTipoPagoCodigo( criterio.tipoPagoCodigo );
                                const conceptoDePago = conceptosDePago.filter( value => value.codigo === criterio.conceptoPagoCriterio );
                                this.criterios.push(
                                    this.fb.group(
                                        {
                                            solicitudPagoFaseId: [ this.solicitudPagoFase.solicitudPagoFaseId ],
                                            solicitudPagoFaseCriterioId: [ criterio.solicitudPagoFaseCriterioId ],
                                            tipoCriterioCodigo: [ criterio.tipoCriterioCodigo ],
                                            nombreCriterio: [ criterioSeleccionado[0].nombre ],
                                            tiposDePago: [ tiposDePago ],
                                            tipoPago: [ tipoDePago[0], Validators.required ],
                                            conceptosDePago: [ conceptosDePago, Validators.required ],
                                            conceptoPago: [ conceptoDePago[0], Validators.required ],
                                            valorFacturado: [ criterio.valorFacturado, Validators.required ]
                                        }
                                    )
                                );
                            } );
                        }
                        this.criteriosArray = response;
                        this.addressForm.get( 'criterioPago' ).setValue( criteriosArray );
                    }
                );
        }
        if ( this.esPreconstruccion === false ) {
            const faseConstruccionFormaPagoCodigo = solicitudPagoCargarFormaPago.faseConstruccionFormaPagoCodigo;
            this.registrarPagosSvc.getCriterioByFormaPagoCodigo( faseConstruccionFormaPagoCodigo )
                .subscribe(
                    response => {
                        const criteriosArray = [];
                        this.solicitudPagoRegistrarSolicitudPago = this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0];
                        this.solicitudPagoFase = this.solicitudPagoRegistrarSolicitudPago.solicitudPagoFase[0];
                        if ( this.solicitudPagoFase.solicitudPagoFaseCriterio.length > 0 ) {
                            this.solicitudPagoFase.solicitudPagoFaseCriterio.forEach( async criterio => {
                                // GET Criterio seleccionado
                                const criterioSeleccionado = response.filter( value => value.codigo === criterio.tipoCriterioCodigo );
                                criteriosArray.push( criterioSeleccionado[0] );
                                // GET tipos de pago
                                const tiposDePago = await this.registrarPagosSvc.getTipoPagoByCriterioCodigo( criterio.tipoCriterioCodigo );
                                const tipoDePago = tiposDePago.filter( value => value.codigo === criterio.tipoPagoCodigo );
                                // GET conceptos de pago
                                const conceptosDePago = await this.registrarPagosSvc.getConceptoPagoCriterioCodigoByTipoPagoCodigo( criterio.tipoPagoCodigo );
                                const conceptoDePago = conceptosDePago.filter( value => value.codigo === criterio.conceptoPagoCriterio );
                                this.criterios.push(
                                    this.fb.group(
                                        {
                                            solicitudPagoFaseId: [ this.solicitudPagoFase.solicitudPagoFaseId ],
                                            solicitudPagoFaseCriterioId: [ criterio.solicitudPagoFaseCriterioId ],
                                            tipoCriterioCodigo: [ criterio.tipoCriterioCodigo ],
                                            nombreCriterio: [ criterioSeleccionado[0].nombre ],
                                            tiposDePago: [ tiposDePago ],
                                            tipoPago: [ tipoDePago[0], Validators.required ],
                                            conceptosDePago: [ conceptosDePago, Validators.required ],
                                            conceptoPago: [ conceptoDePago[0], Validators.required ],
                                            valorFacturado: [ criterio.valorFacturado, Validators.required ]
                                        }
                                    )
                                );
                            } );
                        }
                        this.criteriosArray = response;
                        this.addressForm.get( 'criterioPago' ).setValue( criteriosArray );
                    }
                );
        }
    }

    validateNumberKeypress(event: KeyboardEvent) {
        const alphanumeric = /[0-9]/;
        const inputChar = String.fromCharCode(event.charCode);

        return alphanumeric.test(inputChar) ? true : false;
    }

    getvalues( criteriovalues: { codigo: string, nombre: string }[] ) {
        const values = [ ...criteriovalues ];
        if ( values.length > 0 ) {
            const criteriosSeleccionados = this.addressForm.get( 'criterioPago' ).value;
            if ( this.addressForm.get( 'criterios' ).dirty === true ) {
                this.criterios.controls.forEach( ( criterio, indexValue ) => {
                    values.forEach( ( value, index ) => {
                        if ( value.codigo === criterio.value.tipoCriterioCodigo ) {
                            values.splice( index, 1 );
                        }
                    } );
                    const test = criteriovalues.filter( value => value.codigo === criterio.value.tipoCriterioCodigo );
                    if ( test.length === 0 ) {
                        this.criterios.removeAt( indexValue );
                    }
                } );
                values.forEach( async value => {
                    if ( this.criterios.controls.filter( control => control.value.tipoCriterioCodigo === value.codigo ).length === 0 ) {
                        const tiposDePago = await this.registrarPagosSvc.getTipoPagoByCriterioCodigo( value.codigo );
                        this.criterios.push(
                            this.fb.group(
                                {
                                    solicitudPagoFaseId: [ this.solicitudPagoFase.solicitudPagoFaseId ],
                                    solicitudPagoFaseCriterioId: [ 0 ],
                                    tipoCriterioCodigo: [ value.codigo ],
                                    nombreCriterio: [ value.nombre ],
                                    tiposDePago: [ tiposDePago ],
                                    tipoPago: [ null, Validators.required ],
                                    conceptosDePago: [ [], Validators.required ],
                                    conceptoPago: [ null, Validators.required ],
                                    valorFacturado: [ null, Validators.required ]
                                }
                            )
                        );
                    }
                } );
                this.addressForm.get( 'criterioPago' ).setValue( criteriosSeleccionados );
            }
            if ( this.addressForm.get( 'criterios' ).dirty === false ) {

                if ( this.solicitudPagoFase.solicitudPagoFaseCriterio.length > 0 ) {
                    this.criterios.controls.forEach( ( criterio, indexValue ) => {
                        values.forEach( ( value, index ) => {
                            if ( value.codigo === criterio.value.tipoCriterioCodigo ) {
                                values.splice( index, 1 );
                            }
                        } );
                        const test = criteriovalues.filter( value => value.codigo === criterio.value.tipoCriterioCodigo );
                        if ( test.length === 0 ) {
                            this.criterios.removeAt( indexValue );
                        }
                    } );

                    values.forEach( async value => {
                        if ( this.criterios.controls.filter( control => control.value.tipoCriterioCodigo === value.codigo ).length === 0 ) {
                            const tiposDePago = await this.registrarPagosSvc.getTipoPagoByCriterioCodigo( value.codigo );
                            this.criterios.push(
                                this.fb.group(
                                    {
                                        solicitudPagoFaseId: [ this.solicitudPagoFase.solicitudPagoFaseId ],
                                        solicitudPagoFaseCriterioId: [ 0 ],
                                        tipoCriterioCodigo: [ value.codigo ],
                                        nombreCriterio: [ value.nombre ],
                                        tiposDePago: [ tiposDePago ],
                                        tipoPago: [ null, Validators.required ],
                                        conceptosDePago: [ [], Validators.required ],
                                        conceptoPago: [ null, Validators.required ],
                                        valorFacturado: [ null, Validators.required ]
                                    }
                                )
                            );
                        }
                    } );
                } else {
                    this.criterios.clear();
                    values.forEach( async value => {
                        const tiposDePago = await this.registrarPagosSvc.getTipoPagoByCriterioCodigo( value.codigo );
                        this.criterios.push(
                            this.fb.group(
                                {
                                    solicitudPagoFaseId: [ this.solicitudPagoFase.solicitudPagoFaseId ],
                                    solicitudPagoFaseCriterioId: [ 0 ],
                                    tipoCriterioCodigo: [ value.codigo ],
                                    nombreCriterio: [ value.nombre ],
                                    tiposDePago: [ tiposDePago ],
                                    tipoPago: [ null, Validators.required ],
                                    conceptosDePago: [ [], Validators.required ],
                                    conceptoPago: [ null, Validators.required ],
                                    valorFacturado: [ null, Validators.required ]
                                }
                            )
                        );
                    } );
                }
            }
        } else {
            this.criterios.clear();
        }
    }

    async getConceptosDePago( index: number, tipoPago: any ) {
        const conceptosDePago = await this.registrarPagosSvc.getConceptoPagoCriterioCodigoByTipoPagoCodigo( tipoPago.codigo );
        console.log( conceptosDePago );
        this.criterios.controls[ index ].get( 'conceptosDePago' ).setValue( conceptosDePago );
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

    deleteCriterio( index: number, solicitudPagoFaseCriterioId: number, tipoCriterioCodigo: string ) {
        this.openDialogTrueFalse( '', '<b>¿Está seguro de eliminar esta información?</b>' )
            .subscribe(
                value => {
                    if ( value === true ) {
                        if ( solicitudPagoFaseCriterioId === 0 ) {
                            this.criterios.removeAt( index );
                            const criteriosSeleccionados = this.addressForm.get( 'criterioPago' ).value;
                            if ( criteriosSeleccionados !== null && criteriosSeleccionados.length > 0 ) {
                                criteriosSeleccionados.forEach( ( criterioValue, index ) => {
                                    if ( criterioValue.codigo === tipoCriterioCodigo ) {
                                        criteriosSeleccionados.splice( index, 1 );
                                    }
                                } );
                            }
                            this.addressForm.get( 'criterioPago' ).setValue( criteriosSeleccionados );
                            this.openDialog( '', '<b>La información se ha eliminado correctamente.</b>' );
                        } else {
                            this.criterios.removeAt( index );
                            const criteriosSeleccionados = this.addressForm.get( 'criterioPago' ).value;
                            if ( criteriosSeleccionados !== null && criteriosSeleccionados.length > 0 ) {
                                criteriosSeleccionados.forEach( ( criterioValue, index ) => {
                                    if ( criterioValue.codigo === tipoCriterioCodigo ) {
                                        criteriosSeleccionados.splice( index, 1 );
                                    }
                                } );
                            }
                            this.addressForm.get( 'criterioPago' ).setValue( criteriosSeleccionados );
                            this.registrarPagosSvc.deleteSolicitudPagoFaseCriterio( solicitudPagoFaseCriterioId )
                                .subscribe(
                                    () => this.openDialog( '', '<b>La información se ha eliminado correctamente.</b>' ),
                                    err => this.openDialog( '', `<b>${ err.message }</b>` )
                                )
                        }
                    }
                }
            );
    }

    guardar() {
        if ( this.contratacionProyectoId === 0 ) {
            this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase[0].solicitudPagoFaseCriterio = [];
            this.criterios.controls.forEach( control => {
                const criterio = control.value;
                this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase[0].solicitudPagoFaseCriterio.push(
                    {
                        tipoCriterioCodigo: criterio.tipoCriterioCodigo,
                        solicitudPagoFaseCriterioId: criterio.solicitudPagoFaseCriterioId,
                        tipoPagoCodigo: criterio.tipoPago.codigo,
                        conceptoPagocriterio: criterio.conceptoPago.codigo,
                        valorFacturado: criterio.valorFacturado,
                        solicitudPagoFaseId: criterio.solicitudPagoFaseId
                    }
                );
            } );
            this.registrarPagosSvc.createEditNewPayment( this.solicitudPago )
                .subscribe(
                    response => {
                        this.openDialog( '', `<b>${ response.message }</b>` );
                        this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                            () => this.routes.navigate(
                                [
                                    '/registrarValidarRequisitosPago/verDetalleEditar', this.solicitudPago.contratoId
                                ]
                            )
                        );
                    },
                    err => this.openDialog( '', `<b>${ err.message }</b>` )
                );
        } else {
            this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase[0].solicitudPagoFaseCriterio = [];
            this.criterios.controls.forEach( control => {
                const criterio = control.value;
                const solicitudPagoFaseCriterioProyecto = [];
                solicitudPagoFaseCriterioProyecto.push(
                    {
                        contratacionProyectoId: this.contratacionProyectoId,
                        solicitudPagoFaseCriterioId: criterio.solicitudPagoFaseCriterioId,
                        valorFacturado: criterio.valorFacturado
                    }
                );
                this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase[0].solicitudPagoFaseCriterio.push(
                    {
                        tipoCriterioCodigo: criterio.tipoCriterioCodigo,
                        solicitudPagoFaseCriterioId: criterio.solicitudPagoFaseCriterioId,
                        tipoPagoCodigo: criterio.tipoPago.codigo,
                        conceptoPagocriterio: criterio.conceptoPago.codigo,
                        valorFacturado: criterio.valorFacturado,
                        solicitudPagoFaseId: criterio.solicitudPagoFaseId,
                        solicitudPagoFaseCriterioProyecto
                    }
                );
            } );
            this.registrarPagosSvc.createEditNewPayment( this.solicitudPago )
                .subscribe(
                    response => {
                        this.openDialog( '', `<b>${ response.message }</b>` );
                        this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                            () => this.routes.navigate(
                                [
                                    '/registrarValidarRequisitosPago/verDetalleEditar', this.solicitudPago.contratoId
                                ]
                            )
                        );
                    },
                    err => this.openDialog( '', `<b>${ err.message }</b>` )
                );
        }
    }

}
