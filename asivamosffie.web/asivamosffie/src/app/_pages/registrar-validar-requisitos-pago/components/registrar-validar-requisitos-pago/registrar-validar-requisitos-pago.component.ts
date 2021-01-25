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

    tipoSolicitudCodigo: any = {};
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
        'gestion'
    ];

    constructor(
        private router: Router,
        private dialog: MatDialog,
        private commonSvc: CommonService,
        private registrarPagosSvc: RegistrarRequisitosPagoService, )
    {
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

    devolverSolictud( pSolicitudPagoId: number ){
        this.registrarPagosSvc.deleteSolicitudPago( pSolicitudPagoId )
            .subscribe(
                response => this.openDialog( '', `<b>${ response.message }</b>` ),
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

}
