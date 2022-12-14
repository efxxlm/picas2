import { Router, ActivatedRoute } from '@angular/router';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, Validators, FormGroup, FormArray } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { ObservacionesMultiplesCuService } from 'src/app/core/_services/observacionesMultiplesCu/observaciones-multiples-cu.service';
import { RegistrarRequisitosPagoService } from 'src/app/core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { TiposDeFase } from 'src/app/_interfaces/solicitud-pago.interface';

@Component({
  selector: 'app-obs-criterio-pagos',
  templateUrl: './obs-criterio-pagos.component.html',
  styleUrls: ['./obs-criterio-pagos.component.scss']
})
export class ObsCriterioPagosComponent implements OnInit {

    @Input() solicitudPago: any;
    @Input() contrato: any;
    @Input() contratacionProyectoId: number;
    @Input() solicitudPagoCargarFormaPago: any;
    @Input() tieneObservacion: boolean;
    @Input() listaMenusId: any;
    @Input() criteriosPagoFacturaCodigo: string;
    @Input() tieneObservacionOrdenGiro: boolean;
    @Input() esVerDetalle = false;
    @Input() faseCodigo: string;
    @Input() num: number;
    @Output() semaforoObservacion = new EventEmitter<boolean>();
    @Output() emitAnticipo = new EventEmitter<boolean>();
    criteriosSeleccionadosArray: Dominio[] = [];
    solicitudPagoRegistrarSolicitudPago: any;
    solicitudPagoRegistrarSolicitudPagoId = 0;
    registroCompletoCriterio = false;
    solicitudPagoFase: any;
    esAutorizar: boolean;
    observacion: any;
    fasesContrato = TiposDeFase;
    solicitudPagoObservacionId = 0;
    btnBoolean = false;
    montoMaximoPendiente: any = undefined;
    esPreconstruccion = true;
    manejoAnticipoRequiere = false;
    addressForm = this.fb.group({
        criterioPago: [ null, Validators.required ],
        criterios: this.fb.array( [] )
    });
    criteriosArray: { codigo: string, nombre: string, porcentaje: number }[] = [];
    estaEditando = false;
    forma_pago_codigo: string;
    usosParaElConceoto: any;

    get criterios() {
        return this.addressForm.get( 'criterios' ) as FormArray;
    }

    getConceptos( index: number ) {
        return this.criterios.controls[ index ].get( 'conceptos' ) as FormArray;
    }

    constructor(
        private fb: FormBuilder,
        private dialog: MatDialog,
        private routes: Router,
        private obsMultipleSvc: ObservacionesMultiplesCuService,
        private registrarPagosSvc: RegistrarRequisitosPagoService )
    {}

    ngOnInit(): void {
        this.getCriterios();
    }

    async getCriterios() {
        // Verificar la fase seleccionada en el proyecto
        if ( this.faseCodigo === this.fasesContrato.construccion ) {
            this.esPreconstruccion = false;
        }
        // Se obtiene la forma pago codigo dependiendo la fase seleccionada
        const FORMA_PAGO_CODIGO = this.esPreconstruccion === true ? this.solicitudPagoCargarFormaPago.fasePreConstruccionFormaPagoCodigo : this.solicitudPagoCargarFormaPago.faseConstruccionFormaPagoCodigo
        this.forma_pago_codigo = FORMA_PAGO_CODIGO;

        let LISTA_CRITERIOS_FORMA_PAGO = await this.registrarPagosSvc.getCriterioByFormaPagoCodigo( FORMA_PAGO_CODIGO ).toPromise()
        let seDiligencioAnticipo: boolean;
        let listaSolicitudesPago = [];
        let criterioAnticipo = null;

        //const montoMaximoPendiente = await this.registrarPagosSvc.getMontoMaximoMontoPendiente( this.solicitudPago.solicitudPagoId, FORMA_PAGO_CODIGO, this.esPreconstruccion === true ? 'True' : 'False', this.contratacionProyectoId ).toPromise();
        if ( this.contrato.contratoConstruccion.length === 1 ) this.manejoAnticipoRequiere = this.contrato.contratoConstruccion[0].manejoAnticipoRequiere;
        if ( this.contrato.contratoConstruccion.length > 1 ) this.manejoAnticipoRequiere = this.contrato.contratoConstruccion[this.num].manejoAnticipoRequiere;

       // this.montoMaximoPendiente = montoMaximoPendiente

        criterioAnticipo = LISTA_CRITERIOS_FORMA_PAGO //.find( value => value.nombre === 'Anticipo' )
        if ( this.manejoAnticipoRequiere === false || undefined ) {
            LISTA_CRITERIOS_FORMA_PAGO = LISTA_CRITERIOS_FORMA_PAGO.filter( value => value.nombre !== 'Anticipo' )
        }

        if ( this.solicitudPago.solicitudPagoRegistrarSolicitudPago !== undefined && this.solicitudPago.solicitudPagoRegistrarSolicitudPago.length > 0 ) {
            this.solicitudPagoRegistrarSolicitudPago = this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0]

            if ( this.solicitudPagoRegistrarSolicitudPago.solicitudPagoFase !== undefined ) {
                if ( this.solicitudPagoRegistrarSolicitudPago.solicitudPagoFase.length > 0 ) {
                    const fase = this.solicitudPagoRegistrarSolicitudPago.solicitudPagoFase.find( solicitudPagoFase => solicitudPagoFase.esPreconstruccion === this.esPreconstruccion && solicitudPagoFase.contratacionProyectoId === this.contratacionProyectoId )

                    if ( fase !== undefined ) {
                        if ( LISTA_CRITERIOS_FORMA_PAGO.length > 0 && this.esVerDetalle === false ) {
                            if ( fase.solicitudPagoFaseCriterio.length > 0 ) {
                                // tipoCriterioCodigo
                                if ( criterioAnticipo ) {
                                    const anticipoFind = fase.solicitudPagoFaseCriterio.find( value => value.tipoCriterioCodigo === criterioAnticipo.codigo )

                                    if ( anticipoFind !== undefined ) {
                                        this.emitAnticipo.emit( true )
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        if ( this.contrato.solicitudPago.length > 0 ) {
            listaSolicitudesPago = this.contrato.solicitudPago.filter( value => value.solicitudPagoId !== this.solicitudPago.solicitudPagoId )
        }

        if ( listaSolicitudesPago.length > 0 ) {
            listaSolicitudesPago.forEach( solicitud => {
                if ( solicitud.solicitudPagoRegistrarSolicitudPago !== undefined && solicitud.solicitudPagoRegistrarSolicitudPago.length > 0 ) {
                    const solicitudPagoRegistrarSolicitudPago = solicitud.solicitudPagoRegistrarSolicitudPago[ 0 ]

                    if ( solicitudPagoRegistrarSolicitudPago !== undefined ) {
                        solicitudPagoRegistrarSolicitudPago.solicitudPagoFase.forEach( fase => {
                            if ( fase.solicitudPagoFaseCriterio.length > 0 ) {
                                const faseCriterioFind = fase.solicitudPagoFaseCriterio.find( faseCriterio => faseCriterio.tipoCriterioCodigo === criterioAnticipo.codigo )

                                if ( faseCriterioFind !== undefined ) {
                                    seDiligencioAnticipo = true
                                }
                            }
                        } )
                    }
                }
            } )
        }

        if ( seDiligencioAnticipo === true ) {
            LISTA_CRITERIOS_FORMA_PAGO = LISTA_CRITERIOS_FORMA_PAGO.filter( value => value.codigo !== criterioAnticipo.codigo )
        }

        this.criteriosArray = LISTA_CRITERIOS_FORMA_PAGO;


        if ( this.solicitudPago.solicitudPagoRegistrarSolicitudPago !== undefined && this.solicitudPago.solicitudPagoRegistrarSolicitudPago.length > 0 ) {
            this.solicitudPagoRegistrarSolicitudPago = this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0]
            this.solicitudPagoRegistrarSolicitudPagoId = this.solicitudPagoRegistrarSolicitudPago.solicitudPagoRegistrarSolicitudPagoId

            if ( this.solicitudPagoRegistrarSolicitudPago.solicitudPagoFase !== undefined ) {
                if ( this.solicitudPagoRegistrarSolicitudPago.solicitudPagoFase.length > 0 ) {
                    this.solicitudPagoFase = this.solicitudPagoRegistrarSolicitudPago.solicitudPagoFase.find( solicitudPagoFase => solicitudPagoFase.esPreconstruccion === this.esPreconstruccion && solicitudPagoFase.contratacionProyectoId === this.contratacionProyectoId )

                    if ( this.solicitudPagoFase !== undefined ) {
                        if ( LISTA_CRITERIOS_FORMA_PAGO.length > 0 && this.esVerDetalle === false ) {
                            for ( const criterioValue of LISTA_CRITERIOS_FORMA_PAGO ) {
                                const nombre = criterioValue.nombre.split( ' ' );
                                const porcentaje = nombre[ nombre.length - 1 ].replace( '%', '' );
                                const porcentajeCriterio = Number( porcentaje ) / 100;

                                criterioValue.porcentaje = porcentajeCriterio;
                                criterioValue[ 'criterios' ] = [];
                            }

                            const listSolicitudPago: any[] = this.contrato.solicitudPago;

                            if ( listSolicitudPago.length > 1 ) {
                                const solicitudPagoFaseCriterio = [];
                                for ( const solicitud of listSolicitudPago ) {
                                    if ( solicitud.solicitudPagoId !== this.solicitudPago.solicitudPagoId ) {
                                        if ( solicitud.solicitudPagoRegistrarSolicitudPago[ 0 ] !== undefined ) {
                                            const faseConstruccion = solicitud.solicitudPagoRegistrarSolicitudPago[ 0 ].solicitudPagoFase.find( solicitudPagoFase => solicitudPagoFase.esPreconstruccion === true );

                                            if ( faseConstruccion !== undefined ) {
                                                if ( faseConstruccion.solicitudPagoFaseCriterio.length > 0 ) {
                                                    faseConstruccion.solicitudPagoFaseCriterio.forEach( solicitudPagoFaseCriterioValue => {
                                                        solicitudPagoFaseCriterio.push( solicitudPagoFaseCriterioValue );
                                                    } )
                                                }
                                            }
                                        }
                                    }
                                }

                                if ( solicitudPagoFaseCriterio.length > 0 ) {
                                    solicitudPagoFaseCriterio.forEach( solicitudPagoFaseCriterio => {
                                        const criterioIndex = LISTA_CRITERIOS_FORMA_PAGO.findIndex( criterioValue => criterioValue.codigo === solicitudPagoFaseCriterio.tipoCriterioCodigo );

                                        if ( criterioIndex !== -1 ) {
                                            LISTA_CRITERIOS_FORMA_PAGO[ criterioIndex ][ 'criterios' ].push( solicitudPagoFaseCriterio );
                                        }
                                    } )

                                    LISTA_CRITERIOS_FORMA_PAGO.forEach( ( criterioValue, index ) => {
                                        let totalCriterio = 0;
                                        if ( criterioValue[ 'criterios' ].length > 0 ) {
                                            criterioValue[ 'criterios' ].forEach( criterio => {
                                                totalCriterio += criterio.valorFacturado;
                                            } )
                                        }

                                        if ( totalCriterio > 0 ) {
                                            const restanteCriterio = ( ( this.montoMaximoPendiente * criterioValue.porcentaje ) - totalCriterio );

                                            if ( totalCriterio === ( this.montoMaximoPendiente * criterioValue.porcentaje ) ) {
                                                LISTA_CRITERIOS_FORMA_PAGO.splice( index, 1 );
                                            } else {
                                                criterioValue.porcentaje = ( restanteCriterio / this.montoMaximoPendiente );
                                            }
                                        }
                                    } )

                                    LISTA_CRITERIOS_FORMA_PAGO.forEach( criterioValue => {
                                        delete criterioValue[ 'criterios' ];
                                    } )
                                }
                            }
                        }

                        if ( this.solicitudPagoFase.solicitudPagoFaseCriterio.length > 0 ) {
                            this.estaEditando = true;
                            this.addressForm.markAllAsTouched();
                            this.criterios.markAllAsTouched();

                            for ( const criterio of this.solicitudPagoFase.solicitudPagoFaseCriterio ) {
                                // GET Criterio seleccionado
                                const criterioSeleccionado = LISTA_CRITERIOS_FORMA_PAGO.filter( value => value.codigo === criterio.tipoCriterioCodigo );
                                this.criteriosSeleccionadosArray.push( criterioSeleccionado[0] );
                                // GET tipos de pago
                                const tiposDePago = await this.registrarPagosSvc.getTipoPagoByCriterioCodigo( criterio.tipoCriterioCodigo );
                                const tipoDePago = tiposDePago.filter( value => value.codigo === criterio.tipoPagoCodigo );
                                // GET conceptos de pago
                                const conceptosDePago = await this.registrarPagosSvc.getConceptoPagoCriterioCodigoByTipoPagoCodigo( criterio.tipoPagoCodigo );
                                let conceptoDePagoArray = [];
                                const conceptosDePagoSeleccionados = [];
                                // Get conceptos de pago
                                if ( criterio.solicitudPagoFaseCriterioConceptoPago.length > 0 ) {
                                    criterio.solicitudPagoFaseCriterioConceptoPago.forEach( solicitudPagoFaseCriterioConceptoPago => {
                                        const conceptoFind = conceptosDePago.find( concepto => concepto.codigo === solicitudPagoFaseCriterioConceptoPago.conceptoPagoCriterio );

                                        if ( conceptoFind !== undefined ) {
                                          this.registrarPagosSvc.getMontoMaximoMontoPendiente( this.solicitudPago.solicitudPagoId, FORMA_PAGO_CODIGO, this.esPreconstruccion === true ? 'True' : 'False', this.contratacionProyectoId ,criterio?.tipoCriterioCodigo, conceptoFind?.codigo )
                                            .subscribe(
                                                async response => {
                                                  conceptosDePagoSeleccionados.push( conceptoFind );
                                                  await this.getvaluesConceptoPagoCodigo(conceptosDePagoSeleccionados)
                                                  conceptoDePagoArray.push(
                                                      this.fb.group(
                                                          {
                                                              solicitudPagoFaseCriterioConceptoPagoId: [ solicitudPagoFaseCriterioConceptoPago.solicitudPagoFaseCriterioConceptoPagoId ],
                                                              solicitudPagoFaseCriterioId: [ criterio.solicitudPagoFaseCriterioId ],
                                                              conceptoPagoCriterioNombre: [ conceptoFind.nombre ],
                                                              usoCodigo: [ solicitudPagoFaseCriterioConceptoPago.usoCodigo ],
                                                              conceptoPagoCriterio: [ solicitudPagoFaseCriterioConceptoPago.conceptoPagoCriterio ],
                                                              valorFacturadoConcepto: [ solicitudPagoFaseCriterioConceptoPago.valorFacturadoConcepto !== undefined ? solicitudPagoFaseCriterioConceptoPago.valorFacturadoConcepto : null ],
                                                              montoMaximo: response?.montoMaximo
                                                          }
                                                      )
                                                  );

                                                  const montoMaximo = this.montoMaximoPendiente !== 0 ? ( this.montoMaximoPendiente * criterioSeleccionado[0].porcentaje ) : 0;
                                                  let existeCriterio = false;
                                                  let i ;
                                                  this.criterios.controls.forEach((c, index) => {
                                                    if(c.get('solicitudPagoFaseId').value == this.solicitudPagoFase.solicitudPagoFaseId && c.get('solicitudPagoFaseCriterioId').value == criterio.solicitudPagoFaseCriterioId){
                                                      existeCriterio = true;
                                                      i = index;
                                                      c.get('conceptos').value.forEach(conceptoOld =>{
                                                        conceptoDePagoArray.push(
                                                          this.fb.group(
                                                            {
                                                                solicitudPagoFaseCriterioConceptoPagoId: [ conceptoOld?.solicitudPagoFaseCriterioConceptoPagoId ],
                                                                solicitudPagoFaseCriterioId: [ conceptoOld?.solicitudPagoFaseCriterioId ],
                                                                conceptoPagoCriterioNombre: [ conceptoOld?.conceptoPagoCriterioNombre ],
                                                                usoCodigo: [ conceptoOld?.usoCodigo ],
                                                                conceptoPagoCriterio: [ conceptoOld?.conceptoPagoCriterio ],
                                                                valorFacturadoConcepto: [ conceptoOld?.valorFacturadoConcepto ],
                                                                montoMaximo: conceptoOld?.montoMaximo

                                                            }
                                                          )
                                                        )
                                                      });
                                                    }
                                                  });
                                                  if(existeCriterio){
                                                    this.criterios.removeAt(i);
                                                  }

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
                                                  conceptoDePagoArray = []
                                                }
                                            );
                                        }
                                    } );
                                }


                            }

                            if ( this.esVerDetalle === false ) {
                                // Get observacion CU autorizar solicitud de pago 4.1.9
                                this.obsMultipleSvc.getObservacionSolicitudPagoByMenuIdAndSolicitudPagoId(
                                    this.listaMenusId?.autorizarSolicitudPagoId,
                                    this.solicitudPago.solicitudPagoId,
                                    this.solicitudPagoFase.solicitudPagoFaseCriterio[0].solicitudPagoFaseCriterioId,
                                    this.criteriosPagoFacturaCodigo )
                                    .subscribe(
                                        response => {
                                            const observacion = response.find( obs => obs.archivada === false );

                                            if ( observacion !== undefined ) {
                                                this.esAutorizar = true;
                                                this.observacion = observacion;
                                                if ( this.observacion.tieneObservacion === true ) {
                                                    this.semaforoObservacion.emit( true );
                                                }

                                                this.solicitudPagoObservacionId = observacion.solicitudPagoObservacionId;
                                            }
                                        }
                                    );
                                // Get observacion CU verificar solicitud de pago 4.1.8
                                this.obsMultipleSvc.getObservacionSolicitudPagoByMenuIdAndSolicitudPagoId(
                                    this.listaMenusId?.aprobarSolicitudPagoId,
                                    this.solicitudPago.solicitudPagoId,
                                    this.solicitudPagoFase.solicitudPagoFaseCriterio[0].solicitudPagoFaseCriterioId,
                                    this.criteriosPagoFacturaCodigo )
                                    .subscribe(
                                        response => {
                                            const observacion = response.find( obs => obs.archivada === false );
                                            if ( observacion !== undefined ) {
                                                this.esAutorizar = false;
                                                this.observacion = observacion;

                                                if ( this.observacion.tieneObservacion === true ) {
                                                    this.semaforoObservacion.emit( true );
                                                }

                                                this.solicitudPagoObservacionId = observacion.solicitudPagoObservacionId;
                                            }
                                        }
                                    );
                            }
                        }

                        this.addressForm.get( 'criterioPago' ).setValue( this.criteriosSeleccionadosArray.length > 0 ? this.criteriosSeleccionadosArray : null );
                    }
                }
            }
        }
    }

    /*
    crearFormulario() {
      return this.fb.group({
        fechaCreacion: [ null ],
        tieneObservaciones: [null, Validators.required],
        observaciones:[null, Validators.required],
        criterios: this.fb.array( [] )
      })
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

    openDialog(modalTitle: string, modalText: string) {
        const dialogRef = this.dialog.open(ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText }
        });
    }

    onSubmit() {
        this.estaEditando = true;
        this.addressForm.markAllAsTouched();
        if ( this.addressForm.get( 'tieneObservaciones' ).value !== null && this.addressForm.get( 'tieneObservaciones' ).value === false ) {
            this.addressForm.get( 'observaciones' ).setValue( '' );
        }

        const pSolicitudPagoObservacion = {
            solicitudPagoObservacionId: this.solicitudPagoObservacionId,
            solicitudPagoId: this.solicitudPago.solicitudPagoId,
            observacion: this.addressForm.get( 'observaciones' ).value !== null ? this.addressForm.get( 'observaciones' ).value : this.addressForm.get( 'observaciones' ).value,
            tipoObservacionCodigo: this.criteriosPagoFacturaCodigo,
            menuId: this.aprobarSolicitudPagoId,
            idPadre: this.solicitudPagoFase.solicitudPagoFaseCriterio[0].solicitudPagoFaseCriterioId,
            tieneObservacion: this.addressForm.get( 'tieneObservaciones' ).value !== null ? this.addressForm.get( 'tieneObservaciones' ).value : this.addressForm.get( 'tieneObservaciones' ).value
        };

        console.log( pSolicitudPagoObservacion );
        this.obsMultipleSvc.createUpdateSolicitudPagoObservacion( pSolicitudPagoObservacion )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                        () => this.routes.navigate(
                            [
                                '/verificarSolicitudPago/aprobacionSolicitud',  this.activatedRoute.snapshot.params.idContrato, this.activatedRoute.snapshot.params.idSolicitudPago
                            ]
                        )
                    );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            )
    }
    */

    getvaluesConcepto( conceptos: any[], index: number, criterioCodigo: any ) {
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

                conceptosArray.forEach( async concepto => {
                  const montoMaximoPendienteNew = await this.registrarPagosSvc.getMontoMaximoMontoPendiente( this.solicitudPago.solicitudPagoId, this.forma_pago_codigo, this.esPreconstruccion === true ? 'True' : 'False', this.contratacionProyectoId ,criterioCodigo, concepto.codigo ).toPromise();
                    this.getConceptos( index ).push(
                        this.fb.group(
                            {
                                solicitudPagoFaseCriterioConceptoPagoId: [ 0 ],
                                solicitudPagoFaseCriterioId: [ this.criterios.controls[ index ].get( 'solicitudPagoFaseCriterioId' ).value ],
                                conceptoPagoCriterioNombre: [ concepto.nombre ],
                                conceptoPagoCriterio: [ concepto.codigo ],
                                valorFacturadoConcepto: [ null ],
                                montoMaximo: montoMaximoPendienteNew?.montoMaximo
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

                conceptosArray.forEach( async concepto => {
                  const montoMaximoPendienteNew = await this.registrarPagosSvc.getMontoMaximoMontoPendiente( this.solicitudPago.solicitudPagoId, this.forma_pago_codigo, this.esPreconstruccion === true ? 'True' : 'False', this.contratacionProyectoId ,criterioCodigo, concepto.codigo ).toPromise();
                    this.getConceptos( index ).push(
                        this.fb.group(
                            {
                                solicitudPagoFaseCriterioConceptoPagoId: [ 0 ],
                                solicitudPagoFaseCriterioId: [ this.criterios.controls[ index ].get( 'solicitudPagoFaseCriterioId' ).value ],
                                conceptoPagoCriterioNombre: [ concepto.nombre ],
                                conceptoPagoCriterio: [ concepto.codigo ],
                                valorFacturadoConcepto: [ null ],
                                montoMaximo: montoMaximoPendienteNew?.montoMaximo
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

    getvaluesConceptoPagoCodigo(e) {
      this.usosParaElConceoto = [];
      e.forEach((e: { codigo: any; }) => {
        this.registrarPagosSvc.getUsoByConceptoPagoCodigo( e.codigo )
        .subscribe(response => {
          if(response != null){
            response.forEach((r: { codigo: any; }) => {
              if(!this.usosParaElConceoto?.find(u =>u.codigo == r.codigo)){
                this.usosParaElConceoto.push(r);
              }
            });
          }
        })
      });
  }


    getUsosParaElConceoto(usoCodigo) {
        const nombreUso = this.usosParaElConceoto.find( uso => uso.codigo === usoCodigo)
        return nombreUso?.nombre;
    }

}
