import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-crear-roles',
  templateUrl: './tabla-crear-roles.component.html',
  styleUrls: ['./tabla-crear-roles.component.scss']
})
export class TablaCrearRolesComponent implements OnInit {

    dataSource = new MatTableDataSource();
    @ViewChild( MatPaginator, { static: true } ) paginator: MatPaginator;
    @ViewChild( MatSort, { static: true } ) sort: MatSort;
    displayedColumns: string[] = [ 'fechaCreacion', 'nombreRol', 'estadoRol', 'gestion' ];

    constructor() { }

    ngOnInit(): void {
        const dataTable = [
            {
                fechaCreacion: '24/02/2021',
                nombreRol: 'Interventor',
                estadoRol: 'Activo',
                id: 1
            }
        ]
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
