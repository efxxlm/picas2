import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { ContractualNoveltyService } from 'src/app/core/_services/ContractualNovelty/contractual-novelty.service';
import { NovedadContractual } from 'src/app/_interfaces/novedadContractual';

@Component({
  selector: 'app-ver-detalle-solic-nov-obra',
  templateUrl: './ver-detalle-solic-nov-obra.component.html',
  styleUrls: ['./ver-detalle-solic-nov-obra.component.scss']
})
export class VerDetalleSolicNovObraComponent implements OnInit {

  detalleId: string;
  novedad: NovedadContractual;
  
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
        });

    });
  }

}
