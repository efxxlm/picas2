import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, UrlSegment } from '@angular/router';
import { RegisterContractualLiquidationRequestService } from 'src/app/core/_services/registerContractualLiquidationRequest/register-contractual-liquidation-request.service';

@Component({
  selector: 'app-verificar-requisitos-gtlc',
  templateUrl: './verificar-requisitos-gtlc.component.html',
  styleUrls: ['./verificar-requisitos-gtlc.component.scss']
})
export class VerificarRequisitosGtlcComponent implements OnInit {

  semaforoActualizacionPoliza = 'Incompleto';
  semaforoBalanceFinanciero = 'Incompleto'
  semaforoInformeFinal = 'Incompleto'
  contratacionId: number;
  data: any;
  esRegistroNuevo: boolean;
  esVerDetalle: boolean;

  constructor(
    private route: ActivatedRoute,
    private registerContractualLiquidationRequestService: RegisterContractualLiquidationRequestService
  ) { 
    this.route.params.subscribe((params: Params) => {
      this.contratacionId = params.id;
    });
    this.route.snapshot.url.forEach( ( urlSegment: UrlSegment ) => {
      if ( urlSegment.path === 'verificarRequisitos' ) {
          this.esVerDetalle = false;
          this.esRegistroNuevo = true;
          return;
      }
      if ( urlSegment.path === 'verDetalleEditarRequisitos' ) {
          this.esVerDetalle = false;
          this.esRegistroNuevo = false;
          return;
      }
      if ( urlSegment.path === 'verDetalleRequisitos' ) {
          this.esVerDetalle = true;
          return;
      }
    });
  }


  ngOnInit(): void {
    this.getContratacionProyectoBycontratacionId(this.contratacionId);
  }

  getContratacionProyectoBycontratacionId(contratacionId: number) {
    this.registerContractualLiquidationRequestService.getContratacionByContratacionId(contratacionId).subscribe(response => {
      if(response != null){
        this.data = response[0];
      }
    });
  }

  valueSemaforoActualizacionPoliza(event: any){
    this.semaforoActualizacionPoliza = event;
  }

  valueSemaforoBalanceFinanciero(event: any){
    this.semaforoBalanceFinanciero = event;
  }
  
  valueSemaforoInformeFinal(event: any){
    this.semaforoInformeFinal = event;
  }
}
