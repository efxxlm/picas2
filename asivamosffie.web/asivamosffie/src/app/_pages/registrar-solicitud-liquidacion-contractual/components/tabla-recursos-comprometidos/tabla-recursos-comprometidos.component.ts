import { Component, ViewChild, OnInit } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

const ELEMENT_DATA = [
  {
    aportante: 'Alcaldía de Susacón',
    valorAportante: '$45.000.000',
    valorTotal: '$105.000.000'
    
  },
  {
    aportante: 'Gobernación de Boyacá',
    valorAportante: '$40.000.000',
    valorTotal: '$105.000.000'
    
  },
  {
    aportante: 'FFIE',
    valorAportante: '$20.000.000',
    valorTotal: '$105.000.000'
    
  }
];

@Component({
  selector: 'app-tabla-recursos-comprometidos',
  templateUrl: './tabla-recursos-comprometidos.component.html',
  styleUrls: ['./tabla-recursos-comprometidos.component.scss']
})
export class TablaRecursosComprometidosComponent implements OnInit {

  ELEMENT_DATA: any[];
  displayedColumns: string[] = [
    'aportante',
    'valorAportante',
    'valorTotal'
  ];
  dataSource = new MatTableDataSource(ELEMENT_DATA);

  @ViewChild(MatSort) sort: MatSort;
  constructor() { }

  ngOnInit(): void {
  }

}