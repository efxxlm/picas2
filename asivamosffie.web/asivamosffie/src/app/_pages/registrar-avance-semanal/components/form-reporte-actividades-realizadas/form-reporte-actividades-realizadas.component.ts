import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';
import { RegistrarAvanceSemanalService } from 'src/app/core/_services/registrarAvanceSemanal/registrar-avance-semanal.service';
import { VerificarAvanceSemanalService } from 'src/app/core/_services/verificarAvanceSemanal/verificar-avance-semanal.service';

@Component({
  selector: 'app-form-reporte-actividades-realizadas',
  templateUrl: './form-reporte-actividades-realizadas.component.html',
  styleUrls: ['./form-reporte-actividades-realizadas.component.scss']
})
export class FormReporteActividadesRealizadasComponent implements OnInit {

    @Input() formActividadesRealizadas: FormGroup;
    @Input() formActividadesRealizadasSiguienteSemana: FormGroup;
    @Input() esSiguienteSemana: boolean;
    @Input() esVerDetalle = false;
    @Input() esRegistroNuevo: boolean;
    @Input() reporteActividad: any;
    @Input() tipoReporteActividad: any;
    @Input() seguimientoSemanal: any;
    @Input() seguimientoSemanalReporteActividadId = 0;
    @Output() reporteDeActividades = new EventEmitter();
    @Output() tieneObservacion = new EventEmitter();
    obsApoyo: any;
    obsSupervisor: any;
    obsApoyoSiguienteSemana: any;
    obsSupervisorSiguienteSemana: any;
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
        private verificarAvanceSemanalSvc: VerificarAvanceSemanalService,
        private registrarAvanceSemanalSvc: RegistrarAvanceSemanalService )
    {
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
            if ( this.esVerDetalle === false ) {
                this.registrarAvanceSemanalSvc.getObservacionSeguimientoSemanal( this.seguimientoSemanal.seguimientoSemanalId, this.seguimientoSemanalReporteActividadId, this.tipoReporteActividad.actividadRealizada )
                    .subscribe(
                        response => {
                            if ( response.length > 0 ) {
                                this.obsApoyo = response.find( obs => obs.archivada === false && obs.esSupervisor === false );
                                this.obsSupervisor  = response.find( obs => obs.archivada === false && obs.esSupervisor === true );
                                this.dataHistorial = response;

                                if ( this.obsApoyo !== undefined || this.obsSupervisor !== undefined ) {
                                    this.tieneObservacion.emit();
                                }

                                this.tablaHistorial = new MatTableDataSource( this.dataHistorial );
                            }
                        }
                    );
                this.registrarAvanceSemanalSvc.getObservacionSeguimientoSemanal( this.seguimientoSemanal.seguimientoSemanalId, this.seguimientoSemanalReporteActividadId, this.tipoReporteActividad.actividadRealizadaSiguiente )
                    .subscribe(
                        response => {
                            if ( response.length > 0 ) {
                                this.obsApoyoSiguienteSemana = response.find( obs => obs.archivada === false && obs.esSupervisor === false );
                                this.obsSupervisorSiguienteSemana  = response.find( obs => obs.archivada === false && obs.esSupervisor === true );
                                this.dataHistorialSiguiente = response;

                                if ( this.obsApoyoSiguienteSemana !== undefined || this.obsSupervisorSiguienteSemana !== undefined ) {
                                    this.tieneObservacion.emit();
                                }

                                this.tablaHistorialSiguiente = new MatTableDataSource( this.dataHistorialSiguiente );
                            }
                        }
                    );
            }
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

    async guardar() {
        if ( this.formActividadesRealizadas.dirty === true ) {
            if ( this.obsApoyo !== undefined ) {
                this.obsApoyo.archivada = !this.obsApoyo.archivada;
                await this.verificarAvanceSemanalSvc.seguimientoSemanalObservacion( this.obsApoyo ).toPromise();
            }
            if ( this.obsSupervisor !== undefined ) {
                this.obsSupervisor.archivada = !this.obsSupervisor.archivada;
                await this.verificarAvanceSemanalSvc.seguimientoSemanalObservacion( this.obsSupervisor ).toPromise();
            }
        }

        if ( this.formActividadesRealizadasSiguienteSemana.dirty === true ) {
            if ( this.obsApoyoSiguienteSemana !== undefined ) {
                this.obsApoyoSiguienteSemana.archivada = !this.obsApoyoSiguienteSemana.archivada;
                await this.verificarAvanceSemanalSvc.seguimientoSemanalObservacion( this.obsApoyoSiguienteSemana ).toPromise();
            }
            if ( this.obsSupervisorSiguienteSemana !== undefined ) {
                this.obsSupervisorSiguienteSemana.archivada = !this.obsSupervisorSiguienteSemana.archivada;
                await this.verificarAvanceSemanalSvc.seguimientoSemanalObservacion( this.obsSupervisorSiguienteSemana ).toPromise();
            }
        }

        this.reporteDeActividades.emit()
    }

}
