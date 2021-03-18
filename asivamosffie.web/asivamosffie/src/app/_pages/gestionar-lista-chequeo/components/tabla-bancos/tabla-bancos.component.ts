import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { GestionarListaChequeoService } from './../../../../core/_services/gestionarListaChequeo/gestionar-lista-chequeo.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-tabla-bancos',
  templateUrl: './tabla-bancos.component.html',
  styleUrls: ['./tabla-bancos.component.scss']
})
export class TablaBancosComponent implements OnInit {

    dataSource = new MatTableDataSource();
    @ViewChild( MatPaginator, { static: true } ) paginator: MatPaginator;
    @ViewChild( MatSort, { static: true } ) sort: MatSort;
    displayedColumns: string[] = [ 'nombreRequisito', 'estadoRequisito', 'gestion' ];

    constructor(
        private listaChequeoSvc: GestionarListaChequeoService,
        private dialog: MatDialog,
        private routes: Router )
    {
        this.listaChequeoSvc.getListItem()
            .subscribe(
                listas => {
                    this.dataSource = new MatTableDataSource( listas );
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

    openDialog( modalTitle: string, modalText: string ) {
        this.dialog.open( ModalDialogComponent, {
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

    activarDesactivarItem( registro: any ) {
        const pListaChequeoItem = {
            listaChequeoItemId: registro.listaChequeoItemId,
            activo: !registro.activo
        }

        if ( pListaChequeoItem.activo === false ) {
            this.openDialogTrueFalse( '', `<b>¿Esta seguro que desea desactivar el requisito "${ registro.nombre }"?</b>` )
                .subscribe(
                    value => {
                        if ( value === true ) {
                            this.listaChequeoSvc.activateDeactivateListaChequeoItem( pListaChequeoItem )
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
            this.listaChequeoSvc.activateDeactivateListaChequeoItem( pListaChequeoItem )
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
