import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { OrdenPagoService } from 'src/app/core/_services/ordenPago/orden-pago.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-soporte-orden-giro-gog',
  templateUrl: './soporte-orden-giro-gog.component.html',
  styleUrls: ['./soporte-orden-giro-gog.component.scss']
})
export class SoporteOrdenGiroGogComponent implements OnInit {

    @Input() solicitudPago: any;
    ordenGiroId = 0;
    ordenGiroDetalleId = 0;
    ordenGiroSoporteId = 0;
    ordenGiroDetalle: any;
    addressForm: FormGroup;
    estaEditando = false;
    constructor(
        private fb: FormBuilder,
        private commonSvc: CommonService,
        private ordenPagoSvc: OrdenPagoService,
        private dialog: MatDialog,
        private routes: Router )
    {
        this.addressForm = this.crearFormulario();
    }

    ngOnInit(): void {
        if ( this.solicitudPago.ordenGiro !== undefined ) {
            this.ordenGiroId = this.solicitudPago.ordenGiro.ordenGiroId;
            
            if ( this.solicitudPago.ordenGiro.ordenGiroDetalle !== undefined ) {
                if ( this.solicitudPago.ordenGiro.ordenGiroDetalle.length > 0 ) {
                    this.ordenGiroDetalle = this.solicitudPago.ordenGiro.ordenGiroDetalle[0];
                    this.ordenGiroDetalleId = this.solicitudPago.ordenGiro.ordenGiroDetalle.ordenGiroDetalleId;
    
                    if ( this.ordenGiroDetalle.ordenGiroSoporte !== undefined ) {
                        if ( this.ordenGiroDetalle.ordenGiroSoporte.length > 0 ) {
                            this.ordenGiroSoporteId = this.ordenGiroDetalle.ordenGiroSoporte[0].ordenGiroSoporteId;
    
                            this.addressForm.setValue( { urlSoporte: this.ordenGiroDetalle.ordenGiroSoporte[0].urlSoporte !== undefined ? this.ordenGiroDetalle.ordenGiroSoporte[0].urlSoporte : null } )
                        }
                    }
                }
            }
        }
    }

    crearFormulario() {
      return this.fb.group({
        urlSoporte:[ null, Validators.required ]
      })
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
        const pOrdenGiro = {
            solicitudPagoId: this.solicitudPago.solicitudPagoId,
            ordenGiroId: this.ordenGiroId,
            ordenGiroDetalle: [
                {
                    ordenGiroId: this.ordenGiroId,
                    ordenGiroDetalleId: this.ordenGiroDetalleId,
                    ordenGiroSoporte: [
                        {
                            ordenGiroDetalleId: this.ordenGiroDetalleId,
                            ordenGiroSoporteId: this.ordenGiroSoporteId,
                            urlSoporte: this.addressForm.get( 'urlSoporte' ).value
                        }
                    ]
                }
            ]
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
