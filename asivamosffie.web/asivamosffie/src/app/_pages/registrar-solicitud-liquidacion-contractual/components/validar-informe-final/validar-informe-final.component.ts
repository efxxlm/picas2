import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute, Params } from '@angular/router';
import { RegisterContractualLiquidationRequestService } from 'src/app/core/_services/registerContractualLiquidationRequest/register-contractual-liquidation-request.service';
import { EstadosSolicitudLiquidacionContractual, EstadosSolicitudLiquidacionContractualCodigo, ListaMenuSolicitudLiquidacion, ListaMenuSolicitudLiquidacionId, TipoObservacionLiquidacionContrato, TipoObservacionLiquidacionContratoCodigo } from 'src/app/_interfaces/estados-solicitud-liquidacion-contractual';
import { InformeFinal } from 'src/app/_interfaces/informe-final';

@Component({
  selector: 'app-validar-informe-final',
  templateUrl: './validar-informe-final.component.html',
  styleUrls: ['./validar-informe-final.component.scss']
})
export class ValidarInformeFinalComponent implements OnInit {

  proyectoId: number;
  contratacionProyectoId: number;
  data: any;
  listaMenu: ListaMenuSolicitudLiquidacion = ListaMenuSolicitudLiquidacionId;
  listaTipoObservacionLiquidacionContratacion: TipoObservacionLiquidacionContrato = TipoObservacionLiquidacionContratoCodigo;
  registroCompleto : string;

  constructor(
    private route: ActivatedRoute,
    private registerContractualLiquidationRequestService: RegisterContractualLiquidationRequestService
  ) { 
    this.route.params.subscribe((params: Params) => {
      this.proyectoId = params.proyectoId;
      this.contratacionProyectoId = params.id;
    });
  }

  ngOnInit(): void {
    this.getInformeFinalByProyectoId(this.proyectoId, this.contratacionProyectoId);
  }

  getInformeFinalByProyectoId(proyectoId: number, contratacionProyectoId: number) {
    this.registerContractualLiquidationRequestService.getInformeFinalByProyectoId(proyectoId, contratacionProyectoId, this.listaMenu.registrarSolicitudLiquidacionContratacion).subscribe(report => {
      if(report != null){
        this.data = report[0];
        this.registroCompleto = this.data.registroCompleto ? 'Completo' : 'Incompleto';
      }
    });
  }

}
