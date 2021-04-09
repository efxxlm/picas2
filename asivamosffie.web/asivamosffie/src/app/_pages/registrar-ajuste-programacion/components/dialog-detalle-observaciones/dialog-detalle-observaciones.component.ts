import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-dialog-detalle-observaciones',
  templateUrl: './dialog-detalle-observaciones.component.html',
  styleUrls: ['./dialog-detalle-observaciones.component.scss']
})
export class DialogDetalleObservacionesComponent implements OnInit {

  constructor(private dialog: MatDialog) { }

  ngOnInit(): void {
  }
  onClose(): void {
    this.dialog.closeAll();
  }
}
