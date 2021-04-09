import { Router } from '@angular/router';
import { Component, Input, OnInit } from '@angular/core';
import { Validators, FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActualizarPolizasService } from 'src/app/core/_services/actualizarPolizas/actualizar-polizas.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-observaciones-especificas-rapg',
  templateUrl: './observaciones-especificas-rapg.component.html',
  styleUrls: ['./observaciones-especificas-rapg.component.scss']
})
export class ObservacionesEspecificasRapgComponent implements OnInit {

    @Input() contratoPoliza: any;
    @Input() esVerDetalle: boolean;
    contratoPolizaActualizacion: any;
    addressForm = this.fb.group({
        tieneObservaciones: [null, Validators.required],
        observacionesEspecificas: [null, Validators.required]
    });
    editorStyle = {
        height: '50px'
    };
    config = {
        toolbar: [
            ['bold', 'italic', 'underline'],
            [{ list: 'ordered' }, { list: 'bullet' }],
            [{ indent: '-1' }, { indent: '+1' }],
            [{ align: [] }],
        ]
    };
    estaEditando = false;

    constructor(
        private fb: FormBuilder,
        private dialog: MatDialog,
        private routes: Router,
        private actualizarPolizaSvc: ActualizarPolizasService )
    { }

    ngOnInit(): void {
        if ( this.contratoPoliza.contratoPolizaActualizacion !== undefined ) {
            if ( this.contratoPoliza.contratoPolizaActualizacion.length > 0 ) {
                this.contratoPolizaActualizacion = this.contratoPoliza.contratoPolizaActualizacion[ 0 ];

                this.addressForm.setValue(
                    {
                        tieneObservaciones: this.contratoPolizaActualizacion.tieneObservacionEspecifica !== undefined ? this.contratoPolizaActualizacion.tieneObservacionEspecifica : null,
                        observacionesEspecificas: this.contratoPolizaActualizacion.observacionEspecifica !== undefined ? this.contratoPolizaActualizacion.observacionEspecifica : null
                    }
                )
            }
        }
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

    openDialog( modalTitle: string, modalText: string ) {
        this.dialog.open(ModalDialogComponent, {
            width: '28em',
            data: { modalTitle, modalText }
        });
    }

    onSubmit() {
        if ( this.addressForm.get( 'tieneObservaciones' ).value === false && this.addressForm.get( 'observacionesEspecificas' ).value !== null ) {
            this.addressForm.get( 'observacionesEspecificas' ).setValue( '' );
        }

        this.estaEditando = true;
        this.contratoPolizaActualizacion.tieneObservacionEspecifica = this.addressForm.get( 'tieneObservaciones' ).value;
        this.contratoPolizaActualizacion.observacionEspecifica = this.addressForm.get( 'observacionesEspecificas' ).value;

        this.actualizarPolizaSvc.createorUpdateCofinancing( this.contratoPolizaActualizacion )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                        () => this.routes.navigate(
                            [
                                '/registrarActualizacionesPolizasYGarantias/verDetalleEditarPoliza', this.contratoPoliza.contratoPolizaId
                            ]
                        )
                    );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

}
