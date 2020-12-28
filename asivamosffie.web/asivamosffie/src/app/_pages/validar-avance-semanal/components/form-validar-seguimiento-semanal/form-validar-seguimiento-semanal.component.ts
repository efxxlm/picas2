import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { RegistrarAvanceSemanalService } from 'src/app/core/_services/registrarAvanceSemanal/registrar-avance-semanal.service';

@Component({
  selector: 'app-form-validar-seguimiento-semanal',
  templateUrl: './form-validar-seguimiento-semanal.component.html',
  styleUrls: ['./form-validar-seguimiento-semanal.component.scss']
})
export class FormValidarSeguimientoSemanalComponent implements OnInit {

    seguimientoSemanal: any;
    semaforoAvanceFisico = 'sin-diligenciar';

    constructor(
        private avanceSemanalSvc: RegistrarAvanceSemanalService,
        private activatedRoute: ActivatedRoute )
    {
        this.avanceSemanalSvc
        .getLastSeguimientoSemanalContratacionProyectoIdOrSeguimientoSemanalId( 0, 1224 )
            .subscribe(
              seguimiento => {
                  this.seguimientoSemanal = seguimiento;
                  console.log( this.seguimientoSemanal );
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
