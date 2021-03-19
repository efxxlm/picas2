import { Component, Input, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Dominio } from 'src/app/core/_services/common/common.service';
import { RegistrarRequisitosPagoService } from 'src/app/core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';

@Component({
  selector: 'app-criterios-pago',
  templateUrl: './criterios-pago.component.html',
  styleUrls: ['./criterios-pago.component.scss']
})
export class CriteriosPagoComponent implements OnInit {

    @Input() solicitudPago: any;
    @Input() esVerDetalle = false;
    @Input() esPreconstruccion = true;
    @Input() aprobarSolicitudPagoId: any;
    @Input() criteriosPagoFacturaCodigo: string;
    solicitudPagoObservacionId = 0;
    solicitudPagoRegistrarSolicitudPago: any;
    criteriosArray: { codigo: string, nombre: string }[] = [];
    listaCriterios: Dominio[] = [];
    criteriosArraySeleccionados: Dominio[] = [];
    addressForm: FormGroup;
    solicitudPagoFase: any;
    editorStyle = {
      height: '45px',
      overflow: 'auto'
    };
    config = {
      toolbar: [
        ['bold', 'italic', 'underline'],
        [{ list: 'ordered' }, { list: 'bullet' }],
        [{ indent: '-1' }, { indent: '+1' }],
        [{ align: [] }],
      ]
    };

    get criterios() {
        return this.addressForm.get( 'criterios' ) as FormArray;
    }

    constructor(
        private fb: FormBuilder,
        private registrarPagosSvc: RegistrarRequisitosPagoService )
    {
        this.addressForm = this.crearFormulario();
    }

    ngOnInit(): void {
        this.getCriterios();
    }

    getConceptos( index: number ) {
        return this.criterios.controls[ index ].get( 'conceptos' ) as FormArray;
    }

    getCriterios() {
        const solicitudPagoCargarFormaPago = this.solicitudPago.solicitudPagoCargarFormaPago[0];
        if ( this.esPreconstruccion === true ) {
            const fasePreConstruccionFormaPagoCodigo = solicitudPagoCargarFormaPago.fasePreConstruccionFormaPagoCodigo;
            this.registrarPagosSvc.getCriterioByFormaPagoCodigo( fasePreConstruccionFormaPagoCodigo )
                .subscribe(
                    async response => {
                        const criteriosArray = [];
                        this.solicitudPagoRegistrarSolicitudPago = this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0];
                        this.solicitudPagoFase = this.solicitudPagoRegistrarSolicitudPago.solicitudPagoFase[0];
                        if ( this.solicitudPagoFase.solicitudPagoFaseCriterio.length > 0 ) {
                            for ( const criterio of this.solicitudPagoFase.solicitudPagoFaseCriterio ) {
                                // GET Criterio seleccionado
                                const criterioSeleccionado = response.filter( value => value.codigo === criterio.tipoCriterioCodigo );
                                criteriosArray.push( criterioSeleccionado[0] );
                                // GET tipos de pago
                                const tiposDePago = await this.registrarPagosSvc.getTipoPagoByCriterioCodigo( criterio.tipoCriterioCodigo );
                                const tipoDePago = tiposDePago.filter( value => value.codigo === criterio.tipoPagoCodigo );
                                // GET conceptos de pago
                                const conceptosDePago = await this.registrarPagosSvc.getConceptoPagoCriterioCodigoByTipoPagoCodigo( criterio.tipoPagoCodigo );
                                const conceptoDePagoArray = [];
                                const conceptosDePagoSeleccionados = [];
                                // Get conceptos de pago
                                if ( criterio.solicitudPagoFaseCriterioConceptoPago.length > 0 ) {
                                    criterio.solicitudPagoFaseCriterioConceptoPago.forEach( solicitudPagoFaseCriterioConceptoPago => {
                                        conceptosDePagoSeleccionados.push( conceptosDePago.filter( concepto => concepto.codigo === solicitudPagoFaseCriterioConceptoPago.conceptoPagoCriterio ) );
                                        conceptoDePagoArray.push(
                                            this.fb.group(
                                                {
                                                    solicitudPagoFaseCriterioConceptoPagoId: [ solicitudPagoFaseCriterioConceptoPago.solicitudPagoFaseCriterioConceptoPagoId ],
                                                    solicitudPagoFaseCriterioId: [ criterio.solicitudPagoFaseCriterioId ],
                                                    conceptoPagoCriterioNombre: [ conceptosDePago.filter( concepto => concepto.codigo === solicitudPagoFaseCriterioConceptoPago.conceptoPagoCriterio )[0].nombre ],
                                                    conceptoPagoCriterio: [ solicitudPagoFaseCriterioConceptoPago.conceptoPagoCriterio ],
                                                    valorFacturadoConcepto: [ solicitudPagoFaseCriterioConceptoPago.valorFacturadoConcepto !== undefined ? solicitudPagoFaseCriterioConceptoPago.valorFacturadoConcepto : null ]
                                                }
                                            )
                                        );
                                    } );
                                }
                                this.criterios.push(
                                    this.fb.group(
                                        {
                                            solicitudPagoFaseId: [ this.solicitudPagoFase.solicitudPagoFaseId ],
                                            solicitudPagoFaseCriterioId: [ criterio.solicitudPagoFaseCriterioId ],
                                            tipoCriterioCodigo: [ criterio.tipoCriterioCodigo ],
                                            nombreCriterio: [ criterioSeleccionado[0].nombre ],
                                            tiposDePago: [ tiposDePago ],
                                            tipoPago: [ tipoDePago[0], Validators.required ],
                                            conceptosDePago: [ conceptosDePago, Validators.required ],
                                            conceptoPago: conceptosDePagoSeleccionados,
                                            conceptos: this.fb.array( conceptoDePagoArray ),
                                            valorFacturado: [ { value: criterio.valorFacturado !== undefined ? criterio.valorFacturado : null, disabled: true }, Validators.required ]
                                        }
                                    )
                                );
                            }
                        }
                        this.criteriosArray = response;
                        this.addressForm.get( 'criterioPago' ).setValue( criteriosArray );
                    }
                );
        }
        if ( this.esPreconstruccion === false ) {
            const faseConstruccionFormaPagoCodigo = solicitudPagoCargarFormaPago.faseConstruccionFormaPagoCodigo;
            this.registrarPagosSvc.getCriterioByFormaPagoCodigo( faseConstruccionFormaPagoCodigo )
                .subscribe(
                    async response => {
                        const criteriosArray = [];
                        this.solicitudPagoRegistrarSolicitudPago = this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0];
                        this.solicitudPagoFase = this.solicitudPagoRegistrarSolicitudPago.solicitudPagoFase[0];
                        if ( this.solicitudPagoFase.solicitudPagoFaseCriterio.length > 0 ) {
                            for ( const criterio of this.solicitudPagoFase.solicitudPagoFaseCriterio ) {
                                // GET Criterio seleccionado
                                const criterioSeleccionado = response.filter( value => value.codigo === criterio.tipoCriterioCodigo );
                                criteriosArray.push( criterioSeleccionado[0] );
                                // GET tipos de pago
                                const tiposDePago = await this.registrarPagosSvc.getTipoPagoByCriterioCodigo( criterio.tipoCriterioCodigo );
                                const tipoDePago = tiposDePago.filter( value => value.codigo === criterio.tipoPagoCodigo );
                                // GET conceptos de pago
                                const conceptosDePago = await this.registrarPagosSvc.getConceptoPagoCriterioCodigoByTipoPagoCodigo( criterio.tipoPagoCodigo );
                                const conceptoDePagoArray = [];
                                const conceptosDePagoSeleccionados = [];
                                // Get conceptos de pago
                                if ( criterio.solicitudPagoFaseCriterioConceptoPago.length > 0 ) {
                                    criterio.solicitudPagoFaseCriterioConceptoPago.forEach( solicitudPagoFaseCriterioConceptoPago => {
                                        conceptosDePagoSeleccionados.push( conceptosDePago.filter( concepto => concepto.codigo === solicitudPagoFaseCriterioConceptoPago.conceptoPagoCriterio ) );
                                        conceptoDePagoArray.push(
                                            this.fb.group(
                                                {
                                                    solicitudPagoFaseCriterioConceptoPagoId: [ solicitudPagoFaseCriterioConceptoPago.solicitudPagoFaseCriterioConceptoPagoId ],
                                                    solicitudPagoFaseCriterioId: [ criterio.solicitudPagoFaseCriterioId ],
                                                    conceptoPagoCriterioNombre: [ conceptosDePago.filter( concepto => concepto.codigo === solicitudPagoFaseCriterioConceptoPago.conceptoPagoCriterio )[0].nombre ],
                                                    conceptoPagoCriterio: [ solicitudPagoFaseCriterioConceptoPago.conceptoPagoCriterio ],
                                                    valorFacturadoConcepto: [ solicitudPagoFaseCriterioConceptoPago.valorFacturadoConcepto !== undefined ? solicitudPagoFaseCriterioConceptoPago.valorFacturadoConcepto : null ]
                                                }
                                            )
                                        );
                                    } );
                                }
                                this.criterios.push(
                                    this.fb.group(
                                        {
                                            solicitudPagoFaseId: [ this.solicitudPagoFase.solicitudPagoFaseId ],
                                            solicitudPagoFaseCriterioId: [ criterio.solicitudPagoFaseCriterioId ],
                                            tipoCriterioCodigo: [ criterio.tipoCriterioCodigo ],
                                            nombreCriterio: [ criterioSeleccionado[0].nombre ],
                                            tiposDePago: [ tiposDePago ],
                                            tipoPago: [ tipoDePago[0], Validators.required ],
                                            conceptosDePago: [ conceptosDePago, Validators.required ],
                                            conceptoPago: conceptosDePagoSeleccionados,
                                            conceptos: this.fb.array( conceptoDePagoArray ),
                                            valorFacturado: [ { value: criterio.valorFacturado !== undefined ? criterio.valorFacturado : null, disabled: true }, Validators.required ]
                                        }
                                    )
                                );
                            }
                        }
                        this.criteriosArray = response;
                        this.addressForm.get( 'criterioPago' ).setValue( criteriosArray );
                    }
                );
        }
    }

    crearFormulario() {
        return this.fb.group({
            criterioPago: [ null, Validators.required ],
            criterios: this.fb.array( [] )
        })
    }

    filterCriterios( tipoCriterioCodigo: string ) {
        if ( this.listaCriterios.length > 0 ) {
            const criterio = this.listaCriterios.filter( criterio => criterio.codigo === tipoCriterioCodigo );
            return criterio[0].nombre;
        }
    }

}
