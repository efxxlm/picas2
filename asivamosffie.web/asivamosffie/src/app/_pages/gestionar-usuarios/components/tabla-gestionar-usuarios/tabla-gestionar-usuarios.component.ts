import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-gestionar-usuarios',
  templateUrl: './tabla-gestionar-usuarios.component.html',
  styleUrls: ['./tabla-gestionar-usuarios.component.scss']
})
export class TablaGestionarUsuariosComponent implements OnInit {

    dataSource = new MatTableDataSource();
    @ViewChild( MatPaginator, { static: true } ) paginator: MatPaginator;
    @ViewChild( MatSort, { static: true } ) sort: MatSort;
    displayedColumns: string[] = [ 'fechaCreacion', 'procedencia', 'nombreApellido', 'numeroDocumento', 'rol', 'estado', 'gestion' ];

    constructor( ) { }

    ngOnInit(): void {
        const dataTable = [
            {
                fechaCreacion: '26/02/2021',
                procedencia: 'FFIE/Fiduciaria',
                nombreApellido: 'Nicolas Fernando Sandoval',
                numeroDocumento: '103445663',
                rol: 'Supervisor',
                estado: 'Activo',
                id: 1
            }
        ];
        this.dataSource = new MatTableDataSource( dataTable );
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
        this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
        setTimeout(() => {
            document.getElementsByName( 'desactivarBtn' ).forEach( ( value: HTMLElement ) => value.classList.add( 'd-none' ) );
        }, 500);
    }

    applyFilter(event: Event) {
        const filterValue = (event.target as HTMLInputElement).value;
        this.dataSource.filter = filterValue.trim().toLowerCase();
    }

}
