import { Component, Input } from '@angular/core';
import { RegisterContractualLiquidationRequestService } from 'src/app/core/_services/registerContractualLiquidationRequest/register-contractual-liquidation-request.service';

@Component({
  selector: 'app-observaciones-supervisor',
  templateUrl: './observaciones-supervisor.component.html',
  styleUrls: ['./observaciones-supervisor.component.scss']
})
export class ObservacionesSupervisorComponent {

  @Input() contratacionProyectoId: any;
  @Input() tipoObservacionCodigo: string;
  @Input() menuId: any;
  @Input() informeFinalId: number;
  observacionApoyo: any;

  constructor(
    private registerContractualLiquidationRequestService: RegisterContractualLiquidationRequestService
  ) {}

  ngOnInit(): void {
    this.getHistoricoObservacionLiquidacionContratacionByMenuIdAndContratacionProyectoId();
  }

  getHistoricoObservacionLiquidacionContratacionByMenuIdAndContratacionProyectoId() {
    this.registerContractualLiquidationRequestService.getHistoricoObservacionLiquidacionContratacionByMenuIdAndContratacionProyectoId(
      this.menuId,
      this.contratacionProyectoId,
      this.informeFinalId,
      this.tipoObservacionCodigo
    ).subscribe(response => {
      if(response != null){
        this.observacionApoyo = response[0];
      }
    });
  }
}
