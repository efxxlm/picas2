import { Component, Input, OnInit } from '@angular/core';
import { NovedadContractual } from 'src/app/_interfaces/novedadContractual';

@Component({
  selector: 'app-expansion-panel-interventoria',
  templateUrl: './expansion-panel-interventoria.component.html',
  styleUrls: ['./expansion-panel-interventoria.component.scss']
})
export class ExpansionPanelInterventoriaComponent implements OnInit {
  @Input() proyecto:any;
  @Input() contrato:any;
  @Input() novedad:NovedadContractual;

  constructor() { }

  ngOnInit(): void {
  }

}
