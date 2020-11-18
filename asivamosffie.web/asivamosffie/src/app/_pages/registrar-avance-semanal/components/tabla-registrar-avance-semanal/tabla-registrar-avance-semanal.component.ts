import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-registrar-avance-semanal',
  templateUrl: './tabla-registrar-avance-semanal.component.html',
  styleUrls: ['./tabla-registrar-avance-semanal.component.scss']
})
export class TablaRegistrarAvanceSemanalComponent implements OnInit {

    tablaRegistro              = new MatTableDataSource();
    @ViewChild( MatPaginator, { static: true } ) paginator: MatPaginator;
    @ViewChild( MatSort, { static: true } ) sort          : MatSort;
    displayedColumns: string[]  = [
      'llaveMen',
      'numeroContrato',
      'tipoIntervencion',
      'institucionEducativa',
      'sede',
      'fechaUltimoReporte',
      'estadoObra',
      'gestion'
    ];
    dataTable: any[] = [
        {
            llaveMen: 'LU990088',
            numeroContrato: 'CC223456789',
            tipoIntervencion: 'Remodelación',
            institucionEducativa: 'María Inmaculada',
            sede: 'Sede 2',
            fechaUltimoReporte: 'Sin registro',
            estadoObra: 'Sin registro de avance semanal',
            id: 1
        }
    ];

    constructor() { };

    ngOnInit(): void {
        this.tablaRegistro = new MatTableDataSource( this.dataTable );
    };

    applyFilter ( event: Event ) {
      const filterValue      = (event.target as HTMLInputElement).value;
      this.tablaRegistro.filter = filterValue.trim().toLowerCase();
    };

};