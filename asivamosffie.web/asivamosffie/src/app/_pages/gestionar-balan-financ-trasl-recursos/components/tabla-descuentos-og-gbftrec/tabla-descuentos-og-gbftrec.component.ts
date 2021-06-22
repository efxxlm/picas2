import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-descuentos-og-gbftrec',
  templateUrl: './tabla-descuentos-og-gbftrec.component.html',
  styleUrls: ['./tabla-descuentos-og-gbftrec.component.scss']
})
export class TablaDescuentosOgGbftrecComponent implements OnInit {
  dataSource = new MatTableDataSource();
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @Input() tablaDescuento: any;
  displayedColumns: string[] = [
    'aportante',
    'conceptoPago',
    'ansAplicado',
    'retegarantiaPagar',
    'otrosDescuentos',
    'valorTotalDescuentos'
  ];
  dataTable: any[];
  constructor() {}

  ngOnInit(): void {
    this.loadDataSource();
  }
  loadDataSource() {
    this.dataTable = this.tablaDescuento;
    this.dataSource = new MatTableDataSource(this.dataTable);
    this.dataSource.sort = this.sort;
  }
}
