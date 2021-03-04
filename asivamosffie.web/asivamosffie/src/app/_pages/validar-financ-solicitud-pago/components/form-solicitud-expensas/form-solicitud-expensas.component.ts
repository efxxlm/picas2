import { Router } from '@angular/router';
import { Dominio, CommonService } from 'src/app/core/_services/common/common.service';
import { Component, Input, OnInit } from '@angular/core';
import { Validators, FormBuilder } from '@angular/forms';
import { RegistrarRequisitosPagoService } from 'src/app/core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { MatAutocompleteTrigger } from '@angular/material/autocomplete';

@Component({
  selector: 'app-form-solicitud-expensas',
  templateUrl: './form-solicitud-expensas.component.html',
  styleUrls: ['./form-solicitud-expensas.component.scss']
})
export class FormSolicitudExpensasComponent implements OnInit {

    @Input() tipoSolicitud: string;
    @Input() solicitudPago: any;
    @Input() esVerDetalle = false;
    addressForm = this.fb.group({
      llaveMen: [null, Validators.required],
      llaveMenSeleccionada: [ null, Validators.required ],
      numeroRadicadoSAC: [null, Validators.required],
      numeroFactura: [null, Validators.required],
      valorFacturado: [null, Validators.required],
      tipoPago: [null, Validators.required],
      conceptoPagoCriterio: [null, Validators.required],
      valorFacturadoConcepto: [null, Validators.required]
    });
    contratoId: any;
    llavesMenArray: any[] = [];
    tipoPagoArray: Dominio[] = [];
    conceptoPagoCriterioArray: Dominio[] = [];
    solicitudPagoExpensasId = 0;
    solicitudPagoId = 0;

    constructor(
        private fb: FormBuilder,
        private commonSvc: CommonService )
    {
    }

    ngOnInit(): void {
        this.commonSvc.tiposDePagoExpensas()
            .subscribe( tipoPago => {
                this.tipoPagoArray = tipoPago;
                this.commonSvc.conceptosDePagoExpensas()
                    .subscribe( conceptoPago => {
                        this.conceptoPagoCriterioArray = conceptoPago;
                        if ( this.solicitudPago !== undefined ) {
                            this.solicitudPagoId = this.solicitudPago.solicitudPagoId;
                            const solicitudPagoExpensas = this.solicitudPago.solicitudPagoExpensas[0];
                            this.solicitudPagoExpensasId = solicitudPagoExpensas.solicitudPagoExpensasId;
                            this.addressForm.setValue(
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
}
