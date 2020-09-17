import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { FuenteFinanciacion, FuenteFinanciacionService } from 'src/app/core/_services/fuenteFinanciacion/fuente-financiacion.service';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { forkJoin } from 'rxjs';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { ViewFlags } from '@angular/compiler/src/core';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { Respuesta } from 'src/app/core/_services/autenticacion/autenticacion.service';
export interface Contrato {
  fechaFirma: string;
  numeroContrato: string;
  tipoSolicitud: string;
  estado: string;
}

const ELEMENT_DATA: Contrato[] = [
  {fechaFirma: "20/06/2020", numeroContrato: 'A886675445',tipoSolicitud:'Modificación Contractual',estado:'Sin registro presupuestal'},
  {fechaFirma: "21/06/2020", numeroContrato: 'C223456789',tipoSolicitud:'Contratación',estado:'Sin registro presupuestal'},
  {fechaFirma: "10/06/2020", numeroContrato: 'C848784551',tipoSolicitud:'Modificación Contractual',estado:'Cancelada'},
];
@Component({
  selector: 'app-tabla-registro-presupuestal',
  templateUrl: './tabla-registro-presupuestal.component.html',
  styleUrls: ['./tabla-registro-presupuestal.component.scss']
})
export class TablaRegistroPresupuestalComponent implements OnInit {
  displayedColumns: string[] = [ 'fechaFirma', 'numeroContrato', 'tipoSolicitud', 'estado', 'id'];
  dataSource = new MatTableDataSource();
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, {static: true}) sort: MatSort;
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }
  constructor( private router: Router, public dialog: MatDialog) { }

  ngOnInit(): void {
    this.dataSource = new MatTableDataSource(ELEMENT_DATA);
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    this.paginator._intl.nextPageLabel = 'Siguiente';
    this.paginator._intl.previousPageLabel = 'Anterior';
  }
  gestionarDRP(){
    this.router.navigate(['/generarRegistroPresupuestal/gestionarDrp']);
  }
  verDetalle(){
    this.router.navigate(['/generarRegistroPresupuestal/verDetalle']);
  }
}
