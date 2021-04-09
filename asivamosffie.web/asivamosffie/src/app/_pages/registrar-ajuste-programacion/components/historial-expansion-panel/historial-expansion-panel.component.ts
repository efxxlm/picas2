import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-historial-expansion-panel',
  templateUrl: './historial-expansion-panel.component.html',
  styleUrls: ['./historial-expansion-panel.component.scss']
})
export class HistorialExpansionPanelComponent implements OnInit {
  @Input() ajusteProgramacionInfo:any;
  constructor() { }

  ngOnInit(): void {
  }

}
