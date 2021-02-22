import { Component, OnInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-descuentos-og-gtlc',
  templateUrl: './tabla-descuentos-og-gtlc.component.html',
  styleUrls: ['./tabla-descuentos-og-gtlc.component.scss']
})
export class TablaDescuentosOgGtlcComponent implements OnInit {
  dataSource = new MatTableDataSource();
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = [
    'aportante',
    'conceptoPago',
    'ansAplicado',
    'retegarantiaPagar',
    'otrosDescuentos',
    'valorTotalDescuentos'
  ];
  dataTable: any[] = [
    {
      aportante: 'Alcaldía de Susacón',
      conceptoPago: 'Demolición',
      ansAplicado:'$113.000',
      retegarantiaPagar: '$500.000',
      otrosDescuentos: '60.000',
      valorTotalDescuentos: '$673.000'
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
