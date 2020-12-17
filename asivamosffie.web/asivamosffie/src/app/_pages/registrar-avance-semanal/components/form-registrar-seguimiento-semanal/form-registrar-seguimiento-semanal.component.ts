import { ActivatedRoute } from '@angular/router';
import { RegistrarAvanceSemanalService } from './../../../../core/_services/registrarAvanceSemanal/registrar-avance-semanal.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-form-registrar-seguimiento-semanal',
  templateUrl: './form-registrar-seguimiento-semanal.component.html',
  styleUrls: ['./form-registrar-seguimiento-semanal.component.scss']
})
export class FormRegistrarSeguimientoSemanalComponent implements OnInit {

  seguimientoSemanal: any;
  semaforoGestionObra: string;
  semaforoReporteActividad: string;
  semaforoRegistroFotografico = 'sin-diligenciar';
  semaforoComiteObra = 'sin-diligenciar';

  constructor(
    private avanceSemanalSvc: RegistrarAvanceSemanalService,
    private activatedRoute: ActivatedRoute ) {
    this.avanceSemanalSvc.getLastSeguimientoSemanalContratacionProyectoIdOrSeguimientoSemanalId( this.activatedRoute.snapshot.params.id, 0 )
      .subscribe(
        seguimiento => {
          this.seguimientoSemanal = seguimiento;
          console.log( this.seguimientoSemanal );

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
        }
      );
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

}
