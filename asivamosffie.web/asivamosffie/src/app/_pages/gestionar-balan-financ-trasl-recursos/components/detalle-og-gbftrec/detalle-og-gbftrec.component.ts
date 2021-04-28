import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { OrdenPagoService } from 'src/app/core/_services/ordenPago/orden-pago.service';

@Component({
  selector: 'app-detalle-og-gbftrec',
  templateUrl: './detalle-og-gbftrec.component.html',
  styleUrls: ['./detalle-og-gbftrec.component.scss']
})
export class DetalleOgGbftrecComponent implements OnInit {

  solicitudPagoId : number;
  solicitudPago: any;
  contrato: any;
  contratoInfo : any;

  constructor(
    private route: ActivatedRoute,
    private ordenGiroSvc: OrdenPagoService,
  ) { }

  ngOnInit(): void {

    this.route.params.subscribe((params: Params) => {
      this.solicitudPagoId = params.id;
    });

    if(this.solicitudPagoId != null){
        this.ordenGiroSvc.getSolicitudPagoBySolicitudPagoId( this.solicitudPagoId)
        .subscribe(
            response => {
              this.solicitudPago = response;
              this.contrato = response[ 'contratoSon' ];
              this.contratoInfo = this.contrato.listProyectos[1];
            }
        );
    }
  }

}
