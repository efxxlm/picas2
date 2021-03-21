import { Component, Input, OnInit } from '@angular/core';
import { NovedadContractual } from 'src/app/_interfaces/novedadContractual';

@Component({
  selector: 'app-expansion-panel',
  templateUrl: './expansion-panel.component.html',
  styleUrls: ['./expansion-panel.component.scss']
})
export class ExpansionPanelComponent implements OnInit {

  @Input() proyecto:any;
  @Input() contrato:any;
  @Input() novedad:NovedadContractual;

  constructor() { }

  ngOnInit(): void {
  }

  guardar(){
    
  }

}
