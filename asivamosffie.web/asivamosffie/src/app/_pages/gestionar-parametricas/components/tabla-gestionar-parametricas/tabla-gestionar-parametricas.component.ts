import { GestionarParametricasService } from './../../../../core/_services/gestionarParametricas/gestionar-parametricas.service';
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

    constructor( private gestionarParametricaSvc: GestionarParametricasService )
    {
        this.gestionarParametricaSvc.getParametricas()
            .subscribe( getParametricas => {
                this.dataSource = new MatTableDataSource( getParametricas );
                this.dataSource.paginator = this.paginator;
                this.dataSource.sort = this.sort;
                this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
            } );
    }

    ngOnInit(): void {
    }

    applyFilter(event: Event) {
        const filterValue = (event.target as HTMLInputElement).value;
        this.dataSource.filter = filterValue.trim().toLowerCase();
    }

}
