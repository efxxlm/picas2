import { Router } from '@angular/router';
import { OrdenPagoService } from 'src/app/core/_services/ordenPago/orden-pago.service';
import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { Dominio, CommonService } from 'src/app/core/_services/common/common.service';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-form-estrateg-pagos-gog',
  templateUrl: './form-estrateg-pagos-gog.component.html',
  styleUrls: ['./form-estrateg-pagos-gog.component.scss']
})
export class FormEstrategPagosGogComponent implements OnInit {

    @Input() solicitudPago: any;
    ordenGiroId = 0;
    ordenGiroDetalleId = 0;
    estrategiaPagoArray: Dominio[] = [];
    addressForm: FormGroup;

    constructor(
        private fb: FormBuilder,
        private commonSvc: CommonService,
        private ordenPagoSvc: OrdenPagoService,
        private dialog: MatDialog,
        private routes: Router )
    {
        this.commonSvc.listaEstrategiasPago()
            .subscribe( response => this.estrategiaPagoArray = response );
        this.addressForm = this.crearFormulario();
    }

    ngOnInit(): void {
        if ( this.solicitudPago.ordenGiro !== undefined ) {
            this.ordenGiroId = this.solicitudPago.ordenGiro.ordenGiroId;

            if ( this.solicitudPago.ordenGiro.ordenGiroDetalle !== undefined ) {
                this.ordenGiroDetalleId = this.solicitudPago.ordenGiro.ordenGiroDetalle.ordenGiroDetalleId;
            }
        }
    }

    crearFormulario() {
      return this.fb.group({
        ordenGiroDetalleEstrategiaPagoId: [ null ],
        estrategiaPagoCodigo: [null, Validators.required]
      })
    }

    openDialog(modalTitle: string, modalText: string) {
        const dialogRef = this.dialog.open(ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText }
        });
    }

    onSubmit() {

        const pOrdenGiro = {
            solicitudPagoId: this.solicitudPago.solicitudPagoId,
            ordenGiroId: this.ordenGiroId,
            ordenGiroDetalle: {
                ordenGiroDetalleId: this.ordenGiroDetalleId,
                ordenGiroDetalleEstrategiaPago: this.addressForm.value
            }
        }

        this.ordenPagoSvc.createEditOrdenGiro( pOrdenGiro )
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
