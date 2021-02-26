import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-dialog-avance-resumen-alertas',
  templateUrl: './dialog-avance-resumen-alertas.component.html',
  styleUrls: ['./dialog-avance-resumen-alertas.component.scss']
})
export class DialogAvanceResumenAlertasComponent implements OnInit {

    constructor( @Inject( MAT_DIALOG_DATA ) public observaciones ) { }

    ngOnInit(): void {
    }

    innerObservacion( observacion: string ) {
        if ( observacion !== undefined ) {
            const observacionHtml = observacion.replace( '"', '' );
            return observacionHtml;
        }
    }

}
