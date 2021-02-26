import { RegistrarAvanceSemanalService } from './../../../../core/_services/registrarAvanceSemanal/registrar-avance-semanal.service';
import { Router } from '@angular/router';
import { Component, Input, OnInit } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-registro-fotografico',
  templateUrl: './registro-fotografico.component.html',
  styleUrls: ['./registro-fotografico.component.scss']
})
export class RegistroFotograficoComponent implements OnInit {

    @Input() esVerDetalle = false;
    @Input() seguimientoSemanal: any;
    verAyuda = false;
    formRegistroFotografico: FormGroup;
    seguimientoSemanalId: number;
    seguimientoSemanalRegistroFotograficoId: number;
    reporteFotografico: any;
    editorStyle = {
        height: '45px'
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
        private dialog: MatDialog,
        private fb: FormBuilder,
        private routes: Router,
        private avanceSemanalSvc: RegistrarAvanceSemanalService )
    {
        this.crearFormulario();
    }

    ngOnInit(): void {
        if ( this.seguimientoSemanal !== undefined ) {
            this.seguimientoSemanalId = this.seguimientoSemanal.seguimientoSemanalId;
            this.seguimientoSemanalRegistroFotograficoId =  this.seguimientoSemanal.seguimientoSemanalRegistroFotografico.length > 0 ?
            this.seguimientoSemanal.seguimientoSemanalRegistroFotografico[0].seguimientoSemanalRegistroFotograficoId : 0;

            if ( this.seguimientoSemanal.seguimientoSemanalRegistroFotografico.length > 0 ) {
                this.reporteFotografico = this.seguimientoSemanal.seguimientoSemanalRegistroFotografico[0];
                this.formRegistroFotografico.setValue(
                    {
                        urlSoporteFotografico:  this.reporteFotografico.urlSoporteFotografico !== undefined ?
                                                this.reporteFotografico.urlSoporteFotografico : null,
                        descripcion:    this.reporteFotografico.descripcion !== undefined ?
                                        this.reporteFotografico.descripcion : null
                    }
                );
            }
        }
    }

    crearFormulario() {
        this.formRegistroFotografico = this.fb.group({
            urlSoporteFotografico: [ null ],
            descripcion: [ null ]
        });
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
        const dialogRef = this.dialog.open(ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText }
        });
    }

    guardar() {
        console.log( this.formRegistroFotografico.value );
        const pSeguimientoSemanal = this.seguimientoSemanal;
        const seguimientoSemanalRegistroFotografico = [
            {
                seguimientoSemanalId: this.seguimientoSemanal.seguimientoSemanalId,
                seguimientoSemanalRegistroFotograficoId: this.seguimientoSemanalRegistroFotograficoId,
                urlSoporteFotografico:  this.formRegistroFotografico.get( 'urlSoporteFotografico' ).value !== null ?
                                        this.formRegistroFotografico.get( 'urlSoporteFotografico' ).value : null,
                descripcion:    this.formRegistroFotografico.get( 'descripcion' ).value !== null ?
                this.formRegistroFotografico.get( 'descripcion' ).value : null
            }
        ];
        console.log( seguimientoSemanalRegistroFotografico );
        pSeguimientoSemanal.seguimientoSemanalRegistroFotografico = seguimientoSemanalRegistroFotografico;
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
