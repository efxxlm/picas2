import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { RegisterContractualLiquidationRequestService } from 'src/app/core/_services/registerContractualLiquidationRequest/register-contractual-liquidation-request.service';
import { EstadosSolicitudLiquidacionContractual, EstadosSolicitudLiquidacionContractualCodigo, ListaMenuSolicitudLiquidacion, ListaMenuSolicitudLiquidacionId, TipoObservacionLiquidacionContrato, TipoObservacionLiquidacionContratoCodigo } from 'src/app/_interfaces/estados-solicitud-liquidacion-contractual';
import { InformeFinal } from 'src/app/_interfaces/informe-final';

@Component({
  selector: 'app-verificar-informe-gtlc',
  templateUrl: './verificar-informe-gtlc.component.html',
  styleUrls: ['./verificar-informe-gtlc.component.scss']
})
export class VerificarInformeGtlcComponent implements OnInit {

  proyectoId: number;
  contratacionProyectoId: number;
  datosTabla = [];
  data: any;
  listaMenu: ListaMenuSolicitudLiquidacion = ListaMenuSolicitudLiquidacionId;
  listaTipoObservacionLiquidacionContratacion: TipoObservacionLiquidacionContrato = TipoObservacionLiquidacionContratoCodigo;
  registroCompleto : string;
  informeFinal: InformeFinal;

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
    this.registerContractualLiquidationRequestService.getInformeFinalByProyectoId(proyectoId, contratacionProyectoId, this.listaMenu.gestionarSolicitudLiquidacionContratacion).subscribe(report => {
      if(report != null){
        report.forEach(element => {
          this.datosTabla.push({
            nombreContratistaInterventoria: element.nombreContratistaInterventoria,
            nombreContratistaObra: element.nombreContratistaObra,
            numeroContratoInterventoria: element.numeroContratoInterventoria,
            numeroContratoObra: element.numeroContratoObra,
            informeFinal: element.informeFinal
          });
        })
        if(this.datosTabla.length > 0){
          this.data = this.datosTabla[0];
          this.informeFinal = this.data.informeFinal;
          this.registroCompleto = this.data.registroCompleto ? 'Completo' : 'Incompleto';
        }
      }
    });
  }

}
