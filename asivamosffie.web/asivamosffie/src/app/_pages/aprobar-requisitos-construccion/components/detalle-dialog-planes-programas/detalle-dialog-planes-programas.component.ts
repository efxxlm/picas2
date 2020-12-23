import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-detalle-dialog-planes-programas',
  templateUrl: './detalle-dialog-planes-programas.component.html',
  styleUrls: ['./detalle-dialog-planes-programas.component.scss']
})
export class DetalleDialogPlanesProgramasComponent implements OnInit {

  constructor( @Inject(MAT_DIALOG_DATA) public data ) { }

  ngOnInit(): void {
  }

}
