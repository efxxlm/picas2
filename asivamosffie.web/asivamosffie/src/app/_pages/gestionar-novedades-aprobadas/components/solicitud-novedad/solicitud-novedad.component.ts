import { Component, Input, OnInit } from '@angular/core';
import { ContractualNoveltyService } from 'src/app/core/_services/ContractualNovelty/contractual-novelty.service';
import { NovedadContractual, NovedadContractualObservaciones } from 'src/app/_interfaces/novedadContractual';
import * as moment from 'moment';

@Component({
  selector: 'app-solicitud-novedad',
  templateUrl: './solicitud-novedad.component.html',
  styleUrls: ['./solicitud-novedad.component.scss']
})
export class SolicitudNovedadComponent implements OnInit {

  @Input() novedad: NovedadContractual;
  fechaFinalizacionContrato : any;
  fechaEstimadaFinalizacion : Date;

  tipoNovedadNombre: string = '';

  listaObservaciones = [
    {
      llaveMEN: 'LJ776554',
      departamento: 'Atlántico',
      municipio: 'Galapa',
      institucionEducativa: 'I.E. María Villa Campo',
      sede: 'Única sede'
    }
  ]

  constructor(
    private contractualNoveltyService: ContractualNoveltyService,

  ) { }

  ngOnInit(): void {
    this.fechaFinalizacionContrato = (this.novedad?.contrato?.fechaEstimadaFinalizacion ? this.novedad?.contrato?.fechaEstimadaFinalizacion : this.novedad?.contrato?.fechaTerminacionFase2 ? this.novedad?.contrato?.fechaTerminacionFase2 : this.novedad?.contrato?.fechaTerminacion);
    this.fechaFinalizacionContrato = moment( new Date( this.fechaFinalizacionContrato ).setHours( 0, 0, 0, 0 ) );

    this.novedad.novedadContractualDescripcion.forEach(d => {
        this.tipoNovedadNombre = this.tipoNovedadNombre + d.nombreTipoNovedad + ', ' ;
        const fechaInicio = moment( new Date( d?.fechaInicioSuspension ).setHours( 0, 0, 0, 0 ) );
        const fechaFin = moment( new Date( d?.fechaFinSuspension ).setHours( 0, 0, 0, 0 ) );
        const duracionDias = fechaFin.diff( fechaInicio, 'days' );

        d.fechaEstimadaFinalizacion = moment(this.fechaFinalizacionContrato).add(duracionDias, 'days').toDate();
    });

    //this.contractualNoveltyService.get
  }

}
