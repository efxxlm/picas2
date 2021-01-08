import { Component, Input, OnInit } from '@angular/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { RegistrarAvanceSemanalService } from 'src/app/core/_services/registrarAvanceSemanal/registrar-avance-semanal.service';
import { VerificarAvanceSemanalService } from 'src/app/core/_services/verificarAvanceSemanal/verificar-avance-semanal.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-reporte-actividades',
  templateUrl: './reporte-actividades.component.html',
  styleUrls: ['./reporte-actividades.component.scss']
})
export class ReporteActividadesComponent implements OnInit {

    @Input() esVerDetalle = false;
    @Input() tipoReporteActividad: any;
    @Input() seguimientoSemanal: any;
    seguimientoSemanalId: number;
    seguimientoSemanalReporteActividadId: number;
    seguimientoSemanalObservacionId = 0;
    reporteActividad: any;
    semaforoReporte = 'sin-diligenciar';
    semaforoActividad = 'sin-diligenciar';
    semaforoActividadSiguiente = 'sin-diligenciar';
    tablaHistorial = new MatTableDataSource();
    observacionApoyo: any;
    formReporteActividades: FormGroup = this.fb.group({
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
                //Get Observacion apoyo y supervisor
                if ( this.reporteActividad.observacionApoyoIdEstadoContrato !== undefined || this.reporteActividad.observacionSupervisorIdEstadoContrato !== undefined ) {
                    this.registrarAvanceSemanalSvc.getObservacionSeguimientoSemanal( this.seguimientoSemanalId, this.seguimientoSemanalReporteActividadId, this.tipoReporteActividad.actividadEstadoObra )
                        .subscribe(
                            response => {
                                this.observacionApoyo = response.filter( obs => obs.archivada === false && obs.esSupervisor === false );
                                const observacionSupervisor = response.filter( obs => obs.archivada === false && obs.esSupervisor === true );
                                this.dataHistorial = response.filter( obs => obs.archivada === true );
                                this.tablaHistorial = new MatTableDataSource( this.dataHistorial );
                                if ( observacionSupervisor.length > 0 ) {
                                    this.seguimientoSemanalObservacionId = observacionSupervisor[0].seguimientoSemanalObservacionId;
                                    if ( observacionSupervisor[0].observacion !== undefined ) {
                                        if ( observacionSupervisor[0].observacion.length > 0 ) {
                                            this.formReporteActividades.get( 'observaciones' ).setValue( observacionSupervisor[0].observacion );
                                        }
                                    }
                                    this.formReporteActividades.get( 'tieneObservaciones' ).setValue( this.reporteActividad.tieneObservacionSupervisorEstadoContrato );
                                    this.formReporteActividades.get( 'fechaCreacion' ).setValue( observacionSupervisor[0].fechaCreacion );
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
        if ( this.formReporteActividades.get( 'tieneObservaciones' ).value === false && this.formReporteActividades.get( 'observaciones' ).value !== null ) {
            this.formReporteActividades.get( 'observaciones' ).setValue( '' );
        }
		const pSeguimientoSemanalObservacion = {
			seguimientoSemanalObservacionId: this.seguimientoSemanalObservacionId,
            seguimientoSemanalId: this.seguimientoSemanalId,
            tipoObservacionCodigo: this.tipoReporteActividad.actividadEstadoObra,
            observacionPadreId: this.seguimientoSemanalReporteActividadId,
            observacion: this.formReporteActividades.get( 'observaciones' ).value,
            tieneObservacion: this.formReporteActividades.get( 'tieneObservaciones' ).value,
            esSupervisor: true
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
                                                    '/validarAvanceSemanal/validarSeguimientoSemanal', this.seguimientoSemanal.contratacionProyectoId
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
