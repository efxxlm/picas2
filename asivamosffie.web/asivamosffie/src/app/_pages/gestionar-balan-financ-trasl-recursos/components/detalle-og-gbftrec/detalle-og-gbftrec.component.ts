import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { OrdenPagoService } from 'src/app/core/_services/ordenPago/orden-pago.service';
import { FinancialBalanceService } from 'src/app/core/_services/financialBalance/financial-balance.service';

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
  tablaFacturado : any;
  tablaDescuento : any;
  tablaOtroDescuento : any;

  constructor(
    private route: ActivatedRoute,
    private ordenGiroSvc: OrdenPagoService,
    private financialBalanceService: FinancialBalanceService,
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
              this.seeDetailOdg(this.solicitudPago.ordenGiro.ordenGiroId)
            }
        );
    }
    
  }

  seeDetailOdg(pOrdenGiroId) {
    this.financialBalanceService.getSeeDetailOdg(pOrdenGiroId).subscribe(response => {
      this.tablaFacturado = response.tablaFacturado;
      this.tablaDescuento = response.tablaDescuento;
      this.tablaOtroDescuento = response.tablaOtroDescuento;
    })
   }

}
