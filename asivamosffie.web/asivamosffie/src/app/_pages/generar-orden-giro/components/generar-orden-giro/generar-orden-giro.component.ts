import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';

@Component({
  selector: 'app-generar-orden-giro',
  templateUrl: './generar-orden-giro.component.html',
  styleUrls: ['./generar-orden-giro.component.scss']
})
export class GenerarOrdenGiroComponent implements OnInit {
  verAyuda = false;
  dataSource = new MatTableDataSource();
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = [
    'fechaValidacionFinanciera',
    'numeroSolicitud',
    'modalidadContrato',
    'numeroContrato',
    'estadoGeneracion',
    'estadoRegistro',
    'gestion'
  ];
  dataTable: any[] = [
    {
      fechaValidacionFinanciera: '10/10/2020',
      numeroSolicitud: 'Sol Pago O 001',
      modalidadContrato: 'Tipo B',
      numeroContrato: 'N801801',
      estadoGeneracion: 'Sin generación',
      estadoRegistro: 'Incompleto',
      gestion: 1
    },
    {
      fechaValidacionFinanciera: '15/10/2020',
      numeroSolicitud: 'Sol Pago Expensas 001',
      modalidadContrato: 'No aplica',
      numeroContrato: 'N326326',
      estadoGeneracion: 'Sin generación',
      estadoRegistro: 'Incompleto',
      gestion: 2
    },
    {
      fechaValidacionFinanciera: '20/10/2020',
      numeroSolicitud: 'Sol Pago Otros costos 001',
      modalidadContrato: 'Tipo B',
      numeroContrato: 'N801801',
      estadoGeneracion: 'Sin generación',
      estadoRegistro: 'Incompleto',
      gestion: 3
    },
  ];
  constructor(private routes: Router, public dialog: MatDialog) { }

  ngOnInit(): void {
    this.dataSource = new MatTableDataSource(this.dataTable);
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
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
  generarOrden(id){
    this.routes.navigate(['/generarOrdenDeGiro/generacionOrdenGiro',id]);
  }
}
