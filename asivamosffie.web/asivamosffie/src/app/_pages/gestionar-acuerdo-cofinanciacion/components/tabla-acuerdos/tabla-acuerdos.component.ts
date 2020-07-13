import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

export interface PeriodicElement {
  id: number;
  fechaCreacion: string;
  numeroAcuerdo: string;
  vigenciaAcuerdo: number;
  valorTotal: number;
  estadoRegistro: string;
}

const ELEMENT_DATA: PeriodicElement[] = [
  {id: 1, fechaCreacion: '26/05/2020', numeroAcuerdo: '000001', vigenciaAcuerdo: 2020, valorTotal: 85000000, estadoRegistro: 'Completo'},
  {id: 2, fechaCreacion: '26/05/2020', numeroAcuerdo: '000001', vigenciaAcuerdo: 2020, valorTotal: 85000000, estadoRegistro: 'Completo'},
  {id: 3, fechaCreacion: '26/05/2020', numeroAcuerdo: '000001', vigenciaAcuerdo: 2020, valorTotal: 85000000, estadoRegistro: 'Completo'},
];

@Component({
  selector: 'app-tabla-acuerdos',
  templateUrl: './tabla-acuerdos.component.html',
  styleUrls: ['./tabla-acuerdos.component.scss']
})
export class TablaAcuerdosComponent implements OnInit {

  displayedColumns: string[] = ['fechaCreacion', 'numeroAcuerdo', 'vigenciaAcuerdo', 'valorTotal', 'estadoRegistro', 'id'];
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

  editarAcuerdo(e: number) {
    console.log(e);
  }
  eliminarAcuerdo(e: number) {
    console.log(e);
  }

}
