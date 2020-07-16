import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

export interface ProcesosElement {
  id: number;
  fechaCreacion: string;
  tipo: string;
  numero: string;
  etapa: string;
  estadoDelProceso: string;
  estadoDelRegistro: string;
}

const ELEMENT_DATA: ProcesosElement[] = [
  {
    id: 1,
    fechaCreacion: '01/06/2020',
    tipo: 'Selección privada',
    numero: 'SP 0007-2020',
    etapa: 'Estructuración',
    estadoDelProceso: 'Creado',
    estadoDelRegistro: 'Completo'
  }
];

@Component({
  selector: 'app-tabla-procesos',
  templateUrl: './tabla-procesos.component.html',
  styleUrls: ['./tabla-procesos.component.scss']
})
export class TablaProcesosComponent implements OnInit {

  displayedColumns: string[] = [ 'fechaCreacion', 'tipo', 'numero', 'etapa', 'estadoDelProceso', 'estadoDelRegistro', 'id'];
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

  verMas(e: number) {
    console.log(e);
  }

}
