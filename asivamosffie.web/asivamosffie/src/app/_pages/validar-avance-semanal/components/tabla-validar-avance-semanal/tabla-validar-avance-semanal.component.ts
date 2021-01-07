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
    dataTable: any[] = [
        {
            fechaReporte: new Date(),
            llaveMen: 'LU990088',
            numeroContrato: 'CC223456789',
            tipoIntervencion: 'Remodelación',
            institucionEducativa: 'María Inmaculada',
            sede: 'Sede 2',
            estadoObra: 'Con ejecución normal',
            estadoValidacion: 'Sin validación del supervisor',
            contratacionProyectoId: 131
        }
    ];

    constructor() { }

    ngOnInit(): void {
        this.tablaRegistro = new MatTableDataSource( this.dataTable );
        this.tablaRegistro.sort = this.sort;
        this.tablaRegistro.paginator = this.paginator;
        this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    }

    applyFilter( event: Event ) {
        const filterValue      = (event.target as HTMLInputElement).value;
        this.tablaRegistro.filter = filterValue.trim().toLowerCase();
    }

}
