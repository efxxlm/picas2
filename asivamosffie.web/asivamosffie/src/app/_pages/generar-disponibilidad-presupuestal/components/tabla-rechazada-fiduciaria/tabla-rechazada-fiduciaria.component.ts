import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

export interface OrdenDelDia {
  id: number;
  fecha: string;
  numero: string;
  tipo: string;
  esNovedad: boolean;
  novedadId: number;
}

@Component({
  selector: 'app-tabla-rechazada-fiduciaria',
  templateUrl: './tabla-rechazada-fiduciaria.component.html',
  styleUrls: ['./tabla-rechazada-fiduciaria.component.scss']
})
export class TablaRechazadaFiduciariaComponent implements OnInit {

  displayedColumns: string[] = ['fecha', 'numero', 'tipo', 'id'];
  dataSource = new MatTableDataSource();
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
      if(element.rechazadaFiduciaria === true && element.estadoStr == 'Con disponibilidad presupuestal'){
        elements.push({id:element.disponibilidadPresupuestalId,
          fecha:element.fechaSolicitud,numero:element.numeroSolicitud,
          tipo:element.tipoSolicitud, esNovedad: element.esNovedad,
          novedadId: element.novedadContractualRegistroPresupuestalId})
      }
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
