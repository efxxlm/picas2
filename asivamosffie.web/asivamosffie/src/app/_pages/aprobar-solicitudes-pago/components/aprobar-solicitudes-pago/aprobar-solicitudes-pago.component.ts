import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { ObservacionesMultiplesCuService } from 'src/app/core/_services/observacionesMultiplesCu/observaciones-multiples-cu.service';
import { RegistrarRequisitosPagoService } from 'src/app/core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';
import { DialogEnvioAutorizacionComponent } from '../dialog-envio-autorizacion/dialog-envio-autorizacion.component';

@Component({
  selector: 'app-aprobar-solicitudes-pago',
  templateUrl: './aprobar-solicitudes-pago.component.html',
  styleUrls: ['./aprobar-solicitudes-pago.component.scss']
})
export class AprobarSolicitudesPagoComponent implements OnInit {
    
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
        private obsMultipleSvc: ObservacionesMultiplesCuService,
        private registrarPagosSvc: RegistrarRequisitosPagoService )
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
