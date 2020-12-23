import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';

@Component({
  selector: 'app-control-tabla-actuacion-proceso',
  templateUrl: './control-tabla-actuacion-proceso.component.html',
  styleUrls: ['./control-tabla-actuacion-proceso.component.scss']
})
export class ControlTablaActuacionProcesoComponent implements OnInit {
  displayedColumns: string[] = ['fecha', 'numeroActuacion', 'actuacion', 'estadoRegistro', 'gestion'];
  dataSource = new MatTableDataSource();
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  dataTable: any[] = [
    {
      fecha: '05/08/2020',
      numeroActuacion: 'ACT defensa 0001',
      actuacion: 'En notificación',
      estadoRegistro: 'Completo',
      id: 1
    },
  ]
  constructor(private router: Router) { }

  ngOnInit(): void {
    this.dataSource = new MatTableDataSource(this.dataTable);
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
  }
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };
  irNuevo() {
    this.router.navigate(['/gestionarProcesoDefensaJudicial/registrarActuacionProceso']);
  }
  verDetalle(id){
    this.router.navigate(['/gestionarProcesoDefensaJudicial/verDetalleActuacion',id]);
  }
  verDetalleEditar(id){
    this.router.navigate(['/gestionarProcesoDefensaJudicial/verDetalleEditarActuacionProceso',id]);
  }
}
