import { Router } from '@angular/router';
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
    tipoDescuentoArray: Dominio[] = [];
    listaCriterios: Dominio[] = [];
    listaFuenteTipoFinanciacion: Dominio[] = [];
    cantidadAportantes: number;
    solicitudPagoFase: any;
    solicitudPagoFaseCriterio: any;
    solicitudPagoFaseFactura: any;
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

    constructor (
        private fb: FormBuilder,
        private commonSvc: CommonService,
        private dialog: MatDialog,
        private routes: Router,
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
        this.solicitudPagoFase = this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase[0];
        this.solicitudPagoFaseCriterio = this.solicitudPagoFase.solicitudPagoFaseCriterio;
        this.solicitudPagoFaseFactura = this.solicitudPagoFase.solicitudPagoFaseFactura[0];

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
                    this.ordenGiroSvc.getAportantes( this.solicitudPago, dataAportantes => {
                        // Get boolean si es uno o varios aportantes
                        if ( dataAportantes.listaTipoAportante.length > 1 ) {
                            this.variosAportantes = true;
                        } else {
                            this.variosAportantes = false
                        }
                        // Get cantidad de aportantes para limitar cuantos aportantes se pueden agregar en el formulario
                        this.cantidadAportantes = dataAportantes.listaTipoAportante.length;
                        // Get data del guardado de tercero de causacion
                        for ( const criterio of listCriterios ) {                            
                            if ( this.ordenGiroDetalleTerceroCausacion !== undefined ) {
                                const terceroCausacion = this.ordenGiroDetalleTerceroCausacion.find( tercero => tercero.conceptoPagoCriterio === criterio.tipoCriterioCodigo );
                                const listaDescuentos = [];
                                const listaAportantes = [];
                                const conceptosDePago = [];

                                if ( terceroCausacion.ordenGiroDetalleTerceroCausacionDescuento.length > 0 ) {
                                    terceroCausacion.ordenGiroDetalleTerceroCausacionDescuento.forEach( descuento => {
                                        listaDescuentos.push(
                                            this.fb.group(
                                                {
                                                    ordenGiroDetalleTerceroCausacionDescuentoId: [ descuento.ordenGiroDetalleTerceroCausacionDescuentoId ],
                                                    tipoDescuento: [ descuento.tipoDescuentoCodigo, Validators.required ],
                                                    valorDescuento: [ descuento.valorDescuento, Validators.required ]
                                                }
                                            )
                                        );
                                    } )
                                }
                                // Get lista de aportantes
                                this.ordenGiroSvc.getAportantes( this.solicitudPago, dataAportantes => {
                                    // Get cantidad de aportantes para limitar cuantos aportantes se pueden agregar en el formulario
                                    this.cantidadAportantes = dataAportantes.listaTipoAportante.length;

                                    if ( terceroCausacion.ordenGiroDetalleTerceroCausacionAportante.length > 0 ) {
                                        for ( const aportante of terceroCausacion.ordenGiroDetalleTerceroCausacionAportante ) {
                                            const nombreAportante = dataAportantes.listaNombreAportante.find( nombre => nombre.cofinanciacionAportanteId === aportante.aportanteId );
                                            const tipoAportante = dataAportantes.listaTipoAportante.find( tipo => tipo.dominioId === nombreAportante.tipoAportanteId );
                                            let listaFuenteRecursos: any[];
                                            this.ordenGiroSvc.getFuentesDeRecursosPorAportanteId( nombreAportante.cofinanciacionAportanteId )
                                                .subscribe( fuenteRecursos => {
                                                    listaFuenteRecursos = fuenteRecursos;
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
                                                } );
                                        }
                                    }
                                } );

                                for ( const concepto of criterio.listConceptos ) {
                                    setTimeout(() => {
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
                                    }, 500);
                                }

                                // Set formulario criterios
                                setTimeout(() => {
                                    this.criterios.push( this.fb.group(
                                        {
                                            estadoSemaforo: [ terceroCausacion.registroCompleto === true ? 'completo' : 'en-proceso' ],
                                            ordenGiroDetalleTerceroCausacionId: [ terceroCausacion.ordenGiroDetalleTerceroCausacionId ],
                                            tipoCriterioCodigo: [ criterio.tipoCriterioCodigo ],
                                            nombre: [ criterio.nombre ],
                                            tipoPagoCodigo: [ criterio.tipoPagoCodigo ],
                                            conceptos: this.fb.array( conceptosDePago )
                                        }
                                    ) )
                                }, 1000);
                            } else {
                                const conceptosDePago = [];
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
                                                    aplicaDescuentos:[ null, Validators.required ],
                                                    numeroDescuentos: [ null, Validators.required ],
                                                    descuentos: this.fb.array( [] )
                                                }
                                            ),
                                            aportantes: this.fb.array( [
                                                this.fb.group(
                                                    {
                                                        ordenGiroDetalleTerceroCausacionAportanteId: [ 0 ],
                                                        tipoAportante: [ null, Validators.required ],
                                                        listaNombreAportantes: [ null ],
                                                        nombreAportante: [ null, Validators.required ],
                                                        fuenteDeRecursos: [ null ],
                                                        fuenteRecursos: [ null, Validators.required ],
                                                        fuenteFinanciacionId: [ null ],
                                                        valorDescuento: [ null, Validators.required ]
                                                    }
                                                )
                                            ] )
                                        }
                                    ) )
                                }

                                // Set formulario criterios
                                this.criterios.push( this.fb.group(
                                    {
                                        estadoSemaforo: [ 'sin-diligenciar' ],
                                        ordenGiroDetalleTerceroCausacionId: [ 0 ],
                                        tipoCriterioCodigo: [ criterio.tipoCriterioCodigo ],
                                        nombre: [ criterio.nombre ],
                                        tipoPagoCodigo: [ criterio.tipoPagoCodigo ],
                                        conceptos: this.fb.array( conceptosDePago )
                                    }
                                ) )
                            }
                        }
                    } );
                } 
            );
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
            .subscribe( fuenteRecursos => this.getAportantes( index, jIndex ).controls[ kIndex ].get( 'fuenteDeRecursos' ).setValue( fuenteRecursos ) );
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
                        ordenGiroDetalleTerceroCausacionAportanteId: [ 0 ],
                        tipoAportante: [ null, Validators.required ],
                        listaNombreAportantes: [ null ],
                        nombreAportante: [ null, Validators.required ],
                        fuenteDeRecursos: [ null ],
                        fuenteRecursos: [ null, Validators.required ],
                        fuenteFinanciacionId: [ null ],
                        valorDescuento: [ null, Validators.required ]
                    }
                )
            )
        } else {
            this.openDialog( '', '<b>El contrato no tiene más aportantes asignados.</b>' )
        }
    }

    getCantidadDescuentos( value: number, index: number, jIndex: number ) {
        this.getDescuentos( index, jIndex ).clear();
        if ( value > 0 && value !== null ) {
            for ( let i = 0; i < value; i++ ) {
                this.getDescuentos( index, jIndex ).push(
                    this.fb.group(
                        {
                            ordenGiroDetalleTerceroCausacionDescuentoId: [ 0 ],
                            tipoDescuento: [ null, Validators.required ],
                            valorDescuento: [ null, Validators.required ]
                        }
                    )
                )
            }
        }
    }

    deleteDescuento( index: number, jIndex: number, kIndex: number ) {
        this.openDialogTrueFalse( '', '<b>¿Está seguro de eliminar esta información?</b>' )
        .subscribe(
            value => {
                if ( value === true ) {
                    this.getDescuentos( index, jIndex ).removeAt( kIndex );

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
        
        console.log(this.addressForm.value);

        const getOrdenGiroDetalleTerceroCausacion = ( ) => {
            const listaTerceroCausacion = [];

            this.criterios.controls.forEach( ( criterioControl, indexCriterio ) => {
                let terceroCausacion: any;
                this.getConceptos( indexCriterio ).controls.forEach( ( conceptoControl, indexConcepto ) => {
                    terceroCausacion = {
                        ordenGiroDetalleTerceroCausacionId: criterioControl.get( 'ordenGiroDetalleTerceroCausacionId' ).value,
                        valorNetoGiro: this.valorNetoGiro,
                        ordenGiroDetalleId: this.ordenGiroDetalleId,
                        conceptoPagoCriterio: criterioControl.get( 'tipoCriterioCodigo' ).value,
                        tipoPagoCodigo: criterioControl.get( 'tipoPagoCodigo' ).value,
                        tieneDescuento: conceptoControl.get( 'descuento' ).get( 'aplicaDescuentos' ).value,
                        ordenGiroDetalleTerceroCausacionDescuento: [],
                        ordenGiroDetalleTerceroCausacionAportante: []
                    }
                    this.getAportantes( indexCriterio, indexConcepto ).controls.forEach( aportanteControl => {
                        if ( aportanteControl.dirty === true ) {
                            terceroCausacion.ordenGiroDetalleTerceroCausacionAportante.push(
                                {
                                    ordenGiroDetalleTerceroCausacionId: criterioControl.get( 'ordenGiroDetalleTerceroCausacionId' ).value,
                                    ordenGiroDetalleTerceroCausacionAportanteId: aportanteControl.get( 'ordenGiroDetalleTerceroCausacionAportanteId' ).value,
                                    fuenteRecursoCodigo: aportanteControl.get( 'fuenteRecursos' ).value.codigo,
                                    fuenteFinanciacionId: aportanteControl.get( 'fuenteRecursos' ).value.fuenteFinanciacionId,
                                    aportanteId: aportanteControl.get( 'nombreAportante' ).value.cofinanciacionAportanteId,
                                    conceptoPagoCodigo: conceptoControl.get( 'conceptoPagoCriterio' ).value,
                                    valorDescuento: aportanteControl.get( 'valorDescuento' ).value
                                }
                            )
                        }
                    } )

                    if ( this.getDescuentos( indexCriterio, indexConcepto ).length > 0 && conceptoControl.get( 'descuento' ).get( 'aplicaDescuentos' ).value === true ) {
                        this.getDescuentos( indexCriterio, indexConcepto ).controls.forEach( descuentoControl => {
                            console.log( conceptoControl, descuentoControl );
                            terceroCausacion.ordenGiroDetalleTerceroCausacionDescuento.push(
                                {
                                    ordenGiroDetalleTerceroCausacionId: criterioControl.get( 'ordenGiroDetalleTerceroCausacionId' ).value,
                                    ordenGiroDetalleTerceroCausacionDescuentoId: descuentoControl.get( 'ordenGiroDetalleTerceroCausacionDescuentoId' ).value,
                                    tipoDescuentoCodigo: descuentoControl.get( 'tipoDescuento' ).value,
                                    valorDescuento: descuentoControl.get( 'valorDescuento' ).value
                                }
                            )
                        } )
                    }
                } )

                listaTerceroCausacion.push( terceroCausacion );
            } );

            return listaTerceroCausacion.length > 0 ? listaTerceroCausacion : null;
        }

        const pOrdenGiro = {
            solicitudPagoId: this.solicitudPago.solicitudPagoId,
            ordenGiroId: this.ordenGiroId,
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
                    this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                        () => this.routes.navigate(
                            [
                                '/generarOrdenDeGiro/generacionOrdenGiro', this.solicitudPago.solicitudPagoId
                            ]
                        )
                    );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

}
