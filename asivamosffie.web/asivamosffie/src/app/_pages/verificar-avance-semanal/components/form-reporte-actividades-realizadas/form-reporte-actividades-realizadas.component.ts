import { RegistrarAvanceSemanalService } from 'src/app/core/_services/registrarAvanceSemanal/registrar-avance-semanal.service';
import { VerificarAvanceSemanalService } from './../../../../core/_services/verificarAvanceSemanal/verificar-avance-semanal.service';
import { Router } from '@angular/router';
import { Component, Input, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-form-reporte-actividades-realizadas',
  templateUrl: './form-reporte-actividades-realizadas.component.html',
  styleUrls: ['./form-reporte-actividades-realizadas.component.scss']
})
export class FormReporteActividadesRealizadasComponent implements OnInit {

    @Input() esSiguienteSemana: boolean;
    @Input() esVerDetalle = false;
    @Input() reporteActividad: any;
    @Input() seguimientoSemanal: any;
    @Input() seguimientoSemanalReporteActividadId = 0;
    @Input() tipoReporteActividad: any;
    reporteActividadId = 0;
    reporteActividadSiguienteId = 0;
    tablaHistorial = new MatTableDataSource();
    tablaHistorialSiguiente = new MatTableDataSource();
    formActividadesRealizadas: FormGroup = this.fb.group({
        tieneObservaciones: [ null, Validators.required ],
        observaciones: [ null ],
        fechaCreacion: [ null ]
    });
    formActividadesRealizadasSiguienteSemana: FormGroup = this.fb.group({
        tieneObservaciones: [ null, Validators.required ],
        observaciones: [ null ],
        fechaCreacion: [ null ]
    });
    displayedColumnsHistorial: string[]  = [
        'fechaRevision',
        'responsable',
        'historial'
    ];
    dataHistorial: any[] = [];
    dataHistorialSiguiente: any[] = [];
    editorStyle = {
        height: '100px'
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
        private fb: FormBuilder,
        private dialog: MatDialog,
        private routes: Router,
        private registrarAvanceSemanalSvc: RegistrarAvanceSemanalService,
        private verificarAvanceSemanalSvc: VerificarAvanceSemanalService )
    { }

    ngOnInit(): void {
        if ( this.seguimientoSemanal !== undefined ) {
            this.reporteActividad = this.seguimientoSemanal.seguimientoSemanalReporteActividad[0];
            if ( this.reporteActividad !== undefined ) {
                this.registrarAvanceSemanalSvc.getObservacionSeguimientoSemanal( this.seguimientoSemanal.seguimientoSemanalId, this.seguimientoSemanalReporteActividadId, this.tipoReporteActividad.actividadRealizada )
                    .subscribe(
                        response => {
                            if ( response.length > 0 ) {
                                const observacionApoyo = response.filter( obs => obs.archivada === false && obs.esSupervisor === false );
                                if ( observacionApoyo[0].observacion !== undefined ) {
                                    if ( observacionApoyo[0].observacion.length > 0 ) {
                                        this.formActividadesRealizadas.get( 'observaciones' ).setValue( observacionApoyo[0].observacion );
                                    }
                                }
                                this.dataHistorial = response.filter( obs => obs.archivada === true );
                                this.tablaHistorial = new MatTableDataSource( this.dataHistorial );
                                this.reporteActividadId = observacionApoyo[0].seguimientoSemanalObservacionId;
                                this.formActividadesRealizadas.get( 'tieneObservaciones' ).setValue( this.reporteActividad.tieneObservacionApoyoActividad );
                                this.formActividadesRealizadas.get( 'fechaCreacion' ).setValue( observacionApoyo[0].fechaCreacion );
                            }
                        }
                    );
                this.registrarAvanceSemanalSvc.getObservacionSeguimientoSemanal( this.seguimientoSemanal.seguimientoSemanalId, this.seguimientoSemanalReporteActividadId, this.tipoReporteActividad.actividadRealizadaSiguiente )
                    .subscribe(
                        response => {
                            if ( response.length > 0 ) {
                                const observacionApoyo = response.filter( obs => obs.archivada === false && obs.esSupervisor === false );
                                if ( observacionApoyo[0].observacion !== undefined ) {
                                    if ( observacionApoyo[0].observacion.length > 0 ) {
                                        this.formActividadesRealizadasSiguienteSemana.get( 'observaciones' ).setValue( observacionApoyo[0].observacion );
                                    }
                                }
                                this.dataHistorialSiguiente = response.filter( obs => obs.archivada === true );
                                this.tablaHistorialSiguiente = new MatTableDataSource( this.dataHistorialSiguiente );
                                this.reporteActividadSiguienteId = observacionApoyo[0].seguimientoSemanalObservacionId;
                                this.formActividadesRealizadasSiguienteSemana.get( 'tieneObservaciones' ).setValue( this.reporteActividad.tieneObservacionApoyoActividadSiguiente );
                                this.formActividadesRealizadasSiguienteSemana.get( 'fechaCreacion' ).setValue( observacionApoyo[0].fechaCreacion );
                            }
                        }
                    );
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

    openDialog(modalTitle: string, modalText: string) {
        const dialogRef = this.dialog.open(ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText }
        });
    }

    guardar() {
        if ( this.formActividadesRealizadas.get( 'tieneObservaciones' ).value === false && this.formActividadesRealizadas.get( 'observaciones' ).value !== null ) {
            this.formActividadesRealizadas.get( 'observaciones' ).setValue( '' );
        }
		const pSeguimientoSemanalObservacion = {
			seguimientoSemanalObservacionId: this.reporteActividadId,
            seguimientoSemanalId: this.seguimientoSemanal.seguimientoSemanalId,
            tipoObservacionCodigo: this.tipoReporteActividad.actividadRealizada,
            observacionPadreId: this.seguimientoSemanalReporteActividadId,
            observacion: this.formActividadesRealizadas.get( 'observaciones' ).value,
            tieneObservacion: this.formActividadesRealizadas.get( 'tieneObservaciones' ).value,
            esSupervisor: false
        }
        console.log( pSeguimientoSemanalObservacion );
        this.verificarAvanceSemanalSvc.seguimientoSemanalObservacion( pSeguimientoSemanalObservacion )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.verificarAvanceSemanalSvc.getValidarRegistroCompletoObservaciones( this.seguimientoSemanal.seguimientoSemanalId, 'False' )
                        .subscribe(
                            response => {
                                console.log( response );
                                this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                                    () =>   this.routes.navigate(
                                                [
                                                    '/verificarAvanceSemanal/verificarSeguimientoSemanal', this.seguimientoSemanal.seguimientoSemanalId
                                                ]
                                            )
                                );
                            }
                        );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

    guardarSemanaSiguiente() {
        if ( this.formActividadesRealizadasSiguienteSemana.get( 'tieneObservaciones' ).value === false && this.formActividadesRealizadasSiguienteSemana.get( 'observaciones' ).value !== null ) {
            this.formActividadesRealizadasSiguienteSemana.get( 'observaciones' ).setValue( '' );
        }
		const pSeguimientoSemanalObservacion = {
			seguimientoSemanalObservacionId: this.reporteActividadSiguienteId,
            seguimientoSemanalId: this.seguimientoSemanal.seguimientoSemanalId,
            tipoObservacionCodigo: this.tipoReporteActividad.actividadRealizadaSiguiente,
            observacionPadreId: this.seguimientoSemanalReporteActividadId,
            observacion: this.formActividadesRealizadasSiguienteSemana.get( 'observaciones' ).value,
            tieneObservacion: this.formActividadesRealizadasSiguienteSemana.get( 'tieneObservaciones' ).value,
            esSupervisor: false
        }
        console.log( pSeguimientoSemanalObservacion );
        this.verificarAvanceSemanalSvc.seguimientoSemanalObservacion( pSeguimientoSemanalObservacion )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.verificarAvanceSemanalSvc.getValidarRegistroCompletoObservaciones( this.seguimientoSemanal.seguimientoSemanalId, 'False' )
                        .subscribe(
                            () => {
                                this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                                    () =>   this.routes.navigate(
                                                [
                                                    '/verificarAvanceSemanal/verificarSeguimientoSemanal', this.seguimientoSemanal.seguimientoSemanalId
                                                ]
                                            )
                                );
                            }
                        );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

}
