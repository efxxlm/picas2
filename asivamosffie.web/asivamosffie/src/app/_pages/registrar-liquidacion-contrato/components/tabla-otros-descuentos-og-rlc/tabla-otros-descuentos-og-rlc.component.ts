import { Component, OnInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-otros-descuentos-og-rlc',
  templateUrl: './tabla-otros-descuentos-og-rlc.component.html',
  styleUrls: ['./tabla-otros-descuentos-og-rlc.component.scss']
})
export class TablaOtrosDescuentosOgRlcComponent implements OnInit {
  dataSource = new MatTableDataSource();
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = [
    'concepto',
    'descuento',
    'valorAportante'
  ];
  //displayedColumnsFooter: string[] = [ 'total' ];
  ELEMENT_DATA: any[] = [
    { titulo: 'Alcaldía de Susacón', name: 'valorAportante' }
  ];
  dataTable: any[] = [
    {
      concepto: 'Demolición',
      descuento: '4x1.000',
      valorAportante: '60.000'
    },
  ];
  constructor() { }


  ngOnInit(): void {
    this.loadDataSource();
  }
  loadDataSource() {
    this.dataSource = new MatTableDataSource(this.dataTable);
    this.dataSource.sort = this.sort;
  }
}
