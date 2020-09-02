import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { DevolverPorValidacionComponent } from '../devolver-por-validacion/devolver-por-validacion.component';
import { CancelarDdpComponent } from '../cancelar-ddp/cancelar-ddp.component';

@Component({
  selector: 'app-gestionar-ddp',
  templateUrl: './gestionar-ddp.component.html',
  styleUrls: ['./gestionar-ddp.component.scss']
})
export class GestionarDdpComponent implements OnInit {

  constructor(public dialog: MatDialog) { }

  ngOnInit(): void {
  }

  openDialogDevolver() {
    this.dialog.open(DevolverPorValidacionComponent, {
      width: '70em'
    });
  }

  openDialogCancelarDDP() {
    this.dialog.open(CancelarDdpComponent, {
      width: '70em'
    });
  }

}
