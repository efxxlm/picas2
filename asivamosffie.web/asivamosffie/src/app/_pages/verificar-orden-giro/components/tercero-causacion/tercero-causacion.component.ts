import { MatDialog } from '@angular/material/dialog';
import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { FormGroup, Validators, FormBuilder, FormArray } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { Dominio, CommonService } from 'src/app/core/_services/common/common.service';
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
    @Input() esPreconstruccion: boolean;
    @Input() solicitudPagoFase: any;
    @Input() contratacionProyectoId: number;
    @Output() estadoSemaforo = new EventEmitter<string>();
    listaMenu: ListaMenu = ListaMenuId;
    tipoObservaciones: TipoObservaciones = TipoObservacionesCodigo;
    totalEnProceso = 0;
    totalCompleto = 0;
    tipoDescuentoArray: Dominio[] = [];
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
    valorNetoGiroTercero = 0;
    ordenGiroId = 0;
    ordenGiroDetalleId = 0;
    dataHistorial: any[] = [];
    listaTipoDescuento: Dominio[] = [];
    solicitudesPagoFase: any[];
    tieneAmortizacion: boolean = false;
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

    getAportanteDescuentos( index: number, jIndex: number, kIndex: number ) {
        return this.getDescuentos( index, jIndex ).controls[ kIndex ].get( 'aportantesDescuento' ) as FormArray;
    }

    constructor(
        private fb: FormBuilder,
        private dialog: MatDialog,
        private routes: Router,
        private registrarPagosSvc: RegistrarRequisitosPagoService,
        private ordenGiroSvc: OrdenPagoService,
        private commonSvc: CommonService,
        private obsOrdenGiro: ObservacionesOrdenGiroService )
    {
        this.commonSvc.listaDescuentosOrdenGiro()
            .subscribe( listaDescuentosOrdenGiro => this.tipoDescuentoArray = listaDescuentosOrdenGiro );
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
      this.solicitudPago.ordenGiro?.ordenGiroDetalle?.forEach( ordengiro => {
          ordengiro.ordenGiroDetalleTerceroCausacion.filter(r => r.contratacionProyectoId == this.contratacionProyectoId).forEach(tercero => {
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

                  //const dataAportantes = await this.ordenGiroSvc.getAportantes( this.solicitudPago );
                  let dataAportantes = await this.ordenGiroSvc.getAportantesNew( this.solicitudPago );
                  console.log(dataAportantes);
                  for (let i = 0; i < dataAportantes.listaTipoAportante.length; i++) {
                      const element = dataAportantes.listaTipoAportante[i];
                      const element2 = this.solicitudPago.tablaInformacionFuenteRecursos[i];
                      element.aportanteId = element2.cofinanciacionAportanteId
                  }

                  if (this.solicitudPago.valorXProyectoXFaseXAportanteXConcepto.length > 0) {
                    const conceptosxContratacionProyecto = this.solicitudPago.valorXProyectoXFaseXAportanteXConcepto.filter((r: { contratacionProyectoId: number; }) => r.contratacionProyectoId === this.contratacionProyectoId);
                    const aportantesxContratacionProyecto = [];
                    if(conceptosxContratacionProyecto.length > 0){
                      conceptosxContratacionProyecto.forEach((cp: { aportanteId: any; }) => {
                        if(aportantesxContratacionProyecto.indexOf(cp.aportanteId) === -1) {
                          aportantesxContratacionProyecto.push(cp.aportanteId);
                        }
                      });
                    }
                    const listaNombreAportante = [];
                    const  listaTipoAportante = [];

                    if ( dataAportantes.listaNombreAportante.length > 0  ) {
                      dataAportantes.listaNombreAportante.forEach((r: any, index: number) => {
                        let position = aportantesxContratacionProyecto.indexOf(r.cofinanciacionAportanteId);
                        if(position !== -1) {
                            listaNombreAportante.push(dataAportantes.listaNombreAportante[index]);
                            const lta = dataAportantes.listaTipoAportante[index];
                            if(listaTipoAportante.findIndex(r => r.dominioId == lta.dominioId && r.tipoDominioId == lta.tipoDominioId && r.codigo == lta.codigo) === -1){
                              listaTipoAportante.push(lta);
                            }
                        }
                      });
                    }
                    if(listaNombreAportante.length > 0){
                      dataAportantes = {
                        listaNombreAportante: listaNombreAportante,
                        listaTipoAportante: listaTipoAportante
                      };
                    }

                  }

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
                                                                  nombreAportante.valorActual = Number( aportante.valor.split( '.' ).join( '' ) )
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
                      this.cantidadAportantes = dataAportantes.listaNombreAportante.length;
                      // Get data del guardado de tercero de causacion
                      for ( const criterio of listCriterios ) {
                        let totalCompleto  = 0;
                        let totalIncompleto = 0;
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
                                      valorDescuentoTecnica: [ null ],
                                      cuentaBancariaId: [ null ],
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
                                              valorDescuentoTecnica: [ null ],
                                              cuentaBancariaId: [ aportante.cuentaBancariaId, Validators.required ],
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
                                          valorDescuentoTecnica: [ null ],
                                          cuentaBancariaId: [ null ]
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
                                usoCodigo: [ usoByConcepto[0]?.tipoUsoCodigo ],
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
                                  usoCodigo: [ usoByConcepto[0]?.tipoUsoCodigo ],
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
                                              valorDescuentoTecnica: [ null ],
                                              cuentaBancariaId: [ null ]
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
                      console.log(terceroCausacionxCriterio);
                      if(terceroCausacionxCriterio?.length > 0){
                        const totalTerceroCriterio = terceroCausacionxCriterio?.length;
                        terceroCausacionxCriterio.forEach((element: any) => {
                          let registroCompleto = true;
                          element.ordenGiroDetalleTerceroCausacionAportante.forEach((terceroCausacionAportante: any) => {
                            if (
                                (terceroCausacionAportante.fuenteRecursoCodigo == "" || terceroCausacionAportante.fuenteRecursoCodigo == null)
                                || terceroCausacionAportante.aportanteId == 0
                                || (terceroCausacionAportante.conceptoPagoCodigo == "" || terceroCausacionAportante.conceptoPagoCodigo == null)
                                || terceroCausacionAportante.valorDescuento == 0
                                || terceroCausacionAportante.fuenteFinanciacionId == 0
                              ){
                                registroCompleto = false;
                              }
                          });
                          element.ordenGiroDetalleTerceroCausacionDescuento.forEach((terceroCausacionDescuento: any) => {
                            if (terceroCausacionDescuento.registroCompleto == false){
                                registroCompleto = false;
                            }
                          });
                          if(registroCompleto === true){
                            totalCompleto++;
                          }else{
                            totalIncompleto++;
                          }

                        });
                        let estadoSemaforo = 'en-proceso';

                        if(totalCompleto == totalTerceroCriterio){
                          estadoSemaforo = 'completo';
                        }else if(totalIncompleto == totalTerceroCriterio){
                          estadoSemaforo = 'sin-diligenciar';
                        }else{
                          estadoSemaforo = 'en-proceso';
                        }
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
                  this.solicitudesPagoFase = this.solicitudPago?.solicitudPagoRegistrarSolicitudPago[0]?.solicitudPagoFase;

                  if(this.solicitudesPagoFase.length > 0){
                    let solicitudPagoFaseTmp = this.solicitudesPagoFase.find(r => r.contratacionProyectoId == this.contratacionProyectoId);
                    if(solicitudPagoFaseTmp != null)
                      this.solicitudPagoFase = solicitudPagoFaseTmp;
                  }

                  if(this.solicitudPagoFase?.solicitudPagoFaseAmortizacion.length > 0)
                      this.tieneAmortizacion = true;

              }
          );
  }

    getTerceroCausacionOld() {
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

        this.solicitudPago.ordenGiro.ordenGiroDetalle.forEach( ordengiro => {
          ordengiro.ordenGiroDetalleTerceroCausacion.filter(r => r.contratacionProyectoId == this.contratacionProyectoId).forEach(tercero => {
              this.valorNetoGiroTercero += tercero.valorFacturadoConcepto
              tercero.ordenGiroDetalleTerceroCausacionDescuento.forEach(descuento => {
                  this.valorNetoGiroTercero -= descuento.valorDescuento
              });
          });
        });

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
                    const dataAportantes = await this.ordenGiroSvc.getAportantesNew( this.solicitudPago );
                        // Get cantidad de aportantes para limitar cuantos aportantes se pueden agregar en el formulario
                        this.cantidadAportantes = dataAportantes.listaTipoAportante.length;
                        // Get data del guardado de tercero de causacion
                        for ( const criterio of listCriterios ) {
                            const listDescuento = [ ...this.tipoDescuentoArray ];

                            if ( this.ordenGiroDetalleTerceroCausacion !== undefined ) {
                                const terceroCausacion = this.ordenGiroDetalleTerceroCausacion.find( tercero => tercero.conceptoPagoCriterio === criterio.tipoCriterioCodigo && tercero.esPreconstruccion === this.esPreconstruccion && tercero.contratacionProyectoId === this.solicitudPagoFase.contratacionProyectoId );
                                const listaDescuentos = [];
                                const listaAportantes = [];
                                const conceptosDePago = [];

                                if ( terceroCausacion !== undefined ) {
                                    if ( terceroCausacion.ordenGiroDetalleTerceroCausacionDescuento.length > 0 ) {
                                        for ( const descuento of listDescuento ) {
                                            const ordenGiroDetalleTerceroCausacionDescuento: any[] = terceroCausacion.ordenGiroDetalleTerceroCausacionDescuento.filter( ordenGiroDetalleTerceroCausacionDescuento => ordenGiroDetalleTerceroCausacionDescuento.tipoDescuentoCodigo === descuento.codigo );
                                            const listaAportanteDescuentos = [];

                                            if ( ordenGiroDetalleTerceroCausacionDescuento.length > 0 ) {
                                                for ( const terceroCausacionDescuento of ordenGiroDetalleTerceroCausacionDescuento ) {
                                                    const nombreAportante = dataAportantes.listaNombreAportante.find( nombre => nombre.cofinanciacionAportanteId === terceroCausacionDescuento.aportanteId );
                                                    let listaFuenteRecursos: any[] = await this.ordenGiroSvc.getFuentesDeRecursosPorAportanteId( nombreAportante.cofinanciacionAportanteId ).toPromise();
                                                    const fuente = listaFuenteRecursos.find( fuente => fuente.codigo === terceroCausacionDescuento.fuenteRecursosCodigo );

                                                    listaAportanteDescuentos.push(
                                                        this.fb.group(
                                                            {
                                                                ordenGiroDetalleTerceroCausacionDescuentoId: [ terceroCausacionDescuento.ordenGiroDetalleTerceroCausacionDescuentoId ],
                                                                nombreAportante: [ nombreAportante !== undefined ? nombreAportante : null, Validators.required ],
                                                                valorDescuento: [ terceroCausacionDescuento.valorDescuento, Validators.required ],
                                                                fuente: [ { value: fuente !== undefined ? fuente : null, disabled: true }, Validators.required ]
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
                                    }
                                    // Get lista de aportantes
                                    // Get cantidad de aportantes para limitar cuantos aportantes se pueden agregar en el formulario
                                    this.cantidadAportantes = dataAportantes.listaTipoAportante.length;
                                    console.log("dataAportantes: ",dataAportantes);
                                    if ( terceroCausacion.ordenGiroDetalleTerceroCausacionAportante.length > 0 ) {
                                        for ( const aportante of terceroCausacion.ordenGiroDetalleTerceroCausacionAportante ) {
                                            const nombreAportante = dataAportantes.listaNombreAportante.find( nombre => nombre.cofinanciacionAportanteId === aportante.aportanteId );
                                            if(nombreAportante != null && nombreAportante != undefined){
                                              const tipoAportante = dataAportantes.listaTipoAportante.find( tipo => tipo.dominioId === nombreAportante?.tipoAportanteId );
                                              let listaFuenteRecursos: any[] = await this.ordenGiroSvc.getFuentesDeRecursosPorAportanteId( nombreAportante?.cofinanciacionAportanteId ).toPromise();
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
                                    }

                                    // Set formulario criterios
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
                                }
                            }
                        }

                        if ( this.totalEnProceso > 0 && this.totalEnProceso === this.criterios.length ) {
                            this.estadoSemaforo.emit( 'en-proceso' );
                        }
                        if ( this.totalCompleto > 0 && this.totalCompleto < this.criterios.length ) {
                            this.estadoSemaforo.emit( 'en-proceso' );
                        }
                        if ( this.totalCompleto > 0 && this.totalCompleto === this.criterios.length ) {
                            this.estadoSemaforo.emit( 'completo' );
                        }
                }
            );
            console.log(this.criterios.controls);
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
