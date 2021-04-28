import { RegistrarAvanceSemanalService } from './../../../../core/_services/registrarAvanceSemanal/registrar-avance-semanal.service';
import { Router } from '@angular/router';
import { Component, Input, OnInit, OnDestroy, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { MatTableDataSource } from '@angular/material/table';
import { GuardadoParcialAvanceSemanalService } from 'src/app/core/_services/guardadoParcialAvanceSemanal/guardado-parcial-avance-semanal.service';
import { VerificarAvanceSemanalService } from 'src/app/core/_services/verificarAvanceSemanal/verificar-avance-semanal.service';

@Component({
  selector: 'app-registro-fotografico',
  templateUrl: './registro-fotografico.component.html',
  styleUrls: ['./registro-fotografico.component.scss']
})
export class RegistroFotograficoComponent implements OnInit, OnDestroy {

    @Input() esRegistroNuevo: boolean;
    @Input() esVerDetalle = false;
    @Input() seguimientoSemanal: any;
    @Input() tipoRegistroFotografico: any;
    @Output() tieneObservacion = new EventEmitter();
    obsApoyo: any;
    obsSupervisor: any;
    seRealizoPeticion = false;
    verAyuda = false;
    formRegistroFotografico: FormGroup;
    seguimientoSemanalId: number;
    seguimientoSemanalRegistroFotograficoId: number;
    reporteFotografico: any;
    tablaHistorial = new MatTableDataSource();
    dataHistorial: any[] = [];
    displayedColumnsHistorial: string[]  = [
        'fechaRevision',
        'responsable',
        'historial'
    ];
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
        private avanceSemanalSvc: RegistrarAvanceSemanalService,
        private verificarAvanceSemanalSvc: VerificarAvanceSemanalService,
        private guardadoParcialAvanceSemanalSvc: GuardadoParcialAvanceSemanalService )
    {
        this.crearFormulario();
    }
    ngOnDestroy(): void {
        if ( this.formRegistroFotografico.dirty === true && this.seRealizoPeticion === false ) {
            this.guardadoParcialAvanceSemanalSvc.getDataRegistroFotografico( this.guardadoParcial(), this.seRealizoPeticion )
        } else {
            this.guardadoParcialAvanceSemanalSvc.getDataRegistroFotografico( undefined )
        }
    }

    ngOnInit(): void {
        if ( this.seguimientoSemanal !== undefined ) {
            this.seguimientoSemanalId = this.seguimientoSemanal.seguimientoSemanalId;
            this.seguimientoSemanalRegistroFotograficoId =  this.seguimientoSemanal.seguimientoSemanalRegistroFotografico.length > 0 ?
            this.seguimientoSemanal.seguimientoSemanalRegistroFotografico[0].seguimientoSemanalRegistroFotograficoId : 0;

            if ( this.seguimientoSemanal.seguimientoSemanalRegistroFotografico.length > 0 ) {
                this.reporteFotografico = this.seguimientoSemanal.seguimientoSemanalRegistroFotografico[0];
                if ( this.esVerDetalle === false ) {
                    this.avanceSemanalSvc.getObservacionSeguimientoSemanal( this.seguimientoSemanalId, this.reporteFotografico.seguimientoSemanalRegistroFotograficoId, this.tipoRegistroFotografico )
                        .subscribe(
                            response => {
                                this.obsApoyo = response.find( obs => obs.archivada === false && obs.esSupervisor === false && obs.tieneObservacion === true );
                                this.obsSupervisor  = response.find( obs => obs.archivada === false && obs.esSupervisor === true && obs.tieneObservacion === true );
                                this.dataHistorial = response.filter( obs => obs.tieneObservacion === true );

                                if ( this.obsApoyo !== undefined || this.obsSupervisor !== undefined ) {
                                    this.tieneObservacion.emit();
                                }

                                this.tablaHistorial = new MatTableDataSource( this.dataHistorial );
                            }
                        );
                }
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

        if (!this.esRegistroNuevo) this.formRegistroFotografico.markAllAsTouched();
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

        pSeguimientoSemanal.seguimientoSemanalRegistroFotografico = seguimientoSemanalRegistroFotografico;
        this.avanceSemanalSvc.saveUpdateSeguimientoSemanal( pSeguimientoSemanal )
            .subscribe(
                async response => {
                    if ( this.obsApoyo !== undefined ) {
                        this.obsApoyo.archivada = !this.obsApoyo.archivada;
                        await this.verificarAvanceSemanalSvc.seguimientoSemanalObservacion( this.obsApoyo ).toPromise();
                    }
                    if ( this.obsSupervisor !== undefined ) {
                        this.obsSupervisor.archivada = !this.obsSupervisor.archivada;
                        await this.verificarAvanceSemanalSvc.seguimientoSemanalObservacion( this.obsSupervisor ).toPromise();
                    }

                    this.seRealizoPeticion = true;
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

    guardadoParcial() {
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

        return seguimientoSemanalRegistroFotografico
    }

}
