import { ActivatedRoute } from '@angular/router';
import { RegistrarAvanceSemanalService } from './../../../../core/_services/registrarAvanceSemanal/registrar-avance-semanal.service';
import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';

@Component({
  selector: 'app-ver-detalle-muestras',
  templateUrl: './ver-detalle-muestras.component.html',
  styleUrls: ['./ver-detalle-muestras.component.scss']
})
export class VerDetalleMuestrasComponent implements OnInit {

    ensayoLaboratorio: any;

    constructor(
      private location: Location,
      private avanceSemanalSvc: RegistrarAvanceSemanalService,
      private activatedRoute: ActivatedRoute )
    {
      this.avanceSemanalSvc.getEnsayoLaboratorioMuestras( Number( this.activatedRoute.snapshot.params.idEnsayo ) )
        .subscribe(
          response => {
              this.ensayoLaboratorio = response;
          }
        );
    }

    ngOnInit(): void {
    }

    getRutaAnterior() {
      this.location.back();
    }

}
