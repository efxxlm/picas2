import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { OrdenPagoService } from 'src/app/core/_services/ordenPago/orden-pago.service';

@Component({
  selector: 'app-tabla-valor-total-og-gbftrec',
  templateUrl: './tabla-valor-total-og-gbftrec.component.html',
  styleUrls: ['./tabla-valor-total-og-gbftrec.component.scss']
})
export class TablaValorTotalOgGbftrecComponent implements OnInit {

  @Input() tablaOrdenGiroValorTotal: any[] = [];
  dataSource = new MatTableDataSource();
  dataTable: any[] = [];


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


  constructor(
    private routes: Router,
    private ordenGiroSvc: OrdenPagoService
  ) { }

  ngOnInit(): void {
    if(this.tablaOrdenGiroValorTotal.length > 0){
      this.dataSource.data = this.tablaOrdenGiroValorTotal;
    }
  }

  loadDataSource() {
    //this.dataSource = new MatTableDataSource(this.dataTable);
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
  verDetalle(solicitudPagoId :number){
    this.routes.navigate(['/gestionarBalanceFinancieroTrasladoRecursos/detalleOrdengiro', solicitudPagoId]);
  }
}
