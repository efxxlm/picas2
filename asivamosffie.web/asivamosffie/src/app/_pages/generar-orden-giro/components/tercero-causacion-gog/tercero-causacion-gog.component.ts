import { MatDialog } from '@angular/material/dialog';
import { OrdenPagoService } from './../../../../core/_services/ordenPago/orden-pago.service';
import { TipoAportanteDominio, TipoAportanteCodigo } from './../../../../_interfaces/estados-solicitudPago-ordenGiro.interface';
import { CommonService, Dominio } from './../../../../core/_services/common/common.service';
import { Component, Input, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { RegistrarRequisitosPagoService } from 'src/app/core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';
import humanize from 'humanize-plus';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-tercero-causacion-gog',
  templateUrl: './tercero-causacion-gog.component.html',
  styleUrls: ['./tercero-causacion-gog.component.scss']
})
export class TerceroCausacionGogComponent implements OnInit {

    @Input() solicitudPago: any;
    addressForm: FormGroup;
    formDescuentos: FormGroup = this.fb.group(
        {
            descuento: this.fb.array( [] )
        }
    )
    tipoDescuentoArray: Dominio[] = [];
    listaCriterios: Dominio[] = [];
    listaFuenteTipoFinanciacion: Dominio[] = [];
    cantidadAportantes: number;
    solicitudPagoFase: any;
    solicitudPagoFaseCriterio: any;
    solicitudPagoFaseFactura: any;
    fasePreConstruccionFormaPagoCodigo: any;
    variosAportantes: boolean;
    estaEditando = false;
    valorNetoGiro = 0;

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
    // Get formArray de formDescuentos
    get descuento( ) {
        return this.formDescuentos.get( 'descuento' ) as FormArray;
    }

    getDescuentos( indexDescuento: number ) {
        return this.descuento.controls[ indexDescuento ].get( 'descuentos' ) as FormArray;
    }

    constructor (
        private fb: FormBuilder,
        private commonSvc: CommonService,
        private dialog: MatDialog,
        private registrarPagosSvc: RegistrarRequisitosPagoService,
        private ordenGiroSvc: OrdenPagoService )
    {
        this.commonSvc.listaFuenteTipoFinanciacion()
            .subscribe( listaFuenteTipoFinanciacion => this.listaFuenteTipoFinanciacion = listaFuenteTipoFinanciacion );
        this.commonSvc.listaDescuentosOrdenGiro()
            .subscribe( listaDescuentosOrdenGiro => this.tipoDescuentoArray = listaDescuentosOrdenGiro );
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
            this.registrarPagosSvc.getCriterioByFormaPagoCodigo(
                this.solicitudPagoFase.esPreconstruccion === true ? this.fasePreConstruccionFormaPagoCodigo.fasePreConstruccionFormaPagoCodigo : this.fasePreConstruccionFormaPagoCodigo.faseConstruccionFormaPagoCodigo
            )
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

                        this.ordenGiroSvc.getAportantes( this.solicitudPago, dataAportantes => {
                            console.log( dataAportantes, listCriterios );
                            // Get boolean si es uno o varios aportantes
                            if ( dataAportantes.listaTipoAportante.length > 1 ) {
                                this.variosAportantes = true;
                            } else {
                                this.variosAportantes = false
                            }
                            // Get cantidad de aportantes para limitar cuantos aportantes se pueden agregar en el formulario
                            this.cantidadAportantes = dataAportantes.listaTipoAportante.length;

                            for ( const criterio of listCriterios ) {
                                const conceptosDePago = [];
                                for ( const concepto of criterio.listConceptos ) {

                                    conceptosDePago.push( this.fb.group(
                                        {
                                            conceptoPagoCriterio: [ concepto.codigo ],
                                            nombre: [ concepto.nombre ],
                                            valorFacturadoConcepto: [ concepto.valorFacturadoConcepto ],
                                            tipoDeAportantes: [ dataAportantes.listaTipoAportante ],
                                            nombreDeAportantes: [ dataAportantes.listaNombreAportante ],
                                            aportantes: this.fb.array( [
                                                this.fb.group(
                                                    {
                                                        tipoAportante: [ null, Validators.required ],
                                                        listaNombreAportantes: [ null ],
                                                        nombreAportante: [ null, Validators.required ],
                                                        fuenteDeRecursos: [ null ],
                                                        fuenteRecursos: [ null, Validators.required ],
                                                        valorDescuento: [ null, Validators.required ]
                                                    }
                                                )
                                            ] )
                                        }
                                    ) )
                                }

                                this.criterios.push( this.fb.group(
                                    {
                                        tipoCriterioCodigo: [ criterio.tipoCriterioCodigo ],
                                        nombre: [ criterio.nombre ],
                                        tipoPagoCodigo: [ criterio.tipoPagoCodigo ],
                                        conceptos: this.fb.array( conceptosDePago )
                                    }
                                ) )
                            }
                        } );
                    } 
                );
        }
    }

    firstLetterUpperCase( texto:string ) {
        if ( texto !== undefined ) {
            return humanize.capitalize( String( texto ).toLowerCase() );
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
            listaAportantes.splice( aportanteIndex, 1 );

            this.getConceptos( index ).controls[ jIndex ].get( 'tipoDeAportantes' ).setValue( listaAportantes );
        }
        if ( filterAportantesDominioId.length > 0 ) {
            this.getAportantes( index, jIndex ).controls[ kIndex ].get( 'listaNombreAportantes' ).setValue( filterAportantesDominioId );
        }
    }

    getListaFuenteRecursos( nombreAportante: any, index: number, jIndex: number, kIndex: number ) {
        this.ordenGiroSvc.getFuentesDeRecursosPorAportanteId( nombreAportante.cofinanciacionAportanteId )
            .subscribe( fuenteRecursos => {
                fuenteRecursos.forEach( fuente => {
                    const getFuente = this.listaFuenteTipoFinanciacion.find( tipoFuente => tipoFuente.codigo === fuente.fuenteRecursosCodigo )

                    if ( getFuente !== undefined ) {
                        fuente.nombre = getFuente.nombre;
                    }
                } );

                this.getAportantes( index, jIndex ).controls[ kIndex ].get( 'fuenteDeRecursos' ).setValue( fuenteRecursos );
            } );
    }

    deleteAportante( index: number, jIndex: number, kIndex: number ) {
        this.openDialogTrueFalse( '', '<b>¿Está seguro de eliminar esta información?</b>' )
            .subscribe(
                value => {
                    if ( value === true ) {
                        const aportanteSeleccionado = this.getAportantes( index, jIndex ).controls[ kIndex ].get( 'tipoAportante' ).value;
                        const listaTipoAportantes = this.getConceptos( index ).controls[ jIndex ].get( 'tipoDeAportantes' ).value;
                        listaTipoAportantes.push( aportanteSeleccionado );
                        this.getConceptos( index ).controls[ jIndex ].get( 'tipoDeAportantes' ).setValue( listaTipoAportantes );

                        this.getAportantes( index, jIndex ).removeAt( kIndex );
                        this.openDialog( '', '<b>La información se ha eliminado correctamente.</b>' );
                    }
                }
            )
    }

    addAportante( index: number, jIndex: number ) {
        if ( this.getAportantes( index, jIndex ).length < this.cantidadAportantes ) {
            this.getAportantes( index, jIndex ).push(
                this.fb.group(
                    {
                        tipoAportante: [ null, Validators.required ],
                        listaNombreAportantes: [ null ],
                        nombreAportante: [ null, Validators.required ],
                        fuenteDeRecursos: [ null ],
                        fuenteRecursos: [ null, Validators.required ],
                        valorDescuento: [ null, Validators.required ]
                    }
                )
            )
        } else {
            this.openDialog( '', '<b>El contrato no tiene más aportantes asignados.</b>' )
        }
    }
    // Metodos para el formulario de formDescuento
    addDescuentos( nombreAportante: any ) {
        this.descuento.push(
            this.fb.group(
                {
                    cofinanciacionAportanteId: [ nombreAportante.cofinanciacionAportanteId ],
                    nombreAportante: [ nombreAportante.nombreAportante ],
                    aplicaDescuentos:[ null, Validators.required ],
                    numeroDescuentos: [ null, Validators.required ],
                    descuentos: this.fb.array( [] )
                }
            )
        )
    }

    getCantidadDescuentos( value: number, indexDescuento: number ) {
        console.log( value );
        
        if ( value > 0 && value !== null ) {
            this.getDescuentos( indexDescuento ).clear();
            for ( let i = 0; i < value; i++ ) {
                this.getDescuentos( indexDescuento ).push(
                    this.fb.group(
                        {
                            descuentoId: [ 0 ],
                            tipoDescuento: [ null, Validators.required ],
                            valorDescuento: [ null, Validators.required ]
                        }
                    )
                )
            }
        }
    }

    deleteDescuento( indexDescuento: number, jIndexDescuento: number ) {
        this.openDialogTrueFalse( '', '<b>¿Está seguro de eliminar esta información?</b>' )
        .subscribe(
            value => {
                if ( value === true ) {
                    this.getDescuentos( indexDescuento ).removeAt( jIndexDescuento );

                    this.openDialog( '', '<b>La información se ha eliminado correctamente.</b>' );
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

    onSubmit() {
        this.estaEditando = true;
        this.addressForm.markAllAsTouched();
        // this.descuentos.markAllAsTouched();
        console.log(this.addressForm.value);
    }

}
