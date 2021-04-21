import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import moment from 'moment';
import { ObservacionesMultiplesCuService } from 'src/app/core/_services/observacionesMultiplesCu/observaciones-multiples-cu.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { EstadoSolicitudPagoOrdenGiro, EstadosSolicitudPagoOrdenGiro, TipoSolicitud, TipoSolicitudes } from 'src/app/_interfaces/estados-solicitudPago-ordenGiro.interface';
import { DialogRechazarSolicitudVfspComponent } from '../dialog-rechazar-solicitud-vfsp/dialog-rechazar-solicitud-vfsp.component';

@Component({
  selector: 'app-verificar-financ-solicitud-pago',
  templateUrl: './verificar-financ-solicitud-pago.component.html',
  styleUrls: ['./verificar-financ-solicitud-pago.component.scss']
})
export class VerificarFinancSolicitudPagoComponent implements OnInit {

    verAyuda = false;
    tipoSolicitud: TipoSolicitud = TipoSolicitudes;
    listaEstadoSolicitudPago: EstadoSolicitudPagoOrdenGiro = EstadosSolicitudPagoOrdenGiro;
    dataSource = new MatTableDataSource();
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    displayedColumns: string[] = [
      'fechaSolicitud',
      'numeroSolicitud',
      'modalidadContrato',
      'numeroContrato',
      'estadoVerificacion',
      'estadoRegistro',
      'gestion'
    ];

    constructor(
        private dialog: MatDialog,
        private routes: Router,
        private obsMultipleSvc: ObservacionesMultiplesCuService )
    {
        this.obsMultipleSvc.listaMenu()
            .subscribe(
                listaMenuId => {
                    this.obsMultipleSvc.getListSolicitudPago( listaMenuId.verificarFinancieramenteId )
                        .subscribe(
                            getListSolicitudPago => {
                                getListSolicitudPago.forEach( registro => registro.fechaCreacion = moment( registro.fechaCreacion ).format( 'DD/MM/YYYY' ) )

                                this.dataSource = new MatTableDataSource(getListSolicitudPago);
                                this.dataSource.paginator = this.paginator;
                                this.dataSource.sort = this.sort;
                                this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
                            }
                        );
                }
            );
    }

    ngOnInit(): void {
        this.dataSource = new MatTableDataSource();
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
        this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    }

    applyFilter(event: Event) {
      const filterValue = (event.target as HTMLInputElement).value;
      this.dataSource.filter = filterValue.trim().toLowerCase();
    }

    openDialog(modalTitle: string, modalText: string) {
        const dialogRef = this.dialog.open( ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText }
        });
    }

    getRechazo( registro: any ) {
        this.dialog.open( DialogRechazarSolicitudVfspComponent, {
            width: '90em',
            data: registro
        });
    }

    subsanarSolicitud( pSolicitudPagoId: number ) {
        const pSolicitudPago = {
            solicitudPagoId: pSolicitudPagoId,
            estadoCodigo: this.listaEstadoSolicitudPago.enviadaSubsanacionVerificacionFinanciera
        };

        this.obsMultipleSvc.changueStatusSolicitudPago( pSolicitudPago )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.routes.navigateByUrl( '/', {skipLocationChange: true} )
                        .then( () => this.routes.navigate( ['/verificarFinancieramenteSolicitudDePago'] ) );
                }, err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

    changueStatusSolicitudPago( pSolicitudPagoId: number ) {
        const pSolicitudPago = {
            solicitudPagoId: pSolicitudPagoId,
            estadoCodigo: this.listaEstadoSolicitudPago.enviarParaValidacionFinanciera
        };

        this.obsMultipleSvc.changueStatusSolicitudPago( pSolicitudPago )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.routes.navigateByUrl( '/', {skipLocationChange: true} )
                        .then( () => this.routes.navigate( ['/verificarFinancieramenteSolicitudDePago'] ) );
                }, err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

}
