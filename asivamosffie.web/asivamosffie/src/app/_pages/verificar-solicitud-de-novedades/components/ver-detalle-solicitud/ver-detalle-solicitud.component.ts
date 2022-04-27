import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { ContractualNoveltyService } from 'src/app/core/_services/ContractualNovelty/contractual-novelty.service';
import { NovedadContractual } from 'src/app/_interfaces/novedadContractual';
import * as moment from 'moment';

@Component({
  selector: 'app-ver-detalle-solicitud',
  templateUrl: './ver-detalle-solicitud.component.html',
  styleUrls: ['./ver-detalle-solicitud.component.scss']
})
export class VerDetalleSolicitudComponent implements OnInit {

  detalleId: string;
  novedad: NovedadContractual;
  fechaFinalizacionContrato : any;
  fechaEstimadaFinalizacion : Date;

  presupuestoModificado: number;
  plazoDiasModificado: number;
  plazoMesesModificado: number;
  validaParaModificar = false;

  listaObservaciones = [
    {
      fecha: '25/10/2020',
      responsable: 'Supervisor',
      historial: 'Los documentos de soporte no cumplen con los requerimientos solicitados para registrar una novedad.'
    },
    {
      fecha: '20/10/2020',
      responsable: 'Supervisor',
      historial: 'Es necesario revisar a detalle los soportes entregados, por lo tanto, hay que evaluar si en efecto no hay observaciones a la solicitud de novedad.'
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
          if(this.novedad?.estadoCodigo != "12" && this.novedad?.estadoCodigo != "19" && this.novedad?.estadoCodigo != "25" && this.novedad?.estadoCodigo != "26" && this.novedad?.estadoCodigo != null && this.novedad?.estadoCodigo != "26"  )
            this.validaParaModificar = true;

          this.fechaFinalizacionContrato = this.novedad?.datosContratoProyectoModificadosXNovedad[0]?.fechaFinContrato;

          //si el estado es en proceso y no debe quitarse aun hay que quitar esto, falta por definir
          this.fechaEstimadaFinalizacion = this.novedad?.datosContratoProyectoModificadosXNovedad[0]?.fechaEstimadaFinProyecto;
          this.presupuestoModificado =  this.novedad?.datosContratoProyectoModificadosXNovedad[0]?.valorTotalProyecto;
          this.plazoDiasModificado =  this.novedad?.datosContratoProyectoModificadosXNovedad[0]?.plazoEstimadoDiasProyecto;
          this.plazoMesesModificado = (this.novedad?.datosContratoProyectoModificadosXNovedad[0]?.plazoEstimadoMesesProyecto * 30);

        });

    });
  }

  updateFechaEstimada(pFechaFin: any, PffechaInicio: any){
    const fechaInicio = moment( new Date( PffechaInicio ).setHours( 0, 0, 0, 0 ) );
    const fechaFin = moment( new Date( pFechaFin ).setHours( 0, 0, 0, 0 ) );
    const duracionDias = fechaFin.diff( fechaInicio, 'days' );
    let rFecha;

    if(this.validaParaModificar == true){
      rFecha = moment(this.fechaEstimadaFinalizacion).add(duracionDias, 'days').toDate();
    }else{
      this.fechaEstimadaFinalizacion;
    }
    return rFecha;
  }

  valuePresupuesto(presupuestoAdicional: number){
    return this.presupuestoModificado + (this.validaParaModificar == true ? (presupuestoAdicional > 0 ? presupuestoAdicional : 0) : 0);
  }

  valuePlazoProyecto(meses: number, dias: number){
    let diasAdicionales = this.plazoDiasModificado + (this.validaParaModificar == true ? (dias > 0 ? dias : 0) : 0);
    let mesesAdicionales = this.plazoMesesModificado + (this.validaParaModificar == true ? (meses > 0 ? meses * 30 : 0) : 0);

    let pDias = Math.trunc((diasAdicionales + mesesAdicionales)%30);
    let pMeses = Math.trunc((diasAdicionales + mesesAdicionales)/30);

    return pMeses + " Meses / " + pDias + " Días";
  }

}
