import { OrdenPagoService } from './../../../../core/_services/ordenPago/orden-pago.service';
import { CommonService } from './../../../../core/_services/common/common.service';
import { Dominio } from 'src/app/core/_services/common/common.service';
import { MatDialog } from '@angular/material/dialog';
import { Component, Input, OnInit, EventEmitter, Output } from '@angular/core';
import { FormGroup, Validators, FormBuilder, FormArray } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { RegistrarRequisitosPagoService } from 'src/app/core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';
import humanize from 'humanize-plus';
import { ListaMenu, ListaMenuId, TipoObservaciones, TipoObservacionesCodigo } from 'src/app/_interfaces/estados-solicitudPago-ordenGiro.interface';
import { ObservacionesOrdenGiroService } from 'src/app/core/_services/observacionesOrdenGiro/observaciones-orden-giro.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-descuentos-direccion-tecnica',
  templateUrl: './descuentos-direccion-tecnica.component.html',
  styleUrls: ['./descuentos-direccion-tecnica.component.scss']
})
export class DescuentosDireccionTecnicaComponent implements OnInit {

    @Input() solicitudPago: any;
    @Input() esVerDetalle: boolean;
    @Input() esPreconstruccion: boolean;
    @Input() esRegistroNuevo: boolean;
    @Output() estadoSemaforo = new EventEmitter<string>();
    listaMenu: ListaMenu = ListaMenuId;
    tipoObservaciones: TipoObservaciones = TipoObservacionesCodigo;
    listaTipoDescuento: Dominio[] = [];
    totalEnProceso = 0;
    totalCompleto = 0;
    solicitudPagoFase: any;
    solicitudPagoFaseCriterio: any[];
    solicitudPagoFaseFactura: any;
    fasePreConstruccionFormaPagoCodigo: any;
    ordenGiroDetalle: any;
    ordenGiroId = 0;
    ordenGiroDetalleId = 0;
    tiposDescuentoArray: Dominio[] = [];
    listaConceptoPago: Dominio[] = [];
    listaFuenteTipoFinanciacion: Dominio[] = [];
    listaCriterios: Dominio[] = [];
    dataSource = new MatTableDataSource();
    dataHistorial: any[] = [];
    tablaHistorial = new MatTableDataSource();
    formObservacion: FormGroup = this.fb.group({
        tieneObservaciones: [ null, Validators.required ],
        observaciones: [ null, Validators.required ],
        fechaCreacion: [ null ]
    });
    displayedColumnsHistorial: string[]  = [
        'fechaRevision',
        'responsable',
        'historial'
    ];
    editorStyle = {
        height: '100px'
    };
    config = {
        toolbar: [
            ['bold', 'italic', 'underline'],
            [{ list: 'ordered' }, { list: 'bullet' }],
            [{ indent: '-1' }, { indent: '+1' }],
            [{ align: [] }],
        ]
    };
    displayedColumns: string[] = [
        'tipoDescuento',
        'valorDescuento',
        'valorTotalDescuentos',
        'valorNetoGiro'
    ];
    listData = {
        listaDescuentos: [],
        valorNetoGiro: 0,
        valorTotalDescuentos: 0
    };
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
        private fb: FormBuilder,
        private dialog: MatDialog,
        private commonSvc: CommonService,
        private ordenGiroSvc: OrdenPagoService,
        private registrarPagosSvc: RegistrarRequisitosPagoService,
        private obsOrdenGiro: ObservacionesOrdenGiroService,
        private routes: Router )
    {
        this.commonSvc.tiposDescuento()
            .subscribe(response => this.listaTipoDescuento = response);
    }

    ngOnInit(): void {
        this.getDireccionTecnica();
    }

    getDireccionTecnica() {
        // Get Tablas
        this.solicitudPagoFase = this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase.find( solicitudPagoFase => solicitudPagoFase.esPreconstruccion === this.esPreconstruccion );
        this.solicitudPagoFaseCriterio = this.solicitudPagoFase.solicitudPagoFaseCriterio;
        this.solicitudPagoFaseFactura = this.solicitudPagoFase.solicitudPagoFaseFactura[0];

        if ( this.solicitudPago.contratoSon.solicitudPago.length > 1 ) {
            this.fasePreConstruccionFormaPagoCodigo = this.solicitudPago.contratoSon.solicitudPago[0].solicitudPagoCargarFormaPago[0];
        } else {
            this.fasePreConstruccionFormaPagoCodigo = this.solicitudPago.solicitudPagoCargarFormaPago[0];
        }
        /*
            get listaCriterios para lista desplegable
            Se reutilizan los servicios del CU 4.1.7 "Solicitud de pago"
        */
        this.registrarPagosSvc.getCriterioByFormaPagoCodigo( this.solicitudPagoFase.esPreconstruccion === true ? this.fasePreConstruccionFormaPagoCodigo.fasePreConstruccionFormaPagoCodigo : this.fasePreConstruccionFormaPagoCodigo.faseConstruccionFormaPagoCodigo )
            .subscribe( async getCriterioByFormaPagoCodigo => {
                this.listaCriterios = getCriterioByFormaPagoCodigo;
                // Get data de la tabla descuentos
                this.solicitudPagoFaseCriterio.forEach( criterio => this.listData.valorNetoGiro += criterio.valorFacturado );
                this.solicitudPagoFaseFactura.solicitudPagoFaseFacturaDescuento.forEach( descuento => {
                    this.listData.valorNetoGiro -= descuento.valorDescuento;
                    this.listData.valorTotalDescuentos += descuento.valorDescuento;

                    this.listData.listaDescuentos.push(
                        {
                            tipoDescuentoCodigo: descuento.tipoDescuentoCodigo,
                            valorDescuento: descuento.valorDescuento
                        }
                    )
                } );
                // Get data de los descuentos de direccion tecnica && Get IDs
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
                        for ( const descuento of this.solicitudPagoFaseFactura.solicitudPagoFaseFacturaDescuento ) {
                            const formArrayCriterios = [];
                            const detalleDescuentoTecnica = ordenGiroDetalleDescuentoTecnica.filter( descuentoTecnica => descuentoTecnica.solicitudPagoFaseFacturaDescuentoId === descuento.solicitudPagoFaseFacturaDescuentoId && descuentoTecnica.esPreconstruccion === this.esPreconstruccion );
        
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
                                                nombre: [ descuento.criterioCodigo !== undefined ? this.listaCriterios.find( criterio => criterio.codigo === descuento.criterioCodigo ).nombre : null ],
                                                ordenGiroDetalleDescuentoTecnicaId: [ descuento.ordenGiroDetalleDescuentoTecnicaId !== undefined ? descuento.ordenGiroDetalleDescuentoTecnicaId : 0 ],
                                                criterioCodigo: [ descuento.criterioCodigo !== undefined ? this.listaCriterios.find( criterio => criterio.codigo === descuento.criterioCodigo ).codigo : null ],
                                                tipoPagoNombre: [ tipoPago !== undefined ? tipoPago.nombre : null ],
                                                tipoPagoCodigo: [ tipoPago !== undefined ? tipoPago.codigo : null ],
                                                conceptosDePago: [ conceptosDePago ],
                                                concepto: [ listaConceptos.length > 0 ? listaConceptos : null, Validators.required ],
                                                conceptos: this.fb.array( formArrayConceptos.length > 0 ? formArrayConceptos : [] )
                                            }
                                        )
                                    )
                                }
                                // Get observaciones
                                let estadoSemaforo = 'sin-diligenciar';
                                const historialObservaciones = [];
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

                                if ( listaObservacionVerificar.length > 0 ) {
                                    listaObservacionVerificar.forEach( obs => obs.menuId = this.listaMenu.verificarOrdenGiro );
                                }
                                if ( listaObservacionAprobar.length > 0 ) {
                                    listaObservacionAprobar.forEach( obs => obs.menuId = this.listaMenu.aprobarOrdenGiro );
                                }
                                if ( listaObservacionTramitar.length > 0 ) {
                                    listaObservacionTramitar.forEach( obs => obs.menuId = this.listaMenu.tramitarOrdenGiro )
                                }

                                // Get lista observaciones archivadas
                                const obsArchivadasVerificar = listaObservacionVerificar.filter( obs => obs.archivada === true && obs.tieneObservacion === true );
                                const obsArchivadasAprobar = listaObservacionAprobar.filter( obs => obs.archivada === true && obs.tieneObservacion === true );
                                const obsArchivadasTramitar = listaObservacionTramitar.filter( obs => obs.archivada === true && obs.tieneObservacion === true );

                                if ( obsArchivadasVerificar.length > 0 ) {
                                    obsArchivadasVerificar.forEach( obs => historialObservaciones.push( obs ) );
                                }
                                if ( obsArchivadasAprobar.length > 0 ) {
                                    obsArchivadasAprobar.forEach( obs => historialObservaciones.push( obs ) );
                                }
                                if ( obsArchivadasTramitar.length > 0 ) {
                                    obsArchivadasTramitar.forEach( obs => historialObservaciones.push( obs ) );
                                }
                                // Get observacion actual    
                                const observacion = listaObservacionVerificar.find( obs => obs.archivada === false )
                                if ( observacion !== undefined ) {
                                    if ( observacion.registroCompleto === false ) {
                                        estadoSemaforo = 'en-proceso';
                                    }
                                    if ( observacion.registroCompleto === true ) {
                                        estadoSemaforo = 'completo';
                                    }
                                }
                                // Set contador semaforo observaciones
                                if ( estadoSemaforo === 'en-proceso' ) {
                                    this.totalEnProceso++;
                                }
                                if ( estadoSemaforo === 'completo' ) {
                                    this.totalCompleto++;
                                }

                                this.descuentos.controls.push( this.fb.group(
                                    {
                                        estadoSemaforo,
                                        historialObservaciones: [ historialObservaciones ],
                                        ordenGiroObservacionId: [ observacion !== undefined ? ( observacion.ordenGiroObservacionId !== undefined ? observacion.ordenGiroObservacionId : 0 ) : 0 ],
                                        tieneObservaciones: [ observacion !== undefined ? ( observacion.tieneObservacion !== undefined ? observacion.tieneObservacion : null ) : null, Validators.required ],
                                        observaciones: [ observacion !== undefined ? ( observacion.observacion !== undefined ? ( observacion.observacion.length > 0 ? observacion.observacion : null ) : null ) : null, Validators.required ],
                                        fechaCreacion: [ observacion !== undefined ? ( observacion.fechaCreacion !== undefined ? observacion.fechaCreacion : null ) : null ],
                                        solicitudPagoFaseFacturaDescuentoId: [ descuento.solicitudPagoFaseFacturaDescuentoId ],
                                        tipoDescuentoCodigo: [ descuento.tipoDescuentoCodigo ],
                                        criterio: [ listaCriterios.length > 0 ? listaCriterios : null, Validators.required ],
                                        criterios: this.fb.array( formArrayCriterios.length > 0 ? formArrayCriterios : [] )
                                    }
                                ) );
                            }
                        }
                    }
                }

                /*
                if ( this.ordenGiroDetalle !== undefined ) {
                    const ordenGiroDetalleDescuentoTecnica: any[] = this.ordenGiroDetalle.ordenGiroDetalleDescuentoTecnica;
                
                    if ( ordenGiroDetalleDescuentoTecnica.length > 0 ) {
                        for ( const descuento of this.solicitudPagoFaseFactura.solicitudPagoFaseFacturaDescuento ) {
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
                                                    const dataAportantes = await this.ordenGiroSvc.getAportantes( this.solicitudPago );

                                                    const formArrayAportantes = [];
                                                    
                                                    if ( ordenGiroDetalleDescuentoTecnicaAportante.length > 0 ) {
                                                        for ( const aportante of ordenGiroDetalleDescuentoTecnicaAportante ) {
                                                            const nombreAportante = dataAportantes.listaNombreAportante.find( nombre => nombre.cofinanciacionAportanteId === aportante.aportanteId );
                                                            const tipoAportante = dataAportantes.listaTipoAportante.find( tipo => tipo.dominioId === nombreAportante.tipoAportanteId );
                                                            let listaFuenteRecursos: any[];
                                                            const fuenteRecursos = await this.ordenGiroSvc.getFuentesDeRecursosPorAportanteId( nombreAportante.cofinanciacionAportanteId ).toPromise();
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
                                                        }
                                                    }
                                                
                                                    formArrayConceptos.push( this.fb.group(
                                                        {
                                                            nombre: [ concepto.nombre ],
                                                            conceptoCodigo: [ concepto.codigo ],
                                                            tipoDeAportantes: [ dataAportantes.listaTipoAportante ],
                                                            nombreDeAportantes: [ dataAportantes.listaNombreAportante ],
                                                            aportantes: this.fb.array( formArrayAportantes )
                                                        }
                                                    ) );
                                                }
                                            }

                                            // Set formulario de los criterios
                                            formArrayCriterios.push(
                                                this.fb.group(
                                                    {
                                                        nombre: [ this.listaCriterios.find( criterio => criterio.codigo === codigo ).nombre ],
                                                        criterioCodigo: [ this.listaCriterios.find( criterio => criterio.codigo === codigo ).codigo ],
                                                        tipoPagoNombre: [ tipoPago.nombre ],
                                                        tipoPagoCodigo: [ tipoPago.codigo ],
                                                        conceptosDePago: [ conceptosDePago ],
                                                        concepto: [ listaConceptos, Validators.required ],
                                                        conceptos: this.fb.array( formArrayConceptos )
                                                    }
                                                )
                                            )
                                        }
                                    }
                                }

                                setTimeout( async () => {
                                    for ( const descuentoValue of detalleDescuentoTecnica ) {
                                        // Get observaciones
                                        let estadoSemaforo = 'sin-diligenciar';
                                        const historialObservaciones = [];
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
                                        if ( listaObservacionVerificar.length > 0 ) {
                                            listaObservacionVerificar.forEach( obs => obs.menuId = this.listaMenu.verificarOrdenGiro );
                                        }
                                        if ( listaObservacionAprobar.length > 0 ) {
                                            listaObservacionAprobar.forEach( obs => obs.menuId = this.listaMenu.aprobarOrdenGiro );
                                        }
                                        if ( listaObservacionTramitar.length > 0 ) {
                                            listaObservacionTramitar.forEach( obs => obs.menuId = this.listaMenu.tramitarOrdenGiro )
                                        }
                                        // Get lista observaciones archivadas
                                        const obsArchivadasVerificar = listaObservacionVerificar.filter( obs => obs.archivada === true && obs.tieneObservacion === true );
                                        const obsArchivadasAprobar = listaObservacionAprobar.filter( obs => obs.archivada === true && obs.tieneObservacion === true );
                                        const obsArchivadasTramitar = listaObservacionTramitar.filter( obs => obs.archivada === true && obs.tieneObservacion === true );
                                        if ( obsArchivadasVerificar.length > 0 ) {
                                            obsArchivadasVerificar.forEach( obs => historialObservaciones.push( obs ) );
                                        }
                                        if ( obsArchivadasAprobar.length > 0 ) {
                                            obsArchivadasAprobar.forEach( obs => historialObservaciones.push( obs ) );
                                        }
                                        if ( obsArchivadasTramitar.length > 0 ) {
                                            obsArchivadasTramitar.forEach( obs => historialObservaciones.push( obs ) );
                                        }
                                        // Get observacion actual    
                                        const observacion = listaObservacionVerificar.find( obs => obs.archivada === false )
                                        if ( observacion !== undefined ) {
                                            if ( observacion.registroCompleto === false ) {
                                                estadoSemaforo = 'en-proceso';
                                            }
                                            if ( observacion.registroCompleto === true ) {
                                                estadoSemaforo = 'completo';
                                            }
                                        }
                                        // Set contador semaforo observaciones
                                        if ( estadoSemaforo === 'en-proceso' ) {
                                            this.totalEnProceso++;
                                        }
                                        if ( estadoSemaforo === 'completo' ) {
                                            this.totalCompleto++;
                                        }
                                        // Set formulario de los descuentos
                                        this.descuentos.controls.push( this.fb.group(
                                            {
                                                estadoSemaforo,
                                                historialObservaciones: [ historialObservaciones ],
                                                ordenGiroObservacionId: [ observacion !== undefined ? ( observacion.ordenGiroObservacionId !== undefined ? observacion.ordenGiroObservacionId : 0 ) : 0 ],
                                                tieneObservaciones: [ observacion !== undefined ? ( observacion.tieneObservacion !== undefined ? observacion.tieneObservacion : null ) : null, Validators.required ],
                                                observaciones: [ observacion !== undefined ? ( observacion.observacion !== undefined ? ( observacion.observacion.length > 0 ? observacion.observacion : null ) : null ) : null, Validators.required ],
                                                fechaCreacion: [ observacion !== undefined ? ( observacion.fechaCreacion !== undefined ? observacion.fechaCreacion : null ) : null ],
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
                        setTimeout(() => {
                            if ( this.totalEnProceso > 0 && this.totalEnProceso === this.descuentos.length ) {
                                this.estadoSemaforo.emit( 'en-proceso' );
                            }
                            if ( this.totalCompleto > 0 && this.totalCompleto < this.descuentos.length ) {
                                this.estadoSemaforo.emit( 'en-proceso' );
                            }
                            if ( this.totalCompleto > 0 && this.totalCompleto === this.descuentos.length ) {
                                this.estadoSemaforo.emit( 'completo' );
                            }
                        }, 3000);
                    }
                }
                */

                this.dataSource = new MatTableDataSource( [ this.listData ] );
            } );
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

    getValueCriterio( codigo: string ) {
        if ( this.listaCriterios.length > 0 ) {
            const criterio = this.listaCriterios.find( criterio => criterio.codigo === codigo );
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

    getDataSource( historialObservaciones: any[] ) {
        return new MatTableDataSource( historialObservaciones );
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

    openDialog( modalTitle: string, modalText: string ) {
        this.dialog.open( ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText }
        });
    }

    guardar( descuento: FormGroup ) {
        if ( descuento.get( 'tieneObservaciones' ).value === false && descuento.get( 'observaciones' ).value !== null ) {
            descuento.get( 'observaciones' ).setValue( '' );
        }

        const pOrdenGiroObservacion = {
            ordenGiroObservacionId: descuento.get( 'ordenGiroObservacionId' ).value,
            ordenGiroId: this.ordenGiroId,
            tipoObservacionCodigo: this.tipoObservaciones.direccionTecnica,
            menuId: this.listaMenu.verificarOrdenGiro,
            idPadre: descuento.get( 'solicitudPagoFaseFacturaDescuentoId' ).value,
            observacion: descuento.get( 'observaciones' ).value,
            tieneObservacion: descuento.get( 'tieneObservaciones' ).value
        }

        this.obsOrdenGiro.createEditSpinOrderObservations( pOrdenGiroObservacion )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                        () => this.routes.navigate(
                            [
                                this.esRegistroNuevo === true ? '/verificarOrdenGiro/verificarOrdenGiro' : '/verificarOrdenGiro/editarOrdenGiro', this.solicitudPago.solicitudPagoId
                            ]
                        )
                    );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

}
