import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { ContractualNoveltyService } from 'src/app/core/_services/ContractualNovelty/contractual-novelty.service';
import { NovedadContractual } from 'src/app/_interfaces/novedadContractual';

@Component({
  selector: 'app-ver-detalle',
  templateUrl: './ver-detalle.component.html',
  styleUrls: ['./ver-detalle.component.scss']
})
export class VerDetalleComponent implements OnInit {

  detalleId: string;
  novedad: NovedadContractual;

  listaObservaciones = [
    {
      fecha: '20/10/2020',
      responsable: 'Supervisor',
      historial: 'No es posible acceder a la ruta de soporte, verificar la URL.'
    }
  ]

  tipoNovedadNombre: string = '';

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

          respuesta.novedadContractualDescripcion.forEach( d => {
            if ( d.tipoNovedadCodigo === '3' )
              

              this.tipoNovedadNombre = this.tipoNovedadNombre + d.nombreTipoNovedad + ', ' 

          });

        });

    });
  }

}
