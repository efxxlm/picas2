import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';

@Component({
  selector: 'app-tabla-sin-registro-liquidacion-rlc',
  templateUrl: './tabla-sin-registro-liquidacion-rlc.component.html',
  styleUrls: ['./tabla-sin-registro-liquidacion-rlc.component.scss']
})
export class TablaSinRegistroLiquidacionRlcComponent implements OnInit {
  displayedColumns: string[] = ['fechaSolicitud', 'numeroSolicitud', 'numeroContrato', 'tipoContrato', 'estadoRegistro', 'estadoDocumento', 'gestion'];
  dataSource = new MatTableDataSource();

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  dataTable: any[] = [
    {
      fechaSolicitud: '12/01/2021',
      numeroSolicitud: 'Liq _0009',
      numeroContrato: 'N801801',
      tipoContrato: 'Obra',
      estadoRegistro: 'Incompleto',
      estadoDocumento: 'Sin registro',
      id: 1
    }
  ];
  constructor(private router: Router) { }

  ngOnInit(): void {
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
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };
  gestionSolicitud(id){
    this.router.navigate(['/registrarLiquidacionContrato/gestionarSolicitud', id]);
  };
}
