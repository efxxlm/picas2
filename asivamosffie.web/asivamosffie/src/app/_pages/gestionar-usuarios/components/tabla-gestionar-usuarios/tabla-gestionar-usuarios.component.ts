import { Router } from '@angular/router';
import { GestionarUsuariosService } from './../../../../core/_services/gestionarUsuarios/gestionar-usuarios.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import moment from 'moment';

@Component({
  selector: 'app-tabla-gestionar-usuarios',
  templateUrl: './tabla-gestionar-usuarios.component.html',
  styleUrls: ['./tabla-gestionar-usuarios.component.scss']
})
export class TablaGestionarUsuariosComponent implements OnInit {

    dataSource = new MatTableDataSource();
    @ViewChild( MatPaginator, { static: true } ) paginator: MatPaginator;
    @ViewChild( MatSort, { static: true } ) sort: MatSort;
    displayedColumns: string[] = [ 'fechaCreacion', 'procedencia', 'nombreApellido', 'numeroDocumento', 'rol', 'estado', 'gestion' ];

    constructor(
        private gestionarUsuarioSvc: GestionarUsuariosService,
        private dialog: MatDialog,
        private routes: Router )
    {
        this.gestionarUsuarioSvc.getListUsuario()
            .subscribe(
                getListUsuario => {
                    console.log( getListUsuario );

                    if ( getListUsuario.length > 0 ) {
                        getListUsuario.forEach( registro => registro.fechaCreacion = registro.fechaCreacion !== undefined ? moment( registro.fechaCreacion ).format( 'DD/MM/YYYY' ) : '' );
                    }

                    this.dataSource = new MatTableDataSource( getListUsuario );
                    this.dataSource.paginator = this.paginator;
                    this.dataSource.sort = this.sort;
                    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
                    setTimeout(() => {
                        document.getElementsByName( 'desactivarBtn' ).forEach( ( value: HTMLElement ) => value.classList.add( 'd-none' ) );
                    }, 500);
                }
            );
    }

    ngOnInit(): void {
    }

    applyFilter(event: Event) {
        const filterValue = (event.target as HTMLInputElement).value;
        this.dataSource.filter = filterValue.trim().toLowerCase();
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

    activarDesactivarUsuario( registro: any ) {
        console.log( registro );
        const pUsuario = {
            usuarioId: registro.usuarioId,
            eliminado: !registro.eliminado
        }

        if ( pUsuario.eliminado === true ) {
            this.openDialogTrueFalse( '', `¿Esta seguro que desea desactivar el usuario <b>${ registro.primerNombre } ${ registro.segundoNombre } ${ registro.primerApellido }</b>?` )
                .subscribe(
                    value => {
                        if ( value === true ) {
                            this.gestionarUsuarioSvc.activateDeActivateUsuario( pUsuario )
                                .subscribe(
                                    response => {
                                        this.openDialog( '', `<b>${ response.message }</b>` );
                                        this.routes.navigateByUrl( '/', { skipLocationChange: true } ).then( () => this.routes.navigate( [ '/gestionUsuarios' ] ) );
                                    },
                                    err => this.openDialog( '', `<b>${ err.message }</b>` )
                                );
                        }
                    }
                );
        } else {
            this.gestionarUsuarioSvc.activateDeActivateUsuario( pUsuario )
                .subscribe(
                    response => {
                        this.openDialog( '', `<b>${ response.message }</b>` );
                        this.routes.navigateByUrl( '/', { skipLocationChange: true } ).then( () => this.routes.navigate( [ '/gestionUsuarios' ] ) );
                    },
                    err => this.openDialog( '', `<b>${ err.message }</b>` )
                );
        }
    }

}
