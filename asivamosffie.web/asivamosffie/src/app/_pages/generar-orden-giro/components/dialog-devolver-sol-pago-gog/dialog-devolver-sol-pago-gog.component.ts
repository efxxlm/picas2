import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ObservacionesMultiplesCuService } from 'src/app/core/_services/observacionesMultiplesCu/observaciones-multiples-cu.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { EstadoSolicitudPagoOrdenGiro, EstadosSolicitudPagoOrdenGiro } from 'src/app/_interfaces/estados-solicitudPago-ordenGiro.interface';

@Component({
  selector: 'app-dialog-devolver-sol-pago-gog',
  templateUrl: './dialog-devolver-sol-pago-gog.component.html',
  styleUrls: ['./dialog-devolver-sol-pago-gog.component.scss']
})
export class DialogDevolverSolPagoGogComponent implements OnInit {

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
    estaEditando = true;

    constructor(
        private fb: FormBuilder,
        private routes: Router,
        private dialog: MatDialog,
        private obsMultipleSvc: ObservacionesMultiplesCuService,
        public matDialogRef: MatDialogRef<DialogDevolverSolPagoGogComponent>,
        @Inject(MAT_DIALOG_DATA) public registro: any )
    {
        this.addressForm = this.crearFormulario();
    }

    ngOnInit(): void {
    }

    crearFormulario() {
      return this.fb.group({
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
        const dialogRef = this.dialog.open( ModalDialogComponent, {
            width: '28em',
            data: { modalTitle, modalText }
        });
    }

    onSubmit() {
        const pSolicitudPago = {
            solicitudPagoId: this.registro.registro.solicitudPagoId,
            estadoCodigo: this.registro.esAnular === true ? this.listaEstadoSolicitudPago.ordenGiroAnulada : this.listaEstadoSolicitudPago.solicitudDevueltaPorGenerarOrdenGiroParaEquipoFacturacion,
            observacionDevolucionOrdenGiro: this.addressForm.get( 'observaciones' ).value
        };

        this.obsMultipleSvc.changueStatusSolicitudPago( pSolicitudPago )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.matDialogRef.close();

                    this.routes.navigateByUrl( '/', {skipLocationChange: true} )
                        .then( () => this.routes.navigate( ['/generarOrdenDeGiro'] ) );
                }, err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

}
