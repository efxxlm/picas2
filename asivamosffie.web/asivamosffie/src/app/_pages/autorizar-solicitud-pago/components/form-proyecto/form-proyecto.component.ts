import { FormGroup, FormBuilder, Validators, FormArray } from '@angular/forms';
import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { Dominio, CommonService } from 'src/app/core/_services/common/common.service';
import { TiposDeFase } from 'src/app/_interfaces/solicitud-pago.interface';

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
    @Input() idSolicitud: any;
    @Input() esVerDetalle = false;
    @Input() ocultarAcordeonAmortizacionAnticipo: boolean = true;
    @Input() num: number;
    @Output() estadoSemaforoProyecto = new EventEmitter<boolean>();
    listaFases: Dominio[] = [];
    fasesContrato = TiposDeFase;
    postConstruccion = '3';
    solicitudPagoCargarFormaPago: any;
    solicitudPagoFase = [];
    estadoSemaforoFase = 'sin-diligenciar'
    manejoAnticipoRequiere = false;
    mostrarAmortizacion = false;
    addressForm = this.fb.group(
        {
            fase: [ null, Validators.required ],
            fases: this.fb.array( [] )
        }
    );

    get fases() {
        return this.addressForm.get( 'fases' ) as FormArray
    };

    constructor(
        private fb: FormBuilder,
        private commonSvc: CommonService )
    { }

    ngOnInit(): void {
        console.log( this.proyecto )
        this.getDataProyecto()

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
        if ( this.contrato.contratoConstruccion.length === 1 ) this.manejoAnticipoRequiere = this.contrato.contratoConstruccion[0].manejoAnticipoRequiere;
        if ( this.contrato.contratoConstruccion.length > 1 ) this.manejoAnticipoRequiere = this.contrato.contratoConstruccion[this.num].manejoAnticipoRequiere;


        const solicitudPagoRegistrarSolicitudPago = this.contrato.solicitudPagoOnly.solicitudPagoRegistrarSolicitudPago[ 0 ]
        const fases = []

        if ( solicitudPagoRegistrarSolicitudPago !== undefined ) {
            this.solicitudPagoFase = solicitudPagoRegistrarSolicitudPago.solicitudPagoFase
        }

        if ( solicitudPagoRegistrarSolicitudPago !== undefined ) {
            if ( solicitudPagoRegistrarSolicitudPago.solicitudPagoFase !== undefined && solicitudPagoRegistrarSolicitudPago.solicitudPagoFase.length > 0 ) {
                solicitudPagoRegistrarSolicitudPago.solicitudPagoFase.forEach( solicitudPagoFase => {
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
                        if ( faseFind.registroCompleto === false ) {
                            registroCompletoCriterio = 'en-proceso'
                        }

                        if ( faseFind.registroCompleto === true ) {
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
                            tieneAnticipo: false
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
                        if ( faseFind.registroCompleto === false ) {
                            registroCompletoCriterio = 'en-proceso'
                        }

                        if ( faseFind.registroCompleto === true ) {
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
                            tieneAnticipo: false
                        }
                    )
                )
            }
        }
    }

}
