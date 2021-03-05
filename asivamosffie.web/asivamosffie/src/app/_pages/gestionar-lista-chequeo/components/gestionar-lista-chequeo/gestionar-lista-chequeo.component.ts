import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-gestionar-lista-chequeo',
  templateUrl: './gestionar-lista-chequeo.component.html',
  styleUrls: ['./gestionar-lista-chequeo.component.scss']
})
export class GestionarListaChequeoComponent implements OnInit {

    verAyuda = false;
    bancoRequisitos = false;
    listaChequeo = false;

    constructor() { }

    ngOnInit(): void {
    }

}
