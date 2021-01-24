import { Router } from '@angular/router';
import { Dominio, CommonService } from 'src/app/core/_services/common/common.service';
import { Component, Input, OnInit } from '@angular/core';
import { Validators, FormBuilder } from '@angular/forms';
import { RegistrarRequisitosPagoService } from 'src/app/core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-form-solicitud-expensas',
  templateUrl: './form-solicitud-expensas.component.html',
  styleUrls: ['./form-solicitud-expensas.component.scss']
})
export class FormSolicitudExpensasComponent implements OnInit {

    @Input() tipoSolicitud: string;
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
        private dialog: MatDialog,
        private routes: Router,
        private commonSvc: CommonService,
        private registrarPagosSvc: RegistrarRequisitosPagoService )
    {
        this.commonSvc.tiposDePagoExpensas()
            .subscribe( response => this.tipoPagoArray = response );
        this.commonSvc.conceptosDePagoExpensas()
            .subscribe( response => this.conceptoPagoCriterioArray = response );

    }

    ngOnInit(): void {
    }

    seleccionAutocomplete( llaveMen ){
        this.addressForm.get( 'llaveMenSeleccionada' ).setValue( llaveMen );
    }

    getLlaveMen() {
        if ( this.addressForm.get( 'llaveMen' ).value !== null ) {
            if ( this.addressForm.get( 'llaveMen' ).value.length > 0 ) {
                this.registrarPagosSvc.getListProyectosByLlaveMen( this.addressForm.get( 'llaveMen' ).value )
                    .subscribe(
                        response => {
                            this.llavesMenArray = response;
                            console.log( 'respuesta', response );
                        }
                    );
            }
        }
    }

    validateNumberKeypress(event: KeyboardEvent) {
      const alphanumeric = /[0-9]/;
      const inputChar = String.fromCharCode(event.charCode);
      return alphanumeric.test(inputChar) ? true : false;
    }

    openDialog(modalTitle: string, modalText: string) {
        const dialogRef = this.dialog.open(ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText }
        });
    }

    onSubmit() {
        console.log( this.addressForm.value );
        const pSolicitudPago =  {
            solicitudPagoId: this.solicitudPagoId,
            tipoSolicitudCodigo: this.tipoSolicitud,
            contratacionProyectoId: this.addressForm.get( 'llaveMenSeleccionada' ).value.contratacionProyectoId,
            solicitudPagoExpensas: [
                {
                    solicitudPagoExpensasId: this.solicitudPagoExpensasId,
                    solicitudPagoId: this.solicitudPagoId,
                    numeroRadicadoSac: Number( this.addressForm.get( 'numeroRadicadoSAC' ).value ),
                    numeroFactura: Number( this.addressForm.get( 'numeroFactura' ).value ),
                    valorFacturado: this.addressForm.get( 'valorFacturado' ).value,
                    tipoPagoCodigo: this.addressForm.get( 'tipoPago' ).value !== null ? this.addressForm.get( 'tipoPago' ).value.codigo : this.addressForm.get( 'tipoPago' ).value,
                    conceptoPagoCriterioCodigo: this.addressForm.get( 'conceptoPagoCriterio' ).value !== null ? this.addressForm.get( 'conceptoPagoCriterio' ).value.codigo : this.addressForm.get( 'conceptoPagoCriterio' ).value,
                    valorFacturadoConcepto: this.addressForm.get( 'valorFacturadoConcepto' ).value
                }
            ]
        };
        console.log( pSolicitudPago );
        this.registrarPagosSvc.createEditExpensas( pSolicitudPago )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                        () => this.routes.navigate(
                            [
                                '/registrarValidarRequisitosPago/verDetalleEditar', this.addressForm.get( 'llaveMenSeleccionada' ).value.contratacionProyectoId
                            ]
                        )
                    );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }
}
