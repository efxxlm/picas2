import { EstadoSolicitudPagoOrdenGiro, EstadosSolicitudPagoOrdenGiro, ListaMenu, ListaMenuId } from './../../../../_interfaces/estados-solicitudPago-ordenGiro.interface';
import { OrdenPagoService } from './../../../../core/_services/ordenPago/orden-pago.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { DialogDevolverSolPagoGogComponent } from '../dialog-devolver-sol-pago-gog/dialog-devolver-sol-pago-gog.component';
import moment from 'moment';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-generar-orden-giro',
  templateUrl: './generar-orden-giro.component.html',
  styleUrls: ['./generar-orden-giro.component.scss']
})
export class GenerarOrdenGiroComponent implements OnInit {

    verAyuda = false;
    listaMenu: ListaMenu = ListaMenuId;
    estadoSolicitudPagoOrdenGiro: EstadoSolicitudPagoOrdenGiro = EstadosSolicitudPagoOrdenGiro;
    dataSource = new MatTableDataSource();
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    displayedColumns: string[] = [
      'fechaValidacionFinanciera',
      'numeroSolicitud',
      'modalidadContrato',
      'numeroContrato',
      'estadoGeneracion',
      'estadoRegistro',
      'gestion'
    ];
    dataTable: any[] = [];

    constructor(
        private routes: Router,
        private dialog: MatDialog,
        private ordenGiroSvc: OrdenPagoService )
    {
        this.ordenGiroSvc.getListOrdenGiro( this.listaMenu.generarOrdenGiro )
            .subscribe(
                response => {
                    console.log( response );

                    response.forEach( registro => registro.fechaAprobacionFinanciera = moment( registro.fechaAprobacionFinanciera ).format( 'DD/MM/YYYY' ) );

                    this.dataSource = new MatTableDataSource( response );
                    this.dataSource.paginator = this.paginator;
                    this.dataSource.sort = this.sort;
                    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
                }
            );
    }

    ngOnInit(): void {
    }

    applyFilter(event: Event) {
      const filterValue = (event.target as HTMLInputElement).value;
      this.dataSource.filter = filterValue.trim().toLowerCase();
    }

    devolverSolicitud(){
        const dialogConfig = new MatDialogConfig();
        dialogConfig.height = 'auto';
        dialogConfig.width = '1020px';
        //dialogConfig.data = { id: id, idRol: idRol, numContrato: numContrato, fecha1Titulo: fecha1Titulo, fecha2Titulo: fecha2Titulo };
        const dialogRef = this.dialog.open(DialogDevolverSolPagoGogComponent, dialogConfig);
        //dialogRef.afterClosed().subscribe(value => {});
    }

    openDialog(modalTitle: string, modalText: string) {
        const dialogRef = this.dialog.open( ModalDialogComponent, {
            width: '28em',
            data: { modalTitle, modalText }
        });
    }

    enviarVerificacion( ordenGiroId: number ) {
        const pOrdenGiro = { 
            ordenGiroId,
            estadoCodigo: EstadosSolicitudPagoOrdenGiro.enviadaVerificacionOrdenGiro
        }

        this.ordenGiroSvc.changueStatusOrdenGiro( pOrdenGiro )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                        () => this.routes.navigate( [ '/generarOrdenDeGiro' ] )
                    );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }
}
