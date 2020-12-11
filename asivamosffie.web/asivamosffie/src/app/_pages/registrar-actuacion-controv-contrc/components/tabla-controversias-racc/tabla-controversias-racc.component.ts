import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-controversias-racc',
  templateUrl: './tabla-controversias-racc.component.html',
  styleUrls: ['./tabla-controversias-racc.component.scss']
})
export class TablaControversiasRaccComponent implements OnInit {

  dataSource = new MatTableDataSource();
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = [
    'numeroSolicitud',
    'numeroContrato',
    'tipoControversia',
    'actuacion',
    'fechaActuacion',
    'estadoActuacion',
    'gestion'
  ];
  dataTable: any[] = [
    {
      numeroSolicitud: 'CO001',
      numeroContrato: 'C223456789',
      tipoControversia: 'Terminación anticipada por incumplimiento (TAI)',
      actuacion: 'Actuación 1',
      fechaActuacion: '17/08/2020',
      estadoActuacion: 'Sin registro',
      gestion: 1,
    },
    {
      numeroSolicitud: 'CO002',
      numeroContrato: 'C223456789',
      tipoControversia: 'Terminación anticipada por imposibilidad de ejecucion (TAIE)',
      actuacion: 'Actuación 2',
      fechaActuacion: '17/08/2020',
      estadoActuacion: 'Sin registro',
      gestion: 2,
    }
  ]
  constructor() { }

  ngOnInit(): void {
    this.dataSource = new MatTableDataSource(this.dataTable);
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
  };

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };
}
