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
    if(this.estadoInforme === '2' || this.estadoInforme === '3'){//con informe registrado/ enviado a validación
      this.estadoInformeString = "completo";
    }else if(this.estadoInforme === '1' || this.estadoInforme === '4'){//en proceso de registro, con observaciones
      this.estadoInformeString = "en-proceso";
    }else{
      this.estadoInformeString = "sin-diligenciar";
    }
  }

}
