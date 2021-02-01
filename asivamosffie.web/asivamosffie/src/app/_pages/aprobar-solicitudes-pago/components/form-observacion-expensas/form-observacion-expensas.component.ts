import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';

@Component({
  selector: 'app-form-observacion-expensas',
  templateUrl: './form-observacion-expensas.component.html',
  styleUrls: ['./form-observacion-expensas.component.scss']
})
export class FormObservacionExpensasComponent implements OnInit {

    solicitudPago: any;
    tipoPagoArray: Dominio[] = [];
    conceptoPagoCriterioArray: Dominio[] = [];
    addressForm: FormGroup;
    detalleForm = this.fb.group({
        llaveMen: [null, Validators.required],
        llaveMenSeleccionada: [ null, Validators.required ],
        numeroRadicadoSAC: [null, Validators.required],
        numeroFactura: [null, Validators.required],
        valorFacturado: [null, Validators.required],
        tipoPago: [null, Validators.required],
        conceptoPagoCriterio: [null, Validators.required],
        valorFacturadoConcepto: [null, Validators.required]
    });
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

    constructor(
        private fb: FormBuilder,
        private commonSvc: CommonService )
    {
        this.addressForm = this.crearFormulario();
        this.commonSvc.tiposDePagoExpensas()
            .subscribe( tipoPago => {
                this.tipoPagoArray = tipoPago;
                this.commonSvc.conceptosDePagoExpensas()
                    .subscribe( conceptoPago => {
                        this.conceptoPagoCriterioArray = conceptoPago;
                        if ( this.solicitudPago !== undefined ) {
                            const solicitudPagoExpensas = this.solicitudPago.solicitudPagoExpensas[0];
                            this.detalleForm.setValue(
                                {
                                    llaveMen: this.solicitudPago.contratacionProyecto.proyecto.llaveMen,
                                    llaveMenSeleccionada: {
                                        contratacionProyectoId: this.solicitudPago.contratacionProyectoId,
                                        llaveMen: this.solicitudPago.contratacionProyecto.proyecto.llaveMen
                                    },
                                    numeroRadicadoSAC: solicitudPagoExpensas.numeroRadicadoSac !== undefined ? solicitudPagoExpensas.numeroRadicadoSac : null,
                                    numeroFactura: solicitudPagoExpensas.numeroFactura !== undefined ? solicitudPagoExpensas.numeroFactura : null,
                                    valorFacturado: solicitudPagoExpensas.valorFacturado !== undefined ? solicitudPagoExpensas.valorFacturado : null,
                                    tipoPago: solicitudPagoExpensas.tipoPagoCodigo !== undefined ? this.tipoPagoArray.filter( tipoPago => tipoPago.codigo === solicitudPagoExpensas.tipoPagoCodigo )[0] : null,
                                    conceptoPagoCriterio: solicitudPagoExpensas.conceptoPagoCriterioCodigo !== undefined ? this.conceptoPagoCriterioArray.filter( conceptoPago => conceptoPago.codigo === solicitudPagoExpensas.conceptoPagoCriterioCodigo )[0] : null,
                                    valorFacturadoConcepto: solicitudPagoExpensas.valorFacturadoConcepto !== undefined ? solicitudPagoExpensas.valorFacturadoConcepto : null
                                }
                            );
                        }
                    } );
            } );
    }

    ngOnInit(): void {
    }

    crearFormulario() {
        return this.fb.group({
          tieneObservaciones: [null, Validators.required],
          observaciones:[null, Validators.required]
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

    onSubmit() {
        console.log(this.addressForm.value);
    }

}
