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
        'semanaReporte',
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
                    // console.log( response );
                    this.tablaRegistro = new MatTableDataSource( response );
                    this.tablaRegistro.sort = this.sort;
                    this.tablaRegistro.paginator = this.paginator;
                    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
                    this.paginator._intl.nextPageLabel = 'Siguiente';
                    this.paginator._intl.getRangeLabel = (page, pageSize, length) => {
                    if (length === 0 || pageSize === 0) {
                        return '0 de ' + length;
                    }
                    length = Math.max(length, 0);
                    const startIndex = page * pageSize;
                    // If the start index exceeds the list length, do not try and fix the end index to the end.
                    const endIndex = startIndex < length ?
                        Math.min(startIndex + pageSize, length) :
                        startIndex + pageSize;
                    return startIndex + 1 + ' - ' + endIndex + ' de ' + length;
                    };
                    this.paginator._intl.previousPageLabel = 'Anterior';
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
