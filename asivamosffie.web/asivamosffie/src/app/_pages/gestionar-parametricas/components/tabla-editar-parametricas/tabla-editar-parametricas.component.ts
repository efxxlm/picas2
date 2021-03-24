import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import moment from 'moment';
import { GestionarParametricasService } from 'src/app/core/_services/gestionarParametricas/gestionar-parametricas.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-tabla-editar-parametricas',
  templateUrl: './tabla-editar-parametricas.component.html',
  styleUrls: ['./tabla-editar-parametricas.component.scss']
})
export class TablaEditarParametricasComponent implements OnInit {

    @Input() parametricas: any[];
    dataSource = new MatTableDataSource();
    @ViewChild( MatPaginator, { static: true } ) paginator: MatPaginator;
    @ViewChild( MatSort, { static: true } ) sort: MatSort;
    displayedColumns: string[] = [ 'fechaCreacion', 'idValor', 'nombreValor', 'estadoValor', 'gestion' ];

    constructor(
        private dialog: MatDialog,
        private routes: Router,
        private gestionarParametricaSvc: GestionarParametricasService )
    { }

    ngOnInit(): void {
        console.log( this.parametricas );

        if ( this.parametricas.length > 0 ) {
            this.parametricas.forEach( registro => registro.dateCreation = registro.dateCreation !== undefined ? moment( registro.dateCreation ).format( 'DD/MM/YYYY' ) : '' );
        }

        this.dataSource = new MatTableDataSource( this.parametricas );
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
        this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    }

    applyFilter(event: Event) {
        const filterValue = (event.target as HTMLInputElement).value;
        this.dataSource.filter = filterValue.trim().toLowerCase();
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

    activarDesactivarParametrica( registro: any ) {

        registro.activo = !registro.activo;

        const pTipoDominio = { 
            tipoDominioId: registro.tipoDominioId,
            dominio: [ registro ]
        };

        if ( registro.activo === false ) {
            this.openDialogTrueFalse( '', `<b>¿Esta seguro que desea desactivar el valor "${ registro.nombre }"?</b>` )
                .subscribe(
                    value => {
                        if ( value === true ) {
                            this.gestionarParametricaSvc.createDominio( pTipoDominio )
                                .subscribe(
                                    response => {
                                        this.openDialog( '', `<b>${ response.message }</b>` );
                                        this.routes.navigateByUrl( '/', { skipLocationChange: true } )
                                            .then( () => this.routes.navigate( [ '/gestionParametricas/editarParametricas', registro.tipoDominioId ] ) );
                                    },
                                    err => this.openDialog( '', `<b>${ err.message }</b>` )
                                );
                        }
                    }
                );
        } else {
            this.gestionarParametricaSvc.createDominio( pTipoDominio )
                .subscribe(
                    response => {
                        this.openDialog( '', `<b>${ response.message }</b>` );
                        this.routes.navigateByUrl( '/', { skipLocationChange: true } )
                            .then( () => this.routes.navigate( [ '/gestionParametricas/editarParametricas', registro.tipoDominioId ] ) );
                    },
                    err => this.openDialog( '', `<b>${ err.message }</b>` )
                );
        }
    }

}
