import { CurrencyPipe } from '@angular/common';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { async } from '@angular/core/testing';
import { FormBuilder, Validators, FormArray, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { Dominio } from 'src/app/core/_services/common/common.service';
import { ObservacionesMultiplesCuService } from 'src/app/core/_services/observacionesMultiplesCu/observaciones-multiples-cu.service';
import { RegistrarRequisitosPagoService } from 'src/app/core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { TiposDeFase } from 'src/app/_interfaces/solicitud-pago.interface';

@Component({
  selector: 'app-form-criterios-pago',
  templateUrl: './form-criterios-pago.component.html',
  styleUrls: ['./form-criterios-pago.component.scss']
})
export class FormCriteriosPagoComponent implements OnInit {

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
    @Output() ocultarAmortizacionAnticipo = new EventEmitter<any>();
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
    usosParaElConceoto: any[] = [];
    quitarOpcionAnticipo = true;
    solicitudesPagoFase : any[];

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
        this.criterioPago();
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
                                // const conceptoDePagoArray = [];
                                const conceptosDePagoSeleccionados = [];
                                // Get conceptos de pago
                                if ( criterio.solicitudPagoFaseCriterioConceptoPago.length > 0 ) {
                                    // console.log(criterio.solicitudPagoFaseCriterioConceptoPago);

                                    criterio.solicitudPagoFaseCriterioConceptoPago.forEach( solicitudPagoFaseCriterioConceptoPago => {
                                        // console.log(solicitudPagoFaseCriterioConceptoPago);

                                        const conceptoFind = conceptosDePago.find( concepto => concepto.codigo === solicitudPagoFaseCriterioConceptoPago.conceptoPagoCriterio );
                                        // console.log(conceptoFind);

                                        const usoCodigo =  solicitudPagoFaseCriterioConceptoPago.usoCodigo;

                                        if ( conceptoFind !== undefined ) {
                                        // console.log('usoCodigo: ', usoCodigo)
                                          this.registrarPagosSvc.getMontoMaximoMontoPendiente( this.solicitudPago.solicitudPagoId, FORMA_PAGO_CODIGO, this.esPreconstruccion === true ? 'True' : 'False', this.contratacionProyectoId ,criterio?.tipoCriterioCodigo, conceptoFind?.codigo, usoCodigo )
                                            .subscribe(
                                                async response => {
                                                    // console.log(response)
                                                  const conceptoDePagoArray = [];
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
                                                              montoMaximo: response.montoMaximo
                                                          }
                                                      )
                                                  );

                                                  const montoMaximo = this.montoMaximoPendiente !== 0 ? ( this.montoMaximoPendiente * criterioSeleccionado[0].porcentaje ) : 0;

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
                                                              usoCodigo: [ criterio.solicitudPagoFaseCriterioConceptoPago[0].usoCodigo, Validators.required ],
                                                              conceptos: this.fb.array( conceptoDePagoArray ),
                                                              valorFacturado: [ { value: criterio.valorFacturado !== undefined ? criterio.valorFacturado : null, disabled: true }, Validators.required ]
                                                          }
                                                      )
                                                  );
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
                        if (this.addressForm.get('criterioPago').value) {
                            this.addressForm.get('criterioPago').value.forEach(element => {
                                if(element.codigo === '17'){
                                    this.quitarOpcionAnticipo = false;
                                }
                            });
                        }

                        this.ocultarAmortizacionAnticipo.emit( this.addressForm.get('criterioPago').value );
                    }
                }
            }
        }

        if (this.quitarOpcionAnticipo) {
            for (let i = 0; i < this.contrato.vAmortizacionXproyecto.length; i++) {
                const element = this.contrato.vAmortizacionXproyecto[i];
                if(element.contratacionProyectoId == this.contratacionProyectoId){
                  if(element.tieneAnticipo === true) {
                    console.log(element.tieneAnticipo);
                    // this.addressForm.get('criterioPago').setValue(null);
                    for (let j = 0; j < this.criteriosArray.length; j++) {
                        const element = this.criteriosArray[j];
                        if (element.codigo === '17') this.criteriosArray.splice(j, 1);
                    }
                }
                }
            };
        }

        // this.addressForm.get('criterioPago').valueChanges.subscribe(value => {
        //     console.log(value);
        //     console.log(value.length > 0);
        //     if (value.length > 0) {
        //         value.forEach(element => {
        //             if (element.codigo !== '17'){
        //                 for (let i = 0; i < this.criteriosArray.length; i++) {
        //                     const element = this.criteriosArray[i];
        //                     if (element.codigo === '17') this.criteriosArray.splice(i, 1);
        //                 }
        //             }
        //         });
        //     } else {
        //         this.criteriosArray = LISTA_CRITERIOS_FORMA_PAGO;
        //         console.log(LISTA_CRITERIOS_FORMA_PAGO);
        //         console.log(this.criteriosArray);
        //     }
        // })
    }

    validateNumberKeypress(event: KeyboardEvent) {
        const alphanumeric = /[0-9]/;
        const inputChar = String.fromCharCode(event.charCode);
        return alphanumeric.test(inputChar) ? true : false;
    }

    async getValorTotalConceptos( index: number, jIndex: number, valorConcepto: number ) {
      if(valorConcepto != null){

        let usoByConcepto = await this.registrarPagosSvc.getUsoByConceptoPagoCriterioCodigo( this.getConceptos( index ).controls[ jIndex ].get( 'conceptoPagoCriterio' ).value, this.solicitudPago.contratoId );
        usoByConcepto = usoByConcepto.filter(r => r.contratacionProyectoId == this.contratacionProyectoId);

        let sumaValoresConcepto = 0;
        this.criterios.controls.forEach(criterios => {
            criterios.get('conceptos').value.forEach(concepto => {
                sumaValoresConcepto += concepto.valorFacturadoConcepto
            });
        });


          if(valorConcepto < 0){
            this.getConceptos( index ).controls[ jIndex ].get( 'valorFacturadoConcepto' ).setValue( null );
            return;
          }

          if (
              valorConcepto > this.getConceptos( index ).controls[ jIndex ].get( 'montoMaximo' ).value
          ) {
              this.openDialog( '', `El valor facturado al concepto no puede ser mayor al <b>Monto pendiente por facturar.</b>` );
              this.getConceptos( index ).controls[ jIndex ].get( 'valorFacturadoConcepto' ).setValue( null );
              return;
          }

          if ( usoByConcepto.length > 0 ) {
              let valorTotalUso = 0;
              usoByConcepto.forEach( uso => valorTotalUso += uso.valorUso );
              if ( valorConcepto > valorTotalUso ) {
                  this.openDialog( '', `El valor facturado al concepto no puede ser mayor al uso asociado <b>${ usoByConcepto[ usoByConcepto.length -1 ].nombre }.</b>` );
                  this.getConceptos( index ).controls[ jIndex ].get( 'valorFacturadoConcepto' ).setValue( null );
                  return;
              }
          }

          if ( valorConcepto > this.criterios.controls[ index ].get( 'montoMaximo' ).value ) {
              this.openDialog( '', `<b>El valor facturado al concepto no puede ser mayor al monto maximo del criterio.</b>` );
              this.getConceptos( index ).controls[ jIndex ].get( 'valorFacturadoConcepto' ).setValue( null );
              return;
          }
          if ( this.getConceptos( index ).length > 0 ) {
              let valorTotalCriterios = 0;

              this.getConceptos( index ).controls.forEach( concepto => {
                  if ( concepto.value.valorFacturadoConcepto !== null ) {
                      valorTotalCriterios += concepto.value.valorFacturadoConcepto;
                  }
              } );

              this.criterios.controls[ index ].get( 'valorFacturado' ).setValue( valorTotalCriterios );
          }
      }
      return;
    }

    verifyValorTotalConceptos( index?: number ) {
        let totalValorConceptos = 0;
        this.criterios.controls.forEach( control => {
            if ( control.get( 'valorFacturado' ).value !== null ) {
                totalValorConceptos += control.get( 'valorFacturado' ).value;
            }
        } );

        if ( totalValorConceptos > this.montoMaximoPendiente && this.criterios.length > 1 ) {
            this.openDialog( '', '<b>La sumatoria del valor total de los conceptos, no corresponde con el monto máximo a pagar. Verifique por favor</b>' )

            this.btnBoolean = true;
            return true;
        } else {
            return false;
        }
    }

    disabledBtn() {
        if ( this.btnBoolean === true || this.criterios.dirty === false ) {
            return true;
        }
        return false;
    }

    async getvalues( criteriovalues: { codigo: string, nombre: string, porcentaje: number }[] ) {
        const values = [ ...criteriovalues ];
        if ( values.length > 0 ) {
            const criteriosSeleccionados = this.addressForm.get( 'criterioPago' ).value;

            if ( this.addressForm.get( 'criterios' ).dirty === true ) {
                this.criterios.controls.forEach( ( criterio, indexValue ) => {
                    values.forEach( ( value, index ) => {
                        if ( value.codigo === criterio.value.tipoCriterioCodigo ) {
                            values.splice( index, 1 );
                        }
                    } );
                    const test = criteriovalues.filter( value => value.codigo === criterio.value.tipoCriterioCodigo );
                    if ( test.length === 0 ) {
                        this.criterios.removeAt( indexValue );
                    }
                } );

                for ( const value of values ) {
                    const tiposDePago = await this.registrarPagosSvc.getTipoPagoByCriterioCodigo( value.codigo );

                    const montoMaximo = this.montoMaximoPendiente !== 0 ? ( this.montoMaximoPendiente * value.porcentaje ) : 0;

                    this.criterios.push(
                        this.fb.group(
                            {
                                solicitudPagoFaseId: [ this.solicitudPagoFase !== undefined ? this.solicitudPagoFase.solicitudPagoFaseId : 0 ],
                                solicitudPagoFaseCriterioId: [ 0 ],
                                montoMaximo: [ montoMaximo ],
                                tipoCriterioCodigo: [ value.codigo ],
                                nombreCriterio: [ value.nombre ],
                                tiposDePago: [ tiposDePago ],
                                tipoPago: [ null, Validators.required ],
                                conceptosDePago: [ [], Validators.required ],
                                conceptoPago: [ null ],
                                usoCodigo: [ null ],
                                conceptos: this.fb.array( [] ),
                                valorFacturado: [ { value: null, disabled: true }, Validators.required ]
                            }
                        )
                    );
                }
                this.addressForm.get( 'criterioPago' ).setValue( criteriosSeleccionados );
                this.ocultarAmortizacionAnticipo.emit( this.addressForm.get('criterioPago').value );
            }
            if ( this.addressForm.get( 'criterios' ).dirty === false ) {
                if ( this.solicitudPagoFase !== undefined && this.solicitudPagoFase.solicitudPagoFaseCriterio.length > 0 ) {
                    this.criterios.controls.forEach( ( criterio, indexValue ) => {
                        values.forEach( ( value, index ) => {
                            if ( value.codigo === criterio.value.tipoCriterioCodigo ) {
                                values.splice( index, 1 );
                            }
                        } );
                        const test = criteriovalues.filter( value => value.codigo === criterio.value.tipoCriterioCodigo );
                        if ( test.length === 0 ) {
                            this.criterios.removeAt( indexValue );
                        }
                    } );

                    for ( const value of values ) {
                        const tiposDePago = await this.registrarPagosSvc.getTipoPagoByCriterioCodigo( value.codigo );

                        const montoMaximo = this.montoMaximoPendiente !== 0 ? ( this.montoMaximoPendiente * value.porcentaje ) : 0;

                        this.criterios.push(
                            this.fb.group(
                                {
                                    solicitudPagoFaseId: [ this.solicitudPagoFase !== undefined ? this.solicitudPagoFase.solicitudPagoFaseId : 0 ],
                                    solicitudPagoFaseCriterioId: [ 0 ],
                                    montoMaximo: [ montoMaximo ],
                                    tipoCriterioCodigo: [ value.codigo ],
                                    nombreCriterio: [ value.nombre ],
                                    tiposDePago: [ tiposDePago ],
                                    tipoPago: [ null, Validators.required ],
                                    conceptosDePago: [ [], Validators.required ],
                                    conceptoPago: [ null ],
                                    usoCodigo: [ null ],
                                    conceptos: this.fb.array( [] ),
                                    valorFacturado: [ { value: null, disabled: true }, Validators.required ]
                                }
                            )
                        );
                    }
                } else {
                    this.criterios.clear();

                    for ( const value of values ) {
                        const tiposDePago = await this.registrarPagosSvc.getTipoPagoByCriterioCodigo( value.codigo );

                        const montoMaximo = this.montoMaximoPendiente !== 0 ? ( this.montoMaximoPendiente * value.porcentaje ) : 0;

                        this.criterios.push(
                            this.fb.group(
                                {
                                    solicitudPagoFaseId: [ this.solicitudPagoFase !== undefined ? this.solicitudPagoFase.solicitudPagoFaseId : 0 ],
                                    solicitudPagoFaseCriterioId: [ 0 ],
                                    montoMaximo: [ montoMaximo ],
                                    tipoCriterioCodigo: [ value.codigo ],
                                    nombreCriterio: [ value.nombre ],
                                    tiposDePago: [ tiposDePago ],
                                    tipoPago: [ null, Validators.required ],
                                    conceptosDePago: [ [], Validators.required ],
                                    conceptoPago: [ null ],
                                    usoCodigo: [ null ],
                                    conceptos: this.fb.array( [] ),
                                    valorFacturado: [ { value: null, disabled: true }, Validators.required ]
                                }
                            )
                        );
                    }
                }
            }
        } else {
            this.criterios.clear();
        }
    }

    async getConceptosDePago( index: number, tipoPago: any ) {
        const conceptosDePago = await this.registrarPagosSvc.getConceptoPagoCriterioCodigoByTipoPagoCodigo( tipoPago.codigo );
        this.criterios.controls[ index ].get( 'conceptosDePago' ).setValue( conceptosDePago );
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

    getvaluesConcepto( conceptos: any[], index: number, criterioCodigo: any, e ) {

        this.getConceptos( index ).clear();
        this.criterios.controls[ index ].get( 'valorFacturado' ).setValue( null );

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
                  const montoMaximoPendienteNew = await this.registrarPagosSvc.getMontoMaximoMontoPendiente( this.solicitudPago.solicitudPagoId, this.forma_pago_codigo, this.esPreconstruccion === true ? 'True' : 'False', this.contratacionProyectoId ,criterioCodigo, concepto.codigo, e ).toPromise();
                //   console.log(montoMaximoPendienteNew);

                    this.getConceptos( index ).push(
                        this.fb.group(
                            {
                                solicitudPagoFaseCriterioConceptoPagoId: [ 0 ],
                                solicitudPagoFaseCriterioId: [ this.criterios.controls[ index ].get( 'solicitudPagoFaseCriterioId' ).value ],
                                conceptoPagoCriterioNombre: [ concepto.nombre ],
                                usoCodigo: [ concepto.usoCodigo ],
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
                  const montoMaximoPendienteNew = await this.registrarPagosSvc.getMontoMaximoMontoPendiente( this.solicitudPago.solicitudPagoId, this.forma_pago_codigo, this.esPreconstruccion === true ? 'True' : 'False', this.contratacionProyectoId ,criterioCodigo, concepto.codigo, e ).toPromise();
                //   console.log(montoMaximoPendienteNew);

                    this.getConceptos( index ).push(
                        this.fb.group(
                            {
                                solicitudPagoFaseCriterioConceptoPagoId: [ 0 ],
                                solicitudPagoFaseCriterioId: [ this.criterios.controls[ index ].get( 'solicitudPagoFaseCriterioId' ).value ],
                                conceptoPagoCriterioNombre: [ concepto.nombre ],
                                usoCodigo: [ concepto.usoCodigo ],
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

    openDialog(modalTitle: string, modalText: string) {
        const dialogRef = this.dialog.open(ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText }
        });
    }

    openDialogTrueFalse(modalTitle: string, modalText: string) {

        const dialogRef = this.dialog.open(ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText, siNoBoton: true }
        });

        return dialogRef.afterClosed();
    }

    deleteCriterio( index: number, pSolicitudPagoFaseCriterioConceptoId: number, tipoCriterioCodigo: string ) {
        this.openDialogTrueFalse( '', '<b>¿Está seguro de eliminar esta información?</b>' )
            .subscribe(
                value => {
                    if ( value === true ) {
                        if ( pSolicitudPagoFaseCriterioConceptoId === 0 ) {
                            this.criterios.removeAt( index );
                            const criteriosSeleccionados = this.addressForm.get( 'criterioPago' ).value;
                            if ( criteriosSeleccionados !== null && criteriosSeleccionados.length > 0 ) {
                                criteriosSeleccionados.forEach( ( criterioValue, index ) => {
                                    if ( criterioValue.codigo === tipoCriterioCodigo ) {
                                        criteriosSeleccionados.splice( index, 1 );
                                    }
                                } );
                            }
                            this.addressForm.get( 'criterioPago' ).setValue( criteriosSeleccionados );
                            this.ocultarAmortizacionAnticipo.emit( this.addressForm.get('criterioPago').value );
                            this.openDialog( '', '<b>La información se ha eliminado correctamente.</b>' );
                        } else {
                            this.criterios.removeAt( index );
                            const criteriosSeleccionados = this.addressForm.get( 'criterioPago' ).value;
                            if ( criteriosSeleccionados !== null && criteriosSeleccionados.length > 0 ) {
                                criteriosSeleccionados.forEach( ( criterioValue, index ) => {
                                    if ( criterioValue.codigo === tipoCriterioCodigo ) {
                                        criteriosSeleccionados.splice( index, 1 );
                                    }
                                } );
                            }
                            this.addressForm.get( 'criterioPago' ).setValue( criteriosSeleccionados );
                            this.ocultarAmortizacionAnticipo.emit( this.addressForm.get('criterioPago').value );
                            this.registrarPagosSvc.DeleteSolicitudPagoFaseCriterioConceptoPago( pSolicitudPagoFaseCriterioConceptoId )
                                .subscribe(
                                    () => {
                                        this.openDialog( '', '<b>La información se ha eliminado correctamente.</b>' );
                                        location.reload();
                                    },
                                    err => this.openDialog( '', `<b>${ err.message }</b>` )
                                )
                        }
                    }
                }
            );
    }

    guardar() {
        this.estaEditando = true;
        this.addressForm.markAllAsTouched();
        this.criterios.markAllAsTouched();
        const solicitudPagoFaseCriterio = [];
        let esAnticipio = false;
        this.solicitudesPagoFase = this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase;
        let solicitudPagoFase = this.solicitudesPagoFase.find(r => r.contratacionProyectoId == this.contratacionProyectoId);
        let cumpleCondiciones = true;

        this.criterios.controls.forEach( control => {
            const criterio = control.value;

            /*
            if ( control.get( 'valorFacturado' ).value !== null ) {
                valorTotalConceptos += control.get( 'valorFacturado' ).value;
            }
            */

            let valorAmortizacion = 0;
            if(solicitudPagoFase != null){
              if(cumpleCondiciones == true){
                valorAmortizacion = solicitudPagoFase?.solicitudPagoFaseAmortizacion[0]?.valorAmortizacion;
                if(solicitudPagoFase?.solicitudPagoFaseAmortizacion[0] != null){
                  console.log(control.get( 'conceptos' ).value[0]);
                  if ( (control.get( 'valorFacturado' ).value <= valorAmortizacion) ) {
                    this.openDialog( '', `El valor amortizado no puede ser mayor al valor facturado` );
                    control.get( 'conceptos' ).value[0].valorFacturadoConcepto = control.get( 'conceptos' ).value[0].valorFacturadoConcepto - control.get( 'valorFacturado' ).value;
                    control.get( 'valorFacturado' ).setValue( null );
                    cumpleCondiciones = false;
                  }
                }
              }
            }

            const criterioAnticipo = this.criteriosArray.find( value => value.nombre === 'Anticipo' && criterio.tipoCriterioCodigo === value.codigo )

            if ( criterioAnticipo !== undefined ) {
                esAnticipio = true
            }


            criterio.conceptos.forEach(element => {
                element.usoCodigo = criterio.usoCodigo
            });

            // console.log(criterio.conceptos);

            solicitudPagoFaseCriterio.push(
                {
                    tipoCriterioCodigo: criterio.tipoCriterioCodigo,
                    solicitudPagoFaseCriterioId: criterio.solicitudPagoFaseCriterioId,
                    tipoPagoCodigo: criterio.tipoPago !== null ? criterio.tipoPago.codigo : null,
                    valorFacturado: control.get( 'valorFacturado' ).value,
                    solicitudPagoFaseCriterioConceptoPago: criterio.conceptos,
                    solicitudPagoFaseId: criterio.solicitudPagoFaseId
                }
            );
        } )

        if ( this.faseCodigo === this.fasesContrato.preConstruccion ) {
            if ( this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase.length > 0 ) {
                const solicitudPagoFase = this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase.find( solicitudPagoFase => solicitudPagoFase.esPreconstruccion === true && solicitudPagoFase.contratacionProyectoId === this.contratacionProyectoId )
                const solicitudPagoFaseIndex = this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase.findIndex( solicitudPagoFase => solicitudPagoFase.esPreconstruccion === true && solicitudPagoFase.contratacionProyectoId === this.contratacionProyectoId )

                if ( solicitudPagoFase !== undefined ) {
                    this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase[ solicitudPagoFaseIndex ].solicitudPagoFaseCriterio = solicitudPagoFaseCriterio
                    this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase[ solicitudPagoFaseIndex ].esAnticipio = esAnticipio
                } else {
                    this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase.push(
                        {
                            solicitudPagoFaseId: 0,
                            esPreconstruccion: this.esPreconstruccion,
                            contratacionProyectoId: this.contratacionProyectoId,
                            solicitudPagoRegistrarSolicitudPagoId: this.solicitudPagoRegistrarSolicitudPagoId,
                            solicitudPagoFaseCriterio,
                            esAnticipio
                        }
                    )
                }
            } else {
                this.solicitudPago.solicitudPagoRegistrarSolicitudPago[ 0 ].solicitudPagoFase = [
                    {
                        solicitudPagoFaseId: 0,
                        esPreconstruccion: this.esPreconstruccion,
                        contratacionProyectoId: this.contratacionProyectoId,
                        solicitudPagoRegistrarSolicitudPagoId: this.solicitudPagoRegistrarSolicitudPagoId,
                        solicitudPagoFaseCriterio,
                        esAnticipio
                    }
                ]
            }
        }

        if ( this.faseCodigo === this.fasesContrato.construccion ) {
            if ( this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase.length > 0 ) {
                const solicitudPagoFase = this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase.find( solicitudPagoFase => solicitudPagoFase.esPreconstruccion === false && solicitudPagoFase.contratacionProyectoId === this.contratacionProyectoId )
                const solicitudPagoFaseIndex = this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase.findIndex( solicitudPagoFase => solicitudPagoFase.esPreconstruccion === false && solicitudPagoFase.contratacionProyectoId === this.contratacionProyectoId )

                if ( solicitudPagoFase !== undefined ) {
                    this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase[ solicitudPagoFaseIndex ].solicitudPagoFaseCriterio = solicitudPagoFaseCriterio
                    this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase[ solicitudPagoFaseIndex ].esAnticipio = esAnticipio
                } else {
                    // console.log( 'Test' )
                    this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase.push(
                        {
                            solicitudPagoFaseId: 0,
                            esPreconstruccion: this.esPreconstruccion,
                            contratacionProyectoId: this.contratacionProyectoId,
                            solicitudPagoRegistrarSolicitudPagoId: this.solicitudPagoRegistrarSolicitudPagoId,
                            solicitudPagoFaseCriterio,
                            esAnticipio
                        }
                    )
                }
            } else {
                this.solicitudPago.solicitudPagoRegistrarSolicitudPago[ 0 ].solicitudPagoFase = [
                    {
                        solicitudPagoFaseId: 0,
                        esPreconstruccion: this.esPreconstruccion,
                        contratacionProyectoId: this.contratacionProyectoId,
                        solicitudPagoRegistrarSolicitudPagoId: this.solicitudPagoRegistrarSolicitudPagoId,
                        solicitudPagoFaseCriterio,
                        esAnticipio
                    }
                ]
            }
        }

        console.log(this.solicitudPago);
        if(cumpleCondiciones == true){
          this.registrarPagosSvc.createEditNewPayment( this.solicitudPago )
          .subscribe(
              response => {
                  this.openDialog( '', `<b>${ response.message }</b>` );
                  if ( this.observacion !== undefined ) {
                      this.observacion.archivada = !this.observacion.archivada;
                      this.obsMultipleSvc.createUpdateSolicitudPagoObservacion( this.observacion ).subscribe();
                  }

                  this.registrarPagosSvc.getValidateSolicitudPagoId( this.solicitudPago.solicitudPagoId )
                      .subscribe(
                          () => {
                              this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                                  () => this.routes.navigate(
                                      [
                                          '/registrarValidarRequisitosPago/verDetalleEditar',  this.solicitudPago.contratoId, this.solicitudPago.solicitudPagoId
                                      ]
                                  )
                              );
                          }
                      );
              },
              err => this.openDialog( '', `<b>${ err.message }</b>` )
          )
        }else{
          return;
        }
    }

    criterioPago() {
        this.addressForm.get('criterioPago').valueChanges.subscribe(value => {
            this.ocultarAmortizacionAnticipo.emit( value );
        })
    }

    getUsosParaElConceoto(usoCodigo) {
        const nombreUso = this.usosParaElConceoto.find( uso => uso.codigo === usoCodigo)
        return nombreUso.nombre;
    }

}
