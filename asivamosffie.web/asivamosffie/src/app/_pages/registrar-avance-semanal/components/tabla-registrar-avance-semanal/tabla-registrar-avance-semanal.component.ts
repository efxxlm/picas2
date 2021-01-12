import { DialogCargarActaComponent } from './../dialog-cargar-acta/dialog-cargar-acta.component';
import { RegistrarAvanceSemanalService } from './../../../../core/_services/registrarAvanceSemanal/registrar-avance-semanal.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-tabla-registrar-avance-semanal',
  templateUrl: './tabla-registrar-avance-semanal.component.html',
  styleUrls: ['./tabla-registrar-avance-semanal.component.scss']
})
export class TablaRegistrarAvanceSemanalComponent implements OnInit {

    tablaRegistro = new MatTableDataSource();
    dataTable: any = [];
    estadoAvanceSemanal: any;
    primeraSemana = 1;
    @ViewChild( MatPaginator, { static: true } ) paginator: MatPaginator;
    @ViewChild( MatSort, { static: true } ) sort: MatSort;
    displayedColumns: string[]  = [
      'llaveMen',
      'numeroContrato',
      'tipoIntervencion',
      'institucionEducativa',
      'sede',
      'fechaUltimoReporte',
      'estadoObra',
      'gestion'
    ];

    constructor(
        private avanceSemanalSvc: RegistrarAvanceSemanalService,
        private dialog: MatDialog )
    {
        this.avanceSemanalSvc.estadosAvanceSemanal()
            .subscribe( estados => {
                this.estadoAvanceSemanal = estados;
                console.log( this.estadoAvanceSemanal );
            } );
        this.getDataTable();
    }

    ngOnInit(): void {
    }

    getDataTable() {
        this.avanceSemanalSvc.getVRegistrarAvanceSemanal()
            .subscribe(
                listas => {
                    this.dataTable = listas;
                    console.log( this.dataTable );
                    this.tablaRegistro = new MatTableDataSource( this.dataTable );
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

    openDialogCargarActa( registro: any ) {
        this.dialog.open( DialogCargarActaComponent, {
          width: '70em',
          data : { registro }
        });
    }

    enviarVerificacion( contratacionProyectoId: number ) {
        this.avanceSemanalSvc
            .changueStatusSeguimientoSemanal( contratacionProyectoId, this.estadoAvanceSemanal.enviadoAVerificacion.codigo )
                .subscribe(
                    response => {
                        this.openDialog( '', `<b>${ response.message }</b>` );
                        this.dataTable = [];
                        this.tablaRegistro = new MatTableDataSource();
                        this.getDataTable();
                    }
                );
    }

}
