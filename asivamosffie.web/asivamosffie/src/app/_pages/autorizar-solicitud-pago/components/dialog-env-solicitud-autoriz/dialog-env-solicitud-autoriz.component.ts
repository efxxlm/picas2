import { Router } from '@angular/router';
import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ObservacionesMultiplesCuService } from 'src/app/core/_services/observacionesMultiplesCu/observaciones-multiples-cu.service';
import { EstadoSolicitudPagoOrdenGiro, EstadosSolicitudPagoOrdenGiro } from 'src/app/_interfaces/estados-solicitudPago-ordenGiro.interface';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-dialog-env-solicitud-autoriz',
  templateUrl: './dialog-env-solicitud-autoriz.component.html',
  styleUrls: ['./dialog-env-solicitud-autoriz.component.scss']
})
export class DialogEnvSolicitudAutorizComponent implements OnInit {

    addressForm: FormGroup;
    listaEstadoSolicitudPago: EstadoSolicitudPagoOrdenGiro = EstadosSolicitudPagoOrdenGiro;
    estaEditando = true;

    constructor(
        private fb: FormBuilder,
        public matDialogRef: MatDialogRef<DialogEnvSolicitudAutorizComponent>,
        private obsMultipleSvc: ObservacionesMultiplesCuService,
        private routes: Router,
        private dialog: MatDialog,
        @Inject(MAT_DIALOG_DATA) public solicitud: any )
    {
        this.addressForm = this.crearFormulario();
    }

    ngOnInit(): void {
        console.log( this.solicitud );
    }

    crearFormulario() {
        return this.fb.group({
            fechaRadicacionSAC: [ null, Validators.required ],
            numeroRadicacionSAC:[ null, Validators.required ],
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
            solicitudPagoId: this.solicitud.solicitudPagoId,
            estadoCodigo: this.listaEstadoSolicitudPago.enviadaVerificacionFinanciera,
            fechaRadicacionSacFinanciera: new Date( this.addressForm.get( 'fechaRadicacionSAC' ).value ).toISOString(),
            numeroRadicacionSacFinanciera: this.addressForm.get( 'numeroRadicacionSAC' ).value,
            urlSoporteFinanciera: this.addressForm.get( 'urlSoporte' ).value
        };

        this.obsMultipleSvc.changueStatusSolicitudPago( pSolicitudPago )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.routes.navigateByUrl( '/', {skipLocationChange: true} )
                        .then( () => this.routes.navigate( ['/autorizarSolicitudPago'] ) );
                    this.matDialogRef.close();
                }, err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

}
