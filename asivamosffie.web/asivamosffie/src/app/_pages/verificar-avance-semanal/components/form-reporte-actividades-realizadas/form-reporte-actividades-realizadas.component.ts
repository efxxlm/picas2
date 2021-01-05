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
    reporteActividadId = 0;
    reporteActividadSiguienteId = 0;
    formActividadesRealizadas: FormGroup = this.fb.group({
        tieneObservaciones: [ null, Validators.required ],
        observaciones: [ null ]
    });
    formActividadesRealizadasSiguienteSemana: FormGroup = this.fb.group({
        tieneObservaciones: [ null, Validators.required ],
        observaciones: [ null ]
    });
    tablaHistorial = new MatTableDataSource();
    displayedColumnsHistorial: string[]  = [
        'fechaRevision',
        'responsable',
        'historial'
    ];
    dataHistorial: any[] = [
        {
            fechaRevision: new Date(),
            responsable: 'Apoyo a la supervisión',
            historial: '<p>Se recomienda que en cada actividad se especifique el responsable.</p>'
        }
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
        private dialog: MatDialog,
        private routes: Router,
        private verificarAvanceSemanalSvc: VerificarAvanceSemanalService )
    { }

    ngOnInit(): void {
        this.tablaHistorial = new MatTableDataSource( this.dataHistorial );
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
		const pSeguimientoSemanalObservacion = {
			seguimientoSemanalObservacionId: this.reporteActividadId,
            seguimientoSemanalId: this.seguimientoSemanal.seguimientoSemanalId,
            tipoObservacionCodigo: '17',
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
                    this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                        () =>   this.routes.navigate(
                                    [
                                        '/verificarAvanceSemanal/verificarSeguimientoSemanal', this.seguimientoSemanal.contratacionProyectoId
                                    ]
                                )
                    );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

    guardarSemanaSiguiente() {
        console.log( this.formActividadesRealizadasSiguienteSemana.value );
		const pSeguimientoSemanalObservacion = {
			seguimientoSemanalObservacionId: this.reporteActividadSiguienteId,
            seguimientoSemanalId: this.seguimientoSemanal.seguimientoSemanalId,
            tipoObservacionCodigo: '18',
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
                    this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                        () =>   this.routes.navigate(
                                    [
                                        '/verificarAvanceSemanal/verificarSeguimientoSemanal', this.seguimientoSemanal.contratacionProyectoId
                                    ]
                                )
                    );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

}
