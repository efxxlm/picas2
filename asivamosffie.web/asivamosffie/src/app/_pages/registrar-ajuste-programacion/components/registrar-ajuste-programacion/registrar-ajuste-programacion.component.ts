import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { ContractualNoveltyService } from 'src/app/core/_services/ContractualNovelty/contractual-novelty.service';
import { NovedadContractual } from 'src/app/_interfaces/novedadContractual';
import * as moment from 'moment';

@Component({
  selector: 'app-registrar-ajuste-programacion',
  templateUrl: './registrar-ajuste-programacion.component.html',
  styleUrls: ['./registrar-ajuste-programacion.component.scss']
})
export class RegistrarAjusteProgramacionComponent implements OnInit {

  detalleId: string;
  ajusteProgramacionInfo: any;
  novedad: NovedadContractual;
  fechaFinalizacionContrato : any;
  fechaEstimadaFinalizacion : Date;
  presupuestoAdicionalSolicitado = 0; //X novedad
  esAdicion: boolean = false;
  modificaFecha: boolean = false;
  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private contractualNoveltyService: ContractualNoveltyService
  )
  {
    if (this.router.getCurrentNavigation().extras.replaceUrl) {
      this.router.navigateByUrl('/registrarAjusteProgramacion');
      return;
    };

    if (this.router.getCurrentNavigation().extras.state){
      console.log( this.router.getCurrentNavigation().extras.state.ajusteProgramacion )
      this.ajusteProgramacionInfo = this.router.getCurrentNavigation().extras.state.ajusteProgramacion
    }


   }

  ngOnInit(): void {
    this.route.params.subscribe((params: Params) => {
      this.detalleId = params.id;
      this.contractualNoveltyService.getNovedadContractualById(this.ajusteProgramacionInfo?.novedadContractualId)
      .subscribe(respuesta => {
        this.novedad = respuesta;
        this.fechaFinalizacionContrato = (this.novedad?.contrato?.fechaEstimadaFinalizacion ? this.novedad?.contrato?.fechaEstimadaFinalizacion : this.novedad?.contrato?.fechaTerminacionFase2 ? this.novedad?.contrato?.fechaTerminacionFase2 : this.novedad?.contrato?.fechaTerminacion);
        this.fechaFinalizacionContrato = moment( new Date( this.fechaFinalizacionContrato ).setHours( 0, 0, 0, 0 ) );
        respuesta.novedadContractualDescripcion.forEach( d => {
          if(d.tipoNovedadCodigo == '3')
            this.esAdicion = true;
          if(d.tipoNovedadCodigo == '1' || d.tipoNovedadCodigo == '2' || d.tipoNovedadCodigo == '4')
            this.modificaFecha = true;
          const fechaInicio = moment( new Date( d?.fechaInicioSuspension ).setHours( 0, 0, 0, 0 ) );
          const fechaFin = moment( new Date( d?.fechaFinSuspension ).setHours( 0, 0, 0, 0 ) );
          const duracionDias = fechaFin.diff( fechaInicio, 'days' );
          this.presupuestoAdicionalSolicitado += d.presupuestoAdicionalSolicitado ?? 0;
          d.fechaEstimadaFinalizacion = moment(this.fechaFinalizacionContrato).add(duracionDias, 'days').toDate();
        });
      });
    });
  }

}
