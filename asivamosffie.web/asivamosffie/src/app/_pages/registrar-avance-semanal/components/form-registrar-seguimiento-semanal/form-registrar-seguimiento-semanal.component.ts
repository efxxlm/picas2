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

  constructor(
    private avanceSemanalSvc: RegistrarAvanceSemanalService,
    private activatedRoute: ActivatedRoute ) {
    this.avanceSemanalSvc.getLastSeguimientoSemanalContratacionProyectoIdOrSeguimientoSemanalId( this.activatedRoute.snapshot.params.id, 0 )
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
