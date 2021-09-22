import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { ContractualNoveltyService } from 'src/app/core/_services/ContractualNovelty/contractual-novelty.service';
import { NovedadContractual } from 'src/app/_interfaces/novedadContractual';
import * as moment from 'moment';

@Component({
  selector: 'app-ver-detalle-solic-nov-intervn',
  templateUrl: './ver-detalle-solic-nov-intervn.component.html',
  styleUrls: ['./ver-detalle-solic-nov-intervn.component.scss']
})
export class VerDetalleSolicNovIntervnComponent implements OnInit {

  detalleId: string;
  novedad: NovedadContractual;
  fechaFinalizacionContrato : any;
  fechaEstimadaFinalizacion : Date;

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
