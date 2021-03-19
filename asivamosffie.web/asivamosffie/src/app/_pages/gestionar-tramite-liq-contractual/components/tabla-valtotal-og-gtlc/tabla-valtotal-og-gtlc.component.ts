import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';

@Component({
  selector: 'app-tabla-valtotal-og-gtlc',
  templateUrl: './tabla-valtotal-og-gtlc.component.html',
  styleUrls: ['./tabla-valtotal-og-gtlc.component.scss']
})
export class TablaValtotalOgGtlcComponent implements OnInit {
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
    'gestion'
  ];
  dataTable: any[] = [
    {
      numOrdenGiro: 'ODG_Obra_001',
      contratista: 'Construir futuro',
      facturado: '$67.000.000',
      ansAplicado: '$500.000',
      reteGarantiaAPagar: '$3.500.000',
      otrosDescuentos: '$1.100.000',
      aPagarAntesImpuestos: '$61.890.000',
      id: 1
    },
    {
      numOrdenGiro: 'ODG_Obra_326',
      contratista: 'Contratista 2',
      facturado: '$30.000.000',
      ansAplicado: '$100.000',
      reteGarantiaAPagar: '$500.000',
      otrosDescuentos: '$810.000',
      aPagarAntesImpuestos: '$26.590.000',
      id: 2
    }
  ];
  constructor(
    private routes: Router
  ) { }

  ngOnInit(): void {
    this.loadDataSource();
  }
  loadDataSource() {
    this.dataSource = new MatTableDataSource(this.dataTable);
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
    this.routes.navigate(['/gestionarTramiteLiquidacionContractual/detalleOrdengiro',id]);
  }
}
