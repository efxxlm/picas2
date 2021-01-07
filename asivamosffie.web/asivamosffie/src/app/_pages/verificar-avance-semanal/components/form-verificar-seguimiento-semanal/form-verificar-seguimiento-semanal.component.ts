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
    tipoObservaciones: any;

    constructor(
        private avanceSemanalSvc: RegistrarAvanceSemanalService,
        private activatedRoute: ActivatedRoute,
        private verificarAvanceSemanalSvc: VerificarAvanceSemanalService )
    {
        this.avanceSemanalSvc.getLastSeguimientoSemanalContratacionProyectoIdOrSeguimientoSemanalId( 0, 1224 )
            .subscribe(
                seguimiento => {
                    this.seguimientoSemanal = seguimiento;
                    console.log( this.seguimientoSemanal );
                }
            );
        this.verificarAvanceSemanalSvc.tipoObservaciones()
            .subscribe( response => {
                this.tipoObservaciones = response;
                console.log( this.tipoObservaciones );
            } );
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
