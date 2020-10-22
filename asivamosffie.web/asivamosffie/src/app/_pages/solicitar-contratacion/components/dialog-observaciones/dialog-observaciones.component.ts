import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-dialog-observaciones',
  templateUrl: './dialog-observaciones.component.html',
  styleUrls: ['./dialog-observaciones.component.scss']
})
export class DialogObservacionesComponent implements OnInit {

  constructor ( @Inject(MAT_DIALOG_DATA) public data ) { }

  ngOnInit(): void {
  }

  innerObservacion ( observacion: string ) {
    const observacionHtml = observacion.replace( '"', '' );
    return observacionHtml;
  };

}
