import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-verificar-avance-fisico',
  templateUrl: './verificar-avance-fisico.component.html',
  styleUrls: ['./verificar-avance-fisico.component.scss']
})
export class VerificarAvanceFisicoComponent implements OnInit {

    @Input() esVerDetalle = false;
    @Input() seguimientoSemanal: any;
    @Input() avanceFisicoObs: string;
    semaforoAvanceFisico = 'sin-diligenciar';

    constructor() { }

    ngOnInit(): void {
    }

}
