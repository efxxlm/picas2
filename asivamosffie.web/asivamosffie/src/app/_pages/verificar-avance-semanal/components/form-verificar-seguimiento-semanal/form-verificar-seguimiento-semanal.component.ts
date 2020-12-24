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

}
