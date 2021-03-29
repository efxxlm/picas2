import { Component, Input, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import { Validators, FormBuilder, FormArray } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { RegistrarRequisitosPagoService } from 'src/app/core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import humanize from 'humanize-plus';

@Component({
  selector: 'app-form-descuentos-gog',
  templateUrl: './form-descuentos-gog.component.html',
  styleUrls: ['./form-descuentos-gog.component.scss']
})
export class FormDescuentosGogComponent implements OnInit, OnChanges {

    @Input() solicitudPagoFaseCriterio: any[];
    @Input() solicitudPagoFaseFacturaDescuento: any[];
    @Input() listaCriterios: Dominio[] = [];
    @Input() descuento: any;
    @Input() valorNetoGiro: number;
    estaEditando = false;
    recibeListaCriterios = false;
    criteriosArray: Dominio[] = [];
    tiposDescuentoArray: Dominio[] = [];
    listaConceptoPago: Dominio[] = [];
    tipoAportantesArray: Dominio[] = [
      {
        nombre: 'ET',
        codigo: '1'
      }
    ];
    nombreAportantesArray: Dominio[] = [
      {
        nombre: 'Alcaldía de Susacón',
        codigo: '1'
      }
    ];
    fuenteRecursosArray: Dominio[] = [
      {
        nombre: 'Contingencias',
        codigo: '1'
      }
    ];
    addressForm = this.fb.group(
        {
            descuentos: this.fb.array( [] )
        }
    );

    get descuentos() {
        return this.addressForm.get( 'descuentos' ) as FormArray;
    }

    getCriterios( index: number ) {
        return this.descuentos.controls[ index ].get( 'criterios' ) as FormArray;
    }

    getConceptos( index: number, jIndex: number ) {
        return this.getCriterios( index ).controls[ jIndex ].get( 'conceptos' ) as FormArray;
    }

    getAportantes( index: number, jIndex: number, kIndex: number ) {
        return this.getConceptos( index, jIndex ).controls[ kIndex ].get( 'aportantes' ) as FormArray;
    }

    constructor(
        private dialog: MatDialog,
        private commonSvc: CommonService,
        private registrarPagosSvc: RegistrarRequisitosPagoService,
        private fb: FormBuilder )
    {
        this.commonSvc.tiposDescuento()
            .subscribe(response => this.tiposDescuentoArray = response);
    }

    ngOnChanges( changes: SimpleChanges ): void {
        if ( changes.listaCriterios.currentValue.length > 0 ) {
            this.criteriosArray = this.listaCriterios;
        }
    }

    ngOnInit(): void {
        this.getDescuentos();
    }

    getDescuentos() {
        this.solicitudPagoFaseFacturaDescuento.forEach( descuento => {
            this.descuentos.controls.push( this.fb.group(
                {
                    tipoDescuentoCodigo: [ descuento.tipoDescuentoCodigo ],
                    criterio: [ null, Validators.required ],
                    criterios: this.fb.array( [] )
                }
            ) );
        } );
    }

    firstLetterUpperCase( texto:string ) {
        if ( texto !== undefined ) {
            return humanize.capitalize( String( texto ).toLowerCase() );
        }
    }

    totalDescuentos( cb: { ( totalDescuento: number ): void } ) {
        const totaldescuentos = (): number => {
            let total = 0;
            this.descuentos.controls.forEach( ( descuentoControl, indexDescuento ) => {
                this.getCriterios( indexDescuento ).controls.forEach( ( criterioControl, indexCriterio ) => {
                    this.getConceptos( indexDescuento, indexCriterio ).controls.forEach( ( conceptoControl, indexConcepto ) => {
                        this.getAportantes( indexDescuento, indexCriterio, indexConcepto ).controls.forEach(
                            aportanteControl => aportanteControl.get( 'valorDescuento' ).value !== null ? total += aportanteControl.get( 'valorDescuento' ).value : total += 0
                        );
                    } );
                } );
            } )

            return total;
        }
        
        cb( totaldescuentos() );
    }

    checkTotalDiscountValues( index: number, jIndex: number, kIndex: number, lIndex: number ) {
        this.totalDescuentos( totalDescuento => {
            if ( totalDescuento > this.valorNetoGiro ) {
                this.getAportantes( index, jIndex, kIndex ).controls[ lIndex ].get( 'valorDescuento' ).setValue( null );
                this.openDialog( '', '<b>El valor total de los descuentos no puede ser mayor al valor neto de giro.</b>' )
            }
        } );
    }

    getTipoDescuento( tipoDescuentoCodigo: string ) {
        if (this.tiposDescuentoArray.length > 0) {
            const descuento = this.tiposDescuentoArray.find( descuento => descuento.codigo === tipoDescuentoCodigo );
            
            if ( descuento !== undefined ) {
                return descuento.nombre;
            }
        }
    }

    async getListCriterios( list: string[], index: number ) {
        const listaCriterios = [ ...list ];

        if ( listaCriterios.length === 0 ) {
            this.getCriterios( index ).clear();
            return;
        }

        if ( this.getCriterios( index ).length > 0 ) {
            this.getCriterios( index ).controls.forEach( ( control, indexControl ) => {
                const criterioIndex = listaCriterios.findIndex( codigo => codigo === control.get( 'criterioCodigo' ).value );
                const criterio = listaCriterios.find( criterio => criterio === control.get( 'criterioCodigo' ).value );

                if ( criterioIndex !== -1 ) {
                    listaCriterios.splice( criterioIndex, 1 );
                }

                if ( criterio === undefined ) {
                    this.getCriterios( index ).removeAt( indexControl );
                    listaCriterios.splice( criterioIndex, 1 );
                }
            } );
        }

        for ( const codigo of listaCriterios ) {
            const criterio = this.solicitudPagoFaseCriterio.find( criterio => criterio.tipoCriterioCodigo === codigo );
            const tiposDePago = await this.registrarPagosSvc.getTipoPagoByCriterioCodigo( codigo );
            const tipoPago = tiposDePago.find( tipoPago => tipoPago.codigo === criterio.tipoPagoCodigo );
            const conceptosDePago = await this.registrarPagosSvc.getConceptoPagoCriterioCodigoByTipoPagoCodigo( tipoPago.codigo );

            conceptosDePago.push(
                {
                    codigo: '1',
                    nombre: 'test concepto'
                }
            )

            if ( criterio !== undefined ) {
                this.getCriterios( index ).push( this.fb.group(
                    {
                        nombre: [ this.criteriosArray.find( criterio => criterio.codigo === codigo ).nombre ],
                        criterioCodigo: [ this.criteriosArray.find( criterio => criterio.codigo === codigo ).codigo ],
                        tipoPagoNombre: [ tipoPago.nombre ],
                        conceptosDePago: [ conceptosDePago ],
                        concepto: [ null, Validators.required ],
                        conceptos: this.fb.array( [] )
                    }
                ) );
            }
        }
    }

    async getListConceptos( list: string[], index: number, jIndex: number ) {
        const listaConceptos = [ ...list ];
        const conceptosDePago: Dominio[] = this.getCriterios( index ).controls[ jIndex ].get( 'conceptosDePago' ).value;

        if ( listaConceptos.length === 0 ) {
            this.getConceptos( index, jIndex ).clear();
            return;
        }

        if ( this.getConceptos( index, jIndex ).length > 0 ) {
            this.getConceptos( index, jIndex ).controls.forEach( ( control, indexControl ) => {
                const conceptoIndex = listaConceptos.findIndex( concepto => concepto === control.get( 'conceptoCodigo' ).value );
                const concepto = listaConceptos.find( concepto => concepto === control.get( 'conceptoCodigo' ).value );

                if ( conceptoIndex !== -1 ) {
                    listaConceptos.splice( conceptoIndex, 1 );
                }

                if ( concepto === undefined ) {
                    this.getConceptos( index, jIndex ).removeAt( indexControl );
                    listaConceptos.splice( conceptoIndex, 1 );
                }
            } );
        }
        
        for ( const codigo of listaConceptos ) {
            const concepto = conceptosDePago.find( concepto => concepto.codigo === codigo );

            if ( concepto !== undefined ) {
                this.getConceptos( index, jIndex ).push( this.fb.group(
                    {
                        nombre: [ concepto.nombre ],
                        conceptoCodigo: [ concepto.codigo ],
                        aportantes: this.fb.array( [
                            this.fb.group(
                                {
                                    tipoAportante: [ null, Validators.required ],
                                    nombreAportante: [ null, Validators.required ],
                                    fuenteRecursos: [ null, Validators.required ],
                                    valorDescuento: [ null, Validators.required ]
                                }
                            )
                        ] )
                    }
                ) );
            }
        }
        //console.log( listaConceptos );
    }

    deleteAportante( index: number, jIndex: number, kIndex: number, lIndex: number ) {
        this.openDialogTrueFalse( '', '<b>¿Está seguro de eliminar esta información?</b>' )
            .subscribe(
                value => {
                    if ( value === true ) {
                        this.getAportantes( index, jIndex, kIndex ).removeAt( lIndex );
                        this.openDialog( '', '<b>La información se ha eliminado correctamente.</b>' );
                    }
                }
            )
    }

    deleteConcepto( index: number, jIndex: number, kIndex: number ) {
        this.openDialogTrueFalse( '', '<b>¿Está seguro de eliminar esta información?</b>' )
        .subscribe(
            value => {
                if ( value === true ) {
                    const listConceptos: string[] = this.getCriterios( index ).controls[ jIndex ].get( 'concepto' ).value;
                    const indexConcepto = listConceptos.findIndex( codigo => codigo === this.getConceptos( index, jIndex ).controls[ kIndex ].get( 'conceptoCodigo' ).value );
                    listConceptos.splice( indexConcepto, 1 );

                    this.getCriterios( index ).controls[ jIndex ].get( 'concepto' ).setValue( listConceptos.length > 0 ? listConceptos : null );
                    this.getConceptos( index, jIndex ).removeAt( kIndex );
                    this.openDialog( '', '<b>La información se ha eliminado correctamente.</b>' );
                }
            }
        )
    }

    deleteCriterio( index: number, jIndex: number ) {
        this.openDialogTrueFalse( '', '<b>¿Está seguro de eliminar esta información?</b>' )
            .subscribe(
                value => {
                    if ( value === true ) {
                        const listCriterio: string[] =  this.descuentos.controls[ index ].get( 'criterio' ).value;
                        const indexCriterio = listCriterio.findIndex( codigo => codigo === this.getCriterios( index ).controls[ jIndex ].get( 'criterioCodigo' ).value )
                        listCriterio.splice( indexCriterio, 1 );

                        this.descuentos.controls[ index ].get( 'criterio' ).setValue( listCriterio.length > 0 ? listCriterio : null );
                        this.getCriterios( index ).removeAt( jIndex );
                        this.openDialog( '', '<b>La información se ha eliminado correctamente.</b>' );
                    }
                }
            );
    }

    addAportante( index: number, jIndex: number, kIndex: number ) {

        const totalAportantes = 3;

        if ( this.getAportantes( index, jIndex, kIndex ).length < totalAportantes ) {
            this.getAportantes( index, jIndex, kIndex ).push(
                this.fb.group(
                    {
                        tipoAportante: [ null, Validators.required ],
                        nombreAportante: [ null, Validators.required ],
                        fuenteRecursos: [ null, Validators.required ],
                        valorDescuento: [ null, Validators.required ]
                    }
                )
            );
        } else {
            this.openDialog( '', '<b>El contrato no tiene más aportantes asignados.</b>' )
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

    onSubmit() {
      this.estaEditando = true;
      this.addressForm.markAllAsTouched();
      console.log( this.addressForm )
    }

}
