import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-gestionar-parametricas',
  templateUrl: './tabla-gestionar-parametricas.component.html',
  styleUrls: ['./tabla-gestionar-parametricas.component.scss']
})
export class TablaGestionarParametricasComponent implements OnInit {

    dataSource = new MatTableDataSource();
    @ViewChild( MatPaginator, { static: true } ) paginator: MatPaginator;
    @ViewChild( MatSort, { static: true } ) sort: MatSort;
    displayedColumns: string[] = [ 'id', 'nombreParametrica', 'descripcion', 'gestion' ];

    constructor( ) { }

    ngOnInit(): void {
        const dataTable = [
            {
                id: 'P1',
                nombreParametrica: 'Tipo de Intervención',
                descripcion: 'Lista los tipos de intervención',
                idParametrica: 1,
            },
            {
                id: 'P2',
                nombreParametrica: 'Región',
                descripcion: 'Lista de las regiones de Colombia',
                idParametrica: 1,
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
