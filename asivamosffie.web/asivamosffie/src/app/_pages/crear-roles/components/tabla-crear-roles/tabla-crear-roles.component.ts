import { CrearRolesService } from './../../../../core/_services/crearRoles/crear-roles.service';
import { Component, OnInit, Output, ViewChild, EventEmitter } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import moment from 'moment';
@Component({
  selector: 'app-tabla-crear-roles',
  templateUrl: './tabla-crear-roles.component.html',
  styleUrls: ['./tabla-crear-roles.component.scss']
})
export class TablaCrearRolesComponent implements OnInit {

    dataSource = new MatTableDataSource();
    @Output() sinRegistros = new EventEmitter<boolean>();
    @ViewChild( MatPaginator, { static: true } ) paginator: MatPaginator;
    @ViewChild( MatSort, { static: true } ) sort: MatSort;
    displayedColumns: string[] = [ 'fechaCreacion', 'nombreRol', 'estadoRol', 'gestion' ];

    constructor( private crearRolesSvc: CrearRolesService )
    {
        this.crearRolesSvc.getListPerfil()
            .subscribe(
                listas => {
                    if ( listas.length > 0 ) {

                        listas.forEach( registro => registro.fechaCreacion = registro.fechaCreacion !== undefined ? moment( registro.fechaCreacion ).format( 'DD/MM/YYYY' ) : '' );
                    } else {
                        this.sinRegistros.emit( true );
                    }
                    console.log( listas );
                    this.dataSource = new MatTableDataSource( listas );
                    this.dataSource.paginator = this.paginator;
                    this.dataSource.sort = this.sort;
                    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
                    setTimeout(() => {
                        document.getElementsByName( 'desactivarBtn' ).forEach( ( value: HTMLElement ) => value.classList.add( 'd-none' ) );
                    }, 500);
                }
            );
    }

    ngOnInit(): void {
    }

    applyFilter(event: Event) {
        const filterValue = (event.target as HTMLInputElement).value;
        this.dataSource.filter = filterValue.trim().toLowerCase();
    }

}
