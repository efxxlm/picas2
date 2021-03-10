import { Component, ViewChild, OnInit } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

const ELEMENT_DATA = [
  {
    aportante: 'Alcaldía de Susacón',
    valorAportante: '$45.000.000',
    fuente: 'Recursos propios',
    uso: 'Diseño',
    valorUso: '$12.000.000'
  },
  {
    aportante: 'Gobernación de Boyacá',
    valorAportante: '$45.000.000',
    fuente: 'Contingencias',
    uso: 'Diseño',
    valorUso: '$12.000.000'
  },
  {
    aportante: 'Gobernación de Boyacá',
    valorAportante: '$45.000.000',
    fuente: 'Contingencias',
    uso: 'Obra principal',
    valorUso: '$21.000.000'
  },
  {
    aportante: 'Gobernación de Boyacá',
    valorAportante: '$40.000.000',
    fuente: 'Recursos propios',
    uso: 'Obra principal',
    valorUso: '$40.000.000'
  },
  {
    aportante: 'FFIE',
    valorAportante: '$20.000.000',
    fuente: 'Contingencias',
    uso: 'Obra principal',
    valorUso: '$20.000.000'
  }
];

@Component({
  selector: 'app-tabla-fuentes-y-recursos',
  templateUrl: './tabla-fuentes-y-recursos.component.html',
  styleUrls: ['./tabla-fuentes-y-recursos.component.scss']
})
export class TablaFuentesYRecursosComponent implements OnInit {

  ELEMENT_DATA: any[];
  displayedColumns: string[] = [
    'aportante',
    'valorAportante',
    'fuente',
    'uso',
    'valorUso'
  ];
  dataSource = new MatTableDataSource(ELEMENT_DATA);

  @ViewChild(MatSort) sort: MatSort;
  constructor() { }

  ngOnInit(): void {
  }

}