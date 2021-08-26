import { filter } from 'rxjs/operators';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { OrdenPagoService } from './../../../../core/_services/ordenPago/orden-pago.service';
import { TipoAportanteDominio, TipoAportanteCodigo, ListaMenu, ListaMenuId, TipoObservaciones, TipoObservacionesCodigo } from './../../../../_interfaces/estados-solicitudPago-ordenGiro.interface';
import { CommonService, Dominio } from './../../../../core/_services/common/common.service';
import { Component, Input, OnInit, OnChanges, SimpleChanges, Output, EventEmitter } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { RegistrarRequisitosPagoService } from 'src/app/core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';
import humanize from 'humanize-plus';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { ObservacionesOrdenGiroService } from 'src/app/core/_services/observacionesOrdenGiro/observaciones-orden-giro.service';

@Component({
  selector: 'app-tercero-causacion-gog',
  templateUrl: './tercero-causacion-gog.component.html',
  styleUrls: ['./tercero-causacion-gog.component.scss']
})
export class TerceroCausacionGogComponent implements OnInit {

    @Input() solicitudPago: any;
    @Input() esVerDetalle: boolean;
    @Input() esPreconstruccion: boolean;
    @Input() solicitudPagoFase: any;
    @Output() estadoSemaforo = new EventEmitter<string>();
    listaMenu: ListaMenu = ListaMenuId;
    tipoObservaciones: TipoObservaciones = TipoObservacionesCodigo;
    addressForm: FormGroup;
    tipoDescuentoArray: Dominio[] = [];
    listaTipoDescuento: Dominio[] = [];
    listaCriterios: Dominio[] = [];
    listaFuenteTipoFinanciacion: Dominio[] = [];
    cantidadAportantes: number;
    solicitudPagoFaseCriterio: any;
    solicitudPagoFaseFacturaDescuento: any;
    fasePreConstruccionFormaPagoCodigo: any;
    ordenGiroDetalle: any;
    ordenGiroDetalleTerceroCausacion: any[];
    variosAportantes: boolean;
    estaEditando = false;
    valorNetoGiro = 0;
    ordenGiroId = 0;
    ordenGiroDetalleId = 0;

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

    getAportanteDescuentos( index: number, jIndex: number, kIndex: number ) {
        return this.getDescuentos( index, jIndex ).controls[ kIndex ].get( 'aportantesDescuento' ) as FormArray;
    }

    constructor (
        private fb: FormBuilder,
        private commonSvc: CommonService,
        private dialog: MatDialog,
        private routes: Router,
        private registrarPagosSvc: RegistrarRequisitosPagoService,
        private ordenGiroSvc: OrdenPagoService,
        private obsOrdenGiro: ObservacionesOrdenGiroService )
    {
        this.commonSvc.listaFuenteTipoFinanciacion()
            .subscribe( listaFuenteTipoFinanciacion => this.listaFuenteTipoFinanciacion = listaFuenteTipoFinanciacion );
        this.crearFormulario();
    }

    ngOnInit(): void {
        this.getTerceroCausacion();
    }

    crearFormulario () {
        this.addressForm = this.fb.group(
            {
                criterios: this.fb.array( [] )
            }
        );
    }

    async getTerceroCausacion() {
        this.tipoDescuentoArray = await this.commonSvc.listaDescuentosOrdenGiro().toPromise();
        this.listaTipoDescuento = await this.commonSvc.listaDescuentosOrdenGiro().toPromise();
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
        this.solicitudPagoFaseCriterio = this.solicitudPagoFase.solicitudPagoFaseCriterio;
        this.solicitudPagoFaseFacturaDescuento = this.solicitudPagoFase.solicitudPagoFaseFacturaDescuento;

        if ( this.solicitudPago.contratoSon.solicitudPago.length > 1 ) {
            this.fasePreConstruccionFormaPagoCodigo = this.solicitudPago.contratoSon.solicitudPago[0].solicitudPagoCargarFormaPago[0];
        } else {
            this.fasePreConstruccionFormaPagoCodigo = this.solicitudPago.solicitudPagoCargarFormaPago[0];
        }
        // Get data valor neto giro
        this.solicitudPagoFaseCriterio.forEach( criterio => this.valorNetoGiro += criterio.valorFacturado );
        if ( this.solicitudPagoFaseFacturaDescuento.length > 0 ) {
            this.solicitudPagoFaseFacturaDescuento.forEach( descuento => this.valorNetoGiro -= descuento.valorDescuento );
        }
        //this.solicitudPago.ordenGiro.ordenGiroDetalle[0].ordenGiroDetalleTerceroCausacion[0].ordenGiroDetalleTerceroCausacionDescuento

        this.solicitudPago.ordenGiro.ordenGiroDetalle.forEach( ordengiro => {
            ordengiro.ordenGiroDetalleTerceroCausacion.forEach(tercero => {
                tercero.ordenGiroDetalleTerceroCausacionDescuento.forEach(descuento => {
                    this.valorNetoGiro -= descuento.valorDescuento
                });

            });
        } );
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

                    const dataAportantes = await this.ordenGiroSvc.getAportantes( this.solicitudPago );

                    if ( this.solicitudPago.tablaUsoFuenteAportante !== undefined ) {
                        if ( this.solicitudPago.tablaUsoFuenteAportante.usos !== undefined ) {
                            if ( this.solicitudPago.tablaUsoFuenteAportante.usos.length > 0 ) {
                                this.solicitudPago.tablaUsoFuenteAportante.usos.forEach( uso => {
                                    if ( uso.fuentes !== undefined ) {
                                        if ( uso.fuentes.length > 0 ) {
                                            uso.fuentes.forEach( fuente => {
                                                if ( fuente.aportante !== undefined ) {
                                                    if ( fuente.aportante.length > 0 ) {
                                                        fuente.aportante.forEach( aportante => {
                                                            dataAportantes.listaNombreAportante.find( nombreAportante => {
                                                                if ( nombreAportante.cofinanciacionAportanteId === aportante.aportanteId ) {
                                                                    nombreAportante.valorActual = Number( aportante.valorUso[ 0 ].valorActual.split( '.' ).join( '' ) )
                                                                }
                                                            } )
                                                        } )
                                                    }
                                                }
                                            } )
                                        }
                                    }
                                } )
                            }
                        }
                    }

                        // Get cantidad de aportantes para limitar cuantos aportantes se pueden agregar en el formulario
                        this.cantidadAportantes = dataAportantes.listaTipoAportante.length;
                        // Get data del guardado de tercero de causacion
                        for ( const criterio of listCriterios ) {
                          let terceroCausacionxCriterio;
                          if(this.ordenGiroDetalleTerceroCausacion != null){
                            terceroCausacionxCriterio = this.ordenGiroDetalleTerceroCausacion.filter( tercero => tercero.conceptoPagoCriterio === criterio.tipoCriterioCodigo && tercero.esPreconstruccion === this.esPreconstruccion && tercero.contratacionProyectoId === this.solicitudPagoFase.contratacionProyectoId );
                          }
                          const conceptosDePago = [];
                          for ( const concepto of criterio.listConceptos ) {
                            // debo crear un ordenGiroDetalleTerceroCausacion x conceptoCriterio
                            let terceroCausacion;
                            if(terceroCausacionxCriterio != null){
                              terceroCausacion = terceroCausacionxCriterio.find(r => r.conceptoCodigo == concepto.codigo);
                            }
                            const listDescuento = [ ...this.tipoDescuentoArray ];
                            const listaDescuentos = [];
                            const listaAportanteDescuentos = [];
                            //el ordenGiroDetalleTerceroCausacion tiene por debajo aportantes y descuentos
                            /**Descuentos */
                            if(terceroCausacion != null){
                              if ( terceroCausacion.ordenGiroDetalleTerceroCausacionDescuento.length > 0 ) {
                                for ( const descuento of listDescuento ) {
                                    const ordenGiroDetalleTerceroCausacionDescuento: any[] = terceroCausacion.ordenGiroDetalleTerceroCausacionDescuento.filter( ordenGiroDetalleTerceroCausacionDescuento => ordenGiroDetalleTerceroCausacionDescuento.tipoDescuentoCodigo === descuento.codigo );
                                    const listaAportanteDescuentos = [];

                                    if ( ordenGiroDetalleTerceroCausacionDescuento.length > 0 ) {
                                        for ( const terceroCausacionDescuento of ordenGiroDetalleTerceroCausacionDescuento ) {
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
                                                        ordenGiroDetalleTerceroCausacionId: [ terceroCausacion.ordenGiroDetalleTerceroCausacionId ],
                                                        ordenGiroDetalleTerceroCausacionDescuentoId: [ terceroCausacionDescuento.ordenGiroDetalleTerceroCausacionDescuentoId ],
                                                        nombreAportante: [ nombreAportante !== undefined ? nombreAportante : null, Validators.required ],
                                                        valorDescuento: [ terceroCausacionDescuento.valorDescuento, Validators.required ],
                                                        fuente: [ { value: fuente !== undefined ? fuente : null, disabled: true }, Validators.required ]
                                                    }
                                                )
                                            )
                                        }

                                        if ( listaAportanteDescuentos.length === 0 )  {
                                            listaAportanteDescuentos.push(
                                                this.fb.group(
                                                    {
                                                        ordenGiroDetalleTerceroCausacionId: [ terceroCausacion.ordenGiroDetalleTerceroCausacionId ],
                                                        ordenGiroDetalleTerceroCausacionDescuentoId: [ 0 ],
                                                        nombreAportante: [ null, Validators.required ],
                                                        valorDescuento: [ null, Validators.required ],
                                                        fuente: [ { value: null, disabled: true }, Validators.required ]
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
                                });
                              }
                                /**Aportantes */
                            const listaAportantes = [];
                            if ( terceroCausacion.ordenGiroDetalleTerceroCausacionAportante.filter(r => r.conceptoPagoCodigo == concepto.codigo).length === 0 )  {
                              listaAportantes.push(
                                this.fb.group(
                                    {
                                        ordenGiroDetalleTerceroCausacionId: [ terceroCausacion.ordenGiroDetalleTerceroCausacionId ],
                                        ordenGiroDetalleTerceroCausacionAportanteId: [ 0 ],
                                        tipoAportante: [ null, Validators.required ],
                                        listaNombreAportantes: [ null ],
                                        nombreAportante: [ null, Validators.required ],
                                        fuenteDeRecursos: [ null ],
                                        fuenteRecursos: [ null, Validators.required ],
                                        fuenteFinanciacionId: [ null ],
                                        valorDescuento: [ null, Validators.required ],
                                        valorDescuentoTecnica: [ null ]
                                    }
                                )
                              )
                          }
                            for ( const aportante of terceroCausacion.ordenGiroDetalleTerceroCausacionAportante.filter(r => r.conceptoPagoCodigo == concepto.codigo)) {
                                const nombreAportante = dataAportantes.listaNombreAportante.find( nombre => nombre.cofinanciacionAportanteId === aportante.aportanteId );

                                if ( nombreAportante !== undefined ) {
                                    const tipoAportante = dataAportantes.listaTipoAportante.find( tipo => tipo.dominioId === nombreAportante.tipoAportanteId );
                                    const tipoAportanteIndex = dataAportantes.listaTipoAportante.findIndex( tipo => tipo.dominioId === nombreAportante.tipoAportanteId );
                                    let listaFuenteRecursos: any[] = await this.ordenGiroSvc.getFuentesDeRecursosPorAportanteId( nombreAportante.cofinanciacionAportanteId ).toPromise();
                                    const fuente = listaFuenteRecursos.find( fuente => fuente.codigo === aportante.fuenteRecursoCodigo );
                                    listaAportanteDescuentos.push( nombreAportante )

                                    listaAportantes.push(
                                        this.fb.group(
                                            {
                                                ordenGiroDetalleTerceroCausacionId: [ terceroCausacion.ordenGiroDetalleTerceroCausacionId ],
                                                ordenGiroDetalleTerceroCausacionAportanteId: [ aportante.ordenGiroDetalleTerceroCausacionAportanteId ],
                                                tipoAportante: [ tipoAportante, Validators.required ],
                                                listaNombreAportantes: [ [ nombreAportante ] ],
                                                nombreAportante: [ nombreAportante, Validators.required ],
                                                fuenteDeRecursos: [ listaFuenteRecursos ],
                                                fuenteRecursos: [ fuente, Validators.required ],
                                                fuenteFinanciacionId: [ fuente.fuenteFinanciacionId ],
                                                valorDescuento: [ aportante.valorDescuento, Validators.required ],
                                                valorDescuentoTecnica: [ null ]
                                            }
                                        )
                                    )

                                    // if ( tipoAportanteIndex !== -1 ) {
                                    //     dataAportantes.listaTipoAportante.splice( tipoAportanteIndex, 1 )
                                    // }
                                }else{
                                  listaAportantes.push(
                                    this.fb.group(
                                        {
                                            ordenGiroDetalleTerceroCausacionId: [ terceroCausacion.ordenGiroDetalleTerceroCausacionId ],
                                            ordenGiroDetalleTerceroCausacionAportanteId: [ 0 ],
                                            tipoAportante: [ null, Validators.required ],
                                            listaNombreAportantes: [ null ],
                                            nombreAportante: [ null, Validators.required ],
                                            fuenteDeRecursos: [ null ],
                                            fuenteRecursos: [ null, Validators.required ],
                                            fuenteFinanciacionId: [ null ],
                                            valorDescuento: [ null, Validators.required ],
                                            valorDescuentoTecnica: [ null ]
                                        }
                                    )
                                  )
                                }
                            }
                            //
                            const usoByConcepto = await this.registrarPagosSvc.getUsoByConceptoPagoCriterioCodigo( concepto.codigo, this.solicitudPago.contratoId );
                            let valorTotalUso = 0;
                            if ( usoByConcepto.length > 0 ) {
                                usoByConcepto.forEach( uso => valorTotalUso += uso.valorUso );
                            }

                            conceptosDePago.push( this.fb.group(
                              {
                                  ordenGiroDetalleTerceroCausacionId: [ terceroCausacion.ordenGiroDetalleTerceroCausacionId ],
                                  conceptoPagoCriterio: [ concepto.codigo ],
                                  nombre: [ concepto.nombre ],
                                  valorTotalUso: [ valorTotalUso ],
                                  valorFacturadoConcepto: [ concepto.valorFacturadoConcepto ],
                                  tipoDeAportantes: [ dataAportantes.listaTipoAportante ],
                                  nombreDeAportantes: [ dataAportantes.listaNombreAportante ],
                                  tipoDescuentoArray: [ listDescuento ],
                                  descuento: this.fb.group(
                                      {
                                          ordenGiroDetalleTerceroCausacionId: [ terceroCausacion.ordenGiroDetalleTerceroCausacionId ],
                                          aplicaDescuentos:[ terceroCausacion.tieneDescuento, Validators.required ],
                                          numeroDescuentos: [ listaDescuentos.length > 0 ? listaDescuentos.length : null, Validators.required ],
                                          listaAportanteDescuentos: [ listaAportanteDescuentos, Validators.required ],
                                          descuentos: this.fb.array( listaDescuentos )
                                      }
                                  ),
                                  aportantes: this.fb.array( listaAportantes )
                              }
                            ))
                            }else {
                              const usoByConcepto = await this.registrarPagosSvc.getUsoByConceptoPagoCriterioCodigo( concepto.codigo, this.solicitudPago.contratoId );
                              let valorTotalUso = 0;
                              if ( usoByConcepto.length > 0 ) {
                                  usoByConcepto.forEach( uso => valorTotalUso += uso.valorUso );
                              }

                              conceptosDePago.push( this.fb.group(
                                {
                                    ordenGiroDetalleTerceroCausacionId: [ 0 ],
                                    conceptoPagoCriterio: [ concepto.codigo ],
                                    nombre: [ concepto.nombre ],
                                    valorTotalUso: [ valorTotalUso ],
                                    valorFacturadoConcepto: [ concepto.valorFacturadoConcepto ],
                                    tipoDeAportantes: [ dataAportantes.listaTipoAportante ],
                                    nombreDeAportantes: [ dataAportantes.listaNombreAportante ],
                                    tipoDescuentoArray: [ this.tipoDescuentoArray ],
                                    descuento: this.fb.group(
                                        {
                                            ordenGiroDetalleTerceroCausacionId: [ 0 ],
                                            aplicaDescuentos:[ null, Validators.required ],
                                            numeroDescuentos: [ null, Validators.required ],
                                            listaAportanteDescuentos: [ [], Validators.required ],
                                            descuentos: this.fb.array( [] )
                                        }
                                    ),
                                    aportantes: this.fb.array( [
                                        this.fb.group(
                                            {
                                                ordenGiroDetalleTerceroCausacionId: [ 0 ],
                                                ordenGiroDetalleTerceroCausacionAportanteId: [ 0 ],
                                                tipoAportante: [ null, Validators.required ],
                                                listaNombreAportantes: [ null ],
                                                nombreAportante: [ null, Validators.required ],
                                                fuenteDeRecursos: [ null ],
                                                fuenteRecursos: [ null, Validators.required ],
                                                fuenteFinanciacionId: [ null ],
                                                valorDescuento: [ null, Validators.required ],
                                                valorDescuentoTecnica: [ null ]
                                            }
                                        )
                                    ] )
                                }
                            ) )
                          }

                          }
                          //acaaa?
                          //
                        // Set formulario criterios
                        // Get observaciones
                        if(terceroCausacionxCriterio?.length > 0){
                          let estadoSemaforo = terceroCausacionxCriterio[0].registroCompleto === true ? 'completo' : 'en-proceso';
                          let obsVerificar = undefined;
                          let obsAprobar = undefined;
                          let obsTramitar = undefined;

                          const listaObservacionVerificar = await this.obsOrdenGiro.getObservacionOrdenGiroByMenuIdAndSolicitudPagoId(
                              this.listaMenu.verificarOrdenGiro,
                              this.ordenGiroId,
                              terceroCausacionxCriterio[0].ordenGiroDetalleTerceroCausacionId,
                              this.tipoObservaciones.terceroCausacion );
                          const listaObservacionAprobar = await this.obsOrdenGiro.getObservacionOrdenGiroByMenuIdAndSolicitudPagoId(
                              this.listaMenu.aprobarOrdenGiro,
                              this.ordenGiroId,
                              terceroCausacionxCriterio[0].ordenGiroDetalleTerceroCausacionId,
                              this.tipoObservaciones.terceroCausacion );
                          const listaObservacionTramitar = await this.obsOrdenGiro.getObservacionOrdenGiroByMenuIdAndSolicitudPagoId(
                                  this.listaMenu.tramitarOrdenGiro,
                                  this.ordenGiroId,
                                  terceroCausacionxCriterio[0].ordenGiroDetalleTerceroCausacionId,
                                  this.tipoObservaciones.terceroCausacion );
                          // Get lista de observacion y observacion actual
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
                          this.criterios.push( this.fb.group(
                            {
                                estadoSemaforo,
                                obsVerificar: [ obsVerificar ],
                                obsAprobar: [ obsAprobar ],
                                obsTramitar: [ obsTramitar ],
                                tipoCriterioCodigo: [ criterio.tipoCriterioCodigo ],
                                nombre: [ criterio.nombre ],
                                tipoPagoCodigo: [ criterio.tipoPagoCodigo ],
                                conceptos: this.fb.array( conceptosDePago )
                            }
                        ) )
                        }else{
                          this.criterios.push( this.fb.group(
                            {
                                estadoSemaforo: [ 'sin-diligenciar' ],
                                obsVerificar: [ null ],
                                obsAprobar: [ null ],
                                obsTramitar: [ null ],
                                tipoCriterioCodigo: [ criterio.tipoCriterioCodigo ],
                                nombre: [ criterio.nombre ],
                                tipoPagoCodigo: [ criterio.tipoPagoCodigo ],
                                conceptos: this.fb.array( conceptosDePago )
                            }
                        ) )
                        }
                        }
                    const totalRegistrosCompletos = this.criterios.controls.filter( control => control.get( 'estadoSemaforo' ).value === 'completo' ).length
                    const totalRegistrosEnProceso = this.criterios.controls.filter( control => control.get( 'estadoSemaforo' ).value === 'en-proceso' ).length

                    if ( totalRegistrosCompletos > 0 && totalRegistrosCompletos === this.criterios.length ) {
                        this.estadoSemaforo.emit( 'completo' )
                    }

                    if ( totalRegistrosCompletos > 0 && totalRegistrosCompletos < this.criterios.length ) {
                        this.estadoSemaforo.emit( 'en-proceso' )
                    }

                    if ( totalRegistrosEnProceso > 0 && totalRegistrosEnProceso < this.criterios.length ) {
                        this.estadoSemaforo.emit( 'en-proceso' )
                    }

                    if ( totalRegistrosEnProceso > 0 && totalRegistrosEnProceso === this.criterios.length ) {
                        this.estadoSemaforo.emit( 'en-proceso' )
                    }
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

    getTipoDescuento( codigo: string ): Dominio[] {
        if ( this.listaTipoDescuento.length > 0 ) {
            const descuento = this.listaTipoDescuento.find( descuento => descuento.codigo === codigo );

            if ( descuento !== undefined ) {
                return [ descuento ];
            }
        }
    }

    getCodigoDescuento( codigo: string, index: number, jIndex: number ) {
        const listaDescuento: Dominio[] = this.getConceptos( index ).controls[ jIndex ].get( 'tipoDescuentoArray' ).value;
        const descuentoIndex = listaDescuento.findIndex( descuento => descuento.codigo === codigo );

        if ( descuentoIndex !== -1 ) {
            listaDescuento.splice( descuentoIndex, 1 );
            this.getConceptos( index ).controls[ jIndex ].get( 'tipoDescuentoArray' ).setValue( listaDescuento );
        }
    }

    checkTotalValueAportantes( cb: { ( totalValueAportantes: number ): void } ) {
        let totalAportantes = 0;

        this.criterios.controls.forEach( ( criterioControl, criterioIndex ) => {

            this.getConceptos( criterioIndex ).controls.forEach( ( conceptoControl, conceptoIndex ) => {

                this.getAportantes( criterioIndex, conceptoIndex ).controls.forEach( aportanteControl => totalAportantes += aportanteControl.get( 'valorDescuento' ).value );
            } )
        } )

        cb( totalAportantes );
    }
    // Check valor del descuento del aportante
    validateDiscountAportanteValue( value: number, index: number, jIndex: number, kIndex: number ) {
        if ( value !== null ) {
            if ( value < 0 ) {
                this.getAportantes( index, jIndex ).controls[ kIndex ].get( 'valorDescuento' ).setValue( null )
                return
            }
            let totalValueAportante = 0;
            this.getAportantes( index, jIndex ).controls.forEach( aportanteControl => {
                if ( aportanteControl.get( 'valorDescuento' ).value !== null ) {
                    totalValueAportante += aportanteControl.get( 'valorDescuento' ).value;
                }
            } )

            if ( this.getAportantes( index, jIndex ).controls[ kIndex ].get( 'nombreAportante' ).value !== null ) {
                if ( value > this.getAportantes( index, jIndex ).controls[ kIndex ].get( 'nombreAportante' ).value.valorActual ) {
                    this.getAportantes( index, jIndex ).controls[ kIndex ].get( 'valorDescuento' ).setValue( null );
                    this.openDialog( '', `<b>El valor facturado por el concepto para el aportante no puede ser mayor al valor asignado por DRP al aportante.</b>` )
                    return
                }
            }

            if ( totalValueAportante > this.getConceptos( index ).controls[ jIndex ].get( 'valorTotalUso' ).value ) {
                this.getAportantes( index, jIndex ).controls[ kIndex ].get( 'valorDescuento' ).setValue( null );
                this.openDialog( '', `<b>La suma total del valor facturado por el concepto para el aportante no puede ser mayor al valor del uso asociado al concepto.</b>` )
                return
            }


            let ordenGiroDetalleDescuentoTecnica = [];
            const ordenGiroDetalleDescuentoTecnicaAportante = [];
            let totalDescuentoAportante = 0;
            if ( this.ordenGiroDetalle !== undefined ) {
                if ( this.ordenGiroDetalle.ordenGiroDetalleDescuentoTecnica !== undefined ) {
                    ordenGiroDetalleDescuentoTecnica = this.ordenGiroDetalle.ordenGiroDetalleDescuentoTecnica.filter( ordenGiroDetalleDescuentoTecnica => ordenGiroDetalleDescuentoTecnica.esPreconstruccion === this.esPreconstruccion );
                }
            }

            if ( ordenGiroDetalleDescuentoTecnica.length > 0 ) {
                for ( const descuentoTecnica of ordenGiroDetalleDescuentoTecnica ) {
                    if ( descuentoTecnica.ordenGiroDetalleDescuentoTecnicaAportante !== undefined ) {
                        if ( descuentoTecnica.ordenGiroDetalleDescuentoTecnicaAportante.length > 0 ) {
                            if ( this.getAportantes( index, jIndex ).controls[ kIndex ].get( 'nombreAportante' ).value !== null ) {
                                const aportante = descuentoTecnica.ordenGiroDetalleDescuentoTecnicaAportante.find(
                                    descuentoTecnicaAportante => descuentoTecnicaAportante.conceptoPagoCodigo === this.getConceptos( index ).controls[ jIndex ].get( 'conceptoPagoCriterio' ).value && descuentoTecnicaAportante.aportanteId === this.getAportantes( index, jIndex ).controls[ kIndex ].get( 'nombreAportante' ).value.cofinanciacionAportanteId
                                )

                                if ( aportante !== undefined ) {
                                    ordenGiroDetalleDescuentoTecnicaAportante.push( aportante );
                                }
                            }
                        }
                    }
                }
            }

            if ( ordenGiroDetalleDescuentoTecnicaAportante.length > 0 ) {
                ordenGiroDetalleDescuentoTecnicaAportante.forEach( descuentoTecnica => totalDescuentoAportante += descuentoTecnica.valorDescuento );
            }

            this.getAportantes( index, jIndex ).controls[ kIndex ].get( 'valorDescuentoTecnica' ).setValue( totalDescuentoAportante );
            if ( totalValueAportante > this.getConceptos( index ).controls[ jIndex ].get( 'valorFacturadoConcepto' ).value ) {
                this.getAportantes( index, jIndex ).controls[ kIndex ].get( 'valorDescuento' ).setValue( null );
                this.openDialog( '', `<b>La suma total del valor facturado por el concepto para el aportante no puede ser mayor al valor facturado por concepto.</b>` )
            }
        }
    }
    // Check valor del descuento de los conceptos
    validateDiscountValue( value: number, index: number, jIndex: number, kIndex: number, lIndex: number ) {
        let totalAportantePorConcepto = 0;

        if ( value !== null ) {
            if ( value < 0 ) {
                this.getDescuentos( index, jIndex ).controls[ kIndex ].get( 'valorDescuento' ).setValue( null )
                return
            }
        }

        for ( const aportante of this.getConceptos( index ).controls[ jIndex ].get( 'aportantes' ).value ) {
            totalAportantePorConcepto += aportante.valorDescuento;
        }

        if ( value > totalAportantePorConcepto ) {
            this.getAportanteDescuentos( index, jIndex, kIndex ).controls[ lIndex ].get( 'valorDescuento' ).setValue( null )
            this.openDialog( '', `<b>El valor del descuento del concepto de pago no puede ser mayor al valor total de los aportantes.</b>` );
        }
    }

    getAportanteDescuento( aportante: any, index: number, jIndex: number, kIndex: number, lIndex: number ) {

        const aportantes = this.getConceptos( index ).controls[ jIndex ].get( 'aportantes' ) as FormArray;

        const aportanteControl = aportantes.controls.find( aportanteControl => aportanteControl.get( 'nombreAportante' ).value.tipoAportanteId === aportante.tipoAportanteId ) as FormGroup;
        // console.log( aportantes )
        // console.log( aportante )
        // console.log( aportanteControl )

        if ( aportanteControl !== undefined ) {
            this.getAportanteDescuentos( index, jIndex, kIndex ).controls[ lIndex ].get( 'fuente' ).setValue( aportanteControl.get( 'fuenteRecursos' ).value )
        }
    }

    validateNumberKeypress(event: KeyboardEvent) {
        const alphanumeric = /[0-9]/;
        const inputChar = String.fromCharCode(event.charCode);
        return alphanumeric.test(inputChar) ? true : false;
    }
    // Metodos para el formulario de addressForm
    valuePendingTipoAportante( aportanteSeleccionado: Dominio, index: number, jIndex: number, kIndex: number ) {
        const listaAportantes: Dominio[] = this.getConceptos( index ).controls[ jIndex ].get( 'tipoDeAportantes' ).value;
        const aportanteIndex = listaAportantes.findIndex( aportante => aportante.codigo === aportanteSeleccionado.codigo );
        const listaNombreAportantes: any[] = this.getConceptos( index ).controls[ jIndex ].get( 'nombreDeAportantes' ).value;
        const filterAportantesDominioId = listaNombreAportantes.filter( aportante => aportante.tipoAportanteId === aportanteSeleccionado.dominioId );

        if ( aportanteIndex !== -1 ) {
            listaAportantes.splice( aportanteIndex, 1 )
        }

        if ( filterAportantesDominioId.length > 0 ) {
            this.getAportantes( index, jIndex ).controls[ kIndex ].get( 'listaNombreAportantes' ).setValue( filterAportantesDominioId );
        }
    }

    getListaFuenteRecursos( nombreAportante: any, index: number, jIndex: number, kIndex: number ) {
        const listaAportanteDescuentos: any[] = this.getConceptos( index ).controls[ jIndex ].get( 'descuento' ).get( 'listaAportanteDescuentos' ).value;

        if ( listaAportanteDescuentos.length > 0 ) {
            const aportante = listaAportanteDescuentos.find( aportante => aportante.tipoAportanteId === nombreAportante.tipoAportanteId );

            if ( aportante === undefined ) {
                listaAportanteDescuentos.push( nombreAportante );
            }
        } else {
            listaAportanteDescuentos.push( nombreAportante );
        }

        this.getConceptos( index ).controls[ jIndex ].get( 'descuento' ).get( 'listaAportanteDescuentos' ).setValue( listaAportanteDescuentos )

        this.ordenGiroSvc.getFuentesDeRecursosPorAportanteId( nombreAportante.cofinanciacionAportanteId )
            .subscribe( fuenteRecursos => this.getAportantes( index, jIndex ).controls[ kIndex ].get( 'fuenteDeRecursos' ).setValue( fuenteRecursos ) );
    }

    deleteAportanteDescuento( index: number, jIndex: number, kIndex: number, lIndex: number ) {
        this.openDialogTrueFalse( '', '<b>¿Está seguro de eliminar esta información?</b>' )
            .subscribe(
                value => {
                    if ( value === true ) {
                        // deleteOrdenGiroDetalleTerceroCausacionDescuento
                        if ( this.getAportanteDescuentos( index, jIndex, kIndex ).controls[ lIndex ].get( 'ordenGiroDetalleTerceroCausacionDescuentoId' ).value !== 0 ) {
                            this.ordenGiroSvc.deleteOrdenGiroDetalleTerceroCausacionDescuento( [ this.getAportanteDescuentos( index, jIndex, kIndex ).controls[ lIndex ].get( 'ordenGiroDetalleTerceroCausacionDescuentoId' ).value ] )
                                .subscribe(
                                    response => {
                                        this.getAportanteDescuentos( index, jIndex, kIndex ).removeAt( lIndex );
                                        this.openDialog( '', `<b>${ response.message }</b>` );
                                    }
                                )
                        } else {
                            this.getAportanteDescuentos( index, jIndex, kIndex ).removeAt( lIndex );
                            this.openDialog( '', '<b>La información se ha eliminado correctamente.</b>' );
                        }
                    }
                }
            )
    }

    addAportanteDescuento( index: number, jIndex: number, kIndex: number, lIndex: number ) {
        const listaAportanteDescuentos: any[] = this.getConceptos( index ).controls[ jIndex ].get( 'descuento' ).get( 'listaAportanteDescuentos' ).value;

        if ( this.getAportanteDescuentos( index, jIndex, kIndex ).length < listaAportanteDescuentos.length ) {
            this.getAportanteDescuentos( index, jIndex, kIndex ).push(
                this.fb.group(
                    {
                        ordenGiroDetalleTerceroCausacionDescuentoId: [ 0 ],
                        nombreAportante: [ null, Validators.required ],
                        valorDescuento: [ null, Validators.required ],
                        fuente: [ { value: null, disabled: true }, Validators.required ]
                    }
                )
            )
        }
    }

    deleteAportante( index: number, jIndex: number, kIndex: number ) {
        this.openDialogTrueFalse( '', '<b>¿Está seguro de eliminar esta información?</b>' )
            .subscribe(
                value => {
                    if ( value === true ) {
                        const aportanteSeleccionado = this.getAportantes( index, jIndex ).controls[ kIndex ].get( 'tipoAportante' ).value;

                        if ( aportanteSeleccionado !== null ) {
                            const listaTipoAportantes = this.getConceptos( index ).controls[ jIndex ].get( 'tipoDeAportantes' ).value;
                            listaTipoAportantes.push( aportanteSeleccionado );

                            if ( this.getAportantes( index, jIndex ).controls[ kIndex ].get( 'ordenGiroDetalleTerceroCausacionAportanteId' ).value !== 0 ) {
                                this.ordenGiroSvc.deleteOrdenGiroDetalleTerceroCausacionAportante( this.getAportantes( index, jIndex ).controls[ kIndex ].get( 'ordenGiroDetalleTerceroCausacionAportanteId' ).value )
                                    .subscribe(
                                        response => {
                                            this.getAportantes( index, jIndex ).removeAt( kIndex );
                                            this.openDialog( '', `${ response.message }` );
                                        }
                                    )
                            } else {
                                this.getAportantes( index, jIndex ).removeAt( kIndex );
                                this.openDialog( '', '<b>La información se ha eliminado correctamente.</b>' );
                            }
                        } else {
                            this.getAportantes( index, jIndex ).removeAt( kIndex );
                            this.openDialog( '', '<b>La información se ha eliminado correctamente.</b>' );
                        }
                    }
                }
            )
    }

    addAportante( index: number, jIndex: number ) {
        if ( this.getAportantes( index, jIndex ).length < this.cantidadAportantes ) {
            this.getAportantes( index, jIndex ).push(
                this.fb.group(
                    {
                        ordenGiroDetalleTerceroCausacionAportanteId: [ 0 ],
                        tipoAportante: [ null, Validators.required ],
                        listaNombreAportantes: [ null ],
                        nombreAportante: [ null, Validators.required ],
                        fuenteDeRecursos: [ null ],
                        fuenteRecursos: [ null, Validators.required ],
                        fuenteFinanciacionId: [ null ],
                        valorDescuento: [ null, Validators.required ],
                        valorDescuentoTecnica: [ null ]
                    }
                )
            )
        } else {
            this.openDialog( '', '<b>El contrato no tiene más aportantes asignados.</b>' )
        }
    }

    getCantidadDescuentos( value: number, index: number, jIndex: number ) {
        if ( value !== null && value > 0 ) {
            if (  this.getConceptos( index ).controls[ jIndex ].get( 'tipoDescuentoArray' ).value.length > 0 ) {
                if ( this.getDescuentos( index, jIndex ).dirty === true ) {
                    const nuevosDescuentos = value - this.getDescuentos( index, jIndex ).length;
                    this.getConceptos( index ).controls[ jIndex ].get( 'descuento' ).get( 'numeroDescuentos' ).setValidators( Validators.min( this.getDescuentos( index, jIndex ).length ) );

                    if ( value < this.getDescuentos( index, jIndex ).length ) {
                        this.openDialog( '', '<b>Debe eliminar uno de los registros diligenciados para disminuir el total de los registros requeridos.</b>' );
                        this.getConceptos( index ).controls[ jIndex ].get( 'descuento' ).get( 'numeroDescuentos' ).setValue( this.getDescuentos( index, jIndex ).length );
                        return;
                    }

                    for ( let i = 0; i < nuevosDescuentos; i++ ) {

                        this.getDescuentos( index, jIndex ).push(
                            this.fb.group(
                                {
                                    tipoDescuento: [ null, Validators.required ],
                                    aportantesDescuento: this.fb.array( [
                                        this.fb.group(
                                            {
                                                ordenGiroDetalleTerceroCausacionDescuentoId: [ 0 ],
                                                nombreAportante: [ null, Validators.required ],
                                                valorDescuento: [ null, Validators.required ],
                                                fuente: [ { value: null, disabled: true }, Validators.required ]
                                            }
                                        )
                                    ] )
                                }
                            )
                        )

                    }
                }
                if ( this.getDescuentos( index, jIndex ).dirty === false ) {
                    const nuevosDescuentos = value - this.getDescuentos( index, jIndex ).length;
                    this.getConceptos( index ).controls[ jIndex ].get( 'descuento' ).get( 'numeroDescuentos' ).setValidators( Validators.min( this.getDescuentos( index, jIndex ).length ) );

                    if ( value < this.getDescuentos( index, jIndex ).length ) {
                        this.openDialog( '', '<b>Debe eliminar uno de los registros diligenciados para disminuir el total de los registros requeridos.</b>' );
                        this.getConceptos( index ).controls[ jIndex ].get( 'descuento' ).get( 'numeroDescuentos' ).setValue( this.getDescuentos( index, jIndex ).length );
                        return;
                    }


                    for ( let i = 0; i < nuevosDescuentos; i++ ) {

                        this.getDescuentos( index, jIndex ).push(
                            this.fb.group(
                                {
                                    tipoDescuento: [ null, Validators.required ],
                                    aportantesDescuento: this.fb.array( [
                                        this.fb.group(
                                            {
                                                ordenGiroDetalleTerceroCausacionDescuentoId: [ 0 ],
                                                nombreAportante: [ null, Validators.required ],
                                                valorDescuento: [ null, Validators.required ],
                                                fuente: [ { value: null, disabled: true }, Validators.required ]
                                            }
                                        )
                                    ] )
                                }
                            )
                        )

                    }
                }
            } else {
                this.openDialog( '', '<b>No tiene parametrizados más descuentos para aplicar al pago.</b>' );
            }
        }
    }

    deleteDescuento( index: number, jIndex: number, kIndex: number ) {
        this.openDialogTrueFalse( '', '<b>¿Está seguro de eliminar esta información?</b>' )
        .subscribe(
            value => {
                if ( value === true ) {
                    const codigo: string = this.getDescuentos( index, jIndex ).controls[ kIndex ].get( 'tipoDescuento' ).value;

                    if ( codigo !== null ) {
                        const listaDescuento: Dominio[] = this.getConceptos( index ).controls[ jIndex ].get( 'tipoDescuentoArray' ).value;
                        const descuento = this.listaTipoDescuento.find( descuento => descuento.codigo === codigo );

                        if ( descuento !== undefined ) {
                            listaDescuento.push( descuento );
                            this.getConceptos( index ).controls[ jIndex ].get( 'tipoDescuentoArray' ).setValue( listaDescuento );
                        }
                    }

                    const listIdDescuento = [];

                    this.getAportanteDescuentos( index, jIndex, kIndex ).controls.forEach( aportanteDescuento => {
                        if ( aportanteDescuento.get( 'ordenGiroDetalleTerceroCausacionDescuentoId' ).value !== 0 ) {
                            listIdDescuento.push( aportanteDescuento.get( 'ordenGiroDetalleTerceroCausacionDescuentoId' ).value )
                        }
                    } )

                    this.ordenGiroSvc.deleteOrdenGiroDetalleTerceroCausacionDescuento( listIdDescuento ).subscribe()

                    this.getDescuentos( index, jIndex ).removeAt( kIndex );
                    this.openDialog( '', '<b>La información se ha eliminado correctamente.</b>' );
                    this.getConceptos( index ).controls[ jIndex ].get( 'descuento' ).get( 'numeroDescuentos' ).setValue( this.getDescuentos( index, jIndex ).length );
                }
            }
        )
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

        let aportantesPorDiligenciar: boolean;
        let ordenGiroDetalleDescuentoTecnica = [];
        const ordenGiroDetalleDescuentoTecnicaAportante = [];
        const aportantesDiligenciados = [];
        let totalDescuentoAportante = 0;
        if ( this.ordenGiroDetalle !== undefined ) {
            if ( this.ordenGiroDetalle.ordenGiroDetalleDescuentoTecnica !== undefined ) {
                ordenGiroDetalleDescuentoTecnica = this.ordenGiroDetalle.ordenGiroDetalleDescuentoTecnica.filter( ordenGiroDetalleDescuentoTecnica => ordenGiroDetalleDescuentoTecnica.esPreconstruccion === this.esPreconstruccion );
            }
        }

        if ( ordenGiroDetalleDescuentoTecnica.length > 0 ) {
            this.criterios.controls.forEach( ( criterioControl, indexCriterio ) => {
                this.getConceptos( indexCriterio ).controls.forEach( ( conceptoControl, indexConcepto ) => {
                    this.getAportantes( indexCriterio, indexConcepto ).controls.forEach( aportanteControl => {
                        if ( aportanteControl.get( 'nombreAportante' ).value !== null ) {
                            aportantesDiligenciados.push( aportanteControl.value )
                        }
                    } )
                } )
            } );

            ordenGiroDetalleDescuentoTecnica.forEach( descuentoTecnica => {
                descuentoTecnica.ordenGiroDetalleDescuentoTecnicaAportante.forEach( descuentoAportanteTecnica => ordenGiroDetalleDescuentoTecnicaAportante.push( descuentoAportanteTecnica ) )
            } )

            if ( ordenGiroDetalleDescuentoTecnicaAportante.length > 0 ) {
                ordenGiroDetalleDescuentoTecnicaAportante.forEach( descuentoAportanteTecnica => {
                    const aportante = aportantesDiligenciados.find( aportante => descuentoAportanteTecnica.aportanteId === aportante.nombreAportante.cofinanciacionAportanteId )

                    if ( aportante === undefined ) {
                        aportantesPorDiligenciar = true;
                    }
                } )
            }
        }

        if ( aportantesPorDiligenciar === true ) {
            this.openDialog( '', `<b>Debe diligenciar como minimo los aportantes seleccionados en el acordeon descuentos de dirección tecnica - ${ this.esPreconstruccion === true ? 'Fase 1' : 'Fase 2' }.</b>` );
            return
        }
        const getOrdenGiroDetalleTerceroCausacion = ( ) => {
            const listaTerceroCausacion = [];
            this.criterios.controls.forEach( ( criterioControl, indexCriterio ) => {
                this.getConceptos( indexCriterio ).controls.forEach( ( conceptoControl, indexConcepto ) => {
                    if ( this.getDescuentos( indexCriterio, indexConcepto ).length > 0 && conceptoControl.get( 'descuento' ).get( 'aplicaDescuentos' ).value === true ) {
                        this.getDescuentos( indexCriterio, indexConcepto ).controls.forEach( ( descuentoControl, indexDescuento ) => {
                            this.getAportanteDescuentos( indexCriterio, indexConcepto, indexDescuento ).controls.forEach( aportanteDescuento => {
                                this.valorNetoGiro -= aportanteDescuento.get( 'valorDescuento' ).value
                            } )
                        } )
                    }
                } )
            } );

            this.criterios.controls.forEach( ( criterioControl, indexCriterio ) => {
                let terceroCausacion: any;
                this.getConceptos( indexCriterio ).controls.forEach( ( conceptoControl, indexConcepto ) => {
                  const ordenGiroDetalleTerceroCausacionAportante = [];

                    terceroCausacion = {
                        contratacionProyectoId: this.solicitudPagoFase.contratacionProyectoId,
                        ordenGiroDetalleTerceroCausacionId: conceptoControl.get( 'ordenGiroDetalleTerceroCausacionId' ).value,
                        valorNetoGiro: this.valorNetoGiro,
                        ordenGiroDetalleId: this.ordenGiroDetalleId,
                        esPreconstruccion: this.esPreconstruccion,
                        conceptoPagoCriterio: criterioControl.get( 'tipoCriterioCodigo' ).value,
                        tipoPagoCodigo: criterioControl.get( 'tipoPagoCodigo' ).value,
                        tieneDescuento: conceptoControl.get( 'descuento' ).get( 'aplicaDescuentos' ).value,
                        ordenGiroDetalleTerceroCausacionDescuento: [],
                        ordenGiroDetalleTerceroCausacionAportante: [],
                        conceptoCodigo: conceptoControl.get( 'conceptoPagoCriterio' ).value,
                    }
                    this.getAportantes( indexCriterio, indexConcepto ).controls.forEach( aportanteControl => {
                      if ( aportanteControl.dirty === true ) {
                          if(aportanteControl.get( 'nombreAportante' ).value != null){
                           ordenGiroDetalleTerceroCausacionAportante.push(
                                {
                                    ordenGiroDetalleTerceroCausacionId: conceptoControl.get( 'ordenGiroDetalleTerceroCausacionId' ).value,
                                    ordenGiroDetalleTerceroCausacionAportanteId: aportanteControl.get( 'ordenGiroDetalleTerceroCausacionAportanteId' ).value,
                                    fuenteRecursoCodigo: aportanteControl.get( 'fuenteRecursos' ).value?.codigo,
                                    fuenteFinanciacionId: aportanteControl.get( 'fuenteRecursos' ).value?.fuenteFinanciacionId,
                                    aportanteId: aportanteControl.get( 'nombreAportante' ).value?.cofinanciacionAportanteId,
                                    conceptoPagoCodigo: conceptoControl.get( 'conceptoPagoCriterio' ).value,
                                    valorDescuento: aportanteControl.get( 'valorDescuento' ).value <= aportanteControl.get( 'valorDescuentoTecnica' ).value ? aportanteControl.get( 'valorDescuento' ).value : aportanteControl.get( 'valorDescuento' ).value - aportanteControl.get( 'valorDescuentoTecnica' ).value
                                }
                            )
                          }
                        }
                    } )

                    if ( this.getDescuentos( indexCriterio, indexConcepto ).length > 0 && conceptoControl.get( 'descuento' ).get( 'aplicaDescuentos' ).value === true ) {
                        this.getDescuentos( indexCriterio, indexConcepto ).controls.forEach( ( descuentoControl, indexDescuento ) => {

                            this.getAportanteDescuentos( indexCriterio, indexConcepto, indexDescuento ).controls.forEach( aportanteDescuento => {
                                terceroCausacion.ordenGiroDetalleTerceroCausacionDescuento.push(
                                    {
                                        ordenGiroDetalleTerceroCausacionId: conceptoControl.get( 'ordenGiroDetalleTerceroCausacionId' ).value,
                                        ordenGiroDetalleTerceroCausacionDescuentoId: aportanteDescuento.get( 'ordenGiroDetalleTerceroCausacionDescuentoId' ).value,
                                        tipoDescuentoCodigo: descuentoControl.get( 'tipoDescuento' ).value,
                                        aportanteId: aportanteDescuento.get( 'nombreAportante' ).value.cofinanciacionAportanteId,
                                        fuenteRecursosCodigo: aportanteDescuento.get( 'fuente' ).value.codigo,
                                        fuenteFinanciacionId: aportanteDescuento.get( 'fuente' ).value.fuenteFinanciacionId,
                                        valorDescuento: aportanteDescuento.get( 'valorDescuento' ).value
                                    }
                                )
                            } )
                        } )
                    }
                  terceroCausacion.ordenGiroDetalleTerceroCausacionAportante = ordenGiroDetalleTerceroCausacionAportante;
                  listaTerceroCausacion.push( terceroCausacion );
                } )
            } );
            return listaTerceroCausacion.length > 0 ? listaTerceroCausacion : null;
        }

        const pOrdenGiro = {
            solicitudPagoId: this.solicitudPago.solicitudPagoId,
            ordenGiroId: this.ordenGiroId,
            valorNetoGiro: this.valorNetoGiro,
            ordenGiroDetalle: [
                {
                    ordenGiroId: this.ordenGiroId,
                    ordenGiroDetalleId: this.ordenGiroDetalleId,
                    ordenGiroDetalleTerceroCausacion: getOrdenGiroDetalleTerceroCausacion()
                }
            ]
        }
        this.ordenGiroSvc.createEditOrdenGiro( pOrdenGiro )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );

                    if ( this.criterios.controls[ index ].get( 'obsVerificar' ).value !== null ) {
                        const obsVerificar = this.criterios.controls[ index ].get( 'obsVerificar' ).value;
                        obsVerificar.archivada = !obsVerificar.archivada;
                        this.obsOrdenGiro.createEditSpinOrderObservations( obsVerificar )
                            .subscribe();
                    }
                    if ( this.criterios.controls[ index ].get( 'obsAprobar' ).value !== null ) {
                        const obsAprobar = this.criterios.controls[ index ].get( 'obsAprobar' ).value;
                        obsAprobar.archivada = !obsAprobar.archivada;
                        this.obsOrdenGiro.createEditSpinOrderObservations( obsAprobar )
                            .subscribe();
                    }
                    if ( this.criterios.controls[ index ].get( 'obsTramitar' ).value !== null ) {
                        const obsTramitar = this.criterios.controls[ index ].get( 'obsTramitar' ).value;
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
