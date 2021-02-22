import { Component, OnInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-inforecursos-gtlc',
  templateUrl: './tabla-inforecursos-gtlc.component.html',
  styleUrls: ['./tabla-inforecursos-gtlc.component.scss']
})
export class TablaInforecursosGtlcComponent implements OnInit {
  dataSource = new MatTableDataSource();
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = [
    'nomAportante',
    'fuenteRecursos',
    'saldoActualFRecursos',
    'saldoAfectado'
  ];
  dataTable: any[] = [
    {
      nomAportante: 'Alcaldía de Susacón',
      fuenteRecursos: 'Contingencias',
      saldoActualFRecursos: '$ 75.000.000',
      saldoAfectado: '$ 75.000.000'
    },
    {
      nomAportante: 'Gobernación de Boyacá',
      fuenteRecursos: 'Recursos propios',
      saldoActualFRecursos: '$ 30.000.000',
      saldoAfectado: '$ 15.000.000'
    },
  ]
  constructor() { }

  ngOnInit(): void {
    this.loadDataSource();
  }
  loadDataSource() {
    this.dataSource = new MatTableDataSource(this.dataTable);
    this.dataSource.sort = this.sort;
  }
}
