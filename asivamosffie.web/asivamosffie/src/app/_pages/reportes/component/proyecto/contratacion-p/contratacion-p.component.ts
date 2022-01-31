import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { FichaProyectoService } from 'src/app/core/_services/fichaProyecto/ficha-proyecto.service';

@Component({
  selector: 'app-contratacion-p',
  templateUrl: './contratacion-p.component.html',
  styleUrls: ['./contratacion-p.component.scss']
})
export class ContratacionPComponent implements OnInit {

  proyectoId: number;
  dataContratacion: any = null;
  dataContratoObra: any = null;
  dataContratoInterventoria: any = null;
  openAcordeon = false;

  /*Tipo Dominio: 83 */
  estadoEtapaProceso={
    inicio : '1',
    evaluacion : '2',
    aprobacion : '3',
    adjudicacion : '4'
  }

  listaProyectosAsociados = [
    {
      numeroContrato: 'N801801',
      llaveMEN: 'LL457326',
      tipoIntervension: 'Remodelación',
      departamento: 'Boyacá',
      municipio: 'Susacón',
      institucionEducativa: 'I.E Nuestra Señora Del Carmen',
      sede: 'Única sede'
    }
  ];

  listaNovedades = [
    {
      fechaSolicitudNovedad: '10/12/2020',
      numeroSolicitud: 'NOV_0001',
      tipoNovedad: 'Prórroga',
      estadoNovedad: 'Aprobada',
      urlSoporte: 'http//:prórroga.onedrive'
    }
  ];

  constructor(
    private fichaProyectoService: FichaProyectoService,
    private route: ActivatedRoute
  ) {
    this.route.params.subscribe((params: Params) => {
      this.proyectoId = params.id;
    });
  }

  ngOnInit(): void {
    this.fichaProyectoService.getInfoContratoByProyectoId(this.proyectoId)
    .subscribe(response => {
        this.dataContratacion = response;
        this.dataContratoObra =  this.dataContratacion?.infoProyectosXContrato?.find(r => r.tipoContratoCodigo == '1');
        this.dataContratoInterventoria =  this.dataContratacion?.infoProyectosXContrato?.find(r => r.tipoContratoCodigo == '2');
        if(this.dataContratoObra != null){
          this.dataContratoObra?.listProcesoSeleccion.forEach(ps => {
            ps.fechaApertura = ps?.procesoSeleccionCronograma.find(r => r.estapaCodigo == this.estadoEtapaProceso.inicio)?.fechaEtapa;
            ps.fechaEvaluacion = ps?.procesoSeleccionCronograma.find(r => r.estapaCodigo == this.estadoEtapaProceso.evaluacion)?.fechaEtapa;
            ps.fechaAdjudicacion = ps?.procesoSeleccionCronograma.find(r => r.estapaCodigo == this.estadoEtapaProceso.adjudicacion)?.fechaEtapa;
            ps.fechaCierre = ps?.procesoSeleccionCronograma.find(r => r.estapaCodigo == this.estadoEtapaProceso.aprobacion)?.fechaEtapa;
          });
        }
    });
  }

  downloadPDF() {
    this.openAcordeon = true;
    setTimeout(() => {
      document.title='Contratación '+this.dataContratacion?.infoProyecto?.llaveMen;
      window.print();
    }, 300);
    window.onafterprint = function(){
      window.location.reload();
    }
  }

}
