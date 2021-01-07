import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-gestion-de-obra',
  templateUrl: './gestion-de-obra.component.html',
  styleUrls: ['./gestion-de-obra.component.scss']
})
export class GestionDeObraComponent implements OnInit {

    @Input() seguimientoSemanal: any;
    @Input() esVerDetalle = false;
    @Input() tipoObservaciones: any;

    constructor() { }

    ngOnInit(): void {
    }

}
