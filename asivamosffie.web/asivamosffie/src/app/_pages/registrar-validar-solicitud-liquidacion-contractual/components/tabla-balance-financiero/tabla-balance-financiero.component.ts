import { Component, ViewChild, OnInit } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

const ELEMENT_DATA = [
  {
    id: '1',
    fechaTerminacion: '10/11/2020',
    llaveMen: '----',
    tipoIntervencion: 'N801801',
    institucionEducativa: '$105.000.000',
    sede: '1',
    numeroTraslados: 'Sin validación',
    estadoValidacion: 'Sin validación'
  }
];

@Component({
  selector: 'app-tabla-balance-financiero',
  templateUrl: './tabla-balance-financiero.component.html',
  styleUrls: ['./tabla-balance-financiero.component.scss']
})
export class TablaBalanceFinancieroComponent implements OnInit {

  ELEMENT_DATA: any[];
  displayedColumns: string[] = [
    'fechaTerminacion',
    'llaveMen',
    'tipoIntervencion',
    'institucionEducativa',
    'sede',
    'numeroTraslados',
    'estadoValidacion',
    'id'
  ];
  dataSource = new MatTableDataSource(ELEMENT_DATA);

  @ViewChild(MatSort) sort: MatSort;
  constructor() {}

  ngOnInit(): void {}
}
