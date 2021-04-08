import { Component, Input } from '@angular/core';
import { RegisterContractualLiquidationRequestService } from 'src/app/core/_services/registerContractualLiquidationRequest/register-contractual-liquidation-request.service';
import { ListaMenuSolicitudLiquidacion, ListaMenuSolicitudLiquidacionId, TipoObservacionLiquidacionContrato, TipoObservacionLiquidacionContratoCodigo } from 'src/app/_interfaces/estados-solicitud-liquidacion-contractual';
import { HistoricoLiquidacionContratacionObservacion } from 'src/app/_interfaces/liquidacionContratacionObservacion';

@Component({
  selector: 'app-observaciones-supervisor',
  templateUrl: './observaciones-supervisor.component.html',
  styleUrls: ['./observaciones-supervisor.component.scss']
})
export class ObservacionesSupervisorComponent {

  @Input() contratacionProyectoId: any;
  @Input() tipoObservacionCodigo: string;
  @Input() menuId: any;
  @Input() padreId: number;
  @Input() esVerDetalle: boolean;

  txt_pregunta: string;
  txt_pregunta_detalle: string;
  historicoLiquidacionContratacionObservacion: HistoricoLiquidacionContratacionObservacion;
  listaTipoObservacionLiquidacionContratacion: TipoObservacionLiquidacionContrato = TipoObservacionLiquidacionContratoCodigo;
  listaMenu: ListaMenuSolicitudLiquidacion = ListaMenuSolicitudLiquidacionId;
  observacionApoyo: any;
  observacionDetalle: any;

  constructor(
    private registerContractualLiquidationRequestService: RegisterContractualLiquidationRequestService
  ) {}

  ngOnInit(): void {
    if(this.esVerDetalle){
      this.registerContractualLiquidationRequestService.getObservacionLiquidacionContratacionByMenuIdAndContratacionProyectoId(
        this.listaMenu.aprobarSolicitudLiquidacionContratacion,
        this.contratacionProyectoId,
        this.padreId,
        this.tipoObservacionCodigo
      ).subscribe(response => {
        if(response != null && response.length > 0){
          this.observacionDetalle = response[0];
        }
      });
    }
    this.getHistoricoObservacionLiquidacionContratacionByMenuIdAndContratacionProyectoId();
    if(this.tipoObservacionCodigo == this.listaTipoObservacionLiquidacionContratacion.informeFinal){
        this.txt_pregunta = "¿Desde el apoyo a la supervisión tiene observaciones frente al informe final que deban ser tenidas en cuenta en la liquidación?:";
        this.txt_pregunta_detalle = "¿Tiene observaciones frente a el informe final que deban ser tenidas en cuenta en la liquidación?:";
      }else if(this.tipoObservacionCodigo == this.listaTipoObservacionLiquidacionContratacion.balanceFinanciero){
        this.txt_pregunta = "¿Desde el apoyo a la tiene observaciones frente al balance financiero que deban ser tenidas en cuenta en la liquidación?:";
        this.txt_pregunta_detalle = "¿Desde la supervisión tiene observaciones frente a el balance que deban ser tenidas en cuenta en la liquidación?:";
      }else if(this.tipoObservacionCodigo == this.listaTipoObservacionLiquidacionContratacion.actualizacionPoliza){
      this.txt_pregunta = "¿Tiene observaciones frente a la actualización de la póliza que deban ser tenidas en cuenta en la liquidación?:";
      this.txt_pregunta_detalle = "¿Desde la supervisión tiene observaciones frente a la actualización de la póliza que deban ser tenidas en cuenta en la liquidación?:";
    }
  }

  getHistoricoObservacionLiquidacionContratacionByMenuIdAndContratacionProyectoId() {
    this.registerContractualLiquidationRequestService.getHistoricoObservacionLiquidacionContratacionByMenuIdAndContratacionProyectoId(
      this.menuId,
      this.contratacionProyectoId,
      this.padreId,
      this.tipoObservacionCodigo
    ).subscribe(response => {
      if(response != null && response.length> 0){
        this.historicoLiquidacionContratacionObservacion = response[0];
        if(this.historicoLiquidacionContratacionObservacion.obsVigente != null){
          this.observacionApoyo = this.historicoLiquidacionContratacionObservacion.obsVigente;
        }
      }
    });
  }
}