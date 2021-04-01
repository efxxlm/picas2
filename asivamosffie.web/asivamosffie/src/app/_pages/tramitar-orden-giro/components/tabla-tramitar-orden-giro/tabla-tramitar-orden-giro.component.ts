import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import moment from 'moment';
import { OrdenPagoService } from 'src/app/core/_services/ordenPago/orden-pago.service';
import { EstadoSolicitudPagoOrdenGiro, EstadosSolicitudPagoOrdenGiro, ListaMenu, ListaMenuId } from 'src/app/_interfaces/estados-solicitudPago-ordenGiro.interface';

@Component({
  selector: 'app-tabla-tramitar-orden-giro',
  templateUrl: './tabla-tramitar-orden-giro.component.html',
  styleUrls: ['./tabla-tramitar-orden-giro.component.scss']
})
export class TablaTramitarOrdenGiroComponent implements OnInit {

    listaMenu: ListaMenu = ListaMenuId;
    estadoSolicitudPagoOrdenGiro: EstadoSolicitudPagoOrdenGiro = EstadosSolicitudPagoOrdenGiro;
    tablaTramitar = new MatTableDataSource();
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
        private ordenGiroSvc: OrdenPagoService )
    {
        this.ordenGiroSvc.getListOrdenGiro( this.listaMenu.verificarOrdenGiro )
            .subscribe(
                response => {
                    console.log( response );

                    response.forEach( registro => registro.fechaAprobacionFinanciera = moment( registro.fechaAprobacionFinanciera ).format( 'DD/MM/YYYY' ) );

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

    openDialogEnviarAprobacion() {
        // this.dialog.open( DialogEnviarAprobacionComponent, {
        //   width: '80em'
        // });
    }

}
