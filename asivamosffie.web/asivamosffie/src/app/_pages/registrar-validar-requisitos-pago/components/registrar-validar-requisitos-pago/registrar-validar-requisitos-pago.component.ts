import { EstadoSolicitudPagoOrdenGiro, EstadosSolicitudPagoOrdenGiro, TipoSolicitud, TipoSolicitudes } from './../../../../_interfaces/estados-solicitudPago-ordenGiro.interface';
import { ObservacionesMultiplesCuService } from 'src/app/core/_services/observacionesMultiplesCu/observaciones-multiples-cu.service';
import { CommonService } from './../../../../core/_services/common/common.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { RegistrarRequisitosPagoService } from 'src/app/core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { DialogDevolverSolicitudComponent } from '../dialog-devolver-solicitud/dialog-devolver-solicitud.component';

@Component({
  selector: 'app-registrar-validar-requisitos-pago',
  templateUrl: './registrar-validar-requisitos-pago.component.html',
  styleUrls: ['./registrar-validar-requisitos-pago.component.scss']
})
export class RegistrarValidarRequisitosPagoComponent implements OnInit {

    tipoSolicitud: TipoSolicitud = TipoSolicitudes;
    listaEstadoSolicitudPago: EstadoSolicitudPagoOrdenGiro = EstadosSolicitudPagoOrdenGiro;
    verAyuda = false;
    dataSource = new MatTableDataSource();
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    displayedColumns: string[] = [
        'fechaSolicitud',
        'numeroSolicitud',
        'modalidadContrato',
        'numeroContrato',
        'estadoRegistroPago',
        'estadoRegistro',
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
        this.registrarPagosSvc.getListSolicitudPago()
            .subscribe(
                response => {
                    console.log( response );
                    this.dataSource = new MatTableDataSource( response );
                    this.dataSource.paginator = this.paginator;
                    this.dataSource.sort = this.sort;
                    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
                }
            );
    }

    ngOnInit(): void {
    };

    applyFilter(event: Event) {
        const filterValue = (event.target as HTMLInputElement).value;
        this.dataSource.filter = filterValue.trim().toLowerCase();
    };

    registrarNuevaSolicitud(){
        this.router.navigate(['registrarValidarRequisitosPago/registrarNuevaSolicitudPago'])
    }

    openDialog(modalTitle: string, modalText: string) {
        const dialogRef = this.dialog.open(ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText }
        });
    }

    openDialogTrueFalse(modalTitle: string, modalText: string) {
        const dialogRef = this.dialog.open(ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText, siNoBoton: true }
        });

        return dialogRef.afterClosed();
    }

    devolverSolicitudDialog( registro: any ) {
        const dialogRef = this.dialog.open( DialogDevolverSolicitudComponent , {
          width: '65em',
          data: { registro, solicitudDevueltaEquipoFacturacion: this.listaEstadoSolicitudPago.solicitudDevueltaEquipoFacturacion }
        });
    }

    deleteSolicitudPago( pSolicitudPagoId: number ) {
        this.openDialogTrueFalse( '', '<b>¿Está seguro de eliminar esta información?</b>' )
            .subscribe(
                value => {
                    if ( value === true ) {
                        this.registrarPagosSvc.deleteSolicitudPago( pSolicitudPagoId )
                            .subscribe(
                                response => {
                                    this.openDialog( '', `<b>${ response.message }</b>` );
                                    this.routes.navigateByUrl( '/', {skipLocationChange: true} )
                                        .then( () => this.routes.navigate( ['/registrarValidarRequisitosPago'] )
                                    );
                                }, err => this.openDialog( '', `<b>${ err.message }</b>` )
                            );
                    }
                }
            );
    }

    changueStatusSolicitudPago( pSolicitudPagoId: number ) {
        const pSolicitudPago = {
            solicitudPagoId: pSolicitudPagoId,
            estadoCodigo: this.listaEstadoSolicitudPago.enviadaParaVerificacion
        };

        this.obsMultipleSvc.changueStatusSolicitudPago( pSolicitudPago )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.routes.navigateByUrl( '/', {skipLocationChange: true} )
                        .then( () => this.routes.navigate( ['/registrarValidarRequisitosPago'] )
                    );
                }, err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

}
