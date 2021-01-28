import { RegistrarAvanceSemanalService } from './../../../../core/_services/registrarAvanceSemanal/registrar-avance-semanal.service';
import { Router } from '@angular/router';
import { FormGroup, FormBuilder } from '@angular/forms';
import { Component, Input, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-comite-obra',
  templateUrl: './comite-obra.component.html',
  styleUrls: ['./comite-obra.component.scss']
})
export class ComiteObraComponent implements OnInit {

    @Input() esVerDetalle = false;
    @Input() seguimientoSemanal: any;
    numeroComiteObra: string;
    seguimientoSemanalId: number;
    seguimientoSemanalRegistrarComiteObraId: number;
    gestionComiteObra: any;
    formComiteObra: FormGroup;

    constructor(
        private fb: FormBuilder,
        private dialog: MatDialog,
        private routes: Router,
        private avanceSemanalSvc: RegistrarAvanceSemanalService )
    {
        this.crearFormulario();
    }

    ngOnInit(): void {
        if ( this.seguimientoSemanal !== undefined ) {
            this.seguimientoSemanalId = this.seguimientoSemanal.seguimientoSemanalId;
            this.seguimientoSemanalRegistrarComiteObraId =  this.seguimientoSemanal.seguimientoSemanalRegistrarComiteObra.length > 0 ?
            this.seguimientoSemanal.seguimientoSemanalRegistrarComiteObra[0].seguimientoSemanalRegistrarComiteObraId : 0;
            if ( this.seguimientoSemanal.comiteObraGenerado !== undefined ) {
                this.numeroComiteObra = this.seguimientoSemanal.comiteObraGenerado;
            }

            if ( this.seguimientoSemanal.seguimientoSemanalRegistrarComiteObra.length > 0 ) {
                this.gestionComiteObra = this.seguimientoSemanal.seguimientoSemanalRegistrarComiteObra[0];
                this.numeroComiteObra = this.gestionComiteObra.numeroComite;
                this.formComiteObra.setValue(
                    {
                        fechaComite:    this.gestionComiteObra.fechaComite !== undefined ?
                                        new Date( this.gestionComiteObra.fechaComite ) : null,
                        urlSoporteComite:   this.gestionComiteObra.urlSoporteComite !== undefined ?
                                            this.gestionComiteObra.urlSoporteComite : null
                    }
                );
            }
        }
    }

    crearFormulario() {
        this.formComiteObra = this.fb.group({
            fechaComite: [ null ],
            urlSoporteComite: [ null ]
        });
    }

    openDialog(modalTitle: string, modalText: string) {
        const dialogRef = this.dialog.open(ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText }
        });
    }

    guardar() {
        console.log( this.formComiteObra.value );
        const pSeguimientoSemanal = this.seguimientoSemanal;
        const seguimientoSemanalRegistrarComiteObra = [
            {
                seguimientoSemanalId: this.seguimientoSemanal.seguimientoSemanalId,
                seguimientoSemanalRegistrarComiteObraId: this.seguimientoSemanalRegistrarComiteObraId,
                fechaComite:    this.formComiteObra.get( 'fechaComite' ).value !== null ?
                                new Date( this.formComiteObra.get( 'fechaComite' ).value ).toISOString() : null,
                numeroComite: this.numeroComiteObra,
                urlSoporteComite:   this.formComiteObra.get( 'urlSoporteComite' ).value !== null ?
                                    this.formComiteObra.get( 'urlSoporteComite' ).value : null
            }
        ];
        pSeguimientoSemanal.seguimientoSemanalRegistrarComiteObra = seguimientoSemanalRegistrarComiteObra;
        this.avanceSemanalSvc.saveUpdateSeguimientoSemanal( pSeguimientoSemanal )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                        () =>   this.routes.navigate(
                                    [
                                        '/registrarAvanceSemanal/registroSeguimientoSemanal', this.seguimientoSemanal.contratacionProyectoId
                                    ]
                                )
                    );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

}
