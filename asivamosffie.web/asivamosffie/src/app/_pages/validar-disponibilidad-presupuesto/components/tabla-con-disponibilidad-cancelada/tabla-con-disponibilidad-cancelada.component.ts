import { Component, OnInit, ViewChild, Input } from '@angular/core';
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
  { id: 1, fecha: '07/07/2020', numero: '003', tipo: 'Modificación contractual', estadoRegistro: true },
];

@Component({
  selector: 'app-tabla-con-disponibilidad-cancelada',
  templateUrl: './tabla-con-disponibilidad-cancelada.component.html',
  styleUrls: ['./tabla-con-disponibilidad-cancelada.component.scss']
})
export class TablaConDisponibilidadCanceladaComponent implements OnInit {
  @Input()disponibilidadPresupuestal: any;
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
    let elements:PeriodicElement[]=[];
    this.disponibilidadPresupuestal.disponibilidadPresupuestal.forEach(element => {
      elements.push({id:element.disponibilidadPresupuestalId,
        fecha:element.fechaSolicitud,estadoRegistro:element.estadoRegistro,numero:element.numeroSolicitud,
        tipo:element.tipoSolicitud})
    });
    this.dataSource = new MatTableDataSource(elements);
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
 
  verDetalle(id: number) {
    console.log(id);
    this.router.navigate(['validarDisponibilidadPresupuesto/conValidacionPresupuestal', id]);
  }

}
