import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { FormBuilder, FormArray, Validators, FormGroup } from '@angular/forms';
import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { RegistrarRequisitosPagoService } from 'src/app/core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';
import { OrdenPagoService } from 'src/app/core/_services/ordenPago/orden-pago.service';
import humanize from 'humanize-plus';
import { FinancialBalanceService } from 'src/app/core/_services/financialBalance/financial-balance.service';
import { TipoTrasladoCodigo } from 'src/app/_interfaces/balance-financiero.interface';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-form-tercero-causacion',
  templateUrl: './form-tercero-causacion.component.html',
  styleUrls: ['./form-tercero-causacion.component.scss']
})
export class FormTerceroCausacionComponent implements OnInit {

    @Input() solicitudPago: any;
    @Input() esVerDetalle: boolean;
    @Input() esRegistroNuevo: boolean;
    @Output() estadoSemaforo = new EventEmitter<string>();
    tipoTrasladoCodigo = TipoTrasladoCodigo;
    balanceFinanciero: any;
    balanceFinancieroId = 0;
    ordenGiroId = 0;
    balanceFinancieroTrasladoValor: any;
    tipoDescuentoArray: Dominio[] = [];
    tipoDescuentoTecnicaArray: Dominio[] = [];
    fasePreConstruccionFormaPagoCodigo: any;
    ordenGiroDetalleTerceroCausacion: any[];
    ordenGiroDetalleDescuentoTecnica: any[];
    formTerceroCausacion = this.fb.group(
        {
            terceroCausacion: this.fb.array( [] )
        }
    )

    get terceroCausacion() {
        return this.formTerceroCausacion.get( 'terceroCausacion' ) as FormArray;
    }

    getCriterios( index: number ) {
        return this.terceroCausacion.controls[ index ].get( 'criterios' ) as FormArray;
    }

    getConceptos( index: number, jIndex: number ) {
        return  this.getCriterios( index ).controls[ jIndex ].get( 'conceptos' ) as FormArray;
    }

    getAportantes( index: number, jIndex: number, kIndex: number ) {
        return this.getConceptos( index, jIndex ).controls[ kIndex ].get( 'aportantes' ) as FormArray;
    }

    getDescuentosFinanciera( index: number, jIndex: number, kIndex: number ) {
        return this.getConceptos( index, jIndex ).controls[ kIndex ].get( 'descuento' ).get( 'descuentos' ) as FormArray;
    }

    getDescuentosTecnica( index: number, jIndex: number, kIndex: number ) {
        return this.getConceptos( index, jIndex ).controls[ kIndex ].get( 'descuentoTecnica' ).get( 'descuentos' ) as FormArray;
    }

    getAportanteDescuentos( index: number, jIndex: number, kIndex: number, lIndex: number ) {
        return this.getDescuentosFinanciera( index, jIndex, kIndex ).controls[ lIndex ].get( 'aportantesDescuento' ) as FormArray;
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
    {
    }

    async ngOnInit() {
        let ordenGiroDetalle: any;
        this.balanceFinanciero = await this.balanceSvc.getBalanceFinanciero( this.activatedRoute.snapshot.params.id ).toPromise();
        this.tipoDescuentoTecnicaArray = await this.commonSvc.tiposDescuento().toPromise();
        this.tipoDescuentoArray = await this.commonSvc.listaDescuentosOrdenGiro().toPromise();
        console.log( this.balanceFinanciero )

        this.balanceFinancieroId = this.balanceFinanciero.balanceFinancieroId;
        this.balanceFinancieroTrasladoValor = this.balanceFinanciero.balanceFinancieroTrasladoValor;

        if ( this.solicitudPago.ordenGiro !== undefined ) {
            this.ordenGiroId = this.solicitudPago.ordenGiro.ordenGiroId;
    
            if ( this.solicitudPago.ordenGiro.ordenGiroDetalle !== undefined ) {
                if ( this.solicitudPago.ordenGiro.ordenGiroDetalle.length > 0 ) {
                    ordenGiroDetalle = this.solicitudPago.ordenGiro.ordenGiroDetalle[0];

                    if ( ordenGiroDetalle.ordenGiroDetalleDescuentoTecnica !== undefined ) {
                        if ( ordenGiroDetalle.ordenGiroDetalleDescuentoTecnica.length > 0 ) {
                            this.ordenGiroDetalleDescuentoTecnica = ordenGiroDetalle.ordenGiroDetalleDescuentoTecnica;
                        }
                    }

                    if ( ordenGiroDetalle.ordenGiroDetalleTerceroCausacion !== undefined ) {
                        if ( ordenGiroDetalle.ordenGiroDetalleTerceroCausacion.length > 0 ) {
                            this.ordenGiroDetalleTerceroCausacion = ordenGiroDetalle.ordenGiroDetalleTerceroCausacion;
                        }
                    }
                }
            }
        }

        for ( const solicitudPagoFase of this.solicitudPago.solicitudPagoRegistrarSolicitudPago[ 0 ].solicitudPagoFase ) {
            const criteriosArray = [];
            let valorNetoGiro = 0;
            let nuevoValorRegistrado = 0;
            const solicitudPagoFaseCriterio = solicitudPagoFase.solicitudPagoFaseCriterio;
            const solicitudPagoFaseFactura = solicitudPagoFase.solicitudPagoFaseFactura[0];
            const detalleDescuentoTecnica = this.ordenGiroDetalleDescuentoTecnica.filter( ordenGiroDetalleDescuentoTecnica => ordenGiroDetalleDescuentoTecnica.esPreconstruccion === solicitudPagoFase.esPreconstruccion );

            if ( this.solicitudPago.contratoSon.solicitudPago.length > 1 ) {
                this.fasePreConstruccionFormaPagoCodigo = this.solicitudPago.contratoSon.solicitudPago[0].solicitudPagoCargarFormaPago[0];
            } else {
                this.fasePreConstruccionFormaPagoCodigo = this.solicitudPago.solicitudPagoCargarFormaPago[0];
            }
            const criterioFormaPago = await this.registrarPagosSvc.getCriterioByFormaPagoCodigo(
                solicitudPagoFase.esPreconstruccion === true ? this.fasePreConstruccionFormaPagoCodigo.fasePreConstruccionFormaPagoCodigo : this.fasePreConstruccionFormaPagoCodigo.faseConstruccionFormaPagoCodigo
            ).toPromise();

            // Get constantes y variables
            const listCriterios = [];
            // Busqueda de criterios seleccionados en el CU 4.1.7 en la lista de tipo dominio
            for ( const criterioValue of solicitudPagoFaseCriterio ) {
                const criterioFind = criterioFormaPago.find( value => value.codigo === criterioValue.tipoCriterioCodigo );
                const listConceptos = [];
                if ( criterioFind !== undefined ) {
                    // Get lista dominio de los tipos de pago por criterio codigo
                    const tiposDePago = await this.registrarPagosSvc.getTipoPagoByCriterioCodigo( criterioFind.codigo );
                    const tipoPago = tiposDePago.find( tipoPago => tipoPago.codigo === criterioValue.tipoPagoCodigo );
                    // Get lista dominio de los conceptos de pago por tipo de pago codigo
                    const conceptosDePago = await this.registrarPagosSvc.getConceptoPagoCriterioCodigoByTipoPagoCodigo( tipoPago.codigo );
                    
                    // Get data de los conceptos diligenciados en el CU 4.1.7
                    for ( const conceptoValue of criterioValue.solicitudPagoFaseCriterioConceptoPago ) {
                        const conceptoFind = conceptosDePago.find( value => value.codigo === conceptoValue.conceptoPagoCriterio );
                        if ( conceptoFind !== undefined ) {
                            listConceptos.push( { ...conceptoFind, valorFacturadoConcepto: conceptoValue.valorFacturadoConcepto } );
                        }
                    }
                    listCriterios.push(
                        {
                            tipoCriterioCodigo: criterioFind.codigo,
                            nombre: criterioFind.nombre,
                            tipoPagoCodigo: tipoPago.codigo,
                            listConceptos
                        }
                    );
                }
            }

            const dataAportantes = await this.ordenGiroSvc.getAportantes( this.solicitudPago );
            // Get data del guardado de tercero de causacion
            for ( const criterio of listCriterios ) {
                const listDescuento = [ ...this.tipoDescuentoArray ];

                if ( this.ordenGiroDetalleTerceroCausacion !== undefined ) {
                    const terceroCausacion = this.ordenGiroDetalleTerceroCausacion.find( tercero => tercero.conceptoPagoCriterio === criterio.tipoCriterioCodigo && tercero.esPreconstruccion === solicitudPagoFase.esPreconstruccion );
                    const listaDescuentos = [];
                    const listaAportantes = [];
                    const conceptosDePago = [];
                    const listaDescuentoTecnica = [];

                    if ( terceroCausacion !== undefined ) {
                        /*
                        if ( terceroCausacion.ordenGiroDetalleTerceroCausacionDescuento.length > 0 ) {
                            terceroCausacion.ordenGiroDetalleTerceroCausacionDescuento.forEach( descuento => {
                                let valorTraslado: number;
                                let balanceFinancieroTrasladoValorId = 0;

                                if ( this.balanceFinancieroTrasladoValor !== undefined ) {
                                    if ( this.balanceFinancieroTrasladoValor.length > 0 ) {
                                        const balanceFinancieroTrasladoValor = this.balanceFinancieroTrasladoValor.find(
                                            balanceFinancieroTrasladoValor => balanceFinancieroTrasladoValor.ordenGiroDetalleTerceroCausacionDescuentoId === descuento.ordenGiroDetalleTerceroCausacionDescuentoId
                                            && balanceFinancieroTrasladoValor.tipoTrasladoCodigo === String( this.tipoTrasladoCodigo.direccionFinanciera )
                                        )

                                        if ( balanceFinancieroTrasladoValor !== undefined ) {
                                            valorTraslado = balanceFinancieroTrasladoValor.valorTraslado
                                            balanceFinancieroTrasladoValorId = balanceFinancieroTrasladoValor.balanceFinancieroTrasladoValorId
                                            nuevoValorRegistrado += valorTraslado
                                        }
                                    }
                                }

                                listaDescuentos.push(
                                    this.fb.group(
                                        {
                                            registroCompleto: [ valorTraslado !== undefined ? true : false ],
                                            ordenGiroDetalleTerceroCausacionDescuentoId: [ descuento.ordenGiroDetalleTerceroCausacionDescuentoId ],
                                            balanceFinancieroTrasladoValorId: [ balanceFinancieroTrasladoValorId ],
                                            tipoTrasladoCodigo: [ String( this.tipoTrasladoCodigo.direccionFinanciera ) ],
                                            tipoDescuento: [ descuento.tipoDescuentoCodigo, Validators.required ],
                                            valorDescuento: [ descuento.valorDescuento, Validators.required ],
                                            nuevoValorDescuento: [ valorTraslado !== undefined ? ( valorTraslado < 0 ? null : valorTraslado ) : null, Validators.required ]
                                        }
                                    )
                                );
                            } )
                        }
                        */

                        if ( terceroCausacion.ordenGiroDetalleTerceroCausacionDescuento.length > 0 ) {
                            for ( const descuento of listDescuento ) {
                                const ordenGiroDetalleTerceroCausacionDescuento: any[] = terceroCausacion.ordenGiroDetalleTerceroCausacionDescuento.filter( ordenGiroDetalleTerceroCausacionDescuento => ordenGiroDetalleTerceroCausacionDescuento.tipoDescuentoCodigo === descuento.codigo );
                                const listaAportanteDescuentos = [];

                                if ( ordenGiroDetalleTerceroCausacionDescuento.length > 0 ) {
                                    for ( const terceroCausacionDescuento of ordenGiroDetalleTerceroCausacionDescuento ) {
                                        let valorTraslado: number;
                                        let balanceFinancieroTrasladoValorId = 0;
        
                                        if ( this.balanceFinancieroTrasladoValor !== undefined ) {
                                            if ( this.balanceFinancieroTrasladoValor.length > 0 ) {
                                                const balanceFinancieroTrasladoValor = this.balanceFinancieroTrasladoValor.find(
                                                    balanceFinancieroTrasladoValor => balanceFinancieroTrasladoValor.ordenGiroDetalleTerceroCausacionDescuentoId === terceroCausacionDescuento.ordenGiroDetalleTerceroCausacionDescuentoId
                                                    && balanceFinancieroTrasladoValor.tipoTrasladoCodigo === String( this.tipoTrasladoCodigo.direccionFinanciera )
                                                )
        
                                                if ( balanceFinancieroTrasladoValor !== undefined ) {
                                                    valorTraslado = balanceFinancieroTrasladoValor.valorTraslado
                                                    balanceFinancieroTrasladoValorId = balanceFinancieroTrasladoValor.balanceFinancieroTrasladoValorId
                                                    nuevoValorRegistrado += valorTraslado
                                                }
                                            }
                                        }
                                        const nombreAportante = dataAportantes.listaNombreAportante.find( nombre => nombre.cofinanciacionAportanteId === terceroCausacionDescuento.aportanteId );
                                        let listaFuenteRecursos: any[];
                                        let fuente: any;
                                        if ( nombreAportante !== undefined ) {
                                            listaFuenteRecursos = await this.ordenGiroSvc.getFuentesDeRecursosPorAportanteId( nombreAportante.cofinanciacionAportanteId ).toPromise();
                                            fuente = listaFuenteRecursos.find( fuente => fuente.codigo === terceroCausacionDescuento.fuenteRecursosCodigo );
                                        }

                                        listaAportanteDescuentos.push(
                                            this.fb.group(
                                                {
                                                    registroCompleto: [ valorTraslado !== undefined ? true : false ],
                                                    ordenGiroDetalleTerceroCausacionDescuentoId: [ terceroCausacionDescuento.ordenGiroDetalleTerceroCausacionDescuentoId ],
                                                    balanceFinancieroTrasladoValorId: [ balanceFinancieroTrasladoValorId ],
                                                    tipoTrasladoCodigo: [ String( this.tipoTrasladoCodigo.direccionFinanciera ) ],
                                                    nombreAportante: [ nombreAportante !== undefined ? nombreAportante : null, Validators.required ],
                                                    valorDescuento: [ terceroCausacionDescuento.valorDescuento, Validators.required ],
                                                    fuente: [ { value: fuente !== undefined ? fuente : null, disabled: true }, Validators.required ],
                                                    nuevoValorDescuento: [ valorTraslado !== undefined ? ( valorTraslado < 0 ? null : valorTraslado ) : null, Validators.required ]
                                                }
                                            )
                                        )
                                    }

                                    listaDescuentos.push(
                                        this.fb.group(
                                            {
                                                tipoDescuento: [ descuento.codigo, Validators.required ],
                                                aportantesDescuento: this.fb.array( listaAportanteDescuentos )
                                            }
                                        )
                                    );
                                }
                            }

                            terceroCausacion.ordenGiroDetalleTerceroCausacionDescuento.forEach( descuento => {

                                if ( descuento.tipoDescuentoCodigo !== undefined ) {
                                    const descuentoIndex = listDescuento.findIndex( descuentoIndex => descuentoIndex.codigo === descuento.tipoDescuentoCodigo );

                                    if ( descuentoIndex !== -1 ) {
                                        listDescuento.splice( descuentoIndex, 1 );
                                    }
                                }
                            } )
                        }
                        // Get lista de aportantes
                        if ( terceroCausacion.ordenGiroDetalleTerceroCausacionAportante.length > 0 ) {
                            for ( const aportante of terceroCausacion.ordenGiroDetalleTerceroCausacionAportante ) {
                                const nombreAportante = dataAportantes.listaNombreAportante.find( nombre => nombre.cofinanciacionAportanteId === aportante.aportanteId );

                                if ( nombreAportante !== undefined ) {
                                    const tipoAportante = dataAportantes.listaTipoAportante.find( tipo => tipo.dominioId === nombreAportante.tipoAportanteId );
                                    const tipoAportanteIndex = dataAportantes.listaTipoAportante.findIndex( tipo => tipo.dominioId === nombreAportante.tipoAportanteId );
                                    let listaFuenteRecursos: any[] = await this.ordenGiroSvc.getFuentesDeRecursosPorAportanteId( nombreAportante.cofinanciacionAportanteId ).toPromise();
                                    const fuente = listaFuenteRecursos.find( fuente => fuente.codigo === aportante.fuenteRecursoCodigo );
                                    let valorTraslado: number;
                                    let balanceFinancieroTrasladoValorId = 0;

                                    if ( this.balanceFinancieroTrasladoValor !== undefined ) {
                                        if ( this.balanceFinancieroTrasladoValor.length > 0 ) {
                                            const balanceFinancieroTrasladoValor = this.balanceFinancieroTrasladoValor.find(
                                                balanceFinancieroTrasladoValor => balanceFinancieroTrasladoValor.ordenGiroDetalleTerceroCausacionAportanteId === aportante.ordenGiroDetalleTerceroCausacionAportanteId
                                                && balanceFinancieroTrasladoValor.tipoTrasladoCodigo === String( this.tipoTrasladoCodigo.aportante )
                                            )

                                            if ( balanceFinancieroTrasladoValor !== undefined ) {
                                                valorTraslado = balanceFinancieroTrasladoValor.valorTraslado
                                                balanceFinancieroTrasladoValorId = balanceFinancieroTrasladoValor.balanceFinancieroTrasladoValorId
                                                nuevoValorRegistrado += valorTraslado
                                            }
                                        }
                                    }

                                    listaAportantes.push(
                                        this.fb.group(
                                            {
                                                registroCompleto: [ valorTraslado !== undefined ? true : false ],
                                                ordenGiroDetalleTerceroCausacionAportanteId: [ aportante.ordenGiroDetalleTerceroCausacionAportanteId ],
                                                balanceFinancieroTrasladoValorId: [ balanceFinancieroTrasladoValorId ],
                                                tipoTrasladoCodigo: [ String( this.tipoTrasladoCodigo.aportante ) ],
                                                tipoAportante: [ tipoAportante, Validators.required ],
                                                listaNombreAportantes: [ [ nombreAportante ] ],
                                                nombreAportante: [ nombreAportante, Validators.required ],
                                                fuenteDeRecursos: [ listaFuenteRecursos ],
                                                fuenteRecursos: [ fuente, Validators.required ],
                                                fuenteFinanciacionId: [ fuente.fuenteFinanciacionId ],
                                                valorDescuento: [ aportante.valorDescuento, Validators.required ],
                                                nuevoValorDescuento: [ valorTraslado !== undefined ? ( valorTraslado < 0 ? null : valorTraslado ) : null, Validators.required ]
                                            }
                                        )
                                    )
                                }
                            }
                        }

                        if ( solicitudPagoFaseFactura !== undefined ) {
                            if ( solicitudPagoFaseFactura.solicitudPagoFaseFacturaDescuento !== undefined ) {
                                if ( solicitudPagoFaseFactura.solicitudPagoFaseFacturaDescuento.length > 0 ) {
                                    solicitudPagoFaseFactura.solicitudPagoFaseFacturaDescuento.forEach( descuentoTecnica => {
                                        const descuentoTecnicaOrdenGiro = detalleDescuentoTecnica.find( detalleDescuentoTecnica => detalleDescuentoTecnica.solicitudPagoFaseFacturaDescuentoId === descuentoTecnica.solicitudPagoFaseFacturaDescuentoId && detalleDescuentoTecnica.esPreconstruccion === solicitudPagoFase.esPreconstruccion )

                                        if ( descuentoTecnicaOrdenGiro !== undefined ) {
                                            let valorTraslado: number;

                                            if ( this.balanceFinancieroTrasladoValor !== undefined ) {
                                                if ( this.balanceFinancieroTrasladoValor.length > 0 ) {
                                                    const balanceFinancieroTrasladoValor = this.balanceFinancieroTrasladoValor.find(
                                                        balanceFinancieroTrasladoValor => balanceFinancieroTrasladoValor.ordenGiroDetalleDescuentoTecnicaId === descuentoTecnicaOrdenGiro.ordenGiroDetalleDescuentoTecnicaId
                                                        && balanceFinancieroTrasladoValor.tipoTrasladoCodigo === String( this.tipoTrasladoCodigo.direccionTecnica )
                                                    )
        
                                                    if ( balanceFinancieroTrasladoValor !== undefined ) {
                                                        valorTraslado = balanceFinancieroTrasladoValor.valorTraslado
                                                    }
                                                }
                                            }

                                            // console.log( descuentoTecnicaOrdenGiro, valorTraslado, descuentoTecnicaOrdenGiro.ordenGiroDetalleDescuentoTecnicaId )
                                            listaDescuentoTecnica.push(
                                                this.fb.group(
                                                    {
                                                        ordenGiroDetalleDescuentoTecnicaId: [ descuentoTecnicaOrdenGiro.ordenGiroDetalleDescuentoTecnicaId ],
                                                        solicitudPagoFaseFacturaDescuentoId: [ descuentoTecnica.solicitudPagoFaseFacturaDescuentoId ],
                                                        balanceFinancieroTrasladoValorId: [ 0 ],
                                                        tipoTrasladoCodigo: [ String( this.tipoTrasladoCodigo.direccionTecnica ) ],
                                                        tipoDescuento: [ descuentoTecnica.tipoDescuentoCodigo, Validators.required ],
                                                        valorDescuento: [ descuentoTecnica.valorDescuento, Validators.required ],
                                                        nuevoValorDescuento: [ valorTraslado !== undefined ? ( valorTraslado < 0 ? null : valorTraslado ) : null, Validators.required ]
                                                    }
                                                )
                                            )
                                        }
                                    } )
                                }
                            }
                        }

                        for ( const concepto of criterio.listConceptos ) {
                            conceptosDePago.push( this.fb.group(
                                {
                                    conceptoPagoCriterio: [ concepto.codigo ],
                                    nombre: [ concepto.nombre ],
                                    valorFacturadoConcepto: [ concepto.valorFacturadoConcepto ],
                                    tipoDeAportantes: [ dataAportantes.listaTipoAportante ],
                                    nombreDeAportantes: [ dataAportantes.listaNombreAportante ],
                                    tipoDescuentoArray: [ listDescuento ],
                                    descuento: this.fb.group(
                                        {
                                            aplicaDescuentos:[ terceroCausacion.tieneDescuento, Validators.required ],
                                            numeroDescuentos: [ terceroCausacion.ordenGiroDetalleTerceroCausacionDescuento.length > 0 ? terceroCausacion.ordenGiroDetalleTerceroCausacionDescuento.length : null, Validators.required ],
                                            descuentos: this.fb.array( listaDescuentos )
                                        }
                                    ),
                                    descuentoTecnica: this.fb.group(
                                        {
                                            aplicaDescuentos:[ solicitudPagoFaseFactura.tieneDescuento, Validators.required ],
                                            numeroDescuentos: [ solicitudPagoFaseFactura.solicitudPagoFaseFacturaDescuento.length > 0 ? solicitudPagoFaseFactura.solicitudPagoFaseFacturaDescuento.length : null, Validators.required ],
                                            descuentos: this.fb.array( listaDescuentoTecnica )
                                        }
                                    ),
                                    aportantes: this.fb.array( listaAportantes )
                                }
                            ) )
                        }
                        valorNetoGiro = terceroCausacion.valorNetoGiro;
                        let semaforoCriterio = 'sin-diligenciar'
                        let cantidadRegistroCompletoAportante = 0
                        let cantidadRegistroCompletoDescuento = 0

                        listaAportantes.forEach( aportante => {
                            if ( aportante.get( 'registroCompleto' ).value === true ) {
                                cantidadRegistroCompletoAportante++;
                            }
                        } )

                        if ( terceroCausacion.tieneDescuento === true ) {
                            listaDescuentos.forEach( descuento => {
                                const descuentoAportantes = descuento.get( 'aportantesDescuento' ) as FormArray;

                                descuentoAportantes.controls.forEach( descuentoControl => {
                                    if ( descuentoControl.get( 'registroCompleto' ).value === true ) {
                                        cantidadRegistroCompletoDescuento++;
                                    }
                                } )
                            } )
                        }

                        if ( cantidadRegistroCompletoAportante > 0 && cantidadRegistroCompletoAportante === listaAportantes.length && cantidadRegistroCompletoDescuento === 0 && cantidadRegistroCompletoDescuento < listaDescuentos.length ) {
                            semaforoCriterio = 'en-proceso'
                        }
                        if ( cantidadRegistroCompletoAportante > 0 && cantidadRegistroCompletoAportante === listaAportantes.length && cantidadRegistroCompletoDescuento > 0 && cantidadRegistroCompletoDescuento < listaDescuentos.length ) {
                            semaforoCriterio = 'en-proceso'
                        }
                        if ( cantidadRegistroCompletoAportante > 0 && cantidadRegistroCompletoDescuento > 0 && cantidadRegistroCompletoAportante < listaAportantes.length && cantidadRegistroCompletoDescuento < listaDescuentos.length ) {
                            semaforoCriterio = 'en-proceso'
                        }
                        if ( cantidadRegistroCompletoAportante === 0 && cantidadRegistroCompletoDescuento > 0 && cantidadRegistroCompletoDescuento < listaDescuentos.length ) {
                            semaforoCriterio = 'en-proceso'
                        }
                        if ( cantidadRegistroCompletoAportante > 0 && cantidadRegistroCompletoDescuento === 0 && cantidadRegistroCompletoAportante < listaAportantes.length ) {
                            semaforoCriterio = 'en-proceso'
                        }
                        if ( cantidadRegistroCompletoAportante > 0 && cantidadRegistroCompletoDescuento > 0 && cantidadRegistroCompletoAportante === listaAportantes.length && cantidadRegistroCompletoDescuento === listaDescuentos.length ) {
                            semaforoCriterio = 'completo'
                        }
                        if ( cantidadRegistroCompletoAportante > 0 && cantidadRegistroCompletoAportante === listaAportantes.length && cantidadRegistroCompletoDescuento === 0 && cantidadRegistroCompletoDescuento === listaDescuentos.length ) {
                            semaforoCriterio = 'completo'
                        }

                        criteriosArray.push(
                            this.fb.group(
                                {
                                    semaforo: [ semaforoCriterio ],
                                    ordenGiroDetalleTerceroCausacionId: [ terceroCausacion.ordenGiroDetalleTerceroCausacionId ],
                                    tipoCriterioCodigo: [ criterio.tipoCriterioCodigo ],
                                    nombre: [ criterio.nombre ],
                                    tipoPagoCodigo: [ criterio.tipoPagoCodigo ],
                                    conceptos: this.fb.array( conceptosDePago )
                                }
                            ) 
                        )
                    }
                }
            }
            
            const sinDiligenciarCriterio = criteriosArray.find( descuento => descuento.get( 'semaforo' ).value === 'sin-diligenciar' )
            const enProcesoCriterio = criteriosArray.find( descuento => descuento.get( 'semaforo' ).value === 'en-proceso' )
            const completoCriterio = criteriosArray.find( descuento => descuento.get( 'semaforo' ).value === 'completo' )
            let semaforoFase = 'sin-diligenciar'

            if ( sinDiligenciarCriterio !== undefined && enProcesoCriterio !== undefined && completoCriterio === undefined ) {
                semaforoFase = 'en-proceso'
            }
            if ( sinDiligenciarCriterio !== undefined && completoCriterio !== undefined && enProcesoCriterio === undefined ) {
                semaforoFase = 'en-proceso'
            }
            if ( sinDiligenciarCriterio === undefined && completoCriterio !== undefined && enProcesoCriterio !== undefined ) {
                semaforoFase = 'en-proceso'
            }
            if ( sinDiligenciarCriterio === undefined && completoCriterio !== undefined && enProcesoCriterio === undefined ) {
                semaforoFase = 'completo'
            }

            this.terceroCausacion.push(
                this.fb.group(
                    {
                        valorNetoGiro,
                        nuevoValorRegistrado,
                        semaforo: [ semaforoFase ],
                        esPreconstruccion: solicitudPagoFase.esPreconstruccion,
                        criterios: this.fb.array( criteriosArray )
                    }
                )
            )
        }

        const sinDiligenciarfase = this.terceroCausacion.controls.find( descuento => descuento.get( 'semaforo' ).value === 'sin-diligenciar' )
        const enProcesofase = this.terceroCausacion.controls.find( descuento => descuento.get( 'semaforo' ).value === 'en-proceso' )
        const completofase = this.terceroCausacion.controls.find( descuento => descuento.get( 'semaforo' ).value === 'completo' )
        let estadoSemaforo = 'sin-diligenciar'

        if ( sinDiligenciarfase !== undefined && enProcesofase !== undefined && completofase === undefined ) {
            estadoSemaforo = 'en-proceso'
        }
        if ( sinDiligenciarfase !== undefined && completofase !== undefined && enProcesofase === undefined ) {
            estadoSemaforo = 'en-proceso'
        }
        if ( sinDiligenciarfase === undefined && completofase !== undefined && enProcesofase !== undefined ) {
            estadoSemaforo = 'en-proceso'
        }
        if ( sinDiligenciarfase === undefined && completofase !== undefined && enProcesofase === undefined ) {
            estadoSemaforo = 'completo'
        }

        this.estadoSemaforo.emit( estadoSemaforo )
    }

    firstLetterUpperCase( texto:string ) {
        if ( texto !== undefined ) {
            return humanize.capitalize( String( texto ).toLowerCase() );
        }
    }

    getDescuento( codigo: string ) {
        if ( this.tipoDescuentoArray.length > 0 ) {
            const descuento = this.tipoDescuentoArray.find( descuento => descuento.codigo === codigo );

            if ( descuento !== undefined ) {
                return descuento.nombre;
            }
        }
    }

    getDescuentoTecnica( codigo: string ) {
        if ( this.tipoDescuentoTecnicaArray.length > 0 ) {
            const descuento = this.tipoDescuentoTecnicaArray.find( descuento => descuento.codigo === codigo );

            if ( descuento !== undefined ) {
                return descuento.nombre;
            }
        }
    }

    checkValueAportante( value: number, index: number, jIndex: number, kIndex: number, lIndex: number, mIndex: number, tipoValor: string ) {
        if ( value !== null ) {
            if ( value < 0 ) {
                if ( tipoValor === 'aportante' ) {
                    this.getAportantes( index, jIndex, kIndex ).controls[ lIndex ].get( 'nuevoValorDescuento' ).setValue( null )
                }

                if ( tipoValor === 'descuentoFinanciera' ) {
                    this.getAportanteDescuentos( index, jIndex, kIndex, lIndex ).controls[ mIndex ].get( 'nuevoValorDescuento' ).setValue( null )
                }

                if ( tipoValor === 'descuentoTecnica' ) {
                    this.getDescuentosTecnica( index, jIndex, kIndex ).controls[ lIndex ].get( 'nuevoValorDescuento' ).setValue( null )
                }
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

            this.getCriterios( index ).controls.forEach( ( criterioControl, indexCriterio ) => {
                if ( criterioControl.dirty === true ) {
                    this.getConceptos( index, indexCriterio ).controls.forEach( ( conceptoControl, indexConcepto ) => {
                        this.getAportantes( index, indexCriterio, indexConcepto ).controls.forEach( aportanteControl => {
                            if ( aportanteControl.dirty === true ) {
                                balanceFinancieroTraslado.push(
                                    {
                                        ordenGiroId: this.ordenGiroId,
                                        balanceFinancieroId: this.balanceFinancieroId,
                                        esPreconstruccion: this.terceroCausacion.controls[ index ].get( 'esPreconstruccion' ).value,
                                        balanceFinancieroTrasladoValorId: aportanteControl.get( 'balanceFinancieroTrasladoValorId' ).value,
                                        tipoTrasladoCodigo: aportanteControl.get( 'tipoTrasladoCodigo' ).value,
                                        ordenGiroDetalleTerceroCausacionAportanteId: aportanteControl.get( 'ordenGiroDetalleTerceroCausacionAportanteId' ).value,
                                        valorTraslado: aportanteControl.get( 'nuevoValorDescuento' ).value
                                    }
                                )
                            }
                        } )
                        
                        if ( this.getDescuentosFinanciera( index, indexCriterio, indexConcepto ).length > 0 ) {
                            this.getDescuentosFinanciera( index, indexCriterio, indexConcepto ).controls.forEach( ( descuentoFinanciera, indexDescFinanciera ) => {
                                this.getAportanteDescuentos( index, indexCriterio, indexConcepto, indexDescFinanciera ).controls.forEach( aportanteDescControl => {
                                    if ( aportanteDescControl.dirty === true ) {
                                        balanceFinancieroTraslado.push(
                                            {
                                                ordenGiroId: this.ordenGiroId,
                                                balanceFinancieroId: this.balanceFinancieroId,
                                                esPreconstruccion: this.terceroCausacion.controls[ index ].get( 'esPreconstruccion' ).value,
                                                balanceFinancieroTrasladoValorId: aportanteDescControl.get( 'balanceFinancieroTrasladoValorId' ).value,
                                                tipoTrasladoCodigo: aportanteDescControl.get( 'tipoTrasladoCodigo' ).value,
                                                ordenGiroDetalleTerceroCausacionDescuentoId: aportanteDescControl.get( 'ordenGiroDetalleTerceroCausacionDescuentoId' ).value,
                                                valorTraslado: aportanteDescControl.get( 'nuevoValorDescuento' ).value
                                            }
                                        )
                                    }
                                } )
                            } )
                        }
                        if ( this.getDescuentosTecnica( index, indexCriterio, indexConcepto ).length > 0 ) {
                            this.getDescuentosTecnica( index, indexCriterio, indexConcepto ).controls.forEach( descuentoTecnica => {
                                if ( descuentoTecnica.dirty === true ) {
                                    balanceFinancieroTraslado.push(
                                        {
                                            ordenGiroId: this.ordenGiroId,
                                            balanceFinancieroId: this.balanceFinancieroId,
                                            esPreconstruccion: this.terceroCausacion.controls[ index ].get( 'esPreconstruccion' ).value,
                                            balanceFinancieroTrasladoValorId: descuentoTecnica.get( 'balanceFinancieroTrasladoValorId' ).value,
                                            tipoTrasladoCodigo: descuentoTecnica.get( 'tipoTrasladoCodigo' ).value,
                                            ordenGiroDetalleDescuentoTecnicaId: descuentoTecnica.get( 'ordenGiroDetalleDescuentoTecnicaId' ).value,
                                            valorTraslado: descuentoTecnica.get( 'nuevoValorDescuento' ).value
                                        }
                                    )
                                }
                            } )
                        }
                    } )
                }
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
