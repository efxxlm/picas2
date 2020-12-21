import { ActivatedRoute } from '@angular/router';
import { RegistrarAvanceSemanalService } from './../../../../core/_services/registrarAvanceSemanal/registrar-avance-semanal.service';
import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';

@Component({
  selector: 'app-ver-detalle-avance-semanal',
  templateUrl: './ver-detalle-avance-semanal.component.html',
  styleUrls: ['./ver-detalle-avance-semanal.component.scss']
})
export class VerDetalleAvanceSemanalComponent implements OnInit {

    seguimientoSemanal: any;

    constructor(
        private location: Location,
        private avanceSemanalSvc: RegistrarAvanceSemanalService,
        private activatedRoute: ActivatedRoute )
    {
        this.avanceSemanalSvc
            .getLastSeguimientoSemanalContratacionProyectoIdOrSeguimientoSemanalId( 0,  this.activatedRoute.snapshot.params.idAvance )
                .subscribe(
                  seguimiento => {
                      this.seguimientoSemanal = seguimiento;
                      console.log( this.seguimientoSemanal );
                  }
                );
    }

    ngOnInit(): void {
    }

    getRutaAnterior() {
        this.location.back();
    }


}
