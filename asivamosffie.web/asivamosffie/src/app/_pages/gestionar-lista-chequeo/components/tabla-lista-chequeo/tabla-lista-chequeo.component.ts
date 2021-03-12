import { GestionarListaChequeoService } from './../../../../core/_services/gestionarListaChequeo/gestionar-lista-chequeo.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import moment from 'moment';

@Component({
  selector: 'app-tabla-lista-chequeo',
  templateUrl: './tabla-lista-chequeo.component.html',
  styleUrls: ['./tabla-lista-chequeo.component.scss']
})
export class TablaListaChequeoComponent implements OnInit {

    dataSource = new MatTableDataSource();
    @ViewChild( MatPaginator, { static: true } ) paginator: MatPaginator;
    @ViewChild( MatSort, { static: true } ) sort: MatSort;
    displayedColumns: string[] = [ 'fechaCreacion', 'nombreRequisito', 'estadoRequisito', 'gestion' ];

    constructor(
        private listaChequeoSvc: GestionarListaChequeoService )
    {
        this.listaChequeoSvc.getCheckList()
            .subscribe(
                listas => {
                    if ( listas.length > 0 ) {
                        listas.forEach( registro => registro.fechaCreacion !== undefined ? registro.fechaCreacion = moment( registro.fechaCreacion ).format( 'DD/MM/YYYY' ) : '' );
                    }
                    console.log( listas );
                    this.dataSource = new MatTableDataSource( listas );
                    this.dataSource.paginator = this.paginator;
                    this.dataSource.sort = this.sort;
                    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
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
