import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { DialogEnvSolicitudAutorizComponent } from '../dialog-env-solicitud-autoriz/dialog-env-solicitud-autoriz.component';

@Component({
  selector: 'app-autorizar-solicitud-pago',
  templateUrl: './autorizar-solicitud-pago.component.html',
  styleUrls: ['./autorizar-solicitud-pago.component.scss']
})
export class AutorizarSolicitudPagoComponent implements OnInit {

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
      estadoAprobacion: 'Con solicitud autorizada por el coordinador',
      gestion: 1
    },
    {
      fechaSolicitud: '08/10/2020',
      numeroSolicitud: 'SolPagoEspecial0001',
      modalidadContrato: 'No Aplica',
      numeroContrato: 'N326326',
      estadoAprobacion: 'Sin autorización',
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
  autorizarSolicitud(id){
    this.routes.navigate(['/autorizarSolicitudDePago/autorizacionSolicitud',id]);
  }
  verDetalleEditar(id){
    this.routes.navigate(['/autorizarSolicitudDePago/verDetalleEditarAutorizarSolicitud',id]);
  }
  verDetalle(id){
    this.routes.navigate(['/autorizarSolicitudDePago/verDetalleAutorizarSolicitud',id]);
  }
  openCertificate(){
    const dialogConfig = new MatDialogConfig();
    dialogConfig.height = 'auto';
    dialogConfig.width = '1020px';
    //dialogConfig.data = { id: id, idRol: idRol, numContrato: numContrato, fecha1Titulo: fecha1Titulo, fecha2Titulo: fecha2Titulo };
    const dialogRef = this.dialog.open(DialogEnvSolicitudAutorizComponent, dialogConfig);
    //dialogRef.afterClosed().subscribe(value => {});
  }

}
