import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { DevolverPorValidacionComponent } from '../devolver-por-validacion/devolver-por-validacion.component';
import { CancelarDdpComponent } from '../cancelar-ddp/cancelar-ddp.component';

@Component({
  selector: 'app-detalle-con-disponibilidad-cancelada',
  templateUrl: './detalle-con-disponibilidad-cancelada.component.html',
  styleUrls: ['./detalle-con-disponibilidad-cancelada.component.scss']
})
export class DetalleConDisponibilidadCanceladaComponent implements OnInit {

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
