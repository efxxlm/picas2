import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';

export interface PeriodicElement {
  id: number;
  fecha: string;
  numero: string;
  tipo: string;
  estadoRegistro: boolean;
}

const ELEMENT_DATA: PeriodicElement[] = [
  { id: 1, fecha: '26/05/2020', numero: '003', tipo: 'Modificación contractual', estadoRegistro: false },
  { id: 2, fecha: '26/05/2020', numero: 'PI_003', tipo: 'Contratación', estadoRegistro: false },
  { id: 3, fecha: '26/05/2020', numero: 'PI_002', tipo: 'Contratación', estadoRegistro: false },
];

@Component({
  selector: 'app-tabla-en-validacion',
  templateUrl: './tabla-en-validacion.component.html',
  styleUrls: ['./tabla-en-validacion.component.scss']
})
export class TablaEnValidacionComponent implements OnInit {

  displayedColumns: string[] = ['fecha', 'numero', 'tipo', 'estadoRegistro', 'id'];
  dataSource = new MatTableDataSource(ELEMENT_DATA);

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  constructor(private router: Router) { }

  ngOnInit(): void {
    this.inicializarTabla();
  }
  inicializarTabla() {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    this.paginator._intl.nextPageLabel = 'Siguiente';
    this.paginator._intl.getRangeLabel = (page, pageSize, length) => {
      if (length === 0 || pageSize === 0) {
        return '0 de ' + length;
      }
      length = Math.max(length, 0);
      const startIndex = page * pageSize;
      const endIndex = startIndex < length ?
        Math.min(startIndex + pageSize, length) :
        startIndex + pageSize;
      return startIndex + 1 + ' - ' + endIndex + ' de ' + length;
    };
    this.paginator._intl.previousPageLabel = 'Anterior';
  }

  validar(id: number) {
    console.log(id);
    this.router.navigate(['validarDisponibilidadPresupuesto/validacionPresupuestal', id]);
  }

}
