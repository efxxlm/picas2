import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-informe-final-anexos',
  templateUrl: './informe-final-anexos.component.html',
  styleUrls: ['./informe-final-anexos.component.scss']
})
export class InformeFinalAnexosComponent implements OnInit {
  estadoInformeString :string;
  @Input() id: string;
  @Input() llaveMen: string;
  @Input() estadoInforme: string;

  constructor() { }

  ngOnInit(): void {
    if(this.estadoInforme == '2'){
      this.estadoInformeString = "completo";
    }else if(this.estadoInforme=='1'){
      this.estadoInformeString = "en-proceso";
    }else{
      this.estadoInformeString = "sin-diligenciar";
    }
  }

}
