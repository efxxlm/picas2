import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-crear-roles',
  templateUrl: './crear-roles.component.html',
  styleUrls: ['./crear-roles.component.scss']
})
export class CrearRolesComponent implements OnInit {

    verAyuda = false;
    sinRegistros = false;

    constructor() { }

    ngOnInit(): void {
    }

}
