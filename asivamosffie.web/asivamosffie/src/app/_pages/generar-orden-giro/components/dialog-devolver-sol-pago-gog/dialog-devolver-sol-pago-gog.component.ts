import { Dominio } from './../../../../core/_services/common/common.service';
import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ObservacionesMultiplesCuService } from 'src/app/core/_services/observacionesMultiplesCu/observaciones-multiples-cu.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { DevolucionesCodigo, EstadoSolicitudPagoOrdenGiro, EstadosSolicitudPagoOrdenGiro } from 'src/app/_interfaces/estados-solicitudPago-ordenGiro.interface';

@Component({
  selector: 'app-dialog-devolver-sol-pago-gog',
  templateUrl: './dialog-devolver-sol-pago-gog.component.html',
  styleUrls: ['./dialog-devolver-sol-pago-gog.component.scss']
})
export class DialogDevolverSolPagoGogComponent implements OnInit {

    addressForm: FormGroup;
    listaEstadoSolicitudPago: EstadoSolicitudPagoOrdenGiro = EstadosSolicitudPagoOrdenGiro;
    devolucionesCodigo = DevolucionesCodigo;
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
    listaDevoluciones: any[] = [
        { nombre: 'Registrar y validar requisitos de pago', codigo: 1 },
        { nombre: 'Verificar financieramente solicitud de pago', codigo: 2 },
        { nombre: 'Validar financieramente solicitud de pago', codigo: 3 }
    ]

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
            usuarioADevolver: [ null ],
            observaciones: [ null, Validators.required ]
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
            estadoCodigo: '',
            observacionDevolucionOrdenGiro: this.addressForm.get( 'observaciones' ).value
        };

        if ( this.registro.esAnular === true ) {
            pSolicitudPago.estadoCodigo = this.listaEstadoSolicitudPago.ordenGiroAnulada
        }
        if ( this.registro.esAnular === false ) {
            if ( this.addressForm.get( 'usuarioADevolver' ).value !== null ) {
                if ( this.addressForm.get( 'usuarioADevolver' ).value === this.devolucionesCodigo.solicitudPago ) {
                    pSolicitudPago.estadoCodigo = this.listaEstadoSolicitudPago.solicitudDevueltaPorGenerarOrdenGiroParaEquipoFacturacion
                }

                if ( this.addressForm.get( 'usuarioADevolver' ).value === this.devolucionesCodigo.verificarFinancieramente ) {
                    pSolicitudPago.estadoCodigo = this.listaEstadoSolicitudPago.enProcesoVerificacionFinanciera
                }

                if ( this.addressForm.get( 'usuarioADevolver' ).value === this.devolucionesCodigo.validarFinancieramente ) {
                    pSolicitudPago.estadoCodigo = this.listaEstadoSolicitudPago.enProcesoValidacionFinanciera
                }
            } else {
                return;
            }
        }

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
