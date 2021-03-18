import { DialogDescargarOrdenGiroComponent } from './../dialog-descargar-orden-giro/dialog-descargar-orden-giro.component';
import { MatDialog } from '@angular/material/dialog';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-tramitar-orden-giro',
  templateUrl: './tramitar-orden-giro.component.html',
  styleUrls: ['./tramitar-orden-giro.component.scss']
})
export class TramitarOrdenGiroComponent implements OnInit {

    verAyuda = false;

    constructor( private dialog: MatDialog ) { }

    ngOnInit(): void {
    }

    openDialogDescargarOrdenGiro() {
        this.dialog.open( DialogDescargarOrdenGiroComponent, {
          width: '80em'
        });
    }

}
