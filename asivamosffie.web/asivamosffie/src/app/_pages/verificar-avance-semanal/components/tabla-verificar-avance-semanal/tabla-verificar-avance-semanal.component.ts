import { VerificarAvanceSemanalService } from './../../../../core/_services/verificarAvanceSemanal/verificar-avance-semanal.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { RegistrarAvanceSemanalService } from 'src/app/core/_services/registrarAvanceSemanal/registrar-avance-semanal.service';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-tabla-verificar-avance-semanal',
  templateUrl: './tabla-verificar-avance-semanal.component.html',
  styleUrls: ['./tabla-verificar-avance-semanal.component.scss']
})
export class TablaVerificarAvanceSemanalComponent implements OnInit {

    primeraSemana = 1;
    estadoAvanceSemanal: any;
    tablaRegistro = new MatTableDataSource();
    @ViewChild( MatPaginator, { static: true } ) paginator: MatPaginator;
    @ViewChild( MatSort, { static: true } ) sort: MatSort;
    displayedColumns: string[]  = [
        'fechaReporte',
        'llaveMen',
        'numeroContrato',
        'tipoIntervencion',
        'institucionEducativa',
        'sede',
        'estadoObra',
        'estadoVerificacion',
        'gestion'
    ];

    constructor(
        private verificarAvanceSemanalSvc: VerificarAvanceSemanalService,
        private avanceSemanalSvc: RegistrarAvanceSemanalService,
        private dialog: MatDialog ) {
        this.avanceSemanalSvc.estadosAvanceSemanal()
        .subscribe( estados => {
            this.estadoAvanceSemanal = estados;
        } );
    }

    ngOnInit(): void {
        this.getDataTable();
    }

    getDataTable() {
        this.verificarAvanceSemanalSvc.getListReporteSemanalView()
            .subscribe(
                response => {
                    this.tablaRegistro = new MatTableDataSource( response );
                    this.tablaRegistro.sort = this.sort;
                    this.tablaRegistro.paginator = this.paginator;
                    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
                }
            );
    }

    applyFilter( event: Event ) {
        const filterValue      = (event.target as HTMLInputElement).value;
        this.tablaRegistro.filter = filterValue.trim().toLowerCase();
    }

    openDialog(modalTitle: string, modalText: string) {
        const dialogRef = this.dialog.open(ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText }
        });
    }

    enviarVerificacion( contratacionProyectoId: number ) {
        this.avanceSemanalSvc
            .changueStatusSeguimientoSemanal( contratacionProyectoId, this.estadoAvanceSemanal.enviadoPorVerificacion.codigo )
                .subscribe(
                    response => {
                        this.openDialog( '', `<b>${ response.message }</b>` );
                        this.tablaRegistro = new MatTableDataSource();
                        this.getDataTable();
                    }
                );
    }

}
