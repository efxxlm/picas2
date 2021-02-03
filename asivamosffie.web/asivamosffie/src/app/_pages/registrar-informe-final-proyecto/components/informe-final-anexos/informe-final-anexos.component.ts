import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-informe-final-anexos',
  templateUrl: './informe-final-anexos.component.html',
  styleUrls: ['./informe-final-anexos.component.scss']
})
export class InformeFinalAnexosComponent implements OnInit {

  @Input() id: string;
  @Input() llaveMen: string;

  constructor() { }

  ngOnInit(): void {
  }

}
