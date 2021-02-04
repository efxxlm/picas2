import { Component, OnInit, ViewChild } from '@angular/core';
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
  displayedColumns: string[] = [
    'aportante',
    'valorFacturado',
    'uso',
    'tipoPago',
    'conceptoPago',
    'valorConceptoPago'
  ];
  dataTable: any[] = [
    {
      aportante: 'Alcaldía de Susacón',
      valorFacturado: '$15.000.000',
      uso: [{ nombreUso: 'Obra principal' },{ nombreUso: 'Diseño'}],
      tipoPago : [{tipoPagoName:'Costo variable'},{tipoPagoName:'Tipo de pago 3'}],
      conceptoPago: [{conceptoPagoName:'Demolición'},{conceptoPagoName:'Cimentación'}],
      valorConceptoPago: [{valorConcpetoPagoName:'$8.000.000'},{valorConcpetoPagoName:'$7.000.000'}]
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
