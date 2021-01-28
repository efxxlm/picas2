import { VerificarAvanceSemanalService } from 'src/app/core/_services/verificarAvanceSemanal/verificar-avance-semanal.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { RegistrarAvanceSemanalService } from 'src/app/core/_services/registrarAvanceSemanal/registrar-avance-semanal.service';
import { Location } from '@angular/common';

@Component({
  selector: 'app-ver-detalle-avance-semanal',
  templateUrl: './ver-detalle-avance-semanal.component.html',
  styleUrls: ['./ver-detalle-avance-semanal.component.scss']
})
export class VerDetalleAvanceSemanalComponent implements OnInit {

    seguimientoSemanal: any;
    tipoObservaciones: any;
    semaforoGestionObra = 'sin-diligenciar';

    constructor(
        private location: Location,
        private avanceSemanalSvc: RegistrarAvanceSemanalService,
        private verificarAvanceSemanalSvc: VerificarAvanceSemanalService,
        private activatedRoute: ActivatedRoute )
    {
        this.avanceSemanalSvc
        .getLastSeguimientoSemanalContratacionProyectoIdOrSeguimientoSemanalId( 0,  this.activatedRoute.snapshot.params.idAvance )
            .subscribe(
              seguimiento => {
                  this.seguimientoSemanal = seguimiento;
                  console.log( this.seguimientoSemanal );
                  this.verificarAvanceSemanalSvc.tipoObservaciones()
                    .subscribe( response => this.tipoObservaciones = response );
              }
            );
    }

    ngOnInit(): void {
    }

    getRutaAnterior() {
        this.location.back();
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
