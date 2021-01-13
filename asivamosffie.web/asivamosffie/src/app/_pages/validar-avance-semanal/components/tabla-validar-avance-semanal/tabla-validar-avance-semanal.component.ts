import { ValidarAvanceSemanalService } from './../../../../core/_services/validarAvanceSemanal/validar-avance-semanal.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-validar-avance-semanal',
  templateUrl: './tabla-validar-avance-semanal.component.html',
  styleUrls: ['./tabla-validar-avance-semanal.component.scss']
})
export class TablaValidarAvanceSemanalComponent implements OnInit {

    tablaRegistro = new MatTableDataSource();
    @ViewChild( MatPaginator, { static: true } ) paginator: MatPaginator;
    @ViewChild( MatSort, { static: true } ) sort: MatSort;
    displayedColumns: string[]  = [
        'fechaReporte',
        'llaveMen',
        'numeroContrato',
        'tipoIntervencion',
        'institucionEducativa',
        'sede',
        'estadoObra',
        'estadoValidacion',
        'gestion'
    ];

    constructor( private validarAvanceSemanalSvc: ValidarAvanceSemanalService ) { }

    ngOnInit(): void {
        this.validarAvanceSemanalSvc.getListReporteSemanalView()
            .subscribe(
                response => {
                    console.log( response );
                    this.tablaRegistro = new MatTableDataSource( response );
                    this.tablaRegistro.sort = this.sort;
                    this.tablaRegistro.paginator = this.paginator;
                    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
                }
            );
    }

    applyFilter( event: Event ) {
        const filterValue      = (event.target as HTMLInputElement).value;
        this.tablaRegistro.filter = filterValue.trim().toLowerCase();
    }

}
