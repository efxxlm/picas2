import { RegistrarAvanceSemanalService } from 'src/app/core/_services/registrarAvanceSemanal/registrar-avance-semanal.service';
import { Router } from '@angular/router';
import { MatTableDataSource } from '@angular/material/table';
import { Component, Input, OnInit, Output, ViewChild, EventEmitter } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-tabla-consultar-editar-bitacora',
  templateUrl: './tabla-consultar-editar-bitacora.component.html',
  styleUrls: ['./tabla-consultar-editar-bitacora.component.scss']
})
export class TablaConsultarEditarBitacoraComponent implements OnInit {

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
        private avanceSemanalSvc: RegistrarAvanceSemanalService,
        private dialog: MatDialog )
    {
        this.avanceSemanalSvc.estadosAvanceSemanal()
        .subscribe( estados => {
            this.estadoAvanceSemanal = estados;
        } );
    }

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

    enviarMuestrasVerificacion( seguimientoSemanalId: number ) {
        this.avanceSemanalSvc
            .changueStatusMuestrasSeguimientoSemanal( seguimientoSemanalId, this.estadoAvanceSemanal.enviadoAVerificacion.codigo )
                .subscribe(
                    response => {
                        this.tablaConsultarEditarBitacora = new MatTableDataSource();
                        this.openDialog( '', `<b>${ response.message }</b>` );
                        this.seRealizoPeticion.emit( true );
                        setTimeout(() => {
                            this.getBitacora();
                        }, 1000);
                    }
                );
    }

}
