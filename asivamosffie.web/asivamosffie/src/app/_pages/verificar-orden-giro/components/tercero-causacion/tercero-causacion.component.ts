import { MatDialog } from '@angular/material/dialog';
import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { FormGroup, Validators, FormBuilder, FormArray } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { Dominio } from 'src/app/core/_services/common/common.service';
import humanize from 'humanize-plus';
import { Router } from '@angular/router';
import { RegistrarRequisitosPagoService } from 'src/app/core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';
import { OrdenPagoService } from 'src/app/core/_services/ordenPago/orden-pago.service';
import { ListaMenu, ListaMenuId, TipoObservaciones, TipoObservacionesCodigo } from 'src/app/_interfaces/estados-solicitudPago-ordenGiro.interface';
import { ObservacionesOrdenGiroService } from 'src/app/core/_services/observacionesOrdenGiro/observaciones-orden-giro.service';

@Component({
  selector: 'app-tercero-causacion',
  templateUrl: './tercero-causacion.component.html',
  styleUrls: ['./tercero-causacion.component.scss']
})
export class TerceroCausacionComponent implements OnInit {

    @Input() solicitudPago: any;
    @Input() esVerDetalle: boolean;
    @Input() esRegistroNuevo: boolean;
    @Output() estadoSemaforo = new EventEmitter<string>();
    listaMenu: ListaMenu = ListaMenuId;
    tipoObservaciones: TipoObservaciones = TipoObservacionesCodigo;
    totalEnProceso = 0;
    totalCompleto = 0;
    tipoDescuentoArray: Dominio[] = [];
    listaCriterios: Dominio[] = [];
    listaFuenteTipoFinanciacion: Dominio[] = [];
    cantidadAportantes: number;
    solicitudPagoFase: any;
    solicitudPagoFaseCriterio: any;
    solicitudPagoFaseFactura: any;
    fasePreConstruccionFormaPagoCodigo: any;
    ordenGiroDetalle: any;
    ordenGiroDetalleTerceroCausacion: any[];
    variosAportantes: boolean;
    estaEditando = false;
    valorNetoGiro = 0;
    ordenGiroId = 0;
    ordenGiroDetalleId = 0;
    dataHistorial: any[] = [];
    addressForm: FormGroup;
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

    // Get formArray de addressForm 
    get criterios() {
        return this.addressForm.get( 'criterios' ) as FormArray;
    }

    getConceptos( index: number ) {
        return this.criterios.controls[ index ].get( 'conceptos' ) as FormArray;
    }

    getAportantes( index: number, jIndex: number ) {
        return this.getConceptos( index ).controls[ jIndex ].get( 'aportantes' ) as FormArray;
    }

    getDescuentos( index: number, jIndex: number ) {
        return this.getConceptos( index ).controls[ jIndex ].get( 'descuento' ).get( 'descuentos' ) as FormArray;
    }

    constructor(
        private fb: FormBuilder,
        private dialog: MatDialog,
        private routes: Router,
        private registrarPagosSvc: RegistrarRequisitosPagoService,
        private ordenGiroSvc: OrdenPagoService,
        private obsOrdenGiro: ObservacionesOrdenGiroService )
    {
        this.crearFormulario();
    }

    ngOnInit(): void {
        this.dataHistorial = [
            {
                fechaCreacion: new Date(),
                responsable: 'Coordinador financiera',
                observacion: '<p>test historial</p>'
            }
        ];
        this.getTerceroCausacion();
        this.tablaHistorial = new MatTableDataSource( this.dataHistorial );
    }

    crearFormulario () {
        this.addressForm = this.fb.group(
            {
                criterios: this.fb.array( [] )
            }
        );
    }

    getTerceroCausacion() {
        // Get IDs
        if ( this.solicitudPago.ordenGiro !== undefined ) {
            this.ordenGiroId = this.solicitudPago.ordenGiro.ordenGiroId;

            if ( this.solicitudPago.ordenGiro.ordenGiroDetalle !== undefined ) {
                if ( this.solicitudPago.ordenGiro.ordenGiroDetalle.length > 0 ) {
                    this.ordenGiroDetalle = this.solicitudPago.ordenGiro.ordenGiroDetalle[0];
                    this.ordenGiroDetalleId = this.ordenGiroDetalle.ordenGiroDetalleId;

                    if ( this.ordenGiroDetalle.ordenGiroDetalleTerceroCausacion !== undefined ) {
                        if ( this.ordenGiroDetalle.ordenGiroDetalleTerceroCausacion.length > 0 ) {
                            this.ordenGiroDetalleTerceroCausacion = this.ordenGiroDetalle.ordenGiroDetalleTerceroCausacion;
                        }
                    }
                }
            }
        }
        // Get Tablas
        this.solicitudPagoFase = this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase[0];
        this.solicitudPagoFaseCriterio = this.solicitudPagoFase.solicitudPagoFaseCriterio;
        this.solicitudPagoFaseFactura = this.solicitudPagoFase.solicitudPagoFaseFactura[0];

        if ( this.solicitudPago.contratoSon.solicitudPago.length > 1 ) {
            this.fasePreConstruccionFormaPagoCodigo = this.solicitudPago.contratoSon.solicitudPago[0].solicitudPagoCargarFormaPago[0];
        } else {
            this.fasePreConstruccionFormaPagoCodigo = this.solicitudPago.solicitudPagoCargarFormaPago[0];
        }
        // Get data valor neto giro
        this.solicitudPagoFaseCriterio.forEach( criterio => this.valorNetoGiro += criterio.valorFacturado );
        if ( this.solicitudPagoFaseFactura.solicitudPagoFaseFacturaDescuento.length > 0 ) {
            this.solicitudPagoFaseFactura.solicitudPagoFaseFacturaDescuento.forEach( descuento => this.valorNetoGiro -= descuento.valorDescuento );
        }
        /*
            get listaCriterios para lista desplegable
            Se reutilizan los servicios del CU 4.1.7 "Solicitud de pago"
        */
        this.registrarPagosSvc.getCriterioByFormaPagoCodigo(
            this.solicitudPagoFase.esPreconstruccion === true ? this.fasePreConstruccionFormaPagoCodigo.fasePreConstruccionFormaPagoCodigo : this.fasePreConstruccionFormaPagoCodigo.faseConstruccionFormaPagoCodigo
        )
            .subscribe(
                async getCriterioByFormaPagoCodigo => {
                    // Get constantes y variables
                    const listCriterios = [];
                    // Busqueda de criterios seleccionados en el CU 4.1.7 en la lista de tipo dominio
                    for ( const criterioValue of this.solicitudPagoFaseCriterio ) {
                        const criterioFind = getCriterioByFormaPagoCodigo.find( value => value.codigo === criterioValue.tipoCriterioCodigo );
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
                    this.ordenGiroSvc.getAportantes( this.solicitudPago, dataAportantes => {
                        // Get boolean si es uno o varios aportantes
                        if ( dataAportantes.listaTipoAportante.length > 1 ) {
                            this.variosAportantes = true;
                        } else {
                            
                            this.variosAportantes = false
                        }
                        // Get cantidad de aportantes para limitar cuantos aportantes se pueden agregar en el formulario
                        this.cantidadAportantes = dataAportantes.listaTipoAportante.length;
                        // Get data del guardado de tercero de causacion
                        for ( const criterio of listCriterios ) {
                            if ( this.ordenGiroDetalleTerceroCausacion !== undefined ) {
                                const terceroCausacion = this.ordenGiroDetalleTerceroCausacion.find( tercero => tercero.conceptoPagoCriterio === criterio.tipoCriterioCodigo );
                                const listaDescuentos = [];
                                const listaAportantes = [];
                                const conceptosDePago = [];

                                if ( terceroCausacion.ordenGiroDetalleTerceroCausacionDescuento.length > 0 ) {
                                    terceroCausacion.ordenGiroDetalleTerceroCausacionDescuento.forEach( descuento => {
                                        listaDescuentos.push(
                                            this.fb.group(
                                                {
                                                    ordenGiroDetalleTerceroCausacionDescuentoId: [ descuento.ordenGiroDetalleTerceroCausacionDescuentoId ],
                                                    tipoDescuento: [ descuento.tipoDescuentoCodigo, Validators.required ],
                                                    valorDescuento: [ descuento.valorDescuento, Validators.required ]
                                                }
                                            )
                                        );
                                    } )
                                }
                                // Get lista de aportantes
                                // Get cantidad de aportantes para limitar cuantos aportantes se pueden agregar en el formulario
                                this.cantidadAportantes = dataAportantes.listaTipoAportante.length;

                                if ( terceroCausacion.ordenGiroDetalleTerceroCausacionAportante.length > 0 ) {
                                    for ( const aportante of terceroCausacion.ordenGiroDetalleTerceroCausacionAportante ) {
                                        const nombreAportante = dataAportantes.listaNombreAportante.find( nombre => nombre.cofinanciacionAportanteId === aportante.aportanteId );
                                        const tipoAportante = dataAportantes.listaTipoAportante.find( tipo => tipo.dominioId === nombreAportante.tipoAportanteId );
                                        let listaFuenteRecursos: any[];
                                        this.ordenGiroSvc.getFuentesDeRecursosPorAportanteId( nombreAportante.cofinanciacionAportanteId )
                                            .subscribe( fuenteRecursos => {
                                                listaFuenteRecursos = fuenteRecursos;
                                                const fuente = listaFuenteRecursos.find( fuente => fuente.codigo === aportante.fuenteRecursoCodigo );

                                                listaAportantes.push(
                                                    this.fb.group(
                                                        {
                                                            ordenGiroDetalleTerceroCausacionAportanteId: [ aportante.ordenGiroDetalleTerceroCausacionAportanteId ],
                                                            tipoAportante: [ tipoAportante, Validators.required ],
                                                            listaNombreAportantes: [ [ nombreAportante ] ],
                                                            nombreAportante: [ nombreAportante, Validators.required ],
                                                            fuenteDeRecursos: [ listaFuenteRecursos ],
                                                            fuenteRecursos: [ fuente, Validators.required ],
                                                            fuenteFinanciacionId: [ fuente.fuenteFinanciacionId ],
                                                            valorDescuento: [ aportante.valorDescuento, Validators.required ]
                                                        }
                                                    )
                                                )
                                            } );
                                    }
                                }

                                for ( const concepto of criterio.listConceptos ) {
                                    setTimeout(() => {
                                        conceptosDePago.push( this.fb.group(
                                            {
                                                conceptoPagoCriterio: [ concepto.codigo ],
                                                nombre: [ concepto.nombre ],
                                                valorFacturadoConcepto: [ concepto.valorFacturadoConcepto ],
                                                tipoDeAportantes: [ dataAportantes.listaTipoAportante ],
                                                nombreDeAportantes: [ dataAportantes.listaNombreAportante ],
                                                descuento: this.fb.group(
                                                    {
                                                        aplicaDescuentos:[ terceroCausacion.tieneDescuento, Validators.required ],
                                                        numeroDescuentos: [ terceroCausacion.ordenGiroDetalleTerceroCausacionDescuento.length > 0 ? terceroCausacion.ordenGiroDetalleTerceroCausacionDescuento.length : null, Validators.required ],
                                                        descuentos: this.fb.array( listaDescuentos )
                                                    }
                                                ),
                                                aportantes: this.fb.array( listaAportantes )
                                            }
                                        ) ) 
                                    }, 600);
                                }

                                // Set formulario criterios
                                setTimeout( async () => {
                                    // Get observaciones
                                    const historialObservaciones = [];
                                    let estadoSemaforo = 'sin-diligenciar';

                                    const listaObservacionVerificar = await this.obsOrdenGiro.getObservacionOrdenGiroByMenuIdAndSolicitudPagoId(
                                        this.listaMenu.verificarOrdenGiro,
                                        this.ordenGiroId,
                                        terceroCausacion.ordenGiroDetalleTerceroCausacionId,
                                        this.tipoObservaciones.terceroCausacion );
                                    const listaObservacionAprobar = await this.obsOrdenGiro.getObservacionOrdenGiroByMenuIdAndSolicitudPagoId(
                                        this.listaMenu.aprobarOrdenGiro,
                                        this.ordenGiroId,
                                        terceroCausacion.ordenGiroDetalleTerceroCausacionId,
                                        this.tipoObservaciones.terceroCausacion );
                                    const listaObservacionTramitar = await this.obsOrdenGiro.getObservacionOrdenGiroByMenuIdAndSolicitudPagoId(
                                            this.listaMenu.tramitarOrdenGiro,
                                            this.ordenGiroId,
                                            terceroCausacion.ordenGiroDetalleTerceroCausacionId,
                                            this.tipoObservaciones.terceroCausacion );
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
                                    const obsArchivadasVerificar = listaObservacionVerificar.filter( obs => obs.archivada === true );
                                    const obsArchivadasAprobar = listaObservacionAprobar.filter( obs => obs.archivada === true );
                                    const obsArchivadasTramitar = listaObservacionTramitar.filter( obs => obs.archivada === true );
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

                                    this.criterios.push( this.fb.group(
                                        {
                                            estadoSemaforo,
                                            historialObservaciones: [ historialObservaciones ],
                                            ordenGiroObservacionId: [ observacion !== undefined ? ( observacion.ordenGiroObservacionId !== undefined ? observacion.ordenGiroObservacionId : 0 ) : 0 ],
                                            tieneObservaciones: [ observacion !== undefined ? ( observacion.tieneObservacion !== undefined ? observacion.tieneObservacion : null ) : null, Validators.required ],
                                            observaciones: [ observacion !== undefined ? ( observacion.observacion !== undefined ? ( observacion.observacion.length > 0 ? observacion.observacion : null ) : null ) : null, Validators.required ],
                                            fechaCreacion: [ observacion !== undefined ? ( observacion.fechaCreacion !== undefined ? observacion.fechaCreacion : null ) : null ],
                                            ordenGiroDetalleTerceroCausacionId: [ terceroCausacion.ordenGiroDetalleTerceroCausacionId ],
                                            tipoCriterioCodigo: [ criterio.tipoCriterioCodigo ],
                                            nombre: [ criterio.nombre ],
                                            tipoPagoCodigo: [ criterio.tipoPagoCodigo ],
                                            conceptos: this.fb.array( conceptosDePago )
                                        }
                                    ) )
                                }, 1000);
                            }
                        }
                        setTimeout(() => {
                            if ( this.totalEnProceso > 0 && this.totalEnProceso === this.criterios.length ) {
                                this.estadoSemaforo.emit( 'en-proceso' );
                            }
                            if ( this.totalCompleto > 0 && this.totalCompleto < this.criterios.length ) {
                                this.estadoSemaforo.emit( 'en-proceso' );
                            }
                            if ( this.totalCompleto > 0 && this.totalCompleto === this.criterios.length ) {
                                this.estadoSemaforo.emit( 'completo' );
                            }
                        }, 2000);
                    } );
                } 
            );
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

    guardar( criterio: FormGroup ) {
        if ( criterio.get( 'tieneObservaciones' ).value === false && criterio.get( 'observaciones' ).value !== null ) {
            criterio.get( 'observaciones' ).setValue( '' );
        }

        const pOrdenGiroObservacion = {
            ordenGiroObservacionId: criterio.get( 'ordenGiroObservacionId' ).value,
            ordenGiroId: this.ordenGiroId,
            tipoObservacionCodigo: this.tipoObservaciones.terceroCausacion,
            menuId: this.listaMenu.verificarOrdenGiro,
            idPadre: criterio.get( 'ordenGiroDetalleTerceroCausacionId' ).value,
            observacion: criterio.get( 'observaciones' ).value,
            tieneObservacion: criterio.get( 'tieneObservaciones' ).value
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
