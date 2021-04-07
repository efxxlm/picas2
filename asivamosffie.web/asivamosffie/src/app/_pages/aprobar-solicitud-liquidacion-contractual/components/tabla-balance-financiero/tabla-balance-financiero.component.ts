import { Component, ViewChild, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

const ELEMENT_DATA = [
  {
    id: '1',
    fechaterminacionProyecto: '09/08/2021',
    llaveMEN: 'LL457326',
    tipoIntervencion: 'Remodelación',
    institucionEducativa: 'I.E Nuestra Señora Del Carmen',
    sede: 'única sede',
    numeroTraslados: '1',
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
    'fechaterminacionProyecto',
    'llaveMEN',
    'tipoIntervencion',
    'institucionEducativa',
    'sede',
    'numeroTraslados',
    'estadoValidacion',
    'id'
  ];
  @Input() contratacionProyectoId: number;
  @Output() semaforoBalanceFinanciero = new EventEmitter<string>();
  
  dataSource = new MatTableDataSource(ELEMENT_DATA);

  @ViewChild(MatSort) sort: MatSort;
  constructor() { }

  ngOnInit(): void {
  }

}