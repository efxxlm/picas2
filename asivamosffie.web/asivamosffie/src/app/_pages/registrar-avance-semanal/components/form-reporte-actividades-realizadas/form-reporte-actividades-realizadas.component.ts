import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';
import { RegistrarAvanceSemanalService } from 'src/app/core/_services/registrarAvanceSemanal/registrar-avance-semanal.service';

@Component({
  selector: 'app-form-reporte-actividades-realizadas',
  templateUrl: './form-reporte-actividades-realizadas.component.html',
  styleUrls: ['./form-reporte-actividades-realizadas.component.scss']
})
export class FormReporteActividadesRealizadasComponent implements OnInit {

    @Input() esSiguienteSemana: boolean;
    @Input() esVerDetalle = false;
    @Input() reporteActividad: any;
    @Input() tipoReporteActividad: any;
    @Input() seguimientoSemanal: any;
    @Input() seguimientoSemanalReporteActividadId = 0;
    @Output() reporteDeActividades = new EventEmitter();
    formActividadesRealizadas: FormGroup;
    formActividadesRealizadasSiguienteSemana: FormGroup;
    tablaHistorial = new MatTableDataSource();
    tablaHistorialSiguiente = new MatTableDataSource();
    dataHistorial: any[] = [];
    dataHistorialSiguiente: any[] = [];
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
        private fb: FormBuilder,
        private registrarAvanceSemanalSvc: RegistrarAvanceSemanalService )
    {
        this.crearFormulario();
        this.crearFormularioSiguienteSemana();
    }

    ngOnInit(): void {
        if ( this.reporteActividad !== undefined ) {
            this.formActividadesRealizadas.setValue(
                {
                    actividadTecnica:   this.reporteActividad.actividadTecnica !== undefined ?
                                        this.reporteActividad.actividadTecnica : null,
                    actividadLegal: this.reporteActividad.actividadLegal !== undefined ?
                                    this.reporteActividad.actividadLegal : null,
                    actividadAdministrativaFinanciera:  this.reporteActividad.actividadAdministrativaFinanciera !== undefined ?
                                                        this.reporteActividad.actividadAdministrativaFinanciera : null
                }
            );
            this.formActividadesRealizadasSiguienteSemana.setValue(
                {
                    actividadTecnicaSiguiente:  this.reporteActividad.actividadTecnicaSiguiente !== undefined ?
                                                this.reporteActividad.actividadTecnicaSiguiente : null,
                    actividadLegalSiguiente:    this.reporteActividad.actividadLegalSiguiente !== undefined ?
                                                this.reporteActividad.actividadLegalSiguiente : null,
                    actividadAdministrativaFinancieraSiguiente:
                        this.reporteActividad.actividadAdministrativaFinancieraSiguiente !== undefined ?
                        this.reporteActividad.actividadAdministrativaFinancieraSiguiente : null
                }
            );
            this.registrarAvanceSemanalSvc.getObservacionSeguimientoSemanal( this.seguimientoSemanal.seguimientoSemanalId, this.seguimientoSemanalReporteActividadId, this.tipoReporteActividad.actividadRealizada )
            .subscribe(
                response => {
                    if ( response.length > 0 ) {
                        this.dataHistorial = response.filter( obs => obs.archivada === true );
                        this.tablaHistorial = new MatTableDataSource( this.dataHistorial );
                    }
                }
            );
        this.registrarAvanceSemanalSvc.getObservacionSeguimientoSemanal( this.seguimientoSemanal.seguimientoSemanalId, this.seguimientoSemanalReporteActividadId, this.tipoReporteActividad.actividadRealizadaSiguiente )
            .subscribe(
                response => {
                    if ( response.length > 0 ) {
                        this.dataHistorialSiguiente = response.filter( obs => obs.archivada === true );
                        this.tablaHistorialSiguiente = new MatTableDataSource( this.dataHistorialSiguiente );
                    }
                }
            );
        }
    }

    crearFormulario() {
        this.formActividadesRealizadas = this.fb.group({
            actividadTecnica: [ null ],
            actividadLegal: [ null ],
            actividadAdministrativaFinanciera: [ null ]
        });
    }

    crearFormularioSiguienteSemana() {
        this.formActividadesRealizadasSiguienteSemana = this.fb.group({
            actividadTecnicaSiguiente: [ null ],
            actividadLegalSiguiente: [ null ],
            actividadAdministrativaFinancieraSiguiente: [ null ]
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

    guardar() {
        if ( this.esSiguienteSemana === true ) {
            this.reporteDeActividades.emit(
                {
                    esSiguienteSemana: this.esSiguienteSemana,
                    reporteActividadSiguiente: this.formActividadesRealizadasSiguienteSemana.value,
                    reporteActividad: this.formActividadesRealizadas.value
                }
            );
        } else {
            this.reporteDeActividades.emit(
                {
                    esSiguienteSemana: this.esSiguienteSemana,
                    reporteActividadSiguiente: this.formActividadesRealizadasSiguienteSemana.value,
                    reporteActividad: this.formActividadesRealizadas.value
                }
            );
        }
    }

}
