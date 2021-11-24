import { Component, Input, OnInit } from '@angular/core';

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
  estadoSemaforoObra = 'sin-diligenciar';
  estadoSemaforoFlujo= 'sin-diligenciar';

  constructor() {
    console.log(this.ajusteProgramacionInfo)
   }

  ngOnInit(): void {
  }

}
