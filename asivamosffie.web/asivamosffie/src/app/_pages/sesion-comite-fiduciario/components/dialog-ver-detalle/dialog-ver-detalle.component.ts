import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-dialog-ver-detalle',
  templateUrl: './dialog-ver-detalle.component.html',
  styleUrls: ['./dialog-ver-detalle.component.scss']
})
export class DialogVerDetalleComponent implements OnInit {

  constructor ( @Inject(MAT_DIALOG_DATA) public compromisoSeguimiento ) { }

  ngOnInit(): void {
    console.log( this.compromisoSeguimiento );
  };

};