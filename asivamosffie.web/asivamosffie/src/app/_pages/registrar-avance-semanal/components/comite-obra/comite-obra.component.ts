import { RegistrarAvanceSemanalService } from './../../../../core/_services/registrarAvanceSemanal/registrar-avance-semanal.service';
import { Router } from '@angular/router';
import { FormGroup, FormBuilder } from '@angular/forms';
import { Component, Input, OnInit, OnDestroy, Output, EventEmitter } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { MatTableDataSource } from '@angular/material/table';
import { GuardadoParcialAvanceSemanalService } from 'src/app/core/_services/guardadoParcialAvanceSemanal/guardado-parcial-avance-semanal.service';
import { VerificarAvanceSemanalService } from 'src/app/core/_services/verificarAvanceSemanal/verificar-avance-semanal.service';

@Component({
  selector: 'app-comite-obra',
  templateUrl: './comite-obra.component.html',
  styleUrls: ['./comite-obra.component.scss']
})
export class ComiteObraComponent implements OnInit, OnDestroy {

    @Input() esRegistroNuevo: boolean;
    @Input() esVerDetalle = false;
    @Input() seguimientoSemanal: any;
    @Input() tipoComiteObra: any;
    @Output() tieneObservacion = new EventEmitter();
    obsApoyo: any;
    obsSupervisor: any;
    seRealizoPeticion = false;
    numeroComiteObra: string;
    seguimientoSemanalId: number;
    seguimientoSemanalRegistrarComiteObraId: number;
    gestionComiteObra: any;
    formComiteObra: FormGroup;
    tablaHistorial = new MatTableDataSource();
    dataHistorial: any[] = [];
    displayedColumnsHistorial: string[]  = [
        'fechaRevision',
        'responsable',
        'historial'
    ];

    constructor(
        private fb: FormBuilder,
        private dialog: MatDialog,
        private routes: Router,
        private avanceSemanalSvc: RegistrarAvanceSemanalService,
        private verificarAvanceSemanalSvc: VerificarAvanceSemanalService,
        private guardadoParcialAvanceSemanalSvc: GuardadoParcialAvanceSemanalService )
    {
        this.crearFormulario();
    }

    ngOnDestroy(): void {
        if ( this.formComiteObra.dirty === true && this.seRealizoPeticion === false ) {
            this.guardadoParcialAvanceSemanalSvc.getDataComiteObra( this.guardadoParcial(), this.seRealizoPeticion )
        } else {
            this.guardadoParcialAvanceSemanalSvc.getDataComiteObra( undefined )
        }
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
                if ( this.esVerDetalle === false ) {
                    this.avanceSemanalSvc.getObservacionSeguimientoSemanal( this.seguimientoSemanalId, this.seguimientoSemanalRegistrarComiteObraId, this.tipoComiteObra )
                        .subscribe(
                            response => {
                                this.obsApoyo = response.find( obs => obs.archivada === false && obs.esSupervisor === false && obs.tieneObservacion === true );
                                this.obsSupervisor = response.find( obs => obs.archivada === false && obs.esSupervisor === true && obs.tieneObservacion === true );
                                this.dataHistorial = response.filter( obs => obs.tieneObservacion === true );

                                if ( this.obsApoyo !== undefined || this.obsSupervisor !== undefined ) {
                                    this.tieneObservacion.emit();
                                }

                                this.tablaHistorial = new MatTableDataSource( this.dataHistorial );
                            }
                        );
                }
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

        if (!this.esRegistroNuevo) this.formComiteObra.markAllAsTouched();
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

        return seguimientoSemanalRegistrarComiteObra
    }

}
