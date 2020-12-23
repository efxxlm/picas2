import { RegistrarAvanceSemanalService } from './../../../../core/_services/registrarAvanceSemanal/registrar-avance-semanal.service';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
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
    @Output() estadoSemaforoReporte = new EventEmitter();
    formResumenGeneral: FormGroup;
    seguimientoSemanalId: number;
    seguimientoSemanalReporteActividadId: number;
    reporteActividad: any;
    semaforoReporte = 'sin-diligenciar';
    semaforoActividad = 'sin-diligenciar';
    semaforoActividadSiguiente = 'sin-diligenciar';
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
       private fb: FormBuilder,
       private dialog: MatDialog,
       private routes: Router,
       private avanceSemanalSvc: RegistrarAvanceSemanalService )
    {
        this.crearFormulario();
    }

    ngOnInit(): void {
        if ( this.seguimientoSemanal !== undefined ) {
            let sinDiligenciar = 0;
            let completo = 0;
            const totalAcordeones = 3;
            this.seguimientoSemanalId = this.seguimientoSemanal.seguimientoSemanalId;
            this.seguimientoSemanalReporteActividadId =  this.seguimientoSemanal.seguimientoSemanalReporteActividad.length > 0 ?
            this.seguimientoSemanal.seguimientoSemanalReporteActividad[0].seguimientoSemanalReporteActividadId : 0;

            if ( this.seguimientoSemanal.seguimientoSemanalReporteActividad.length > 0 ) {
                this.reporteActividad = this.seguimientoSemanal.seguimientoSemanalReporteActividad[0];
                this.formResumenGeneral.setValue(
                    {
                        resumenEstadoContrato:  this.reporteActividad.resumenEstadoContrato !== undefined ?
                                                this.reporteActividad.resumenEstadoContrato : null
                    }
                );
                // Semaforo reporte
                if ( this.reporteActividad.registroCompletoEstadoContrato === true ) {
                    this.semaforoReporte = 'completo';
                }

                if ( this.semaforoReporte === 'sin-diligenciar' ) {
                    sinDiligenciar++;
                }

                if ( this.semaforoReporte === 'completo' ) {
                    completo++;
                }
                // Semaforo actividad
                if ( this.reporteActividad.actividadTecnica !== undefined && this.reporteActividad.registroCompletoActividad === false ) {
                    this.semaforoActividad = 'en-proceso';
                }
                if ( this.reporteActividad.registroCompletoActividad === true ) {
                    this.semaforoActividad = 'completo';
                }

                if ( this.semaforoActividad === 'sin-diligenciar' ) {
                    sinDiligenciar++;
                }

                if ( this.semaforoActividad === 'completo' ) {
                    completo++;
                }
                // Semaforo actividad siguiente
                if (    this.reporteActividad.actividadTecnicaSiguiente !== undefined
                        && this.reporteActividad.registroCompletoActividadSiguiente === false ) {
                    this.semaforoActividadSiguiente = 'en-proceso';
                }
                if ( this.reporteActividad.registroCompletoActividadSiguiente === true ) {
                    this.semaforoActividadSiguiente = 'completo';
                }

                if ( this.semaforoActividadSiguiente === 'sin-diligenciar' ) {
                    sinDiligenciar++;
                }

                if ( this.semaforoActividadSiguiente === 'completo' ) {
                    completo++;
                }
                if ( totalAcordeones === completo ) {
                    this.estadoSemaforoReporte.emit( 'completo' );
                }
                if ( totalAcordeones === sinDiligenciar ) {
                    this.estadoSemaforoReporte.emit( 'sin-diligenciar' );
                }
                if ( totalAcordeones > completo && completo > 0 ) {
                    this.estadoSemaforoReporte.emit( 'en-proceso' );
                }
            } else {
                this.estadoSemaforoReporte.emit( 'sin-diligenciar' );
            }
        }
    }

    crearFormulario() {
        this.formResumenGeneral = this.fb.group({
            resumenEstadoContrato: [ null ]
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

    guardar( reporte?: any ) {
        const pSeguimientoSemanal = this.seguimientoSemanal;
        const seguimientoSemanalReporteActividad: any = [
            {
                seguimientoSemanalId: this.seguimientoSemanal.seguimientoSemanalId,
                seguimientoSemanalReporteActividadId: this.seguimientoSemanalReporteActividadId,
                resumenEstadoContrato:  this.formResumenGeneral.get( 'resumenEstadoContrato' ).value !== null ?
                                        this.formResumenGeneral.get( 'resumenEstadoContrato' ).value : null
            }
        ];
        if ( reporte !== undefined ) {
            seguimientoSemanalReporteActividad[0].actividadTecnicaSiguiente = reporte.reporteActividadSiguiente.actividadTecnicaSiguiente;
            seguimientoSemanalReporteActividad[0].actividadLegalSiguiente = reporte.reporteActividadSiguiente.actividadLegalSiguiente;
            seguimientoSemanalReporteActividad[0]
                .actividadAdministrativaFinancieraSiguiente = reporte.reporteActividadSiguiente.actividadAdministrativaFinancieraSiguiente;
            seguimientoSemanalReporteActividad[0].actividadTecnica = reporte.reporteActividad.actividadTecnica;
            seguimientoSemanalReporteActividad[0].actividadLegal = reporte.reporteActividad.actividadLegal;
            seguimientoSemanalReporteActividad[0]
                .actividadAdministrativaFinanciera = reporte.reporteActividad.actividadAdministrativaFinanciera;
        }

        console.log( seguimientoSemanalReporteActividad );
        pSeguimientoSemanal.seguimientoSemanalReporteActividad = seguimientoSemanalReporteActividad;
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
