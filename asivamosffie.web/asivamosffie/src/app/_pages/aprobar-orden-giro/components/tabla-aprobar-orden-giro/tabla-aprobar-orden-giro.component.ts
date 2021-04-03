import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import moment from 'moment';
import { OrdenPagoService } from 'src/app/core/_services/ordenPago/orden-pago.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { EstadoSolicitudPagoOrdenGiro, EstadosSolicitudPagoOrdenGiro, ListaMenu, ListaMenuId } from 'src/app/_interfaces/estados-solicitudPago-ordenGiro.interface';
import { DialogEnviarAprobacionComponent } from '../dialog-enviar-aprobacion/dialog-enviar-aprobacion.component';

@Component({
  selector: 'app-tabla-aprobar-orden-giro',
  templateUrl: './tabla-aprobar-orden-giro.component.html',
  styleUrls: ['./tabla-aprobar-orden-giro.component.scss']
})
export class TablaAprobarOrdenGiroComponent implements OnInit {

    listaMenu: ListaMenu = ListaMenuId;
    estadoSolicitudPagoOrdenGiro: EstadoSolicitudPagoOrdenGiro = EstadosSolicitudPagoOrdenGiro;
    tablaAprobar = new MatTableDataSource();
    @ViewChild( MatPaginator, { static: true } ) paginator: MatPaginator;
    @ViewChild( MatSort, { static: true } ) sort: MatSort;
    displayedColumns: string[]  = [
      'fechaVerificacion',
      'numeroOrden',
      'modalidad',
      'numeroContrato',
      'estadoAprobacion',
      'gestion'
    ];

    constructor(
        private routes: Router,
        private dialog: MatDialog,
        private ordenGiroSvc: OrdenPagoService )
    {
        this.ordenGiroSvc.getListOrdenGiro( this.listaMenu.verificarOrdenGiro )
            .subscribe(
                response => {
                    console.log( response );

                    response.forEach( registro => registro.fechaAprobacionFinanciera = moment( registro.fechaAprobacionFinanciera ).format( 'DD/MM/YYYY' ) );

                    this.tablaAprobar = new MatTableDataSource( response );
                    this.tablaAprobar.paginator = this.paginator;
                    this.tablaAprobar.sort = this.sort;
                    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
                }
            );
    }

    ngOnInit(): void {
    }

    applyFilter( event: Event ) {
        const filterValue = (event.target as HTMLInputElement).value;
        this.tablaAprobar.filter = filterValue.trim().toLowerCase();
    }

    openDialog( modalTitle: string, modalText: string ) {
        this.dialog.open( ModalDialogComponent, {
          width: '40em',
          data : { modalTitle, modalText }
        });
    }

    openDialogEnviarAprobacion( registro: any ) {
        this.dialog.open( DialogEnviarAprobacionComponent, {
            width: '80em',
            data: registro
        });
    }

    devolverOrdenGiro( registro: any ) {
        const pOrdenGiro = {
            ordenGiroId: registro.ordenGiroId,
            estadoCodigo: EstadosSolicitudPagoOrdenGiro.ordenGiroDevueltaPorAprobacion
        }

        this.ordenGiroSvc.changueStatusOrdenGiro( pOrdenGiro )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                        () => this.routes.navigate( [ '/aprobarOrdenGiro' ] )
                    );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

}
