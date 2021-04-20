import { ActivatedRoute, UrlSegment } from '@angular/router';
import { RegistrarAvanceSemanalService } from './../../../../core/_services/registrarAvanceSemanal/registrar-avance-semanal.service';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { VerificarAvanceSemanalService } from 'src/app/core/_services/verificarAvanceSemanal/verificar-avance-semanal.service';
import { GuardadoParcialAvanceSemanalService } from 'src/app/core/_services/guardadoParcialAvanceSemanal/guardado-parcial-avance-semanal.service';

@Component({
  selector: 'app-form-registrar-seguimiento-semanal',
  templateUrl: './form-registrar-seguimiento-semanal.component.html',
  styleUrls: ['./form-registrar-seguimiento-semanal.component.scss']
})
export class FormRegistrarSeguimientoSemanalComponent implements OnInit, OnDestroy {

  seguimientoSemanal: any;
  tipoObservaciones: any;
  semaforoGestionObra: string;
  semaforoReporteActividad: string;
  semaforoAvanceFisico = 'sin-diligenciar';
  semaforoRegistroFotografico = 'sin-diligenciar';
  semaforoComiteObra = 'sin-diligenciar';
  esRegistroNuevo: any;
  dataGestionAmbiental: any;
  tieneObsAvanceFinanciero = false;

  constructor(
    private avanceSemanalSvc: RegistrarAvanceSemanalService,
    private verificarAvanceSemanalSvc: VerificarAvanceSemanalService,
    private guardadoParcialAvanceSemanalSvc: GuardadoParcialAvanceSemanalService,
    private activatedRoute: ActivatedRoute )
  {
    this.activatedRoute.snapshot.url.forEach( ( urlSegment: UrlSegment ) => {
      if ( urlSegment.path === 'registroSeguimientoSemanal' ) {
        this.esRegistroNuevo = true;
      }
      if ( urlSegment.path === 'verDetalleEditar' ) {
        this.esRegistroNuevo = false;
      }
    } );
      
    this.avanceSemanalSvc.getLastSeguimientoSemanalContratacionProyectoIdOrSeguimientoSemanalId( this.activatedRoute.snapshot.params.id, 0 )
      .subscribe(
        seguimiento => {
          this.verificarAvanceSemanalSvc.tipoObservaciones()
            .subscribe( tipoObservaciones => {
              this.tipoObservaciones = tipoObservaciones;
              this.seguimientoSemanal = seguimiento;
              // console.log( this.seguimientoSemanal, this.tipoObservaciones );
              // Semaforo avance fisico
              if ( this.seguimientoSemanal.seguimientoSemanalAvanceFisico.length > 0 ) {
                const avanceFisico = this.seguimientoSemanal.seguimientoSemanalAvanceFisico[0];
                if ( avanceFisico.registroCompleto === false ) {
                  this.semaforoAvanceFisico = 'en-proceso';
                }
                if ( avanceFisico.registroCompleto === true ) {
                  this.semaforoAvanceFisico = 'completo';
                }
              }
              // Semaforo registro fotografico
              if ( this.seguimientoSemanal.seguimientoSemanalRegistroFotografico.length > 0 ) {
                const registroFotografico = this.seguimientoSemanal.seguimientoSemanalRegistroFotografico[0];
                if ( registroFotografico.urlSoporteFotografico !== undefined && registroFotografico.registroCompleto === false ) {
                  this.semaforoRegistroFotografico = 'en-proceso';
                }
                if ( registroFotografico.registroCompleto === true ) {
                  this.semaforoRegistroFotografico = 'completo';
                }
              }
              // Semaforo comite obra
              if ( this.seguimientoSemanal.seguimientoSemanalRegistrarComiteObra.length > 0 ) {
                const comiteObra = this.seguimientoSemanal.seguimientoSemanalRegistrarComiteObra[0];
                if ( comiteObra.fechaComite !== undefined && comiteObra.registroCompleto === false ) {
                  this.semaforoComiteObra = 'en-proceso';
                }
                if ( comiteObra.registroCompleto === true ) {
                  this.semaforoComiteObra = 'completo';
                }
              }
            } );
        }
      );
  }

  ngOnDestroy() {
    this.guardadoParcialAvanceSemanalSvc.getGuardadoParcial( this.seguimientoSemanal )
  }

  ngOnInit(): void {
  }

  valuePendingSemaforo( value: string, tipoSemaforo: string ) {
    if ( tipoSemaforo === 'reporteActividad' ) {
      this.semaforoReporteActividad = value;
    }
    if ( tipoSemaforo === 'gestionObra' ) {
      this.semaforoGestionObra = value;
    }
  }

  valuePending( value: number ) {
    if ( value % 5 === 0 ) {
      let semaforoFinanciero = 'sin-diligenciar';
      if ( this.seguimientoSemanal !== undefined ) {
        if ( this.seguimientoSemanal.seguimientoSemanalAvanceFinanciero.length > 0 ) {
          const avanceFinanciero = this.seguimientoSemanal.seguimientoSemanalAvanceFinanciero[0];
          if ( avanceFinanciero.requiereObservacion !== undefined && avanceFinanciero.registroCompleto === false ) {
            semaforoFinanciero = 'en-proceso';
          }
          if ( avanceFinanciero.registroCompleto === true ) {
            semaforoFinanciero = 'completo';
          }
        }
      }
      return semaforoFinanciero;
    } else {
      return 'en-alerta';
    }
  }

}
