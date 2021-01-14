import { RegistrarAvanceSemanalService } from 'src/app/core/_services/registrarAvanceSemanal/registrar-avance-semanal.service';
import { Router } from '@angular/router';
import { VerificarAvanceSemanalService } from './../../../../core/_services/verificarAvanceSemanal/verificar-avance-semanal.service';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-reporte-actividades',
  templateUrl: './reporte-actividades.component.html',
  styleUrls: ['./reporte-actividades.component.scss']
})
export class ReporteActividadesComponent implements OnInit {

    @Input() esVerDetalle = false;
    @Input() seguimientoSemanal: any;
    @Input() tipoReporteActividad: any;
    @Output() estadoSemaforo = new EventEmitter<number>();
    tablaHistorial = new MatTableDataSource();
    dataHistorial: any[] = [];
    seguimientoSemanalId: number;
    seguimientoSemanalReporteActividadId: number;
    seguimientoSemanalObservacionId = 0;
    reporteActividad: any;
    semaforoReporte = 'sin-diligenciar';
    semaforoActividad = 'sin-diligenciar';
    semaforoActividadSiguiente = 'sin-diligenciar';
    formResumenGeneral: FormGroup = this.fb.group({
        tieneObservaciones: [ null, Validators.required ],
        observaciones: [ null ],
        fechaCreacion: [ null ]
    });
    displayedColumnsHistorial: string[]  = [
        'fechaRevision',
        'responsable',
        'historial'
    ];
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
        private registrarAvanceSemanalSvc: RegistrarAvanceSemanalService,
        private verificarAvanceSemanalSvc: VerificarAvanceSemanalService,
        private dialog: MatDialog,
        private routes: Router )
    { }

    ngOnInit(): void {
        if ( this.seguimientoSemanal !== undefined ) {
            this.seguimientoSemanalId = this.seguimientoSemanal.seguimientoSemanalId;
            this.seguimientoSemanalReporteActividadId =  this.seguimientoSemanal.seguimientoSemanalReporteActividad.length > 0 ?
            this.seguimientoSemanal.seguimientoSemanalReporteActividad[0].seguimientoSemanalReporteActividadId : 0;

            if ( this.seguimientoSemanal.seguimientoSemanalReporteActividad.length > 0 ) {
                this.reporteActividad = this.seguimientoSemanal.seguimientoSemanalReporteActividad[0];
                if ( this.reporteActividad !== undefined ) {
                    if ( this.esVerDetalle === false ) {
                        let totalReportes = 0;
                        // Semaforo resumen general
                        if ( this.reporteActividad.registroCompletoObservacionApoyoEstadoContrato === false ) {
                            this.semaforoReporte = 'en-proceso';
                        }
                        if ( this.reporteActividad.registroCompletoObservacionApoyoEstadoContrato === true ) {
                            this.semaforoReporte = 'completo';
                            totalReportes++;
                        }
                        // Semaforo actividad
                        if ( this.reporteActividad.registroCompletoObservacionApoyoActividad === false ) {
                            this.semaforoActividad = 'en-proceso';
                        }
                        if ( this.reporteActividad.registroCompletoObservacionApoyoActividad === true ) {
                            this.semaforoActividad = 'completo';
                            totalReportes++;
                        }
                        // Semaforo actividad siguiente
                        if ( this.reporteActividad.registroCompletoObservacionApoyoActividadSiguiente === false ) {
                            this.semaforoActividadSiguiente = 'en-proceso';
                        }
                        if ( this.reporteActividad.registroCompletoObservacionApoyoActividadSiguiente === true ) {
                            this.semaforoActividadSiguiente = 'completo';
                            totalReportes++;
                        }
                        this.estadoSemaforo.emit( totalReportes );
                    }
                    if ( this.esVerDetalle === true ) {
                        this.semaforoReporte = '';
                        this.semaforoActividad = '';
                        this.semaforoActividadSiguiente = '';
                    }
                    this.registrarAvanceSemanalSvc.getObservacionSeguimientoSemanal( this.seguimientoSemanalId, this.seguimientoSemanalReporteActividadId, this.tipoReporteActividad.actividadEstadoObra )
                        .subscribe(
                            response => {
                                if ( response.length > 0 ) {
                                    const observacionApoyo = response.filter( obs => obs.archivada === false && obs.esSupervisor === false );
                                    if ( observacionApoyo[0].observacion !== undefined ) {
                                        if ( observacionApoyo[0].observacion.length > 0 ) {
                                            this.formResumenGeneral.get( 'observaciones' ).setValue( observacionApoyo[0].observacion );
                                        }
                                    }
                                    this.dataHistorial = response.filter( obs => obs.archivada === true );
                                    this.tablaHistorial = new MatTableDataSource( this.dataHistorial );
                                    this.seguimientoSemanalObservacionId = observacionApoyo[0].seguimientoSemanalObservacionId;
                                    this.formResumenGeneral.get( 'tieneObservaciones' ).setValue( this.reporteActividad.tieneObservacionApoyoEstadoContrato );
                                    this.formResumenGeneral.get( 'fechaCreacion' ).setValue( observacionApoyo[0].fechaCreacion );
                                }
                            }
                        );
                }
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
        if ( this.formResumenGeneral.get( 'tieneObservaciones' ).value === false && this.formResumenGeneral.get( 'observaciones' ).value !== null ) {
            this.formResumenGeneral.get( 'observaciones' ).setValue( '' );
        }
		const pSeguimientoSemanalObservacion = {
			seguimientoSemanalObservacionId: this.seguimientoSemanalObservacionId,
            seguimientoSemanalId: this.seguimientoSemanalId,
            tipoObservacionCodigo: this.tipoReporteActividad.actividadEstadoObra,
            observacionPadreId: this.seguimientoSemanalReporteActividadId,
            observacion: this.formResumenGeneral.get( 'observaciones' ).value,
            tieneObservacion: this.formResumenGeneral.get( 'tieneObservaciones' ).value,
            esSupervisor: false
        }
        console.log( pSeguimientoSemanalObservacion );
        this.verificarAvanceSemanalSvc.seguimientoSemanalObservacion( pSeguimientoSemanalObservacion )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.verificarAvanceSemanalSvc.getValidarRegistroCompletoObservaciones( this.seguimientoSemanalId, 'False' )
                        .subscribe(
                            () => {
                                this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                                    () =>   this.routes.navigate(
                                                [
                                                    '/verificarAvanceSemanal/verificarSeguimientoSemanal', this.seguimientoSemanalId
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
