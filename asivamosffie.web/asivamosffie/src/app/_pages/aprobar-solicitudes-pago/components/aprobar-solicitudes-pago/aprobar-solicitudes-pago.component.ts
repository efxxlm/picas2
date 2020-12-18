import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { DialogEnvioAutorizacionComponent } from '../dialog-envio-autorizacion/dialog-envio-autorizacion.component';

@Component({
  selector: 'app-aprobar-solicitudes-pago',
  templateUrl: './aprobar-solicitudes-pago.component.html',
  styleUrls: ['./aprobar-solicitudes-pago.component.scss']
})
export class AprobarSolicitudesPagoComponent implements OnInit {
  verAyuda = false;
  dataSource = new MatTableDataSource();
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = [
    'fechaSolicitud',
    'numeroSolicitud',
    'modalidadContrato',
    'numeroContrato',
    'estadoAprobacion',
    'gestion'
  ];
  dataTable: any[] = [
    {
      fechaSolicitud: '05/10/2020',
      numeroSolicitud: 'SolPagoO0001',
      modalidadContrato: 'Tipo B',
      numeroContrato: 'N801801',
      estadoAprobacion: 'Con solicitud aprobada por el supervisor',
      gestion: 1
    },
    {
      fechaSolicitud: '08/10/2020',
      numeroSolicitud: 'SolPagoEspecial0001',
      modalidadContrato: 'No Aplica',
      numeroContrato: 'N326326',
      estadoAprobacion: 'Sin aprobación',
      gestion: 2
    },
  ];
  constructor(private routes: Router, public dialog: MatDialog) { }

  ngOnInit(): void {
    this.dataSource = new MatTableDataSource(this.dataTable);
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
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
  };

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };
  aprobarSolicitud(id){
    this.routes.navigate(['/aprobarSolicitudesPago/aprobacionSolicitud',id]);
  }
  verDetalleEditar(id){
    this.routes.navigate(['/aprobarSolicitudesPago/verDetalleEditarAprobarSolicitud',id]);
  }
  verDetalle(id){
    this.routes.navigate(['/aprobarSolicitudesPago/verDetalleAprobarSolicitud',id]);
  }
  openCertificate(){
    const dialogConfig = new MatDialogConfig();
    dialogConfig.height = 'auto';
    dialogConfig.width = '1020px';
    //dialogConfig.data = { id: id, idRol: idRol, numContrato: numContrato, fecha1Titulo: fecha1Titulo, fecha2Titulo: fecha2Titulo };
    const dialogRef = this.dialog.open(DialogEnvioAutorizacionComponent, dialogConfig);
    //dialogRef.afterClosed().subscribe(value => {});
  }
}
