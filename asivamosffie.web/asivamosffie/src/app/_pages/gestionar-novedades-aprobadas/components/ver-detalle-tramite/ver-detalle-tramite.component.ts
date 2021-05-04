import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { ContractualNoveltyService } from 'src/app/core/_services/ContractualNovelty/contractual-novelty.service';
import { NovedadContractual, NovedadContractualAportante } from 'src/app/_interfaces/novedadContractual';

@Component({
  selector: 'app-ver-detalle-tramite',
  templateUrl: './ver-detalle-tramite.component.html',
  styleUrls: ['./ver-detalle-tramite.component.scss']
})
export class VerDetalleTramiteComponent implements OnInit {

  detalleId: string;
  novedad: NovedadContractual;
  tieneAdicion: boolean = false;

  detallarSolicitud = []
  tipoNovedadNombre: string = '';

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private contractualNoveltyService: ContractualNoveltyService,
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe((params: Params) => {
      this.detalleId = params.id;

      this.contractualNoveltyService.getNovedadContractualById(this.detalleId)
        .subscribe(respuesta => {
          this.novedad = respuesta;

          respuesta.novedadContractualDescripcion.forEach(d => {
            if (d.tipoNovedadCodigo === '3')
              this.tieneAdicion = true;

              this.tipoNovedadNombre = this.tipoNovedadNombre + d.nombreTipoNovedad + ', ' 
          });


          if (this.tieneAdicion === true) {
            this.novedad.novedadContractualAportante.forEach( na => {
              na.componenteAportanteNovedad.forEach( ca => {
                ca.componenteFuenteNovedad.forEach( cf => {

                  cf.componenteUsoNovedad.forEach( cu => {

                    this.detallarSolicitud.push(
                      { 
                        aportante: na.nombreAportante,
                        valorAportante: na.valorAporte,
                        componente: ca.nombreTipoComponente,
                        fase: ca.nombrefase,
                        uso: cu.nombreUso,
                        valorUso: cu.valorUso 
                      })
  
                  });

                });
                

              });

            });
          }


        });

      console.log(this.detalleId);
    });
  }

}
