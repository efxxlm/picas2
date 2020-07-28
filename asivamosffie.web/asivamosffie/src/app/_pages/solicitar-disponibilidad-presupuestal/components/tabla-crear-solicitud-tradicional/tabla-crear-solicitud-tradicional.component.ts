import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

export interface Solicitudes {
  id: number;
  fecha: string;
  tipo: string;
  numero: string;
  opcionPorContratar: string;
  valorSolicitado: number;
  estado: string;
}

const ELEMENT_DATA: Solicitudes[] = [
  {
    id: 1,
    fecha: '22/06/2020',
    tipo: 'Contratación',
    numero: 'PI_002',
    opcionPorContratar: 'Obra',
    valorSolicitado: 400000000,
    estado: 'Sin registrar'
  },
  {
    id: 2,
    fecha: '23/06/2020',
    tipo: 'Modificación contractual ',
    numero: 'CO_001',
    opcionPorContratar: 'Obra',
    valorSolicitado: 60000000,
    estado: 'Sin registrar'
  }
];

@Component({
  selector: 'app-tabla-crear-solicitud-tradicional',
  templateUrl: './tabla-crear-solicitud-tradicional.component.html',
  styleUrls: ['./tabla-crear-solicitud-tradicional.component.scss']
})
export class TablaCrearSolicitudTradicionalComponent implements OnInit {

  verAyuda = false;

  displayedColumns: string[] = [
    'fecha',
    'tipo',
    'numero',
    'opcionPorContratar',
    'valorSolicitado',
    'estado',
    'id'
  ];
  dataSource = new MatTableDataSource(ELEMENT_DATA);

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, {static: true}) sort: MatSort;

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  constructor() { }

  ngOnInit(): void {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    this.paginator._intl.nextPageLabel = 'Siguiente';
    this.paginator._intl.previousPageLabel = 'Anterior';
  }

  enviarSolicitud(e: number) {
    console.log(e);
  }

  verDetalle(e: number) {
    console.log(e);
  }

  registrarInformacion(e: number) {
    console.log(e);
  }

}
