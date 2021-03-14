import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-gestionar-usuarios',
  templateUrl: './gestionar-usuarios.component.html',
  styleUrls: ['./gestionar-usuarios.component.scss']
})
export class GestionarUsuariosComponent implements OnInit {

    verAyuda = false;

    constructor() { }

    ngOnInit(): void {
    }

}
