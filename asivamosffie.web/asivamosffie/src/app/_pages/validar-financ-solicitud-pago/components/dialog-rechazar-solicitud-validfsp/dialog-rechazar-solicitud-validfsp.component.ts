import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ObservacionesMultiplesCuService } from 'src/app/core/_services/observacionesMultiplesCu/observaciones-multiples-cu.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { EstadoSolicitudPagoOrdenGiro, EstadosSolicitudPagoOrdenGiro } from 'src/app/_interfaces/estados-solicitudPago-ordenGiro.interface';

@Component({
  selector: 'app-dialog-rechazar-solicitud-validfsp',
  templateUrl: './dialog-rechazar-solicitud-validfsp.component.html',
  styleUrls: ['./dialog-rechazar-solicitud-validfsp.component.scss']
})
export class DialogRechazarSolicitudValidfspComponent implements OnInit {

    estaEditando = false;
    addressForm: FormGroup;
    listaEstadoSolicitudPago: EstadoSolicitudPagoOrdenGiro = EstadosSolicitudPagoOrdenGiro;
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
        public matDialogRef: MatDialogRef<DialogRechazarSolicitudValidfspComponent>,
        @Inject(MAT_DIALOG_DATA) public registro: any,
        private dialog: MatDialog,
        private routes: Router,
        private obsMultipleSvc: ObservacionesMultiplesCuService,
        private fb: FormBuilder )
    {
        this.addressForm = this.crearFormulario();
    }

    ngOnInit(): void {
    }

    crearFormulario() {
      return this.fb.group({
        fechaRadicacionSAC: [null, Validators.required],
        numeroRadicacionSAC: [null, Validators.required],
        observaciones: [null, Validators.required]
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
        const dialogRef = this.dialog.open( ModalDialogComponent, {
            width: '28em',
            data: { modalTitle, modalText }
        });
    }

    onSubmit() {
        //De forma maquetada
        this.estaEditando = true;
        this.addressForm.markAllAsTouched();

        const pSolicitudPago = {
            solicitudPagoId: this.registro.solicitudPagoId,
            estadoCodigo: this.listaEstadoSolicitudPago.solicitudRechazadaPorValidacionFinanciera,
            fechaRadicacionSacTecnica: this.addressForm.get( 'fechaRadicacionSAC' ).value !== null ? new Date( this.addressForm.get( 'fechaRadicacionSAC' ).value ).toISOString() : this.addressForm.get( 'fechaRadicacionSAC' ).value,
            numeroRadicacionSacTecnica: this.addressForm.get( 'numeroRadicacionSAC' ).value,
            observacionRadicacionSacTecnica: this.addressForm.get( 'observaciones' ).value
        };

        this.obsMultipleSvc.changueStatusSolicitudPago( pSolicitudPago )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.routes.navigateByUrl( '/', {skipLocationChange: true} )
                        .then( () => this.routes.navigate( ['/validarFinancieramenteSolicitudDePago'] ) );
                }, err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

}
