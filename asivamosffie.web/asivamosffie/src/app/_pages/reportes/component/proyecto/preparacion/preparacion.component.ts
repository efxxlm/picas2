import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { FichaProyectoService } from 'src/app/core/_services/fichaProyecto/ficha-proyecto.service';

@Component({
  selector: 'app-preparacion',
  templateUrl: './preparacion.component.html',
  styleUrls: ['./preparacion.component.scss']
})
export class PreparacionComponent implements OnInit {

  proyectoId: number;
  dataPreparacion: any = null;
  dataPreConstruccionObra: any = null;
  dataPreConstruccionInterventoria: any = null;
  dataConstruccionObra: any = null;
  dataConstruccionInterventoria: any = null;
  listaPlanes = [];
  openAcordeon = false;

  constructor(
    private fichaProyectoService: FichaProyectoService,
    private route: ActivatedRoute,
    private commonSvc: CommonService
  ) {
    this.route.params.subscribe((params: Params) => {
      this.proyectoId = params.id;
    });
   }

  ngOnInit(): void {
    this.listaPlanes = [];
    this.fichaProyectoService.getInfoPreparacionByProyectoId(this.proyectoId)
    .subscribe(response => {
      this.dataPreparacion = response;
      if(this.dataPreparacion != null){
        if(this.dataPreparacion?.preconstruccion != null){
           this.dataPreConstruccionObra =  this.dataPreparacion?.preconstruccion?.find(r => r.tipoContratoCodigo == '1');
           this.dataPreConstruccionInterventoria =  this.dataPreparacion?.preconstruccion?.find(r => r.tipoContratoCodigo == '2');
        }
        if(this.dataPreparacion?.construccion != null){
          this.dataConstruccionObra =  this.dataPreparacion?.construccion?.find(r => r.codigoTipoContrato == '1');
          this.dataConstruccionInterventoria =  this.dataPreparacion?.construccion?.find(r => r.codigoTipoContrato == '2');
        }
        if(this.dataConstruccionObra?.planesYProgramas != null){
          this.listaPlanes.push(this.dataConstruccionObra?.planesYProgramas?.planLicenciaVigente);
          this.listaPlanes.push(this.dataConstruccionObra?.planesYProgramas?.planCambioConstructorLicencia);
          this.listaPlanes.push(this.dataConstruccionObra?.planesYProgramas?.planActaApropiacion);
          this.listaPlanes.push(this.dataConstruccionObra?.planesYProgramas?.planResiduosDemolicion);
          this.listaPlanes.push(this.dataConstruccionObra?.planesYProgramas?.planManejoTransito);
          this.listaPlanes.push(this.dataConstruccionObra?.planesYProgramas?.planManejoAmbiental);
          this.listaPlanes.push(this.dataConstruccionObra?.planesYProgramas?.planMansejoAmbiental);
          this.listaPlanes.push(this.dataConstruccionObra?.planesYProgramas?.planProgramaSeguridad);
          this.listaPlanes.push(this.dataConstruccionObra?.planesYProgramas?.planProgramaSalud);
          this.listaPlanes.push(this.dataConstruccionObra?.planesYProgramas?.planInventarioArboreo);
          this.listaPlanes.push(this.dataConstruccionObra?.planesYProgramas?.planAprovechamientoForestal);
          this.listaPlanes.push(this.dataConstruccionObra?.planesYProgramas?.planManejoAguasLluvias);
        }
      }
    });
  }

  descargar(esFlujoInversion: boolean, id: number) {
    this.commonSvc.getFileById(id)
      .subscribe(respuesta => {
        const documento = esFlujoInversion ? 'FlujoInversion.xlsx' : 'ProgramacionObra.xlsx';
        const  blob = new Blob([respuesta], { type: 'application/octet-stream' });
        const  anchor = document.createElement('a');
        anchor.download = documento;
        anchor.href = window.URL.createObjectURL(blob);
        anchor.dataset.downloadurl = ['application/octet-stream', anchor.download, anchor.href].join(':');
        anchor.click();
      });
  }

 downloadPDF() {
    this.openAcordeon = true;
    setTimeout(() => {
      document.title='Preparación '+this.dataPreparacion?.informacion?.llaveMen;
      window.print();
    }, 300);
    window.onafterprint = function(){
      window.location.reload();
    }
  }

}
