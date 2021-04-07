import { Router } from '@angular/router';
import { OrdenPagoService } from './../../../../core/_services/ordenPago/orden-pago.service';
import { Component, Input, OnInit, OnChanges, SimpleChanges, Output, EventEmitter } from '@angular/core';
import { Validators, FormBuilder, FormArray } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { RegistrarRequisitosPagoService } from 'src/app/core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import humanize from 'humanize-plus';
import { ListaMenu, ListaMenuId, TipoObservaciones, TipoObservacionesCodigo } from 'src/app/_interfaces/estados-solicitudPago-ordenGiro.interface';
import { ObservacionesOrdenGiroService } from 'src/app/core/_services/observacionesOrdenGiro/observaciones-orden-giro.service';

@Component({
  selector: 'app-form-descuentos-gog',
  templateUrl: './form-descuentos-gog.component.html',
  styleUrls: ['./form-descuentos-gog.component.scss']
})
export class FormDescuentosGogComponent implements OnInit, OnChanges {

    @Input() solicitudPagoFaseCriterio: any[];
    @Input() solicitudPagoFaseFacturaDescuento: any[];
    @Input() listaCriterios: Dominio[] = [];
    @Input() solicitudPago: any;
    @Input() descuento: any;
    @Input() valorNetoGiro: number;
    @Input() esVerDetalle: boolean;
    @Output() tieneObservacion = new EventEmitter<boolean>();
    listaMenu: ListaMenu = ListaMenuId;
    tipoObservaciones: TipoObservaciones = TipoObservacionesCodigo;
    estaEditando = false;
    recibeListaCriterios = false;
    cantidadAportantes: number;
    ordenGiroDetalle: any;
    ordenGiroId = 0;
    ordenGiroDetalleId = 0;
    criteriosArray: Dominio[] = [];
    tiposDescuentoArray: Dominio[] = [];
    listaConceptoPago: Dominio[] = [];
    listaFuenteTipoFinanciacion: Dominio[] = [];
    addressForm = this.fb.group(
        {
            descuentos: this.fb.array( [] )
        }
    );

    get descuentos() {
        return this.addressForm.get( 'descuentos' ) as FormArray;
    }

    getCriterios( index: number ) {
        return this.descuentos.controls[ index ].get( 'criterios' ) as FormArray;
    }

    getConceptos( index: number, jIndex: number ) {
        return this.getCriterios( index ).controls[ jIndex ].get( 'conceptos' ) as FormArray;
    }

    getAportantes( index: number, jIndex: number, kIndex: number ) {
        return this.getConceptos( index, jIndex ).controls[ kIndex ].get( 'aportantes' ) as FormArray;
    }

    constructor(
        private dialog: MatDialog,
        private commonSvc: CommonService,
        private registrarPagosSvc: RegistrarRequisitosPagoService,
        private obsOrdenGiro: ObservacionesOrdenGiroService,
        private ordenGiroSvc: OrdenPagoService,
        private routes: Router,
        private fb: FormBuilder )
    {
        this.commonSvc.listaFuenteTipoFinanciacion()
            .subscribe( listaFuenteTipoFinanciacion => this.listaFuenteTipoFinanciacion = listaFuenteTipoFinanciacion );
        this.commonSvc.tiposDescuento()
            .subscribe(response => this.tiposDescuentoArray = response);
    }

    ngOnChanges( changes: SimpleChanges ): void {
        if ( changes.listaCriterios.currentValue.length > 0 || changes.listaCriterios.currentValue !== undefined ) {
            this.criteriosArray = [ ...changes.listaCriterios.currentValue ];

            if ( this.solicitudPago !== undefined ) {
                const solicitudPagoFaseCriterio = this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase[0].solicitudPagoFaseCriterio;

                this.criteriosArray.forEach( ( criterio, indexCriterio ) => {
                    const criterioFind = solicitudPagoFaseCriterio.find( value => value.tipoCriterioCodigo === criterio.codigo );

                    if ( criterioFind === undefined ) {
                        this.criteriosArray.splice( indexCriterio, 1 );
                    }
                } );
            }
        }
    }

    ngOnInit(): void {
        if ( this.criteriosArray.length > 0 ) {
            this.getDescuentos();
        }
    }

    async getDescuentos() {
        // Get IDs
        if ( this.solicitudPago.ordenGiro !== undefined ) {
            this.ordenGiroId = this.solicitudPago.ordenGiro.ordenGiroId;

            if ( this.solicitudPago.ordenGiro.ordenGiroDetalle !== undefined ) {
                if ( this.solicitudPago.ordenGiro.ordenGiroDetalle.length > 0 ) {
                    this.ordenGiroDetalle = this.solicitudPago.ordenGiro.ordenGiroDetalle[0];
                    this.ordenGiroDetalleId = this.ordenGiroDetalle.ordenGiroDetalleId;
                }
            }
        }
        // Set data Formulario
        if ( this.ordenGiroDetalle !== undefined ) {
            const ordenGiroDetalleDescuentoTecnica: any[] = this.ordenGiroDetalle.ordenGiroDetalleDescuentoTecnica;

            if ( ordenGiroDetalleDescuentoTecnica.length > 0 ) {
                for ( const descuento of this.solicitudPagoFaseFacturaDescuento ) {
                    const formArrayCriterios = [];
                    const detalleDescuentoTecnica = ordenGiroDetalleDescuentoTecnica.filter( descuentoTecnica => descuentoTecnica.solicitudPagoFaseFacturaDescuentoId === descuento.solicitudPagoFaseFacturaDescuentoId );

                    if ( detalleDescuentoTecnica.length > 0 ) {
                        // Get forArray de los criterios
                        const listaCriterios = [];
                        detalleDescuentoTecnica.forEach( descuentoValue => listaCriterios.push( descuentoValue.criterioCodigo ) );

                        if ( listaCriterios.length > 0 ) {
                            for ( const codigo of listaCriterios ) {
                                const criterio = this.solicitudPagoFaseCriterio.find( criterio => criterio.tipoCriterioCodigo === codigo );

                                if ( criterio !== undefined ) {
                                    const tiposDePago = await this.registrarPagosSvc.getTipoPagoByCriterioCodigo( codigo );
                                    const tipoPago = tiposDePago.find( tipoPago => tipoPago.codigo === criterio.tipoPagoCodigo );
                                    const conceptosDePago = await this.registrarPagosSvc.getConceptoPagoCriterioCodigoByTipoPagoCodigo( tipoPago.codigo );
                                    // Get data del formulario de los conceptos seleccionados
                                    const listaConceptos = [];
                                    const formArrayConceptos = [];
                                    const ordenGiroDetalleDescuentoTecnicaAportante = [];

                                    detalleDescuentoTecnica.forEach( descuentoValue => {
                                        descuentoValue.ordenGiroDetalleDescuentoTecnicaAportante.forEach( value => {
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
                                    } )
                                    for ( const codigo of listaConceptos ) {
                                        const concepto = conceptosDePago.find( concepto => concepto.codigo === codigo );
                            
                                        if ( concepto !== undefined ) {
                                            this.ordenGiroSvc.getAportantes( this.solicitudPago, dataAportantes => {
                                                const formArrayAportantes = [];
                                                // Get cantidad de aportantes para limitar cuantos aportantes se pueden agregar en el formulario
                                                this.cantidadAportantes = dataAportantes.listaTipoAportante.length;

                                                if ( ordenGiroDetalleDescuentoTecnicaAportante.length > 0 ) {
                                                    for ( const aportante of ordenGiroDetalleDescuentoTecnicaAportante ) {
                                                        const nombreAportante = dataAportantes.listaNombreAportante.find( nombre => nombre.cofinanciacionAportanteId === aportante.aportanteId );
                                                        const tipoAportante = dataAportantes.listaTipoAportante.find( tipo => tipo.dominioId === nombreAportante.tipoAportanteId );
                                                        let listaFuenteRecursos: any[];
                                                        this.ordenGiroSvc.getFuentesDeRecursosPorAportanteId( nombreAportante.cofinanciacionAportanteId )
                                                            .subscribe( fuenteRecursos => {
                                                                listaFuenteRecursos = fuenteRecursos;

                                                                formArrayAportantes.push(
                                                                    this.fb.group(
                                                                        {
                                                                            ordenGiroDetalleDescuentoTecnicaAportanteId: [ aportante.ordenGiroDetalleDescuentoTecnicaAportanteId ],
                                                                            tipoAportante: [ tipoAportante, Validators.required ],
                                                                            listaNombreAportantes: [ [ nombreAportante ] ],
                                                                            nombreAportante: [ nombreAportante, Validators.required ],
                                                                            fuenteDeRecursos: [ listaFuenteRecursos ],
                                                                            fuenteRecursos: [ aportante.fuenteRecursosCodigo, Validators.required ],
                                                                            valorDescuento: [ aportante.valorDescuento, Validators.required ]
                                                                        }
                                                                    )
                                                                )
                                                            } );
                                                    }
                                                }

                                                setTimeout(() => {
                                                    formArrayConceptos.push( this.fb.group(
                                                        {
                                                            nombre: [ concepto.nombre ],
                                                            conceptoCodigo: [ concepto.codigo ],
                                                            tipoDeAportantes: [ dataAportantes.listaTipoAportante ],
                                                            nombreDeAportantes: [ dataAportantes.listaNombreAportante ],
                                                            aportantes: this.fb.array( formArrayAportantes )
                                                        }
                                                    ) );
                                                }, 1000);
                                            } );
                                        }
                                    }

                                    setTimeout(() => {
                                        // Set formulario de los criterios
                                        formArrayCriterios.push(
                                            this.fb.group(
                                                {
                                                    nombre: [ this.criteriosArray.find( criterio => criterio.codigo === codigo ).nombre ],
                                                    criterioCodigo: [ this.criteriosArray.find( criterio => criterio.codigo === codigo ).codigo ],
                                                    tipoPagoNombre: [ tipoPago.nombre ],
                                                    tipoPagoCodigo: [ tipoPago.codigo ],
                                                    conceptosDePago: [ conceptosDePago ],
                                                    concepto: [ listaConceptos, Validators.required ],
                                                    conceptos: this.fb.array( formArrayConceptos )
                                                }
                                            )
                                        )
                                    }, 1500);
                                }
                            }
                        }
                        let estadoSemaforo = 'sin-diligenciar';
                        let totalRegistroCompleto = 0;
                        detalleDescuentoTecnica.forEach( descuento => {
                            if ( descuento.registroCompleto === true ) {
                                totalRegistroCompleto++;
                            }
                        } );

                        if ( totalRegistroCompleto === detalleDescuentoTecnica.length ) {
                            estadoSemaforo = 'completo';
                        } else {
                            estadoSemaforo = 'en.proceso';
                        }
                        if ( totalRegistroCompleto === 0 ) {
                            estadoSemaforo = 'en-proceso';
                        }
                        setTimeout( async () => {
                            // Set formulario de los descuentos
                            for ( const descuentoValue of detalleDescuentoTecnica ) {
                                // Get observaciones
                                const listaObservacionVerificar = await this.obsOrdenGiro.getObservacionOrdenGiroByMenuIdAndSolicitudPagoId(
                                    this.listaMenu.verificarOrdenGiro,
                                    this.ordenGiroId,
                                    descuentoValue.ordenGiroDetalleDescuentoTecnicaId,
                                    this.tipoObservaciones.direccionTecnica );
                                const listaObservacionAprobar = await this.obsOrdenGiro.getObservacionOrdenGiroByMenuIdAndSolicitudPagoId(
                                    this.listaMenu.aprobarOrdenGiro,
                                    this.ordenGiroId,
                                    descuentoValue.ordenGiroDetalleDescuentoTecnicaId,
                                    this.tipoObservaciones.direccionTecnica );
                                const listaObservacionTramitar = await this.obsOrdenGiro.getObservacionOrdenGiroByMenuIdAndSolicitudPagoId(
                                        this.listaMenu.tramitarOrdenGiro,
                                        this.ordenGiroId,
                                        descuentoValue.ordenGiroDetalleDescuentoTecnicaId,
                                        this.tipoObservaciones.direccionTecnica );
                                // Get lista de observacion y observacion actual
                                let obsVerificar = undefined;
                                let obsAprobar = undefined;
                                let obsTramitar = undefined;
                                if ( listaObservacionVerificar.find( obs => obs.archivada === false ) !== undefined ) {
                                    if ( listaObservacionVerificar.find( obs => obs.archivada === false ).tieneObservacion === true ) {
                                        obsVerificar = listaObservacionVerificar.find( obs => obs.archivada === false );
                                    }
                                }
                                if ( listaObservacionAprobar.find( obs => obs.archivada === false ) !== undefined ) {
                                    if ( listaObservacionAprobar.find( obs => obs.archivada === false ).tieneObservacion === true ) {
                                        obsAprobar = listaObservacionAprobar.find( obs => obs.archivada === false );
                                    }
                                }
                                if ( listaObservacionTramitar.find( obs => obs.archivada === false ) !== undefined ) {
                                    if ( listaObservacionTramitar.find( obs => obs.archivada === false ).tieneObservacion === true ) {
                                        obsTramitar = listaObservacionTramitar.find( obs => obs.archivada === false );
                                    }
                                }

                                if ( obsVerificar !== undefined ) {
                                    estadoSemaforo = 'en-proceso';
                                    this.tieneObservacion.emit( true );
                                }
                                if ( obsAprobar !== undefined ) {
                                    estadoSemaforo = 'en-proceso';
                                    this.tieneObservacion.emit( true );
                                }
                                if ( obsTramitar !== undefined ) {
                                    estadoSemaforo = 'en-proceso';
                                    this.tieneObservacion.emit( true );
                                }
                                this.descuentos.push( this.fb.group(
                                    {
                                        estadoSemaforo,
                                        obsVerificar: [ obsVerificar ],
                                        obsAprobar: [ obsAprobar ],
                                        obsTramitar: [ obsTramitar ],
                                        ordenGiroDetalleDescuentoTecnicaId: [ descuentoValue.ordenGiroDetalleDescuentoTecnicaId ],
                                        solicitudPagoFaseFacturaDescuentoId: [ descuentoValue.solicitudPagoFaseFacturaDescuentoId ],
                                        tipoDescuentoCodigo: [ descuento.tipoDescuentoCodigo ],
                                        criterio: [ listaCriterios, Validators.required ],
                                        criterios: this.fb.array( formArrayCriterios )
                                    }
                                ) );
                            }
                        }, 2000);
                    }
                }
            } else {
                this.solicitudPagoFaseFacturaDescuento.forEach( descuento => {

                    this.descuentos.push( this.fb.group(
                        {
                            estadoSemaforo: [ 'sin-diligenciar' ],
                            ordenGiroDetalleDescuentoTecnicaId: [ 0 ],
                            solicitudPagoFaseFacturaDescuentoId: [ descuento.solicitudPagoFaseFacturaDescuentoId ],
                            tipoDescuentoCodigo: [ descuento.tipoDescuentoCodigo ],
                            criterio: [ null, Validators.required ],
                            criterios: this.fb.array( [] )
                        }
                    ) );
    
                } );
            }
        } else {
            this.solicitudPagoFaseFacturaDescuento.forEach( descuento => {

                this.descuentos.push( this.fb.group(
                    {
                        estadoSemaforo: [ 'sin-diligenciar' ],
                        ordenGiroDetalleDescuentoTecnicaId: [ 0 ],
                        solicitudPagoFaseFacturaDescuentoId: [ descuento.solicitudPagoFaseFacturaDescuentoId ],
                        tipoDescuentoCodigo: [ descuento.tipoDescuentoCodigo ],
                        criterio: [ null, Validators.required ],
                        criterios: this.fb.array( [] )
                    }
                ) );

            } );
        }
    }

    firstLetterUpperCase( texto:string ) {
        if ( texto !== undefined ) {
            return humanize.capitalize( String( texto ).toLowerCase() );
        }
    }

    totalDescuentos( cb: { ( totalDescuento: number ): void } ) {
        const totaldescuentos = (): number => {
            let total = 0;
            this.descuentos.controls.forEach( ( descuentoControl, indexDescuento ) => {
                this.getCriterios( indexDescuento ).controls.forEach( ( criterioControl, indexCriterio ) => {
                    this.getConceptos( indexDescuento, indexCriterio ).controls.forEach( ( conceptoControl, indexConcepto ) => {
                        this.getAportantes( indexDescuento, indexCriterio, indexConcepto ).controls.forEach(
                            aportanteControl => aportanteControl.get( 'valorDescuento' ).value !== null ? total += aportanteControl.get( 'valorDescuento' ).value : total += 0
                        );
                    } );
                } );
            } )

            return total;
        }
        
        cb( totaldescuentos() );
    }

    checkTotalDiscountValues( index: number, jIndex: number, kIndex: number, lIndex: number ) {
        this.totalDescuentos( totalDescuento => {
            if ( totalDescuento > this.valorNetoGiro ) {
                this.getAportantes( index, jIndex, kIndex ).controls[ lIndex ].get( 'valorDescuento' ).setValue( null );
                this.openDialog( '', '<b>El valor total de los descuentos no puede ser mayor al valor neto de giro.</b>' )
            }
        } );
    }

    getTipoDescuento( tipoDescuentoCodigo: string ) {
        if (this.tiposDescuentoArray.length > 0) {
            const descuento = this.tiposDescuentoArray.find( descuento => descuento.codigo === tipoDescuentoCodigo );
            
            if ( descuento !== undefined ) {
                return descuento.nombre;
            }
        }
    }

    getValueCriterio( codigo: string ) {
        if ( this.criteriosArray.length > 0 ) {
            const criterio = this.criteriosArray.find( criterio => criterio.codigo === codigo );

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

    async getListCriterios( list: string[], index: number ) {
        const listaCriterios = [ ...list ];

        if ( listaCriterios.length === 0 ) {
            this.getCriterios( index ).clear();
            return;
        }

        if ( this.getCriterios( index ).length > 0 ) {
            this.getCriterios( index ).controls.forEach( ( control, indexControl ) => {
                const criterioIndex = listaCriterios.findIndex( codigo => codigo === control.get( 'criterioCodigo' ).value );
                const criterio = listaCriterios.find( criterio => criterio === control.get( 'criterioCodigo' ).value );

                if ( criterioIndex !== -1 ) {
                    listaCriterios.splice( criterioIndex, 1 );
                }

                if ( criterio === undefined ) {
                    this.getCriterios( index ).removeAt( indexControl );
                    listaCriterios.splice( criterioIndex, 1 );
                }
            } );
        }

        for ( const codigo of listaCriterios ) {
            const criterio = this.solicitudPagoFaseCriterio.find( criterio => criterio.tipoCriterioCodigo === codigo );

            if ( criterio !== undefined ) {
                const tiposDePago = await this.registrarPagosSvc.getTipoPagoByCriterioCodigo( codigo );
                const tipoPago = tiposDePago.find( tipoPago => tipoPago.codigo === criterio.tipoPagoCodigo );
                const conceptosDePago = await this.registrarPagosSvc.getConceptoPagoCriterioCodigoByTipoPagoCodigo( tipoPago.codigo );

                this.getCriterios( index ).push( this.fb.group(
                    {
                        nombre: [ this.criteriosArray.find( criterio => criterio.codigo === codigo ).nombre ],
                        criterioCodigo: [ this.criteriosArray.find( criterio => criterio.codigo === codigo ).codigo ],
                        tipoPagoNombre: [ tipoPago.nombre ],
                        tipoPagoCodigo: [ tipoPago.codigo ],
                        conceptosDePago: [ conceptosDePago ],
                        concepto: [ null, Validators.required ],
                        conceptos: this.fb.array( [] )
                    }
                ) );
            }
        }
    }

    async getListConceptos( list: string[], index: number, jIndex: number ) {
        const listaConceptos = [ ...list ];
        const conceptosDePago: Dominio[] = this.getCriterios( index ).controls[ jIndex ].get( 'conceptosDePago' ).value;

        if ( listaConceptos.length === 0 ) {
            this.getConceptos( index, jIndex ).clear();
            return;
        }

        if ( this.getConceptos( index, jIndex ).length > 0 ) {
            this.getConceptos( index, jIndex ).controls.forEach( ( control, indexControl ) => {
                const conceptoIndex = listaConceptos.findIndex( concepto => concepto === control.get( 'conceptoCodigo' ).value );
                const concepto = listaConceptos.find( concepto => concepto === control.get( 'conceptoCodigo' ).value );

                if ( conceptoIndex !== -1 ) {
                    listaConceptos.splice( conceptoIndex, 1 );
                }

                if ( concepto === undefined ) {
                    this.getConceptos( index, jIndex ).removeAt( indexControl );
                    listaConceptos.splice( conceptoIndex, 1 );
                }
            } );
        }
        
        for ( const codigo of listaConceptos ) {
            const concepto = conceptosDePago.find( concepto => concepto.codigo === codigo );

            if ( concepto !== undefined ) {
                this.ordenGiroSvc.getAportantes( this.solicitudPago, dataAportantes => {
                    // Get cantidad de aportantes para limitar cuantos aportantes se pueden agregar en el formulario
                    this.cantidadAportantes = dataAportantes.listaTipoAportante.length;

                    this.getConceptos( index, jIndex ).push( this.fb.group(
                        {
                            nombre: [ concepto.nombre ],
                            conceptoCodigo: [ concepto.codigo ],
                            tipoDeAportantes: [ dataAportantes.listaTipoAportante ],
                            nombreDeAportantes: [ dataAportantes.listaNombreAportante ],
                            aportantes: this.fb.array( [
                                this.fb.group(
                                    {
                                        ordenGiroDetalleDescuentoTecnicaAportanteId: [ 0 ],
                                        tipoAportante: [ null, Validators.required ],
                                        listaNombreAportantes: [ null ],
                                        nombreAportante: [ null, Validators.required ],
                                        fuenteDeRecursos: [ null ],
                                        fuenteRecursos: [ null, Validators.required ],
                                        valorDescuento: [ null, Validators.required ]
                                    }
                                )
                            ] )
                        }
                    ) );
                } );
            }
        }
    }

    valuePendingTipoAportante( aportanteSeleccionado: Dominio, index: number, jIndex: number, kIndex: number, lIndex: number ) {
        const listaAportantes: Dominio[] = this.getConceptos( index, jIndex ).controls[ kIndex ].get( 'tipoDeAportantes' ).value;
        const aportanteIndex = listaAportantes.findIndex( aportante => aportante.codigo === aportanteSeleccionado.codigo );
        const listaNombreAportantes: any[] = this.getConceptos( index, jIndex ).controls[ kIndex ].get( 'nombreDeAportantes' ).value;
        const filterAportantesDominioId = listaNombreAportantes.filter( aportante => aportante.tipoAportanteId === aportanteSeleccionado.dominioId );

        if ( aportanteIndex !== -1 ) {
            listaAportantes.splice( aportanteIndex, 1 );

            this.getConceptos( index, jIndex ).controls[ kIndex ].get( 'tipoDeAportantes' ).setValue( listaAportantes );
        }
        if ( filterAportantesDominioId.length > 0 ) {
            this.getAportantes( index, jIndex, kIndex ).controls[ lIndex ].get( 'listaNombreAportantes' ).setValue( filterAportantesDominioId );
        }
    }

    deleteAportante( index: number, jIndex: number, kIndex: number, lIndex: number ) {
        this.openDialogTrueFalse( '', '<b>¿Está seguro de eliminar esta información?</b>' )
            .subscribe(
                value => {
                    if ( value === true ) {
                        const aportanteSeleccionado = this.getAportantes( index, jIndex, kIndex ).controls[ lIndex ].get( 'tipoAportante' ).value;
                        const listaTipoAportantes = this.getConceptos( index, jIndex ).controls[ kIndex ].get( 'tipoDeAportantes' ).value;
                        listaTipoAportantes.push( aportanteSeleccionado );
                        this.getConceptos( index, jIndex ).controls[ kIndex ].get( 'tipoDeAportantes' ).setValue( listaTipoAportantes );

                        if ( this.getAportantes( index, jIndex, kIndex ).controls[ lIndex ].get( 'ordenGiroDetalleDescuentoTecnicaAportanteId' ).value !== 0 ) {
                            this.ordenGiroSvc.deleteOrdenGiroDetalleDescuentoTecnicaAportante( this.getAportantes( index, jIndex, kIndex ).controls[ lIndex ].get( 'ordenGiroDetalleDescuentoTecnicaAportanteId' ).value )
                                .subscribe(
                                    response => {
                                        this.getAportantes( index, jIndex, kIndex ).removeAt( lIndex );
                                        this.openDialog( '', response.message );
                                    },
                                    err => this.openDialog( '', err.message )
                                )
                        } else {
                            this.getAportantes( index, jIndex, kIndex ).removeAt( lIndex );
                            this.openDialog( '', '<b>La información se ha eliminado correctamente.</b>' );
                        }
                    }
                }
            )
    }

    deleteConcepto( index: number, jIndex: number, kIndex: number ) {
        this.openDialogTrueFalse( '', '<b>¿Está seguro de eliminar esta información?</b>' )
        .subscribe(
            value => {
                if ( value === true ) {
                    const listConceptos: string[] = this.getCriterios( index ).controls[ jIndex ].get( 'concepto' ).value;
                    const indexConcepto = listConceptos.findIndex( codigo => codigo === this.getConceptos( index, jIndex ).controls[ kIndex ].get( 'conceptoCodigo' ).value );
                    listConceptos.splice( indexConcepto, 1 );

                    this.getCriterios( index ).controls[ jIndex ].get( 'concepto' ).setValue( listConceptos.length > 0 ? listConceptos : null );
                    this.getConceptos( index, jIndex ).removeAt( kIndex );
                    this.openDialog( '', '<b>La información se ha eliminado correctamente.</b>' );
                }
            }
        )
    }

    deleteCriterio( index: number, jIndex: number ) {
        this.openDialogTrueFalse( '', '<b>¿Está seguro de eliminar esta información?</b>' )
            .subscribe(
                value => {
                    if ( value === true ) {
                        const listCriterio: string[] =  this.descuentos.controls[ index ].get( 'criterio' ).value;
                        const indexCriterio = listCriterio.findIndex( codigo => codigo === this.getCriterios( index ).controls[ jIndex ].get( 'criterioCodigo' ).value )
                        listCriterio.splice( indexCriterio, 1 );

                        this.descuentos.controls[ index ].get( 'criterio' ).setValue( listCriterio.length > 0 ? listCriterio : null );
                        this.getCriterios( index ).removeAt( jIndex );
                        this.openDialog( '', '<b>La información se ha eliminado correctamente.</b>' );
                    }
                }
            );
    }

    addAportante( index: number, jIndex: number, kIndex: number ) {

        if ( this.getAportantes( index, jIndex, kIndex ).length < this.cantidadAportantes ) {
            this.getAportantes( index, jIndex, kIndex ).push(
                this.fb.group(
                    {
                        ordenGiroDetalleDescuentoTecnicaAportanteId: [ 0 ],
                        tipoAportante: [ null, Validators.required ],
                        listaNombreAportantes: [ null ],
                        nombreAportante: [ null, Validators.required ],
                        fuenteDeRecursos: [ null ],
                        fuenteRecursos: [ null, Validators.required ],
                        valorDescuento: [ null, Validators.required ]
                    }
                )
            );
        } else {
            this.openDialog( '', '<b>El contrato no tiene más aportantes asignados.</b>' )
        }
    }

    getListaFuenteRecursos( nombreAportante: any, index: number, jIndex: number, kIndex: number, lIndex: number ) {
        this.ordenGiroSvc.getFuentesDeRecursosPorAportanteId( nombreAportante.cofinanciacionAportanteId )
            .subscribe( fuenteRecursos => this.getAportantes( index, jIndex, kIndex ).controls[ lIndex ].get( 'fuenteDeRecursos' ).setValue( fuenteRecursos ) );
    }

    openDialog(modalTitle: string, modalText: string) {
        const dialogRef = this.dialog.open( ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText }
        });
    }

    openDialogTrueFalse(modalTitle: string, modalText: string) {

        const dialogRef = this.dialog.open( ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText, siNoBoton: true }
        });

        return dialogRef.afterClosed();
    }

    onSubmit( index: number ) {
        this.estaEditando = true;
        this.addressForm.markAllAsTouched();

        const getOrdenGiroDetalleDescuentoTecnica = ( ) => {
            const listaDescuentosTecnica = [];

            this.descuentos.controls.forEach( ( descuentoControl, indexDescuento ) => {
                let ordenGiroDetalleDescuentoTecnica: any;
    
                this.getCriterios( indexDescuento ).controls.forEach( ( criterioControl, indexCriterio ) => {
                    ordenGiroDetalleDescuentoTecnica = {
                        ordenGiroDetalleId: this.ordenGiroDetalleId,
                        ordenGiroDetalleDescuentoTecnicaId: descuentoControl.get( 'ordenGiroDetalleDescuentoTecnicaId' ).value,
                        solicitudPagoFaseFacturaDescuentoId: descuentoControl.get( 'solicitudPagoFaseFacturaDescuentoId' ).value,
                        tipoPagoCodigo: criterioControl.get( 'tipoPagoCodigo' ).value,
                        criterioCodigo: criterioControl.get( 'criterioCodigo' ).value,
                        ordenGiroDetalleDescuentoTecnicaAportante: []
                    };
    
                    this.getConceptos( indexDescuento, indexCriterio ).controls.forEach( ( conceptoControl, indexConcepto ) => {
    
                        this.getAportantes( indexDescuento, indexCriterio, indexConcepto ).controls.forEach( aportanteControl => {
    
                            ordenGiroDetalleDescuentoTecnica.ordenGiroDetalleDescuentoTecnicaAportante.push(
                                {
                                    ordenGiroDetalleDescuentoTecnicaAportanteId: aportanteControl.get( 'ordenGiroDetalleDescuentoTecnicaAportanteId' ).value,
                                    ordenGiroDetalleDescuentoTecnicaId: descuentoControl.get( 'ordenGiroDetalleDescuentoTecnicaId' ).value,
                                    solicitudPagoFaseFacturaDescuentoId: descuentoControl.get( 'solicitudPagoFaseFacturaDescuentoId' ).value,
                                    aportanteId: aportanteControl.get( 'nombreAportante' ).value.cofinanciacionAportanteId,
                                    valorDescuento: aportanteControl.get( 'valorDescuento' ).value,
                                    conceptoPagoCodigo: conceptoControl.get( 'conceptoCodigo' ).value,
                                    fuenteRecursosCodigo: aportanteControl.get( 'fuenteRecursos' ).value
                                }
                            )
    
                        } );
                    } );
                } )

                if ( ordenGiroDetalleDescuentoTecnica !== undefined ) {
                    listaDescuentosTecnica.push( ordenGiroDetalleDescuentoTecnica );
                }
            } )

            return listaDescuentosTecnica.length > 0 ? listaDescuentosTecnica : null;
        }

        const pOrdenGiro = {
            solicitudPagoId: this.solicitudPago.solicitudPagoId,
            ordenGiroId: this.ordenGiroId,
            ordenGiroDetalle: [
                {
                    ordenGiroId: this.ordenGiroId,
                    ordenGiroDetalleId: this.ordenGiroDetalleId,
                    ordenGiroDetalleDescuentoTecnica: getOrdenGiroDetalleDescuentoTecnica()
                }
            ]
        }

        console.log( getOrdenGiroDetalleDescuentoTecnica() );

        this.ordenGiroSvc.createEditOrdenGiro( pOrdenGiro )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );

                    if ( this.descuentos.controls[ index ].get( 'obsVerificar' ).value !== null ) {
                        const obsVerificar = this.descuentos.controls[ index ].get( 'obsVerificar' ).value;
                        obsVerificar.archivada = !obsVerificar.archivada;
                        this.obsOrdenGiro.createEditSpinOrderObservations( obsVerificar )
                            .subscribe();
                    }
                    if ( this.descuentos.controls[ index ].get( 'obsAprobar' ).value !== null ) {
                        const obsAprobar = this.descuentos.controls[ index ].get( 'obsAprobar' ).value;
                        obsAprobar.archivada = !obsAprobar.archivada;
                        this.obsOrdenGiro.createEditSpinOrderObservations( obsAprobar )
                            .subscribe();
                    }
                    if ( this.descuentos.controls[ index ].get( 'obsTramitar' ).value !== null ) {
                        const obsTramitar = this.descuentos.controls[ index ].get( 'obsTramitar' ).value;
                        obsTramitar.archivada = !obsTramitar.archivada;
                        this.obsOrdenGiro.createEditSpinOrderObservations( obsTramitar )
                            .subscribe();
                    }
                    this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                        () => this.routes.navigate(
                            [
                                '/generarOrdenDeGiro/generacionOrdenGiro', this.solicitudPago.solicitudPagoId
                            ]
                        )
                    );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

}
