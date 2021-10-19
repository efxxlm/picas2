import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';

export interface OrdenDelDia {
  id: number;
  fecha: string;
  numero: string;
  tipo: string;
  esNovedad: boolean;
  novedadId: number;
}

@Component({
  selector: 'app-tabla-con-disponibilidad-presupuestal',
  templateUrl: './tabla-con-disponibilidad-presupuestal.component.html',
  styleUrls: ['./tabla-con-disponibilidad-presupuestal.component.scss']
})
export class TablaConDisponibilidadPresupuestalComponent implements OnInit {

  displayedColumns: string[] = ['fecha', 'numero', 'tipo', 'id'];
  dataSource = new MatTableDataSource();
  @Input()disponibilidadPresupuestal: any;
  @Input()esLiberacion: boolean;

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  constructor(private router: Router) { }

  ngOnInit(): void {
    let elements:OrdenDelDia[]=[];
    this.disponibilidadPresupuestal.disponibilidadPresupuestal.forEach(element => {
      if(element.rechazadaFiduciaria !== true){
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

  verDetalle(id: number, esNovedad, novedadId) {
    if(this.esLiberacion == true){
      this.router.navigate(['generarDisponibilidadPresupuestal/conLiberacionSaldo', id, esNovedad, novedadId ? novedadId : 0]);
    }
    else{
      this.router.navigate(['generarDisponibilidadPresupuestal/detalleConDisponibilidadPresupuestal', id, esNovedad, novedadId ? novedadId : 0]);
    }
  }


}
