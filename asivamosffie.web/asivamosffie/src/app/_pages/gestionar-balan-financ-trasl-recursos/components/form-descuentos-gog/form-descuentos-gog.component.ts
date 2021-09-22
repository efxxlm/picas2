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
    @Input() esPreconstruccion: boolean;
    @Input() listaCriterios: Dominio[] = [];
    @Input() solicitudPago: any;
    @Input() solicitudPagoFase: any;
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
                    const detalleDescuentoTecnica = ordenGiroDetalleDescuentoTecnica.filter( descuentoTecnica => descuentoTecnica.solicitudPagoFaseFacturaDescuentoId === descuento.solicitudPagoFaseFacturaDescuentoId && descuentoTecnica.esPreconstruccion === this.esPreconstruccion && descuentoTecnica.contratacionProyectoId === this.solicitudPagoFase.contratacionProyectoId );

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

                            listaCriterios.push( this.criteriosArray.find( criterio => criterio.codigo === descuento.criterioCodigo ).codigo );

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

                                    // Get cantidad de aportantes para limitar cuantos aportantes se pueden agregar en el formulario
                                    this.cantidadAportantes = dataAportantes.listaTipoAportante.length;

                                    if ( ordenGiroDetalleDescuentoTecnicaAportante.length > 0 ) {
                                        for ( const aportante of ordenGiroDetalleDescuentoTecnicaAportante ) {
                                            const nombreAportante = dataAportantes.listaNombreAportante.find( nombre => nombre.cofinanciacionAportanteId === aportante.aportanteId );
                                            let tipoAportante: any;
                                            let listaFuenteRecursos: any[];

                                            if ( nombreAportante !== undefined ) {
                                                tipoAportante = dataAportantes.listaTipoAportante.find( tipo => tipo.dominioId === nombreAportante.tipoAportanteId );
                                                listaFuenteRecursos = await this.ordenGiroSvc.getFuentesDeRecursosPorAportanteId( nombreAportante.cofinanciacionAportanteId ).toPromise();

                                                formArrayAportantes.push(
                                                    this.fb.group(
                                                        {
                                                            ordenGiroDetalleDescuentoTecnicaAportanteId: [ aportante.ordenGiroDetalleDescuentoTecnicaAportanteId !== undefined ? aportante.ordenGiroDetalleDescuentoTecnicaAportanteId : 0 ],
                                                            tipoAportante: [ tipoAportante !== undefined ? tipoAportante : null, Validators.required ],
                                                            listaNombreAportantes: [ [ nombreAportante !== undefined ? nombreAportante : null ] ],
                                                            nombreAportante: [ nombreAportante !== undefined ? nombreAportante : null, Validators.required ],
                                                            fuenteDeRecursos: [ listaFuenteRecursos ],
                                                            fuenteRecursos: [ aportante.fuenteRecursosCodigo !== undefined ? aportante.fuenteRecursosCodigo : null, Validators.required ],
                                                            valorDescuento: [ aportante.valorDescuento !== undefined ? aportante.valorDescuento : null, Validators.required ]
                                                        }
                                                    )
                                                )
                                            } else {
                                                formArrayAportantes.push(
                                                    this.fb.group(
                                                        {
                                                            ordenGiroDetalleDescuentoTecnicaAportanteId: [ aportante.ordenGiroDetalleDescuentoTecnicaAportanteId !== undefined ? aportante.ordenGiroDetalleDescuentoTecnicaAportanteId : 0 ],
                                                            tipoAportante: [ null, Validators.required ],
                                                            listaNombreAportantes: [ null ],
                                                            nombreAportante: [ null, Validators.required ],
                                                            fuenteDeRecursos: [ null ],
                                                            fuenteRecursos: [ null, Validators.required ],
                                                            valorDescuento: [ null, Validators.required ]
                                                        }
                                                    )
                                                )
                                            }
                                        }
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

                            // Set formulario de los criterios
                            formArrayCriterios.push(
                                this.fb.group(
                                    {
                                        nombre: [ descuento.criterioCodigo !== undefined ? this.criteriosArray.find( criterio => criterio.codigo === descuento.criterioCodigo ).nombre : null ],
                                        ordenGiroDetalleDescuentoTecnicaId: [ descuento.ordenGiroDetalleDescuentoTecnicaId !== undefined ? descuento.ordenGiroDetalleDescuentoTecnicaId : 0 ],
                                        criterioCodigo: [ descuento.criterioCodigo !== undefined ? this.criteriosArray.find( criterio => criterio.codigo === descuento.criterioCodigo ).codigo : null ],
                                        tipoPagoNombre: [ tipoPago !== undefined ? tipoPago.nombre : null ],
                                        tipoPagoCodigo: [ tipoPago !== undefined ? tipoPago.codigo : null ],
                                        conceptosDePago: [ conceptosDePago ],
                                        concepto: [ listaConceptos.length > 0 ? listaConceptos : null, Validators.required ],
                                        conceptos: this.fb.array( formArrayConceptos.length > 0 ? formArrayConceptos : [] )
                                    }
                                )
                            )
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

                        // Get observaciones
                        const listaObservacionVerificar = await this.obsOrdenGiro.getObservacionOrdenGiroByMenuIdAndSolicitudPagoId(
                            this.listaMenu.verificarOrdenGiro,
                            this.ordenGiroId,
                            descuento.solicitudPagoFaseFacturaDescuentoId,
                            this.tipoObservaciones.direccionTecnica );
                        const listaObservacionAprobar = await this.obsOrdenGiro.getObservacionOrdenGiroByMenuIdAndSolicitudPagoId(
                            this.listaMenu.aprobarOrdenGiro,
                            this.ordenGiroId,
                            descuento.solicitudPagoFaseFacturaDescuentoId,
                            this.tipoObservaciones.direccionTecnica );
                        const listaObservacionTramitar = await this.obsOrdenGiro.getObservacionOrdenGiroByMenuIdAndSolicitudPagoId(
                                this.listaMenu.tramitarOrdenGiro,
                                this.ordenGiroId,
                                descuento.solicitudPagoFaseFacturaDescuentoId,
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
                                obsVerificar: [ obsVerificar !== undefined ? obsVerificar : null ],
                                obsAprobar: [ obsAprobar !== undefined ? obsAprobar : null ],
                                obsTramitar: [ obsTramitar !== undefined ? obsTramitar : null ],
                                solicitudPagoFaseFacturaDescuentoId: [ descuento.solicitudPagoFaseFacturaDescuentoId ],
                                tipoDescuentoCodigo: [ descuento.tipoDescuentoCodigo ],
                                valorDescuento: [ descuento.valorDescuento ],
                                criterio: [ listaCriterios.length > 0 ? listaCriterios : null, Validators.required ],
                                criterios: this.fb.array( formArrayCriterios.length > 0 ? formArrayCriterios : [] )
                            }
                        ) );
                    } else {
                        this.descuentos.push( this.fb.group(
                            {
                                estadoSemaforo: [ 'sin-diligenciar' ],
                                obsVerificar: [ null ],
                                obsAprobar: [ null ],
                                obsTramitar: [ null ],
                                solicitudPagoFaseFacturaDescuentoId: [ descuento.solicitudPagoFaseFacturaDescuentoId ],
                                tipoDescuentoCodigo: [ descuento.tipoDescuentoCodigo ],
                                valorDescuento: [ descuento.valorDescuento ],
                                criterio: [ null, Validators.required ],
                                criterios: this.fb.array( [] )
                            }
                        ) );
                    }
                }
            } else {
                this.solicitudPagoFaseFacturaDescuento.forEach( descuento => {

                    this.descuentos.push( this.fb.group(
                        {
                            estadoSemaforo: [ 'sin-diligenciar' ],
                            obsVerificar: [ null ],
                            obsAprobar: [ null ],
                            obsTramitar: [ null ],
                            solicitudPagoFaseFacturaDescuentoId: [ descuento.solicitudPagoFaseFacturaDescuentoId ],
                            tipoDescuentoCodigo: [ descuento.tipoDescuentoCodigo ],
                            valorDescuento: [ descuento.valorDescuento ],
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
                        obsVerificar: [ null ],
                        obsAprobar: [ null ],
                        obsTramitar: [ null ],
                        solicitudPagoFaseFacturaDescuentoId: [ descuento.solicitudPagoFaseFacturaDescuentoId ],
                        tipoDescuentoCodigo: [ descuento.tipoDescuentoCodigo ],
                        valorDescuento: [ descuento.valorDescuento ],
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

    checkValueDescuento( value: number, index: number, jIndex: number, kIndex: number, lIndex: number ) {
        if ( value !== null ) {
            if ( value < 0 ) {
                this.getAportantes( index, jIndex, kIndex ).controls[ lIndex ].get( 'valorDescuento' ).setValue( null );
            }
        }
    }

    checkTotalDiscountValues( index: number, jIndex: number, kIndex: number, lIndex: number ) {
        // const solicitudPagoFaseFacturaDescuento = this.solicitudPagoFaseFacturaDescuento.find( solicitudPagoFaseFacturaDescuento => solicitudPagoFaseFacturaDescuento.solicitudPagoFaseFacturaDescuentoId === this.descuentos.controls[ index ].get( 'solicitudPagoFaseFacturaDescuentoId' ).value )

        let totalDescuentoCriterio = 0;
            
        this.getCriterios( index ).controls.forEach( ( criterioControl, indexCriterio ) => {
            this.getConceptos( index, indexCriterio ).controls.forEach( ( conceptoControl, indexConcepto ) => {
                this.getAportantes( index, indexCriterio, indexConcepto ).controls.forEach( aportanteControl => {
                    if ( aportanteControl.get( 'valorDescuento' ).value !== null ) {
                        totalDescuentoCriterio += aportanteControl.get( 'valorDescuento' ).value
                    }
                } )
            } )
        } )

        if ( totalDescuentoCriterio > this.descuentos.controls[ index ].get( 'valorDescuento' ).value ) {
            this.getAportantes( index, jIndex, kIndex ).controls[ lIndex ].get( 'valorDescuento' ).setValue( null );
            this.openDialog( '', `<b>El valor total de los descuentos no puede ser mayor al valor del descuento de la dirección tecnica <br> ${ this.getTipoDescuento( this.descuentos.controls[ index ].get( 'tipoDescuentoCodigo' ).value ) }.</b>` )
        }

        /*if ( solicitudPagoFaseFacturaDescuento !== undefined ) {
            if ( totalDescuentoCriterio > solicitudPagoFaseFacturaDescuento.valorDescuento ) {
                this.openDialog( '', '<b>El valor total de los descuentos no puede ser mayor al valor del descuento.</b>' )
            }
        }*/
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
                        ordenGiroDetalleDescuentoTecnicaId: [ 0 ],
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
                const dataAportantes = await this.ordenGiroSvc.getAportantes( this.solicitudPago );

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

                        if ( aportanteSeleccionado !== null ) {
                            listaTipoAportantes.push( aportanteSeleccionado );
                            this.getConceptos( index, jIndex ).controls[ kIndex ].get( 'tipoDeAportantes' ).setValue( listaTipoAportantes );
                        }

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

                        if ( this.getCriterios( index ).controls[ jIndex ].get( 'ordenGiroDetalleDescuentoTecnicaId' ).value !== 0 ) {
                            this.ordenGiroSvc.deleteOrdenGiroDetalleDescuentoTecnica( this.getCriterios( index ).controls[ jIndex ].get( 'ordenGiroDetalleDescuentoTecnicaId' ).value )
                                .subscribe(
                                    response => {
                                        this.getCriterios( index ).removeAt( jIndex );
                                        this.openDialog( '', `<b>${ response.message }</b>` );
                                    }
                                )
                        } else {
                            this.getCriterios( index ).removeAt( jIndex );
                            this.openDialog( '', '<b>La información se ha eliminado correctamente.</b>' );
                        }
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
                const ordenGiroDetalleDescuentoTecnica = [];
    
                this.getCriterios( indexDescuento ).controls.forEach( ( criterioControl, indexCriterio ) => {
                    const ordenGiroDetalleDescuento = {
                        contratacionProyectoId: this.solicitudPagoFase.contratacionProyectoId,
                        ordenGiroDetalleId: this.ordenGiroDetalleId,
                        ordenGiroDetalleDescuentoTecnicaId: criterioControl.get( 'ordenGiroDetalleDescuentoTecnicaId' ).value,
                        solicitudPagoFaseFacturaDescuentoId: descuentoControl.get( 'solicitudPagoFaseFacturaDescuentoId' ).value,
                        tipoPagoCodigo: criterioControl.get( 'tipoPagoCodigo' ).value,
                        criterioCodigo: criterioControl.get( 'criterioCodigo' ).value,
                        esPreconstruccion: this.esPreconstruccion,
                        ordenGiroDetalleDescuentoTecnicaAportante: []
                    }
    
                    this.getConceptos( indexDescuento, indexCriterio ).controls.forEach( ( conceptoControl, indexConcepto ) => {
    
                        this.getAportantes( indexDescuento, indexCriterio, indexConcepto ).controls.forEach( aportanteControl => {
                            let fuenteFinanciacionId = 0;
                            
                            if ( aportanteControl.get( 'fuenteDeRecursos' ).value !== null && aportanteControl.get( 'fuenteRecursos' ).value !== null ) {
                                const fuenteDeRecursos = aportanteControl.get( 'fuenteDeRecursos' ).value
                                const fuente = fuenteDeRecursos.find( fuente => fuente.codigo === aportanteControl.get( 'fuenteRecursos' ).value )

                                if ( fuente !== undefined ) {
                                    fuenteFinanciacionId = fuente.fuenteFinanciacionId
                                }
                            }

                            ordenGiroDetalleDescuento.ordenGiroDetalleDescuentoTecnicaAportante.push(
                                {
                                    ordenGiroDetalleDescuentoTecnicaAportanteId: aportanteControl.get( 'ordenGiroDetalleDescuentoTecnicaAportanteId' ).value,
                                    ordenGiroDetalleDescuentoTecnicaId: criterioControl.get( 'ordenGiroDetalleDescuentoTecnicaId' ).value,
                                    solicitudPagoFaseFacturaDescuentoId: descuentoControl.get( 'solicitudPagoFaseFacturaDescuentoId' ).value,
                                    aportanteId: aportanteControl.get( 'nombreAportante' ).value !== null ? aportanteControl.get( 'nombreAportante' ).value.cofinanciacionAportanteId : null,
                                    valorDescuento: aportanteControl.get( 'valorDescuento' ).value,
                                    conceptoPagoCodigo: conceptoControl.get( 'conceptoCodigo' ).value,
                                    fuenteRecursosCodigo: aportanteControl.get( 'fuenteRecursos' ).value,
                                    fuenteFinanciacionId: fuenteFinanciacionId
                                }
                            )
    
                        } );
                    } );

                    ordenGiroDetalleDescuentoTecnica.push( ordenGiroDetalleDescuento );
                } )

                if ( ordenGiroDetalleDescuentoTecnica.length > 0 ) {
                    listaDescuentosTecnica.push( ...ordenGiroDetalleDescuentoTecnica );
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
                                '/generarOrdenDeGiro/verDetalleEditarOrdenGiro', this.solicitudPago.solicitudPagoId
                            ]
                        )
                    );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

}
