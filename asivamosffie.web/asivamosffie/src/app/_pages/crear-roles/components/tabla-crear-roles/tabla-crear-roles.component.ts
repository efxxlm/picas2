import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { CrearRolesService } from './../../../../core/_services/crearRoles/crear-roles.service';
import { Component, OnInit, Output, ViewChild, EventEmitter } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import moment from 'moment';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
@Component({
  selector: 'app-tabla-crear-roles',
  templateUrl: './tabla-crear-roles.component.html',
  styleUrls: ['./tabla-crear-roles.component.scss']
})
export class TablaCrearRolesComponent implements OnInit {

    dataSource = new MatTableDataSource();
    @Output() sinRegistros = new EventEmitter<boolean>();
    @ViewChild( MatPaginator, { static: true } ) paginator: MatPaginator;
    @ViewChild( MatSort, { static: true } ) sort: MatSort;
    displayedColumns: string[] = [ 'fechaCreacion', 'nombreRol', 'estadoRol', 'gestion' ];

    constructor(
        private crearRolesSvc: CrearRolesService,
        private dialog: MatDialog,
        private routes: Router )
    {
        this.crearRolesSvc.getListPerfil()
            .subscribe(
                listas => {
                    if ( listas.length > 0 ) {

                        listas.forEach( registro => registro.fechaCreacion = registro.fechaCreacion !== undefined ? moment( registro.fechaCreacion ).format( 'DD/MM/YYYY' ) : '' );
                    } else {
                        this.sinRegistros.emit( true );
                    }
                    console.log( listas );
                    this.dataSource = new MatTableDataSource( listas );
                    this.dataSource.paginator = this.paginator;
                    this.dataSource.sort = this.sort;
                    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
                    /*
                    setTimeout(() => {
                        document.getElementsByName( 'desactivarBtn' ).forEach( ( value: HTMLElement ) => value.classList.add( 'd-none' ) );
                    }, 500);
                    */
                }
            );
    }

    ngOnInit(): void {
    }

    applyFilter(event: Event) {
        const filterValue = (event.target as HTMLInputElement).value;
        this.dataSource.filter = filterValue.trim().toLowerCase();
    }

    openDialog( modalTitle: string, modalText: string ) {
        this.dialog.open( ModalDialogComponent, {
          width: '30em',
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

    activarDesactivarPerfil( registro: any ) {
        const pPerfil = {
            perfilId: registro.perfilId,
            eliminado: !registro.eliminado
        }

        if ( pPerfil.eliminado === true ) {
            this.openDialogTrueFalse( '', `¿Esta seguro que desea desactivar el rol <b>${ registro.nombre }</b>?` )
                .subscribe(
                    value => {
                        if ( value === true ) {
                            this.crearRolesSvc.activateDeactivatePerfil( pPerfil )
                                .subscribe(
                                    response => {
                                        this.openDialog( '', `<b>${ response.message }</b>` );
                                        this.routes.navigateByUrl( '/', { skipLocationChange: true } ).then( () => this.routes.navigate( [ '/crearRoles' ] ) );
                                    },
                                    err => this.openDialog( '', `<b>${ err.message }</b>` )
                                );
                        }
                    }
                );
        } else {
            this.crearRolesSvc.activateDeactivatePerfil( pPerfil )
                .subscribe(
                    response => {
                        this.openDialog( '', `<b>${ response.message }</b>` );
                        this.routes.navigateByUrl( '/', { skipLocationChange: true } ).then( () => this.routes.navigate( [ '/crearRoles' ] ) );
                    },
                    err => this.openDialog( '', `<b>${ err.message }</b>` )
                );
        }
    }

}
