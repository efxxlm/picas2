import { Component, Input, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { RegistrarRequisitosPagoService } from 'src/app/core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-obs-criterio-pagos-autoriz',
  templateUrl: './obs-criterio-pagos-autoriz.component.html',
  styleUrls: ['./obs-criterio-pagos-autoriz.component.scss']
})
export class ObsCriterioPagosAutorizComponent implements OnInit {

    @Input() solicitudPago: any;
    @Input() esVerDetalle = false;
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
        private registrarPagosSvc: RegistrarRequisitosPagoService,
        private dialog: MatDialog,
        private commonSvc: CommonService )
    {
        this.addressForm = this.crearFormulario();  
    }

    ngOnInit(): void {
        this.getCriterios();
    }

    getCriterios() {
        if ( this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase[0].esPreconstruccion === true ) {
            this.solicitudPagoFase = this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase[0];
            const fasePreConstruccionFormaPagoCodigo = this.solicitudPago.solicitudPagoCargarFormaPago[0].fasePreConstruccionFormaPagoCodigo;
            this.registrarPagosSvc.getCriterioByFormaPagoCodigo( fasePreConstruccionFormaPagoCodigo )
                .subscribe(
                    response => {
                        this.listaCriterios = response;
                        this.solicitudPagoFase.solicitudPagoFaseCriterio.forEach( criterio => {
                            this.criteriosArraySeleccionados.push( this.listaCriterios.filter( criterioValue => criterioValue.codigo === criterio.tipoCriterioCodigo )[0] );
                        } );
                        const criteriosArray = [];

                        if ( this.solicitudPagoFase.solicitudPagoFaseCriterio.length > 0 ) {
                            this.solicitudPagoFase.solicitudPagoFaseCriterio.forEach( async criterio => {
                                // GET Criterio seleccionado
                                const criterioSeleccionado = response.filter( value => value.codigo === criterio.tipoCriterioCodigo );
                                criteriosArray.push( criterioSeleccionado[0] );
                                // GET tipos de pago
                                const tiposDePago = await this.registrarPagosSvc.getTipoPagoByCriterioCodigo( criterio.tipoCriterioCodigo );
                                const tipoDePago = tiposDePago.filter( value => value.codigo === criterio.tipoPagoCodigo );
                                // GET conceptos de pago
                                const conceptosDePago = await this.registrarPagosSvc.getConceptoPagoCriterioCodigoByTipoPagoCodigo( criterio.tipoPagoCodigo );
                                const conceptoDePago = conceptosDePago.filter( value => value.codigo === criterio.conceptoPagoCriterio );
                                this.criterios.push(
                                    this.fb.group(
                                        {
                                            solicitudPagoFaseId: [ this.solicitudPagoFase.solicitudPagoFaseId ],
                                            solicitudPagoFaseCriterioId: [ criterio.solicitudPagoFaseCriterioId ],
                                            tipoCriterioCodigo: [ criterio.tipoCriterioCodigo ],
                                            nombreCriterio: [ criterioSeleccionado[0].nombre ],
                                            tiposDePago: [ tiposDePago ],
                                            tipoPago: [ tipoDePago.length > 0 ? tipoDePago[0] : null ],
                                            conceptosDePago: [ conceptosDePago ],
                                            conceptoPago: [ conceptoDePago.length > 0 ? conceptoDePago[0] : null ],
                                            valorFacturado: [ criterio.valorFacturado !== undefined ? criterio.valorFacturado : null ],
                                            tieneObservaciones: [null, Validators.required],
                                            observaciones:[null, Validators.required]
                                        }
                                    )
                                );
                            } );
                        }
                    }
                );
        }
        if ( this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase[0].esPreconstruccion === false ) {
            this.solicitudPagoFase = this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase[0];
            const faseConstruccionFormaPagoCodigo = this.solicitudPago.solicitudPagoCargarFormaPago[0].faseConstruccionFormaPagoCodigo;
            this.registrarPagosSvc.getCriterioByFormaPagoCodigo( faseConstruccionFormaPagoCodigo )
                .subscribe(
                    response => {
                        this.listaCriterios = response;
                        this.solicitudPagoFase.solicitudPagoFaseCriterio.forEach( criterio => {
                            this.criteriosArraySeleccionados.push( this.listaCriterios.filter( criterioValue => criterioValue.codigo === criterio.tipoCriterioCodigo )[0] );
                        } );
                        const criteriosArray = [];
                        if ( this.solicitudPagoFase.solicitudPagoFaseCriterio.length > 0 ) {
                            this.solicitudPagoFase.solicitudPagoFaseCriterio.forEach( async criterio => {
                                // GET Criterio seleccionado
                                const criterioSeleccionado = response.filter( value => value.codigo === criterio.tipoCriterioCodigo );
                                criteriosArray.push( criterioSeleccionado[0] );
                                // GET tipos de pago
                                const tiposDePago = await this.registrarPagosSvc.getTipoPagoByCriterioCodigo( criterio.tipoCriterioCodigo );
                                const tipoDePago = tiposDePago.filter( value => value.codigo === criterio.tipoPagoCodigo );
                                // GET conceptos de pago
                                const conceptosDePago = await this.registrarPagosSvc.getConceptoPagoCriterioCodigoByTipoPagoCodigo( criterio.tipoPagoCodigo );
                                const conceptoDePago = conceptosDePago.filter( value => value.codigo === criterio.conceptoPagoCriterio );
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
                                            conceptoPago: [ conceptoDePago[0], Validators.required ],
                                            valorFacturado: [ criterio.valorFacturado, Validators.required ]
                                        }
                                    )
                                );
                            } );
                        }
                    }
                );
        }
    }

    crearFormulario() {
        return this.fb.group({
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

    filterCriterios( tipoCriterioCodigo: string ) {
        if ( this.listaCriterios.length > 0 ) {
            const criterio = this.listaCriterios.filter( criterio => criterio.codigo === tipoCriterioCodigo );
            return criterio[0].nombre;
        }
    }

    openDialog(modalTitle: string, modalText: string) {
        const dialogRef = this.dialog.open(ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText }
        });
    }

    onSubmit() {
      console.log(this.addressForm.value);
    }

}
