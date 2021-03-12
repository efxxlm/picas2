import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { GestionarListaChequeoService } from './../../../../core/_services/gestionarListaChequeo/gestionar-lista-chequeo.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import moment from 'moment';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-tabla-lista-chequeo',
  templateUrl: './tabla-lista-chequeo.component.html',
  styleUrls: ['./tabla-lista-chequeo.component.scss']
})
export class TablaListaChequeoComponent implements OnInit {

    listaEstadoChequeo: any = {};
    listaEstadoListaChequeo: Dominio[] = [];
    dataSource = new MatTableDataSource();
    @ViewChild( MatPaginator, { static: true } ) paginator: MatPaginator;
    @ViewChild( MatSort, { static: true } ) sort: MatSort;
    displayedColumns: string[] = [ 'fechaCreacion', 'nombreRequisito', 'estadoRequisito', 'gestion' ];

    constructor(
        private listaChequeoSvc: GestionarListaChequeoService,
        private commonSvc: CommonService,
        private dialog: MatDialog,
        private routes: Router )
    {
        this.commonSvc.listaEstadoListaChequeo()
            .subscribe( listaEstadoListaChequeo => {
                this.listaEstadoListaChequeo = listaEstadoListaChequeo;
                listaEstadoListaChequeo.forEach( estado => {
                    if ( estado.codigo === '1' ) {
                        this.listaEstadoChequeo.estadoActivoTerminado = {
                            codigo: estado.codigo
                        }
                    }
                    if ( estado.codigo === '2' ) {
                        this.listaEstadoChequeo.estadoActivoEnProceso = {
                            codigo: estado.codigo
                        }
                    }
                    if ( estado.codigo === '3' ) {
                        this.listaEstadoChequeo.inactivoEnProceso = {
                            codigo: estado.codigo
                        }
                    }
                    if ( estado.codigo === '4' ) {
                        this.listaEstadoChequeo.inactivoTerminado = {
                            codigo: estado.codigo
                        }
                    }
                } );
                console.log( this.listaEstadoChequeo );
                this.listaChequeoSvc.getCheckList()
                    .subscribe(
                        listas => {
                            if ( listas.length > 0 ) {
                                listas.forEach( registro => registro.fechaCreacion !== undefined ? registro.fechaCreacion = moment( registro.fechaCreacion ).format( 'DD/MM/YYYY' ) : '' );
                            }
                            console.log( listas );
                            this.dataSource = new MatTableDataSource( listas );
                            this.dataSource.paginator = this.paginator;
                            this.dataSource.sort = this.sort;
                            this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
                        }
                    );
            } );
    }

    ngOnInit(): void {
    }

    applyFilter(event: Event) {
        const filterValue = (event.target as HTMLInputElement).value;
        this.dataSource.filter = filterValue.trim().toLowerCase();
    }

    getEstado( estadoCodigo: string ) {
        if ( this.listaEstadoListaChequeo.length > 0 ) {
            const estado = this.listaEstadoListaChequeo.find( estado => estado.codigo === estadoCodigo );

            if ( estado !== undefined ) {
                return estado.nombre;
            }
        }
    }

    openDialog( modalTitle: string, modalText: string ) {
        this.dialog.open( ModalDialogComponent, {
          width: '30em',
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

    deleteListaChequeo( pListaChequeoId: number ) {
        this.openDialogTrueFalse( '', '<b>¿Está seguro de eliminar esta información?</b>' )
            .subscribe(
                value => {
                    if ( value === true ) {
                        this.listaChequeoSvc.deleteListaChequeo( pListaChequeoId )
                            .subscribe(
                                response => {
                                    this.openDialog( '', `<b>${ response.message }</b>` );
                                    this.routes.navigateByUrl( '/', { skipLocationChange: true } ).then( () => this.routes.navigate( [ '/gestionListaChequeo' ] ) );
                                },
                                err => this.openDialog( '', `<b>${ err.message }</b>` )
                            );
                    }
                }
            );
    }

    activarDesactivarLista( registro: any, activar: boolean ) {

        let estadoCodigo: string;

        if ( registro.estadoCodigo === this.listaEstadoChequeo.estadoActivoTerminado.codigo ) {
            estadoCodigo = this.listaEstadoChequeo.inactivoTerminado.codigo;
        }

        if ( registro.estadoCodigo === this.listaEstadoChequeo.inactivoTerminado.codigo ) {
            estadoCodigo = this.listaEstadoChequeo.estadoActivoTerminado.codigo;
        }

        if ( registro.estadoCodigo === this.listaEstadoChequeo.estadoActivoEnProceso.codigo ) {
            estadoCodigo = this.listaEstadoChequeo.inactivoEnProceso.codigo;
        }

        if ( registro.estadoCodigo === this.listaEstadoChequeo.inactivoEnProceso.codigo ) {
            estadoCodigo = this.listaEstadoChequeo.estadoActivoEnProceso.codigo;
        }

        const pListaChequeo = {
            listaChequeoId: registro.listaChequeoId,
            estadoCodigo
        }

        if ( activar === false ) {
            this.openDialogTrueFalse( '', `<b>¿Esta seguro que desea desactivar la lista de chequeo "${ registro.nombre }"?</b>` )
                .subscribe(
                    value => {
                        if ( value === true ) {
                            this.listaChequeoSvc.activateDeactivateListaChequeo( pListaChequeo )
                                .subscribe(
                                    response => {
                                        this.openDialog( '', `<b>${ response.message }</b>` );
                                        this.routes.navigateByUrl( '/', { skipLocationChange: true } ).then( () => this.routes.navigate( [ '/gestionListaChequeo' ] ) );
                                    },
                                    err => this.openDialog( '', `<b>${ err.message }</b>` )
                                );
                        }
                    }
                );
        } else {
            this.listaChequeoSvc.activateDeactivateListaChequeo( pListaChequeo )
                .subscribe(
                    response => {
                        this.openDialog( '', `<b>${ response.message }</b>` );
                        this.routes.navigateByUrl( '/', { skipLocationChange: true } ).then( () => this.routes.navigate( [ '/gestionListaChequeo' ] ) );
                    },
                    err => this.openDialog( '', `<b>${ err.message }</b>` )
                );
        }
    }

}
