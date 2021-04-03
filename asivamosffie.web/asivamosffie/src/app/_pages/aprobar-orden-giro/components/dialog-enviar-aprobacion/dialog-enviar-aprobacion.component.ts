import { Component, Inject, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { OrdenPagoService } from 'src/app/core/_services/ordenPago/orden-pago.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { EstadoSolicitudPagoOrdenGiro, EstadosSolicitudPagoOrdenGiro } from 'src/app/_interfaces/estados-solicitudPago-ordenGiro.interface';

@Component({
  selector: 'app-dialog-enviar-aprobacion',
  templateUrl: './dialog-enviar-aprobacion.component.html',
  styleUrls: ['./dialog-enviar-aprobacion.component.scss']
})
export class DialogEnviarAprobacionComponent implements OnInit {

    formUrl: FormGroup = this.fb.group( { urlSoporte: [ null, Validators.required ] } );
    estadoSolicitudPagoOrdenGiro: EstadoSolicitudPagoOrdenGiro = EstadosSolicitudPagoOrdenGiro;

    constructor(
        private fb: FormBuilder,
        private dialog: MatDialog,
        private routes: Router,
        private ordenGiroSvc: OrdenPagoService,
        private matDialogRef: MatDialogRef<DialogEnviarAprobacionComponent>,
        @Inject(MAT_DIALOG_DATA) public data )
    { }

    ngOnInit(): void {
    }

    openDialog( modalTitle: string, modalText: string ) {
        this.dialog.open( ModalDialogComponent, {
            width: '40em',
            data : { modalTitle, modalText }
        });
    }

    guardar() {
        const pOrdenGiro = {
            ordenGiroId: this.data.ordenGiroId,
            estadoCodigo: EstadosSolicitudPagoOrdenGiro.enviadaParaTramiteFiduciaria,
            urlSoporteFirmadoAprobar: this.formUrl.get( 'urlSoporte' ).value
        }

        this.ordenGiroSvc.changueStatusOrdenGiro( pOrdenGiro )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.matDialogRef.close();
                    this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                        () => this.routes.navigate( [ '/aprobarOrdenGiro' ] )
                    );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

}
