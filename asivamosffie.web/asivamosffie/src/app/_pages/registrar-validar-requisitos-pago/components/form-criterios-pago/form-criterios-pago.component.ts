import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { async } from '@angular/core/testing';
import { FormBuilder, Validators, FormArray, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { Dominio } from 'src/app/core/_services/common/common.service';
import { ObservacionesMultiplesCuService } from 'src/app/core/_services/observacionesMultiplesCu/observaciones-multiples-cu.service';
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
    @Input() solicitudPagoCargarFormaPago: any;
    @Input() tieneObservacion: boolean;
    @Input() listaMenusId: any;
    @Input() criteriosPagoFacturaCodigo: string;
    @Input() tieneObservacionOrdenGiro: boolean;
    @Input() esVerDetalle = false;
    @Output() semaforoObservacion = new EventEmitter<boolean>();
    criteriosSeleccionadosArray: Dominio[] = [];
    solicitudPagoRegistrarSolicitudPago: any;
    registroCompletoCriterio = false;
    solicitudPagoFase: any;
    esAutorizar: boolean;
    observacion: any;
    solicitudPagoObservacionId = 0;
    btnBoolean = false;
    montoMaximoPendiente: number = 0;
    addressForm = this.fb.group({
        criterioPago: [ null, Validators.required ],
        criterios: this.fb.array( [] )
    });
    criteriosArray: { codigo: string, nombre: string, porcentaje: number }[] = [];
    estaEditando = false;

    get criterios() {
        return this.addressForm.get( 'criterios' ) as FormArray;
    }

    constructor(
        private fb: FormBuilder,
        private dialog: MatDialog,
        private routes: Router,
        private obsMultipleSvc: ObservacionesMultiplesCuService,
        private registrarPagosSvc: RegistrarRequisitosPagoService )
    {
        setTimeout(() => {
            if ( this.tieneObservacionOrdenGiro === undefined ) {
                if ( this.registroCompletoCriterio === true && this.tieneObservacion === false ) {
                    this.addressForm.controls[ 'criterioPago' ].disable();
                }
            }
        }, 2000 );
    }

    ngOnInit(): void {
        this.getCriterios();
    }

    getConceptos( index: number ) {
        return this.criterios.controls[ index ].get( 'conceptos' ) as FormArray;
    }

    getCriterios() {
        
        if ( this.esPreconstruccion === true ) {
            const fasePreConstruccionFormaPagoCodigo = this.solicitudPagoCargarFormaPago.fasePreConstruccionFormaPagoCodigo;

            this.registrarPagosSvc.getCriterioByFormaPagoCodigo( fasePreConstruccionFormaPagoCodigo )
                .subscribe(
                    async response => {
                        const montoMaximoPendiente = await this.registrarPagosSvc.getMontoMaximo( this.solicitudPago.solicitudPagoId, 'True' ).toPromise();
                        this.montoMaximoPendiente = montoMaximoPendiente.valorPendientePorPagar;

                        this.solicitudPagoRegistrarSolicitudPago = this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0];

                        if ( this.solicitudPagoRegistrarSolicitudPago.solicitudPagoFase.length > 0 ) {
                            for ( const solicitudPagoFase of this.solicitudPagoRegistrarSolicitudPago.solicitudPagoFase ) {
                                if ( solicitudPagoFase.esPreconstruccion === true ) {
                                    this.solicitudPagoFase = solicitudPagoFase;
                                }
                            }
                        }

                        if ( response.length > 0 ) {
                            for ( const criterioValue of response ) {
                                const nombre = criterioValue.nombre.trim().split( ' ' );
                                const porcentaje = nombre[ nombre.length - 1 ].replace( '%', '' );
                                const porcentajeCriterio = Number( porcentaje ) / 100;

                                criterioValue.porcentaje = porcentajeCriterio;
                            }
                        }

                        this.registroCompletoCriterio = this.solicitudPagoFase.registroCompletoCriterio;

                        if ( this.solicitudPagoFase.solicitudPagoFaseCriterio.length > 0 ) {
                            this.estaEditando = true;
                            this.addressForm.markAllAsTouched();
                            this.criterios.markAllAsTouched();
                            for ( const criterio of this.solicitudPagoFase.solicitudPagoFaseCriterio ) {
                                // GET Criterio seleccionado
                                const criterioSeleccionado = response.filter( value => value.codigo === criterio.tipoCriterioCodigo );
                                this.criteriosSeleccionadosArray.push( criterioSeleccionado[0] );
                                // GET tipos de pago
                                const tiposDePago = await this.registrarPagosSvc.getTipoPagoByCriterioCodigo( criterio.tipoCriterioCodigo );
                                const tipoDePago = tiposDePago.filter( value => value.codigo === criterio.tipoPagoCodigo );
                                // GET conceptos de pago
                                const conceptosDePago = await this.registrarPagosSvc.getConceptoPagoCriterioCodigoByTipoPagoCodigo( criterio.tipoPagoCodigo );
                                const conceptoDePagoArray = [];
                                const conceptosDePagoSeleccionados = [];
                                // Get conceptos de pago
                                if ( criterio.solicitudPagoFaseCriterioConceptoPago.length > 0 ) {
                                    criterio.solicitudPagoFaseCriterioConceptoPago.forEach( solicitudPagoFaseCriterioConceptoPago => {
                                        const conceptoFind = conceptosDePago.find( concepto => concepto.codigo === solicitudPagoFaseCriterioConceptoPago.conceptoPagoCriterio );

                                        if ( conceptoFind !== undefined ) {
                                            conceptosDePagoSeleccionados.push( conceptoFind );
                                            conceptoDePagoArray.push(
                                                this.fb.group(
                                                    {
                                                        solicitudPagoFaseCriterioConceptoPagoId: [ solicitudPagoFaseCriterioConceptoPago.solicitudPagoFaseCriterioConceptoPagoId ],
                                                        solicitudPagoFaseCriterioId: [ criterio.solicitudPagoFaseCriterioId ],
                                                        conceptoPagoCriterioNombre: [ conceptoFind.nombre ],
                                                        conceptoPagoCriterio: [ solicitudPagoFaseCriterioConceptoPago.conceptoPagoCriterio ],
                                                        valorFacturadoConcepto: [ solicitudPagoFaseCriterioConceptoPago.valorFacturadoConcepto !== undefined ? solicitudPagoFaseCriterioConceptoPago.valorFacturadoConcepto : null ]
                                                    }
                                                )
                                            );
                                        }
                                    } );
                                }

                                const montoMaximo = ( this.montoMaximoPendiente * criterioSeleccionado[0].porcentaje );

                                this.criterios.push(
                                    this.fb.group(
                                        {
                                            solicitudPagoFaseId: [ this.solicitudPagoFase.solicitudPagoFaseId ],
                                            solicitudPagoFaseCriterioId: [ criterio.solicitudPagoFaseCriterioId ],
                                            montoMaximo: [ montoMaximo ],
                                            tipoCriterioCodigo: [ criterio.tipoCriterioCodigo ],
                                            nombreCriterio: [ criterioSeleccionado[0].nombre ],
                                            tiposDePago: [ tiposDePago ],
                                            tipoPago: [ tipoDePago.length > 0 ? tipoDePago[0] : null ],
                                            conceptosDePago: [ conceptosDePago ],
                                            conceptoPago: [ conceptosDePagoSeleccionados, Validators.required ],
                                            conceptos: this.fb.array( conceptoDePagoArray ),
                                            valorFacturado: [ { value: criterio.valorFacturado !== undefined ? criterio.valorFacturado : null, disabled: true }, Validators.required ]
                                        }
                                    )
                                );
                            }

                            if ( this.esVerDetalle === false ) {
                                // Get observacion CU autorizar solicitud de pago 4.1.9
                                this.obsMultipleSvc.getObservacionSolicitudPagoByMenuIdAndSolicitudPagoId(
                                    this.listaMenusId.autorizarSolicitudPagoId,
                                    this.solicitudPago.solicitudPagoId,
                                    this.solicitudPagoFase.solicitudPagoFaseCriterio[0].solicitudPagoFaseCriterioId,
                                    this.criteriosPagoFacturaCodigo )
                                    .subscribe(
                                        response => {
                                            const observacion = response.find( obs => obs.archivada === false );

                                            if ( observacion !== undefined ) {
                                                this.esAutorizar = true;
                                                this.observacion = observacion;
                                                this.criterios.enable();
                                                if ( this.observacion.tieneObservacion === true ) {
                                                    this.semaforoObservacion.emit( true );
                                                    setTimeout(() => {
                                                        this.addressForm.controls[ 'criterioPago' ].enable()
                                                    }, 1500);
                                                }

                                                this.solicitudPagoObservacionId = observacion.solicitudPagoObservacionId;
                                            }
                                        }
                                    );
                                // Get observacion CU verificar solicitud de pago 4.1.8
                                this.obsMultipleSvc.getObservacionSolicitudPagoByMenuIdAndSolicitudPagoId(
                                    this.listaMenusId.aprobarSolicitudPagoId,
                                    this.solicitudPago.solicitudPagoId,
                                    this.solicitudPagoFase.solicitudPagoFaseCriterio[0].solicitudPagoFaseCriterioId,
                                    this.criteriosPagoFacturaCodigo )
                                    .subscribe(
                                        response => {
                                            const observacion = response.find( obs => obs.archivada === false );
                                            if ( observacion !== undefined ) {
                                                this.esAutorizar = false;
                                                this.observacion = observacion;
                                                this.criterios.enable();

                                                if ( this.observacion.tieneObservacion === true ) {
                                                    this.semaforoObservacion.emit( true );
                                                    setTimeout(() => {
                                                        this.addressForm.controls[ 'criterioPago' ].enable()
                                                    }, 1500);
                                                }

                                                this.solicitudPagoObservacionId = observacion.solicitudPagoObservacionId;
                                            }
                                        }
                                    );
                            }
                        }
                        this.criteriosArray = response;
                        this.addressForm.get( 'criterioPago' ).setValue( this.criteriosSeleccionadosArray.length > 0 ? this.criteriosSeleccionadosArray : null );
                        if ( this.registroCompletoCriterio === true && this.tieneObservacion === false && this.tieneObservacionOrdenGiro === undefined ) {
                            this.criterios.disable();
                        }
                    }
                );
        }
        if ( this.esPreconstruccion === false ) {
            const faseConstruccionFormaPagoCodigo = this.solicitudPagoCargarFormaPago.faseConstruccionFormaPagoCodigo;

            this.registrarPagosSvc.getCriterioByFormaPagoCodigo( faseConstruccionFormaPagoCodigo )
                .subscribe(
                    async response => {
                        const montoMaximoPendiente = await this.registrarPagosSvc.getMontoMaximo( this.solicitudPago.solicitudPagoId, 'False' ).toPromise();
                        this.montoMaximoPendiente = montoMaximoPendiente.valorPendientePorPagar;

                        this.solicitudPagoRegistrarSolicitudPago = this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0];

                        if ( this.solicitudPagoRegistrarSolicitudPago.solicitudPagoFase.length > 0 ) {
                            for ( const solicitudPagoFase of this.solicitudPagoRegistrarSolicitudPago.solicitudPagoFase ) {
                                if ( solicitudPagoFase.esPreconstruccion === false ) {
                                    this.solicitudPagoFase = solicitudPagoFase;
                                }
                            }
                        }

                        this.registroCompletoCriterio = this.solicitudPagoFase.registroCompletoCriterio;

                        if ( response.length > 0 ) {
                            for ( const criterioValue of response ) {
                                const nombre = criterioValue.nombre.split( ' ' );
                                const porcentaje = nombre[ nombre.length - 1 ].replace( '%', '' );
                                const porcentajeCriterio = Number( porcentaje ) / 100;

                                criterioValue.porcentaje = porcentajeCriterio;
                            }
                        }

                        if ( this.solicitudPagoFase.solicitudPagoFaseCriterio.length > 0 ) {
                            this.estaEditando = true;
                            this.addressForm.markAllAsTouched();
                            this.criterios.markAllAsTouched();
                            for ( const criterio of this.solicitudPagoFase.solicitudPagoFaseCriterio ) {
                                // GET Criterio seleccionado
                                const criterioSeleccionado = response.filter( value => value.codigo === criterio.tipoCriterioCodigo );
                                this.criteriosSeleccionadosArray.push( criterioSeleccionado[0] );
                                // GET tipos de pago
                                const tiposDePago = await this.registrarPagosSvc.getTipoPagoByCriterioCodigo( criterio.tipoCriterioCodigo );
                                const tipoDePago = tiposDePago.filter( value => value.codigo === criterio.tipoPagoCodigo );
                                // GET conceptos de pago
                                const conceptosDePago = await this.registrarPagosSvc.getConceptoPagoCriterioCodigoByTipoPagoCodigo( criterio.tipoPagoCodigo );
                                const conceptoDePagoArray = [];
                                const conceptosDePagoSeleccionados = [];
                                // Get conceptos de pago
                                if ( criterio.solicitudPagoFaseCriterioConceptoPago.length > 0 ) {
                                    criterio.solicitudPagoFaseCriterioConceptoPago.forEach( solicitudPagoFaseCriterioConceptoPago => {
                                        const conceptoFind = conceptosDePago.find( concepto => concepto.codigo === solicitudPagoFaseCriterioConceptoPago.conceptoPagoCriterio );

                                        if ( conceptoFind !== undefined ) {
                                            conceptosDePagoSeleccionados.push( conceptoFind );
                                            conceptoDePagoArray.push(
                                                this.fb.group(
                                                    {
                                                        solicitudPagoFaseCriterioConceptoPagoId: [ solicitudPagoFaseCriterioConceptoPago.solicitudPagoFaseCriterioConceptoPagoId ],
                                                        solicitudPagoFaseCriterioId: [ criterio.solicitudPagoFaseCriterioId ],
                                                        conceptoPagoCriterioNombre: [ conceptoFind.nombre ],
                                                        conceptoPagoCriterio: [ solicitudPagoFaseCriterioConceptoPago.conceptoPagoCriterio ],
                                                        valorFacturadoConcepto: [ solicitudPagoFaseCriterioConceptoPago.valorFacturadoConcepto !== undefined ? solicitudPagoFaseCriterioConceptoPago.valorFacturadoConcepto : null ]
                                                    }
                                                )
                                            );
                                        }
                                    } );
                                }

                                const montoMaximo = ( this.montoMaximoPendiente * criterioSeleccionado[0].porcentaje );

                                this.criterios.push(
                                    this.fb.group(
                                        {
                                            solicitudPagoFaseId: [ this.solicitudPagoFase.solicitudPagoFaseId ],
                                            solicitudPagoFaseCriterioId: [ criterio.solicitudPagoFaseCriterioId ],
                                            montoMaximo: [ montoMaximo ],
                                            tipoCriterioCodigo: [ criterio.tipoCriterioCodigo ],
                                            nombreCriterio: [ criterioSeleccionado[0].nombre ],
                                            tiposDePago: [ tiposDePago ],
                                            tipoPago: [ tipoDePago[0], Validators.required ],
                                            conceptosDePago: [ conceptosDePago, Validators.required ],
                                            conceptoPago: [ conceptosDePagoSeleccionados, Validators.required ],
                                            conceptos: this.fb.array( conceptoDePagoArray ),
                                            valorFacturado: [ { value: criterio.valorFacturado !== undefined ? criterio.valorFacturado : null, disabled: true }, Validators.required ]
                                        }
                                    )
                                );
                            }

                            if ( this.esVerDetalle === false ) {
                                // Get observacion CU autorizar solicitud de pago 4.1.9
                                this.obsMultipleSvc.getObservacionSolicitudPagoByMenuIdAndSolicitudPagoId(
                                    this.listaMenusId.autorizarSolicitudPagoId,
                                    this.solicitudPago.solicitudPagoId,
                                    this.solicitudPagoFase.solicitudPagoFaseCriterio[0].solicitudPagoFaseCriterioId,
                                    this.criteriosPagoFacturaCodigo )
                                    .subscribe(
                                        response => {
                                            const observacion = response.find( obs => obs.archivada === false );

                                            if ( observacion !== undefined ) {
                                                this.esAutorizar = true;
                                                this.observacion = observacion;
                                                this.criterios.enable();
                                                if ( this.observacion.tieneObservacion === true ) {
                                                    this.semaforoObservacion.emit( true );
                                                    setTimeout(() => {
                                                        this.addressForm.controls[ 'criterioPago' ].enable()
                                                    }, 1500);
                                                }

                                                this.solicitudPagoObservacionId = observacion.solicitudPagoObservacionId;
                                            }
                                        }
                                    );
                                // Get observacion CU verificar solicitud de pago 4.1.8
                                this.obsMultipleSvc.getObservacionSolicitudPagoByMenuIdAndSolicitudPagoId(
                                    this.listaMenusId.aprobarSolicitudPagoId,
                                    this.solicitudPago.solicitudPagoId,
                                    this.solicitudPagoFase.solicitudPagoFaseCriterio[0].solicitudPagoFaseCriterioId,
                                    this.criteriosPagoFacturaCodigo )
                                    .subscribe(
                                        response => {
                                            const observacion = response.find( obs => obs.archivada === false );
                                            if ( observacion !== undefined ) {
                                                this.esAutorizar = false;
                                                this.observacion = observacion;
                                                this.criterios.enable();

                                                if ( this.observacion.tieneObservacion === true ) {
                                                    this.semaforoObservacion.emit( true );
                                                    setTimeout(() => {
                                                        this.addressForm.controls[ 'criterioPago' ].enable()
                                                    }, 1500);
                                                }

                                                this.solicitudPagoObservacionId = observacion.solicitudPagoObservacionId;
                                            }
                                        }
                                    );
                            }
                        }
                    this.criteriosArray = response;
                    this.addressForm.get( 'criterioPago' ).setValue( this.criteriosSeleccionadosArray.length > 0 ? this.criteriosSeleccionadosArray : null );
                    if ( this.registroCompletoCriterio === true && this.tieneObservacion === false && this.tieneObservacionOrdenGiro === undefined ) {
                        this.criterios.disable();
                    }
                }
            );
        }
    }

    validateNumberKeypress(event: KeyboardEvent) {
        const alphanumeric = /[0-9]/;
        const inputChar = String.fromCharCode(event.charCode);

        return alphanumeric.test(inputChar) ? true : false;
    }

    async getValorTotalConceptos( index: number, jIndex: number, valorConcepto: number ) {
        const usoByConcepto = await this.registrarPagosSvc.getUsoByConceptoPagoCriterioCodigo( this.getConceptos( index ).controls[ jIndex ].get( 'conceptoPagoCriterio' ).value, this.solicitudPago.contratoId );
        
        if ( usoByConcepto.length > 0 ) {
            let valorTotalUso = 0;
            usoByConcepto.forEach( uso => valorTotalUso += uso.valorUso );

            if ( valorConcepto > valorTotalUso ) {
                this.openDialog( '', `El valor facturado al concepto no puede ser mayor al uso asociado <b>${ usoByConcepto[ usoByConcepto.length -1 ].nombre }.</b>` );
                this.getConceptos( index ).controls[ jIndex ].get( 'valorFacturadoConcepto' ).setValue( null );
            }
        }

        if ( valorConcepto > this.criterios.controls[ index ].get( 'montoMaximo' ).value ) {
            this.openDialog( '', '<b>El valor facturado al concepto no puede ser mayor al monto maximo del criterio.</b>' );
            this.getConceptos( index ).controls[ jIndex ].get( 'valorFacturadoConcepto' ).setValue( null );
        }
        if ( this.getConceptos( index ).length > 0 ) {
            let valorTotalCriterios = 0;

            this.getConceptos( index ).controls.forEach( concepto => {
                if ( concepto.value.valorFacturadoConcepto !== null ) {
                    valorTotalCriterios += concepto.value.valorFacturadoConcepto;
                }
            } );

            this.criterios.controls[ index ].get( 'valorFacturado' ).setValue( valorTotalCriterios );
        }
    }

    verifyValorTotalConceptos( index?: number ){
        let totalValorConceptos = 0;
        this.criterios.controls.forEach( control => {
            if ( control.get( 'valorFacturado' ).value !== null ) {
                totalValorConceptos += control.get( 'valorFacturado' ).value;
            }
        } );

        if ( totalValorConceptos > this.montoMaximoPendiente && this.criterios.length > 1 ) {
            this.openDialog( '', '<b>La sumatoria del valor total de los conceptos, no corresponde con el monto máximo a pagar. Verifique por favor</b>' )

            this.btnBoolean = true;
            return true;
        } else {
            return false;
        }
    }

    disabledBtn() {
        if ( this.btnBoolean === true || this.criterios.dirty === false ) {
            return true;
        }
        return false;
    }

    getvalues( criteriovalues: { codigo: string, nombre: string, porcentaje: number }[] ) {
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

                        const montoMaximo = ( this.montoMaximoPendiente * value.porcentaje );

                        this.criterios.push(
                            this.fb.group(
                                {
                                    solicitudPagoFaseId: [ this.solicitudPagoFase.solicitudPagoFaseId ],
                                    solicitudPagoFaseCriterioId: [ 0 ],
                                    montoMaximo: [ montoMaximo ],
                                    tipoCriterioCodigo: [ value.codigo ],
                                    nombreCriterio: [ value.nombre ],
                                    tiposDePago: [ tiposDePago ],
                                    tipoPago: [ null, Validators.required ],
                                    conceptosDePago: [ [], Validators.required ],
                                    conceptoPago: [ null ],
                                    conceptos: this.fb.array( [] ),
                                    valorFacturado: [ { value: null, disabled: true }, Validators.required ]
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

                            const montoMaximo = ( this.montoMaximoPendiente * value.porcentaje );

                            this.criterios.push(
                                this.fb.group(
                                    {
                                        solicitudPagoFaseId: [ this.solicitudPagoFase.solicitudPagoFaseId ],
                                        solicitudPagoFaseCriterioId: [ 0 ],
                                        montoMaximo: [ montoMaximo ],
                                        tipoCriterioCodigo: [ value.codigo ],
                                        nombreCriterio: [ value.nombre ],
                                        tiposDePago: [ tiposDePago ],
                                        tipoPago: [ null, Validators.required ],
                                        conceptosDePago: [ [], Validators.required ],
                                        conceptoPago: [ null ],
                                        conceptos: this.fb.array( [] ),
                                        valorFacturado: [ { value: null, disabled: true }, Validators.required ]
                                    }
                                )
                            );
                        }
                    } );
                } else {
                    this.criterios.clear();

                    values.forEach( async value => {
                        const tiposDePago = await this.registrarPagosSvc.getTipoPagoByCriterioCodigo( value.codigo );

                        const montoMaximo = ( this.montoMaximoPendiente * value.porcentaje );

                        this.criterios.push(
                            this.fb.group(
                                {
                                    solicitudPagoFaseId: [ this.solicitudPagoFase.solicitudPagoFaseId ],
                                    solicitudPagoFaseCriterioId: [ 0 ],
                                    montoMaximo: [ montoMaximo ],
                                    tipoCriterioCodigo: [ value.codigo ],
                                    nombreCriterio: [ value.nombre ],
                                    tiposDePago: [ tiposDePago ],
                                    tipoPago: [ null, Validators.required ],
                                    conceptosDePago: [ [], Validators.required ],
                                    conceptoPago: [ null ],
                                    conceptos: this.fb.array( [] ),
                                    valorFacturado: [ { value: null, disabled: true }, Validators.required ]
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
        this.criterios.controls[ index ].get( 'conceptosDePago' ).setValue( conceptosDePago );
    }

    getvaluesConcepto( conceptos: any[], index: number ) {
        const conceptosArray = [ ...conceptos ];
        if ( conceptosArray.length > 0 ) {
            if ( this.criterios.controls[ index ].get( 'conceptos' ).dirty === true ) {

                if ( this.getConceptos( index ).controls.length > 0 ) {

                    this.getConceptos( index ).controls.forEach( ( control, jIndex ) => {
                        const conceptoIndex = conceptosArray.findIndex( concepto => concepto.codigo === control.get( 'conceptoPagoCriterio' ).value );
                        const concepto = conceptosArray.find( concepto => concepto.codigo === control.get( 'conceptoPagoCriterio' ).value );
                        
                        if ( conceptoIndex !== -1 ) {
                            conceptosArray.splice( conceptoIndex, 1 );
                        }

                        if ( concepto === undefined ) {
                            this.getConceptos( index ).removeAt( jIndex );
                        }
                    } );


                }

                conceptosArray.forEach( concepto => {
                    this.getConceptos( index ).push(
                        this.fb.group(
                            {
                                solicitudPagoFaseCriterioConceptoPagoId: [ 0 ],
                                solicitudPagoFaseCriterioId: [ this.criterios.controls[ index ].get( 'solicitudPagoFaseCriterioId' ).value ],
                                conceptoPagoCriterioNombre: [ concepto.nombre ],
                                conceptoPagoCriterio: [ concepto.codigo ],
                                valorFacturadoConcepto: [ null ]
                            }
                        )
                    );
                } );
            }

            if ( this.criterios.controls[ index ].get( 'conceptos' ).dirty === false ) {

                if ( this.getConceptos( index ).controls.length > 0 ) {

                    this.getConceptos( index ).controls.forEach( ( control, jIndex ) => {
                        const conceptoIndex = conceptosArray.findIndex( concepto => concepto.codigo === control.get( 'conceptoPagoCriterio' ).value );
                        const concepto = conceptosArray.find( concepto => concepto.codigo === control.get( 'conceptoPagoCriterio' ).value );
                        
                        if ( conceptoIndex !== -1 ) {
                            conceptosArray.splice( conceptoIndex, 1 );
                        }

                        if ( concepto === undefined ) {
                            this.getConceptos( index ).removeAt( jIndex );
                        }
                    } );


                }

                conceptosArray.forEach( concepto => {
                    this.getConceptos( index ).push(
                        this.fb.group(
                            {
                                solicitudPagoFaseCriterioConceptoPagoId: [ 0 ],
                                solicitudPagoFaseCriterioId: [ this.criterios.controls[ index ].get( 'solicitudPagoFaseCriterioId' ).value ],
                                conceptoPagoCriterioNombre: [ concepto.nombre ],
                                conceptoPagoCriterio: [ concepto.codigo ],
                                valorFacturadoConcepto: [ null ]
                            }
                        )
                    );
                } );
            }
        } else {
            this.getConceptos( index ).clear();
            this.criterios.controls[ index ].get( 'valorFacturado' ).setValue( null );
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
        this.estaEditando = true;
        this.addressForm.markAllAsTouched();
        this.criterios.markAllAsTouched();

        if ( this.contratacionProyectoId === 0 ) {
            let valorTotalConceptos = 0;
            const solicitudPagoFaseCriterio = [];

            this.criterios.controls.forEach( control => {
                const criterio = control.value;

                if ( control.get( 'valorFacturado' ).value !== null ) {
                    valorTotalConceptos += control.get( 'valorFacturado' ).value;
                }

                solicitudPagoFaseCriterio.push(
                    {
                        tipoCriterioCodigo: criterio.tipoCriterioCodigo,
                        solicitudPagoFaseCriterioId: criterio.solicitudPagoFaseCriterioId,
                        tipoPagoCodigo: criterio.tipoPago.codigo,
                        valorFacturado: control.get( 'valorFacturado' ).value,
                        solicitudPagoFaseCriterioConceptoPago: criterio.conceptos,
                        solicitudPagoFaseId: criterio.solicitudPagoFaseId,
                        solicitudPagoFaseCriterioProyecto: []
                    }
                );
            } );

            if ( this.esPreconstruccion === true ) {
                if ( this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase.length > 0 ) {
                    for ( const solicitudPagoFase of this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase ) {
                        if ( solicitudPagoFase.esPreconstruccion === true ) {
                            solicitudPagoFase.solicitudPagoFaseCriterio = solicitudPagoFaseCriterio;
                        }
                    }
                }
            }
    
            if ( this.esPreconstruccion === false ) {
                if ( this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase.length > 0 ) {
                    for ( const solicitudPagoFase of this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase ) {
                        if ( solicitudPagoFase.esPreconstruccion === false ) {
                            solicitudPagoFase.solicitudPagoFaseCriterio = solicitudPagoFaseCriterio;
                        }
                    }
                }
            }

            /*if ( this.montoMaximoPendiente !== valorTotalConceptos ) {
                this.openDialog( '', '<b>La sumatoria del valor total de los conceptos, no corresponde con el monto máximo a pagar. Verifique por favor</b>' )
                return;
            }*/

            this.registrarPagosSvc.createEditNewPayment( this.solicitudPago )
                .subscribe(
                    response => {
                        this.openDialog( '', `<b>${ response.message }</b>` );
                        if ( this.observacion !== undefined ) {
                            this.observacion.archivada = !this.observacion.archivada;
                            this.obsMultipleSvc.createUpdateSolicitudPagoObservacion( this.observacion ).subscribe();
                        }
                        this.registrarPagosSvc.getValidateSolicitudPagoId( this.solicitudPago.solicitudPagoId )
                            .subscribe(
                                () => {
                                    this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                                        () => this.routes.navigate(
                                            [
                                                '/registrarValidarRequisitosPago/verDetalleEditar',  this.solicitudPago.contratoId, this.solicitudPago.solicitudPagoId
                                            ]
                                        )
                                    );
                                }
                            );
                    },
                    err => this.openDialog( '', `<b>${ err.message }</b>` )
                );
        } else {
            let valorTotalConceptos = 0;

            const solicitudPagoFaseCriterio = [];
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
                
                if ( control.get( 'valorFacturado' ).value !== null ) {
                    valorTotalConceptos += control.get( 'valorFacturado' ).value;
                }

                solicitudPagoFaseCriterio.push(
                    {
                        tipoCriterioCodigo: criterio.tipoCriterioCodigo,
                        solicitudPagoFaseCriterioId: criterio.solicitudPagoFaseCriterioId,
                        tipoPagoCodigo: criterio.tipoPago.codigo,
                        valorFacturado: control.get( 'valorFacturado' ).value,
                        solicitudPagoFaseCriterioConceptoPago: criterio.conceptos,
                        solicitudPagoFaseId: criterio.solicitudPagoFaseId,
                        solicitudPagoFaseCriterioProyecto
                    }
                );
            } );

            if ( this.esPreconstruccion === true ) {
                if ( this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase.length > 0 ) {
                    for ( const solicitudPagoFase of this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase ) {
                        if ( solicitudPagoFase.esPreconstruccion === true ) {
                            solicitudPagoFase.solicitudPagoFaseCriterio = solicitudPagoFaseCriterio;
                        }
                    }
                }
            }
    
            if ( this.esPreconstruccion === false ) {
                if ( this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase.length > 0 ) {
                    for ( const solicitudPagoFase of this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase ) {
                        if ( solicitudPagoFase.esPreconstruccion === false ) {
                            solicitudPagoFase.solicitudPagoFaseCriterio = solicitudPagoFaseCriterio;
                        }
                    }
                }
            }

            /*if ( this.montoMaximoPendiente !== valorTotalConceptos ) {
                this.openDialog( '', '<b>La sumatoria del valor total de los conceptos, no corresponde con el monto máximo a pagar. Verifique por favor</b>' )
                return;
            }*/

            this.registrarPagosSvc.createEditNewPayment( this.solicitudPago )
                .subscribe(
                    response => {
                        this.openDialog( '', `<b>${ response.message }</b>` );
                        if ( this.observacion !== undefined ) {
                            this.observacion.archivada = !this.observacion.archivada;
                            this.obsMultipleSvc.createUpdateSolicitudPagoObservacion( this.observacion ).subscribe();
                        }
                        this.registrarPagosSvc.getValidateSolicitudPagoId( this.solicitudPago.solicitudPagoId )
                            .subscribe(
                                () => {
                                    this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                                        () => this.routes.navigate(
                                            [
                                                '/registrarValidarRequisitosPago/verDetalleEditar',  this.solicitudPago.contratoId, this.solicitudPago.solicitudPagoId
                                            ]
                                        )
                                    );
                                }
                            );
                    },
                    err => this.openDialog( '', `<b>${ err.message }</b>` )
                );
        }
    }

}
