import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

export interface PeriodicElement {
  id: number;
  fechaCreacion: string;
  tipoAportante: string;
  aportante: string;
  vigencia: string;
  fuenteDeRecursos: string;
  valorAporteFuenteDeRecursos: number;
  valorAporteEnCuenta: number;
  estado: string;
}

const ELEMENT_DATA: PeriodicElement[] = [
  {
    id: 1,
    fechaCreacion: '24/03/2020',
    tipoAportante: 'FFIE',
    aportante: 'FFIE',
    vigencia: '',
    fuenteDeRecursos: 'Recursos propios',
    valorAporteFuenteDeRecursos: 177000000,
    valorAporteEnCuenta: null,
    estado: 'Completo'
  },
  {
    id: 2,
    fechaCreacion: '20/03/2020',
    tipoAportante: 'Tercero',
    aportante: 'Fundación Pies Descalzos',
    vigencia: '2020',
    fuenteDeRecursos: 'Recursos propios',
    valorAporteFuenteDeRecursos: 55000000,
    valorAporteEnCuenta: null,
    estado: 'Completo'
  }
];

@Component({
  selector: 'app-tabla-fuentes',
  templateUrl: './tabla-fuentes.component.html',
  styleUrls: ['./tabla-fuentes.component.scss']
})
export class TablaFuentesComponent implements OnInit {

  displayedColumns: string[] = [ 'fechaCreacion', 'tipoAportante', 'aportante', 'vigencia', 'fuenteDeRecursos', 'valorAporteFuenteDeRecursos', 'valorAporteEnCuenta', 'estado', 'id'];
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

  editarFuente(e: number) {
    console.log(e);
  }
  editarAcuerdo(e: number) {
    console.log(e);
  }
  eliminarFuente(e: number) {
    console.log(e);
  }
  eliminarAcuerdo(e: number) {
    console.log(e);
  }
  controlRecursosFuente(e: number) {
    console.log(e);
  }

}
