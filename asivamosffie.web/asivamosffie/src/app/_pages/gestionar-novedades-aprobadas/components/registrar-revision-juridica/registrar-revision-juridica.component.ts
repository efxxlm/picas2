import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { ContractualNoveltyService } from 'src/app/core/_services/ContractualNovelty/contractual-novelty.service';
import { NovedadContractual } from 'src/app/_interfaces/novedadContractual';

@Component({
  selector: 'app-registrar-revision-juridica',
  templateUrl: './registrar-revision-juridica.component.html',
  styleUrls: ['./registrar-revision-juridica.component.scss']
})
export class RegistrarRevisionJuridicaComponent implements OnInit {

  detalleId: string;
  novedad: NovedadContractual;
  tieneAdicion: boolean = false;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private contractualNoveltyService: ContractualNoveltyService,
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe((params: Params) => {
      this.detalleId = params.id;

      this.contractualNoveltyService.getNovedadContractualById( this.detalleId )
        .subscribe( respuesta => {
          this.novedad = respuesta;

          respuesta.novedadContractualDescripcion.forEach( d => {
            if ( d.tipoNovedadCodigo === '3' )
              this.tieneAdicion = true;
          });

        });

      console.log(this.detalleId);
    });
  }

}
