import { Dominio } from './../../../../core/_services/common/common.service';
import { FormGroup, FormBuilder, FormArray, Validators } from '@angular/forms';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { RegistrarRequisitosPagoService } from 'src/app/core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';
import { OrdenPagoService } from 'src/app/core/_services/ordenPago/orden-pago.service';
import { FinancialBalanceService } from 'src/app/core/_services/financialBalance/financial-balance.service';
import { MatDialog } from '@angular/material/dialog';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { TipoTrasladoCodigo } from 'src/app/_interfaces/balance-financiero.interface';
import humanize from 'humanize-plus';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-form-descuentos-direccion-tecnica',
  templateUrl: './form-descuentos-direccion-tecnica.component.html',
  styleUrls: ['./form-descuentos-direccion-tecnica.component.scss']
})
export class FormDescuentosDireccionTecnicaComponent implements OnInit {

    @Input() solicitudPago: any;
    @Input() esVerDetalle: boolean;
    @Input() esRegistroNuevo: boolean;
    @Output() estadoSemaforo = new EventEmitter<string>();
    tipoTrasladoCodigo = TipoTrasladoCodigo;
    balanceFinanciero: any;
    balanceFinancieroId = 0;
    ordenGiroId = 0;
    ordenGiroDetalle: any;
    balanceFinancieroTrasladoValor: any;
    fasePreConstruccionFormaPagoCodigo: any;
    listaTipoDescuento: Dominio[] = [];
    listaDescuentosDireccionTecnica: Dominio[] = [];
    ordenGiroDetalleDescuentoTecnica: any[];
    formDescuentosDireccionTecnica = this.fb.group(
        {
            fases: this.fb.array( [] )
        }
    )

    get fases() {
        return this.formDescuentosDireccionTecnica.get( 'fases' ) as FormArray;
    }

    getDescuentos( index: number ) {
        return this.fases.controls[ index ].get( 'descuentos' ) as FormArray;
    }

    getCriterios( index: number, jIndex: number ) {
        return this.getDescuentos( index ).controls[ jIndex ].get( 'criterios' ) as FormArray;
    }

    getConceptos( index: number, jIndex: number, kIndex: number ) {
        return this.getCriterios( index, jIndex ).controls[ kIndex ].get( 'conceptos' ) as FormArray;
    }

    getAportantes( index: number, jIndex: number, kIndex: number, lIndex: number ) {
        return this.getConceptos( index, jIndex, kIndex ).controls[ lIndex ].get( 'aportantes' ) as FormArray;
    }

    constructor(
        private fb: FormBuilder,
        private activatedRoute: ActivatedRoute,
        private registrarPagosSvc: RegistrarRequisitosPagoService,
        private ordenGiroSvc: OrdenPagoService,
        private routes: Router,
        private balanceSvc: FinancialBalanceService,
        private dialog: MatDialog,
        private commonSvc: CommonService )
    { }

    ngOnInit() {
        this.getDireccionTecnica();
    }

    async getDireccionTecnica() {
        this.listaTipoDescuento = await this.commonSvc.tiposDescuento().toPromise()
        this.balanceFinanciero = await this.balanceSvc.getBalanceFinanciero( this.activatedRoute.snapshot.params.id ).toPromise()
        this.listaDescuentosDireccionTecnica = await this.commonSvc.tiposDescuento().toPromise()

        this.balanceFinancieroId = this.balanceFinanciero.balanceFinancieroId;
        this.balanceFinancieroTrasladoValor = this.balanceFinanciero.balanceFinancieroTrasladoValor;

        if ( this.solicitudPago.ordenGiro !== undefined ) {
            this.ordenGiroId = this.solicitudPago.ordenGiro.ordenGiroId;
    
            if ( this.solicitudPago.ordenGiro.ordenGiroDetalle !== undefined ) {
                if ( this.solicitudPago.ordenGiro.ordenGiroDetalle.length > 0 ) {
                    this.ordenGiroDetalle = this.solicitudPago.ordenGiro.ordenGiroDetalle[0];

                    if ( this.ordenGiroDetalle.ordenGiroDetalleDescuentoTecnica !== undefined ) {
                        if ( this.ordenGiroDetalle.ordenGiroDetalleDescuentoTecnica.length > 0 ) {
                            this.ordenGiroDetalleDescuentoTecnica = this.ordenGiroDetalle.ordenGiroDetalleDescuentoTecnica;
                        }
                    }
                }
            }
        }

        if ( this.solicitudPago.contratoSon.solicitudPago.length > 1 ) {
            this.fasePreConstruccionFormaPagoCodigo = this.solicitudPago.contratoSon.solicitudPago[0].solicitudPagoCargarFormaPago[0];
        } else {
            this.fasePreConstruccionFormaPagoCodigo = this.solicitudPago.solicitudPagoCargarFormaPago[0];
        }

        for ( const solicitudPagoFase of this.solicitudPago.solicitudPagoRegistrarSolicitudPago[ 0 ].solicitudPagoFase ) {
            // Get Tablas
            const solicitudPagoFaseCriterio = solicitudPagoFase.solicitudPagoFaseCriterio;
            const solicitudPagoFaseFactura = solicitudPagoFase.solicitudPagoFaseFactura[0];
            const descuentos = [];
            let nuevoValorRegistrado = 0;
            const listData = {
                listaDescuentos: [],
                valorNetoGiro: 0,
                valorTotalDescuentos: 0
            };

            if ( solicitudPagoFaseFactura.tieneDescuento === true ) {
            /*
                get listaCriterios para lista desplegable
                Se reutilizan los servicios del CU 4.1.7 "Solicitud de pago"
            */
                const getCriterioByFormaPagoCodigo = await this.registrarPagosSvc.getCriterioByFormaPagoCodigo( solicitudPagoFase.esPreconstruccion === true ? this.fasePreConstruccionFormaPagoCodigo.fasePreConstruccionFormaPagoCodigo : this.fasePreConstruccionFormaPagoCodigo.faseConstruccionFormaPagoCodigo ).toPromise()

                const listaCriteriosFormaPago = getCriterioByFormaPagoCodigo;
                // Get data de la tabla descuentos
                solicitudPagoFaseCriterio.forEach( criterio => listData.valorNetoGiro += criterio.valorFacturado );
                solicitudPagoFaseFactura.solicitudPagoFaseFacturaDescuento.forEach( descuento => {
                    listData.valorNetoGiro -= descuento.valorDescuento;
                    listData.valorTotalDescuentos += descuento.valorDescuento;
    
                    listData.listaDescuentos.push(
                        {
                            tipoDescuentoCodigo: descuento.tipoDescuentoCodigo,
                            valorDescuento: descuento.valorDescuento
                        }
                    )
                } );
    
                // Set data Formulario
                if ( this.ordenGiroDetalle !== undefined ) {
                    const ordenGiroDetalleDescuentoTecnica: any[] = this.ordenGiroDetalle.ordenGiroDetalleDescuentoTecnica;
    
                    if ( ordenGiroDetalleDescuentoTecnica.length > 0 ) {
                        for ( const descuento of solicitudPagoFaseFactura.solicitudPagoFaseFacturaDescuento ) {
                            const formArrayCriterios = [];
                            const detalleDescuentoTecnica = ordenGiroDetalleDescuentoTecnica.filter( descuentoTecnica => descuentoTecnica.solicitudPagoFaseFacturaDescuentoId === descuento.solicitudPagoFaseFacturaDescuentoId && descuentoTecnica.esPreconstruccion === solicitudPagoFase.esPreconstruccion );
    
                            if ( detalleDescuentoTecnica.length > 0 ) {
                                // Get forArray de los criterios
                                const listaCriterios = [];
    
                                for ( const descuento of detalleDescuentoTecnica ) {
                                    const tiposDePago = await this.registrarPagosSvc.getTipoPagoByCriterioCodigo( descuento.criterioCodigo );
                                    const tipoPago = tiposDePago.find( tipoPago => tipoPago.codigo === descuento.tipoPagoCodigo );
                                    const conceptosDePago = await this.registrarPagosSvc.getConceptoPagoCriterioCodigoByTipoPagoCodigo( tipoPago.codigo );
                                    // Get data del formulario de los conceptos seleccionados
                                    const listaConceptos = [];
                                    const formArrayConceptos = [];
                                    const ordenGiroDetalleDescuentoTecnicaAportante = [];
                                    let registroCompleto: boolean;
                                    let cantidadRegistroAportantes = 0;
                                    let cantidadRegistroCompleto = 0;
    
                                    listaCriterios.push( descuento.criterioCodigo )
    
                                    descuento.ordenGiroDetalleDescuentoTecnicaAportante.forEach( value => {
                                        ordenGiroDetalleDescuentoTecnicaAportante.push( value );
                                        if ( listaConceptos.length > 0 ) {
                                            const tieneConceptoCodigo = listaConceptos.includes( value.conceptoPagoCodigo );
    
                                            if ( tieneConceptoCodigo === false ) {
                                                listaConceptos.push( value.conceptoPagoCodigo );    
                                            }
                                        } else {
                                            listaConceptos.push( value.conceptoPagoCodigo )
                                        }
                                    } );
    
                                    for ( const codigo of listaConceptos ) {
                                        const concepto = conceptosDePago.find( concepto => concepto.codigo === codigo );
                                        if ( concepto !== undefined ) {
                                            const dataAportantes = await this.ordenGiroSvc.getAportantes( this.solicitudPago )
                                            const formArrayAportantes = [];
    
                                            if ( ordenGiroDetalleDescuentoTecnicaAportante.length > 0 ) {
                                                for ( const aportante of ordenGiroDetalleDescuentoTecnicaAportante ) {
                                                    const nombreAportante = dataAportantes.listaNombreAportante.find( nombre => nombre.cofinanciacionAportanteId === aportante.aportanteId );
                                                    let tipoAportante: any;
                                                    let listaFuenteRecursos: any[];
    
                                                    if ( nombreAportante !== undefined ) {
                                                        tipoAportante = dataAportantes.listaTipoAportante.find( tipo => tipo.dominioId === nombreAportante.tipoAportanteId );
                                                        listaFuenteRecursos = await this.ordenGiroSvc.getFuentesDeRecursosPorAportanteId( nombreAportante.cofinanciacionAportanteId ).toPromise();
                                                        let valorTraslado: number;
                                                        let balanceFinancieroTrasladoValorId = 0;

                                                        if ( this.balanceFinancieroTrasladoValor !== undefined ) {
                                                            if ( this.balanceFinancieroTrasladoValor.length > 0 ) {
                                                                const balanceFinancieroTrasladoValor = this.balanceFinancieroTrasladoValor.find(
                                                                    balanceFinancieroTrasladoValor => balanceFinancieroTrasladoValor.ordenGiroDetalleDescuentoTecnicaAportanteId === aportante.ordenGiroDetalleDescuentoTecnicaAportanteId
                                                                    && balanceFinancieroTrasladoValor.tipoTrasladoCodigo === String( this.tipoTrasladoCodigo.direccionTecnica )
                                                                )
                        
                                                                if ( balanceFinancieroTrasladoValor !== undefined ) {
                                                                    valorTraslado = balanceFinancieroTrasladoValor.valorTraslado
                                                                    balanceFinancieroTrasladoValorId = balanceFinancieroTrasladoValor.balanceFinancieroTrasladoValorId
                                                                }
                                                            }
                                                        }

                                                        if ( valorTraslado !== undefined ) {
                                                            nuevoValorRegistrado += valorTraslado;
                                                            cantidadRegistroCompleto++;
                                                        }

                                                        formArrayAportantes.push(
                                                            this.fb.group(
                                                                {
                                                                    ordenGiroDetalleDescuentoTecnicaAportanteId: [ aportante.ordenGiroDetalleDescuentoTecnicaAportanteId !== undefined ? aportante.ordenGiroDetalleDescuentoTecnicaAportanteId : 0 ],
                                                                    balanceFinancieroTrasladoValorId: [ balanceFinancieroTrasladoValorId ],
                                                                    tipoTrasladoCodigo: [ String( this.tipoTrasladoCodigo.direccionTecnica ) ],
                                                                    tipoAportante: [ tipoAportante !== undefined ? tipoAportante : null, Validators.required ],
                                                                    listaNombreAportantes: [ [ nombreAportante !== undefined ? nombreAportante : null ] ],
                                                                    nombreAportante: [ nombreAportante !== undefined ? nombreAportante : null, Validators.required ],
                                                                    fuenteDeRecursos: [ listaFuenteRecursos ],
                                                                    fuenteRecursos: [ aportante.fuenteRecursosCodigo !== undefined ? aportante.fuenteRecursosCodigo : null, Validators.required ],
                                                                    valorDescuento: [ aportante.valorDescuento !== undefined ? aportante.valorDescuento : null, Validators.required ],
                                                                    nuevoValorDescuento: [ valorTraslado !== undefined ? ( valorTraslado < 0 ? null : valorTraslado ) : null, Validators.required ]
                                                                }
                                                            )
                                                        )
                                                    }
                                                }
                                            }

                                            if ( formArrayAportantes.length > 0 ) {
                                                cantidadRegistroAportantes = formArrayAportantes.length;
                                            }
    
                                            formArrayConceptos.push( this.fb.group(
                                                {
                                                    nombre: [ concepto !== undefined ? concepto.nombre : null ],
                                                    conceptoCodigo: [ concepto !== undefined ? concepto.codigo : null ],
                                                    tipoDeAportantes: [ dataAportantes.listaTipoAportante ],
                                                    nombreDeAportantes: [ dataAportantes.listaNombreAportante ],
                                                    aportantes: this.fb.array( formArrayAportantes.length > 0 ? formArrayAportantes : [] )
                                                }
                                            ) );
                                        }
                                    }

                                    if ( cantidadRegistroAportantes > 0 ) {
                                        if ( cantidadRegistroCompleto > 0 && cantidadRegistroCompleto < cantidadRegistroAportantes ) {
                                            registroCompleto = false
                                        }
                                        if ( cantidadRegistroCompleto > 0 && cantidadRegistroCompleto === cantidadRegistroAportantes ) {
                                            registroCompleto = true
                                        }
                                    }
    
                                    // Set formulario de los criterios
                                    formArrayCriterios.push(
                                        this.fb.group(
                                            {
                                                registroCompleto,
                                                nombre: [ descuento.criterioCodigo !== undefined ? listaCriteriosFormaPago.find( criterio => criterio.codigo === descuento.criterioCodigo ).nombre : null ],
                                                ordenGiroDetalleDescuentoTecnicaId: [ descuento.ordenGiroDetalleDescuentoTecnicaId !== undefined ? descuento.ordenGiroDetalleDescuentoTecnicaId : 0 ],
                                                criterioCodigo: [ descuento.criterioCodigo !== undefined ? listaCriteriosFormaPago.find( criterio => criterio.codigo === descuento.criterioCodigo ).codigo : null ],
                                                tipoPagoNombre: [ tipoPago !== undefined ? tipoPago.nombre : null ],
                                                tipoPagoCodigo: [ tipoPago !== undefined ? tipoPago.codigo : null ],
                                                conceptosDePago: [ conceptosDePago ],
                                                concepto: [ listaConceptos.length > 0 ? listaConceptos : null, Validators.required ],
                                                conceptos: this.fb.array( formArrayConceptos.length > 0 ? formArrayConceptos : [] )
                                            }
                                        )
                                    )
                                }

                                const sinDiligenciar = formArrayCriterios.find( criterio => criterio.get( 'registroCompleto' ).value === null )
                                const enProceso = formArrayCriterios.find( criterio => criterio.get( 'registroCompleto' ).value === false )
                                const completo = formArrayCriterios.find( criterio => criterio.get( 'registroCompleto' ).value === true )
                                let semaforoDescuento = 'sin-diligenciar'

                                if ( sinDiligenciar !== undefined && enProceso !== undefined && completo === undefined ) {
                                    semaforoDescuento = 'en-proceso'
                                }
                                if ( sinDiligenciar !== undefined && completo !== undefined && enProceso === undefined ) {
                                    semaforoDescuento = 'en-proceso'
                                }
                                if ( sinDiligenciar === undefined && enProceso !== undefined && completo !== undefined ) {
                                    semaforoDescuento = 'en-proceso'
                                }
                                if ( sinDiligenciar === undefined && enProceso === undefined && completo !== undefined ) {
                                    semaforoDescuento = 'completo'
                                }
    
                                descuentos.push( this.fb.group(
                                    {
                                        semaforo: [ semaforoDescuento ],
                                        solicitudPagoFaseFacturaDescuentoId: [ descuento.solicitudPagoFaseFacturaDescuentoId ],
                                        tipoDescuentoCodigo: [ descuento.tipoDescuentoCodigo ],
                                        listaCriteriosFormaPago: [ listaCriteriosFormaPago ],
                                        criterio: [ listaCriterios, Validators.required ],
                                        criterios: this.fb.array( formArrayCriterios )
                                    }
                                ) );
                            }
                        }
                    }
                }

                const sinDiligenciarDescuento = descuentos.find( descuento => descuento.get( 'semaforo' ).value === 'sin-diligenciar' )
                const enProcesoDescuento = descuentos.find( descuento => descuento.get( 'semaforo' ).value === 'en-proceso' )
                const completoDescuento = descuentos.find( descuento => descuento.get( 'semaforo' ).value === 'completo' )
                let semaforoFase = 'sin-diligenciar'

                if ( sinDiligenciarDescuento !== undefined && enProcesoDescuento !== undefined && completoDescuento === undefined ) {
                    semaforoFase = 'en-proceso'
                }
                if ( sinDiligenciarDescuento !== undefined && completoDescuento !== undefined && enProcesoDescuento === undefined ) {
                    semaforoFase = 'en-proceso'
                }
                if ( sinDiligenciarDescuento === undefined && completoDescuento !== undefined && enProcesoDescuento !== undefined ) {
                    semaforoFase = 'en-proceso'
                }
                if ( sinDiligenciarDescuento === undefined && completoDescuento !== undefined && enProcesoDescuento === undefined ) {
                    semaforoFase = 'completo'
                }

    
                this.fases.push(
                    this.fb.group(
                        {
                            semaforo: [ semaforoFase ],
                            valorNetoGiro: [ listData.valorNetoGiro ],
                            nuevoValorRegistrado,
                            esPreconstruccion: [ solicitudPagoFase.esPreconstruccion ],
                            descuentos: this.fb.array( descuentos )
                        }
                    )
                )
            }
        }

        if ( this.fases.length > 0 ) {
            const sinDiligenciarFase = this.fases.controls.find( descuento => descuento.get( 'semaforo' ).value === 'sin-diligenciar' )
            const enProcesoFase = this.fases.controls.find( descuento => descuento.get( 'semaforo' ).value === 'en-proceso' )
            const completoFase = this.fases.controls.find( descuento => descuento.get( 'semaforo' ).value === 'completo' )
            let estadoSemaforo = 'sin-diligenciar'
    
            if ( sinDiligenciarFase !== undefined && enProcesoFase !== undefined && completoFase === undefined ) {
                estadoSemaforo = 'en-proceso'
            }
            if ( sinDiligenciarFase !== undefined && completoFase !== undefined && enProcesoFase === undefined ) {
                estadoSemaforo = 'en-proceso'
            }
            if ( sinDiligenciarFase === undefined && completoFase !== undefined && enProcesoFase !== undefined ) {
                estadoSemaforo = 'en-proceso'
            }
            if ( sinDiligenciarFase === undefined && completoFase !== undefined && enProcesoFase === undefined ) {
                estadoSemaforo = 'completo'
            }
    
            this.estadoSemaforo.emit( estadoSemaforo )
        }
    }

    checkValue( value: number, index: number, jIndex: number, kIndex: number, lIndex: number, mIndex: number ) {
        if ( value !== null ) {
            if ( value < 0 ) {
                this.getAportantes( index, jIndex, kIndex, lIndex ).controls[ mIndex ].get( 'nuevoValorDescuento' ).setValue( null )
            }
        }
    }

    getTipoDescuento( tipoDescuentoCodigo: string ) {
        if (this.listaTipoDescuento.length > 0) {
            const descuento = this.listaTipoDescuento.find( descuento => descuento.codigo === tipoDescuentoCodigo );
            
            if ( descuento !== undefined ) {
                return descuento.nombre;
            }
        }
    }

    firstLetterUpperCase( texto:string ) {
        if ( texto !== undefined ) {
            return humanize.capitalize( String( texto ).toLowerCase() );
        }
    }

    getValueCriterio( codigo: string, listaCriterios: any[] ) {
        if ( listaCriterios.length > 0 ) {
            const criterio = listaCriterios.find( criterio => criterio.codigo === codigo );

            if ( criterio !== undefined ) {
                return criterio.nombre;
            }
        }
    }

    getValueConcepto( codigo: string, listaConceptos: Dominio[] ) {
        if ( listaConceptos.length > 0 ) {
            const concepto = listaConceptos.find( concepto => concepto.codigo === codigo );

            if ( concepto !== undefined ) {
                return concepto.nombre;
            }
        }
    }

    getValueFuente( codigo: string, listaFuentes: Dominio[] ) {
        if ( listaFuentes.length > 0 ) {
            const fuente = listaFuentes.find( concepto => concepto.codigo === codigo );

            if ( fuente !== undefined ) {
                return fuente.nombre;
            }
        }
    }

    validateNumberKeypress(event: KeyboardEvent) {
        const alphanumeric = /[0-9]/;
        const inputChar = String.fromCharCode(event.charCode);
        return alphanumeric.test(inputChar) ? true : false;
    }

    openDialog(modalTitle: string, modalText: string) {
        const dialogRef = this.dialog.open( ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText }
        });
    }

    guardar( index: number ) {
        const getBalanceFinancieroTrasladoValor = () => {
            const balanceFinancieroTraslado = [];

            this.getDescuentos( index ).controls.forEach(( descuentoControl, indexDescuento ) => {
                this.getCriterios( index, indexDescuento ).controls.forEach( ( criterioControl, indexCriterio ) => {
                    this.getConceptos( index, indexDescuento, indexCriterio ).controls.forEach( ( conceptoControl, indexConcepto ) => {
                        this.getAportantes( index, indexDescuento, indexCriterio, indexConcepto ).controls.forEach( aportanteControl => {
                            //ordenGiroDetalleDescuentoTecnicaAportanteId
                            if ( aportanteControl.dirty === true ) {
                                balanceFinancieroTraslado.push(
                                    {
                                        ordenGiroId: this.ordenGiroId,
                                        balanceFinancieroId: this.balanceFinancieroId,
                                        esPreconstruccion: this.fases.controls[ index ].get( 'esPreconstruccion' ).value,
                                        balanceFinancieroTrasladoValorId: aportanteControl.get( 'balanceFinancieroTrasladoValorId' ).value,
                                        tipoTrasladoCodigo: aportanteControl.get( 'tipoTrasladoCodigo' ).value,
                                        ordenGiroDetalleDescuentoTecnicaAportanteId: aportanteControl.get( 'ordenGiroDetalleDescuentoTecnicaAportanteId' ).value,
                                        valorTraslado: aportanteControl.get( 'nuevoValorDescuento' ).value
                                    }
                                )
                            }
                        } )
                    } )
                } )
            } )

            return balanceFinancieroTraslado.length > 0 ? balanceFinancieroTraslado : null;
        }

        this.balanceFinanciero.balanceFinancieroTrasladoValor = getBalanceFinancieroTrasladoValor()

        this.balanceSvc.createEditBalanceFinanciero( this.balanceFinanciero )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );

                    this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                        () => this.routes.navigate(
                            [
                                '/gestionarBalanceFinancieroTrasladoRecursos/verDetalleEditarTraslado', this.activatedRoute.snapshot.params.id
                            ]
                        )
                    );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            )
    }

}
