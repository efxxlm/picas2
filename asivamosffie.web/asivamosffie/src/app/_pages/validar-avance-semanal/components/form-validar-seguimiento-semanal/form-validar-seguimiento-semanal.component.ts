import { VerificarAvanceSemanalService } from './../../../../core/_services/verificarAvanceSemanal/verificar-avance-semanal.service';
import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { RegistrarAvanceSemanalService } from 'src/app/core/_services/registrarAvanceSemanal/registrar-avance-semanal.service';

@Component({
  selector: 'app-form-validar-seguimiento-semanal',
  templateUrl: './form-validar-seguimiento-semanal.component.html',
  styleUrls: ['./form-validar-seguimiento-semanal.component.scss']
})
export class FormValidarSeguimientoSemanalComponent implements OnInit {

    seguimientoSemanal: any;
    tipoObservaciones: any;
    semaforoAvanceFisico = 'sin-diligenciar';
    semaforoGestionObra = 'sin-diligenciar';
    semaforoReporteActividad = 'sin-diligenciar';
    semaforoRegistroFotografico = 'sin-diligenciar';
    semaforoComiteObra = 'sin-diligenciar';

    constructor(
        private avanceSemanalSvc: RegistrarAvanceSemanalService,
        private verificarAvanceSemanalSvc: VerificarAvanceSemanalService,
        private activatedRoute: ActivatedRoute )
    {
        this.avanceSemanalSvc
        .getLastSeguimientoSemanalContratacionProyectoIdOrSeguimientoSemanalId( 0, this.activatedRoute.snapshot.params.id )
            .subscribe(
              seguimiento => {
                  this.seguimientoSemanal = seguimiento;
                  console.log( this.seguimientoSemanal );
                  this.verificarAvanceSemanalSvc.tipoObservaciones()
                  .subscribe( response => {
                      this.tipoObservaciones = response;
                      console.log( this.tipoObservaciones );
                      // Semaforo avance fisico
                      const avanceFisico = this.seguimientoSemanal.seguimientoSemanalAvanceFisico[0];
                      if ( avanceFisico.registroCompletoObservacionSupervisor === false ) {
                          this.semaforoAvanceFisico = 'en-proceso';
                      } 
                      if ( avanceFisico.registroCompletoObservacionSupervisor === true ) {
                          this.semaforoAvanceFisico = 'completo';
                      }
                      // Semaforo registro fotografico
                      const registroFotografico = this.seguimientoSemanal.seguimientoSemanalRegistroFotografico[0];
                      if ( registroFotografico.registroCompletoObservacionSupervisor === false ) {
                          this.semaforoRegistroFotografico = 'en-proceso';
                      } 
                      if ( registroFotografico.registroCompletoObservacionSupervisor === true ) {
                          this.semaforoRegistroFotografico = 'completo';
                      }
                      // Semaforo comite de obra
                      const comiteObra = this.seguimientoSemanal.seguimientoSemanalRegistrarComiteObra[0];
                      if ( comiteObra.registroCompletoObservacionSupervisor === false ) {
                          this.semaforoComiteObra = 'en-proceso';
                      } 
                      if ( comiteObra.registroCompletoObservacionSupervisor === true ) {
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
              if ( avanceFinanciero.registroCompletoObservacionSupervisor === false ) {
                semaforoFinanciero = 'en-proceso';
              }
              if ( avanceFinanciero.registroCompletoObservacionSupervisor === true ) {
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
