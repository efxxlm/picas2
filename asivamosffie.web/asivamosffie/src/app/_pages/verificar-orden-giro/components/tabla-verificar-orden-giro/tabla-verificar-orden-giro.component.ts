import { DialogEnviarAprobacionComponent } from './../dialog-enviar-aprobacion/dialog-enviar-aprobacion.component';
import { MatDialog } from '@angular/material/dialog';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { EstadoSolicitudPagoOrdenGiro, EstadosSolicitudPagoOrdenGiro, ListaMenu, ListaMenuId } from 'src/app/_interfaces/estados-solicitudPago-ordenGiro.interface';
import { Router } from '@angular/router';
import { OrdenPagoService } from 'src/app/core/_services/ordenPago/orden-pago.service';
import moment from 'moment';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-tabla-verificar-orden-giro',
  templateUrl: './tabla-verificar-orden-giro.component.html',
  styleUrls: ['./tabla-verificar-orden-giro.component.scss']
})
export class TablaVerificarOrdenGiroComponent implements OnInit {

    listaMenu: ListaMenu = ListaMenuId;
    estadoSolicitudPagoOrdenGiro: EstadoSolicitudPagoOrdenGiro = EstadosSolicitudPagoOrdenGiro;
    tablaVerificar = new MatTableDataSource();
    @ViewChild( MatPaginator, { static: true } ) paginator: MatPaginator;
    @ViewChild( MatSort, { static: true } ) sort: MatSort;
    displayedColumns: string[]  = [
      'fechaGeneracion',
      'numeroOrden',
      'modalidad',
      'numeroContrato',
      'estadoVerificacion',
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

                    this.tablaVerificar = new MatTableDataSource( response );
                    this.tablaVerificar.paginator = this.paginator;
                    this.tablaVerificar.sort = this.sort;
                    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
                }
            );
    }

    ngOnInit(): void {
    }

    applyFilter( event: Event ) {
        const filterValue = (event.target as HTMLInputElement).value;
        this.tablaVerificar.filter = filterValue.trim().toLowerCase();
    }

    openDialog( modalTitle: string, modalText: string ) {
        this.dialog.open( ModalDialogComponent, {
          width: '28em',
          data : { modalTitle, modalText }
        });
    }

    openDialogEnviarAprobacion( registro: any ) {
        this.dialog.open( DialogEnviarAprobacionComponent, {
          width: '28em',
          data: registro
        });
    }

    devolverOrdenGiro( registro: any ) {
        const pOrdenGiro = {
            ordenGiroId: registro.ordenGiroId,
            estadoCodigo: EstadosSolicitudPagoOrdenGiro.ordenGiroDevueltaPorVerificacion
        }

        this.ordenGiroSvc.changueStatusOrdenGiro( pOrdenGiro )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                        () => this.routes.navigate( [ '/verificarOrdenGiro' ] )
                    );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

}
