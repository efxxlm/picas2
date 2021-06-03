import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import moment from 'moment';
import { ObservacionesOrdenGiroService } from 'src/app/core/_services/observacionesOrdenGiro/observaciones-orden-giro.service';
import { OrdenPagoService } from 'src/app/core/_services/ordenPago/orden-pago.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { EstadoSolicitudPagoOrdenGiro, EstadosSolicitudPagoOrdenGiro, ListaMenu, ListaMenuId } from 'src/app/_interfaces/estados-solicitudPago-ordenGiro.interface';
import { DialogDescargarOrdenGiroComponent } from '../dialog-descargar-orden-giro/dialog-descargar-orden-giro.component';

@Component({
  selector: 'app-tabla-tramitar-orden-giro',
  templateUrl: './tabla-tramitar-orden-giro.component.html',
  styleUrls: ['./tabla-tramitar-orden-giro.component.scss']
})
export class TablaTramitarOrdenGiroComponent implements OnInit {

    listaMenu: ListaMenu = ListaMenuId;
    estadoSolicitudPagoOrdenGiro: EstadoSolicitudPagoOrdenGiro = EstadosSolicitudPagoOrdenGiro;
    tablaTramitar = new MatTableDataSource();
    tieneOrdenTramitada = false;
    @ViewChild( MatPaginator, { static: true } ) paginator: MatPaginator;
    @ViewChild( MatSort, { static: true } ) sort: MatSort;
    displayedColumns: string[]  = [
      'fechaAprobacion',
      'numeroOrden',
      'modalidad',
      'numeroContrato',
      'estadoTramite',
      'gestion'
    ];

    constructor(
        private routes: Router,
        private dialog: MatDialog,
        private ordenGiroSvc: OrdenPagoService,
        private obsOrdenGiro: ObservacionesOrdenGiroService )
    {
        this.ordenGiroSvc.getListOrdenGiro( this.listaMenu.verificarOrdenGiro )
            .subscribe(
                response => {
                    console.log( response );
                    const registroTramitado = response.find( registro => registro.estadoCodigo === this.estadoSolicitudPagoOrdenGiro.conOrdenGiroTramitada );

                    if ( registroTramitado !== undefined ) {
                        this.tieneOrdenTramitada = true;
                    }
                    response.forEach( registro => registro.fechaRegistroCompletoAprobar = moment( registro.fechaRegistroCompletoAprobar ).format( 'DD/MM/YYYY' ) );

                    this.tablaTramitar = new MatTableDataSource( response );
                    this.tablaTramitar.paginator = this.paginator;
                    this.tablaTramitar.sort = this.sort;
                    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
                }
            );
    }

    ngOnInit(): void {
    }

    applyFilter( event: Event ) {
        const filterValue = (event.target as HTMLInputElement).value;
        this.tablaTramitar.filter = filterValue.trim().toLowerCase();
    }

    openDialog( modalTitle: string, modalText: string ) {
        this.dialog.open( ModalDialogComponent, {
          width: '28em',
          data : { modalTitle, modalText }
        });
    }

    openDialogDescargarOrdenGiro() {
        this.dialog.open( DialogDescargarOrdenGiroComponent, {
          width: '80em'
        });
    }

    getOrdenGiroAprobadas() {
        const pDescargarOrdenGiro = {
            registrosAprobados: true
        }

        this.obsOrdenGiro.getListOrdenGiro( pDescargarOrdenGiro )
        .subscribe(
            response => {
                const blob = new Blob( [ response ], { type: 'application/pdf' } );
                const anchor = document.createElement('a');

                anchor.download = `Listado órdenes de giro aprobadas`;
                anchor.href = window.URL.createObjectURL( blob );
                anchor.dataset.downloadurl = ['application/vnd.openxmlformats-officedocument.wordprocessingml.document', anchor.download, anchor.href].join(':');
                anchor.click();
            }
        );
    }

    gestionarOrdenGiro( registro: any ) {
        const pOrdenGiro = {
            ordenGiroId: registro.ordenGiroId,
            estadoCodigo: EstadosSolicitudPagoOrdenGiro.conOrdenGiroTramitada
        }

        this.ordenGiroSvc.changueStatusOrdenGiro( pOrdenGiro )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                        () => this.routes.navigate( [ '/tramitarOrdenGiro' ] )
                    );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

    devolverOrdenGiro( registro: any ) {
        const pOrdenGiro = {
            ordenGiroId: registro.ordenGiroId,
            estadoCodigo: EstadosSolicitudPagoOrdenGiro.ordenGiroDevueltaPorTramiteFiduciario
        }

        this.ordenGiroSvc.changueStatusOrdenGiro( pOrdenGiro )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                        () => this.routes.navigate( [ '/tramitarOrdenGiro' ] )
                    );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

}
