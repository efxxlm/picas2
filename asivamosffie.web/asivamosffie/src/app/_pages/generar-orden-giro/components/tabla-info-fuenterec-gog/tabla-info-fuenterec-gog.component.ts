import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-info-fuenterec-gog',
  templateUrl: './tabla-info-fuenterec-gog.component.html',
  styleUrls: ['./tabla-info-fuenterec-gog.component.scss']
})
export class TablaInfoFuenterecGogComponent implements OnInit {
  dataSource = new MatTableDataSource();
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = [
    'nombreAportante',
    'fuenteRecursos',
    'saldoActualFRecursos',
    'saldoValorFacturado'
  ];
  dataTable: any[] = [
    {
      nombreAportante: 'Alcaldía de Susacón',
      fuenteRecursos: 'Contingencias',
      saldoActualFRecursos: '$ 75.000.000',
      saldoValorFacturado: '$ 75.000.000'
    },
    {
      nombreAportante: 'Fundación Pies Descalzos',
      fuenteRecursos: 'Recursos propios',
      saldoActualFRecursos: '$ 30.000.000',
      saldoValorFacturado: '$ 15.000.000'
    }
  ];
  constructor() { }

  ngOnInit(): void {
    this.dataSource = new MatTableDataSource(this.dataTable);
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  };

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };

}
