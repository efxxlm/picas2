import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-bancos',
  templateUrl: './tabla-bancos.component.html',
  styleUrls: ['./tabla-bancos.component.scss']
})
export class TablaBancosComponent implements OnInit {

    dataSource = new MatTableDataSource();
    @ViewChild( MatPaginator, { static: true } ) paginator: MatPaginator;
    @ViewChild( MatSort, { static: true } ) sort: MatSort;
    displayedColumns: string[] = [ 'nombreRequisito', 'estadoRequisito', 'gestion' ];

    constructor() { }

    ngOnInit(): void {
        const dataTable = [
            {
                nombreRequisito: 'NIT actualizado del contratista a cargo de la obra',
                estadoRequisito: 'Activo',
                id: 1,
            }
        ];

        this.dataSource = new MatTableDataSource( dataTable );
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
        this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    }

    applyFilter(event: Event) {
        const filterValue = (event.target as HTMLInputElement).value;
        this.dataSource.filter = filterValue.trim().toLowerCase();
    }

}
