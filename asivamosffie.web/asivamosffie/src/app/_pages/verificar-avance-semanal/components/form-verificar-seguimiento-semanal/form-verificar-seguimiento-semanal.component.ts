import { VerificarAvanceSemanalService } from './../../../../core/_services/verificarAvanceSemanal/verificar-avance-semanal.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { RegistrarAvanceSemanalService } from 'src/app/core/_services/registrarAvanceSemanal/registrar-avance-semanal.service';

@Component({
  selector: 'app-form-verificar-seguimiento-semanal',
  templateUrl: './form-verificar-seguimiento-semanal.component.html',
  styleUrls: ['./form-verificar-seguimiento-semanal.component.scss']
})
export class FormVerificarSeguimientoSemanalComponent implements OnInit {

    seguimientoSemanal: any;
    semaforoAvanceFisico = 'sin-diligenciar';
    semaforoGestionObra = 'sin-diligenciar';
    semaforoReporteActividad = 'sin-diligenciar';
    semaforoRegistroFotografico = 'sin-diligenciar';
    semaforoComiteObra = 'sin-diligenciar';
    tipoObservaciones: any;

    constructor(
        private avanceSemanalSvc: RegistrarAvanceSemanalService,
        private activatedRoute: ActivatedRoute,
        private verificarAvanceSemanalSvc: VerificarAvanceSemanalService )
    {
        this.avanceSemanalSvc.getLastSeguimientoSemanalContratacionProyectoIdOrSeguimientoSemanalId( activatedRoute.snapshot.params.id, 0 )
            .subscribe(
                seguimiento => {
                    this.seguimientoSemanal = seguimiento;
                    this.verificarAvanceSemanalSvc.tipoObservaciones()
                    .subscribe( response => {
                        this.tipoObservaciones = response;
                        // Semaforo avance fisico
                        const avanceFisico = this.seguimientoSemanal.seguimientoSemanalAvanceFisico[0];
                        if ( avanceFisico.registroCompletoObservacionApoyo === false ) {
                            this.semaforoAvanceFisico = 'en-proceso';
                        } 
                        if ( avanceFisico.registroCompletoObservacionApoyo === true ) {
                            this.semaforoAvanceFisico = 'completo';
                        }
                        // Semaforo registro fotografico
                        const registroFotografico = this.seguimientoSemanal.seguimientoSemanalRegistroFotografico[0];
                        if ( registroFotografico.registroCompletoObservacionApoyo === false ) {
                            this.semaforoRegistroFotografico = 'en-proceso';
                        } 
                        if ( registroFotografico.registroCompletoObservacionApoyo === true ) {
                            this.semaforoRegistroFotografico = 'completo';
                        }
                        // Semaforo comite de obra
                        const comiteObra = this.seguimientoSemanal.seguimientoSemanalRegistrarComiteObra[0];
                        if ( comiteObra.registroCompletoObservacionApoyo === false ) {
                            this.semaforoComiteObra = 'en-proceso';
                        } 
                        if ( comiteObra.registroCompletoObservacionApoyo === true ) {
                            this.semaforoComiteObra = 'completo';
                        }
                    } );
                }
            );
    }

    ngOnInit(): void {
    }

    valuePending( value: number ) {
        if ( value % 5 === 0 ) {
          let semaforoFinanciero = 'sin-diligenciar';
          if ( this.seguimientoSemanal !== undefined ) {
            if ( this.seguimientoSemanal.seguimientoSemanalAvanceFinanciero.length > 0 ) {
              const avanceFinanciero = this.seguimientoSemanal.seguimientoSemanalAvanceFinanciero[0];
              if ( avanceFinanciero.registroCompletoObservacionApoyo === false ) {
                semaforoFinanciero = 'en-proceso';
              }
              if ( avanceFinanciero.registroCompletoObservacionApoyo === true ) {
                semaforoFinanciero = 'completo';
              }
            }
          }
          return semaforoFinanciero;
        } else {
          return 'en-alerta';
        }
    }

    valuePendingSemaforo( tipoAcordeon: string, totalGestion: number, totalAcordeon: number ) {
        if ( tipoAcordeon === 'gestionObra' ) {
            if ( totalGestion > 0 && ( totalGestion < totalAcordeon ) ) {
                this.semaforoGestionObra = 'en-proceso';
            }
            if ( totalGestion > 0 && ( totalGestion === totalAcordeon ) ) {
                this.semaforoGestionObra = 'completo';
            }
        }
        if ( tipoAcordeon === 'reporteActividad' ) {
            if ( totalGestion > 0 && ( totalGestion < totalAcordeon ) ) {
                this.semaforoReporteActividad = 'en-proceso';
            }
            if ( totalGestion > 0 && ( totalGestion === totalAcordeon ) ) {
                this.semaforoReporteActividad = 'completo';
            }
        }
    }

}
