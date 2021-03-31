import { OrdenPagoService } from './../../../../core/_services/ordenPago/orden-pago.service';
import { CommonService } from './../../../../core/_services/common/common.service';
import { Dominio } from 'src/app/core/_services/common/common.service';
import { MatDialog } from '@angular/material/dialog';
import { Component, Input, OnInit } from '@angular/core';
import { FormGroup, Validators, FormBuilder, FormArray } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { RegistrarRequisitosPagoService } from 'src/app/core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';
import humanize from 'humanize-plus';

@Component({
  selector: 'app-descuentos-direccion-tecnica',
  templateUrl: './descuentos-direccion-tecnica.component.html',
  styleUrls: ['./descuentos-direccion-tecnica.component.scss']
})
export class DescuentosDireccionTecnicaComponent implements OnInit {

    @Input() solicitudPago: any;
    @Input() esVerDetalle: boolean;
    @Input() esRegistroNuevo: boolean;
    listaTipoDescuento: Dominio[] = [];
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
        private registrarPagosSvc: RegistrarRequisitosPagoService )
    {
        this.commonSvc.tiposDescuento()
            .subscribe(response => this.listaTipoDescuento = response);
    }

    ngOnInit(): void {
        this.getDireccionTecnica();
    }

    getDireccionTecnica() {
        // Get Tablas
        this.solicitudPagoFase = this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase[0];
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
                                            }, 1500);
                                        }
                                    }
                                }
                                let estadoSemaforo = 'sin-diligenciar';

                                setTimeout(() => {
                                    // Set formulario de los descuentos
                                    detalleDescuentoTecnica.forEach( descuentoValue => {
                                        this.descuentos.controls.push( this.fb.group(
                                            {
                                                estadoSemaforo,
                                                tieneObservaciones: [ null, Validators.required ],
                                                observaciones: [ null, Validators.required ],
                                                fechaCreacion: [ null ],
                                                ordenGiroDetalleDescuentoTecnicaId: [ descuentoValue.ordenGiroDetalleDescuentoTecnicaId ],
                                                solicitudPagoFaseFacturaDescuentoId: [ descuentoValue.solicitudPagoFaseFacturaDescuentoId ],
                                                tipoDescuentoCodigo: [ descuento.tipoDescuentoCodigo ],
                                                criterio: [ listaCriterios, Validators.required ],
                                                criterios: this.fb.array( formArrayCriterios )
                                            }
                                        ) );
                                    } )
                                }, 2000);
                            }
                        }
                    }
                }

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

    guardar() {
        if ( this.formObservacion.get( 'tieneObservaciones' ).value === false && this.formObservacion.get( 'observaciones' ).value !== null ) {
            this.formObservacion.get( 'observaciones' ).setValue( '' );
        }
        console.log( this.formObservacion );
    }

}
