import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { ContractualNoveltyService } from 'src/app/core/_services/ContractualNovelty/contractual-novelty.service';
import { NovedadContractual } from 'src/app/_interfaces/novedadContractual';
import * as moment from 'moment';

@Component({
  selector: 'app-ver-detalle-interventoria',
  templateUrl: './ver-detalle-interventoria.component.html',
  styleUrls: ['./ver-detalle-interventoria.component.scss']
})
export class VerDetalleInterventoriaComponent implements OnInit {

  detalleId: string;
  novedad: NovedadContractual;
  fechaFinalizacionContrato : any;
  fechaEstimadaFinalizacion : Date;

  listaObservaciones = [
    {
      fecha: '20/10/2020',
      responsable: 'Supervisor',
      historial: 'No es posible acceder a la ruta de soporte, verificar la URL.'
    }
  ]

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private contractualNoveltyService: ContractualNoveltyService,
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe((params: Params) => {
      this.detalleId = params.id;
      //console.log(this.detalleId);

      this.contractualNoveltyService.getNovedadContractualById( this.detalleId )
        .subscribe( respuesta => {
          this.novedad = respuesta;

          this.fechaFinalizacionContrato = (this.novedad?.contrato?.fechaTerminacionFase2 ? this.novedad?.contrato?.fechaTerminacionFase2 : this.novedad?.contrato?.fechaTerminacion);
          this.fechaFinalizacionContrato = moment( new Date( this.fechaFinalizacionContrato ).setHours( 0, 0, 0, 0 ) );
          respuesta.novedadContractualDescripcion.forEach( d => {
            const fechaInicio = moment( new Date( d?.fechaInicioSuspension ).setHours( 0, 0, 0, 0 ) );
            const fechaFin = moment( new Date( d?.fechaFinSuspension ).setHours( 0, 0, 0, 0 ) );
            const duracionDias = fechaFin.diff( fechaInicio, 'days' );
            d.fechaEstimadaFinalizacion = moment(this.fechaFinalizacionContrato).add(duracionDias, 'days').toDate();
          });
        });

    });
  }

}
