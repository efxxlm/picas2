import { Router } from '@angular/router';
import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Console } from 'console';
import { ObservacionesMultiplesCuService } from 'src/app/core/_services/observacionesMultiplesCu/observaciones-multiples-cu.service';
import { EstadoSolicitudPagoOrdenGiro, EstadosSolicitudPagoOrdenGiro } from 'src/app/_interfaces/estados-solicitudPago-ordenGiro.interface';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-dialog-envio-autorizacion',
  templateUrl: './dialog-envio-autorizacion.component.html',
  styleUrls: ['./dialog-envio-autorizacion.component.scss']
})
export class DialogEnvioAutorizacionComponent implements OnInit {

    addressForm: FormGroup;
    listaEstadoSolicitudPago: EstadoSolicitudPagoOrdenGiro = EstadosSolicitudPagoOrdenGiro;
    estaEditando = true;

    constructor(
        public matDialogRef: MatDialogRef<DialogEnvioAutorizacionComponent>,
        @Inject( MAT_DIALOG_DATA ) public solicitud: any,
        private fb: FormBuilder,
        private obsMultipleSvc: ObservacionesMultiplesCuService,
        private routes: Router,
        private dialog: MatDialog )
    {
        this.addressForm = this.crearFormulario();
    }

    ngOnInit(): void {
        console.log( this.solicitud )
    }

    crearFormulario() {
        return this.fb.group({
            urlSoporte:[ null, Validators.required ],
        })
    }

    openDialog(modalTitle: string, modalText: string) {
        const dialogRef = this.dialog.open( ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText }
        });
    }

    onSubmit() {
        this.addressForm.markAllAsTouched();
        console.log(this.addressForm.value);

        const pSolicitudPago = {
            solicitudPagoId: this.solicitud.pSolicitudPagoId,
            estadoCodigo: this.listaEstadoSolicitudPago.enviadaAutorizacion,
            solicitudPagoCertificado: [
                {
                    solicitudPagoCertificadoId: 0,
                    solicitudPagoId: this.solicitud.pSolicitudPagoId,
                    url: this.addressForm.get( 'urlSoporte' ).value
                }
            ]
        };

        this.obsMultipleSvc.changueStatusSolicitudPago( pSolicitudPago )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.routes.navigateByUrl( '/', {skipLocationChange: true} )
                        .then( () => this.routes.navigate( ['/verificarSolicitudPago'] )
                    );
                }, err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

}
