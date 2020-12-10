import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-avance-fisico-financiero',
  templateUrl: './avance-fisico-financiero.component.html',
  styleUrls: ['./avance-fisico-financiero.component.scss']
})
export class AvanceFisicoFinancieroComponent implements OnInit {

  @Input() esVerDetalle = false;
  @Input() seguimientoDiario: any;

  constructor() { }

  ngOnInit(): void {
  }

}
