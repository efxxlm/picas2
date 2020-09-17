import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AplazarSesionComponent } from '../aplazar-sesion/aplazar-sesion.component';
@Component({
  selector: 'app-registrar-sesion-comite-tecnico',
  templateUrl: './registrar-sesion-comite-tecnico.component.html',
  styleUrls: ['./registrar-sesion-comite-tecnico.component.scss']
})
export class RegistrarSesionComiteTecnicoComponent implements OnInit {

  constructor(
    public dialog: MatDialog
  ) { }

  openDialogAplazarSesion() {
    this.dialog.open(AplazarSesionComponent, {
      width: '42em'
    });
  }

  ngOnInit(): void {
  }

}
