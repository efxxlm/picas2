import { Component, Input, OnInit } from '@angular/core';
import { ReprogrammingService } from 'src/app/core/_services/reprogramming/reprogramming.service';

@Component({
  selector: 'app-expansion-panel',
  templateUrl: './expansion-panel.component.html',
  styleUrls: ['./expansion-panel.component.scss']
})
export class ExpansionPanelComponent implements OnInit {

  @Input() ajusteProgramacionInfo:any;
  @Input() novedadContractualRegistroPresupuestal:any;
  @Input() plazoContratacion:any;
  @Input() valorContrato:number;
  @Input() esVerDetalle:boolean;

  estadoSemaforoObra = 'sin-diligenciar';
  estadoSemaforoFlujo= 'sin-diligenciar';
  ajusteProgramacion: any;

  constructor(
    private reprogrammingService: ReprogrammingService,
  ) {
    console.log(this.ajusteProgramacionInfo)
   }

  ngOnInit(): void {
    this.reprogrammingService.getAjusteProgramacionById( this.ajusteProgramacionInfo.ajusteProgramacionId )
    .subscribe( ajuste => {
      this.ajusteProgramacion = ajuste;
    });
  }

}
