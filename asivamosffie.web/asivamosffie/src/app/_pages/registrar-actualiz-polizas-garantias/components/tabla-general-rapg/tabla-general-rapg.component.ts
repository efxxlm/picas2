import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import moment from 'moment';
import { ActualizarPolizasService } from 'src/app/core/_services/actualizarPolizas/actualizar-polizas.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { EstadosActualizacionPoliza } from 'src/app/_interfaces/estados-actualizacion-polizas.interface';
import humanize from 'humanize-plus';

@Component({
  selector: 'app-tabla-general-rapg',
  templateUrl: './tabla-general-rapg.component.html',
  styleUrls: ['./tabla-general-rapg.component.scss']
})
export class TablaGeneralRapgComponent implements OnInit {

    @Input() listaActualizacionPoliza = [];
    estadosActualizacionPoliza = EstadosActualizacionPoliza;
    dataSource = new MatTableDataSource();
    estadosPolizaActualizacion: Dominio[] = [];
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    displayedColumns: string[] = [
      'fechaExpedicionActualizacionPoliza',
      'numeroContrato',
      'nombreContratista',
      'numeroPoliza',
      'numeroActualizacion',
      'estadoActualizacion',
      'registroCompleto',
      'gestion'
    ];

    constructor(
        private routes: Router,
        private dialog: MatDialog,
        private commonSvc: CommonService,
        private actualizarPolizaSvc: ActualizarPolizasService )
    {
        this.commonSvc.estadosPolizaActualizacion()
            .subscribe( estadosPolizaActualizacion => this.estadosPolizaActualizacion = estadosPolizaActualizacion );
    }

    ngOnInit(): void {
        this.listaActualizacionPoliza.forEach( registro => registro.fechaExpedicionActualizacionPoliza = moment( registro.fechaExpedicionActualizacionPoliza ).format( 'DD/MM/YYYY' ) );

        this.dataSource = new MatTableDataSource( this.listaActualizacionPoliza );
        this.dataSource.sort = this.sort;
        this.dataSource.paginator = this.paginator;
        this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    }

    firstLetterUpperCase( texto:string ) {
        if ( texto !== undefined ) {
            return humanize.capitalize( String( texto ).toLowerCase() );
        }
    }

    getEstadoRegistro( estadoCodigo: string ) {
        if ( this.estadosPolizaActualizacion.length > 0 ) {
            const estado = this.estadosPolizaActualizacion.find( estado => estado.codigo === estadoCodigo );

            if ( estado !== undefined ) {
                return estado.nombre;
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

    deleteActualizacion( contratoPolizaActualizacionId: number ) {
        const pContratoPolizaActualizacion = {
            contratoPolizaActualizacionId
        }

        this.openDialogTrueFalse( '', '<b>¿Está seguro de eliminar esta información?</b>' )
            .subscribe(
                response => {
                    if ( response === true ) {
                        this.actualizarPolizaSvc.deleteContratoPolizaActualizacion( pContratoPolizaActualizacion )
                            .subscribe(
                                response => {
                                    this.openDialog( '', `<b> ${ response.message } </b>` );
                                    this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                                        () => this.routes.navigate( [ '/registrarActualizacionesPolizasYGarantias' ] )
                                    );
                                },
                                err => this.openDialog( '', `<b> ${ err.message } </b>` )
                            )
                    }
                }
            )
    }

    aprobarActualizacion( contratoPolizaActualizacionId: number ) {
        const pContratoPolizaActualizacion = {
            contratoPolizaActualizacionId,
            estadoActualizacion: this.estadosActualizacionPoliza.conAprobacionActualizacionPoliza
        }

        this.actualizarPolizaSvc.changeStatusContratoPolizaActualizacionSeguro( pContratoPolizaActualizacion )
            .subscribe(
                response => {
                    this.openDialog( '', `<b> ${ response.message } </b>` );
                    this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                        () => this.routes.navigate( [ '/registrarActualizacionesPolizasYGarantias' ] )
                    );
                },
                err => this.openDialog( '', `<b> ${ err.message } </b>` )
            )
    }

    applyFilter(event: Event) {
        const filterValue = (event.target as HTMLInputElement).value;
        this.dataSource.filter = filterValue.trim().toLowerCase();
    
        if (this.dataSource.paginator) {
          this.dataSource.paginator.firstPage();
        }
      }

}
