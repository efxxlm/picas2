import { MatTableDataSource } from '@angular/material/table';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';

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
    dataTable: any[] = [
        {
            nombreCapitulo: 'Preliminares',
            programacion: '8%',
            avanceAcumulado: '0%'
        }
    ];

    constructor() { }

    ngOnInit(): void {
        this.tablaAvanceAcumulado = new MatTableDataSource( this.dataTable );
        this.tablaAvanceAcumulado.paginator = this.paginator;
        this.tablaAvanceAcumulado.sort = this.sort;
        this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    }

}
