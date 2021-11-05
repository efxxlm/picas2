import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';

@Component({
  selector: 'app-tabla-valtotal-og-rlc',
  templateUrl: './tabla-valtotal-og-rlc.component.html',
  styleUrls: ['./tabla-valtotal-og-rlc.component.scss']
})
export class TablaValtotalOgRlcComponent implements OnInit {

  @Input() tablaOrdenGiroValorTotal: any[] = [];

  dataSource = new MatTableDataSource();
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = [
    'numOrdenGiro',
    'contratista',
    'facturado',
    'ansAplicado',
    'reteGarantiaAPagar',
    'otrosDescuentos',
    'aPagarAntesImpuestos',
    //'gestion'
  ];

  constructor(
    private routes: Router
  ) { }

  ngOnInit(): void {
    if(this.tablaOrdenGiroValorTotal.length > 0){
      this.dataSource.data = this.tablaOrdenGiroValorTotal;
    }
  }
  loadDataSource() {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    this.paginator._intl.getRangeLabel = (page, pageSize, length) => {
      if (length === 0 || pageSize === 0) {
        return '0 de ' + length;
      }
      length = Math.max(length, 0);
      const startIndex = page * pageSize;
      // If the start index exceeds the list length, do not try and fix the end index to the end.
      const endIndex = startIndex < length ?
        Math.min(startIndex + pageSize, length) :
        startIndex + pageSize;
      return startIndex + 1 + ' - ' + endIndex + ' de ' + length;
    };
  }
  verDetalle(id){
    this.routes.navigate(['/registrarLiquidacionContrato/detalleOrdengiro',id]);
  }
}
