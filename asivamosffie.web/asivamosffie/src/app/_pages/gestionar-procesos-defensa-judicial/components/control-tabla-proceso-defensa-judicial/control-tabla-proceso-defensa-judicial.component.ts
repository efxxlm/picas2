import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
@Component({
  selector: 'app-control-tabla-proceso-defensa-judicial',
  templateUrl: './control-tabla-proceso-defensa-judicial.component.html',
  styleUrls: ['./control-tabla-proceso-defensa-judicial.component.scss']
})
export class ControlTablaProcesoDefensaJudicialComponent implements OnInit {
  displayedColumns: string[] = ['fecha', 'legitimacion', 'tipoAccion', 'numeroProceso', 'estadoProceso', 'gestion'];
  dataSource = new MatTableDataSource();
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  dataTable: any[] = [
    {
      fecha: '19/06/2020',
      legitimacion: 'Activa',
      tipoAccion: 'Reparación Directa',
      numeroProceso: 'DJ0012020',
      estadoProceso: 'En análisis jurídico',
      id: 1
    },
    {
      fecha: '22/06/2020',
      legitimacion: 'Pasiva',
      tipoAccion: 'Reparación Directa',
      numeroProceso: 'DJ0012021',
      estadoProceso: 'En análisis jurídico',
      id: 2
    },
    {
      fecha: '22/06/2020',
      legitimacion: 'Activa',
      tipoAccion: 'Reparación Directa',
      numeroProceso: 'DJ0012021',
      estadoProceso: 'Aprobado por comité técnico',
      id: 3
    }
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
    this.router.navigate(['/gestionarProcesoDefensaJudicial/registrarNuevoProcesoJudicial']);
  }
  editProceso(id){
    this.router.navigate(['/gestionarProcesoDefensaJudicial/verDetalleEditarProceso',id]);
  }
  actualizarProceso(id){
    this.router.navigate(['/gestionarProcesoDefensaJudicial/actualizarProceso',id]);
  }
}
