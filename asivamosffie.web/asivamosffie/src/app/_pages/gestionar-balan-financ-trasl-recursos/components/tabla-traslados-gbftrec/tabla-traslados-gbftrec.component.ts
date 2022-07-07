import { Dominio } from './../../../../core/_services/common/common.service';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import moment from 'moment';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { EstadoTraslado } from 'src/app/_interfaces/balance-financiero.interface';
import { FinancialBalanceService } from 'src/app/core/_services/financialBalance/financial-balance.service';

@Component({
  selector: 'app-tabla-traslados-gbftrec',
  templateUrl: './tabla-traslados-gbftrec.component.html',
  styleUrls: ['./tabla-traslados-gbftrec.component.scss']
})
export class TablaTrasladosGbftrecComponent implements OnInit {

    @Input() balanceFinancieroTraslado: any[];
    @Input() esVerDetalle!: boolean;
    estadoTraslado = EstadoTraslado;
    proyectoId = 0;
    listaEstadoTraslado: Dominio[] = [];
    dataSource = new MatTableDataSource();
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    displayedColumns: string[] = [
      'fechaTraslado',
      'numTraslado',
      'numContrato',
      'numOrdenGiro',
      'valorTraslado',
      'estadoTraslado',
      'gestion'
    ];

    constructor(
        public dialog: MatDialog,
        private activatedRoute: ActivatedRoute,
        private commonSvc: CommonService,
        private routes: Router,
        private balanceSvc: FinancialBalanceService )
    {
        this.proyectoId = Number( this.activatedRoute.snapshot.params.id )
    }

    async ngOnInit() {
        this.listaEstadoTraslado = await this.commonSvc.listaEstadoTraslado().toPromise()

        this.loadTable();
    }

    loadTable(){
        this.balanceFinancieroTraslado.forEach( registro => registro.fechaCreacion = moment( registro.fechaCreacion ).format( 'DD/MM/YYYY' ) )
        this.dataSource = new MatTableDataSource( this.balanceFinancieroTraslado );
        this.dataSource.sort = this.sort;
        this.dataSource.paginator = this.paginator;
        this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
        this.paginator._intl.getRangeLabel = (page, pageSize, length) => {
            if (length === 0 || pageSize === 0) {
              return '0 de ' + length;
            }
            length = Math.max(length, 0);
            const startIndex = page * pageSize;
            // If the start index exceeds the list length, do not try and fix the end index to the end.
            const endIndex = startIndex < length ?
              Math.min(startIndex + pageSize, length) :
              startIndex + pageSize;
            return startIndex + 1 + ' - ' + endIndex + ' de ' + length;
        };
    }

    applyFilter(event: Event) {
      const filterValue = (event.target as HTMLInputElement).value;
      this.dataSource.filter = filterValue.trim().toLowerCase();
    }

    getEstadoTraslado( codigo: string ) {
        if ( this.listaEstadoTraslado.length > 0 ) {
            const traslado = this.listaEstadoTraslado.find( traslado => traslado.codigo === codigo )

            if ( traslado !== undefined ) {
                return traslado.nombre
            }
        }
    }

    openDialog(modalTitle: string, modalText: string) {
        const dialogRef = this.dialog.open( ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText }
        });
    }

    openDialogTrueFalse(modalTitle: string, modalText: string) {

        const dialogRef = this.dialog.open( ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText, siNoBoton: true }
        });

        return dialogRef.afterClosed();
    }

    getAprobarTraslado( registro: any ) {
        const pBalanceFinancieroTraslado = {
            balanceFinancieroTrasladoId: registro.balanceFinancieroTrasladoId,
            estadoCodigo: this.estadoTraslado.trasladoAprobado
        }

        this.balanceSvc.changeStatudBalanceFinancieroTraslado( pBalanceFinancieroTraslado )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );

                    this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                        () => this.routes.navigate(
                            [
                                '/gestionarBalanceFinancieroTrasladoRecursos/verDetalleEditarBalance', this.proyectoId
                            ]
                        )
                    );
                },
                err => this.openDialog( '', `${ err.message }` )
            )
    }

    getNotificarFiduciaria( registro: any ) {
        const pBalanceFinancieroTraslado = {
            balanceFinancieroTrasladoId: registro.balanceFinancieroTrasladoId,
            estadoCodigo: this.estadoTraslado.notificadoFiduciaria
        }

        this.balanceSvc.changeStatudBalanceFinancieroTraslado( pBalanceFinancieroTraslado )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );

                    this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                        () => this.routes.navigate(
                            [
                                `/gestionarBalanceFinancieroTrasladoRecursos/${ this.esVerDetalle ? 'verDetalleBalance' : 'verDetalleEditarBalance' }`, this.proyectoId
                            ]
                        )
                    );
                },
                err => this.openDialog( '', `${ err.message }` )
            )
    }

    getAnularTraslado( registro: any ) {
        this.openDialogTrueFalse( '', '<b>¿Esta seguro que desea anular el traslado?, tenga en cuenta que puede afectar el balance</b>' )
            .subscribe(
                value => {
                    if ( value === true ) {
                        const pBalanceFinancieroTraslado = {
                            balanceFinancieroTrasladoId: registro.balanceFinancieroTrasladoId,
                            estadoCodigo: this.estadoTraslado.anulado
                        }

                        this.balanceSvc.changeStatudBalanceFinancieroTraslado( pBalanceFinancieroTraslado )
                            .subscribe(
                                response => {
                                    this.openDialog( '', `<b>${ response.message }</b>` );

                                    this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                                        () => this.routes.navigate(
                                            [
                                                '/gestionarBalanceFinancieroTrasladoRecursos/verDetalleEditarBalance', this.proyectoId
                                            ]
                                        )
                                    );
                                },
                                err => this.openDialog( '', `${ err.message }` )
                            )
                    }
                }
            )
    }

}
