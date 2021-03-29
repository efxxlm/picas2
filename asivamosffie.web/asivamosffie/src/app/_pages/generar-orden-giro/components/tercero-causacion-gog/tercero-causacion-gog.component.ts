import { CommonService, Dominio } from './../../../../core/_services/common/common.service';
import { Component, Input, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { RegistrarRequisitosPagoService } from 'src/app/core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';

@Component({
  selector: 'app-tercero-causacion-gog',
  templateUrl: './tercero-causacion-gog.component.html',
  styleUrls: ['./tercero-causacion-gog.component.scss']
})
export class TerceroCausacionGogComponent implements OnInit {

    @Input() solicitudPago: any;
    addressForm: FormGroup;
    tipoDescuentoArray: Dominio[] = [];
    listaCriterios: Dominio[] = [];
    solicitudPagoFase: any;
    solicitudPagoFaseCriterio: any;
    solicitudPagoFaseFactura: any;
    fasePreConstruccionFormaPagoCodigo: any;
    estaEditando = false;
    valorNetoGiro = 0;

    get criterios () {
        return this.addressForm.get( 'criterios' ) as FormArray;
    }

    constructor (
        private fb: FormBuilder,
        private commonSvc: CommonService,
        private registrarPagosSvc: RegistrarRequisitosPagoService )
    {
        this.commonSvc.listaDescuentosOrdenGiro()
            .subscribe( listaDescuentosOrdenGiro => this.tipoDescuentoArray = listaDescuentosOrdenGiro );
        this.crearFormulario();
        this.getValueDescuentos();
    }
  
    ngOnInit(): void {
        this.getTerceroCausacion();
    }

    crearFormulario () {
        this.addressForm = this.fb.group(
            {
                criterios: this.fb.array( [] )
                // aplicarDescuentos:[null],
                // numeroDescuentos: [ '' ],
                // descuentos: this.fb.array([])
            }
        );
    }

    getValueDescuentos() {
        // this.addressForm.get( 'numeroDescuentos' ).valueChanges
        //     .subscribe( value => {
        //         this.descuentos.clear();
        //         for ( let i = 0; i < Number(value); i++ ) {
        //             this.descuentos.push( 
        //                 this.fb.group(
        //                     {
        //                         tipoDescuento: [ null ],
        //                         valorDescuento: [ '' ],
        //                     }
        //                 ) 
        //             )
        //         }
        //     } )
    }

    getTerceroCausacion() {
        // Get Tablas
        this.solicitudPagoFase = this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase[0];
        this.solicitudPagoFaseCriterio = this.solicitudPagoFase.solicitudPagoFaseCriterio;
        this.solicitudPagoFaseFactura = this.solicitudPagoFase.solicitudPagoFaseFactura[0];
        console.log( this.solicitudPagoFaseCriterio );

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
        if ( this.solicitudPagoFase.esPreconstruccion === false ) {
            this.registrarPagosSvc.getCriterioByFormaPagoCodigo( this.fasePreConstruccionFormaPagoCodigo.fasePreConstruccionFormaPagoCodigo )
                .subscribe( getCriterioByFormaPagoCodigo => {
                    this.listaCriterios = getCriterioByFormaPagoCodigo;
                } );
        }

        if ( this.solicitudPagoFase.esPreconstruccion === false ) {
            this.registrarPagosSvc.getCriterioByFormaPagoCodigo( this.fasePreConstruccionFormaPagoCodigo.faseConstruccionFormaPagoCodigo )
                .subscribe(
                    async getCriterioByFormaPagoCodigo => {
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
                                    // Valor temporal por temas de integracion
                                    conceptoValue.conceptoPagoCriterio = '37';
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
                        console.log( listCriterios );
                        for ( const criterio of listCriterios ) {
                            this.criterios.push( this.fb.group(
                                {

                                }
                            ) )
                        }
                    } 
                );
        }
    }

    validateNumberKeypress(event: KeyboardEvent) {
        const alphanumeric = /[0-9]/;
        const inputChar = String.fromCharCode(event.charCode);
        return alphanumeric.test(inputChar) ? true : false;
    }

    eliminarDescuento ( numerodesc: number ) {
        // this.descuentos.removeAt( numerodesc );
        // this.addressForm.patchValue({
        //   numeroDescuentos: `${ this.descuentos.length }`
        // });
    }

    onSubmit() {
        this.estaEditando = true;
        this.addressForm.markAllAsTouched();
        // this.descuentos.markAllAsTouched();
        console.log(this.addressForm.value);
    }

}
