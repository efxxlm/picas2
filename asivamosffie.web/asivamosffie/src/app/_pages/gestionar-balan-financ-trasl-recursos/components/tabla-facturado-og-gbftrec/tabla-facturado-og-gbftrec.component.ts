import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-facturado-og-gbftrec',
  templateUrl: './tabla-facturado-og-gbftrec.component.html',
  styleUrls: ['./tabla-facturado-og-gbftrec.component.scss']
})
export class TablaFacturadoOgGbftrecComponent implements OnInit {
  dataSource = new MatTableDataSource();
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @Input() tablaFacturado: any;
  displayedColumns: string[] = [
    'aportante',
    'valorFacturado',
    'uso',
    'tipoPago',
    'conceptoPago',
    'valorConceptoPago'
  ];
  dataTable: any[];

  constructor() {}

  ngOnInit(): void {
    this.loadDataSource();
  }
  loadDataSource() {
    this.dataTable = this.tablaFacturado;
    this.dataSource = new MatTableDataSource(this.dataTable);
    this.dataSource.sort = this.sort;
  }
}
