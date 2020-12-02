import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

export interface OrdenDelDia {
  id: number;
  fecha: string;
  numero: string;
  estadoRegistro:string;
  tipo: string;
}

const ELEMENT_DATA: OrdenDelDia[] = [];

@Component({
  selector: 'app-tabla-con-validacion-presupuestal',
  templateUrl: './tabla-con-validacion-presupuestal.component.html',
  styleUrls: ['./tabla-con-validacion-presupuestal.component.scss']
})
export class TablaConValidacionPresupuestalComponent implements OnInit {

  displayedColumns: string[] = ['fecha', 'numero', 'tipo','estadoRegistro', 'id'];
  dataSource = new MatTableDataSource(ELEMENT_DATA);
  @Input()disponibilidadPresupuestal: any;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  constructor() { }

  ngOnInit(): void {
    let elements:OrdenDelDia[]=[];
    this.disponibilidadPresupuestal.disponibilidadPresupuestal.forEach(element => {
      if(element.numeroDdp==null)
      {
        if(element.estadoRegistro)
        {
          element.estadoRegistro=false;
        }
      }      
      elements.push({id:element.disponibilidadPresupuestalId,
        fecha:element.fechaSolicitud,numero:element.numeroSolicitud,estadoRegistro:element.estadoRegistro,
        tipo:element.tipoSolicitud})
    });
    this.dataSource = new MatTableDataSource(elements);
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

}
