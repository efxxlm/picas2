import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { FichaProyectoService } from 'src/app/core/_services/fichaProyecto/ficha-proyecto.service';

@Component({
  selector: 'app-resumen-p',
  templateUrl: './resumen-p.component.html',
  styleUrls: ['./resumen-p.component.scss']
})
export class ResumenPComponent implements OnInit {

  proyectoId: number;
  dataResumen: any = null;
  dataConstruccionObra: any = null;
  dataConstruccionInterventoria: any = null;
  openAcordeon = false;
  displayedColumns: string[] = [ 'nombreAportante', 'valorAportante', 'fuente', 'uso', 'valorUso' ];

  listaAlcance = [
    {
      infraestructura: 'Laboratorio de quimíca',
      cantidad: '4'
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
    this.fichaProyectoService.getInfoResumenByProyectoId(this.proyectoId)
    .subscribe(response => {
        this.dataResumen = response;
        if(this.dataResumen?.infoContratos != null){
          this.dataConstruccionObra =  this.dataResumen?.infoContratos?.find(r => r.tipoSolicitudCodigo == '1');
          this.dataConstruccionInterventoria =  this.dataResumen?.infoContratos?.find(r => r.tipoSolicitudCodigo == '2');
          if(this.dataConstruccionObra != null){
            let aportantes = [];
            if(this.dataConstruccionObra?.infoContrato != null){
              this.dataConstruccionObra?.infoContrato.forEach(element => {
                let aportante = aportantes.find(r => r.contratacionProyectoAportanteId == element.contratacionProyectoAportanteId);
                let fuentes = [];
                let usos =[];
                usos.push({
                  uso: element.uso,
                  valorUso: element.valorUso
                });
                if(aportante != null && aportante != undefined){
                  aportante.valorAportante += element.valorUso ?? 0;
                  let fuente = aportante?.fuentes.find(r => r.fuenteFinanciacionId == element.fuenteFinanciacionId);
                  if(fuente != null && fuente != undefined){
                    fuente.usos.push({
                      uso: element.uso,
                      valorUso: element.valorUso
                    });
                  }else{
                    aportante.fuentes.push({
                      fuenteFinanciacionId: element.fuenteFinanciacionId,
                      fuente: element.fuente,
                      usos: usos
                    });
                  }
                }else{
                  fuentes.push({
                    fuenteFinanciacionId: element.fuenteFinanciacionId,
                    fuente: element.fuente,
                    usos: usos
                  });
                  aportantes.push({
                    contratacionProyectoAportanteId: element.contratacionProyectoAportanteId,
                    nombreAportante: element.nombreAportante,
                    valorAportante: element.valorUso,
                    fuentes: fuentes
                  });
                }
              });
              this.dataConstruccionObra.listFuentes = aportantes;
            }
          }
          if(this.dataConstruccionInterventoria != null){
            let aportantes = [];
            if(this.dataConstruccionInterventoria?.infoContrato != null){
              this.dataConstruccionInterventoria?.infoContrato.forEach(element => {
                let aportante = aportantes.find(r => r.contratacionProyectoAportanteId == element.contratacionProyectoAportanteId);
                let fuentes = [];
                let usos =[];
                usos.push({
                  uso: element.uso,
                  valorUso: element.valorUso
                });
                if(aportante != null && aportante != undefined){
                  aportante.valorAportante += element.valorUso ?? 0;
                  let fuente = aportante?.fuentes.find(r => r.fuenteFinanciacionId == element.fuenteFinanciacionId);
                  if(fuente != null && fuente != undefined){
                    fuente.usos.push({
                      uso: element.uso,
                      valorUso: element.valorUso
                    });
                  }else{
                    aportante.fuentes.push({
                      fuenteFinanciacionId: element.fuenteFinanciacionId,
                      fuente: element.fuente,
                      usos: usos
                    });
                  }
                }else{
                  fuentes.push({
                    fuenteFinanciacionId: element.fuenteFinanciacionId,
                    fuente: element.fuente,
                    usos: usos
                  });
                  aportantes.push({
                    contratacionProyectoAportanteId: element.contratacionProyectoAportanteId,
                    nombreAportante: element.nombreAportante,
                    valorAportante: element.valorUso,
                    fuentes: fuentes
                  });
                }
              });
              this.dataConstruccionInterventoria.listFuentes = aportantes;
            }
          }
        }
    });
  }


 downloadPDF() {
  this.openAcordeon = true;
  setTimeout(() => {
    document.title='Resumen '+this.dataResumen?.infoProyecto?.llaveMen;
    window.print();
  }, 300);
  window.onafterprint = function(){
    window.location.reload();
  }
}

}
