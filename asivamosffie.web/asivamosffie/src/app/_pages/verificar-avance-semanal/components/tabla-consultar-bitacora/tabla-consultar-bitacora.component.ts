import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-tabla-consultar-bitacora',
  templateUrl: './tabla-consultar-bitacora.component.html',
  styleUrls: ['./tabla-consultar-bitacora.component.scss']
})
export class TablaConsultarBitacoraComponent implements OnInit {

    tablaConsultarEditarBitacora = new MatTableDataSource();
    @Input() consultarBitacora: any;
    @Output() seRealizoPeticion = new EventEmitter<boolean>();
    @ViewChild( MatPaginator, { static: true } ) paginator: MatPaginator;
    @ViewChild( MatSort, { static: true } ) sort: MatSort;
    estadoAvanceSemanal: any;
    displayedColumns: string[]  = [
        'semanaNumero',
        'periodoReporte',
        'estadoObra',
        'programacionObraAcumulada',
        'avanceFisico',
        'estadoRegistro',
        'estadoReporteSemanal',
        'estadoMuestras',
        'gestion'
    ];

    constructor(
        private routes: Router,
        private dialog: MatDialog )
    { }

    ngOnInit(): void {
        this.getBitacora();
    }

    getBitacora() {
        if ( this.consultarBitacora !== undefined ) {
            this.tablaConsultarEditarBitacora = new MatTableDataSource( this.consultarBitacora );
            this.tablaConsultarEditarBitacora.sort = this.sort;
            this.tablaConsultarEditarBitacora.paginator = this.paginator;
            this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
        }
    }

    applyFilter( event: Event ) {
        const filterValue      = (event.target as HTMLInputElement).value;
        this.tablaConsultarEditarBitacora.filter = filterValue.trim().toLowerCase();
    }

    getVerDetalleAvance( seguimientoSemanalId: number ) {
        this.routes.navigate( [ `${ this.routes.url }/verDetalleAvanceSemanal`, seguimientoSemanalId ] );
    }

    openDialog(modalTitle: string, modalText: string) {
        const dialogRef = this.dialog.open(ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText }
        });
    }

}
