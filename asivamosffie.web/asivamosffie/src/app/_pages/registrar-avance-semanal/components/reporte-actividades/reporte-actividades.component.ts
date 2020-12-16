import { RegistrarAvanceSemanalService } from './../../../../core/_services/registrarAvanceSemanal/registrar-avance-semanal.service';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Component, Input, OnInit } from '@angular/core';
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
    formResumenGeneral: FormGroup;
    seguimientoSemanalId: number;
    seguimientoSemanalReporteActividadId: number;
    reporteActividad: any;
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
            console.log( reporte );
            if ( reporte.esSiguienteSemana === true ) {
                seguimientoSemanalReporteActividad[0].actividadTecnicaSiguiente = reporte.reporteActividades.actividadTecnicaSiguiente;
                seguimientoSemanalReporteActividad[0].actividadLegalSiguiente = reporte.reporteActividades.actividadLegalSiguiente;
                seguimientoSemanalReporteActividad[0]
                    .actividadAdministrativaFinancieraSiguiente = reporte.reporteActividades.actividadAdministrativaFinancieraSiguiente;
            } else {
                seguimientoSemanalReporteActividad[0].actividadTecnica = reporte.reporteActividades.actividadTecnica;
                seguimientoSemanalReporteActividad[0].actividadLegal = reporte.reporteActividades.actividadLegal;
                seguimientoSemanalReporteActividad[0]
                    .actividadAdministrativaFinanciera = reporte.reporteActividades.actividadAdministrativaFinanciera;
            }
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
