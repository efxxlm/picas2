import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';

@Component({
  selector: 'app-registrar-validar-requisitos-pago',
  templateUrl: './registrar-validar-requisitos-pago.component.html',
  styleUrls: ['./registrar-validar-requisitos-pago.component.scss']
})
export class RegistrarValidarRequisitosPagoComponent implements OnInit {
  verAyuda = false;
  
  dataSource = new MatTableDataSource();
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = [
    'fechaSolicitud',
    'numeroSolicitud',
    'modalidadContrato',
    'numeroContrato',
    'estadoRegistroPago',
    'gestion'
  ];
  dataTable: any[] = [
    {
      fechaSolicitud: '05/10/2020',
      numeroSolicitud: 'SolPagoO0001',
      modalidadContrato: 'Tipo B',
      numeroContrato: 'N801801',
      estadoRegistroPago: 'En proceso de registro',
      gestion: 1,
    }
  ];

  constructor(private router: Router) { }

  ngOnInit(): void {
    this.dataSource = new MatTableDataSource(this.dataTable);
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
  };

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };
  registrarNuevaSolicitud(){
    this.router.navigate(['registrarValidarRequisitosPago/registrarNuevaSolicitudPago'])
  }
}
