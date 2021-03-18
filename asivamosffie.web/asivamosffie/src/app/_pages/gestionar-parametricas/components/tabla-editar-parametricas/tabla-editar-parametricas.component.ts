import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import moment from 'moment';

@Component({
  selector: 'app-tabla-editar-parametricas',
  templateUrl: './tabla-editar-parametricas.component.html',
  styleUrls: ['./tabla-editar-parametricas.component.scss']
})
export class TablaEditarParametricasComponent implements OnInit {

    @Input() parametricas: any[];
    dataSource = new MatTableDataSource();
    @ViewChild( MatPaginator, { static: true } ) paginator: MatPaginator;
    @ViewChild( MatSort, { static: true } ) sort: MatSort;
    displayedColumns: string[] = [ 'fechaCreacion', 'idValor', 'nombreValor', 'estadoValor', 'gestion' ];

    constructor() { }

    ngOnInit(): void {
        console.log( this.parametricas );

        if ( this.parametricas.length > 0 ) {
            this.parametricas.forEach( registro => registro.dateCreation = registro.dateCreation !== undefined ? moment( registro.dateCreation ).format( 'DD/MM/YYYY' ) : '' );
        }

        this.dataSource = new MatTableDataSource( this.parametricas );
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
        this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    }

    applyFilter(event: Event) {
        const filterValue = (event.target as HTMLInputElement).value;
        this.dataSource.filter = filterValue.trim().toLowerCase();
    }

}
