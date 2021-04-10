import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import moment from 'moment';
import { Subscription } from 'rxjs';
import { PolizaGarantiaService } from 'src/app/core/_services/polizaGarantia/poliza-garantia.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { EstadoPolizaCodigo } from 'src/app/_interfaces/gestionar-polizas-garantias.interface';

@Component({
  selector: 'app-tabla-en-revision-de-polizas',
  templateUrl: './tabla-en-revision-de-polizas.component.html',
  styleUrls: ['./tabla-en-revision-de-polizas.component.scss']
})
export class TablaEnRevisionDePolizasComponent implements OnInit {

    @Output() estadoSemaforo1 = new EventEmitter<string>();
    estadoPolizaCodigo = EstadoPolizaCodigo;
    displayedColumns: string[] = ['fechaFirma', 'numeroContrato', 'tipoSolicitud', 'estadoPoliza', 'contratoId'];
    dataSource = new MatTableDataSource();
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    dataTable: any[] = [];
    loadDataItems: Subscription;

    constructor(
        private polizaSvc: PolizaGarantiaService,
        private routes: Router,
        private dialog: MatDialog )
    {
        this.polizaSvc.getListGrillaContratoGarantiaPoliza( this.estadoPolizaCodigo.enRevision )
            .subscribe(
                getListGrillaContratoGarantiaPoliza => {
                    let totalIncompleto = 0;
                    let totalCompleto = 0;

                    if ( getListGrillaContratoGarantiaPoliza.length === 0 ) {
                        this.estadoSemaforo1.emit( 'completo' );
                        return;
                    }

                    getListGrillaContratoGarantiaPoliza.forEach( registro => {
                        registro.fechaFirma = registro.fechaFirma !== undefined ? ( moment( registro.fechaFirma ).format( 'DD/MM/YYYY' ) ) : '';

                        if ( registro.registroCompletoPoliza === true ) {
                            totalCompleto++;
                        }
                        if ( registro.registroCompletoPoliza !== true ) {
                            totalIncompleto++;
                        }
                    } );

                    if ( totalIncompleto > 0 && totalIncompleto === getListGrillaContratoGarantiaPoliza.length ) {
                        this.estadoSemaforo1.emit( 'sin-diligenciar' );
                    }
                    if ( totalIncompleto > 0 && totalIncompleto < getListGrillaContratoGarantiaPoliza.length ) {
                        this.estadoSemaforo1.emit( 'en-proceso' );
                    }
                    if ( totalCompleto > 0 && totalCompleto < getListGrillaContratoGarantiaPoliza.length ) {
                        this.estadoSemaforo1.emit( 'en-proceso' );
                    }
                    if ( totalCompleto > 0 && totalCompleto === getListGrillaContratoGarantiaPoliza.length ) {
                        this.estadoSemaforo1.emit( 'completo' );
                    }

                    this.dataSource = new MatTableDataSource( getListGrillaContratoGarantiaPoliza );
                    this.dataSource.sort = this.sort;
                    this.dataSource.paginator = this.paginator;
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

    openDialog(modalTitle: string, modalText: string) {
        this.dialog.open(ModalDialogComponent, {
            width: '28em',
            data: { modalTitle, modalText }
        });
    }

    aprobarPoliza( contratoPolizaId: number ) {
        const pContratoPoliza = {
            contratoPolizaId,
            estadoPolizaCodigo: this.estadoPolizaCodigo.conAprobacion
        };

        this.polizaSvc.changeStatusEstadoPoliza( pContratoPoliza )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.routes.navigateByUrl( '/', { skipLocationChange: true } ).then(
                        () => this.routes.navigate( [ '/generarPolizasYGarantias' ] )
                    );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

}
