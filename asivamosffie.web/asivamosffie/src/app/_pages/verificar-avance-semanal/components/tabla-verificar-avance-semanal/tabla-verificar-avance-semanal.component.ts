import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-verificar-avance-semanal',
  templateUrl: './tabla-verificar-avance-semanal.component.html',
  styleUrls: ['./tabla-verificar-avance-semanal.component.scss']
})
export class TablaVerificarAvanceSemanalComponent implements OnInit {

    tablaRegistro              = new MatTableDataSource();
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
        'estadoVerificacion',
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
            estadoVerificacion: 'Avance semanal verificado',
            contratacionProyectoId: 1
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
