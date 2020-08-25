import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { DevolverPorValidacionComponent } from '../devolver-por-validacion/devolver-por-validacion.component';

@Component({
  selector: 'app-validacion-presupuestal',
  templateUrl: './validacion-presupuestal.component.html',
  styleUrls: ['./validacion-presupuestal.component.scss']
})
export class ValidacionPresupuestalComponent implements OnInit {

  constructor(public dialog: MatDialog) { }

  ngOnInit(): void {
  }

  openDialogDevolver() {
    this.dialog.open(DevolverPorValidacionComponent, {
      width: '70em'
    });
  }

}
