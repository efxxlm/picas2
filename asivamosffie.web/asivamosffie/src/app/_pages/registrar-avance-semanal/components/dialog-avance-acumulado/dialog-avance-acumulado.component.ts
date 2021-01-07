import { MatTableDataSource } from '@angular/material/table';
import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-dialog-avance-acumulado',
  templateUrl: './dialog-avance-acumulado.component.html',
  styleUrls: ['./dialog-avance-acumulado.component.scss']
})
export class DialogAvanceAcumuladoComponent implements OnInit {

    @ViewChild( MatPaginator, { static: true } ) paginator: MatPaginator;
    @ViewChild( MatSort, { static: true } ) sort: MatSort;
    tablaAvanceAcumulado = new MatTableDataSource();
    displayedColumns: string[]  = [
      'nombreCapitulo',
      'programacion',
      'avanceAcumulado'
    ];

    constructor( @Inject(MAT_DIALOG_DATA) public data ) {
    }

    ngOnInit(): void {
        this.tablaAvanceAcumulado = new MatTableDataSource( this.data.avanceAcumulado );
        this.tablaAvanceAcumulado.paginator = this.paginator;
        this.tablaAvanceAcumulado.sort = this.sort;
        this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    }

}
