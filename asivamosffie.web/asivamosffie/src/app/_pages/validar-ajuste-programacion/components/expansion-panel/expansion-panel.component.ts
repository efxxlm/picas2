import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-expansion-panel',
  templateUrl: './expansion-panel.component.html',
  styleUrls: ['./expansion-panel.component.scss']
})
export class ExpansionPanelComponent implements OnInit {

  @Input() ajusteProgramacion: any;
  @Input() esVerDetalle: boolean;
  estadoSemaforoProgramacion: string;
  estadoSemaforoFlujo: string;

  constructor() { }

  ngOnInit(): void {
  }
}
