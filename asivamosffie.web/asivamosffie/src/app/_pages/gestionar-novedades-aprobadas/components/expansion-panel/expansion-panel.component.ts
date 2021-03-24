import { Component, Input, OnInit } from '@angular/core';
import { NovedadContractual } from 'src/app/_interfaces/novedadContractual';

@Component({
  selector: 'app-expansion-panel',
  templateUrl: './expansion-panel.component.html',
  styleUrls: ['./expansion-panel.component.scss']
})
export class ExpansionPanelComponent implements OnInit {

  @Input() novedad: NovedadContractual
  @Input() tieneAdicion: boolean

  constructor() { }

  ngOnInit(): void {
  }

}
