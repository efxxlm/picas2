import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { ObservacionesMultiplesCuService } from 'src/app/core/_services/observacionesMultiplesCu/observaciones-multiples-cu.service';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-form-observacion-expensas',
  templateUrl: './form-observacion-expensas.component.html',
  styleUrls: ['./form-observacion-expensas.component.scss']
})
export class FormObservacionExpensasComponent implements OnInit {

    solicitudPago: any;
    solicitudPagoObservacionId = 0;
    menusIdPath: any; // Se obtienen los ID de los respectivos PATH de cada caso de uso que se implementaran observaciones.
    listaTipoObservacionSolicitudes: any; // Interfaz lista tipos de observaciones.
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
        private dialog: MatDialog,
        private obsMultipleSvc: ObservacionesMultiplesCuService,
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

    openDialog(modalTitle: string, modalText: string) {
        const dialogRef = this.dialog.open(ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText }
        });
    }

    onSubmit() {
        const pSolicitudPagoObservacion = {
            solicitudPagoObservacionId: this.solicitudPagoObservacionId,
            solicitudPagoId: this.solicitudPago.solicitudPagoId,
            observacion: this.addressForm.get( 'observaciones' ).value !== null ? this.addressForm.get( 'observaciones' ).value : this.addressForm.get( 'observaciones' ).value,
            tipoObservacionCodigo: this.listaTipoObservacionSolicitudes.expensasCodigo,
            menuId: this.menusIdPath.autorizarSolicitudPagoId,
            idPadre: this.solicitudPago.solicitudPagoExpensas[0].solicitudPagoExpensasId,
            tieneObservacion: this.addressForm.get( 'tieneObservaciones' ).value !== null ? this.addressForm.get( 'tieneObservaciones' ).value : this.addressForm.get( 'tieneObservaciones' ).value
        };

        console.log( pSolicitudPagoObservacion );
        this.obsMultipleSvc.createUpdateSolicitudPagoObservacion( pSolicitudPagoObservacion )
            .subscribe(
                response => this.openDialog( '', `<b>${ response.message }</b>` ),
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            )
    }

}
