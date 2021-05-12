import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { FormBuilder, FormArray, Validators, FormGroup } from '@angular/forms';
import { Component, Input, OnInit } from '@angular/core';
import { RegistrarRequisitosPagoService } from 'src/app/core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';
import { OrdenPagoService } from 'src/app/core/_services/ordenPago/orden-pago.service';
import humanize from 'humanize-plus';

@Component({
  selector: 'app-form-tercero-causacion',
  templateUrl: './form-tercero-causacion.component.html',
  styleUrls: ['./form-tercero-causacion.component.scss']
})
export class FormTerceroCausacionComponent implements OnInit {

    @Input() solicitudPago: any;
    tipoDescuentoArray: Dominio[] = [];
    tipoDescuentoTecnicaArray: Dominio[] = [];
    fasePreConstruccionFormaPagoCodigo: any;
    ordenGiroDetalleTerceroCausacion: any[];
    esRegistroNuevo = true;
    formTerceroCausacion = this.fb.group(
        {
            terceroCausacion: this.fb.array( [] )
        }
    )

    get terceroCausacion() {
        return this.formTerceroCausacion.get( 'terceroCausacion' ) as FormArray;
    }

    getCriterios( index: number ) {
        return this.terceroCausacion.controls[ index ].get( 'criterios' ) as FormArray;
    }

    getConceptos( index: number, jIndex: number ) {
        return  this.getCriterios( index ).controls[ jIndex ].get( 'conceptos' ) as FormArray;
    }

    getAportantes( index: number, jIndex: number, kIndex: number ) {
        return this.getConceptos( index, jIndex ).controls[ kIndex ].get( 'aportantes' ) as FormArray;
    }

    getDescuentosFinanciera( index: number, jIndex: number, kIndex: number ) {
        return this.getConceptos( index, jIndex ).controls[ kIndex ].get( 'descuento' ).get( 'descuentos' ) as FormArray;
    }

    getDescuentosTecnica( index: number, jIndex: number, kIndex: number ) {
        return this.getConceptos( index, jIndex ).controls[ kIndex ].get( 'descuentoTecnica' ).get( 'descuentos' ) as FormArray;
    }

    constructor(
        private fb: FormBuilder,
        private registrarPagosSvc: RegistrarRequisitosPagoService,
        private ordenGiroSvc: OrdenPagoService,
        private commonSvc: CommonService )
    { }

    async ngOnInit() {
        let ordenGiroDetalle: any;
        this.tipoDescuentoTecnicaArray = await this.commonSvc.tiposDescuento().toPromise();
        this.tipoDescuentoArray = await this.commonSvc.listaDescuentosOrdenGiro().toPromise();

        if ( this.solicitudPago.ordenGiro !== undefined ) {
    
            if ( this.solicitudPago.ordenGiro.ordenGiroDetalle !== undefined ) {
                if ( this.solicitudPago.ordenGiro.ordenGiroDetalle.length > 0 ) {
                    ordenGiroDetalle = this.solicitudPago.ordenGiro.ordenGiroDetalle[0];

                    if ( ordenGiroDetalle.ordenGiroDetalleTerceroCausacion !== undefined ) {
                        if ( ordenGiroDetalle.ordenGiroDetalleTerceroCausacion.length > 0 ) {
                            this.ordenGiroDetalleTerceroCausacion = ordenGiroDetalle.ordenGiroDetalleTerceroCausacion;
                        }
                    }
                }
            }
        }

        for ( const solicitudPagoFase of this.solicitudPago.solicitudPagoRegistrarSolicitudPago[ 0 ].solicitudPagoFase ) {
            const criteriosArray = [];
            let valorNetoGiro = 0;
            let nuevoValorRegistrado = 0;
            const solicitudPagoFaseCriterio = solicitudPagoFase.solicitudPagoFaseCriterio;
            const solicitudPagoFaseFactura = solicitudPagoFase.solicitudPagoFaseFactura[0];

            if ( this.solicitudPago.contratoSon.solicitudPago.length > 1 ) {
                this.fasePreConstruccionFormaPagoCodigo = this.solicitudPago.contratoSon.solicitudPago[0].solicitudPagoCargarFormaPago[0];
            } else {
                this.fasePreConstruccionFormaPagoCodigo = this.solicitudPago.solicitudPagoCargarFormaPago[0];
            }
            const criterioFormaPago = await this.registrarPagosSvc.getCriterioByFormaPagoCodigo(
                solicitudPagoFase.esPreconstruccion === true ? this.fasePreConstruccionFormaPagoCodigo.fasePreConstruccionFormaPagoCodigo : this.fasePreConstruccionFormaPagoCodigo.faseConstruccionFormaPagoCodigo
            ).toPromise();

            // Get constantes y variables
            const listCriterios = [];
            // Busqueda de criterios seleccionados en el CU 4.1.7 en la lista de tipo dominio
            for ( const criterioValue of solicitudPagoFaseCriterio ) {
                const criterioFind = criterioFormaPago.find( value => value.codigo === criterioValue.tipoCriterioCodigo );
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
            // Get data del guardado de tercero de causacion
            for ( const criterio of listCriterios ) {
                const listDescuento = [ ...this.tipoDescuentoArray ];

                if ( this.ordenGiroDetalleTerceroCausacion !== undefined ) {
                    const terceroCausacion = this.ordenGiroDetalleTerceroCausacion.find( tercero => tercero.conceptoPagoCriterio === criterio.tipoCriterioCodigo && tercero.esPreconstruccion === solicitudPagoFase.esPreconstruccion );
                    const listaDescuentos = [];
                    const listaAportantes = [];
                    const conceptosDePago = [];

                    if ( terceroCausacion !== undefined ) {
                        if ( terceroCausacion.ordenGiroDetalleTerceroCausacionDescuento.length > 0 ) {
                            terceroCausacion.ordenGiroDetalleTerceroCausacionDescuento.forEach( descuento => {

                                if ( descuento.tipoDescuentoCodigo !== undefined ) {                                            
                                    const descuentoIndex = listDescuento.findIndex( descuentoIndex => descuentoIndex.codigo === descuento.tipoDescuentoCodigo );

                                    if ( descuentoIndex !== -1 ) {
                                        listDescuento.splice( descuentoIndex, 1 );
                                    }
                                }
                                listaDescuentos.push(
                                    this.fb.group(
                                        {
                                            ordenGiroDetalleTerceroCausacionDescuentoId: [ descuento.ordenGiroDetalleTerceroCausacionDescuentoId ],
                                            tipoDescuento: [ descuento.tipoDescuentoCodigo, Validators.required ],
                                            valorDescuento: [ descuento.valorDescuento, Validators.required ],
                                            nuevoValorDescuento: [ null, Validators.required ]
                                        }
                                    )
                                );
                            } )
                        }
                        // Get lista de aportantes
                        if ( terceroCausacion.ordenGiroDetalleTerceroCausacionAportante.length > 0 ) {
                            for ( const aportante of terceroCausacion.ordenGiroDetalleTerceroCausacionAportante ) {
                                const nombreAportante = dataAportantes.listaNombreAportante.find( nombre => nombre.cofinanciacionAportanteId === aportante.aportanteId );

                                if ( nombreAportante !== undefined ) {
                                    const tipoAportante = dataAportantes.listaTipoAportante.find( tipo => tipo.dominioId === nombreAportante.tipoAportanteId );
                                    const tipoAportanteIndex = dataAportantes.listaTipoAportante.findIndex( tipo => tipo.dominioId === nombreAportante.tipoAportanteId );
                                    let listaFuenteRecursos: any[] = await this.ordenGiroSvc.getFuentesDeRecursosPorAportanteId( nombreAportante.cofinanciacionAportanteId ).toPromise();
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
                                                valorDescuento: [ aportante.valorDescuento, Validators.required ],
                                                nuevoValorDescuento: [ null, Validators.required ]
                                            }
                                        )
                                    )
                                }
                            }
                        }

                        for ( const concepto of criterio.listConceptos ) {
                            const listaDescuentoTecnica = [];

                            if ( solicitudPagoFaseFactura !== undefined ) {
                                if ( solicitudPagoFaseFactura.solicitudPagoFaseFacturaDescuento !== undefined ) {
                                    if ( solicitudPagoFaseFactura.solicitudPagoFaseFacturaDescuento.length > 0 ) {
                                        solicitudPagoFaseFactura.solicitudPagoFaseFacturaDescuento.forEach( descuentoTecnica => {
                                            listaDescuentoTecnica.push(
                                                this.fb.group(
                                                    {
                                                        solicitudPagoFaseFacturaDescuentoId: [ descuentoTecnica.solicitudPagoFaseFacturaDescuentoId ],
                                                        tipoDescuento: [ descuentoTecnica.tipoDescuentoCodigo, Validators.required ],
                                                        valorDescuento: [ descuentoTecnica.valorDescuento, Validators.required ],
                                                        nuevoValorDescuento: [ null, Validators.required ]
                                                    }
                                                )
                                            )
                                        } )
                                    }
                                }
                            }

                            conceptosDePago.push( this.fb.group(
                                {
                                    conceptoPagoCriterio: [ concepto.codigo ],
                                    nombre: [ concepto.nombre ],
                                    valorFacturadoConcepto: [ concepto.valorFacturadoConcepto ],
                                    tipoDeAportantes: [ dataAportantes.listaTipoAportante ],
                                    nombreDeAportantes: [ dataAportantes.listaNombreAportante ],
                                    tipoDescuentoArray: [ listDescuento ],
                                    descuento: this.fb.group(
                                        {
                                            aplicaDescuentos:[ terceroCausacion.tieneDescuento, Validators.required ],
                                            numeroDescuentos: [ terceroCausacion.ordenGiroDetalleTerceroCausacionDescuento.length > 0 ? terceroCausacion.ordenGiroDetalleTerceroCausacionDescuento.length : null, Validators.required ],
                                            descuentos: this.fb.array( listaDescuentos )
                                        }
                                    ),
                                    descuentoTecnica: this.fb.group(
                                        {
                                            aplicaDescuentos:[ solicitudPagoFaseFactura.tieneDescuento, Validators.required ],
                                            numeroDescuentos: [ solicitudPagoFaseFactura.solicitudPagoFaseFacturaDescuento.length > 0 ? solicitudPagoFaseFactura.solicitudPagoFaseFacturaDescuento.length : null, Validators.required ],
                                            descuentos: this.fb.array( listaDescuentoTecnica )
                                        }
                                    ),
                                    aportantes: this.fb.array( listaAportantes )
                                }
                            ) )
                        }

                        valorNetoGiro = terceroCausacion.valorNetoGiro;
                        criteriosArray.push(
                            this.fb.group(
                                {
                                    ordenGiroDetalleTerceroCausacionId: [ terceroCausacion.ordenGiroDetalleTerceroCausacionId ],
                                    tipoCriterioCodigo: [ criterio.tipoCriterioCodigo ],
                                    nombre: [ criterio.nombre ],
                                    tipoPagoCodigo: [ criterio.tipoPagoCodigo ],
                                    conceptos: this.fb.array( conceptosDePago )
                                }
                            ) 
                        )
                    }
                }
            }

            this.terceroCausacion.push(
                this.fb.group(
                    {
                        valorNetoGiro,
                        nuevoValorRegistrado,
                        esPreconstruccion: solicitudPagoFase.esPreconstruccion,
                        criterios: this.fb.array( criteriosArray )
                    }
                )
            )
        }
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

    getDescuentoTecnica( codigo: string ) {
        if ( this.tipoDescuentoTecnicaArray.length > 0 ) {
            const descuento = this.tipoDescuentoTecnicaArray.find( descuento => descuento.codigo === codigo );

            if ( descuento !== undefined ) {
                return descuento.nombre;
            }
        }
    }

    validateNumberKeypress(event: KeyboardEvent) {
        const alphanumeric = /[0-9]/;
        const inputChar = String.fromCharCode(event.charCode);
        return alphanumeric.test(inputChar) ? true : false;
    }

    guardar( index: number ) {
        const pBalanceFinanciero = [];

        this.getCriterios( index ).controls.forEach( ( criterioControl, indexCriterio ) => {
            pBalanceFinanciero.push(
                {

                }
            )
        } )
    }

}
