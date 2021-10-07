import { FormGroup, FormBuilder, Validators, FormArray } from '@angular/forms';
import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { Dominio, CommonService } from 'src/app/core/_services/common/common.service';
import { TiposDeFase } from 'src/app/_interfaces/solicitud-pago.interface';
import { RegistrarRequisitosPagoService } from 'src/app/core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';

@Component({
  selector: 'app-form-proyecto',
  templateUrl: './form-proyecto.component.html',
  styleUrls: ['./form-proyecto.component.scss']
})
export class FormProyectoComponent implements OnInit {

    @Input() proyecto: FormGroup;
    @Input() contrato: any;
    @Input() listaMenusId: any;
    @Input() registrarSolicitudPagoObs: any;
    @Input() esVerDetalle = false;
    @Input() idSolicitud: any;
    @Input() contratacionProyectoId: any;
    @Input() ocultarAcordeonAmortizacionAnticipo: boolean = true;
    @Output() estadoSemaforoProyecto = new EventEmitter<boolean>();
    listaFases: Dominio[] = [];
    fasesContrato = TiposDeFase;
    postConstruccion = '3';
    solicitudPagoCargarFormaPago: any;
    solicitudPagoFase = [];
    estadoSemaforoFase = 'sin-diligenciar'
    manejoAnticipoRequiere = false;
    addressForm = this.fb.group(
        {
            fase: [ null, Validators.required ],
            fases: this.fb.array( [] )
        }
    );
    desabilitarAcordeonAmortizacionAnticipo = true;
    boolAplicaDescuentos = false;
    mostrarAmortizacion = false;
    get fases() {
        return this.addressForm.get( 'fases' ) as FormArray
    };

    constructor(
        private fb: FormBuilder,
        private commonSvc: CommonService,
        private registrarPagosSvc: RegistrarRequisitosPagoService )
    { }

    ngOnInit(): void {
        console.log( 'proyecto: ', this.proyecto )
        this.getDataProyecto();

        if(this.contrato.solicitudPago) {
            if(this.contrato.solicitudPago.length > 1 && this.contrato.solicitudPagoOnly.esFactura) {
                let solicitudesId = this.contrato.solicitudPago.map(e => e.solicitudPagoId)
                solicitudesId.forEach(element => {
                    if (this.idSolicitud > element) {
                        this.mostrarAmortizacion = true;
                    }
                });
                
            }
        }
    }

    async getDataProyecto() {
        if ( this.contrato.solicitudPago.length > 1 ) {
            this.solicitudPagoCargarFormaPago = this.contrato.solicitudPago[0].solicitudPagoCargarFormaPago[0];
        } else {
            this.solicitudPagoCargarFormaPago = this.contrato.solicitudPagoOnly.solicitudPagoCargarFormaPago[0];
        }
        const LISTA_FASES = await this.commonSvc.listaFases().toPromise()

        LISTA_FASES.forEach( ( fase, index ) => {
            if ( fase.codigo === this.postConstruccion ) {
                LISTA_FASES.splice( index, 1 );
            }
            if ( ( this.solicitudPagoCargarFormaPago.tieneFase1 === false || this.solicitudPagoCargarFormaPago.tieneFase1 === undefined ) && this.fasesContrato.preConstruccion === fase.codigo ) {
                LISTA_FASES.splice( index, 1 );
            }
        } );

        this.listaFases = LISTA_FASES
        if ( this.contrato.contratoConstruccion.length > 0 ) this.manejoAnticipoRequiere = this.contrato.contratoConstruccion[0].manejoAnticipoRequiere;


        const solicitudPagoRegistrarSolicitudPago = this.contrato.solicitudPagoOnly.solicitudPagoRegistrarSolicitudPago[ 0 ]
        const fases = []

        if ( solicitudPagoRegistrarSolicitudPago !== undefined ) {
            this.solicitudPagoFase = solicitudPagoRegistrarSolicitudPago.solicitudPagoFase.filter( fase => fase.contratacionProyectoId === this.proyecto.get( 'contratacionProyectoId' ).value )
        }

        if ( solicitudPagoRegistrarSolicitudPago !== undefined ) {
            if ( solicitudPagoRegistrarSolicitudPago.solicitudPagoFase !== undefined && this.solicitudPagoFase.length > 0 ) {
                this.solicitudPagoFase.forEach( solicitudPagoFase => {
                    if ( solicitudPagoFase.esPreconstruccion === true ) {
                        const fase = LISTA_FASES.find( fase => fase.codigo === this.fasesContrato.preConstruccion )

                        if ( fase !== undefined && fases[fases.length - 1] != fase ) {
                            fases.push( fase )
                        }
                    } else {
                        const fase = LISTA_FASES.find( fase => fase.codigo === this.fasesContrato.construccion )

                        if ( fase !== undefined && fases[fases.length - 1] != fase ) {
                            fases.push( fase )
                        }
                    }
                } )
            }
        }

        if ( fases.length > 0 ) {
            this.addressForm.get( 'fase' ).setValue( fases )
        }

        if ( this.solicitudPagoFase.length > 0 ) {
            const fasescompleto = this.solicitudPagoFase.filter( fase => fase.contratacionProyectoId === this.proyecto.get( 'contratacionProyectoId' ).value && fase.registroCompleto === true )
            const fasesEnProceso = this.solicitudPagoFase.filter( fase => fase.contratacionProyectoId === this.proyecto.get( 'contratacionProyectoId' ).value && fase.registroCompleto === false )

            if ( fasescompleto.length === fases.length ) {
                this.estadoSemaforoFase = 'completo'
                this.estadoSemaforoProyecto.emit( true )
            }

            if ( fasescompleto.length < fases.length && fasesEnProceso.length > 0 ) {
                this.estadoSemaforoFase = 'en-proceso'
                this.estadoSemaforoProyecto.emit( false )
            }

            if ( fasesEnProceso.length === fases.length ) {
                this.estadoSemaforoFase = 'en-proceso'
                this.estadoSemaforoProyecto.emit( false )
            }
        }

        console.log('contrato: ', this.contrato);
        this.contrato.vAmortizacionXproyecto.forEach(element => {
            if(element.tieneAnticipo === true) {
                this.desabilitarAcordeonAmortizacionAnticipo = false;
                // this.ocultarAcordeonAmortizacionAnticipo = true;
            }
        });
    }

    getValueFase( listFaseCodigo: Dominio[] ) {
        const listaFase = [ ...listFaseCodigo ];

        if ( listaFase.length === 0 ) {
            this.fases.clear()
            return
        }

        if ( this.fases.length > 0 ) {
            this.fases.controls.forEach( ( faseControl, indexControl ) => {
                const FASE_INDEX = listaFase.findIndex( value => value.codigo === faseControl.get( 'codigo' ).value );

                if ( FASE_INDEX !== -1 ) {
                    listaFase.splice( FASE_INDEX, 1 )
                } else {
                    this.fases.removeAt( indexControl )
                }
            } )

            for ( const fase of listaFase ) {
                const esPreconstruccion = fase.codigo === this.fasesContrato.preConstruccion ? true : false
                let registroCompletoCriterio = 'sin-diligenciar'
                let registroCompletoDescuentos = 'sin-diligenciar'
                
                if ( this.solicitudPagoFase.length > 0 ) {
                    const faseFind = this.solicitudPagoFase.find( value => value.esPreconstruccion === esPreconstruccion && value.contratacionProyectoId === this.proyecto.get( 'contratacionProyectoId' ).value )

                    if ( faseFind !== undefined ) {
                        // Set Value semaforo criterios de pago
                        if ( faseFind.registroCompletoCriterio === false ) {
                            registroCompletoCriterio = 'en-proceso'
                        }

                        if ( faseFind.registroCompletoCriterio === true ) {
                            registroCompletoCriterio = 'completo'
                        }

                        // Set Value semaforo descuentos de direccion tecnica
                        const solicitudPagoFaseFacturaDescuento = faseFind.solicitudPagoFaseFacturaDescuento

                        if ( faseFind.tieneDescuento !== undefined ) {
                            if ( faseFind.tieneDescuento === false ) {
                                registroCompletoDescuentos = 'completo'
                            }

                            if ( faseFind.tieneDescuento === true ) {
                                if ( solicitudPagoFaseFacturaDescuento.length > 0 ) {
                                    const descuentosRegistroCompleto = solicitudPagoFaseFacturaDescuento.filter( descuento => descuento.registroCompleto === true )

                                    if ( descuentosRegistroCompleto.length < solicitudPagoFaseFacturaDescuento.length ) {
                                        registroCompletoDescuentos = 'en-proceso'
                                    }

                                    if ( descuentosRegistroCompleto.length === solicitudPagoFaseFacturaDescuento.length ) {
                                        registroCompletoDescuentos = 'completo'
                                    }
                                } else {
                                    registroCompletoDescuentos = 'en-proceso'
                                }
                            }
                        }
                    }
                }

                this.fases.push(
                    this.fb.group(
                        {
                            codigo: fase.codigo,
                            nombre: fase.nombre,
                            registroCompletoCriterio,
                            registroCompletoDescuentos,
                            contratacionProyectoId: this.proyecto.get( 'contratacionProyectoId' ).value,
                            llaveMen: this.proyecto.get( 'llaveMen' ).value,
                            tieneAnticipo: false,
                            esPreconstruccion
                        }
                    )
                )
            }
        } else {
            for ( const fase of listaFase ) {
                const esPreconstruccion = fase.codigo === this.fasesContrato.preConstruccion ? true : false
                let registroCompletoCriterio = 'sin-diligenciar'
                let registroCompletoDescuentos = 'sin-diligenciar'
                
                if ( this.solicitudPagoFase.length > 0 ) {
                    const faseFind = this.solicitudPagoFase.find( value => value.esPreconstruccion === esPreconstruccion && value.contratacionProyectoId === this.proyecto.get( 'contratacionProyectoId' ).value )

                    if ( faseFind !== undefined ) {
                        // Set Value semaforo criterios de pago
                        if ( faseFind.registroCompletoCriterio === false ) {
                            registroCompletoCriterio = 'en-proceso'
                        }

                        if ( faseFind.registroCompletoCriterio === true ) {
                            registroCompletoCriterio = 'completo'
                        }

                        // Set Value semaforo descuentos de direccion tecnica
                        const solicitudPagoFaseFacturaDescuento = faseFind.solicitudPagoFaseFacturaDescuento

                        if ( faseFind.tieneDescuento !== undefined ) {
                            if ( faseFind.tieneDescuento === false ) {
                                registroCompletoDescuentos = 'completo'
                            }

                            if ( faseFind.tieneDescuento === true ) {
                                if ( solicitudPagoFaseFacturaDescuento.length > 0 ) {
                                    const descuentosRegistroCompleto = solicitudPagoFaseFacturaDescuento.filter( descuento => descuento.registroCompleto === true )

                                    if ( descuentosRegistroCompleto.length < solicitudPagoFaseFacturaDescuento.length ) {
                                        registroCompletoDescuentos = 'en-proceso'
                                    }

                                    if ( descuentosRegistroCompleto.length === solicitudPagoFaseFacturaDescuento.length ) {
                                        registroCompletoDescuentos = 'completo'
                                    }
                                } else {
                                    registroCompletoDescuentos = 'en-proceso'
                                }
                            }
                        }
                    }
                }

                this.fases.push(
                    this.fb.group(
                        {
                            codigo: fase.codigo,
                            nombre: fase.nombre,
                            registroCompletoCriterio,
                            registroCompletoDescuentos,
                            contratacionProyectoId: this.proyecto.get( 'contratacionProyectoId' ).value,
                            llaveMen: this.proyecto.get( 'llaveMen' ).value,
                            tieneAnticipo: false,
                            esPreconstruccion
                        }
                    )
                )
            }
        }
    }

    async guardarDescuentoAnticipo( tieneAnticipo: boolean, fase: FormGroup ) {
        if ( tieneAnticipo === true ) {
            const solicitudPago = this.contrato.solicitudPagoOnly

            if ( solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase.length > 0 ) {
              for (const solicitudPagoFase of solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase) {
                if ( solicitudPagoFase.esPreconstruccion === fase.get( 'esPreconstruccion' ).value && solicitudPagoFase.contratacionProyectoId === this.proyecto.get( 'contratacionProyectoId' ).value ) {
                  solicitudPagoFase.tieneDescuento = !tieneAnticipo
                }
              }
            }
          
            // await this.registrarPagosSvc.createEditNewPayment( solicitudPago ).toPromise()
        }
    }

    ocultarAmortizacionAnticipo(event) {
        // if (event.length === 1 && event[0].codigo === "17") this.ocultarAcordeonAmortizacionAnticipo = true;
        // else this.ocultarAcordeonAmortizacionAnticipo = false;

        if(event) {
            this.boolAplicaDescuentos = false;
            event.forEach(element => {
                if (element.codigo === "17") this.boolAplicaDescuentos = true;
            });
        }
    }

}
