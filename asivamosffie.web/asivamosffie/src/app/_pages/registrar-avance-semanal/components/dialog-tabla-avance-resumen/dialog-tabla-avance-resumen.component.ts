import { Component, Inject, OnInit } from '@angular/core';
import { MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-dialog-tabla-avance-resumen',
  templateUrl: './dialog-tabla-avance-resumen.component.html',
  styleUrls: ['./dialog-tabla-avance-resumen.component.scss']
})
export class DialogTablaAvanceResumenComponent implements OnInit {

    constructor ( @Inject( MAT_DIALOG_DATA ) public observaciones ) { };

    ngOnInit(): void {};

    innerObservacion ( observacion: string ) {
      if ( observacion !== undefined ) {
        const observacionHtml = observacion.replace( '"', '' );
        return observacionHtml;
      };
    };

};