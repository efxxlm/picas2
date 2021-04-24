import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-detalle-giro',
  templateUrl: './detalle-giro.component.html',
  styleUrls: ['./detalle-giro.component.scss']
})
export class DetalleGiroComponent implements OnInit {

    @Input() solicitudPago: any;
    @Input() esVerDetalle: boolean;
    @Output() estadoSemaforo = new EventEmitter<string>();
    ordenGiro: any;
    listaSemaforos = {
        semaforoOrigen: 'sin-diligenciar',
        semaforoObservacion: 'sin-diligenciar',
        semaforoSoporteUrl: 'sin-diligenciar'
    };

    constructor() { }

    ngOnInit(): void {
    }

    checkSemaforoOrigen( value: boolean ) {
        if ( value === false ) {
            delete this.listaSemaforos.semaforoOrigen;
        }
    }

}
