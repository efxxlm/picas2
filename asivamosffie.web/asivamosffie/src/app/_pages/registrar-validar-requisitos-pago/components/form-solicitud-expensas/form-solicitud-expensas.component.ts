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
    estaEditando = false;
    constructor(
        private fb: FormBuilder,
        private dialog: MatDialog,
        private routes: Router,
        private commonSvc: CommonService,
        private registrarPagosSvc: RegistrarRequisitosPagoService )
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
                            this.estaEditando = true;
                            this.addressForm.markAllAsTouched();
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

    seleccionAutocomplete( llaveMen ){
        this.addressForm.get( 'llaveMenSeleccionada' ).setValue( llaveMen );
    }

    getLlaveMen( trigger: MatAutocompleteTrigger ) {
        if ( this.addressForm.get( 'llaveMen' ).value !== null ) {
            if ( this.addressForm.get( 'llaveMen' ).value.length > 0 ) {
                this.registrarPagosSvc.getListProyectosByLlaveMen( this.addressForm.get( 'llaveMen' ).value )
                    .subscribe(
                        response => {
                            this.llavesMenArray = response;
                            if ( response.length === 0 ) {
                                this.openDialog( '', '<b>No se encontro una Llave Men relacionada en la busqueda.</b>' );
                            } else {
                                trigger.openPanel();
                            }
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
        this.estaEditando = true;
        this.addressForm.markAllAsTouched();
        
        if ( this.addressForm.get( 'llaveMenSeleccionada' ).value === null ) {
            this.openDialog( '', '<b>Debe seleccionar una llave MEN valida</b>' );
            return;
        }

        const pSolicitudPago =  {
            solicitudPagoId: this.solicitudPagoId,
            tipoSolicitudCodigo: this.tipoSolicitud,
            contratacionProyectoId: this.addressForm.get( 'llaveMenSeleccionada' ).value.contratacionProyectoId,
            solicitudPagoExpensas: [
                {
                    solicitudPagoExpensasId: this.solicitudPagoExpensasId,
                    solicitudPagoId: this.solicitudPagoId,
                    numeroRadicadoSac: this.addressForm.get( 'numeroRadicadoSAC' ).value,
                    numeroFactura: this.addressForm.get( 'numeroFactura' ).value,
                    valorFacturado: this.addressForm.get( 'valorFacturado' ).value,
                    tipoPagoCodigo: this.addressForm.get( 'tipoPago' ).value !== null ? this.addressForm.get( 'tipoPago' ).value.codigo : this.addressForm.get( 'tipoPago' ).value,
                    conceptoPagoCriterioCodigo: this.addressForm.get( 'conceptoPagoCriterio' ).value !== null ? this.addressForm.get( 'conceptoPagoCriterio' ).value.codigo : this.addressForm.get( 'conceptoPagoCriterio' ).value,
                    valorFacturadoConcepto: this.addressForm.get( 'valorFacturadoConcepto' ).value
                }
            ]
        };
        this.registrarPagosSvc.createEditExpensas( pSolicitudPago )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    if ( this.solicitudPagoId > 0 ) {
                        this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                            () => this.routes.navigate(
                                [
                                    '/registrarValidarRequisitosPago/verDetalleEditarExpensas', this.solicitudPagoId
                                ]
                            )
                        );
                    } else {
                        this.routes.navigateByUrl( '/', {skipLocationChange: true} )
                            .then( () => this.routes.navigate( [ '/registrarValidarRequisitosPago' ] ) );
                    }
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }
}
