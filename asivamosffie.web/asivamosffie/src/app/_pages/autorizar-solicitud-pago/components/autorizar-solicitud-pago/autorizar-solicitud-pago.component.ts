import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { ObservacionesMultiplesCuService } from 'src/app/core/_services/observacionesMultiplesCu/observaciones-multiples-cu.service';
import { DialogEnvSolicitudAutorizComponent } from '../dialog-env-solicitud-autoriz/dialog-env-solicitud-autoriz.component';

@Component({
  selector: 'app-autorizar-solicitud-pago',
  templateUrl: './autorizar-solicitud-pago.component.html',
  styleUrls: ['./autorizar-solicitud-pago.component.scss']
})
export class AutorizarSolicitudPagoComponent implements OnInit {

    verAyuda = false;
    tipoSolicitudCodigo: any = {};
    listaEstadoSolicitudPago: any = {};
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

    constructor(
        private router: Router,
        private dialog: MatDialog,
        private routes: Router,
        private commonSvc: CommonService,
        private obsMultipleSvc: ObservacionesMultiplesCuService )
    {
        this.commonSvc.listaEstadoSolicitudPago()
            .subscribe(
                listaEstadoSolicitudPago => {
                    this.listaEstadoSolicitudPago = listaEstadoSolicitudPago;
                    console.log( this.listaEstadoSolicitudPago );
                    this.commonSvc.tiposDeSolicitudes()
                        .subscribe(
                            solicitudes => {
                                for ( const solicitud of solicitudes ) {
                                    if ( solicitud.codigo === '1' ) {
                                        this.tipoSolicitudCodigo.contratoObra = solicitud.codigo;
                                    }
                                    if ( solicitud.codigo === '2' ) {
                                        this.tipoSolicitudCodigo.contratoInterventoria = solicitud.codigo;
                                    }
                                    if ( solicitud.codigo === '3' ) {
                                        this.tipoSolicitudCodigo.expensas = solicitud.codigo;
                                    }
                                    if ( solicitud.codigo === '4' ) {
                                        this.tipoSolicitudCodigo.otrosCostos = solicitud.codigo;
                                    }
                                }
                                this.obsMultipleSvc.listaMenu()
                                    .subscribe(
                                        listaMenuId => {
                                            this.obsMultipleSvc.getListSolicitudPago( listaMenuId.aprobarSolicitudPagoId )
                                                .subscribe(
                                                    getListSolicitudPago => {
                                                        console.log( getListSolicitudPago );
                                                        this.dataSource = new MatTableDataSource(getListSolicitudPago);
                                                        this.dataSource.paginator = this.paginator;
                                                        this.dataSource.sort = this.sort;
                                                        this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
                                                    }
                                                );
                                        }
                                    );
                            }
                        );
                }
            )
    }

    ngOnInit(): void {
    }

    applyFilter(event: Event) {
      const filterValue = (event.target as HTMLInputElement).value;
      this.dataSource.filter = filterValue.trim().toLowerCase();
    }

    autorizarSolicitud(id){
      this.routes.navigate(['/autorizarSolicitudPago/autorizacionSolicitud',id]);
    }

    verDetalleEditar(id){
      this.routes.navigate(['/autorizarSolicitudPago/verDetalleEditarAutorizarSolicitud',id]);
    }

    verDetalle(id){
      this.routes.navigate(['/autorizarSolicitudPago/verDetalleAutorizarSolicitud',id]);
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
